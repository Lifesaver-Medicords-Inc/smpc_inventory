using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Services.Setup.Warehouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
//using System.Globalization; //test for global currency change
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_receiving_report_setup : UserControl
    {
        public frm_receiving_report_setup()
        {
            InitializeComponent();
            dgChildList = new List<DataGridView> { dg_receiving_report_details, dg_receiving_report_inventory };
            dtChildlist = new List<DataTable> { receivingreportdetails, receivingreportinventory };
            refDocRecordRowIndex = 0;
        }
        //list of dg children 
        List<DataGridView> dgChildList;

        ReceivingReportList receivingReportContents;

        List<DataTable> dtChildlist;
        DataTable receivingreport; //parent 
        DataTable receivingreportdetails; //panganay
        DataTable receivingreportinventory; //ampon


        static int selectedRecord = 0; //currentIndex //invert this to make the highest shows first


        //Por-en-gers
        PurchaseOrdersWithDetails purchaseOrderWithDetailsRecords; //for po list
        DataTable purchaseorder; //foreign parent
        DataTable purchaseorderdetails; //foreign child

        WarehouseList warehouseList; //for warehouse
        DataTable warehousename; //foreign's parent
        DataTable warehouseaddress; //foreign's child

        WarehouseUseTypeModel warehouseUseType;
        DataTable warehouseusetype;

        static int refDocRecordRowIndex; //currentIndex //invert this to make the highest shows first nt


        private async void frm_receiving_report_setup_Load(object sender, EventArgs e)
        { 
            Cursor.Current = Cursors.WaitCursor;
            cmb_ref_doc.Items.Clear();
            isInitialLoad = true;
            SetCurrentUser();
            initializeDG();

            var response = await RequestToApi<ApiResponseModel<ReceivingReportList>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT);
            receivingReportContents = response.Data;
            if (!response.Success)
            {
                BtnToggleEnabillity("empty");
                Helpers.ShowDialogMessage("success", "No prior Receiving Report exists. Creating a new one now.");
                return;
            }
            receivingreport = JsonHelper.ToDataTable(receivingReportContents.receiving_report/*, "id = desc"*/);
            selectedRecord = receivingreport.Rows.Count - 1;
            receivingreport = null;

            await FetchLatestTables(); //updates table
            await GetData(); 
            await NextAndPrevBtn();
            //await populateRefDoc();

            Cursor.Current = Cursors.Default;
            btn_new.Enabled = true;
        }

        private async Task FetchLatestTables()
        {
            var receivingReportDataList = await RequestToApi<ApiResponseModel<ReceivingReportList>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT);
            receivingReportContents = receivingReportDataList.Data;//assigning list into variable

            var receivingReportContentsDataList = await RequestToApi<ApiResponseModel<ReceivingReportList>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT);
            receivingReportContents = receivingReportContentsDataList.Data;

            var warehouseDictionary = await WarehouseNameServices.GetWarehouseInfos();
            warehouseAreaTable = JsonHelper.ToDataTable(warehouseDictionary.warehouse_area);

            var purchaseOrderWithDetailsRecordsDataList = await RequestToApi<ApiResponseModel<PurchaseOrdersWithDetails>>.Get(ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);
            purchaseOrderWithDetailsRecords = purchaseOrderWithDetailsRecordsDataList.Data;

            TableContentChanged.WarehouseUseType = false;
            TableContentChanged.WarehouseName = false;
            isLocalContentChanged = false;
        }

        private ToolTip toolTip = new ToolTip();
        static bool isFetchingData = false;
        private async Task LoadReceivingReport()
        {
            cmb_ref_doc.Text = currentRefDoc;

            Cursor.Current = Cursors.WaitCursor;

            //panels
            Panel[] pnlHead = { pnl_head }; //parent pnl - header
            
            //to data table converter
            receivingreport = JsonHelper.ToDataTable(receivingReportContents.receiving_report/*, "id = desc"*/);

            Helpers.BindControls(pnlHead, receivingreport, selectedRecord); //parent [rr]

            //setting data that aint in db
            if (!txt_doc.Text.ToLower().Contains("rr#") && !string.IsNullOrEmpty(txt_doc.Text))
            {
                txt_doc.Text = "RR#" + txt_doc.Text;
            }
            if (!cmb_ref_doc.Text.ToLower().Contains("po#") && !string.IsNullOrEmpty(cmb_ref_doc.Text))
            {
                cmb_ref_doc.Text = "PO#" + cmb_ref_doc.Text;
            }

            if (string.IsNullOrEmpty(txt_date_received.Text))
            {
                txt_date_received.Text = DateTime.Now.ToString();
            }

            var address = receivingreport.Rows[selectedRecord]["address"].ToString();
            if (!string.IsNullOrEmpty(address))
            {
                cmb_address.Items.Clear();
                cmb_address.Items.Add(address);
                cmb_address.SelectedIndex = 0;
            }

            if (string.IsNullOrEmpty(txt_prepared_by.Text))
            {
                txt_prepared_by.Text = userCredential;
            }

            //set cmb width //as suggested by PM
            if (string.IsNullOrEmpty(cmb_address.Text)
                || string.IsNullOrEmpty(receivingreport.Rows[selectedRecord]["address"].ToString()))
            {
                cmb_address.Items.Clear();
                cmb_address.Width = 200;
            }

            var warehousename = receivingreport.Rows[selectedRecord]["warehouse_name"].ToString();
            if (!string.IsNullOrEmpty(warehousename))
            {
                cmb_warehouse_name.Items.Clear();
                cmb_warehouse_name.Items.Add(warehousename);
                cmb_warehouse_name.SelectedIndex = 0;
            }

            //set cmb width //as suggested by PM
            if (string.IsNullOrEmpty(cmb_warehouse_name.Text)
                || string.IsNullOrEmpty(receivingreport.Rows[selectedRecord]["warehouse_name"].ToString()))
            {
                cmb_warehouse_name.Items.Clear();
                cmb_warehouse_name.Width = 200;
            }
            else
            {
                AdjustComboBoxWidthToContent();
                AdjustComboBoxNameWidthToContent();
            }
        }
        
        private async Task GetData()
        {
            isFetchingData = true;

            if (TableContentChanged.WarehouseName
                || TableContentChanged.WarehouseUseType)
            {
                await FetchLatestTables();
            } 
            else if (isLocalContentChanged)
            {
                await FetchLatestTables();
            }

            //binders
            //parent
            await LoadReceivingReport();
            //childrens
            await FetchDG("dg_receiving_report_details"); //child main dg

            // TRYYY
            await FetchDG("dg_receiving_report_inventory"); //child inv dg
            //await FetchDGRRInventory(); //child inventory dg //lagay d2 if magffetch sa db ng path (not implemented yet)
            LoadExistingFiles(); //fetch attachments (locally saved only)
            
            cmb_ref_doc.DroppedDown = false;
            isFetchingData = false;
             
            BtnToggleEnabillity("initial");
            Cursor.Current = Cursors.Default;
        }

        static bool isPopulatingDG = false;
        private async Task FetchDG(string dgName = "")
        {
            isPopulatingDG = true;

            var dataViewDetails = new DataView();

            if (dgName == dgChildList[0].Name) //dg_rr_deets
            {
                receivingreportdetails = JsonHelper.ToDataTable(receivingReportContents.receiving_report_details/*, "id = desc"*/);
                dataViewDetails = new DataView(receivingreportdetails);
                bindingSource1.DataSource = null;
            }
            else if (dgName == dgChildList[1].Name) //dg_rr_inv
            {
                receivingreportinventory = JsonHelper.ToDataTable(receivingReportContents.receiving_report_inventory/*, "id = desc"*/);
                dataViewDetails = new DataView(receivingreportinventory);
                bindingSource2.DataSource = null;
            }
            else return;
            //idk if this is needed

            if (dataViewDetails.Count != 0 && receivingreport.Rows.Count > selectedRecord)
            {
                string selectedId = receivingreport.Rows[selectedRecord]["id"].ToString();
                dataViewDetails.RowFilter = $"receiving_report_id = '{selectedId}'";
            }

            if (dgName == dgChildList[0].Name)
            {
                bindingSource1.DataSource = dataViewDetails;
                dg_receiving_report_details.DataSource = bindingSource1;
            }
            else if (dgName == dgChildList[1].Name)
            {
                if (dataViewDetails.Count == 0)
                {
                    bindingSource2.DataSource = bindingSource1.DataSource;
                    dg_receiving_report_inventory.DataSource = bindingSource2;

                    foreach (DataGridViewRow row in dg_receiving_report_inventory.Rows)
                    {
                        if (!row.IsNewRow)
                            row.Cells["dg_receiving_report_inventory_id"].Value = 0;
                    }
                }
                else
                {
                    bindingSource2.DataSource = dataViewDetails;
                    dg_receiving_report_inventory.DataSource = bindingSource2;

                    var binColumn = dg_receiving_report_inventory
                        .Columns["dg_receiving_report_inventory_bin_location"]
                        as DataGridViewComboBoxColumn;

                    if (binColumn != null)
                    {
                        binColumn.Items.Clear();

                        binColumn.DataPropertyName = "bin_location";

                        var uniqueBinLocations = dataViewDetails
                            .Cast<DataRowView>()
                            .Select(r => r["bin_location"]?.ToString())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Distinct();

                        foreach (string bin in uniqueBinLocations)
                        {
                            binColumn.Items.Add(bin);
                        }
                    }

                    foreach (DataGridViewRow row1 in dg_receiving_report_inventory.Rows)
                    {
                        if (row1.IsNewRow) continue;

                        var cell = row1.Cells["dg_receiving_report_inventory_bin_location"];
                        var cellValue = cell?.Value?.ToString();

                        //if (!string.IsNullOrEmpty(cellValue) && colorDictionary.TryGetValue(cellValue, out Color color))
                        //{
                        //    cell.Style.BackColor = color;
                        //    cell.Style.ForeColor = color.GetBrightness() < 0.6f ? Color.White : Color.Black;
                        //}
                        foreach (DataRow row2 in warehouseusetype.Rows)
                        {
                            string useType = row2["name"]?.ToString().ToUpper();
                            string colorName = row2["bg_color"]?.ToString();

                            if (!string.IsNullOrWhiteSpace(useType) && !string.IsNullOrWhiteSpace(colorName))
                            {
                                Color color = Color.FromName(colorName);

                                if (!colorDictionary.ContainsKey(useType))
                                    colorDictionary[useType] = color;

                                if (cellValue.Contains(useType))
                                {
                                    cell.Style.BackColor = color;
                                    cell.Style.ForeColor = color.GetBrightness() < 0.5f ? Color.White : Color.Black;
                                } 
                            }
                        }
                    }
                }
            }
            isPopulatingDG = false;
        }

        private void LoadHeadByRefDoc()
        {
            if (isLoading()) return;
            purchaseOrderIndexer();

            Panel[] pnlHead = { pnl_head }; //parent pnl - header


            Helpers.BindControls(pnlHead, purchaseorder, refDocRecordRowIndex);

            txt_purchase_order_id.Text = purchaseorder.Rows[refDocRecordRowIndex]["id"].ToString();
            txt_date_received.Text = DateTime.Now.ToString();
            txt_doc.Text = "Auto Generated";
            txt_id.Text = currentRRID;
        }

        private void purchaseOrderIndexer() //filter to find the index for binding
        {
            purchaseorder = JsonHelper.ToDataTable(purchaseOrderWithDetailsRecords.purchaseorder);
            string targetDocNo = cmb_ref_doc.Text;

            for (int i = 0; i < purchaseorder.Rows.Count; i++)
            {
                if (purchaseorder.Rows[i]["doc_no"].ToString().TrimStart('0') == targetDocNo.TrimStart('0'))
                {
                    refDocRecordRowIndex = i;
                    break;
                }
            }
        }

        private void LoadDGPOID(string currentDV = "")
        {
            //string toSearchPODocNo = cmb_ref_doc.Text.ToString();
            purchaseorder = JsonHelper.ToDataTable(purchaseOrderWithDetailsRecords.purchaseorder);
            purchaseorderdetails = JsonHelper.ToDataTable(purchaseOrderWithDetailsRecords.purchaseorderdetails);
            DataView purchaseOrderDetails = new DataView(purchaseorderdetails);
            //bindingSource1.DataSource = null;
            dg_receiving_report_details.DataSource = null;
            dg_receiving_report_inventory.DataSource = null;

            //cmb_ref_doc.Text 

            if (purchaseOrderDetails.Count != 0 && purchaseorder.Rows.Count > selectedRecord)
            {                 
                string selectedId = purchaseorder.Rows[refDocRecordRowIndex]["id"].ToString();
                purchaseOrderDetails.RowFilter = $"based_id = '{selectedId}'"; 

                bindingSource1.DataSource = purchaseOrderDetails; //not actually needed
            }

            dg_receiving_report_details.Rows.Clear();
            dg_receiving_report_inventory.Rows.Clear();
            //set read only to item code,desc, and purchased qty and uom
            //dataGridSpecificReadOnly(true);

            Console.WriteLine("\ncmb_ref_doc.SelectedItem: " + cmb_ref_doc.SelectedItem);
            for (int i = 0; i < purchaseOrderDetails.Count; i++)
            {
                DataRowView rowView = purchaseOrderDetails[i];
                dg_receiving_report_details.Rows.Add();
                purchaseOrderDetails[i]["id"] = DBNull.Value;

                string[] colSuffixes = new[]
                { //cols that has to be same with the referenced
                    "item_code",
                    "item_description",
                    "ordered_qty",
                    "received_uom",
                    "rejected_uom",
                    "ordered_uom",
                    "receiving_report_id",
                    "ref_id"
                };

                foreach (var grid in dgChildList)
                {
                    foreach (var suffix in colSuffixes)
                    {
                        string colName = grid.Name + "_" + suffix;

                        if (!grid.Columns.Contains(colName))
                        {
                            Console.WriteLine($"{grid.Name} doesn't have column: {colName}");
                            continue;
                        }

                        //checks if 'i' is a valid index for this grid:
                        if (i < 0 || i >= grid.Rows.Count)
                        {
                            Console.WriteLine($"Row index {i} is out of range for grid {grid.Name}");
                            continue;
                        }

                        var row = grid.Rows[i];

                        //skip the new row placeholder (the lower row)
                        if (row.IsNewRow)
                        {
                            Console.WriteLine($"Row {i} in grid {grid.Name} is the new row placeholder. Skipping.");
                            continue;
                        }

                        object value = null;

                        if (suffix == "item_code")
                        {
                            value = rowView["item_code"].ToString();
                        }
                        else if (suffix == "item_description")
                        {
                            value = !string.IsNullOrEmpty(rowView["item_description"].ToString())
                                ? rowView["item_description"]
                                : "ITEM NAME: " + rowView["item_name"].ToString();
                        }
                        else if (suffix == "ordered_qty")
                        {
                            value = rowView["order_qty"];
                        }
                        else if (suffix == "received_uom" || suffix == "rejected_uom" || suffix == "ordered_uom")
                        {
                            value = rowView["unit_of_measure"];
                        }
                        else if (suffix == "receiving_report_id")
                        {
                            value = txt_id.Text;
                        }
                        else if (suffix == "ref_id")
                        {
                            value = txt_purchase_order_id.Text;
                        }

                        grid.Rows[i].Cells[colName].Value = value;
                        Console.WriteLine(grid.Name + " loaded");
                    }
                }
            }
        }

        //set user for easier access
        private void SetCurrentUser()
        {
            string firstName = CacheData.CurrentUser.first_name.ToString();
            string lastName = CacheData.CurrentUser.last_name.ToString();
            if ((lastName + " " + firstName).Length > 20)
            {
                firstName = firstName.Length > 12 ? firstName.Split()[0] : firstName;
                lastName = lastName.Length > 12 ? lastName.Split()[0] : lastName;
            }
            string userFullName = firstName + " " + lastName;
            string userPosition = CacheData.CurrentUser.position_id;
            userCredential = userFullName + " | " + userPosition;
        }
        static string userCredential = "";

        private void BtnToggleEnabillity(string action = "initial", bool initial = false) //simplify this AH
        {
            action = action.ToLower();
            btn_search.Enabled = true;
            btn_delete.Enabled = true;
            btn_upload.Enabled = true;

            if (action == "initial")
            {
                btn_new.Visible = true;
                btn_edit.Visible = true;
                btn_delete.Visible = true;
                btn_search.Visible = true;

                btn_setTime.Enabled = false;
                btn_setTime.Visible = false;
                btn_cancel.Visible = false;
                btn_save.Visible = false;

                Helpers.SetPanelToReadOnly(pnl_head, true);
                Helpers.SetPanelToReadOnly(pnl_main, true);
                Helpers.SetPanelToReadOnly(pnl_inventory, true);
                Console.WriteLine(pnl_head.Parent);
                Helpers.SetPanelToReadOnly(pnl_attachment, true);

                NextAndPrevBtn();
            }
            else if (action == "edit" || action == "new")
            {
                Helpers.SetPanelToReadOnly(pnl_head, false);
                Helpers.SetPanelToReadOnly(pnl_main, false);
                Helpers.SetPanelToReadOnly(pnl_inventory, false);
                Helpers.SetPanelToReadOnly(pnl_attachment, false);

                btn_cancel.Visible = true;
                btn_save.Visible = true;

                btn_delete.Visible = false;
                btn_search.Visible = false;

                btn_setTime.Enabled = true;
                btn_setTime.Visible = true;

                btn_next.Enabled = false;
                btn_prev.Enabled = false;

                txt_doc.ReadOnly = true;
                txt_supplier_code.ReadOnly = true;
                txt_supplier_name.ReadOnly = true;

                if (!string.IsNullOrEmpty(cmb_ref_doc.Text))
                {
                    dataGridSpecificReadOnly(true);
                }
            }

            if (action == "edit")
            { 
                btn_new.Visible = false; 
                btn_save.Text = "&Save";
                btn_save.ToolTipText = "Save";
                dg_receiving_report_details.AllowUserToAddRows = false;
                dg_receiving_report_inventory.AllowUserToAddRows = false;
                dataGridSpecificReadOnly(true);
            }
            else if (action == "new")
            { 
                btn_save.Text = "Create";
                btn_save.ToolTipText = "Create";
                btn_edit.Visible = false;
                btn_upload.Enabled = false;
                dg_receiving_report_details.AllowUserToAddRows = false;
                dg_receiving_report_inventory.AllowUserToAddRows = false;

                dataGridSpecificReadOnly(true);

                if (dg_receiving_report_details.DataSource != null)
                {
                    dg_receiving_report_details.DataSource = null;
                }
                else
                {
                    dg_receiving_report_details.Rows.Clear();
                }

                if (dg_receiving_report_inventory.DataSource != null)
                {
                    dg_receiving_report_inventory.DataSource = null;
                }
                else
                {
                    dg_receiving_report_inventory.Rows.Clear();
                }
            }

            if (action == "save" || action == "cancel")
            {
                Helpers.SetPanelToReadOnly(pnl_head, true);
                Helpers.SetPanelToReadOnly(pnl_main, true);
                Helpers.SetPanelToReadOnly(pnl_inventory, true);
                Helpers.SetPanelToReadOnly(pnl_attachment, true);

                btn_delete.Visible = true;
                btn_delete.Enabled = true;
                btn_edit.Visible = true;
                btn_edit.Enabled = true;
                btn_new.Visible = true;
                btn_new.Enabled = true;
                btn_search.Visible = true;
                btn_search.Enabled = true;

                btn_cancel.Visible = false;
                btn_save.Visible = false;
                btn_setTime.Visible = false;

                dataGridSpecificReadOnly(true);

                NextAndPrevBtn();

                cmb_ref_doc.DroppedDown = false;
                return;
            }
            else if (action == "empty") //filter this for empty rows in dt
            {
                btn_new.Visible = true;
                btn_new.Enabled = true;
                btn_delete.Visible = false;
                btn_edit.Visible = false;
                btn_search.Visible = false;

                Helpers.SetPanelToReadOnly(pnl_head, false);
                Helpers.SetPanelToReadOnly(pnl_main, false);
                Helpers.SetPanelToReadOnly(pnl_inventory, false);
                Helpers.SetPanelToReadOnly(pnl_attachment, false);

                btn_setTime.Enabled = true;
                btn_setTime.Visible = true;

                btn_save.Visible = true;
                btn_cancel.Visible = false;

                return;
            }

            pnl_head.Enabled = true;
            pnl_main.Enabled = true;
        }
        private void dataGridSpecificReadOnly(bool isReadOnly)
        {
            //List of column names to set readonly
            var columnNames = new List<string>
            {
                "item_code",
                "item_description",
                "ordered_qty",
                "ordered_uom"
            };

            foreach (var dgv in dgChildList)
            {
                foreach (var colName in columnNames)
                {
                    if (dgv.Columns.Contains(dgv.Name + "_" + colName))
                    {
                        dgv.Columns[dgv.Name + "_" + colName].ReadOnly = isReadOnly;
                    }
                }
            }
        }

        Dictionary<string, dynamic> ReceivingReport;
        private void SaveDG(string JsonHeadName)
        {
            DataGridView datagridChild = null;
            if (JsonHeadName.Equals("receiving_report_details")) datagridChild = dg_receiving_report_details;
            else if (JsonHeadName.Equals("receiving_report_inventory")) datagridChild = dg_receiving_report_inventory;
            else return; //catch no child value

            //this changes the naming of the datagrid (fix)
            //datagridChild.DataSource = null;
            var dgReceivingReportDetails = Helpers.ConvertDataGridViewToDataTable(datagridChild, JsonHeadName); //converts DG to DT

            var dgDictionary = Helpers.ConvertDataTableToDictionary(dgReceivingReportDetails, JsonHeadName); //converts DT to Dictionary
            string parentJson = "receiving_report";

            //dgDictionary.Remove("receiving_report_id");
            //ApiResponseModel response;
            //if (dgDictionary.TryGetValue("receiving_report_details", out var detailsList)) //wtf sa newer ver. lng toh

            List<Dictionary<string, dynamic>> detailsList;
            if (dgDictionary.TryGetValue(JsonHeadName, out detailsList))
            {
                bool isNotEmpty = false;
                foreach (var dict in detailsList)
                {
                    if (!string.IsNullOrEmpty(dict.ToString()))
                    {
                        isNotEmpty = true;
                    }
                }
                if (isNotEmpty)
                { 
                    foreach (var dict in detailsList)
                    { 
                        if (dict.ContainsKey(parentJson + "_id") && string.IsNullOrEmpty(dict[parentJson + "_id"]?.ToString()))
                        {
                            if (!dict.ContainsKey(JsonHeadName + "_ref_id") || string.IsNullOrEmpty(dict[JsonHeadName + "_ref_id"]?.ToString()))
                            {
                                if (int.TryParse(txt_purchase_order_id.Text, out int txtPOIDValue))
                                {
                                    dict["ref_id"] = txtPOIDValue; //setting purchase order id
                                }
                            }
                        }
                        else
                        {
                            if (int.TryParse(txt_id.Text, out int txtIdValue))
                            {
                                dict[parentJson + "_id"] = txtIdValue; //setting parent id
                            }
                        } 
                    }
                }
            }

            //ReceivingReport[JsonHeadName] = dgDictionary;
            ReceivingReport[JsonHeadName] = detailsList;
        }


        //cmb reference document number (purchase order's doc no.) RefDoc
        private static string currentRefDoc = "";
        private static string currentRRID = "";
        static bool isPopulatingRefDoc = false; //for changes event trigger stopper
        static bool isComboBoxInitialClick = false; //not used event stopper
        static string previousRefDoc = "no prev ref doc"; 
        private async Task populateRefDoc() 
        {
            isPopulatingRefDoc = true;

            try
            {
                //Cursor.Current = Cursors.WaitCursor;

                //put this out and it'll make a leaving value error
                var response = await RequestToApi<ApiResponseModel<PurchaseOrdersWithDetails>>.Get(ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);
                purchaseOrderWithDetailsRecords = response.Data;

                string currentText = cmb_ref_doc.Text;
                int caretPos = cmb_ref_doc.SelectionStart;

                cmb_ref_doc.Items.Clear();

                if (purchaseOrderWithDetailsRecords != null &&
                    purchaseOrderWithDetailsRecords.purchaseorder != null)
                {
                    string searchText = cmb_ref_doc.Text.Trim().ToLower();

                    var filteredPOs = purchaseOrderWithDetailsRecords.purchaseorder
                        .Where(po => po.doc_no.ToLower().Contains(searchText))
                        .Select(po => po.doc_no)
                        .ToList();

                    if (filteredPOs.Count > 0)
                    {
                        foreach (var docNo in filteredPOs)
                        {
                            cmb_ref_doc.Items.Add(docNo);
                        }
                    }
                    else
                    {
                        cmb_ref_doc.Items.Add("No Match");
                    }
                }
                else
                {
                    cmb_ref_doc.Items.Add("Null or Empty PO / PO's Doc No."); //to handle null exceptions
                }

                //cmb_ref_doc.Text = currentText;
                cmb_ref_doc.SelectionStart = caretPos;
                //cmb_ref_doc.SelectionLength = 0;
            }
            catch (Exception e) 
            {
                Console.WriteLine("\n\nERROR: \n" + e.StackTrace + "\n" + e.ToString());
            }
            finally
            {
                isPopulatingRefDoc = false;
            }
        }
        private async void cmb_ref_doc_Enter(object sender, EventArgs e)
        {
            currentRRID = txt_id.Text;
            DialogResult result = MessageBox.Show(
                "Are you sure you want to change REFERENCE DOC? \nChanging this will overwrite other fields.",
                "Change Reference Document Number",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
                );
            if (result == DialogResult.Yes)
            {
                //previousRefDoc = cmb_ref_doc.Text;
                previousRefDoc = string.IsNullOrEmpty(previousRefDoc) 
                    ? cmb_ref_doc.Text.Replace("PO#", "") 
                    : previousRefDoc.Replace("PO#", "");
                cmb_ref_doc.Text = cmb_ref_doc.Text.Replace("PO#", "");
                await populateRefDoc();
                cmb_ref_doc.DroppedDown = true;
            }
            else
            {
                this.ActiveControl = txt_id; //focus to somethin else
            }
        }
        private async void cmb_ref_doc_TextChanged(object sender, EventArgs e)
        {
            if (isLoading()) return; 

            await populateRefDoc();

            if (cmb_ref_doc.Items.Count > 0 &&
                !(cmb_ref_doc.Text.ToString().Equals("No Match") ||
                cmb_ref_doc.Text.ToString().Contains("PO#") ||
                string.IsNullOrEmpty(cmb_ref_doc.Text)))
            {
                cmb_ref_doc.DroppedDown = true;
            }
        }
        private void cmb_ref_doc_Leave(object sender, EventArgs e)
        {
            isPopulatingRefDoc = false;
            if (cmb_ref_doc.Items.Contains("No Match") 
                || cmb_ref_doc.Items.Count == 0 
                || cmb_ref_doc.Text.Contains("PO#") 
                && !string.IsNullOrWhiteSpace(cmb_ref_doc.Text))
            {
                //cmb_ref_doc.Text = previousRefDoc;
                Helpers.ShowDialogMessage("error", 
                    "There is no matching Purchase Order Doc No." 
                    + (cmb_ref_doc.Text.Contains("PO#") 
                        ? ", remove 'PO#' when filtering" 
                        : cmb_ref_doc.Text.Replace("No Match","")));
            }

            bool isRefDocDuplicate = false;
            foreach (DataRow row in receivingreport.Rows)
            {
                if (row != null && row["ref_doc"] != DBNull.Value)
                {
                    if (row["ref_doc"].ToString().Replace("PO#", "") == cmb_ref_doc.Text
                        && !string.IsNullOrEmpty(cmb_ref_doc.Text))
                    {
                        isRefDocDuplicate = true;
                        break; 
                    }
                }
            }

            if (isRefDocDuplicate)
            {
                cmb_ref_doc.Text = string.Empty;
                Helpers.ShowDialogMessage("warning", "The selected reference document has already been created. Please choose a different document number to reference, or delete the report with the same reference document");
            }
            else if (cmb_ref_doc.Text != currentRefDoc)
            {
                LoadHeadByRefDoc(); //binding based on ref doc 
                LoadDGPOID();
            }


            if (!string.IsNullOrEmpty(cmb_ref_doc.Text) && !cmb_ref_doc.Text.ToString().Equals("No Match"))
            {
                if (cmb_ref_doc.Text.Count() < 4)
                { 
                    cmb_ref_doc.Text = cmb_ref_doc.Text.PadLeft(4, '0'); 
                }
                cmb_ref_doc.Text = "PO#" + cmb_ref_doc.Text.Replace("PO#",""); //redouble check if theres a po# in doc
            }
            cmb_ref_doc.DroppedDown = false;
        }
        private void cmb_ref_doc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
                this.ActiveControl = txt_id; 
                if (cmb_ref_doc.Text.Equals("No Match")
                    || string.IsNullOrEmpty(cmb_ref_doc.Text))
                {
                    cmb_ref_doc.Text = previousRefDoc;
                }
                else cmb_ref_doc.SelectedIndex = 0;

                if (cmb_ref_doc.Text != currentRefDoc)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true); 
                } 
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                cmb_ref_doc.SelectedIndex = 0;
                cmb_ref_doc.DroppedDown = false;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }
        private void cmb_ref_doc_MouseClick(object sender, MouseEventArgs e)
        {
            if (cmb_ref_doc.Text.Equals("No Match")) cmb_ref_doc.Text = ""; //remove this
            else currentRefDoc = cmb_ref_doc.Text;
            isComboBoxInitialClick = true;
            //await populateRefDocAsync();
            //cmb_ref_doc.Text = currentRefDoc;
            isComboBoxInitialClick = false; 
        }
        private void cmb_ref_doc_DropDown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default; //fixes cursor visibility issue
        }
        private void cmb_ref_doc_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ref_doc.DroppedDown = false;
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }
        
        //btns
        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_head);
            BtnToggleEnabillity("new");
            btn_new.Visible = false;

            lst_receiving_report_attachment_files.Items.Clear();
            dg_receiving_report_details.DataSource = null;
            dg_receiving_report_inventory.DataSource = null;
            cmb_ref_doc.Text = "";
            previousRefDoc = string.Empty;
            txt_doc.Text = "Auto Generated Field";
            txt_prepared_by.Text = userCredential;
            //clear entries
        }
        private void btn_setTime_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_date_received.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to overwrite the \n'Date Received' [" + txt_date_received.Text + "] " +
                    "to \ncurrent time [" + DateTime.Now.ToString() + "]?",
                    "Overwritting Time",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );
                if (result != DialogResult.Yes) return;
            }

            txt_date_received.Text = DateTime.Now.ToString();
        }
        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggleEnabillity("edit");
            btn_edit.Visible = false;
        }
        private async void btn_cancel_Click(object sender, EventArgs e)
        {
            isPopulatingRefDoc = false; //i forgot why I set this 
            BtnToggleEnabillity("cancel");
            await GetData();
        }
          
        private async Task<ApiResponseModel> CreateNewReceivingReport(Dictionary<string, dynamic> receivingReportData)
        {
            receivingReportData.Remove("id");
            receivingReportData.Remove("doc");

            //parent
            ReceivingReport["receiving_report"] = receivingReportData;

            //adds child to the dictionary of dictionary 
            SaveDG("receiving_report_details");    //child 1 rrdeets
            SaveDG("receiving_report_inventory");  //child 2 inventory ah
                                                   //saveDG("receiving_report_attachment");  //favorite child attachment

            var response = await ReceivingReportServices.Insert(ReceivingReport);

            if (receivingreport != null && receivingreport.Rows.Count == 0)
            {
                selectedRecord = receivingreport.Rows.Count; 
            }
            else if (response.Success)
            {
                selectedRecord = receivingreport.Rows.Count;
                CreateAttachmentFolder(receivingReportData);
            }

            ShowSaveMessage(response, true);
            return response;
        }

        private async Task<ApiResponseModel> UpdateReceivingReport(Dictionary<string, dynamic> receivingReportData)
        {
            var docValue = receivingReportData["doc"] as string;

            if (!string.IsNullOrEmpty(docValue))
            {
                docValue = docValue.Replace("PO#", "").Trim(); //anti lag

                if (int.TryParse(docValue, out int parsedDoc))
                {
                    receivingReportData["doc"].Value = parsedDoc.ToString();
                }
                else if (docValue.ToLower().Contains("auto"))
                {  //remove if cant parse (string na save)
                    receivingReportData.Remove("doc");
                }
            }

            //parent
            ReceivingReport["receiving_report"] = receivingReportData;

            //adds child to the dictionary of dictionary 
            SaveDG("receiving_report_details");    //child 1 rrdeets
            SaveDG("receiving_report_inventory");  //child 2 inventory ah
                                                   //saveDG("receiving_report_attachment");  //favorite child attachment

            var response = await ReceivingReportServices.Update(ReceivingReport);

            if (response.Success)
            {
                CreateAttachmentFolder(receivingReportData);
            }

            ShowSaveMessage(response, false);
            return response;
        }

        private void CreateAttachmentFolder(Dictionary<string, dynamic> receivingReportData)
        {
            if (receivingReportData.TryGetValue("doc", out var docObj) && docObj != null)
            {
                string docValue = docObj.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(docValue))
                {
                    //create the path
                    string baseFolder = Path.GetFullPath(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        @"..\..\Resources\SharedFiles\ReceivingReportAttachments"));

                    string targetFolder = Path.Combine(baseFolder, docValue);

                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);
                }
            }
        }

        private void ShowSaveMessage(ApiResponseModel response, bool isNewRecord)
        {
            string currDoc = txt_doc.Text;
            string currRefDoc = cmb_ref_doc.Text;

            string message = response.Success
                ? (isNewRecord
                    ? $"PO#{currRefDoc} Added Successfully"
                    : $"{currDoc} Updated Successfully")
                : (isNewRecord
                    ? $"Failed To Add PO#{currRefDoc}\n{response.message}"
                    : $"Failed To Update {currDoc}\n{response.message}");

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);

            Console.WriteLine("\nRESPONSE FOR PROCESS: " + (isNewRecord ? "INSERT" : "UPDATE") + " IS: " + response.Success + " | " + message);
        }

        private async void btn_save_Click(object sender, EventArgs e) //for head / (parent if children is also thrown)
        {
            if (string.IsNullOrEmpty(txt_supplier_name.Text)
                && string.IsNullOrEmpty(txt_supplier_code.Text)
                && string.IsNullOrEmpty(cmb_address.Text)
                && string.IsNullOrEmpty(cmb_warehouse_name.Text)
                && string.IsNullOrEmpty(txt_supplier_id.Text)
                && string.IsNullOrEmpty(cmb_ref_doc.Text)
                && string.IsNullOrEmpty(txt_id.Text)
                && string.IsNullOrEmpty(txt_purchase_order_id.Text)
                && txt_doc.Text == "Auto Generated Field")
            {
                Helpers.ShowDialogMessage("warning", "Fill up at least a single field in the header");
                return;
            }

            foreach (var dg in dgChildList)
            {
                dg.EndEdit(); //to force save current typed in the cell (bad quirk of dgs)
            }

            previousRefDoc = "No Match";

            DeleteSpecificRow(); //commit prev deleted rows

            // Build receiving report data
            ReceivingReport = new Dictionary<string, dynamic>();
            var receivingReportData = Helpers.GetControlsValues(pnl_head);

            if (int.TryParse(txt_supplier_id.Text, out int supplier_id))
            {
                receivingReportData["supplier_id"] = supplier_id;
            }
            else
            {
                receivingReportData.Remove("supplier_id");
            }

            if (cmb_ref_doc.Text.Contains("PO#"))  //remove PO#
                cmb_ref_doc.Text = cmb_ref_doc.Text.Replace("PO#", "");

            if (int.TryParse(txt_purchase_order_id.Text, out int purchase_order_id))
            {
                receivingReportData["purchase_order_id"] = purchase_order_id;
            }
            else
            {
                receivingReportData.Remove("purchase_order_id");
            }

            

            //if (string.IsNullOrWhiteSpace(txt_id.Text))
            //    await CreateNewReceivingReport(receivingReportData);
            //else
            //    await UpdateReceivingReport(receivingReportData);

            var response = await (string.IsNullOrWhiteSpace(txt_id.Text)
            ? CreateNewReceivingReport(receivingReportData)
            : UpdateReceivingReport(receivingReportData));

            BtnToggleEnabillity("save");

            if (response.Success)
            {
                TableContentChanged.ReceivingReport = true;
                isLocalContentChanged = true;
            }

            await NextAndPrevBtn();
            await GetData();
        }

        static bool isLocalContentChanged = false;

        private void DeleteSpecificRow()
        {
            if (inventoryDeletedRowIds.Count > 0)
            {
                commitRowDeletion();
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            btn_delete.Enabled = false;

            string currentDoc = txt_doc.Text;

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this " + currentDoc + " Receiving Report?",
                "Deletion Of Receipt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                DeleteReport(currentDoc);
            }

            BtnToggleEnabillity("initial");
        }
        private async void DeleteReport(string currentDoc)
        {
            Dictionary<string, dynamic> ReceivingReport = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> id = new Dictionary<string, dynamic>
                {
                    { "id", int.Parse(txt_id.Text) }
                };

            ReceivingReport["receiving_report"] = id;

            bool isSuccess = await ReceivingReportServices.Delete(ReceivingReport);

            if (isSuccess)
            {
                if (selectedRecord < 0 || receivingreport == null || receivingreport.Rows.Count < selectedRecord)
                {
                    BtnToggleEnabillity("empty");
                }
                selectedRecord--; 

                DeleteFolder(currentDoc);

                TableContentChanged.ReceivingReport = true;
                isLocalContentChanged = true;
                await GetData();
                await NextAndPrevBtn("prev");
            }
            else
            {
                Helpers.ShowDialogMessage("error", "Failed to delete " + currentDoc);
            }
        }
        private void DeleteFolder(string folderName)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = Path.Combine(baseDir, @"..\..\Resources\SharedFiles\ReceivingReportAttachments", folderName);

                if (Directory.Exists(folderPath))
                {
                    DialogResult result = MessageBox.Show(
                        folderName + " was deleted successfully.\n" +
                        $"Do you want to archive the attachments folder '{folderName}'?",
                        "SMPC SOFTWARE",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        string originalFolderPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\Resources\SharedFiles\ReceivingReportAttachments", folderName));
                        string archivedFolderName = folderName + " (archive)";
                        string archivedFolderPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\Resources\SharedFiles\ReceivingReportAttachments", archivedFolderName));

                        if (Directory.Exists(archivedFolderPath))
                        {
                            Helpers.ShowDialogMessage("error", $"An archived folder named '{archivedFolderName}' already exists.");
                            return;
                        }

                        Directory.Move(originalFolderPath, archivedFolderPath);

                        Helpers.ShowDialogMessage("success", $"Folder '{archivedFolderPath}' was archived successfully.");
                    }
                    else
                    {
                        Directory.Delete(folderPath, true);
                        Helpers.ShowDialogMessage("success", "Folder '" + baseDir + @"..\..\Resources\SharedFiles\ReceivingReportAttachments\" + folderName + "' was deleted successfully.");
                    }
                }
                else
                {
                    Helpers.ShowDialogMessage("success", folderName + " deleted successfully " +
                        $"\nHowever, the folder '{folderName}' does not exist, so it cannot be archived.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting the folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_prev_Click(object sender, EventArgs e)
        {
            await NextAndPrevBtn("prev");
            await GetData();
        }
        private async void btn_next_Click(object sender, EventArgs e)
        {
            await NextAndPrevBtn("next");
            await GetData();
        }
        private async Task NextAndPrevBtn(string goIndex = "") //increment and decrement was reversed to show latest
        {
            Cursor.Current = Cursors.WaitCursor;
            while (isLoading()) //can make stupid infinite loop so check this one
            {
                //Console.WriteLine("\n\nWTH THIS IS TRIGGERING??!\n\n");
                await Task.Delay(5);
            }
            if (string.IsNullOrEmpty(txt_id.Text)) return; //I forgot why I need this
            int lastIndex = receivingreport.Rows.Count - 1;

            if (!string.IsNullOrEmpty(goIndex))
            {
                if (goIndex.Equals("next") && selectedRecord > 0)
                    selectedRecord--;
                else if (goIndex.Equals("prev") && selectedRecord < lastIndex)
                    selectedRecord++;
            }

            if (receivingreport.Rows.Count != 0)
            {
                //out of bound checker
                if (selectedRecord > lastIndex)
                {
                    selectedRecord = lastIndex;
                }
                if (selectedRecord < 0)
                {
                    selectedRecord = 0;
                }

                btn_prev.Enabled = selectedRecord < lastIndex;
                btn_next.Enabled = selectedRecord > 0;
            }

            //GetData();
            Cursor.Current = Cursors.Default;
        }

        //important event stopper
        private bool isLoading()
        {
            Console.WriteLine("\nisLoading sleep is triggered\nValue: " +
                ("\tisFetchingData: " + isFetchingData
                + "\tisPopulatingRefDoc: " + isPopulatingRefDoc
                + "\tisPopulatingDG: " + isPopulatingDG
                + "\tisPopulatingBinLocation: " + isPopulatingBinLocation
                + "\tisComboBoxInitialClick: " + isComboBoxInitialClick
                + "\tisPainting: " + isPainting));

            return (isFetchingData 
                || isPopulatingRefDoc
                || isPopulatingDG
                || isPopulatingBinLocation
                || isComboBoxInitialClick
                || isPainting);
        }

        //cmb_warehouse_name
        private void cmb_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_warehouse_name.SelectedItem != null)
            {
                string selectedValue = cmb_warehouse_name.SelectedItem.ToString();
                toolTip.SetToolTip(cmb_warehouse_name, selectedValue);
            }
        }

        private void cmb_name_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (isLoading() || !isNotCurrentlyViewing()) return;

            // Find the selected warehouse by name
            var selectedWarehouse = warehouseList.warehouse_name
                .FirstOrDefault(wh => wh.name == cmb_warehouse_name.Text);

            if (selectedWarehouse != null)
            {
                // Get all addresses under this warehouse
                var matchingAddresses = warehouseList.warehouse_address
                    .Where(addr => addr.warehouse_name_id == selectedWarehouse.id)
                    .ToList();

                cmb_address.Items.Clear();
                foreach (var addr in matchingAddresses)
                {
                    string fullAddress = $"{addr.building_no} {addr.street} {addr.barangay_no} {addr.city} {addr.zip_code} {addr.country}";
                    cmb_address.Items.Add(fullAddress);
                }

                if (cmb_address.Items.Count > 0)
                {
                    cmb_address.SelectedIndex = 0; // auto-select first address
                }
            }
        }

        private async void PopulateWarehouseName()
        {
            var records = await RequestToApi<ApiResponseModel<WarehouseList>>.Get(ENUM_ENDPOINT.WAREHOUSE);
            warehouseList = records.Data;

            var warehouseNames = warehouseList.warehouse_name;

            if (warehouseNames != null && warehouseNames.Count > 0)
            {
                string currentText = cmb_warehouse_name.Text; // preserve current selection

                cmb_warehouse_name.Items.Clear();
                foreach (var wh in warehouseNames)
                {
                    if (wh.is_inactive.HasValue && wh.is_inactive.Value == true)
                    {
                        cmb_warehouse_name.Items.Add(wh.name);
                    }
                }

                // restore previous selection if still valid
                if (!string.IsNullOrEmpty(currentText) && cmb_warehouse_name.Items.Contains(currentText))
                {
                    cmb_warehouse_name.Text = currentText;
                }
                else
                {
                    cmb_warehouse_name.SelectedIndex = -1; // no selection
                }
            }
        }

        private void cmb_name_MouseHover(object sender, EventArgs e)
        {
            if (cmb_warehouse_name.SelectedItem != null)
            {
                string selectedValue = cmb_warehouse_name.SelectedItem.ToString();
                toolTip.SetToolTip(cmb_warehouse_name, selectedValue);
            }
        }

        private void cmb_name_DropDown(object sender, EventArgs e)
        {
            if (isNotCurrentlyViewing())
            {
                PopulateWarehouseName();
            }
            else
            {  //commented as for no copy button is needed
                //btn_copyAddress.Visible = true; 
                //btn_copyAddress.Enabled = true;
            }
        }

        private void cmb_name_DropDownClosed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmb_warehouse_name.Text) && isNotCurrentlyViewing())
            {
                //btn_copyAddress.Visible = false; //commented as for no copy button
                //btn_copyAddress.Enabled = false;
                AdjustComboBoxNameWidthToContent();
            }
        }

        private void AdjustComboBoxNameWidthToContent()
        {
            ComboBox cmb = cmb_warehouse_name;
            if (cmb.SelectedItem == null) return;

            using (Graphics g = cmb.CreateGraphics())
            {
                string text = cmb.GetItemText(cmb.SelectedItem);
                int width = (int)g.MeasureString(text, cmb.Font).Width + SystemInformation.VerticalScrollBarWidth - 10;
                cmb.Width = Math.Max(width, 200); // 50 is general min width
            }
        }

        //cmb_address
        static string concatedWarehouseAddress = "";
        private void cmb_address_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_address.SelectedItem != null)
            {
                string selectedValue = cmb_address.SelectedItem.ToString();
                toolTip.SetToolTip(cmb_address, selectedValue);
            }
        }
        private void cmb_address_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (isLoading() || !isNotCurrentlyViewing()) return;

            // Match back the WarehouseAddressModel based on the selected address text
            var selectedAddress = warehouseList.warehouse_address
                .FirstOrDefault(addr =>
                    $"{addr.building_no} {addr.street} {addr.barangay_no} {addr.city} {addr.zip_code} {addr.country}"
                    == cmb_address.Text);

            if (selectedAddress != null)
            {
                // Find the parent warehouse by warehouse_name_id
                var matchingWarehouse = warehouseList.warehouse_name
                    .FirstOrDefault(wh => wh.id == selectedAddress.warehouse_name_id);

                if (matchingWarehouse != null)
                {
                    cmb_warehouse_name.SelectedItem = matchingWarehouse.name;
                }
            }
        }

        private async void PopulateAddress()
        {
            var records = await RequestToApi<ApiResponseModel<WarehouseList>>.Get(ENUM_ENDPOINT.WAREHOUSE);
            warehouseList = records.Data;

            cmb_address.Items.Clear();

            if (warehouseList?.warehouse_address != null && warehouseList?.warehouse_name != null)
            {
                foreach (var addr in warehouseList.warehouse_address)
                {
                    // find related warehouse
                    var wh = warehouseList.warehouse_name
                        .FirstOrDefault(x => x.id == addr.warehouse_name_id);

                    // include only if warehouse exists and is_inactive == true (1 in DB)
                    if (wh != null && wh.is_inactive.HasValue && wh.is_inactive.Value)
                    {
                        // Build full address
                        var parts = new List<string>
                {
                    addr.building_no,
                    addr.street,
                    addr.barangay_no,
                    addr.city,
                    addr.zip_code,
                    addr.country
                };

                        string fullAddress = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        // Add to combo box with id reference
                        cmb_address.Items.Add(new ComboBoxItem
                        {
                            Text = fullAddress,
                            Value = addr.id
                        });
                    }
                }
            }
        }

        // helper class for combo box binding
        class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text; // ensures Text is displayed
        }

        private void cmb_address_MouseHover(object sender, EventArgs e)//scratched, since it isconverted to resize not tooltip [keep! if maam Cai changes her mind]
        {
            if (cmb_address.SelectedItem != null)
            {
                string selectedValue = cmb_address.SelectedItem.ToString();
                toolTip.SetToolTip(cmb_address, selectedValue);
            }
        }
        private void cmb_address_DropDown(object sender, EventArgs e)
        {
            if (isNotCurrentlyViewing())
            {
                PopulateAddress();
            }
            else
            {  //commented as for no copy button is needed
                //btn_copyAddress.Visible = true; 
                //btn_copyAddress.Enabled = true;
            }
        }
        private void cmb_address_DropDownClosed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmb_address.Text) && isNotCurrentlyViewing())
            {
                //btn_copyAddress.Visible = false; //commented as for no copy button
                //btn_copyAddress.Enabled = false;
                AdjustComboBoxWidthToContent();
            }
        }
        private void AdjustComboBoxWidthToContent()
        {
            ComboBox cmb = cmb_address;
            if (cmb.SelectedItem == null) return;

            using (Graphics g = cmb.CreateGraphics())
            {
                string text = cmb.GetItemText(cmb.SelectedItem);
                int width = (int)g.MeasureString(text, cmb.Font).Width + SystemInformation.VerticalScrollBarWidth - 10;
                cmb.Width = Math.Max(width, 200); // 50 is general min width
            }
        }

        private void btn_copyAddress_Click(object sender, EventArgs e)  //will use? add the button first
        { //not working since zaxis of dropdown is always above
            Clipboard.SetText(cmb_address.Text);
            Point cellLocationOnForm = cmb_address.PointToScreen(cmb_address.Location);
            Point locationRelativeToForm = this.PointToClient(cellLocationOnForm);
            ToolTip tip = new ToolTip();
            tip.Show("Text copied to clipboard!", this,
                locationRelativeToForm.X + 10,
                locationRelativeToForm.Y - 15,
                1000);

            Clipboard.SetText(cmb_warehouse_name.Text);
            Point cellLocationOnForm2 = cmb_warehouse_name.PointToScreen(cmb_warehouse_name.Location);
            Point locationRelativeToForm2 = this.PointToClient(cellLocationOnForm2);
            ToolTip tip2 = new ToolTip();
            tip2.Show("Text copied to clipboard!", this,
                locationRelativeToForm2.X + 10,
                locationRelativeToForm2.Y - 15,
                1000);
        }
        //btn named btn_copyAddress was temporarily removed since it was deemed unnecessary

        //i cant solve error happening after this event T^T ps. not happening, most prob cause is dataerr
        private void dg_receiving_report_details_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dg_receiving_report_details.Rows.Count)
            {
                var row = dg_receiving_report_details.Rows[e.RowIndex];

                if (!row.IsNewRow)
                {
                    //perform validation here if will have one hehe
                }
            }
        }

        //Datagrids
        //designs
        private bool isPainting = false;
        private async void initializeDG()
        {
            foreach (var grid in dgChildList)
            {
                grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grid.AllowUserToOrderColumns = false;

                grid.ColumnHeadersHeight = ((grid.ColumnHeadersHeight * 2) / 2) + 5;

                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            }

            //fetch one time
            warehouseusetype = await WarehouseUseTypeServices.GetDataTable();

            dg_receiving_report_inventory.EditingControlShowing += (s, e) => //temporary making an event just to set the cmb in dgv dropstyle
            {
                if (dg_receiving_report_inventory.CurrentCell.ColumnIndex ==
                    dg_receiving_report_inventory.Columns["dg_receiving_report_inventory_bin_location"].Index)
                {
                    if (e.Control is ComboBox comboBox)
                    {
                        comboBox.DropDownStyle = ComboBoxStyle.DropDown; //or DropDownList
                    }
                }
            };

            //dg_receiving_report_details.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dg_receiving_report_details.Columns["dg_receiving_report_details_reason_for_rejection"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void dg_DoubleClick(object sender, EventArgs e)
        {
            if (isNotCurrentlyViewing()) return;
            DataGridView dg = sender as DataGridView;
            Rectangle cellDisplayRect = dg.GetCellDisplayRectangle(
                    dg.CurrentCell.ColumnIndex,
                    dg.CurrentCell.RowIndex,
                    false
                );
            Point cellLocationOnForm = dg.PointToScreen(cellDisplayRect.Location);
            Point locationRelativeToForm = this.PointToClient(cellLocationOnForm);

            ToolTip tip = new ToolTip();
            tip.Show("Switch to Edit Mode to change value", this,
                locationRelativeToForm.X + 10,
                locationRelativeToForm.Y - 15,
                1000);
        }
        private void datagrid_Paint(object sender, PaintEventArgs e)
        {
            if (isPainting) return;
            isPainting = true;

            try
            {
                var currentDG = sender as DataGridView;
                if (currentDG == null) return;

                if (currentDG.Name == "dg_receiving_report_details")
                {
                    PaintMergedHeader(currentDG, e.Graphics, "ORDERED", 2, 3);
                    PaintMergedHeader(currentDG, e.Graphics, "RECEIVED", 4, 5);
                    PaintMergedHeader(currentDG, e.Graphics, "REJECTED", 6, 7);
                }
                else if (currentDG.Name == "dg_receiving_report_inventory")
                {
                    PaintMergedHeader(currentDG, e.Graphics, "ORDERED", 2, 3);
                }
            }
            finally
            {
                isPainting = false;
            }
        }
        private void PaintMergedHeader(DataGridView dgv, Graphics g, string headerText, int colStartIndex, int colEndIndex)
        {
            if (colStartIndex >= 0 && colEndIndex >= 0 &&
                colStartIndex < dgv.ColumnCount && colEndIndex < dgv.ColumnCount)
            {
                Rectangle rectStart = dgv.GetCellDisplayRectangle(colStartIndex, -1, true);
                Rectangle rectEnd = dgv.GetCellDisplayRectangle(colEndIndex, -1, true);

                Rectangle mergedRect = new Rectangle
                {
                    X = rectStart.X,
                    Y = rectStart.Y + 2,  //padding for text 2 is most pleasing to my screen, might change in others
                    Width = rectEnd.Right - rectStart.Left - 1,
                    Height = dgv.ColumnHeadersHeight / 2
                };

                using (SolidBrush brush = new SolidBrush(Color.White))
                    g.FillRectangle(brush, mergedRect);

                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using (SolidBrush textBrush = new SolidBrush(dgv.ColumnHeadersDefaultCellStyle.ForeColor))
                    g.DrawString(headerText, dgv.ColumnHeadersDefaultCellStyle.Font, textBrush, mergedRect, format);

                using (Pen borderPen = new Pen(dgv.ColumnHeadersDefaultCellStyle.BackColor))
                    g.DrawLine(borderPen, mergedRect.Left, mergedRect.Bottom - 1, mergedRect.Right, mergedRect.Bottom - 1);
            }
        }
        private void datagrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            var currentDG = sender as DataGridView;
            if (currentDG == null) return;

            Rectangle headerRect = currentDG.DisplayRectangle;
            headerRect.Height = currentDG.ColumnHeadersHeight / 2;

            if (!isPainting)
                currentDG.Invalidate(headerRect);
        }
        private void datagrid_Scroll(object sender, ScrollEventArgs e)
        {

            var currentDG = sender as DataGridView;
            if (currentDG == null) return;

            Rectangle headerRect = currentDG.DisplayRectangle;
            headerRect.Height = currentDG.ColumnHeadersHeight / 2;

            if (!isPainting)
                currentDG.Invalidate(headerRect);
        }
        private void datagrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var currentDG = sender as DataGridView;
            if (currentDG == null) return;

            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle rect = e.CellBounds;
                rect.Y += e.CellBounds.Height / 2;
                rect.Height = e.CellBounds.Height / 2;

                e.PaintBackground(rect, true);

                TextRenderer.DrawText(
                    e.Graphics,
                    e.FormattedValue?.ToString(),
                    e.CellStyle.Font,
                    rect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom
                );

                e.Handled = true;
            }
        }
        private void datagrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var currentDG = sender as DataGridView;
            if (currentDG == null) return;

            string childName = currentDG.Name.Contains("dg_") ? currentDG.Name : "dg_" + currentDG.Name;

            var itmDescCol = currentDG.Columns[childName + "_item_description"];
            if (itmDescCol != null)
                itmDescCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            var reasonCol = currentDG.Columns[childName + "_reason_for_rejection"];
            if (reasonCol != null)
                reasonCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //isPopulatingDG = false;
        }

        //dgs crud and other event
        static List<int> receivingReportDetailsDeletedRowIds = new List<int>();
        static List<int> inventoryDeletedRowIds = new List<int>();
        static int currentRowId = 0;
        static string currentRowDetails = "";
        static int tmpRowValue = 0;
        private void dg_receiving_report_details_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading()) return;
            if (e.RowIndex >= 0
                && e.ColumnIndex >= 0
                //&& e.RowIndex < (sender as DataGridView).Rows.Count - 1 //how to fucking 
                && isNotCurrentlyViewing())
            {
                dg_receiving_report_details.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dg_receiving_report_details.EndEdit();
                autoCalculateQtys((sender as DataGridView).Name, e);
            }
        }
        private void autoCalculateQtys(string sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dg_receiving_report_details.Rows[e.RowIndex].IsNewRow)
                return;

            var row = dg_receiving_report_details.Rows[e.RowIndex];
            string columnName = dg_receiving_report_details.Columns[e.ColumnIndex].Name;
            string dgPrefix = sender + "_";

            //parse quantities
            int.TryParse(row.Cells[dgPrefix + "ordered_qty"]?.Value?.ToString(), out int orderQty);
            int.TryParse(row.Cells[dgPrefix + "received_qty"]?.Value?.ToString(), out int receivedQty);
            int.TryParse(row.Cells[dgPrefix + "rejected_qty"]?.Value?.ToString(), out int rejectedQty);

            //clamp negative values to 0 so there wont be negatives
            orderQty = Math.Max(0, orderQty);
            receivedQty = Math.Max(0, receivedQty);
            rejectedQty = Math.Max(0, rejectedQty);

            if (columnName == dgPrefix + "received_qty")
            {
                receivedQty = Math.Min(receivedQty, orderQty);
                rejectedQty = orderQty - receivedQty;
            }
            else if (columnName == dgPrefix + "rejected_qty")
            {
                rejectedQty = Math.Min(rejectedQty, orderQty);
                receivedQty = orderQty - rejectedQty;
            }
            else if (columnName == dgPrefix + "ordered_qty")
            {
                receivedQty = Math.Min(receivedQty, orderQty);
                rejectedQty = orderQty - receivedQty;
            }
            else if (columnName == dgPrefix + "ordered_uom"
                || columnName == dgPrefix + "ordered_uom"
                || columnName == dgPrefix + "ordered_uom") { }//just to not return if uom is the triggering
            else return;

            //safely assign updated values
            row.Cells[dgPrefix + "ordered_qty"].Value = orderQty;
            row.Cells[dgPrefix + "received_qty"].Value = receivedQty;
            row.Cells[dgPrefix + "rejected_qty"].Value = rejectedQty;

            //UOM fallback logic
            var orderedUom = row.Cells[dgPrefix + "ordered_uom"]?.Value;
            var receivedUomCell = row.Cells[dgPrefix + "received_uom"];
            var rejectedUomCell = row.Cells[dgPrefix + "rejected_uom"];

            if (rejectedUomCell.Value == null || string.IsNullOrWhiteSpace(rejectedUomCell.Value.ToString()))
                rejectedUomCell.Value = orderedUom;
            else if (rejectedQty.ToString() == "0") rejectedUomCell.Value = "N/A";

            //if (receivedUomCell.Value == null || string.IsNullOrWhiteSpace(receivedUomCell.Value.ToString()))
            //if (receivedUomCell.Value == null || string.IsNullOrWhiteSpace(receivedUomCell.Value.ToString()))//change the received regardless
            if (columnName != dgPrefix + "received_uom")
                receivedUomCell.Value = orderedUom;
        }
        private void dg_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var currentDG = sender as DataGridView;
            if (isLoading()
                || e.RowIndex < 0
                || e.RowIndex >= currentDG.Rows.Count
                || currentDG.Rows.Count == 0) return; 

            if (currentDG.Name == null) return;

            if (currentDG.Name == "dg_receiving_report_details")
                receivingReportDetailsDeletedRowIds.Add(currentRowId);
            else if (currentDG.Name == "dg_receiving_report_inventory")
                inventoryDeletedRowIds.Add(currentRowId);
            else
                Console.WriteLine("Missing Datagrid Value");  

            currentRowId = 0;
        }
        static string binLocationOldValue = "";
        static int currentInvRow = 0;
        private void dg_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            currentInvRow = e.RowIndex;
            if (e.RowIndex < 0 || isLoading()) return;
            if (e.RowIndex >= (sender as DataGridView).Rows.Count)
            {
                (sender as DataGridView).CurrentCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex - 1];
            }
            var currentDG = sender as DataGridView;
            if (string.IsNullOrEmpty(currentDG.Rows[e.RowIndex].Cells[currentDG.Name + "_id"]?.Value?.ToString()))
            {
                var bs = currentDG.DataSource as BindingSource;
                if (bs?.DataSource is DataView view)
                {
                    DataTable dt = view.Table;
                    if (dt != null && e.RowIndex >= dg_receiving_report_inventory.Rows.Count - 1)
                    {
                        DataRow newRow1 = dt.NewRow();
                        newRow1["receiving_report_id"] = txt_id.Text;
                        dt.Rows.Add(newRow1);
                    }
                }
            }

            if (currentDG == null) return;

            tmpRowValue = e.RowIndex;
            Console.WriteLine(tmpRowValue);

            string columnName = currentDG.Name + "_id";
            object cellValue = currentDG.Rows[e.RowIndex].Cells[columnName].Value;

            currentRowDetails = "details:\n";
            currentRowDetails += currentDG.Columns[1].HeaderText.ToLower()
                               + " of '"
                               + (string.IsNullOrEmpty(currentDG.Rows[e.RowIndex].Cells[1].Value?.ToString())
                                  ? "n/a"
                                  : currentDG.Rows[e.RowIndex].Cells[1].Value.ToString())
                               + "' and with the ";
            currentRowDetails += currentDG.Columns[2].HeaderText.ToLower()
                               + " of '"
                               + (string.IsNullOrEmpty(currentDG.Rows[e.RowIndex].Cells[2].Value?.ToString())
                                  ? "n/a"
                                  : currentDG.Rows[e.RowIndex].Cells[2].Value.ToString())
                               + "'?";

            if (int.TryParse(cellValue?.ToString(), out int parsedId))
            {
                currentRowId = parsedId;
            }
        }
        private void dg_receiving_report_inventory_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dg_receiving_report_inventory.Columns[e.ColumnIndex].Name != "dg_receiving_report_inventory_bin_location") return;
            //if (dg_receiving_report_inventory.Columns[e.ColumnIndex].Name == "dg_receiving_report_inventory_bin_location")
            //{
            //    var cellValue = dg_receiving_report_inventory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            //    if (!string.IsNullOrWhiteSpace(cellValue))
            //    {
            //        binLocationOldValues[e.RowIndex] = cellValue;
            //    }
            //}
            
            if (dg_receiving_report_inventory.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                var cell = (DataGridViewComboBoxCell)dg_receiving_report_inventory.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var currentValue = cell.Value?.ToString();

                if (cell?.Items != null 
                    && cell.Value != null
                    && cell.Items.Contains(currentValue))
                {
                    cell.Items.Add(currentValue);
                }
            }

        }
        private async void commitRowDeletion()
        { 
            string debugstring = " !isSuccess:\n";

            //commit deletions
            foreach (var item in dgChildList)
            {
                List<int> idsToDelete = null;
                string currentDataGridName = "";

                if (item.Name == "dg_receiving_report_details")
                {
                    if (receivingReportDetailsDeletedRowIds.Count == 0)
                        continue;

                    idsToDelete = receivingReportDetailsDeletedRowIds;
                    currentDataGridName = "receiving_report_details";
                }
                else if (item.Name == "dg_receiving_report_inventory")
                {
                    if (inventoryDeletedRowIds.Count == 0)
                        continue;

                    idsToDelete = inventoryDeletedRowIds;
                    currentDataGridName = "receiving_report_inventory";
                }
                else
                {
                    continue;
                }

                foreach (int idToDelete in idsToDelete)
                {
                    var data = new Dictionary<string, dynamic>
                    {
                        [currentDataGridName] = new Dictionary<string, int>
                        {
                            ["id"] = idToDelete
                        }
                    };

                    bool isSuccess = item.Name == "dg_receiving_report_details" 
                        ? await ReceivingReportServices.DeleteDetailsSpecificRow(data) 
                        : await ReceivingReportServices.DeleteInventorySpecificRow(data);

                    Console.WriteLine("deletion result: " + isSuccess);
                    if (!isSuccess) debugstring += idToDelete + "\n";
                }
            }

            receivingReportDetailsDeletedRowIds.Clear();
            inventoryDeletedRowIds.Clear();
        }

        private void dg_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid.CurrentCell == null) return;
            string currentColumnName = grid.Columns[grid.CurrentCell.ColumnIndex].Name;

            if (e.KeyCode == Keys.Delete)
            {
                var dg = sender as DataGridView;

                if (dg == null || dg.SelectedRows.Count == 0)
                    return;

                string message = "Are you sure you want to delete the row with " + currentRowDetails;

                DialogResult result = MessageBox.Show(
                    message,
                    "Row deletion on" + (sender as DataGridView).Name.Replace("_", " ").Replace("dg", "").ToUpper(),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                {
                    e.SuppressKeyPress = true; //cancel the Delete key action
                    e.Handled = true;
                }
                return;
            }
            if ((e.KeyCode == Keys.Tab
                || e.KeyCode == Keys.Enter
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
                && currentColumnName == "dg_receiving_report_inventory_bin_location")
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }

        }
        private void dg_receiving_report_inventory_DataError(object sender, DataGridViewDataErrorEventArgs e) //malupitang off ng data error XD
        {
            string errorMessage = $@"
                Exception: {e.Exception?.GetType().Name}
                Message: {e.Exception?.Message}
                ColumnIndex: {e.ColumnIndex}
                RowIndex: {e.RowIndex}
                Context: {e.Context}
            ";

            //MessageBox.Show(errorMessage);

           //var value = dg_receiving_report_inventory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
           // MessageBox.Show($"Invalid value: {value}");

            //to prevent crash
            e.ThrowException = false;

            return;
        }



        //bin location dg's cmb area
        private Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>();
        //all custom event trigger for dg_reaceivng_report_inventory_bin_location in here
        private async void dg_receiving_report_inventory_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        { //async so all will be loaded before event triggers
            if (dg_receiving_report_inventory.CurrentCell.ColumnIndex ==
                dg_receiving_report_inventory.Columns["dg_receiving_report_inventory_bin_location"].Index)
            {
                if (e.Control is ComboBox dg_cmb_bin_location)
                {
                    textChangeSuppressor = true;
                    //await PopulateBinLocation_UseType();
                    binLocationOldValue = dg_cmb_bin_location.Text;

                    if (!string.IsNullOrEmpty(dg_cmb_bin_location.Text)) 
                    { 
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to change Bin Location?",
                            "Change Bin Location",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                            );
                        if (result != DialogResult.Yes)
                        {
                            dg_cmb_bin_location.Items.Add(binLocationOldValue);
                            dg_cmb_bin_location.Text = binLocationOldValue;
                            this.SelectNextControl(this.ActiveControl, true, true, true, true);
                            dg_cmb_bin_location.DroppedDown = false;
                            return;
                        }
                    }
                    if (warehouseusetype.Rows.Count <= 0) return;
                    dg_cmb_bin_location.Items.Clear();
                    dg_cmb_bin_location.Text = "";

                    colorDictionary.Clear();

                    dg_cmb_bin_location.DrawMode = DrawMode.OwnerDrawFixed;
                    foreach (DataRow row in warehouseusetype.Rows)
                    {
                        string useType = row["name"]?.ToString().ToUpper();
                        string colorName = row["bg_color"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(useType) && !string.IsNullOrWhiteSpace(colorName))
                        {
                            Color color = Color.FromName(colorName);

                            dg_cmb_bin_location.BackColor = Color.White;
                            dg_cmb_bin_location.ForeColor = color.GetBrightness() < 0.6f ? Color.White : Color.Black;

                            if (!dg_cmb_bin_location.Items.Contains(useType))
                                dg_cmb_bin_location.Items.Add(useType);

                            if (!colorDictionary.ContainsKey(useType))
                                colorDictionary[useType] = color;
                        }
                    }
                    //if (!string.isNullOrEmpty(binLocationOldValue)) dg_cmb_bin_location.Items.Add(binLocationOldValue); 
                     
                    textChangeSuppressor = false;

                    //custom event triger for dg cells 
                    //setting up item color preview
                    dg_cmb_bin_location.DrawItem -= dg_cmb_bin_location_DrawItem;
                    dg_cmb_bin_location.DrawItem += dg_cmb_bin_location_DrawItem;
                    //fixing basic visual
                    dg_cmb_bin_location.DropDown -= dg_cmb_bin_location_DropDown;
                    dg_cmb_bin_location.DropDown += dg_cmb_bin_location_DropDown;
                    //initializes the fetching for use type
                    dg_cmb_bin_location.Enter -= dg_cmb_bin_location_Enter;
                    dg_cmb_bin_location.Enter += dg_cmb_bin_location_Enter;
                    //saves the selected value to a new seperate item so it'll persist until saving / refetching if canceled
                    dg_cmb_bin_location.Leave -= dg_cmb_bin_location_Leave;
                    dg_cmb_bin_location.Leave += dg_cmb_bin_location_Leave;

                    //where when an item is clicked it'll will reflect on this, this is where the content += "-" occur
                    dg_cmb_bin_location.SelectedIndexChanged -= dg_cmb_bin_location_SelectedIndexChanged; 
                    dg_cmb_bin_location.SelectedIndexChanged += dg_cmb_bin_location_SelectedIndexChanged; 

                    dg_cmb_bin_location.SelectionChangeCommitted -= dg_cmb_bin_location_SelectionChangeCommitted;
                    dg_cmb_bin_location.SelectionChangeCommitted += dg_cmb_bin_location_SelectionChangeCommitted;  

                    //mainly for tab & enter key suppressor, it causes absurd out puts
                    dg_cmb_bin_location.KeyDown -= dg_cmb_bin_location_KeyDown;
                    dg_cmb_bin_location.KeyDown += dg_cmb_bin_location_KeyDown;

                    //where detection of dashes "-" happens, what and when the set of items are loaded, and automatical capitalization  //not optimal! can throw stack overflow if spammed (not consistent)
                    dg_cmb_bin_location.TextChanged -= dg_cmb_bin_location_TextChanged;
                    dg_cmb_bin_location.TextChanged += dg_cmb_bin_location_TextChanged; 
                }
            }
        }
        private async void dg_cmb_bin_location_TextChanged(object sender, EventArgs e)
        {
            //if (isLoading()) return;
            //if (textChangeSuppressor)
            //{
            //    textChangeSuppressor = false;
            //    return;
            //}

            //await PopulateBinLocation(sender);  
            if (isLoading() || textChangeSuppressor) return; 
            try
            {
                textChangeSuppressor = true;
                await PopulateBinLocation(sender);
            }
            finally
            { 
                textChangeSuppressor = false; 
            } 
        } 
        private async Task PopulateBinLocation(object sender)
        { 
            ComboBox dg_cmb_bin_location = sender as ComboBox;
 
            if (dg_cmb_bin_location.Text.Contains("-") && dg_cmb_bin_location.Text.Replace("-", "").Length <= 1)
            {
                textChangeSuppressor = true;
                dg_cmb_bin_location.Text = "";
                return;
            }
            dg_cmb_bin_location.SelectionStart = dg_cmb_bin_location.Text.Length;
            if (!dg_cmb_bin_location.Text.All(c => char.IsUpper(c)))
                dg_cmb_bin_location.Text = dg_cmb_bin_location.Text.ToUpper();

            tmpStringForBins = dg_cmb_bin_location.Text;
            if (!tmpStringForBins.Contains("-"))
            {
                //if (dg_cmb_bin_location.Items.Contains)
                await PopulateBinLocation_UseType(dg_cmb_bin_location);
            }
            else
            {
                dashCounter = tmpStringForBins.Count(c => c == '-');
                if (dashCounter >= warehouseAreaColumns.Count - 1)
                {
                    dg_cmb_bin_location.Text = dg_cmb_bin_location.Text.TrimEnd('-');

                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    dg_receiving_report_inventory.EndEdit();
                    return;
                }
                else if (tmpStringForBins.Replace("-", "").Length >= 1) //to skip loading area when no usetype chose
                {
                    await PopulateBinLocation_WarehouseArea(dg_cmb_bin_location);
                }
            }
            dg_cmb_bin_location.DroppedDown = true;

        }
        private Dictionary<int, string> binLocationOldValues = new Dictionary<int, string>();
        private async Task PopulateBinLocation_UseType(ComboBox dg_cmb_bin_location) //has to be awaited because coloring sometimes lags
        {
            if (warehouseusetype.Rows.Count <= 0) return; 

            dg_cmb_bin_location.Items.Clear();
            colorDictionary.Clear();

            foreach (DataRow row in warehouseusetype.Rows)
            {
                string useType = row["name"]?.ToString().ToUpper();
                string colorName = row["bg_color"]?.ToString();

                if (!string.IsNullOrWhiteSpace(useType) && !string.IsNullOrWhiteSpace(colorName))
                {
                    Color color = Color.FromName(colorName);

                    if (!dg_cmb_bin_location.Items.Contains(useType))
                        dg_cmb_bin_location.Items.Add(useType);

                    if (!colorDictionary.ContainsKey(useType))
                        colorDictionary[useType] = color;
                }
            }
            dg_cmb_bin_location.Items.Add(binLocationOldValue);
        }
        public class ComboBoxColorItem
        {
            public string Name { get; set; }
            public Color BgColor { get; set; }

            public ComboBoxColorItem(string name, Color bgColor)
            {
                Name = name;
                BgColor = bgColor;
            }
        }
        private void dg_cmb_bin_location_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox dg_cmb_bin_location = sender as ComboBox;
            string itemName = dg_cmb_bin_location.Items[e.Index]?.ToString();

            Rectangle fullRect = e.Bounds;
            int swatchWidth = (int)(fullRect.Width * 0.1); //width 10%
            int margin = 2;

            Rectangle textRect = new Rectangle(
                fullRect.X,
                fullRect.Y,
                fullRect.Width - swatchWidth,
                fullRect.Height
            );

            Rectangle colorRect = new Rectangle(
                fullRect.Right - swatchWidth + margin - 2,
                fullRect.Y + margin,
                swatchWidth - 3 * margin,
                fullRect.Height - 2 * margin
            );

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color backgroundColor = isSelected ? SystemColors.Highlight : SystemColors.Window;

            using (var backgroundBrush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(backgroundBrush, fullRect);

            if (itemName != null && colorDictionary.TryGetValue(itemName, out Color swatchColor))
            {
                using (Brush colorBrush = new SolidBrush(swatchColor))
                    e.Graphics.FillRectangle(colorBrush, colorRect);

                e.Graphics.DrawRectangle(Pens.Black, colorRect);
            }

            Color textColor = isSelected ? SystemColors.HighlightText : SystemColors.ControlText;
            using (Brush textBrush = new SolidBrush(textColor))
                e.Graphics.DrawString(itemName, e.Font, textBrush, textRect, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
        static bool textChangeSuppressor = false;
        private void dg_cmb_bin_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox dg_cmb_bin_location = sender as ComboBox;
            if (dg_cmb_bin_location.SelectedItem == null) return;

            string selectedValue = dg_cmb_bin_location.SelectedItem.ToString().ToUpper();
             
            foreach (DataRow row in warehouseusetype.Rows)
            {
                string useType = row["name"]?.ToString().ToUpper();
                string colorName = row["bg_color"]?.ToString();

                if (!string.IsNullOrWhiteSpace(useType) && !string.IsNullOrWhiteSpace(colorName))
                {
                    Color color = Color.FromName(colorName); 

                    if (!colorDictionary.ContainsKey(useType))
                        colorDictionary[useType] = color;

                    if (selectedValue.Contains(useType))
                    {
                        dg_cmb_bin_location.BackColor = color;
                        dg_cmb_bin_location.ForeColor = color.GetBrightness() < 0.5f ? Color.White : Color.Black;
                    }
                }
            } 
            var segments = tmpStringForBins.Split('-')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            if (segments.Count < warehouseAreaColumns.Count && segments.Count > 0)
            {
                if (!selectedValue.Contains('-'))
                {
                    if (!segments.Last().TrimEnd('-').Equals(dg_cmb_bin_location.Text))
                        segments.Add(selectedValue); 
                } 

                tmpStringForBins = string.Join("-", segments);

                if (segments.Count < warehouseAreaColumns.Count - 1)
                {
                    if (string.IsNullOrWhiteSpace(tmpStringForBins)) tmpStringForBins += "-";
                }

                dg_cmb_bin_location.Text = tmpStringForBins; 
                dg_cmb_bin_location.SelectionStart = dg_cmb_bin_location.Text.Length;
                 
                if (dg_receiving_report_inventory.CurrentCell is DataGridViewComboBoxCell comboCell)
                {
                    comboCell.Value = tmpStringForBins;

                    if (!comboCell.Items.Contains(tmpStringForBins))
                    {
                        comboCell.Items.Clear();
                        while (tmpStringForBins.EndsWith("-")) tmpStringForBins = tmpStringForBins.TrimEnd('-');
                        comboCell.Items.Add(tmpStringForBins);
                    }
                }

                dg_receiving_report_inventory.Rows[currentInvRow].Cells["dg_receiving_report_inventory_bin_location"]
                    .Value = tmpStringForBins;
            }

        } 
        private void dg_cmb_bin_location_Enter(object sender, EventArgs e)
        {
            textChangeSuppressor = false;
            ComboBox dg_cmb_bin_location = sender as ComboBox;
            dg_cmb_bin_location.BackColor = Color.White;
            dg_cmb_bin_location.ForeColor = Color.Black;
            dg_cmb_bin_location.DroppedDown = true;
        }
        private void dg_cmb_bin_location_Leave(object sender, EventArgs e)
        {
            ComboBox dg_cmb_bin_location = sender as ComboBox;
            textChangeSuppressor = true;

            //trim trailing dash
            tmpStringForBins = dg_cmb_bin_location.Text.TrimEnd('-');

            //split segments and remove duplicates (consecutive or global)
            var segments = tmpStringForBins
                .Split('-')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            //rebuild the final value
            tmpStringForBins = string.Join("-", segments);

            //clear and re-add cleaned item to ComboBox
            dg_cmb_bin_location.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpStringForBins))
            {
                dg_cmb_bin_location.Items.Add(tmpStringForBins);
            }

            //sets value to cell
            var cell = (DataGridViewComboBoxCell)dg_receiving_report_inventory.Rows[currentInvRow]
                .Cells["dg_receiving_report_inventory_bin_location"];

            cell.Value = tmpStringForBins;

            //cleans item list
            if (dg_receiving_report_inventory.Columns["dg_receiving_report_inventory_bin_location"]
                is DataGridViewComboBoxColumn && !cell.Items.Contains(tmpStringForBins))
            {
                cell.Items.Add(tmpStringForBins);
            }

            //update coloring
            foreach (DataRow row in warehouseusetype.Rows)
            {
                string useType = row["name"]?.ToString().ToUpper();
                string colorName = row["bg_color"]?.ToString();

                if (!string.IsNullOrWhiteSpace(useType) && !string.IsNullOrWhiteSpace(colorName))
                {
                    Color color = Color.FromName(colorName);
                    if (!colorDictionary.ContainsKey(useType))
                        colorDictionary[useType] = color;

                    if (tmpStringForBins.Contains(useType))
                    {
                        cell.Style.BackColor = color;
                        cell.Style.ForeColor = color.GetBrightness() < 0.5f ? Color.White : Color.Black;
                    }
                }
            }

            textChangeSuppressor = false;
        }
        
        static int dashCounter = 0;
        static string tmpStringForBins = "";
        static List<string> warehouseAreaColumns = new List<string>
            {
                "use_type", "zone", "area", "rack", "level", "bins", "location_code"
            };
        static bool isPopulatingBinLocation = false;
        static string nextColumn = "";
        DataTable warehouseAreaTable;
        private async Task PopulateBinLocation_WarehouseArea(ComboBox dg_cmb_bin_location) //doesnt filter inactive, yet
        {
            try
            {
                isPopulatingBinLocation = true;
                bool shouldDroppedDown = true;

                if (dashCounter >= warehouseAreaColumns.Count - 2)
                {
                    tmpStringForBins = tmpStringForBins.TrimEnd('-');
                    dg_cmb_bin_location.Text = tmpStringForBins;
                    shouldDroppedDown = false;
                }

                dg_cmb_bin_location.Items.Clear();
                string currentText = dg_cmb_bin_location.Text;

                string[] segments = tmpStringForBins.Split('-');
                bool isLastOpen = string.Equals(segments.Reverse().FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)), "OPEN", StringComparison.OrdinalIgnoreCase);
                if (isLastOpen)
                {
                    tmpStringForBins = tmpStringForBins.TrimEnd('-');
                    dg_cmb_bin_location.DroppedDown = false;
                }

                nextColumn = warehouseAreaColumns[dashCounter];
                string lastColumn = warehouseAreaColumns.Last();
                var filtered = warehouseAreaTable.AsEnumerable();

                for (int i = 0; i < dashCounter; i++)
                {
                    string filterColumn = warehouseAreaColumns[i];
                    string filterValue = segments[i];
                    filtered = filtered.Where(row => string.Equals(row.Field<string>(filterColumn), filterValue, StringComparison.OrdinalIgnoreCase));
                }

                var filteredList = filtered.ToList();

                bool shouldAddOpen = filteredList.Any(row =>
                {
                    bool allEmptyTillLast = warehouseAreaColumns.Skip(dashCounter).All(col =>
                        string.IsNullOrWhiteSpace(row[col]?.ToString())
                    );

                    string locationCode = row[lastColumn]?.ToString();
                    return allEmptyTillLast && !string.IsNullOrEmpty(locationCode) && locationCode.Contains("OPEN");
                });

                var nextOptions = filteredList
                    .Select(row => row.Field<string>(nextColumn))
                    .Where(val => !string.IsNullOrWhiteSpace(val))
                    .Distinct()
                    .OrderBy(val => val)
                    .ToList();

                if (!currentText.EndsWith("-") && !currentText.Contains("OPEN"))
                {
                    dg_cmb_bin_location.Items.Add(currentText);
                    dg_cmb_bin_location.Text = currentText;
                }

                if (shouldAddOpen && !dg_cmb_bin_location.Items.Cast<object>().Any(i => i.ToString().Equals("OPEN", StringComparison.OrdinalIgnoreCase)))
                {
                    dg_cmb_bin_location.Items.Add("OPEN");
                }

                foreach (var item in nextOptions)
                {
                    if (int.TryParse(item, out int binRange))
                    {
                        dg_cmb_bin_location.Items.Clear();
                        for (int i = 1; i <= binRange; i++)
                            dg_cmb_bin_location.Items.Add(i.ToString());
                    }
                    else
                    {
                        dg_cmb_bin_location.Items.Add(item.ToUpper());
                    }
                }

                if (!nextOptions.Any())
                {
                    var lastOptions = filteredList
                        .Select(row => row.Field<string>(lastColumn))
                        .Where(val => !string.IsNullOrWhiteSpace(val))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(val => val);

                    var firstOptions = filteredList
                        .Select(row => row.Field<string>(warehouseAreaColumns.First()))
                        .Where(val => !string.IsNullOrWhiteSpace(val))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(val => val);

                    string finalOption = string.Join("-", firstOptions) + "-" + string.Join("-", lastOptions);

                    finalOption = finalOption.TrimEnd('-');

                    if (!dg_cmb_bin_location.Items.Cast<object>().Any(i => i.ToString().Equals(finalOption, StringComparison.OrdinalIgnoreCase)))
                    {
                        dg_cmb_bin_location.Items.Add(finalOption.ToUpper());
                    }
                }

                dg_cmb_bin_location.MaxDropDownItems = 10;
                dg_cmb_bin_location.DroppedDown = shouldDroppedDown;
                dg_cmb_bin_location.SelectionStart = dg_cmb_bin_location.Text.Length;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("index"))
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
            finally
            {
                isPopulatingBinLocation = false;
                dg_cmb_bin_location.SelectionStart = dg_cmb_bin_location.Text.Length;
            }
        }
        private void dg_cmb_bin_location_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                dg_receiving_report_inventory.EndEdit();
            }
            else if (e.KeyCode == Keys.Tab
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
            {
                this.ActiveControl = null;

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
        private void dg_cmb_bin_location_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox dg_cmb_bin_location = sender as ComboBox;

            if (dg_cmb_bin_location.SelectedItem == null)
                return;

            string selectedText = dg_cmb_bin_location.SelectedItem.ToString();
            string currentText = dg_cmb_bin_location.Text.TrimEnd('-');

            if (string.Equals(selectedText, currentText, StringComparison.OrdinalIgnoreCase))
            {
                dg_cmb_bin_location.DroppedDown = false;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                dg_receiving_report_inventory.EndEdit();
                return;
            }

            var segments = tmpStringForBins.Split('-').ToList();

            if (segments.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(segments.Last()))
                    segments[segments.Count - 1] = selectedText;
                else
                    segments[segments.Count - 1] = selectedText;
            }
            else
            {
                segments.Add(selectedText);
            }

            tmpStringForBins = string.Join("-", segments);

            if (dashCounter < warehouseAreaColumns.Count - 1 && !tmpStringForBins.EndsWith("-"))
                tmpStringForBins += "-";

            dg_cmb_bin_location.Text = tmpStringForBins;
        }
        private void dg_cmb_bin_location_DropDown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
          
        private int hoveredIndex = -1;
        private Cursor defaultCursor = Cursors.Default;
        private Cursor hoverCursor = Cursors.Hand;
        public class SmoothListBox : ListBox
        {
            public SmoothListBox()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.DrawMode = DrawMode.OwnerDrawFixed;
                this.ItemHeight = 30;
            }
        }
        private string GetTargetFolder()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string docFolderName = string.IsNullOrWhiteSpace(txt_doc.Text) ? "DefaultDoc" : txt_doc.Text.Trim();
            string folderPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\Resources\SharedFiles\ReceivingReportAttachments", docFolderName));
            Directory.CreateDirectory(folderPath);  //ensure folder exists
            //string fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\SharedFiles\ReceivingReportAttachments", docFolderName)); MessageBox.Show(fullPath);  //just to show the its path locally

            return folderPath;
        }
        private string GetUniqueTargetPath(string fileName)//no reference but this is used! dont remove!
        {
            string folder = GetTargetFolder();
            string targetPath = Path.Combine(folder, fileName);

            if (File.Exists(targetPath))
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                string ext = Path.GetExtension(fileName);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                targetPath = Path.Combine(folder, $"{name}_{timestamp}{ext}");
            }

            return targetPath;
        }
        //currently no *open others file from my local functionality
        private void LoadExistingFiles()
        {
            string folderPath = GetTargetFolder();
            if (!Directory.Exists(folderPath))
            {
                lst_receiving_report_attachment_files.Items.Clear();
                lst_receiving_report_attachment_files.Items.Add("No attachments found");
                lst_receiving_report_attachment_files.Enabled = false; //optional, can disable if you want fewer lines
                return;
            }

            string[] files = Directory.GetFiles(folderPath);

            string[] allowedExtensions = { ".doc", ".docx", ".jpg", ".jpeg", ".png", ".pdf", ".txt" };
            lst_receiving_report_attachment_files.DrawMode = DrawMode.OwnerDrawFixed;
            lst_receiving_report_attachment_files.ItemHeight = 30;
            lst_receiving_report_attachment_files.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            lst_receiving_report_attachment_files.Items.Clear();

            var allowedFiles = files.Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower())).ToArray();

            if (allowedFiles.Length == 0)
            {
                lst_receiving_report_attachment_files.Items.Add("No attachments found");
                lst_receiving_report_attachment_files.Enabled = false;  //optionally disable the list
            }
            else
            {
                lst_receiving_report_attachment_files.Enabled = true;
                foreach (string file in allowedFiles)
                {
                    string fileName = Path.GetFileName(file);
                    lst_receiving_report_attachment_files.Items.Add(fileName);
                }
            }
        }
        private void btn_upload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter =
                    "Unfiltered (*.doc;*.docx;*.pdf;*.txt;*.jpg;*.jpeg)|*.doc;*.docx;*.pdf;*.txt;*.jpg;*.jpeg|" +
                    "Documents (*.doc;*.docx;*.pdf;*.txt)|*.doc;*.docx;*.pdf;*.txt|" +
                    "Images (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff|" +
                    "All files (*.*)|*.*";

                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = openFileDialog.FileName;
                    string originalName = Path.GetFileNameWithoutExtension(sourcePath);
                    string extension = Path.GetExtension(sourcePath).ToLower();

                    string[] allowedExtensions = { ".doc", ".docx", ".pdf", ".txt", ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        MessageBox.Show($"The selected file type '{extension}' is not supported.", "Unsupported File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string targetFolder = GetTargetFolder();

                    string[] convertibleImageExts = { ".png", ".bmp", ".gif", ".tiff" };
                    bool isConvertibleImage = convertibleImageExts.Contains(extension);

                    string finalExtension = isConvertibleImage ? ".jpg" : extension;
                    string fileName = originalName + finalExtension;
                    string targetPath = Path.Combine(targetFolder, fileName);

                    int count = 1;
                    while (File.Exists(targetPath))
                    {
                        fileName = $"{originalName} copy-{count}{finalExtension}";
                        targetPath = Path.Combine(targetFolder, fileName);
                        count++;
                    }
                    string isConvertedTo = "";
                    if (isConvertibleImage)
                    {
                        using (Image img = Image.FromFile(sourcePath))
                        {
                            img.Save(targetPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        isConvertedTo = $"{originalName}{finalExtension} was converted to {originalName}.jpg,\n";
                    }
                    else
                    {
                        File.Copy(sourcePath, targetPath);
                    }

                    lst_receiving_report_attachment_files.Items.Add(fileName);
                    Helpers.ShowDialogMessage("success", string.IsNullOrEmpty(isConvertedTo) ? $"File saved to:\n{targetPath}" : isConvertedTo + $"File saved to:\n{targetPath}");
                    LoadExistingFiles(); //to re arrange
                    lst_receiving_report_attachment_files.SelectedItem = fileName;
                }
            }
        }
        private void lst_receiving_report_attachment_DoubleClick(object sender, EventArgs e)
        {
            openSelectedFile();
        }
        private void openSelectedFile()
        {
            if (lst_receiving_report_attachment_files.SelectedItem != null)
            {
                string fileName = lst_receiving_report_attachment_files.SelectedItem.ToString();

                string targetFolder = GetTargetFolder();
                string fullPath = Path.Combine(targetFolder, fileName);

                if (File.Exists(fullPath))
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
                }
                else
                {
                    Helpers.ShowDialogMessage("error", "File not found");
                }
            }
        }
        private void lst_receiving_report_attachment_files_KeyDown(object sender, KeyEventArgs e)
        {
            if (lst_receiving_report_attachment_files.SelectedItem == null) return;
            if (e.KeyCode == Keys.Delete)
            {
                string selectedFileName = lst_receiving_report_attachment_files.SelectedItem.ToString();

                string targetFolder = GetTargetFolder();
                string fullPath = Path.Combine(targetFolder, selectedFileName);

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{selectedFileName}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DeleteSelectedAttachmentItem(fullPath);
                    }
                    catch (Exception ex)
                    {
                        Helpers.ShowDialogMessage("error", "Error deleting file:\n" + ex.Message);
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                openSelectedFile();
            }
        }
        private void DeleteSelectedAttachmentItem(string fullPath)
        { 
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            LoadExistingFiles();
        }
        //since we dont have universal icon for images, docs, and pdf and such
        private void lst_receiving_report_attachment_files_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string text = lst_receiving_report_attachment_files.Items[e.Index].ToString();

            bool isSelected = (e.State & DrawItemState.Selected) != 0;
            bool isHovered = (e.Index == hoveredIndex);
             
            Color bgColor;
            Color textColor;

            if (isSelected)
            {
                bgColor = SystemColors.Highlight;
                textColor = Color.White;
            }
            else if (isHovered)
            {
                bgColor = Color.LightBlue;
                textColor = Color.Black;
            }
            else
            {
                bgColor = lst_receiving_report_attachment_files.BackColor;
                textColor = Color.Black;
            }
             
            Rectangle paddedBounds = new Rectangle(
                e.Bounds.X + 5,
                e.Bounds.Y + 4,
                e.Bounds.Width - 10,
                e.Bounds.Height - 8
            );
             
            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(bgBrush, paddedBounds);
            }
             
            using (Brush textBrush = new SolidBrush(textColor))
            {
                string bullet = "• ";
                int paddingLeft = 24;
                e.Graphics.DrawString(bullet + text, e.Font, textBrush, paddedBounds.Left + paddingLeft, paddedBounds.Top + 2);
            }
             
            if ((e.State & DrawItemState.Focus) != 0 && isSelected)
            { 
                ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, textColor, bgColor);
            }
        }
        private void lst_receiving_report_attachment_files_MouseMove(object sender, MouseEventArgs e)
        {
            int index = lst_receiving_report_attachment_files.IndexFromPoint(e.Location);

            if (index != hoveredIndex)
            {
                // Invalidate old hovered item
                if (hoveredIndex >= 0 && hoveredIndex < lst_receiving_report_attachment_files.Items.Count)
                {
                    Rectangle oldBounds = lst_receiving_report_attachment_files.GetItemRectangle(hoveredIndex);
                    lst_receiving_report_attachment_files.Invalidate(oldBounds);
                }

                // Invalidate new hovered item
                if (index >= 0 && index < lst_receiving_report_attachment_files.Items.Count)
                {
                    Rectangle newBounds = lst_receiving_report_attachment_files.GetItemRectangle(index);
                    lst_receiving_report_attachment_files.Invalidate(newBounds);
                }

                hoveredIndex = index;
            }

            lst_receiving_report_attachment_files.Cursor = (index >= 0) ? Cursors.Hand : Cursors.Default;
        }
        private void lst_receiving_report_attachment_files_MouseLeave(object sender, EventArgs e)
        {
            if (hoveredIndex >= 0 && hoveredIndex < lst_receiving_report_attachment_files.Items.Count)
            {
                Rectangle bounds = lst_receiving_report_attachment_files.GetItemRectangle(hoveredIndex);
                lst_receiving_report_attachment_files.Invalidate(bounds);
            }

            hoveredIndex = -1;
            lst_receiving_report_attachment_files.Cursor = Cursors.Default;
        }
          
        private bool isNotCurrentlyViewing(string request = "")
        {
            bool output = false;
            if (!string.IsNullOrEmpty(request))
            {
                if (request.Contains("edit"))
                {
                    if (btn_edit.Visible)
                        output = true;
                }
                else if (request.Contains("creat"))
                {
                    if (btn_new.Visible)
                        output = true;
                }
            }
            else 
            {
                if (btn_cancel.Visible )
                    output = true;
            }
            return output;
        }

        //need to load again for recoloring
        static bool isInitialLoad;
        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialLoad && tabControl1.SelectedIndex == 1)
            {
                await FetchDG("dg_receiving_report_inventory");
                isInitialLoad = false;
            }
        }

        private void SyncDataGridViews()//failed attempt to implement SRP and DRY
        {
            if (isNotCurrentlyViewing())
            {
                lst_receiving_report_attachment_files.Items.Clear();
                dg_receiving_report_details.DataSource = null;
                dg_receiving_report_inventory.DataSource = null;
                return;
            }
            var dgv1 = dgChildList[0];
            var dgv2 = dgChildList[1];
             
            string prefix1 = dgv1.Name.EndsWith("_") ? dgv1.Name : dgv1.Name + "_";
            string prefix2 = dgv2.Name.EndsWith("_") ? dgv2.Name : dgv2.Name + "_";
             
            var dgv1Cols = dgv1.Columns
                .Cast<DataGridViewColumn>()
                .ToDictionary(col => col.Name.Replace(prefix1, ""), col => col.Name);

            var dgv2Cols = dgv2.Columns
                .Cast<DataGridViewColumn>()
                .ToDictionary(col => col.Name.Replace(prefix2, ""), col => col.Name);
             
            var sharedColumns = dgv1Cols.Keys.Intersect(dgv2Cols.Keys);
             
            var sourceDGV = dgv1.Rows.Count > dgv2.Rows.Count ? dgv1 : dgv2;
            var targetDGV = sourceDGV == dgv1 ? dgv2 : dgv1;

            var sourcePrefix = sourceDGV.Name.EndsWith("_") ? sourceDGV.Name : sourceDGV.Name + "_";
            var targetPrefix = targetDGV.Name.EndsWith("_") ? targetDGV.Name : targetDGV.Name + "_";

            var sourceCols = sourceDGV.Columns
                .Cast<DataGridViewColumn>()
                .ToDictionary(col => col.Name.Replace(sourcePrefix, ""), col => col.Name);

            var targetCols = targetDGV.Columns
                .Cast<DataGridViewColumn>()
                .ToDictionary(col => col.Name.Replace(targetPrefix, ""), col => col.Name);

            int rowCount = sourceDGV.Rows.Count;
             
            while (targetDGV.Rows.Count < rowCount)
            {
                if (!isNotCurrentlyViewing("creating")) ((DataTable)targetDGV.DataSource).Rows.Add();
                else rowCount = 0;
            }
             
            for (int i = 0; i < rowCount; i++)
            {
                foreach (var logicalName in sharedColumns)
                {
                    if (sourceCols.ContainsKey(logicalName) && targetCols.ContainsKey(logicalName))
                    {
                        var value = sourceDGV.Rows[i].Cells[sourceCols[logicalName]].Value;
                        targetDGV.Rows[i].Cells[targetCols[logicalName]].Value = value;
                    }
                }
            }
        }
    }
}
//ComboBox dg_cmb_bin_location = sender as ComboBox; //make this as a class level so no repetitive

