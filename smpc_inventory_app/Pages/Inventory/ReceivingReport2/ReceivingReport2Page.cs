using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_inventory_app.Services.Setup.Inventory;
using smpc_inventory_app.Model;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Pages.Inventory.ReceivingReport2.ReceivingReport2Modals;
using smpc_inventory_app.Data;
using smpc_inventory_app.Properties;
using System.IO;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Pages.Inventory.ReceivingReport2
{
    public partial class ReceivingReport2Page : UserControl
    {
        ReceivingReport2Service receivingReportService = new ReceivingReport2Service();
        private int _currentRRIndex = -1;
        private int _previousRRIndex = -1;
        private ReceivingReportList _rrdata;
        private List<ReceivingWarehouseView> _warehousedata;
        private List<ReceivingWarehouseAreaView> _warehouseAreadata;
        private List<ReceivingPurchaseOrderDocView> _purchaseDocdata;
        private PurchaseOrderReceivingViewList _purchasedata;
        private List<ReceivingReportModel> _receivingReports;
        private DataTable _rrTable;
        private BindingList<ReceivingReportDetailsModel> _currentDetails;
        private bool _isNewMode = false;
        private bool _isEditMode = false;
        private bool _isEditing = false;
        private string _userName;
        private readonly string receivingReportPath = Settings.Default.RECEIVINGREPORTPATH;
        private TreeNode _selectedNode;
        GeneralService<ReceivingWarehouseView> warehouseServiceSetup;
        GeneralService<ReceivingWarehouseAreaView> warehouseAreaServiceSetup;
        GeneralService<ReceivingPurchaseOrderDocView> purchaseOrderDocserviceSetup;
        GeneralService<PurchaseOrderReceivingViewList> purchaseOrderserviceSetup;
        private int _warehouse_id;
        private int _purchase_order_id;
        private BinLocationComboOverlay _binLocationOverlay;

        private readonly string[] _systemFolders =
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
            { "REMAINING", new string[] { "remaining_qty", "remaining_uom" } },
        };

        public ReceivingReport2Page()
        {
            InitializeComponent();

            Helpers.EnableGroupHeaders(dgv_main, columnGroupsMain);

            _userName = CacheData.CurrentUser.first_name + " " + CacheData.CurrentUser.last_name;

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

            _binLocationOverlay = new BinLocationComboOverlay(
               dgv_main,
               () => _warehouseAreadata
           );
        }

        private void SetEditableColumns(bool isEdit)
        {
            var alwaysEditableColumns = new[] { "received_qty", "rejected_qty", "serial_number", "bin_location" };
            var newModeOnlyColumns = new[] { "received_qty", "rejected_qty", "bin_location" };

            foreach (var colName in alwaysEditableColumns)
            {
                if (dgv_main.Columns.Contains(colName))
                {
                    var column = dgv_main.Columns[colName];

                    // received_qty is editable ONLY in new mode, readonly in edit mode
                    if (newModeOnlyColumns.Contains(colName))
                        column.ReadOnly = !isEdit || _isEditMode;
                    else
                        column.ReadOnly = !isEdit;

                    column.DefaultCellStyle.BackColor = column.ReadOnly ? Color.Gainsboro : Color.White;
                }
            }

            // Apply per-row rejection reason readonly state when entering edit mode
            if (isEdit)
                UpdateRejectionReasonReadOnly();
        }

        private void UpdateRejectionReasonReadOnly()
        {
            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                var rejectedQtyVal = row.Cells["rejected_qty"].Value;

                decimal.TryParse(rejectedQtyVal?.ToString(), out decimal rejectedQty);

                bool isReadOnly = rejectedQty <= 0;

                row.Cells["reason_for_rejection"].ReadOnly = isReadOnly;
                row.Cells["reason_for_rejection"].Style.BackColor = isReadOnly ? Color.Gainsboro : Color.White;

                // Clear the value if it becomes readonly
                if (isReadOnly)
                    row.Cells["reason_for_rejection"].Value = null;
            }
        }

        private async Task SetEditMode(bool enable, bool isNewMode = false)
        {
            _isNewMode = isNewMode;
            _isEditing = enable;
            _isEditMode = enable && !isNewMode;

            SetEditableColumns(enable);

            // buttons
            string[] editButtons = { "btn_save", "btn_cancel" };
            string[] navButtons = { "btn_new", "btn_print", "btn_edit", "btn_delete", "btn_next", "btn_prev", "btn_search" };

            Helpers.SetButtonVisibility(
                toolStrip1,
                pnl_main,
                visibleButtons: enable ? editButtons : navButtons,
                hiddenButtons: enable ? navButtons : editButtons
            );

            Helpers.SetChildControlsEnabled2(new[] { pnl_main }, !enable, new string[] { "txt_doc_no", "txt_supplier_code",
                "txt_warehouse_address", "txt_supplier", "txt_prepared_by", "cmb_ref_doc", "cmb_warhouse" });

            // Manually control cmb_ref_doc: enabled only in new mode, disabled in edit mode
            cmb_ref_doc.Enabled = enable && _isNewMode;
            cmb_ref_doc.DropDownStyle = (enable && _isNewMode) ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
            cmb_ref_doc.BackColor = (enable && _isNewMode) ? Color.White : Color.FromArgb(235, 235, 235);

            cmb_warehouse.Enabled = enable && _isNewMode;
            cmb_warehouse.DropDownStyle = (enable && _isNewMode) ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
            cmb_warehouse.BackColor = (enable && _isNewMode) ? Color.White : Color.FromArgb(235, 235, 235);

            // Load and bind combos only when entering edit or new mode
            if (enable)
            {
                await LoadPurchaseOrderDoc();
                await LoadWarehouse();

                cmb_ref_doc.SelectedIndexChanged -= cmb_ref_doc_SelectedIndexChanged;
                cmb_warehouse.SelectedIndexChanged -= cmb_warehouse_SelectedIndexChanged;

                if (_purchaseDocdata != null)
                {
                    cmb_ref_doc.DataSource = _purchaseDocdata;
                    cmb_ref_doc.DisplayMember = "po_doc_no";
                    cmb_ref_doc.ValueMember = "purchase_order_id";
                }

                if (_isEditMode)
                {
                    cmb_ref_doc.SelectedIndex = -1;
                    cmb_ref_doc.Text = txt_ref_doc.Text;
                }

                if (_warehousedata != null)
                {
                    cmb_warehouse.DataSource = _warehousedata;
                    cmb_warehouse.DisplayMember = "warehouse";
                    cmb_warehouse.ValueMember = "warehouse_id";
                }

                cmb_ref_doc.SelectedIndexChanged += cmb_ref_doc_SelectedIndexChanged;
                cmb_warehouse.SelectedIndexChanged += cmb_warehouse_SelectedIndexChanged;
            }
            else
            {
                // Clear combos when leaving edit mode
                cmb_ref_doc.DataSource = null;
                cmb_warehouse.DataSource = null;
            }

            _binLocationOverlay.SetEditingMode(enable);
        }

        private void ChangeRecord(int step)
        {
            if (_receivingReports == null || !_receivingReports.Any()) return;

            int newIndex = _currentRRIndex + step;
            if (newIndex >= 0 && newIndex < _receivingReports.Count)
            {
                _currentRRIndex = newIndex;
                ShowCurrentRecord();
            }
        }

        private async void btn_new_Click(object sender, EventArgs e)
        {
            // Save current index before clearing
            _previousRRIndex = _currentRRIndex;
            await SetEditMode(true, isNewMode: true);

            //Clear only the rows, keep columns
            _currentDetails = new BindingList<ReceivingReportDetailsModel>();
            dgv_main.AutoGenerateColumns = false;
            dgv_main.DataSource = _currentDetails;
            Helpers.ResetControls(pnl_main);
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
                        Helpers.ShowDialogMessage("error", "Invalid record ID selected.");
                    }
                }
            }
        }

        private async void btn_edit_Click(object sender, EventArgs e)
        {
            if (_currentRRIndex < 0 || _receivingReports == null || !_receivingReports.Any())
            {
                Helpers.ShowDialogMessage("error", "No record selected to edit.");
                return;
            }

            // Store last viewed record index
            _previousRRIndex = _currentRRIndex;

            await SetEditMode(true);
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            ChangeRecord(-1);
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            ChangeRecord(1);
        }

        private async void btn_cancel_Click(object sender, EventArgs e)
        {
            await SetEditMode(false);

            // If no records exist, clear everything
            if (_receivingReports == null || !_receivingReports.Any())
            {
                ClearReceivingReportUI();
                return;
            }

            // Return to the previous record index if available
            if (_previousRRIndex >= 0 && _receivingReports != null && _receivingReports.Count > 0)
            {
                _currentRRIndex = _previousRRIndex;
                await LoadReceivingReports();
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            if (_currentRRIndex < 0)
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

                var receivingReportParent = Helpers.BuildModelFromPanels<ReceivingReportModel>(new Panel[] { pnl_main });
                var receivingReportDetails = Helpers.DatagridviewMapper.BuildModelsFromData<ReceivingReportDetailsModel>(dgv_main);

                var rrPayload = new ReceivingReportPayload
                {
                    receiving_report = receivingReportParent,
                    receiving_report_details = receivingReportDetails
                };

                var result = await receivingReportService.DeleteReceivingReport(rrPayload);

                if (!result.Success)
                {
                    Helpers.ShowDialogMessage("error", "Receiving Report not deleted.");
                    return;
                }

                Helpers.ShowDialogMessage("success", "Receiving Report deleted successfully.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to delete: {ex.Message}");
            }
            finally
            {
                await LoadReceivingReports();

                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;

            _binLocationOverlay.Hide();

            try
            {
                dgv_main.EndEdit();

                string[] saveExcludes = _isEditMode ? new string[] { "cmb_ref_doc" } : null;

                if (Helpers.ValidateControlsValues2(new Panel[] { pnl_main }, saveExcludes))
                {
                    Helpers.ShowDialogMessage("error", "Please fill in all required fields.");
                    return;
                }

                //Validate Datagridview columns
                string[] columnsToValidate = { "item_code", "item_desc", "ordered_qty" };
                if (await Helpers.ValidateDataGridViewCells(dgv_main, columnsToValidate))
                    return;

                // Validate receiving detail quantities and rejection reasons
                if (!ValidateReceivingDetails())
                    return;

                var receivingReportParent = Helpers.BuildModelFromPanels<ReceivingReportModel>(new Panel[] { pnl_main });

                receivingReportParent.prepared_by = _userName;

                //Validate that receiving report details are not empty
                var receivingReportDetails = Helpers.DatagridviewMapper.BuildModelsFromData<ReceivingReportDetailsModel>(dgv_main);
                if (receivingReportDetails == null || receivingReportDetails.Count == 0)
                {
                    Helpers.ShowDialogMessage("error", "Receiving Report cannot be empty.");
                    return;
                }

                // Wrap everything into Journal Entry Payload
                var rrPayload = new ReceivingReportPayload
                {
                    receiving_report = receivingReportParent,
                    receiving_report_details = receivingReportDetails
                };

                Helpers.Loading.ShowLoading(dgv_main, "Saving data...");

                if (_isNewMode)
                {
                    var result = await receivingReportService.CreateReceivingReport(rrPayload);

                    if (!result.Success)
                    {
                        Helpers.ShowDialogMessage("error", "Receiving Report not created.");
                        return;
                    }

                    Helpers.ShowDialogMessage("success", "Receiving Report created successfully.");
                }
                else
                {
                    var result = await receivingReportService.UpdateReceivingReport(rrPayload);

                    if (!result.Success)
                    {
                        Helpers.ShowDialogMessage("error", "Receiving Report not updated.");
                        return;
                    }

                    Helpers.ShowDialogMessage("success", "Receiving Report updated successfully.");
                }

                await SetEditMode(false);
                await LoadReceivingReports();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to save: {ex.Message}");
            }
            finally
            {
                btn_save.Enabled = true;
                btn_cancel.Enabled = true;

                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private bool ValidateReceivingDetails()
        {
            if (_isEditMode)
                return true;

            bool hasAtLeastOneReceived = false;

            for (int i = 0; i < dgv_main.Rows.Count; i++)
            {
                var row = dgv_main.Rows[i];

                // Get cell values
                var receivedQtyVal = row.Cells["received_qty"].Value;
                var rejectedQtyVal = row.Cells["rejected_qty"].Value;
                var remainingQtyVal = row.Cells["remaining_qty"].Value;
                var reasonVal = row.Cells["reason_for_rejection"].Value;

                decimal receivedQty = 0, rejectedQty = 0, remainingQty = 0;

                decimal.TryParse(receivedQtyVal?.ToString(), out receivedQty);
                decimal.TryParse(rejectedQtyVal?.ToString(), out rejectedQty);
                decimal.TryParse(remainingQtyVal?.ToString(), out remainingQty);

                // Check if at least one row has received qty
                if (receivedQty > 0)
                    hasAtLeastOneReceived = true;

                // Validate: received qty requires bin_location
                if (receivedQty > 0)
                {
                    var binLocationVal = row.Cells["bin_location"].Value;
                    bool hasBinLocation = binLocationVal != null
                        && !string.IsNullOrWhiteSpace(binLocationVal.ToString());

                    if (!hasBinLocation)
                    {
                        string itemCode = row.Cells["item_code"].Value?.ToString() ?? $"Row {i + 1}";
                        Helpers.ShowDialogMessage("error",
                            $"Row {i + 1} ({itemCode}): A bin location is required when a received quantity is entered.");
                        return false;
                    }
                }

                // Validate: received + rejected must not exceed remaining
                if ((receivedQty + rejectedQty) > remainingQty)
                {
                    string itemCode = row.Cells["item_code"].Value?.ToString() ?? $"Row {i + 1}";
                    Helpers.ShowDialogMessage("error",
                        $"Row {i + 1} ({itemCode}): The sum of received ({receivedQty}) and rejected ({rejectedQty}) quantities exceeds the remaining quantity ({remainingQty}).");
                    return false;
                }

                // Validate: rejected qty requires reason_for_rejection
                if (rejectedQty > 0)
                {
                    bool hasReason = reasonVal != null
                        && !string.IsNullOrWhiteSpace(reasonVal.ToString());

                    if (!hasReason)
                    {
                        string itemCode = row.Cells["item_code"].Value?.ToString() ?? $"Row {i + 1}";
                        Helpers.ShowDialogMessage("error",
                            $"Row {i + 1} ({itemCode}): A reason for rejection is required when a rejected quantity is entered.");
                        return false;
                    }
                }
            }

            // Validate: at least one row must have a received qty
            if (!hasAtLeastOneReceived)
            {
                Helpers.ShowDialogMessage("error", "At least one item must have a received quantity greater than zero.");
                return false;
            }

            return true;
        }

        private async void ReceivingReport2Page_Load(object sender, EventArgs e)
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_main, "Fetching data...");
                await LoadReceivingReports();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_main);
            }
        }

        private void ClearReceivingReportUI()
        {
            _receivingReports = new List<ReceivingReportModel>();
            _currentRRIndex = -1;
            _previousRRIndex = -1;

            // Clear panel fields
            Helpers.ResetControls(new Panel[] { pnl_main });

            // Clear grid
            dgv_main.DataSource = null;
            dgv_main.Rows.Clear();

            // Disable navigation buttons
            btn_prev.Enabled = false;
            btn_next.Enabled = false;
        }

        private async Task LoadReceivingReports()
        {
            // save current index before reload
            int oldIndex = _currentRRIndex;

            //fill this declared value by the receiving report data
            _rrdata = await receivingReportService.GetAsModel();

            if (_rrdata != null && _rrdata.receiving_report != null && _rrdata.receiving_report.Count > 0)
            {
                //set this variable to the parent of the receiving report
                _receivingReports = _rrdata.receiving_report;

                // restore old index if valid, otherwise fallback to 0
                if (oldIndex >= 0 && oldIndex < _receivingReports.Count)
                    _currentRRIndex = oldIndex;
                else
                    _currentRRIndex = 0;

                ShowCurrentRecord();
            }
            else
            {
                ClearReceivingReportUI();
            }
        }

        private async Task LoadWarehouse()
        {
            if (!_isEditing)
                return;

            try
            {
                warehouseServiceSetup = new GeneralService<ReceivingWarehouseView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE);
                _warehousedata = await warehouseServiceSetup.GetAsList();
            }
            catch (NullReferenceException)
            {
                Helpers.ShowDialogMessage("error", "No Warehouse found.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
        }

        private async Task LoadWarehouseArea()
        {
            if (!_isEditing)
                return;

            if (_warehouse_id <= 0)
            {
                Helpers.ShowDialogMessage("error", "Please select a warehouse first.");
                return;
            }

            try
            {
                warehouseAreaServiceSetup = new GeneralService<ReceivingWarehouseAreaView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE_AREA + _warehouse_id);
                _warehouseAreadata = await warehouseAreaServiceSetup.GetAsList();
                _binLocationOverlay.RefreshAreaData();
            }
            catch (NullReferenceException)
            {
                Helpers.ShowDialogMessage("error", "No Warehouse area found.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
        }

        private async Task LoadPurchaseOrderDoc()
        {
            try
            {
                purchaseOrderDocserviceSetup = new GeneralService<ReceivingPurchaseOrderDocView>(ENUM_ENDPOINT.RECEIVING_REPORT_PURCHASE_DOC);
                _purchaseDocdata = await purchaseOrderDocserviceSetup.GetAsList();
            }
            catch (NullReferenceException)
            {
                Helpers.ShowDialogMessage("error", "No Purchase Order Doc found.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
        }

        private async Task LoadPurchaseOrders()
        {
            if (!_isEditing)
                return;

            if (_purchase_order_id <= 0)
            {
                Helpers.ShowDialogMessage("error", "Please select a reference doc first.");
                return;
            }

            try
            {
                purchaseOrderserviceSetup = new GeneralService<PurchaseOrderReceivingViewList>(ENUM_ENDPOINT.RECEIVING_REPORT_PURCHASE + _purchase_order_id);
                _purchasedata = await purchaseOrderserviceSetup.GetAsModel();

                // Bind purchase_order_view to the panel
                if (_purchasedata?.purchase_order_view != null && _purchasedata.purchase_order_view.Any())
                {
                    var poTable = Helpers.ToDataTable(_purchasedata.purchase_order_view);
                    Helpers.BindControls2(new Panel[] { pnl_main }, poTable, 0);
                }

                // Bind purchase_order_details_view to the datagridview
                if (_purchasedata?.purchase_order_details_view != null)
                {
                    _currentDetails = new BindingList<ReceivingReportDetailsModel>(
                        _purchasedata.purchase_order_details_view
                            .Select(d => new ReceivingReportDetailsModel
                            {
                                purchase_order_details_id = d.purchase_order_details_id,
                                item_id = d.item_id,
                                item_code = d.item_code,
                                item_desc = d.item_desc,
                                ordered_qty = d.ordered_qty,
                                ordered_uom = d.ordered_uom,
                                remaining_qty = d.remaining_qty,
                                remaining_uom = d.remaining_uom,
                                warehouse_id = _warehouse_id
                            })
                            .ToList()
                    );

                    dgv_main.AutoGenerateColumns = false;
                    dgv_main.DataSource = _currentDetails;
                }
                else
                {
                    _currentDetails = new BindingList<ReceivingReportDetailsModel>();
                    dgv_main.DataSource = _currentDetails;
                }
            }
            catch (NullReferenceException)
            {
                Helpers.ShowDialogMessage("error", "No Purchase Order found.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
        }

        private void ShowCurrentRecord()
        {
            if (_currentRRIndex < 0 || _rrdata == null || _rrdata.receiving_report == null || !_rrdata.receiving_report.Any())
                return;

            // Convert receiving report list to DataTable using helper
            _rrTable = Helpers.ToDataTable(_rrdata.receiving_report);

            Helpers.BindControls2(new Panel[] { pnl_main }, _rrTable, _currentRRIndex);

            //Disable auto column generation before setting the data source
            dgv_main.AutoGenerateColumns = false;

            var current = _receivingReports[_currentRRIndex];

            //Bind child details (grids)
            if (_rrdata?.receiving_report_details != null)
            {
                _currentDetails = new BindingList<ReceivingReportDetailsModel>(
                    _rrdata.receiving_report_details
                        .Where(d => d.receiving_report_id == current.id)
                        .ToList()
                );

                dgv_main.DataSource = _currentDetails;
            }
            else
            {
                dgv_main.DataSource = null;
            }

            //Enable/disable navigation buttons
            btn_prev.Enabled = _currentRRIndex > 0;
            btn_next.Enabled = _currentRRIndex < _receivingReports.Count - 1;
        }

        private async void cmb_warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isEditing)
                return;

            if (cmb_warehouse.SelectedItem is ReceivingWarehouseView selectedWarehouse)
            {
                txt_warehouse_address.Text = selectedWarehouse.warehouse_address;
                _warehouse_id = selectedWarehouse.warehouse_id;
                txt_warehouse_id.Text = _warehouse_id.ToString();

                if (_currentDetails != null)
                {
                    foreach (var detail in _currentDetails)
                        detail.warehouse_id = _warehouse_id;

                    dgv_main.Refresh();
                }

                await LoadWarehouseArea();
            }
            else
            {
                txt_warehouse_address.Text = string.Empty;
                _warehouse_id = 0;
            }
        }

        private async void cmb_ref_doc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isEditing)
                return;

            if (cmb_ref_doc.SelectedItem is ReceivingPurchaseOrderDocView selectedPurchaseOrder)
            {
                _purchase_order_id = selectedPurchaseOrder.purchase_order_id;
                txt_ref_doc.Text = selectedPurchaseOrder.po_doc_no;
                await LoadPurchaseOrders();
            }
            else
            {
                _purchase_order_id = 0;
                _purchasedata = null;
            }
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

        private void panel1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = (panel3.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (panel3.Height - pictureBox1.Height) / 2;

            label27.Left = (panel3.Width - label27.Width) / 2;
            label27.Top = pictureBox1.Bottom + 5;
        }

        private void TV1_preview_Resize(object sender, EventArgs e)
        {
            pictureBox3.Left = (panel2.Width - pictureBox3.Width) / 2;
            pictureBox3.Top = (panel2.Height - pictureBox3.Height) / 2;

            label29.Left = (panel2.Width - label29.Width) / 2;
            label29.Top = pictureBox3.Bottom + 5;
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

        private void RECEIVING_TV_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the node under the mouse pointer
                _selectedNode = RECEIVING_TV.GetNodeAt(e.X, e.Y);
                if (_selectedNode != null)
                {
                    RECEIVING_TV.SelectedNode = _selectedNode;

                    // Enable/disable menu items based on node type
                    bool isRoot = _selectedNode.Parent == null;
                    bool isCategory = _selectedNode.Text == "ACTIVE" || _selectedNode.Text == "BENCHED";
                    bool isSystemFolder = IsSystemFolder(_selectedNode);

                    treeViewContextMenu.Items[0].Enabled = !isRoot; // Add Folder
                    treeViewContextMenu.Items[2].Enabled = !isRoot && !isCategory && !isSystemFolder; // Rename
                    treeViewContextMenu.Items[3].Enabled = !isRoot && !isCategory && !isSystemFolder; // Delete
                }
            }
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
                foreach (string folderName in _systemFolders)
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
                    if (_systemFolders.Contains(folderName))
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
                    !_systemFolders.Contains(folderName)) // system folders always show
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

        private string GetFullPath(TreeNode node)
        {
            if (node.Parent == null) return node.Text;
            return Path.Combine(GetFullPath(node.Parent), node.Text);
        }

        private bool IsSystemFolder(TreeNode node)
        {
            // Check if this is one of the predefined system folders
            string[] systemFolders = _systemFolders;
            return systemFolders.Contains(node.Text);
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

        private string GetCurrentDirectory()
        {
            if (RECEIVING_TV.SelectedNode != null)
            {
                return RECEIVING_TV.SelectedNode.Tag?.ToString() ?? string.Empty;
            }
            return string.Empty;
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

        private void AddFolderItem_Click(object sender, EventArgs e)
        {
            if (_selectedNode == null) return;

            string parentPath = _selectedNode.Tag?.ToString();
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
                        _selectedNode.Nodes.Add(newNode);
                        _selectedNode.Expand();

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
            if (_selectedNode == null || _selectedNode.Parent == null) return;

            string currentPath = _selectedNode.Tag?.ToString();
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
                        _selectedNode.Text = newFolderName;
                        _selectedNode.Tag = newFolderPath;
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
            if (_selectedNode == null || _selectedNode.Parent == null) return;

            string folderPath = _selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(folderPath)) return;

            var result = MessageBox.Show($"Are you sure you want to delete the folder '{_selectedNode.Text}'?",
                                       "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete directory (recursively)
                    Directory.Delete(folderPath, true);

                    // Remove from TreeView
                    _selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void dgv_main_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Prevent default crash dialog
            e.ThrowException = false;

            Helpers.ShowDialogMessage("error", "Invalid numeric value. Please enter a valid amount.");
        }

        private void dgv_main_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            Helpers.HandleNumericColumns(dgv_main, e, new[] { "received_qty", "rejected_qty" });
        }

        private void dgv_main_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgv_main.Rows[e.RowIndex];
            string changedCol = dgv_main.Columns[e.ColumnIndex].Name;

            // Map qty columns to their respective uom columns
            var qtyToUomMap = new Dictionary<string, string>
            {
                { "received_qty", "received_uom" },
                { "rejected_qty", "rejected_uom" }
            };

            if (!qtyToUomMap.ContainsKey(changedCol)) return;

            string uomCol = qtyToUomMap[changedCol];
            var qtyValue = row.Cells[changedCol].Value;

            bool isZeroOrEmpty = qtyValue == null
                || qtyValue == DBNull.Value
                || string.IsNullOrWhiteSpace(qtyValue.ToString())
                || (decimal.TryParse(qtyValue.ToString(), out decimal parsed) && parsed == 0);

            if (isZeroOrEmpty)
            {
                row.Cells[uomCol].Value = null;
            }
            else
            {
                // Copy from ordered_uom
                var orderedUom = row.Cells["ordered_uom"].Value;
                row.Cells[uomCol].Value = orderedUom;
            }

            // Update reason_for_rejection readonly state whenever any cell changes
            if (_isEditing)
                UpdateRejectionReasonReadOnly();
        }
    }
}
