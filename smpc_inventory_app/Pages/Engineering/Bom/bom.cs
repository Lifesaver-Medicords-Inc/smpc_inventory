using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Engineering.Bom;
using smpc_inventory_app.Pages.Setup;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Bom;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Bom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages
{
    public partial class bom : UserControl
    {
        BomClass records;
        // Initialised empty rather than left null (2026-09-05). Both are only ever
        // assigned inside LoadAll, so any load that does not complete - a failed or
        // unauthenticated request, or the early return when the response carries no
        // payload - left them null while the screen stayed open and usable. Nine places
        // dereference them without checking (BomDetail.Clone() in btn_new_Click and two
        // other handlers, new DataView(BomDetail), BomHead.Rows.Count, BomHead.Select(...),
        // BindControls(..., BomHead, ...)), so clicking New on a screen whose data had not
        // loaded threw NullReferenceException.
        //
        // An empty DataTable satisfies every one of those: Clone() returns an empty table,
        // DataView binds to nothing, Rows.Count is 0, Select() returns no rows. The screen
        // shows no records instead of crashing, and a later successful LoadAll replaces
        // both wholesale.
        DataTable BomHead = new DataTable();
        DataTable BomDetail = new DataTable();
        DataTable bomItemList;
        private bool _suppressEvents = false;

        // The screen holds one page of 20 BOMs (user request 2026-09-26: "optimize of BOM 5,842
        // items and 130k for item details", "create pagination 20 item per page in BOM").
        // selectedRecord indexes the loaded page; PREV/NEXT fetch the neighbouring page at its
        // edges, travelling on the first/last BOM id so nothing renumbers if someone else adds
        // or deletes a BOM meanwhile.
        private const int LAST_ROW = -1;
        private int _pageFirstId;
        private int _pageLastId;
        private bool _hasNextPage;
        private bool _hasPrevPage;
        private bool _isLoadingPage;

        public bom()
        {
            InitializeComponent();

            dg_bom.CellValueChanged += dg_bom_CellValueChanged;
            txt_man_days.TextChanged += txt_man_days_TextChanged;
            txt_labor_rate.TextChanged += txt_labor_rate_TextChanged;

            Helpers.Placeholder.SetPlaceholder(txt_labor_rate, "[RATE]");
            Helpers.Placeholder.SetPlaceholder(txt_man_days, "[DAYS]");

            var pesoCulture = new System.Globalization.CultureInfo("en-PH");

            dg_bom.Columns["net_price"].DefaultCellStyle.Format = "c2";
            dg_bom.Columns["net_price"].DefaultCellStyle.FormatProvider = pesoCulture;

            dg_bom.Columns["unit_price"].DefaultCellStyle.Format = "c2";
            dg_bom.Columns["unit_price"].DefaultCellStyle.FormatProvider = pesoCulture;

            // Were added inside LoadAll, so every reload stacked one more pair of handlers.
            txt_labor_rate.EnabledChanged += (s, ev) => ApplyPesoFormat(txt_labor_rate);
            txt_production_cost.EnabledChanged += (s, ev) => ApplyPesoFormat(txt_production_cost);
        }

        int selectedRecord = 0;

        // The item picker's list (items that may take a BOM - ~4,000 rows on test_fresh). It used
        // to be downloaded on every reload together with the all-items list (~9,800 rows), which
        // nothing on this screen ever read. Now it is fetched the first time the picker opens,
        // and dropped after a save or delete so the next pick sees the change.
        private async Task EnsureBomItemList()
        {
            if (bomItemList != null && bomItemList.Rows.Count > 0) return;

            Helpers.Loading.ShowLoading(dg_bom);
            try
            {
                bomItemList = await ItemListBomServices.GetAsDatatable();
            }
            finally
            {
                Helpers.Loading.HideLoading(dg_bom);
            }
        }

        private void ApplyPesoFormat(params TextBox[] textBoxes)
        {
            var pesoCulture = new System.Globalization.CultureInfo("en-PH");

            foreach (var tb in textBoxes)
            {
                if (!tb.Enabled) // Disabled → show formatted peso currency
                {
                    if (decimal.TryParse(tb.Text, out decimal value))
                    {
                        tb.Text = value.ToString("c2", pesoCulture);
                    }
                }
                else // Enabled → show raw number for editing
                {
                    if (decimal.TryParse(tb.Text, System.Globalization.NumberStyles.Currency, pesoCulture, out decimal value))
                    {
                        tb.Text = value.ToString();
                    }
                }
            }
        }

        private void BtnToogle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;
            btn_search.Visible = !isEdit;

            pnl_header.Enabled = isEdit;

            dg_bom.Enabled = true;
            dg_bom.AllowUserToAddRows = false;
            dg_bom.ClearSelection();

            _suppressEvents = !isEdit;

            // First lock all columns
            foreach (DataGridViewColumn col in dg_bom.Columns)
            {
                col.ReadOnly = true;
                col.DefaultCellStyle.BackColor = Color.LightGray;
            }

            if (isEdit)
            {
                // Explicitly allow editing for these columns only
                dg_bom.Columns["unit_price"].ReadOnly = false;
                dg_bom.Columns["unit_price"].DefaultCellStyle.BackColor = Color.White;
                dg_bom.Columns["bom_qty"].ReadOnly = false;
                dg_bom.Columns["bom_qty"].DefaultCellStyle.BackColor = Color.White;


                dg_bom.AllowUserToAddRows = true;
            }
        }

        private bool ValidateFields(out string errorFieldMessage)
        {
            bool isValid = true;
            errorFieldMessage = string.Empty;

            if (string.IsNullOrEmpty(txt_general_name.Text))
            {
                isValid = false;
                errorFieldMessage += "General Name cannot be empty.\n";
            }
            if (string.IsNullOrEmpty(txt_item_model.Text))
            {
                isValid = false;
                errorFieldMessage += "Model cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_item_code.Text))
            {
                isValid = false;
                errorFieldMessage += "Item Code cannot be empty.\n";
            }

            // Bug #149 (Trello): this only checked that the field parsed as SOME
            // integer, so a literal 0 passed straight through.
            if (string.IsNullOrEmpty(txt_production_qty.Text) || !int.TryParse(txt_production_qty.Text, out int productionQtyVal) || productionQtyVal <= 0)
            {
                isValid = false;
                errorFieldMessage += "Production Quantity must be greater than 0.\n";
            }

            if (string.IsNullOrEmpty(txt_production_cost.Text))
            {
                isValid = false;
                errorFieldMessage += "Production Cost cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_man_days.Text) || !int.TryParse(txt_man_days.Text, out _))
            {
                isValid = false;
                errorFieldMessage += "Days cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_labor_rate.Text) || !int.TryParse(txt_man_days.Text, out _))
            {
                isValid = false;
                errorFieldMessage += "Rate cannot be empty.\n";
            }

            if (cmb_production_type.SelectedItem == null)
            {
                isValid = false;
                errorFieldMessage += "Production Type must be selected.\n";
            }

            BtnToogle(true);
            btn_save.Enabled = true;
            btn_close.Enabled = true;
            Helpers.Loading.HideLoading(dg_bom);
            pnl_header.Enabled = true;
            pnl_components.Enabled = true;

            return isValid;
        }

        //private void txt_item_name_TextChanged(object sender, EventArgs e)
        //{
        //    Helpers.ResetControls(pnl_header);
        //    Helpers.ResetControls(pnl_components);

        //    dg_bom.DataSource = null;
        //    dg_bom.Rows.Clear();

        //    BtnToogle(true);

        //    txt_general_name.Clear();
        //    txt_item_model.Clear();
        //    txt_item_code.Clear();
        //    txt_production_qty.Clear();
        //    txt_labor.Clear();
        //    cmb_production_type.SelectedIndex = 0;
        //}

        private async void btn_get_item_Click(object sender, EventArgs e)
        {
            await EnsureBomItemList();

            List<int> t1 = new List<int>();
            List<string> s1 = new List<string>();
            string Title = "Item";

            SetupItemModal item_bom = new SetupItemModal(Title, ENUM_ENDPOINT.BomItemList, bomItemList, t1, s1, 0);
            DialogResult r = item_bom.ShowDialog();

            if (r == DialogResult.OK)
            {
                int result = item_bom.GetResult();
                Panel[] pnl_list = { pnl_header };
                Helpers.BindControls(pnl_list, bomItemList, result);
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            Helpers.ResetControls(pnl_components);
            dg_bom.ClearSelection();
            BtnToogle(true);

            dataBindingBomComponents.DataSource = BomDetail.Clone();
        }

        private async void btn_close_Click(object sender, EventArgs e)
        {
            BtnToogle(false);

            await ReloadCurrentPage();
        }

        private async void btn_prev_Click(object sender, EventArgs e) => await ChangeRecord(-1);

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(pnl_header);

            if (data.ContainsKey("id") && data["id"] != null && !string.IsNullOrWhiteSpace(data["id"].ToString()))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this data?",
                                                      "Confirm Delete",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Helpers.Loading.ShowLoading(dg_bom, "Deleting data...");
                        pnl_header.Enabled = false;
                        pnl_components.Enabled = false;
                        var deletePayload = new Dictionary<string, object>
                {
                    { "id", int.Parse(data["id"].ToString()) }
                };

                        ApiResponseModel response = await BomServices.DeleteBom(deletePayload);

                        if (response != null && response.Success)
                        {
                            Helpers.ShowDialogMessage("success", "BOM deleted successfully!");
                            bomItemList = null;
                            Helpers.ResetControls(pnl_header);
                            BtnToogle(false);

                            btn_save.Enabled = true;
                            btn_close.Enabled = true;
                            dataBindingBomComponents.DataSource = BomDetail.Clone();

                            Helpers.Loading.HideLoading(dg_bom);
                            pnl_header.Enabled = true;
                            pnl_components.Enabled = true;
                        }
                        else
                        {
                            Helpers.ShowDialogMessage("error", response?.message ?? "Operation failed. Please try again later.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the BOM: {ex.Message}",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    } finally
                    {
                        // One reload (the success branch used to reload as well, so a delete
                        // fetched everything twice). The page is reopened where it began: if the
                        // deleted BOM headed it, the next one moves up; if it was the last BOM
                        // there is, the page before it opens (FetchBomPage).
                        Helpers.Loading.HideLoading(dg_bom);
                        pnl_components.Enabled = true;
                        await ReloadCurrentPage();
                    }
                }
            }
            else
            {
                MessageBox.Show("No BOM data selected for deletion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            // Bugs #151/#152/#153 (Trello): validation used to run AFTER the form
            // was already disabled and the loading overlay shown, and the early
            // return on a validation failure never undid either - so a first Save
            // attempt with an invalid field (e.g. Unit Price cleared, or QTY/Unit
            // Price left over an edge value) left the whole form permanently
            // disabled under "Updating data...", which is what a second Save
            // attempt on that already-broken state was crashing into. Validate
            // first, before touching anything.
            string errorMessage;
            bool isValid = ValidateFields(out errorMessage);

            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Helpers.Loading.ShowLoading(dg_bom, "Updating data...");
            btn_close.Enabled = false;
            btn_save.Enabled = false;
            pnl_header.Enabled = false;
            pnl_components.Enabled = false;

            string message;
            ApiResponseModel response = new ApiResponseModel();

            CalculateProductionCost();

            var data = Helpers.GetControlsValues(pnl_header);
            data["item_id"] = int.Parse(data["item_id"].ToString());
            data["production_qty"] = int.Parse(data["production_qty"].ToString());
            data["production_cost"] = decimal.TryParse(data["production_cost"].ToString(), out decimal productionCost) ? (float)productionCost : 0f;
            data["man_days"] = decimal.TryParse(data["man_days"].ToString(), out decimal manDays) ? (int)manDays : 0;
            data["labor_rate"] = decimal.TryParse(data["labor_rate"].ToString(), out decimal laborRate) ? (float)laborRate : 0f; 

            data.Remove("general_name");
            data.Remove("item_model");
            data.Remove("item_code");

            var isUpdate = !txt_id.Text.Equals("");
            var bomDetail = SaveBomDetails(isUpdate);
            // A new BOM reopens on itself (the first row of its page); an edited one on the
            // page it was edited on.
            int? reopenAt = null;

            data.Add("bom_details", bomDetail);
            data.Remove("item_code");
            data.Remove("short_desc");
            data.Remove("uom");

            try
            {
                if (txt_id.Text.Equals(""))
                {
                    data.Remove("id");
                    response = await BomServices.Insert(data);
                    message = response.Success ? "Insert Bom Data Succesfully" : "Failed to add Bom\n" + response.message;

                    if (response.Success)
                    {
                        try { reopenAt = Convert.ToInt32(response.Data?["id"]); }
                        catch { reopenAt = null; }
                    }

                    MessageBox.Show(response.Success ? "BOM saved successfully!" : "Failed to save BOM.",
                        response.Success ? "Success" : "Error",
                        MessageBoxButtons.OK,
                        response.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }
                else
                {
                    response = await BomServices.UpdateBom(data);
                    message = response.Success ? "BOM updated successfully!" : "Failed to update BOM.";

                    MessageBox.Show(response.Success ? "BOM updated successfully!" : "Failed to update BOM.",
                        response.Success ? "Success" : "Error",
                        MessageBoxButtons.OK,
                        response.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }

                if (!response.Success)
                {
                    Helpers.ShowDialogMessage("error", message);
                    return;
                }

                bomItemList = null;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the BOM: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Helpers.ResetControls(pnl_header);
                BtnToogle(false);
                btn_save.Enabled = true;
                btn_close.Enabled = true;
                dataBindingBomComponents.DataSource = BomDetail.Clone();

                Helpers.Loading.HideLoading(dg_bom);
                pnl_header.Enabled = true;
                pnl_components.Enabled = true;

                if (reopenAt.HasValue && reopenAt.Value > 0)
                    await LoadPage(at: reopenAt.Value);
                else
                    await ReloadCurrentPage();
            }
        }

        private async void btn_next_Click(object sender, EventArgs e) => await ChangeRecord(1);

        private List<ItemBomDetails> SaveBomDetails(bool isUpdate)
        {

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_bom);
            List<ItemBomDetails> bomDetails = new List<ItemBomDetails>();
            ItemBomDetails bomChild = null;

            int item_bom_id = 0;
            int id = 0;
            foreach (DataRow row in dataSource.Rows)
            {
                if (isUpdate)
                {
                    if (row.IsNull("item_bom_id") || string.IsNullOrWhiteSpace(row["item_bom_id"].ToString()) || row.IsNull("id") || string.IsNullOrWhiteSpace(row["id"].ToString()))
                    {
                        item_bom_id = 0;
                        id = 0;
                    }
                    else
                    {
                        item_bom_id = int.Parse(row["item_bom_id"].ToString());
                        id = int.Parse(row["id"].ToString());
                    }
                }

                int item_id = 0;
                int bom_qty = 0;
                int unit_price = 0;
                float net_price = 0;

                int.TryParse(row["item_id"].ToString(), out item_id);
                int.TryParse(row["bom_qty"].ToString(), out bom_qty);
                int.TryParse(row["unit_price"].ToString(), out unit_price);
                float.TryParse(row["net_price"].ToString(), out net_price);

                string remarks = dataSource.Columns.Contains("remarks") && !row.IsNull("remarks")
                    ? row["remarks"].ToString()
                    : "";

                bomChild = new ItemBomDetails(id, item_bom_id, item_id, bom_qty, unit_price, net_price, remarks);
                bomDetails.Add(bomChild);
            }
            return bomDetails;
        }
         
        private async void bom_Load(object sender, EventArgs e)
        {
            await LoadPage();
        }

        // This used to call GET /setup/bom: every BOM head and every component line in the
        // system - 5,842 heads and 130,379 lines on test_fresh - to show one BOM. It now loads
        // the page of 20 it needs (GET /setup/bom/paged), in the same bom_head/bom_details shape.
        // after/before/at are BOM ids, not page numbers - see BomServices.GetPaged. selectRow
        // indexes the NEW page; LAST_ROW lands on its end, which PREV into the page before wants.
        private async Task FetchBomPage(int? after, int? before, int? at, int selectRow)
        {
            BtnToogle(false);

            var result = await BomServices.GetPaged(after, before, at);
            records = result?.Data;

            // A failed or empty BOM response used to walk straight into
            // records.bom_head, and from there into ToDataTable with a null list -
            // NullReferenceException on opening the screen.
            if (records == null)
            {
                Helpers.ShowDialogMessage("error",
                    "The service is busy right now, so the bill of materials could not be loaded. Please try again in a moment.");
                return;
            }

            // The lists themselves come back null when there is no BOM data at all -
            // a legitimate state on a fresh system, not an error. ToDataTable now
            // tolerates null, and these keep the rest of the method working on real
            // (empty) collections rather than nulls.
            if (records.bom_head == null) records.bom_head = new List<BomHead>();
            if (records.bom_details == null) records.bom_details = new List<BomDetail>();

            // Reopened at a BOM that no longer exists and had nothing after it - the last BOM
            // was just deleted: show the page before it instead of an empty screen.
            if (records.bom_head.Count == 0 && at.HasValue && at.Value > 0)
            {
                await FetchBomPage(null, at.Value, null, LAST_ROW);
                return;
            }

            _hasNextPage = result.Pagination?.has_next ?? false;
            _hasPrevPage = result.Pagination?.has_prev ?? false;

            BomHead = JsonHelper.ToDataTable(records.bom_head);
            BomDetail = JsonHelper.ToDataTable(records.bom_details);

            if (BomHead.Rows.Count == 0)
            {
                // Nothing to bind. (A stale -1 here used to survive into a later load that did
                // find rows, and FetchAllChilds then indexed BomHead.Rows[-1] - the live crash
                // "There is no row at position -1"; selectedRecord is now always set below.)
                this.selectedRecord = -1;
                _pageFirstId = 0;
                _pageLastId = 0;
                Helpers.ResetControls(pnl_header);
                dataBindingBomComponents.DataSource = BomDetail.Clone();
                return;
            }

            _pageFirstId = records.bom_head.First().id;
            _pageLastId = records.bom_head.Last().id;
            this.selectedRecord = selectRow == LAST_ROW
                ? BomHead.Rows.Count - 1
                : Math.Min(Math.Max(selectRow, 0), BomHead.Rows.Count - 1);

            // Was bound only when the load also carried component lines, so a page whose BOMs
            // had none showed nothing at all. The header binds on its own.
            Bind(true);
            ApplyPesoFormat(txt_labor_rate, txt_production_cost);
        }

        // Every page turn is a round trip, so it gets the loading screen. A second click while
        // one is on its way is dropped rather than queued behind it.
        private async Task LoadPage(int? after = null, int? before = null, int? at = null, int selectRow = 0)
        {
            if (_isLoadingPage) return;
            _isLoadingPage = true;

            Helpers.Loading.ShowLoading(dg_bom);
            try
            {
                await FetchBomPage(after, before, at, selectRow);
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", "Failed to load the bill of materials.\n" + ex.Message);
            }
            finally
            {
                Helpers.Loading.HideLoading(dg_bom);
                _isLoadingPage = false;
            }
        }

        // The page on screen again, on the same row - after Close, Save of an existing BOM, or
        // Delete. Opening at the page's first id keeps it the same 20 unless one of them is gone.
        private Task ReloadCurrentPage()
        {
            if (_pageFirstId > 0)
                return LoadPage(at: _pageFirstId, selectRow: Math.Max(this.selectedRecord, 0));

            return LoadPage();
        }

        // Walks within the loaded page, and fetches the next or previous one at its edges.
        private async Task ChangeRecord(int step)
        {
            if (_isLoadingPage) return;

            int newIndex = this.selectedRecord + step;

            if (BomHead.Rows.Count == 0
                || (newIndex < 0 && !_hasPrevPage)
                || (newIndex >= BomHead.Rows.Count && !_hasNextPage))
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (newIndex < 0)
            {
                await LoadPage(before: _pageFirstId, selectRow: LAST_ROW);
                return;
            }

            if (newIndex >= BomHead.Rows.Count)
            {
                await LoadPage(after: _pageLastId);
                return;
            }

            this.selectedRecord = newIndex;
            Bind(true);
            ApplyPesoFormat(txt_labor_rate, txt_production_cost);
        }

        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                BindDataToPanel();
                FetchAllChilds();
            }
        }

        private void BindDataToPanel()
        {
            Panel[] pnlHeader = { pnl_header };

            Helpers.BindControls(pnlHeader, BomHead, this.selectedRecord);
        }

        private void FetchAllChilds()
        {
            DataView dataViewItemDetails = new DataView(BomDetail);

            if (dataViewItemDetails.Count != 0)
            {
                dataViewItemDetails.RowFilter = "item_bom_id = '" + BomHead.Rows[this.selectedRecord]["id"].ToString() + "'";

            }
            DataTable filteredTable = dataViewItemDetails.ToTable();
            dataBindingBomComponents.DataSource = filteredTable;
        }

        // Searches every BOM on the server (BomPagedSearchModal), then opens the page that starts
        // at the chosen one. The old BomSearch filtered the downloaded heads - now only the 20 on
        // screen - and it also refused to open until the unrelated item-picker list had arrived.
        private async void btn_search_Click(object sender, EventArgs e)
        {
            int chosenId;
            using (BomPagedSearchModal bomSearch = new BomPagedSearchModal())
            {
                if (bomSearch.ShowDialog() != DialogResult.OK || bomSearch.SelectedBomId <= 0)
                    return;

                chosenId = bomSearch.SelectedBomId;
            }

            await LoadPage(at: chosenId);
        }

        private void CalculateProductionCost()
        {
            decimal totalNetPrice = 0;
            foreach(DataGridViewRow row in dg_bom.Rows)
            {
                if (row.Cells["bom_qty"].Value != null && decimal.TryParse(row.Cells["bom_qty"].Value.ToString(), out decimal qty) &&
                    row.Cells["unit_price"].Value != null && decimal.TryParse(row.Cells["unit_price"].Value.ToString(), out decimal unitPrice))
                {
                    decimal netPrice = qty * unitPrice;
                    row.Cells["net_price"].Value = netPrice;
                    totalNetPrice += netPrice;
                }
            }

            decimal ManDays = 0;
            decimal LaborRate = 0;

            if (decimal.TryParse(txt_man_days.Text, out ManDays) && decimal.TryParse(txt_labor_rate.Text, out LaborRate))
            {
                decimal laborCost = ManDays * LaborRate;
                decimal A = totalNetPrice  + laborCost;
                decimal markUp = 1.186m;
                decimal productionCost = A * markUp;

                txt_production_cost.Text = productionCost.ToString("F2");
            }
            else
            {
                txt_production_cost.Text = "0.00";
            }
        }

        private void dg_bom_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           // if (e.RowIndex >= 0 && e.ColumnIndex == dg_bom.Columns["unit_price"].Index) 
           // {
           //     CalculateProductionCost();
           // }
        }

        private void txt_man_days_TextChanged(object sender, EventArgs e)
        {
            //CalculateProductionCost();
        }

        private void txt_labor_rate_TextChanged(object sender, EventArgs e)
        {
            //CalculateProductionCost();
        }

        private void dg_bom_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_suppressEvents) return;

            CalculateProductionCost();
        }

        private void dg_bom_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dg_bom.CurrentCell.ColumnIndex == dg_bom.Columns["unit_price"].Index ||
       dg_bom.CurrentCell.ColumnIndex == dg_bom.Columns["bom_qty"].Index)
            {
                if (e.Control is TextBox tb)
                {
                    // Remove any previously added handler to avoid multiple subscriptions
                    tb.KeyPress -= NumericColumn_KeyPress;
                    tb.KeyPress += NumericColumn_KeyPress;
                }
            }
            else
            {
                if (e.Control is TextBox tb)
                {
                    tb.KeyPress -= NumericColumn_KeyPress; // remove if not numeric column
                }
            }
        }

        private void NumericColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys (backspace), and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // block invalid input
            }

            // Prevent multiple decimal points
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        // For integer-only input (e.g., man_days, qty)
        private void IntegerOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block non-numeric
            }
        }

        // For decimal input (e.g., labor_rate, unit_price)
        private void DecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent multiple decimals
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void dg_bom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_suppressEvents) return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dg_bom.Columns[e.ColumnIndex].Name == "item_code")
                {
                    BomItemModal modal = new BomItemModal();
                    DialogResult r = modal.ShowDialog();

                    if (r == DialogResult.OK)
                    {

                        Dictionary<string, dynamic> result = modal.GetResult();

                        // Bug #150 (Trello): nothing stopped a BOM from listing its own
                        // parent item as one of its components. Reject that pick here,
                        // before any cell is populated, using the same item_id the
                        // parent record itself is keyed on (txt_item_id).
                        if (int.TryParse(txt_item_id.Text, out int parentItemId)
                            && result.ContainsKey("item_id")
                            && Convert.ToInt32(result["item_id"]) == parentItemId)
                        {
                            MessageBox.Show("This item is the parent BOM item and cannot be added as its own component.", "Invalid Component", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        this.dg_bom.Rows[e.RowIndex].Cells[0].Value = result["item_code"];
                        this.dg_bom.Rows[e.RowIndex].Cells[1].Value = result["short_desc"];
                        this.dg_bom.Rows[e.RowIndex].Cells[2].Value = result["item_id"];
                        this.dg_bom.Rows[e.RowIndex].Cells[4].Value = result["size"];
                        this.dg_bom.Rows[e.RowIndex].Cells[6].Value = result["uom_name"];
                    }
                }
            }
        }
    }
}