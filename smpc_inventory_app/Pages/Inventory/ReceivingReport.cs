using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Setup.Inventory;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Pages.Inventory.ReceivingReportModals;
using smpc_inventory_app.Properties;
using System.IO;

namespace smpc_inventory_app.Pages.Inventory
{
    public partial class ReceivingReport : UserControl
    {
        private WarehouseList _warehouseData;
        private List<ReceivingReportModel> _receivingReports;
        private ReceivingReportList2 _rrData;
        private int _currentRRIndex = -1;
        private bool _suppressRefDocBinding = false;
        private bool _suppressWarehouseBinding = false;
        private List<PurchaseOrderViewModel> _poData;
        private List<PurchaseOrderDetailsViewModel> _poDetailsView;
        private bool _isNewMode = false;
        private bool _isNotClickable = true;
        private List<WarehouseAreaModel> _warehouseAreas = new List<WarehouseAreaModel>();
        private ComboBox _binComboBox;
        private string _currentZone = "";
        private string _currentArea = "";
        private string _currentRack = "";
        private string _currentLevel = "";
        private readonly string receivingReportPath = Settings.Default.RECEIVINGREPORTPATH;
        private TreeNode selectedNode;

        private readonly string[] systemFolders =
        {
        "DELIVERY RECEIPT",
        "ITEM PICTURES"
        };

        //Dictionaries for the column grouping of datagridviews
        Dictionary<string, string[]> columnGroupsMain = new Dictionary<string, string[]>()
        {
            { "ORDER", new string[] { "ordered_qty", "ordered_uom" } },
            { "RECEIVED", new string[] { "received_qty", "received_uom" } },
            { "REJECTED", new string[] { "rejected_qty", "rejected_uom" } },
        };

        public ReceivingReport()
        {
            InitializeComponent();

            dgv_main.AutoGenerateColumns = false;
            Helpers.EnableGroupHeaders(dgv_main, columnGroupsMain);
            Helpers.RestrictColumnsToNumbers(dgv_main, "received_qty", "rejected_qty");

            dgv_main.DataError += dgv_main_DataError;

            LoadDirectory(RECEIVING_TV, receivingReportPath);

            // Create ImageList
            ImageList imageList = new ImageList();
            imageList.Images.Add("folder", Properties.Resources.FolderIcon);
            imageList.Images.Add("pdf", Properties.Resources.pdf);
            imageList.Images.Add("word", Properties.Resources.word);
            imageList.Images.Add("excel", Properties.Resources.excel);
            imageList.Images.Add("image", Properties.Resources.img);
            imageList.Images.Add("file", Properties.Resources.file);

            // Assign to TreeView
            RECEIVING_TV.ImageList = imageList;
            RECEIVING_LV.SmallImageList = imageList;

            // Enable drag and drop for ListView
            RECEIVING_LV.AllowDrop = true;
            RECEIVING_LV.DragEnter += RECEIVING_LV_DragEnter;
            RECEIVING_LV.DragDrop += RECEIVING_LV_DragDrop;

            InitializeListViewContextMenu();
            InitializeContextMenu();
        }

        private void ToggleButtons(bool isVisible)
        {
            btn_close.Visible = isVisible;
            btn_save.Visible = isVisible;
            btn_new.Visible = !isVisible;
            btn_search.Visible = !isVisible;
            btn_edit.Visible = !isVisible;
            btn_delete.Visible = !isVisible;
            btn_prev.Visible = !isVisible;
            btn_next.Visible = !isVisible;

            txt_date_received.ReadOnly = !isVisible;
            cmb_warehouse_name.Enabled = true;
        }

        private void ToggleColumn(bool isEdit)
        {
            try
            {
                if (dgv_main.Columns.Contains("bin_location"))
                    dgv_main.Columns["bin_location"].Visible = !isEdit;

                if (dgv_main.Columns.Contains("cmb_bin_location"))
                    dgv_main.Columns["cmb_bin_location"].Visible = isEdit;

                if (dgv_main.Columns.Contains("cmb_bin_location"))
                    dgv_main.Columns["cmb_bin_location"].ReadOnly = !isEdit;
            }
            catch (InvalidOperationException)
            {
                if (dgv_main.Columns.Contains("cmb_bin_location"))
                    dgv_main.Columns["cmb_bin_location"].Visible = !isEdit;
            }
        }

        private void ClearDGVs()
        {
            //Clear only the rows, keep columns
            dgv_main.DataSource = null;
            dgv_main.Rows.Clear();
        }

        private void SetEditableColumns(bool isEdit)
        {
            if (dgv_main.Columns.Contains("received_qty"))
                dgv_main.Columns["received_qty"].ReadOnly = !isEdit;

            if (dgv_main.Columns.Contains("rejected_qty"))
                dgv_main.Columns["rejected_qty"].ReadOnly = !isEdit;

            if (dgv_main.Columns.Contains("reason_for_rejection"))
                dgv_main.Columns["reason_for_rejection"].ReadOnly = true;

            if (dgv_main.Columns.Contains("serial_number"))
                dgv_main.Columns["serial_number"].ReadOnly = !isEdit;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            _isNotClickable = false;
            _isNewMode = false;
            _suppressWarehouseBinding = false;
            ToggleButtons(true);
            SetEditableColumns(true);
            ToggleColumn(false);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            cmb_ref_doc.SelectedIndex = -1;
            cmb_ref_doc.Text = string.Empty;
            cmb_ref_doc.DropDownStyle = ComboBoxStyle.DropDownList;

            _isNotClickable = false;
            _suppressRefDocBinding = false;
            _suppressWarehouseBinding = false;
            _isNewMode = true;

            ToggleButtons(true);
            ClearDGVs();
            SetEditableColumns(true);
            ToggleColumn(true);

            cmb_ref_doc.Enabled = true;
            Helpers.ResetControls(pnl_main);
            cmb_ref_doc.SelectedIndex = -1;
            cmb_warehouse_name.SelectedIndex = -1;
        }

        private async void btn_close_Click(object sender, EventArgs e)
        {
            _isNotClickable = true;
            await DisableEditMode();
            SetEditableColumns(false);
            ToggleColumn(false);
            cmb_ref_doc.DropDownStyle = ComboBoxStyle.DropDown;
            await LoadReceivingReports();
        }

        private async Task DisableEditMode()
        {
            _suppressRefDocBinding = true;
            _suppressWarehouseBinding = true;
            ToggleButtons(false);
            cmb_ref_doc.Enabled = false;
            cmb_warehouse_name.Enabled = false;
            await LoadReceivingReports();
        }

