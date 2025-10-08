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
        DataTable BomHead;
        DataTable BomDetail;
        DataTable bomItemList;
        DataTable allBomItemList;
        private bool _suppressEvents = false;

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
        }

        int selectedRecord = 0;

        private async void GetBomItemList()
        {
            var data = await ItemListBomServices.GetAsDatatable();
            bomItemList = data;
        }

        private async void GetAllBomItemList()
        {
            var data = await ItemListBomServices.GetAllAsDatatable();
            allBomItemList = data;
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

            if (string.IsNullOrEmpty(txt_production_qty.Text) || !int.TryParse(txt_production_qty.Text, out _))
            {
                isValid = false;
                errorFieldMessage += "Production Quantity cannot be empty.\n";
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

        private void btn_get_item_Click(object sender, EventArgs e)
        {
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

        private void btn_close_Click(object sender, EventArgs e)
        {
            BtnToogle(false);

            LoadAll();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {
                this.selectedRecord--;
                Bind(true);
                ApplyPesoFormat(txt_labor_rate, txt_production_cost);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

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
                            GetBomItemList();
                            GetAllBomItemList();
                            Helpers.ResetControls(pnl_header);
                            BtnToogle(false);

                            btn_save.Enabled = true;
                            btn_close.Enabled = true;
                            dataBindingBomComponents.DataSource = BomDetail.Clone();

                            Helpers.Loading.HideLoading(dg_bom);
                            pnl_header.Enabled = true;
                            pnl_components.Enabled = true;

                            LoadAll();
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
                        LoadAll();
                        Helpers.Loading.HideLoading(dg_bom);
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

            Helpers.Loading.ShowLoading(dg_bom, "Updating data...");
            btn_close.Enabled = false;
            btn_save.Enabled = false;
            pnl_header.Enabled = false;
            pnl_components.Enabled = false;

            string message;

            string errorMessage;
            bool isValid = ValidateFields(out errorMessage);

            ApiResponseModel response = new ApiResponseModel();

            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

                LoadAll();
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.BomHead.Rows.Count - 1 > this.selectedRecord)
            {
                this.selectedRecord++;
                Bind(true);
                ApplyPesoFormat(txt_labor_rate, txt_production_cost);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

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

                bomChild = new ItemBomDetails(id, item_bom_id, item_id, bom_qty, unit_price, net_price);
                bomDetails.Add(bomChild);
            }
            return bomDetails;
        }
         
        private void bom_Load(object sender, EventArgs e)
        {
            LoadAll();
        }

        private async void LoadAll()
        {
            Helpers.Loading.ShowLoading(dg_bom, "Fetching data...");

            GetBomItemList();
            GetAllBomItemList();
            BtnToogle(false);

            var response = await RequestToApi<ApiResponseModel<BomClass>>.Get(ENUM_ENDPOINT.BOM);
            records = response.Data;

            BomHead = JsonHelper.ToDataTable(records.bom_head);
            BomDetail = JsonHelper.ToDataTable(records.bom_details);

            //Ensure selectedRecord is always valid
            if (BomHead.Rows.Count == 0)
            {
                this.selectedRecord = -1; // nothing to bind
            }
            else if (this.selectedRecord >= BomHead.Rows.Count)
            {
                this.selectedRecord = 0; // fallback to first row
            }

            if (records.bom_head.Count != 0 && records.bom_details.Count != 0)
            {
                Bind(true);
            }

            ApplyPesoFormat(txt_labor_rate, txt_production_cost);
            // Also handle if Enabled changes later
            txt_labor_rate.EnabledChanged += (s, ev) => ApplyPesoFormat(txt_labor_rate);
            txt_production_cost.EnabledChanged += (s, ev) => ApplyPesoFormat(txt_production_cost);

            Helpers.Loading.HideLoading(dg_bom);
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (bomItemList == null || bomItemList.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] columnsToShow = { "item_id", "general_name", "item_code", "item_model", "short_desc" };

            using (BomSearch bomSearch = new BomSearch("Search Items", BomHead, columnsToShow))
            {
                if (bomSearch.ShowDialog() == DialogResult.OK && bomSearch.SelectedItem != null)
                {
                    // get item_id from the selected row
                    string selectedItemId = bomSearch.SelectedItem["item_id"].ToString();

                    // assign it into txt_item_id
                    txt_item_id.Text = selectedItemId;

                    // find the corresponding record in BomHead
                    DataRow[] foundRows = BomHead.Select($"item_id = '{selectedItemId}'");
                    if (foundRows.Length > 0)
                    {
                        // get the index of that row
                        this.selectedRecord = BomHead.Rows.IndexOf(foundRows[0]);

                        // bind to panel
                        Bind(true);
                    }
                    else
                    {
                        MessageBox.Show("Selected item not found in BOM records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
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