        private async void btn_search_Click(object sender, EventArgs e)
        {
            if (_receivingReports == null || _receivingReports.Count == 0)
            {
                await LoadReceivingReports();
            }

            using (var searchForm = new ReceivingReportSearch())
            {
                if (searchForm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(searchForm.SelectedRRId))
                {
                    if (int.TryParse(searchForm.SelectedRRId, out int selectedId))
                    {
                        int index = _receivingReports.FindIndex(r => r.id == selectedId);
                        if (index >= 0)
                        {
                            _currentRRIndex = index;
                            await LoadReceivingReports();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid record ID selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (_currentRRIndex > 0)
            {
                _currentRRIndex--;
                ShowCurrentRecord();
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (_receivingReports != null && _currentRRIndex < _receivingReports.Count - 1)
            {
                _currentRRIndex++;
                ShowCurrentRecord();
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_main != null)
                {
                    if (dgv_main.IsCurrentCellInEditMode)
                    {
                        dgv_main.EndEdit();
                    }

                    if (dgv_main.CurrentCell != null)
                    {
                        dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                }
            }
            catch (Exception)
            {
                Helpers.ShowDialogMessage("error", "Please finish editing in the bin location before saving.");
                return;
            }

            bool hasError = Helpers.ValidateControlsValues(pnl_main);

            if (hasError) // if validation failed
            {
                Helpers.ShowDialogMessage("error", "Please fill in all required fields.");
                return;
            }

            // Validation for received_qty
            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                if (row.IsNewRow) continue;

                string receivedQty = row.Cells["received_qty"]?.Value?.ToString();
                if (string.IsNullOrWhiteSpace(receivedQty))
                {
                    Helpers.ShowDialogMessage("error", "Received quantity is required for all items.");
                    return; // stop save
                }
            }

            // Validation for bin location
            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                if (row.IsNewRow) continue;

                string receivedQty = row.Cells["bin_location"]?.Value?.ToString();
                if (string.IsNullOrWhiteSpace(receivedQty))
                {
                    Helpers.ShowDialogMessage("error", "Bin location is required for all items.");
                    return; // stop save
                }
            }

            // Extra validation for rejection reason
            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                if (row.IsNewRow) continue;

                decimal orderedQty = 0;
                decimal.TryParse(row.Cells["ordered_qty"].Value?.ToString(), out orderedQty);
                decimal receivedQty = 0;
                decimal.TryParse(row.Cells["received_qty"].Value?.ToString(), out receivedQty);
                decimal rejectedQty = 0;
                decimal.TryParse(row.Cells["rejected_qty"].Value?.ToString(), out rejectedQty);

                if (rejectedQty > 0)
                {
                    string reason = row.Cells["reason_for_rejection"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(reason))
                    {
                        Helpers.ShowDialogMessage("error", "Reason for rejection is required when rejected quantity is entered.");
                        return; // stop save
                    }
                }
            }

            // If empty, set current date automatically
            if (string.IsNullOrWhiteSpace(txt_date_received.Text))
            {
                txt_date_received.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                // Only validate if user actually entered something
                DateTime parsedDate;
                if (!DateTime.TryParseExact(txt_date_received.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    Helpers.ShowDialogMessage("error", "Invalid Date format. Please use dd/MM/yyyy (e.g. 23/09/2025).");
                    return;
                }
            }

            // Try parsing supplier_id
            int supplierId = 0;
            int.TryParse(txt_supplier_id.Text, out supplierId);

            // Try parsing purchase_order_id
            int purchaseOrderId = 0;
            int.TryParse(txt_purchase_order_id.Text, out purchaseOrderId);

            int warehouseId = 0;
            if (cmb_warehouse_name.SelectedValue != null)
            {
                int.TryParse(cmb_warehouse_name.SelectedValue.ToString(), out warehouseId);
            }

            // Build ReceivingReportModel from your form controls
            var rrModel = new ReceivingReportModel
            {
                id = string.IsNullOrWhiteSpace(txt_id.Text) ? 0 : int.Parse(txt_id.Text),
                supplier_name = txt_supplier_name.Text,
                supplier_code = txt_supplier_code.Text,
                supplier_id = supplierId,
                purchase_order_id = purchaseOrderId,
                warehouse_name = cmb_warehouse_name.Text,
                ref_doc = cmb_ref_doc.Text,
                date_received = txt_date_received.Text,
                prepared_by = txt_prepared_by.Text,
                address = txt_address.Text,
                warehouse_id = warehouseId
            };

            // Build ReceivingReportDetails from dgv_main
            var rrDetails = new List<ReceivingReportDetailsModel2>();

            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                if (row.IsNewRow) continue;

                int detailId = row.Cells["id"]?.Value == null ? 0 : Convert.ToInt32(row.Cells["id"].Value);
                int podId = row.Cells["pod_id"]?.Value == null ? 0 : Convert.ToInt32(row.Cells["pod_id"].Value);
                string binLocation = row.Cells["cmb_bin_location"]?.Value?.ToString();

                // Validate and transform bin_location
                if (!string.IsNullOrWhiteSpace(binLocation))
                {
                    int dashCount = binLocation.Count(c => c == '-');
                    if (dashCount <= 2) // 3 or fewer parts
                    {
                        binLocation += "-OPEN";
                    }
                }

                var detail = new ReceivingReportDetailsModel2
                {
                    id = detailId,
                    pod_id = podId,
                    item_code = row.Cells["item_code"]?.Value?.ToString(),
                    item_description = row.Cells["item_description"]?.Value?.ToString(),
                    ordered_qty = row.Cells["ordered_qty"]?.Value?.ToString(),
                    ordered_uom = row.Cells["ordered_uom"]?.Value?.ToString(),
                    received_qty = row.Cells["received_qty"]?.Value?.ToString(),
                    received_uom = row.Cells["received_uom"]?.Value?.ToString(),
                    rejected_qty = row.Cells["rejected_qty"]?.Value?.ToString(),
                    rejected_uom = row.Cells["rejected_uom"]?.Value?.ToString(),
                    serial_number = row.Cells["serial_number"]?.Value?.ToString(),
                    reason_for_rejection = row.Cells["reason_for_rejection"]?.Value?.ToString(),
                    bin_location = binLocation,
                    ref_id = Convert.ToInt32(txt_purchase_order_id.Text)
                };

                rrDetails.Add(detail);
            }

            // Wrap everything into ReceivingReportPayload
            var rrPayload = new ReceivingReportPayload
            {
                receiving_report = rrModel,
                receiving_report_details = rrDetails
            };

            try
            {
                Helpers.Loading.ShowLoading(dgv_main, "Saving data...");

                if (_isNewMode) // CREATE
                {
                    var result = await ReceivingReportService.CreateRRRecord(rrPayload);
                    Helpers.ShowDialogMessage("success", "Receiving Report created successfully.");
                }
                else // UPDATE
                {
                    var result = await ReceivingReportService.UpdateRRRecord(rrPayload);
                    Helpers.ShowDialogMessage("success", "Receiving Report updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to save: {ex.Message}");
            }
            finally
            {
                int savedRecordId = 0;
                int.TryParse(txt_id.Text, out savedRecordId);

                await DisableEditMode();
                await LoadPODoc();

                // Reload list and find the index of the saved record
                if (_receivingReports != null && _receivingReports.Count > 0)
                {
                    int index = _receivingReports.FindIndex(r => r.id == savedRecordId);
                    if (index >= 0)
                    {
                        _currentRRIndex = index;
                    }
                    else
                    {
                        // Fallback to last record if new one was added and ID wasn’t in previous list
                        _currentRRIndex = _receivingReports.Count - 1;
                    }

                    ShowCurrentRecord();
                }

                SetEditableColumns(false);
                ToggleColumn(false);
                _isNotClickable = true;
                cmb_ref_doc.DropDownStyle = ComboBoxStyle.DropDown;
                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            if (_currentRRIndex < 0 || _receivingReports == null || _receivingReports.Count == 0)
            {
                Helpers.ShowDialogMessage("error", "No record selected to delete.");
                return;
            }

            var current = _receivingReports[_currentRRIndex];

            var confirm = MessageBox.Show($"Are you sure you want to delete Receiving Report #{current.id}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Helpers.Loading.ShowLoading(dgv_main, "Deleting data...");

                // Build the receiving_report object the same way you do when saving
                int supplierId = 0;
                int.TryParse(txt_supplier_id.Text, out supplierId);

                int purchaseOrderId = 0;
                int.TryParse(txt_purchase_order_id.Text, out purchaseOrderId);

                var rrModel = new ReceivingReportModel
                {
                    id = string.IsNullOrWhiteSpace(txt_id.Text) ? current.id : int.Parse(txt_id.Text),
                };

                var rrPayload = new ReceivingReportPayload
                {
                    receiving_report = rrModel
                };

                // Call delete using the same JSON shape as save
                await ReceivingReportService.DeleteRRRecord(rrPayload);

                Helpers.ShowDialogMessage("success", "Receiving Report deleted successfully.");

                // Move index back if possible to avoid out-of-range after reload
                if (_currentRRIndex > 0) _currentRRIndex--;


            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to delete: {ex.Message}");
            }
            finally
            {
                // Reload list (LoadReceivingReports will re-populate _receivingReports and call ShowCurrentRecord)
                await DisableEditMode();
                await LoadPODoc();
                ClearDGVs();
                SetEditableColumns(false);

                // Reset index back to the first record
                _currentRRIndex = 0;

                // If you have a method to display the current record, call it here
                if (_receivingReports != null && _receivingReports.Count > 0)
                {
                    ShowCurrentRecord();
                }

                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private async void ReceivingReport_Load(object sender, EventArgs e)
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_main, "Fetching data...");
                await LoadPODoc();
                await LoadWarehouse();
                await LoadReceivingReports();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to delete: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private async Task LoadPODoc()
        {
            _suppressRefDocBinding = true;

            _poData = await ReceivingReportService.GetPOWithDetails();

            if (_poData != null && _poData.Count > 0)
            {
                var uniquePOs = _poData
                    .GroupBy(x => x.doc_no)
                    .Select(g => g.First())
                    .ToList();

                cmb_ref_doc.DataSource = uniquePOs;
                cmb_ref_doc.DisplayMember = "doc_no";
                cmb_ref_doc.ValueMember = "id";
                cmb_ref_doc.SelectedIndex = -1;

                Console.WriteLine($"{uniquePOs.Count} unique purchase orders loaded.");
            }
            else
            {
                cmb_ref_doc.DataSource = null;
                Console.WriteLine("No purchase orders found.");
            }

            _suppressRefDocBinding = false;
        }

        private async void cmb_ref_doc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressRefDocBinding) return;

            if (cmb_ref_doc.SelectedIndex >= 0 && cmb_ref_doc.SelectedItem is PurchaseOrderViewModel selectedPO)
            {
                txt_supplier_id.Text = selectedPO.supplier_id.ToString();
                txt_purchase_order_id.Text = selectedPO.id.ToString();
                txt_supplier_name.Text = selectedPO.supplier_name.ToString();
                txt_supplier_code.Text = selectedPO.supplier_code.ToString();
                txt_prepared_by.Text = CacheData.CurrentUser.first_name + " " + CacheData.CurrentUser.last_name + " | " + CacheData.CurrentUser.department;

                // Get fresh details from API using selected PO id
                _poDetailsView = await ReceivingReportService.GetPODetailsView(selectedPO.id);

                // Bind child details into DataGridView
                if (_poDetailsView != null)
                {
                    foreach (var item in _poDetailsView)
                    {
                        // Write to the Output window in Visual Studio
                        Console.WriteLine(
                            $"Id: {item.pod_id}, ItemCode: {item.item_code}, ItemDescription: {item.item_description}, IsComplete: {item.is_complete}"
                        );
                    }

                    dgv_main.DataSource = _poDetailsView;
                }
                else
                {
                    dgv_main.DataSource = null;
                }
            }
            else
            {
                // Clear textboxes when no PO is selected
                txt_supplier_id.Text = string.Empty;
                txt_purchase_order_id.Text = string.Empty;
                txt_supplier_name.Text = string.Empty;

                // Clear grids too
                dgv_main.DataSource = null;
            }
        }

        private async Task LoadWarehouse()
        {
            _suppressWarehouseBinding = true;

            //fill this declared value by the warehouse data
            _warehouseData = await ReceivingReportService.GetWarehouseDetails();

            if (_warehouseData != null && _warehouseData.warehouse_name != null && _warehouseData.warehouse_name.Count > 0)
            {
                cmb_warehouse_name.DataSource = _warehouseData.warehouse_name;
                cmb_warehouse_name.DisplayMember = "name";   // shows warehouse name
                cmb_warehouse_name.ValueMember = "id";       // id is key
                cmb_warehouse_name.SelectedIndex = -1;
            }
            else
            {
                cmb_warehouse_name.DataSource = null;
                Console.WriteLine("No warehouse found.");
            }

            _suppressWarehouseBinding = false;
        }

        private void PopulateBinLocationColumn(List<WarehouseAreaModel> warehouseAreas)
        {
            if (dgv_main.Columns.Contains("cmb_bin_location") &&
                dgv_main.Columns["cmb_bin_location"] is DataGridViewComboBoxColumn cmbCol)
            {
                // Clear old items
                cmbCol.Items.Clear();

                // Build cascade list
                var binLocations = BuildBinLocationList(warehouseAreas);

                foreach (var loc in binLocations)
                {
                    cmbCol.Items.Add(loc);
                }

                // Clear existing selections in the grid to avoid mismatches
                foreach (DataGridViewRow row in dgv_main.Rows)
                {
                    if (row.Cells["cmb_bin_location"] is DataGridViewComboBoxCell cell)
                    {
                        cell.Value = null; // reset the cell value
                    }
                }
            }
        }

        private async void cmb_warehouse_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressWarehouseBinding) return;

            bool enabled = cmb_warehouse_name.SelectedItem != null;

            var col = dgv_main.Columns["cmb_bin_location"];
            col.ReadOnly = !enabled;

            col.DefaultCellStyle.BackColor = enabled ? Color.White : Color.LightGray;

            if (cmb_warehouse_name.SelectedIndex >= 0 && cmb_warehouse_name.SelectedItem is WarehouseNameModel selectedWarehouse)
            {
                // Get filtered warehouse areas
                var warehouseAreas = await ReceivingReportService.GetWarehouseArea(selectedWarehouse.id);

                _warehouseAreas = warehouseAreas ?? new List<WarehouseAreaModel>();

                PopulateBinLocationColumn(_warehouseAreas);

                // Find matching address
                var address = _warehouseData?.warehouse_address?.FirstOrDefault(a => a.warehouse_name_id == selectedWarehouse.id);

                if (address != null)
                {
                    // Build full address string
                    var parts = new List<string>
                    {
                        address.building_no, address.street, address.barangay_no, address.city, address.zip_code, address.country
                    };

                    // Join non-empty values with commas
                    string fullAddress = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                    txt_address.Text = fullAddress;
                }
                else
                {
                    txt_address.Text = string.Empty;
                }
            }
            else
            {
                txt_address.Text = string.Empty;
            }
        }

        private async Task LoadReceivingReports()
        {
            // save current index before reload
            int oldIndex = _currentRRIndex;

            //fill this declared value by the receiving reports data
            _rrData = await ReceivingReportService.GetRRRecords();

            if (_rrData != null && _rrData.receiving_report != null && _rrData.receiving_report.Count > 0)
            {
                //set this variable to the parent of the rr
                _receivingReports = _rrData.receiving_report;

                // restore old index if valid, otherwise fallback to 0
                if (oldIndex >= 0 && oldIndex < _receivingReports.Count)
                    _currentRRIndex = oldIndex;
                else
                    _currentRRIndex = 0;

                ShowCurrentRecord();
            }
            else
            {
                _receivingReports = new List<ReceivingReportModel>();
                _currentRRIndex = -1;
                txt_id.Text = string.Empty;
                dgv_main.DataSource = null;
                btn_prev.Enabled = false;
                btn_next.Enabled = false;
            }
        }

        private void ShowCurrentRecord()
        {
            _suppressRefDocBinding = true;
            _suppressWarehouseBinding = true;

            if (_currentRRIndex < 0 || _rrData == null || _rrData.receiving_report == null || !_rrData.receiving_report.Any())
                return;

            // Convert receiving report list to DataTable using helper
            DataTable rrTable = Helpers.ToDataTable(_rrData.receiving_report);

            if (rrTable.Rows.Count == 0 || _currentRRIndex >= rrTable.Rows.Count)
                return;

            //Bind controls automatically (textboxes, checkboxes, etc.)
            Helpers.BindControls(new Panel[] { pnl_main }, rrTable, _currentRRIndex);
            //Handle ComboBoxes manually (need lookups)
            var current = _receivingReports[_currentRRIndex];

            if (cmb_ref_doc.DataSource != null)
            {
                var poList = cmb_ref_doc.DataSource as List<PurchaseOrderViewModel>;
                var match = poList?.FirstOrDefault(po => po.doc_no == current.ref_doc);
                cmb_ref_doc.SelectedItem = match ?? null;
            }

            if (cmb_ref_doc.DataSource != null)
            {
                var poList = cmb_ref_doc.DataSource as List<PurchaseOrderViewModel>;
                var match = poList?.FirstOrDefault(po => po.doc_no == current.ref_doc);

                if (match != null)
                {
                    cmb_ref_doc.SelectedItem = match;
                }
                else
                {
                    // Show the raw value even if not in the list
                    cmb_ref_doc.SelectedIndex = -1;
                    cmb_ref_doc.Text = current.ref_doc;
                }
            }
            else
            {
                cmb_ref_doc.Text = current.ref_doc;
            }

            //Bind child details (grids)
            if (_rrData?.receiving_report_details != null)
            {
                var detailsForCurrent = _rrData.receiving_report_details.Where(d => d.receiving_report_id == current.id).ToList();

                //Reset mapping for Receiving Report Details
                dgv_main.Columns["ordered_qty"].DataPropertyName = "ordered_qty";
                dgv_main.Columns["ordered_uom"].DataPropertyName = "ordered_uom";

                dgv_main.DataSource = detailsForCurrent;
            }
            else
            {
                dgv_main.DataSource = null;
            }

            // Load files filtered by RR#
            if (RECEIVING_TV.SelectedNode != null)
            {
                string currentPath = RECEIVING_TV.SelectedNode.Tag?.ToString();
                if (!string.IsNullOrEmpty(currentPath))
                {
                    LoadFiles(currentPath); // This will now filter by RR#
                }
            }

            LoadDirectory(RECEIVING_TV, receivingReportPath);

            //Enable/disable navigation buttons
            btn_prev.Enabled = _currentRRIndex > 0;
            btn_next.Enabled = _currentRRIndex < _receivingReports.Count - 1;
        }

        private void dgv_main_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;

            // Ensure the numbering column exists
            if (grid.Columns.Contains("number"))
            {
                grid.Rows[e.RowIndex].Cells["number"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void dgv_main_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var dgv = sender as DataGridView;
            var colName = dgv.Columns[e.ColumnIndex].Name;
            var row = dgv.Rows[e.RowIndex];

            // Get ordered qty
            decimal orderedQty = 0;
            decimal.TryParse(row.Cells["ordered_qty"].Value?.ToString(), out orderedQty);

            if (colName == "received_qty" || colName == "rejected_qty")
            {
                decimal receivedQty = 0, rejectedQty = 0;
                decimal.TryParse(row.Cells["received_qty"].Value?.ToString(), out receivedQty);
                decimal.TryParse(row.Cells["rejected_qty"].Value?.ToString(), out rejectedQty);

                // Validation: received or rejected cannot exceed ordered individually
                if (receivedQty > orderedQty)
                {
                    MessageBox.Show($"Received quantity cannot exceed ordered quantity ({orderedQty}).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["received_qty"].Value = orderedQty;
                    return;
                }

                if (rejectedQty > orderedQty)
                {
                    MessageBox.Show($"Rejected quantity cannot exceed ordered quantity ({orderedQty}).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["rejected_qty"].Value = orderedQty;
                    return;
                }

                // Validation: sum of received + rejected cannot exceed ordered
                if ((receivedQty + rejectedQty) > orderedQty)
                {
                    MessageBox.Show($"The sum of received ({receivedQty}) and rejected ({rejectedQty}) " +
                                    $"cannot exceed ordered quantity ({orderedQty}).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Reset the changed cell back to keep consistency
                    row.Cells[colName].Value = 0;
                    return;
                }

                // Auto-fill UOMs if qty has value and UOM is empty
                string orderedUom = row.Cells["ordered_uom"].Value?.ToString();

                if (receivedQty > 0 && string.IsNullOrWhiteSpace(row.Cells["received_uom"].Value?.ToString()))
                {
                    row.Cells["received_uom"].Value = orderedUom;
                }
                else if (string.IsNullOrWhiteSpace(row.Cells["received_qty"].Value?.ToString()))
                {
                    // Clear received_uom if received_qty is cleared or backspaced
                    row.Cells["received_uom"].Value = string.Empty;
                }

                if (rejectedQty > 0)
                {
                    // Require reason_for_rejection
                    string reason = row.Cells["reason_for_rejection"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(reason))
                    {
                        MessageBox.Show("Reason for rejection is required when a rejected quantity is entered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (string.IsNullOrWhiteSpace(row.Cells["rejected_uom"].Value?.ToString()))
                    {
                        row.Cells["rejected_uom"].Value = orderedUom;
                    }
                }
                else
                {
                    // Clear reason_for_rejection if rejected qty is 0 or empty
                    row.Cells["reason_for_rejection"].Value = string.Empty;

                    // Clear rejected_uom if rejected_qty is cleared or backspaced
                    row.Cells["rejected_uom"].Value = string.Empty;
                }

                if (dgv.Columns.Contains("reason_for_rejection"))
                {
                    row.Cells["reason_for_rejection"].ReadOnly = !(rejectedQty > 0);

                    if (rejectedQty <= 0)
                    {
                        row.Cells["reason_for_rejection"].Value = string.Empty;
                    }
                }
            }
        }

        private void dgv_main_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_main.IsCurrentCellDirty)
            {
                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private List<string> BuildBinLocationList(List<WarehouseAreaModel> warehouseAreas)
        {
            var list = new List<string>();

            foreach (var a in warehouseAreas)
            {
                // Build cascade string, skipping null/empty levels
                var parts = new List<string> { a.zone, a.area, a.rack, a.level, a.bins };
                string cascade = string.Join(" - ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                // Use location_code if you want unique value mapping
                list.Add(cascade);
            }

            return list.Distinct().OrderBy(x => x).ToList();
        }

        private void dgv_main_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Assign combo box control properties to the combo box column in the datagridview
            if (dgv_main.CurrentCell.ColumnIndex == dgv_main.Columns["cmb_bin_location"].Index && e.Control is ComboBox combo)
            {
                //Remove old event handler and replace with our new one
                _binComboBox = combo;
                _binComboBox.SelectedIndexChanged -= BinComboBox_SelectedIndexChanged;
                _binComboBox.SelectedIndexChanged += BinComboBox_SelectedIndexChanged;

                _binComboBox.DropDown -= BinComboBox_DropDown;
                _binComboBox.DropDown += BinComboBox_DropDown;

                _binComboBox.Leave -= BinComboBox_Leave;
                _binComboBox.Leave += BinComboBox_Leave;

                // Start fresh with zones
                LoadZones(_binComboBox);
            }
        }

        //Load the zones
        private void LoadZones(ComboBox combo)
        {
            combo.Items.Clear();
            var zones = _warehouseAreas.Select(x => x.zone).Distinct().OrderBy(x => x);
            foreach (var z in zones) combo.Items.Add(z);
            _currentZone = "";
            _currentArea = "";
            _currentRack = "";
            _currentLevel = "";
        }

        // Load Areas
        private void LoadAreas(ComboBox combo, string zone)
        {
            combo.Items.Clear();
            var areas = _warehouseAreas
                .Where(x => x.zone == zone)
                .Select(x => x.area)
                .Distinct()
                .OrderBy(x => x);

            if (!areas.Any())
            {
                string finalValue = zone;
                dgv_main.CurrentCell.Value = finalValue;
                dgv_main.CurrentRow.Cells["bin_location"].Value = finalValue;

                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgv_main.EndEdit();

                combo.DroppedDown = false; // auto-close
                return;
            }

            foreach (var a in areas)
                combo.Items.Add($"{zone}-{a}");
        }

        // Load Racks
        private void LoadRacks(ComboBox combo, string zone, string area)
        {
            combo.Items.Clear();
            var racks = _warehouseAreas
                .Where(x => x.zone == zone && x.area == area)
                .Select(x => x.rack)
                .Distinct()
                .OrderBy(x => x);

            if (!racks.Any())
            {
                string finalValue = $"{zone}-{area}";
                dgv_main.CurrentCell.Value = finalValue;
                dgv_main.CurrentRow.Cells["bin_location"].Value = finalValue;

                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgv_main.EndEdit();

                combo.DroppedDown = false;
                return;
            }

            foreach (var r in racks)
                combo.Items.Add($"{zone}-{area}-{r}");
        }

        // Load Levels
        private void LoadLevels(ComboBox combo, string zone, string area, string rack)
        {
            combo.Items.Clear();
            var levels = _warehouseAreas
                .Where(x => x.zone == zone && x.area == area && x.rack == rack)
                .Select(x => x.level)
                .Distinct()
                .OrderBy(x => x);

            if (!levels.Any())
            {
                string finalValue = $"{zone}-{area}-{rack}";
                dgv_main.CurrentCell.Value = finalValue;
                dgv_main.CurrentRow.Cells["bin_location"].Value = finalValue;

                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgv_main.EndEdit();

                combo.DroppedDown = false;
                return;
            }

            foreach (var l in levels)
                combo.Items.Add($"{zone}-{area}-{rack}-{l}");
        }

        // Load Bins (with numeric expansion)
        private void LoadBins(ComboBox combo, string zone, string area, string rack, string level)
        {
            combo.Items.Clear();

            var binValues = _warehouseAreas
                .Where(x => x.zone == zone && x.area == area && x.rack == rack && x.level == level)
                .Select(x => x.bins)
                .Distinct()
                .OrderBy(x => x);

            if (!binValues.Any())
            {
                dgv_main.CurrentCell.Value = $"{zone}-{area}-{rack}-{level}";
                combo.DroppedDown = false; // auto-close
                return;
            }

            foreach (var b in binValues)
            {
                if (int.TryParse(b, out int maxBin))
                {
                    for (int i = 1; i <= maxBin; i++)
                        combo.Items.Add($"{zone}-{area}-{rack}-{level}-{i}");
                }
                else
                {
                    combo.Items.Add($"{zone}-{area}-{rack}-{level}-{b}");
                }
            }
        }

        private void BinComboBox_Leave(object sender, EventArgs e)
        {
            ToggleColumn(false);
        }

        private void dgv_main_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isNotClickable) return;

            // Make sure it's not the header row
            if (e.RowIndex >= 0 && e.ColumnIndex == dgv_main.Columns["bin_location"].Index)
            {
                ToggleColumn(true);
                var cmbCol = (DataGridViewComboBoxColumn)dgv_main.Columns["cmb_bin_location"];
                cmbCol.Items.Clear();
            }
        }

        private void BinComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_binComboBox?.SelectedItem == null) return;

            string value = _binComboBox.SelectedItem.ToString();
            dgv_main.CurrentCell.Value = value; // keep selection

            // Also set the value in the bin_location column (text column)
            if (dgv_main.CurrentRow != null && dgv_main.CurrentRow.Cells["bin_location"] != null)
            {
                dgv_main.CurrentRow.Cells["bin_location"].Value = value;
            }

            var parts = value.Split('-');
            if (parts.Length >= 1) _currentZone = parts[0];
            if (parts.Length >= 2) _currentArea = parts[1];
            if (parts.Length >= 3) _currentRack = parts[2];
            if (parts.Length >= 4) _currentLevel = parts[3];
        }

        private void BinComboBox_DropDown(object sender, EventArgs e)
        {
            if (_binComboBox == null) return;

            // Load appropriate items depending on current value
            if (string.IsNullOrEmpty(_currentZone))
                LoadZones(_binComboBox);
            else if (string.IsNullOrEmpty(_currentArea))
                LoadAreas(_binComboBox, _currentZone);
            else if (string.IsNullOrEmpty(_currentRack))
                LoadRacks(_binComboBox, _currentZone, _currentArea);
            else if (string.IsNullOrEmpty(_currentLevel))
                LoadLevels(_binComboBox, _currentZone, _currentArea, _currentRack);
            else
                LoadBins(_binComboBox, _currentZone, _currentArea, _currentRack, _currentLevel);
        }

        private void dgv_main_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Suppress the default exception error
            e.ThrowException = false;
        }
        
        //File managing
        private void panel1_Resize(object sender, EventArgs e)
        {
            int halfWidth = panel1.Width / 2;

            panel2.Width = halfWidth;
            panel2.Dock = DockStyle.Left;

            panel3.Width = halfWidth;
            panel3.Dock = DockStyle.Right;
        }

        private void TV1_preview_Resize(object sender, EventArgs e)
        {
            pictureBox3.Left = (panel2.Width - pictureBox3.Width) / 2;
            pictureBox3.Top = (panel2.Height - pictureBox3.Height) / 2;

            label29.Left = (panel2.Width - label29.Width) / 2;
            label29.Top = pictureBox3.Bottom + 5;
        }

        private void pnl_Receiving_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = (panel3.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (panel3.Height - pictureBox1.Height) / 2;

            label27.Left = (panel3.Width - label27.Width) / 2;
            label27.Top = pictureBox1.Bottom + 5;
        }

        //File Storage
        private void LoadDirectory(TreeView treeView, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Ensure ACTIVE and BENCHED exist with subfolders
            CreateSubDirectories(directoryPath);

            // Clear tree view and add root
            treeView.Nodes.Clear();
            treeView.ImageKey = "folder";
            treeView.SelectedImageKey = "folder";

            TreeNode rootNode = new TreeNode(directoryPath)
            {
                Tag = directoryPath,
                ImageKey = "folder",
                SelectedImageKey = "folder"
            };
            treeView.Nodes.Add(rootNode);

            // Add ACTIVE and BENCHED with subfolders
            LoadManualSubDirectories(directoryPath, rootNode);

            rootNode.ExpandAll();
        }

        private void CreateSubDirectories(string directoryPath)
        {
            string activeDir = Path.Combine(directoryPath, "ACTIVE");
            string benchedDir = Path.Combine(directoryPath, "BENCHED");

            if (!Directory.Exists(activeDir)) Directory.CreateDirectory(activeDir);
            if (!Directory.Exists(benchedDir)) Directory.CreateDirectory(benchedDir);

            foreach (string subDir in new[] { activeDir, benchedDir })
            {
                foreach (string folderName in systemFolders)
                {
                    string folderPath = Path.Combine(subDir, folderName);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
            }
        }

        private void LoadManualSubDirectories(string path, TreeNode parentNode)
        {
            string currentRR = txt_id.Text;
            string rrSuffix = $"_RR{currentRR}";

            foreach (var category in new[] { "ACTIVE", "BENCHED" })
            {
                string categoryPath = Path.Combine(path, category);
                TreeNode categoryNode = new TreeNode(category)
                {
                    Tag = categoryPath,
                    ImageKey = "folder",
                    SelectedImageKey = "folder"
                };
                parentNode.Nodes.Add(categoryNode);

                foreach (var subFolder in Directory.GetDirectories(categoryPath))
                {
                    string folderName = Path.GetFileName(subFolder);

                    // Always show system folders
                    if (systemFolders.Contains(folderName))
                    {
                        TreeNode sysNode = new TreeNode(folderName)
                        {
                            Tag = subFolder,
                            ImageKey = "folder",
                            SelectedImageKey = "folder"
                        };
                        categoryNode.Nodes.Add(sysNode);

                        // Use recursive loader
                        LoadSubDirectoriesRecursive(sysNode, subFolder, rrSuffix);

                        continue;
                    }

                    // For other folders → filter by RR suffix
                    if (!string.IsNullOrEmpty(currentRR) && !folderName.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase))
                        continue;

                    TreeNode subNode = new TreeNode(folderName)
                    {
                        Tag = subFolder,
                        ImageKey = "folder",
                        SelectedImageKey = "folder"
                    };
                    categoryNode.Nodes.Add(subNode);
                }
            }
        }

        private void LoadSubDirectoriesRecursive(TreeNode parentNode, string parentPath, string rrSuffix)
        {
            foreach (var dir in Directory.GetDirectories(parentPath))
            {
                string folderName = Path.GetFileName(dir);

                // Apply RR filter if RR is selected (check for suffix instead of prefix)
                if (!string.IsNullOrEmpty(rrSuffix) &&
                    !folderName.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase) &&
                    !systemFolders.Contains(folderName)) // system folders always show
                {
                    continue;
                }

                TreeNode newNode = new TreeNode(folderName)
                {
                    Tag = dir,
                    ImageKey = "folder",
                    SelectedImageKey = "folder"
                };

                parentNode.Nodes.Add(newNode);

                //Recursive call to load subfolders inside this folder
                LoadSubDirectoriesRecursive(newNode, dir, rrSuffix);
            }
        }

        // Drag and drop event handlers
        private void RECEIVING_LV_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void RECEIVING_LV_DragDrop(object sender, DragEventArgs e)
        {
            if (RECEIVING_TV.SelectedNode == null)
            {
                MessageBox.Show("Please select a folder first to upload files.", "Info",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string targetFolder = RECEIVING_TV.SelectedNode.Tag?.ToString();

            if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
            {
                UploadFiles(files, targetFolder);
            }
        }

        private void InitializeListViewContextMenu()
        {
            ContextMenuStrip lvContextMenu = new ContextMenuStrip();

            ToolStripMenuItem renameFileItem = new ToolStripMenuItem("Rename File");
            renameFileItem.Click += RenameFileItem_Click;

            ToolStripMenuItem deleteFileItem = new ToolStripMenuItem("Delete File");
            deleteFileItem.Click += DeleteFileItem_Click;

            lvContextMenu.Items.Add(renameFileItem);
            lvContextMenu.Items.Add(deleteFileItem);

            RECEIVING_LV.ContextMenuStrip = lvContextMenu;
        }

        private void DeleteFileItem_Click(object sender, EventArgs e)
        {
            if (RECEIVING_LV.SelectedItems.Count == 0 || RECEIVING_LV.SelectedItems[0].Text == "No files found")
                return;

            string currentFile = Path.Combine(GetCurrentDirectory(), RECEIVING_LV.SelectedItems[0].Text);

            if (!File.Exists(currentFile)) return;

            var result = MessageBox.Show($"Are you sure you want to delete the file '{Path.GetFileName(currentFile)}'?",
                                         "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    File.Delete(currentFile);

                    // Refresh the ListView
                    LoadFiles(GetCurrentDirectory());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetCurrentDirectory()
        {
            if (RECEIVING_TV.SelectedNode != null)
            {
                return RECEIVING_TV.SelectedNode.Tag?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        // InputDialog class for getting user input
        private class InputDialog : Form
        {
            private TextBox textBox;
            private Button okButton;
            private Button cancelButton;

            public string InputText => textBox.Text;

            public InputDialog(string title, string prompt, string defaultValue = "")
            {
                InitializeComponents(title, prompt, defaultValue);
            }

            private void InitializeComponents(string title, string prompt, string defaultValue)
            {
                this.Text = title;
                this.Size = new Size(300, 150);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                Label promptLabel = new Label
                {
                    Text = prompt,
                    Location = new Point(10, 10),
                    Size = new Size(260, 20)
                };

                textBox = new TextBox
                {
                    Text = defaultValue,
                    Location = new Point(10, 40),
                    Size = new Size(260, 20)
                };

                okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 70),
                    Size = new Size(75, 25)
                };

                cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(180, 70),
                    Size = new Size(75, 25)
                };

                this.Controls.Add(promptLabel);
                this.Controls.Add(textBox);
                this.Controls.Add(okButton);
                this.Controls.Add(cancelButton);

                this.AcceptButton = okButton;
                this.CancelButton = cancelButton;
            }
        }

        private void RenameFileItem_Click(object sender, EventArgs e)
        {
            if (RECEIVING_LV.SelectedItems.Count == 0 || RECEIVING_LV.SelectedItems[0].Text == "No files found")
                return;

            string currentFile = Path.Combine(GetCurrentDirectory(), RECEIVING_LV.SelectedItems[0].Text);

            if (!File.Exists(currentFile)) return;

            string currentFileName = Path.GetFileName(currentFile);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(currentFileName);
            string extension = Path.GetExtension(currentFileName);

            // Extract RR# suffix (changed from prefix)
            string currentRR = txt_id.Text;
            string rrSuffix = $"_RR{currentRR}"; // Changed from RR{currentRR}_

            if (!nameWithoutExt.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This file is not associated with the current RR and cannot be renamed.",
                                "Rename Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ask user for the new name (without RR# suffix)
            string nameWithoutSuffix = nameWithoutExt.Substring(0, nameWithoutExt.Length - rrSuffix.Length);

            using (var dialog = new InputDialog("Rename File", "Enter new file name:", nameWithoutSuffix))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFileNameWithoutSuffix = dialog.InputText.Trim();
                    string newFileName = $"{newFileNameWithoutSuffix}{rrSuffix}{extension}"; // Changed from {rrPrefix}{newFileNameWithoutPrefix}{extension}
                    string newFilePath = Path.Combine(GetCurrentDirectory(), newFileName);

                    try
                    {
                        File.Move(currentFile, newFilePath);

                        // Refresh the ListView
                        LoadFiles(GetCurrentDirectory());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming file: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UploadFiles(string[] files, string targetFolder)
        {
            try
            {
                int successCount = 0;
                int errorCount = 0;
                string rrNumber = txt_id.Text;

                foreach (string file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            string originalFileName = Path.GetFileName(file);
                            string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                            string extension = Path.GetExtension(originalFileName);

                            // Change: Move RR# to suffix instead of prefix
                            string newFileName = $"{nameWithoutExt}_RR{rrNumber}{extension}"; // Changed from RR{rrNumber}_{originalFileName}

                            string destinationPath = Path.Combine(targetFolder, newFileName);

                            // If file exists, ask to overwrite or rename
                            if (File.Exists(destinationPath))
                            {
                                var result = MessageBox.Show($"File '{newFileName}' already exists. Overwrite?",
                                                           "File Exists",
                                                           MessageBoxButtons.YesNoCancel,
                                                           MessageBoxIcon.Question);

                                if (result == DialogResult.No)
                                {
                                    // Add timestamp to filename
                                    newFileName = $"{nameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}_RR{rrNumber}{extension}";
                                    destinationPath = Path.Combine(targetFolder, newFileName);
                                }
                                else if (result == DialogResult.Cancel)
                                {
                                    continue;
                                }
                            }

                            File.Copy(file, destinationPath, true);
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"Error uploading {file}: {ex.Message}");
                    }
                }

                // Refresh the file list
                LoadFiles(targetFolder);

                MessageBox.Show($"Files uploaded successfully: {successCount}\nFailed: {errorCount}",
                              "Upload Complete",
                              MessageBoxButtons.OK,
                              successCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFiles(string path)
        {
            try
            {
                RECEIVING_LV.Items.Clear();

                // Configure ListView for better appearance
                RECEIVING_LV.View = View.Details;
                RECEIVING_LV.FullRowSelect = true;
                RECEIVING_LV.GridLines = false;
                RECEIVING_LV.HeaderStyle = ColumnHeaderStyle.Nonclickable;

                // Ensure columns exist and are properly sized
                if (RECEIVING_LV.Columns.Count == 0)
                {
                    RECEIVING_LV.Columns.Add("File Name", 250);
                    RECEIVING_LV.Columns.Add("Size", 80);
                    RECEIVING_LV.Columns.Add("Modified", 120);
                    RECEIVING_LV.Columns.Add("Type", 100);
                }

                if (Directory.Exists(path))
                {
                    // Get all files and sort by name
                    var files = Directory.GetFiles(path)
                                        .OrderBy(f => Path.GetFileName(f))
                                        .ToArray();

                    // Get current RR number for filtering
                    string currentRRNumber = txt_id.Text;
                    string rrSuffix = $"_RR{currentRRNumber}"; // Changed from RR{currentRRNumber}_

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        string fileName = fi.Name;
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

                        // Filter: Only show files that end with the current RR# suffix
                        // OR show all files if no RR is selected (txt_doc is empty)
                        if (!string.IsNullOrEmpty(currentRRNumber) &&
                            !nameWithoutExt.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase))
                        {
                            continue; // Skip files that don't match the RR# suffix
                        }

                        ListViewItem item = new ListViewItem(fileName);

                        // Format file size with appropriate units
                        string fileSize = FormatFileSize(fi.Length);

                        // Format date in a more readable format
                        string modifiedDate = fi.LastWriteTime.ToString("MMM dd, yyyy hh:mm tt");

                        // Get file type/extension
                        string fileType = fi.Extension.ToUpper().TrimStart('.');
                        if (string.IsNullOrEmpty(fileType)) fileType = "File";

                        item.SubItems.Add(fileSize);
                        item.SubItems.Add(modifiedDate);
                        item.SubItems.Add(fileType);

                        // Set appropriate icon based on file type
                        SetFileIcon(item, fi.Extension);

                        RECEIVING_LV.Items.Add(item);
                    }

                    // Show message if no files found
                    if (RECEIVING_LV.Items.Count == 0)
                    {
                        ListViewItem emptyItem = new ListViewItem("No files found");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        RECEIVING_LV.Items.Add(emptyItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private void SetFileIcon(ListViewItem item, string extension)
        {
            // You can expand this method to set different icons based on file type
            // For now, using a simple approach - you might want to use ImageList with icons

            switch (extension.ToLower())
            {
                case ".pdf":
                    item.ImageKey = "pdf";
                    break;
                case ".doc":
                case ".docx":
                    item.ImageKey = "word";
                    break;
                case ".xls":
                case ".xlsx":
                    item.ImageKey = "excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    item.ImageKey = "image";
                    break;
                default:
                    item.ImageKey = "file";
                    break;
            }
        }

        private void InitializeContextMenu()
        {
            // Create context menu items
            ToolStripMenuItem addFolderItem = new ToolStripMenuItem("Add Folder");
            ToolStripMenuItem renameItem = new ToolStripMenuItem("Rename");
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
            ToolStripSeparator separator = new ToolStripSeparator();

            // Add click events
            addFolderItem.Click += AddFolderItem_Click;
            renameItem.Click += RenameItem_Click;
            deleteItem.Click += DeleteItem_Click;

            // Add items to context menu
            treeViewContextMenu.Items.AddRange(new ToolStripItem[] {
                addFolderItem,
                separator,
                renameItem,
                deleteItem
            });

            // Assign context menu to TreeView
            RECEIVING_TV.ContextMenuStrip = treeViewContextMenu;
        }

        private void AddFolderItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null) return;

            string parentPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(parentPath)) return;

            using (var dialog = new InputDialog("Add New Folder", "Enter folder name:"))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFolderName = dialog.InputText.Trim();

                    // Change: Move RR# prefix to the end
                    string rrNumber = txt_id.Text;
                    newFolderName = $"{newFolderName}_RR{rrNumber}"; // Changed from RR#{rrNumber}_{newFolderName}

                    string newFolderPath = Path.Combine(parentPath, newFolderName);

                    try
                    {
                        Directory.CreateDirectory(newFolderPath);

                        TreeNode newNode = new TreeNode(newFolderName)
                        {
                            Tag = newFolderPath,
                            ImageKey = "folder",
                            SelectedImageKey = "folder"
                        };
                        selectedNode.Nodes.Add(newNode);
                        selectedNode.Expand();

                        RECEIVING_TV.SelectedNode = newNode;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating folder: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RenameItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || selectedNode.Parent == null) return;

            string currentPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(currentPath)) return;

            string currentFolderName = Path.GetFileName(currentPath);

            // Extract RR# suffix (changed from prefix)
            string currentRR = txt_id.Text;
            string rrSuffix = $"_RR{currentRR}"; // Changed from RR#{currentRR}_

            // If folder doesn't have suffix, do not allow renaming
            if (!currentFolderName.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This folder is not associated with the current RR and cannot be renamed.",
                                "Rename Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ask user for new name (excluding suffix)
            string nameWithoutSuffix = currentFolderName.Substring(0, currentFolderName.Length - rrSuffix.Length);

            using (var dialog = new InputDialog("Rename Folder", "Enter new folder name:", nameWithoutSuffix))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFolderNameWithoutSuffix = dialog.InputText.Trim();
                    string newFolderName = $"{newFolderNameWithoutSuffix}{rrSuffix}"; // Changed from {rrPrefix}{newFolderNameWithoutPrefix}

                    string parentDirectory = Path.GetDirectoryName(currentPath);
                    string newFolderPath = Path.Combine(parentDirectory, newFolderName);

                    try
                    {
                        // Rename directory
                        Directory.Move(currentPath, newFolderPath);

                        // Update TreeView
                        selectedNode.Text = newFolderName;
                        selectedNode.Tag = newFolderPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming folder: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || selectedNode.Parent == null) return;

            string folderPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(folderPath)) return;

            var result = MessageBox.Show($"Are you sure you want to delete the folder '{selectedNode.Text}'?",
                                       "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete directory (recursively)
                    Directory.Delete(folderPath, true);

                    // Remove from TreeView
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RECEIVING_TV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Check if the node has children (means it's a parent node)
            if (e.Node.Nodes.Count > 0)
            {
                // Parent node → show panel
                pnl_Receiving.Visible = true;
            }
            else
            {
                // Child node → hide panel
                pnl_Receiving.Visible = false;
            }

            string path = GetFullPath(e.Node);
            LoadFiles(path);
        }

        private string GetFullPath(TreeNode node)
        {
            if (node.Parent == null) return node.Text;
            return Path.Combine(GetFullPath(node.Parent), node.Text);
        }

        private bool IsSystemFolder(TreeNode node)
        {
            // Check if this is one of the predefined system folders
            string[] systemFolders = {"DELIVERY RECEIPT", "ITEM PICTURES" };
            return systemFolders.Contains(node.Text);
        }

        private void RECEIVING_TV_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the node under the mouse pointer
                selectedNode = RECEIVING_TV.GetNodeAt(e.X, e.Y);
                if (selectedNode != null)
                {
                    RECEIVING_TV.SelectedNode = selectedNode;

                    // Enable/disable menu items based on node type
                    bool isRoot = selectedNode.Parent == null;
                    bool isCategory = selectedNode.Text == "ACTIVE" || selectedNode.Text == "BENCHED";
                    bool isSystemFolder = IsSystemFolder(selectedNode);

                    treeViewContextMenu.Items[0].Enabled = !isRoot; // Add Folder
                    treeViewContextMenu.Items[2].Enabled = !isRoot && !isCategory && !isSystemFolder; // Rename
                    treeViewContextMenu.Items[3].Enabled = !isRoot && !isCategory && !isSystemFolder; // Delete
                }
            }
        }

        private void RECEIVING_LV_DoubleClick(object sender, EventArgs e)
        {
            if (RECEIVING_LV.SelectedItems.Count > 0 && RECEIVING_LV.SelectedItems[0].Text != "No files found")
            {
                string selectedFile = Path.Combine(GetCurrentDirectory(), RECEIVING_LV.SelectedItems[0].Text);
                try
                {
                    System.Diagnostics.Process.Start(selectedFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RECEIVING_LV_MouseEnter(object sender, EventArgs e)
        {
            if (RECEIVING_TV.SelectedNode != null)
            {
                toolTip1.SetToolTip(RECEIVING_LV, "Drag and drop files here to upload to the selected folder");
            }
            else
            {
                toolTip1.SetToolTip(RECEIVING_LV, "Select a folder first to upload files");
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (RECEIVING_TV.SelectedNode == null)
            {
                MessageBox.Show("Please select a folder first to upload files.", "Info",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select files to upload";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string targetFolder = RECEIVING_TV.SelectedNode.Tag?.ToString();
                    if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
                    {
                        UploadFiles(openFileDialog.FileNames, targetFolder);
                    }
                }
            }
        }
    }
}
