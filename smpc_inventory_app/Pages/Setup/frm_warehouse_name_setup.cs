using Newtonsoft.Json;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Services.Setup.Warehouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_warehouse_name_setup : UserControl
    {
        string currentUserPosition = CacheData.CurrentUser.position_id.ToString().ToLower();
        WarehouseList warehouseDataList;
        DataTable warehousename;
        DataTable address;
        DataTable areas;
        int selectedRecord = 0;
        public frm_warehouse_name_setup()
        {
            InitializeComponent();
        }

        private async void frm_warehouse_name_setup_Load(object sender, EventArgs e)
        {
            authorityChecker();
            await FetchLatestTables("initial");
            isInitialLoad = true;
            if (!hasAuthority)
            {
                MessageBox.Show(
                "Please use an account that is at the Inventory Manager level or above to make changes",
                "Lack of Authority",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
            }

            if (this.WarehouseNameTable.Rows.Count > 0)
            {
                setExistingWarehouseToList();
                currentIndex = this.WarehouseNameTable.Rows.Count - 1;
                await GetData();
                SetCurrentUser();
                NextAndPrevBtn();

                int useTypeColIndex = -1;
                for (int j = 0; j < dg_areas.Columns.Count; j++)
                {
                    if (dg_areas.Columns[j].Name == "use_type")
                    {
                        useTypeColIndex = j;
                        break;
                    }
                }
                if (useTypeColIndex >= 0)
                {
                    for (int i = 0; i < dg_areas.Rows.Count; i++)
                    {
                        if (dg_areas.Rows[i].IsNewRow) continue;

                        dg_areas.CurrentCell = dg_areas.Rows[i].Cells[useTypeColIndex];
                        dg_areas.BeginEdit(true);
                    }
                }

                BtnToggleEnabillity("initial");
                Cursor.Current = Cursors.Default;
                return;
            }

            cmb_warehouse_manager.Items.Clear();

            FreshTablePopUp();

            PopulateManagerList();

            BtnToggleEnabillity("empty");
            btn_new.PerformClick();
        }

        List<string> existingWarehouse = new List<string>();
        private async void setExistingWarehouseToList()
        {
            if (isLocalContentChanged)
            {
                await FetchLatestTables();
            }

            warehousename = JsonHelper.ToDataTable(warehouseDataList.warehouse_name);
            existingWarehouse.Clear();
            foreach (DataRow row in warehousename.Rows)
            {
                if (row["name"] != DBNull.Value)
                    existingWarehouse.Add(row["name"].ToString());
            }

        }
        private async Task FetchLatestTables(string usedBy = "") //change this if i-l-load yung changed table
        {
            usedBy = usedBy.ToLower();

            var WarehouseNameData = await WarehouseNameServices.GetWarehouseInfos();

            if (usedBy == "warehousenametable" || usedBy == "initial") this.WarehouseNameTable = JsonHelper.ToDataTable(WarehouseNameData.warehouse_name); 
            else if (usedBy == "addressdatatable") addressDataTable = JsonHelper.ToDataTable(WarehouseNameData.warehouse_address);
            else if (usedBy == "parentinfo") parentInfo = JsonHelper.ToDataTable(WarehouseNameData.warehouse_name); 

            var warehouseDataListData = await RequestToApi<ApiResponseModel<WarehouseList>>.Get(ENUM_ENDPOINT.WAREHOUSE);
            warehouseDataList = warehouseDataListData.Data; 

            isLocalContentChanged = false;
            TableContentChanged.WarehouseUseType = false;
        }
         
        DataTable addressDataTable;
        public async Task GetData()
        {
            eventsuppressor = true;
            DG_eventsuppressor = true;
            dashCounter = 0;
            Cursor.Current = Cursors.WaitCursor;

            if (isLocalContentChanged)
            {
                await FetchLatestTables();
                isLocalContentChanged = false;
            }

            warehousename = JsonHelper.ToDataTable(warehouseDataList.warehouse_name);
            address = JsonHelper.ToDataTable(warehouseDataList.warehouse_address);
            areas = JsonHelper.ToDataTable(warehouseDataList.warehouse_area);

            //Fetching data using dedicated functions
            await FetchLatestTables("WarehouseNameTable");
            await FetchLatestTables("addressDataTable");

            if (WarehouseNameTable == null || WarehouseNameTable.Rows.Count <= 0)
            {
                BtnToggleEnabillity("empty");
                NextAndPrevBtn();
                return;
            }

            NextAndPrevBtn();

            Panel[] panelHead = { pnl_head };
            Panel[] panelAddress = { pnl_address };

            DataView dataviewAddress = new DataView(addressDataTable);

            if (currentIndex < 0 || WarehouseNameTable == null)
            {
                FreshTablePopUp();
                BtnToggleEnabillity("empty");
                return;
            }

            OutOfBoundChecker();
            selectedRecord = currentIndex;

            Helpers.BindControls(panelHead, WarehouseNameTable, currentIndex);
            Helpers.BindControls(panelAddress, dataviewAddress.Table, currentIndex);

            FetchWarehouseAreas(); // dg binder

            string warehouseManager = WarehouseNameTable.Rows[currentIndex]["warehouse_manager"].ToString();
            chk_is_inactive.Checked = await currentIsInactive();

            PopulateManagerList(warehouseManager);

            NextAndPrevBtn();
            updateCurrentWarehouse();

            Cursor.Current = Cursors.Default;
            eventsuppressor = false;
            DG_eventsuppressor = false;
            tabControl1.Enabled = true;
        }


        static bool DG_eventsuppressor = false;
        private async void FetchWarehouseAreas() 
        {
            DG_eventsuppressor = true;
            DataView dataViewWarehouseAreas = new DataView(areas);

            try
            {
                bindingSource1.DataSource = null;
                if (warehousename.Rows.Count > selectedRecord)
                {
                    string selectedId = warehousename.Rows[selectedRecord]["id"].ToString();
                    dataViewWarehouseAreas.RowFilter = $"warehouse_name_id = '{selectedId}'";

                    if (dataViewWarehouseAreas.Count > 0)
                    {
                        bindingSource1.DataSource = dataViewWarehouseAreas;
                        dg_areas.DataSource = bindingSource1;
                    }
                    else
                    {
                        bindingSource1.DataSource = null;
                        dg_areas.DataSource = null;
                    }
                }
                 
                for (int i = 0; i < dataViewWarehouseAreas.Count; i++)
                {
                    PopulateUsetype(new DataGridViewCellEventArgs(1, i), i);
                }
                 
                //apply colors based on bg_color from WarehouseUseType
                var useTypeTable = await WarehouseUseTypeServices.GetDataTable();
                foreach (DataGridViewRow row in dg_areas.Rows)
                {
                    if (row.IsNewRow) continue;

                    string useTypeValue = row.Cells["dg_use_type"]?.Value?.ToString();
                    if (string.IsNullOrWhiteSpace(useTypeValue))
                        continue;

                    var matches = useTypeTable.AsEnumerable()
                        .Where(r => string.Equals(r["name"]?.ToString(), useTypeValue, StringComparison.OrdinalIgnoreCase));

                    foreach (var match in matches)
                    {
                        string colorName = match["bg_color"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(colorName))
                        {
                            Color bg = Color.FromName(colorName);
                            row.Cells["dg_use_type"].Style.BackColor = bg;
                            row.Cells["dg_use_type"].Style.ForeColor =
                                bg.GetBrightness() < 0.5f ? Color.White : Color.Black;
                        }
                    }
                }
            }
            finally
            {
                DG_eventsuppressor = false;
            }
        }



        static bool shouldShowPopup = true;
        private void FreshTablePopUp()
        {
            if (shouldShowPopup)
            {
                MessageBox.Show(
                    "      No warehouse data found. Please add a warehouse",
                    "New Warehouse"
                );
            }
            shouldShowPopup = !shouldShowPopup;
        }
        DataTable parentInfo;
        private async Task<bool> currentIsInactive()
        {
            await FetchLatestTables("parentInfo");
            DataTable warehouseNameIsInactiveCheckerTable = parentInfo;
            return warehouseNameIsInactiveCheckerTable.Rows[currentIndex]["is_inactive"].ToString().ToLower() == "true" ||
                warehouseNameIsInactiveCheckerTable.Rows[currentIndex]["is_inactive"].ToString().ToLower() == "1";
        } //to accepts binary or boolean

        private string SetCurrentUser()
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
            string userCredential = userFullName + " | " + userPosition;

            return userCredential;
        }

        static bool isPopulatingManager = false;
        private async void PopulateManagerList(string allPurposeString = "")
        {
            if (!hasAuthority)
                return;

            isPopulatingManager = true;
            string action = allPurposeString;
            string currentUser = SetCurrentUser();
            if (action.Equals("new"))
            {
                cmb_warehouse_manager.Items.Add(currentUser);
                cmb_warehouse_manager.SelectedIndex =
                    cmb_warehouse_manager.FindStringExact(currentUser);
                return;
            }

            cmb_warehouse_manager.Items.Clear();

            DataTable users = await ListOfUsersServices.GetAsDatatable();
            //string currentUserPosition = CacheData.CurrentUser.position.ToString().ToLower();

            List<string> managerList = new List<string>();
            int Counter = 0;
            foreach (DataRow userRow in users.Rows)
            {
                string firstName = userRow["first_name"].ToString();
                string lastName = userRow["last_name"].ToString();

                if ((lastName + " " + firstName).Length > 20)
                {
                    firstName = firstName.Length > 12 ? firstName.Split()[0] : firstName;
                    lastName = lastName.Length > 12 ? lastName.Split()[0] : lastName;
                }

                string fullName = $"{firstName} {lastName}";
                string position = userRow["position"].ToString().ToLower();

                if (position.Contains("admin") || position.Contains("manager")) //filter as manager (not just from inventory)
                {
                    string displayText = $"{fullName} | {userRow["position"]}";
                    managerList.Add(displayText);
                    Counter++;
                }
            }

            if (cmb_warehouse_manager.Items.Count == 0 ||
                cmb_warehouse_manager.Items.Count != Counter)
            {
                cmb_warehouse_manager.Items.Clear();
                cmb_warehouse_manager.Items.AddRange(managerList.ToArray());
            }

            if ((WarehouseNameTable != null && WarehouseNameTable.Rows.Count > 0) &&
                (currentUserPosition.ToLower().Contains("manager") ||
                 currentUserPosition.ToLower().Contains("admin")))
            {
                int index;
                if (string.IsNullOrEmpty(
                    WarehouseNameTable.Rows[currentIndex]["warehouse_manager"].ToString())
                    )
                {
                    index = cmb_warehouse_manager.FindStringExact(SetCurrentUser());
                }
                else
                {
                    index = cmb_warehouse_manager.FindStringExact(WarehouseNameTable.Rows[currentIndex]["warehouse_manager"].ToString());
                    //cmb_warehouse_manager.SelectedItem = headDataTable.Rows[currentIndex]["warehouse_manager"].ToString(); 
                    //cmb_warehouse_manager.SelectedIndex = -1; 
                }
                //await Task.Delay(20);
                cmb_warehouse_manager.SelectedIndex = index;
            }
            else cmb_warehouse_manager.Enabled = false;

            isPopulatingManager = false;
        }


        private static bool hasAuthority = false;
        private void authorityChecker()
        {
            Console.WriteLine("\ncurrentUserPosition: " + currentUserPosition);
            if (currentUserPosition.ToString().Contains("admin") || currentUserPosition.ToString().Contains("manager"))
            { //please fix manager (should be specific like inventory or something)
                hasAuthority = true;
                cmb_warehouse_manager.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb_warehouse_manager.Items.Clear();
            }
            else
            {
                hasAuthority = false;
            }
            if (!currentUserPosition.ToString().Contains("admin")) cmb_warehouse_manager.Tag = "no_edit";
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            cmb_warehouse_manager.Items.Clear();
            PopulateManagerList("new");
            tabControl1.SelectedIndex = 0;
            toolTip1.SetToolTip(cmb_warehouse_manager, "Warehouse should be named before changing the manager");
            

            Helpers.ResetControls(pnl_head);
            Helpers.ResetControls(pnl_address);
            Helpers.ResetControls(pnl_areas);

            BtnToggleEnabillity("new");

            cmb_warehouse_manager.SelectedItem = SetCurrentUser();
        }

        private void BtnToggleEnabillity(string action, bool initial = false)
        {
            action = action.ToLower();
            btn_search.Enabled = true;

            tabControl1.Enabled = string.IsNullOrEmpty(txt_id.Text) ? false : true;

            if (action == "initial")
            {
                Helpers.SetPanelToReadOnly(pnl_head, true);
                Helpers.SetPanelToReadOnly(pnl_address, true);
                Helpers.SetPanelToReadOnly(pnl_areas, true);

                if (!hasAuthority) Helpers.SetReadOnlyControl(dg_areas, true);

                cmb_warehouse_manager.Enabled = false;
            }

            if (action == "search")
            {
                tmplbl.Visible = !tmplbl.Visible;
                cmb_search.Visible = !cmb_search.Visible; 
            }

            if (action == "edit" || action == "new")
            {
                cmb_warehouse_manager.Enabled = currentUserPosition.ToLower().ToString().Contains("admin");

                Helpers.SetPanelToReadOnly(pnl_head, false);
                Helpers.SetPanelToReadOnly(pnl_address, false);
                Helpers.SetPanelToReadOnly(pnl_areas, false);

                btn_cancel.Visible = true;
                btn_save.Visible = true;
                btn_new.Visible = true;
                btn_edit.Visible = true;

                btn_delete.Visible = false;

                btn_search.Visible = false;
                txt_code.ReadOnly = true;

                btn_next.Enabled = false;
                btn_prev.Enabled = false;
            }
            if (action == "edit")
            {
                btn_new.Visible = false;
                tabControl1.Enabled = true;
                if (string.IsNullOrEmpty(txt_id.Text) && tabControl1.SelectedIndex == 1)
                {
                    Helpers.SetPanelToReadOnly(pnl_areas, true);
                }
            }
            else if (action == "new")
            {
                btn_edit.Visible = false;

                chk_is_inactive.Checked = false;

                if (dg_areas.DataSource != null)
                {
                    dg_areas.DataSource = null;  // or clear the actual data source if needed
                }
                else
                {
                    dg_areas.Rows.Clear();
                }
            }

            if (action == "save" || action == "cancel")
            {
                toolTip1.SetToolTip(cmb_warehouse_manager, "");
                Helpers.SetPanelToReadOnly(pnl_head, true);
                Helpers.SetPanelToReadOnly(pnl_address, true);
                if (pnl_areas.Visible)
                {
                    Helpers.SetPanelToReadOnly(pnl_areas, true);
                }


                btn_delete.Visible = true;
                btn_delete.Enabled = true;
                btn_edit.Visible = true;
                btn_edit.Enabled = true;
                btn_new.Visible = true;
                btn_new.Enabled = true;
                btn_search.Visible = true;
                btn_search.Enabled = true;

                btn_cancel.Visible = false;

                cmb_warehouse_manager.Enabled = false;
                btn_save.Visible = false;

                btn_save.Text = "Save";
                NextAndPrevBtn();
                return;
            }

            if (!hasAuthority)
            {
                Helpers.SetPanelToReadOnly(pnl_head, true);
                Helpers.SetPanelToReadOnly(pnl_address, true);
                Helpers.SetReadOnlyControl(dg_areas, true);

                btn_new.Enabled = false;
                btn_delete.Enabled = false;
                btn_edit.Enabled = false;
                return;
            }

            if (action == "empty")
            {
                btn_new.Visible = true;
                btn_new.Enabled = true;
                btn_delete.Visible = false;
                btn_edit.Visible = false;
                btn_search.Visible = false;

                pnl_address.Enabled = true;
                pnl_areas.Enabled = false; //doesnt matter

                btn_save.Visible = true;
                btn_cancel.Visible = false;

                Helpers.SetPanelToReadOnly(pnl_head, false);
                Helpers.SetPanelToReadOnly(pnl_address, false);
                if (pnl_areas != null) Helpers.SetPanelToReadOnly(pnl_areas, false);

                return;
            }

            pnl_head.Enabled = true;
            pnl_address.Enabled = true;
            pnl_areas.Enabled = true;


        }

        DataTable WarehouseNameTable { get; set; } = new DataTable(); 
        private async void NextAndPrevBtn(string goIndex = "")
        {
            if (string.IsNullOrEmpty(txt_id.Text)) return;

            if (isPopulatingUsetype || isPopulatingManager)
            {
                await Task.Delay(5);
                NextAndPrevBtn(goIndex);
                return;
            }

            if (goIndex.Equals("next")) btn_next.Enabled = false;
            else if (goIndex.Equals("prev")) btn_prev.Enabled = false;

            OutOfBoundChecker();
            int lastIndex = WarehouseNameTable.Rows.Count - 1;

            if (!string.IsNullOrEmpty(goIndex))
            {
                if (goIndex.Equals("next") && currentIndex > 0)
                    currentIndex--;
                else if (goIndex.Equals("prev") && currentIndex < lastIndex)
                    currentIndex++;
            }

            if (!string.IsNullOrEmpty(goIndex))
                await GetData();

            updateCurrentWarehouse();
        }


        static string currentWarehouseName;
        static int currentIndex = 0; //1 based index

        private void updateCurrentWarehouse() //used for visuals 
        {
            var data = Helpers.GetControlsValues(pnl_head);
            currentWarehouseName = "'" + txt_name.Text + "'";
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            btn_delete.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            if (await ConfirmDeleteAsync())
            {
                await DeleteWarehouse();
            }

            btn_delete.Enabled = true;
            BtnToggleEnabillity("delete");
            Cursor.Current = Cursors.Default;
        }
        private async Task<bool> ConfirmDeleteAsync()
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete {currentWarehouseName}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            return await Task.FromResult(result == DialogResult.Yes);
        }
        private async Task DeleteWarehouse()
        {
            try
            {
                updateCurrentWarehouse();
                string recentDeletedWarehouse = currentWarehouseName;

                Dictionary<string, dynamic> warehouseData = new Dictionary<string, dynamic>
                {
                    ["warehouse_name"] = new Dictionary<string, dynamic>
                    {
                        { "id", int.Parse(txt_id.Text) }
                    }
                };

                bool isSuccess = await WarehouseNameServices.Delete(warehouseData);

                if (isSuccess)
                {
                    HandleSuccessfulDeletion(recentDeletedWarehouse);
                }
                else
                {
                    Helpers.ShowDialogMessage("error", $"Failed to delete {recentDeletedWarehouse}");
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"An error occurred while deleting the warehouse:\n{ex.Message}");
            }
        }
        private async void HandleSuccessfulDeletion(string deletedWarehouseName)
        {
            if (currentIndex < 0 || WarehouseNameTable == null || currentIndex >= WarehouseNameTable.Rows.Count)
            {
                BtnToggleEnabillity("empty");
            }

            Helpers.ResetControls(pnl_head);
            Helpers.ResetControls(pnl_address);
            Helpers.ResetControls(pnl_areas);

            Helpers.ShowDialogMessage("success", $"{deletedWarehouseName} deleted successfully");

            setExistingWarehouseToList();

            NextAndPrevBtn("prev");
            await GetData();
        }


        private async void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_id.Text))
                currentIndex = WarehouseNameTable.Rows.Count;

            if (tabControl1.SelectedIndex == 1)
            {
                dg_areas.EndEdit(); // commits/save dg
            }

            await saveButtonFunction();
        }


        private async Task saveButtonFunction()
        {
            NextAndPrevBtn(); 
            if (!ValidateInput())
            {
                BtnToggleEnabillity("new"); 
                return;
            } 

            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
             
            ApiResponseModel response = isNewRecord
                ? await CreateNewWarehouse()
                : await UpdateWarehouse();
             
            if (response != null)
            {
                string message = response.Success
                    ? (isNewRecord
                        ? $"{currentWarehouseName} Added Successfully"
                        : $"{currentWarehouseName} Updated Successfully")
                    : (isNewRecord
                        ? $"Failed To Add Warehouse\n{response.message}"
                        : $"Failed To Update {currentWarehouseName}\n{response.message}");

                Console.WriteLine("\nRESPONSE FOR PROCESS: " + (isNewRecord ? "INSERT" : "UPDATE") + " IS: " + response.Success + " | " + message);

                if (response.Success)
                {
                    setExistingWarehouseToList();
                    BtnToggleEnabillity("save");
                    NextAndPrevBtn();
                    TableContentChanged.WarehouseName = true;
                    isLocalContentChanged = true;

                    await GetData();
                }
            }
        }
        private static bool isLocalContentChanged = false;
        private bool ValidateInput()
        {
            string errorMessage = string.IsNullOrEmpty(txt_name.Text)
                ? "Warehouse's 'Name' is empty"
                : "";

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Helpers.ShowDialogMessage("error", errorMessage);
                return false;
            }

            return true;
        }
        private async Task<ApiResponseModel> UpdateWarehouse()
        {
            var nameData = Helpers.GetControlsValues(pnl_head);
            var addressData = Helpers.GetControlsValues(pnl_address);

            nameData["warehouse_manager"] = cmb_warehouse_manager.Text;
            nameData["is_inactive"] = chk_is_inactive.Checked;

            updateCurrentWarehouse();

            Dictionary<string, dynamic> WarehouseData = new Dictionary<string, dynamic>
            {
                ["warehouse_name"] = nameData,
                ["warehouse_address"] = addressData
            };

            var response = await WarehouseNameServices.Update(WarehouseData);
            return response;
        }
        private async Task<ApiResponseModel> CreateNewWarehouse()
        {
            if (existingWarehouse.Contains(txt_name.Text))
            {
                Helpers.ShowDialogMessage("error", $"Warehouse '{txt_name.Text}' already exists");
                txt_name.Text = "";
                return new ApiResponseModel { Success = false, message = "Duplicate warehouse name" };
            }

            var nameData = Helpers.GetControlsValues(pnl_head);
            var addressData = Helpers.GetControlsValues(pnl_address);

            nameData["warehouse_manager"] = cmb_warehouse_manager.Text;
            nameData["is_inactive"] = chk_is_inactive.Checked;

            nameData.Remove("id");
            addressData.Remove("warehouse_name_id");

            updateCurrentWarehouse();

            Dictionary<string, dynamic> WarehouseData = new Dictionary<string, dynamic>
            {
                ["warehouse_name"] = nameData,
                ["warehouse_address"] = addressData
            };

            txt_code.Clear();
            var response = await WarehouseNameServices.Insert(WarehouseData);

            currentIndex = WarehouseNameTable.Rows.Count;
            if (response.Success)
            {
                await GetData();
            }
            return response;
        }



        private async void btn_cancel_Click(object sender, EventArgs e)
        {
            if (!(WarehouseNameTable == null || WarehouseNameTable.Rows.Count == 0))
            {
                await GetData();

                NextAndPrevBtn();
                BtnToggleEnabillity("cancel"); 
            }
            else
            {
                BtnToggleEnabillity("empty");
            }
        }

        private void OutOfBoundChecker()
        {
            //await Task.Delay(50);
            int lastIndex = WarehouseNameTable.Rows.Count - 1;

            if (WarehouseNameTable.Rows.Count != 0)
            {
                if (currentIndex >= lastIndex) currentIndex = lastIndex;
                if (currentIndex < 0) currentIndex = 0;

                btn_prev.Enabled = currentIndex < lastIndex;
                btn_next.Enabled = currentIndex > 0;
            }
        }

        private async void btn_next_Click(object sender, EventArgs e)
        {
            btn_prev.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            OutOfBoundChecker();

            while (isLoading()) //recursion watch for err
            {
                await Task.Delay(5);
                DG_eventsuppressor = false;
            }

            NextAndPrevBtn("next");
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            btn_next.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            OutOfBoundChecker();

            if (isLoading())
            {
                DG_eventsuppressor = false;
                return;
            }

            NextAndPrevBtn("prev");
        }

        private void btn_edit_Click(object sender, EventArgs e) //remove this ass
        { 
            BtnToggleEnabillity("edit");
        }

        private static string AreaCodeGenerator(
            string useType, 
            string zone, 
            string area, 
            string rack, 
            string level, 
            string binsInput)
        {
            if (string.IsNullOrWhiteSpace(useType)) return "";
            List<string> parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(zone)) parts.Add(zone);
            if (!string.IsNullOrWhiteSpace(area)) parts.Add(area);
            if (!string.IsNullOrWhiteSpace(rack)) parts.Add(rack);
            if (!string.IsNullOrWhiteSpace(level)) parts.Add(level);

            //if ((string.IsNullOrWhiteSpace(area)
            //        && string.IsNullOrWhiteSpace(rack)
            //        && string.IsNullOrWhiteSpace(level)) 
            //    && (string.IsNullOrWhiteSpace(binsInput) 
            //        || binsInput == "0") 
            //    && !string.IsNullOrWhiteSpace(area)) 
            //    parts.Add("OPEN");
            if (string.IsNullOrWhiteSpace(rack))
            {
                parts.Add("OPEN");
            }

            if (int.TryParse(binsInput, out int bins) && bins > 0)
            {
                if (bins > 1)
                    parts.Add($"B1 TO B{bins}");
                else
                    parts.Add($"B{bins}");
            }

            return string.Join("-", parts);
        }

        private void UpdateLocationCodeInRow(DataGridViewRow row)
        { 
            string useType = row.Cells["dg_use_type"].Value?.ToString().ToUpper() ?? "";
            string binsInput = row.Cells["dg_bins"].Value?.ToString() ?? "";

            List<string> cellKeys = new List<string> { "dg_zone", "dg_area", "dg_rack", "dg_level" };
            Dictionary<string, string> values = new Dictionary<string, string>();
             
            foreach (var key in cellKeys)
            {
                values[key] = row.Cells[key].Value?.ToString().ToUpper() ?? "";
            }
             
            if (string.IsNullOrEmpty(useType))
            {
                foreach (var key in cellKeys)
                    row.Cells[key].Value = "";
            }
            else
            {
                bool gapDetected = false;
                bool foundEmpty = false;

                foreach (var key in cellKeys)
                {
                    string cellValue = values[key]?.Trim();

                    if (string.IsNullOrEmpty(cellValue))
                    {
                        foundEmpty = true;
                        values[key] = "";
                        row.Cells[key].Value = "";
                    }
                    else if (foundEmpty && !string.IsNullOrEmpty(cellValue))
                    { 
                        gapDetected = true;
                        values[key] = "";
                        row.Cells[key].Value = "";
                    }
                }

                if (gapDetected)
                {
                    Helpers.ShowDialogMessage("error", "Prior fields are required before going forward");
                }

            }

            bool isBinsEmpty = string.IsNullOrWhiteSpace(binsInput);
            bool isValidBins = int.TryParse(binsInput, out int bins);

            if (isBinsEmpty || (!isValidBins && binsInput != "") || bins == 0)
            {
                row.Cells["dg_bins"].Value = "";
                bins = 0; // Reset bins to a safe default
            }
             
            string areaCode = AreaCodeGenerator(
                useType, 
                values["dg_zone"], 
                values["dg_area"], 
                values["dg_rack"], 
                values["dg_level"], 
                bins.ToString());

            if (!string.IsNullOrWhiteSpace(areaCode))
            {
                row.Cells["dg_location_code"].Value = areaCode;
            } 
        }

        static bool isPopulatingUsetype = true;
        private bool isSavingRow = false;
        string columnName = "";
        private async void dg_areas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            await Task.Delay(10); 

            var grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            columnName = grid.Columns[e.ColumnIndex].Name.ToString();
 
            if (columnName == "dg_location_code" || columnName == "dg_use_type" || columnName == "dg_notes")
            {
                UpdateWarehouseAreas(e.RowIndex); 
            }
        } 

        private async void UpdateWarehouseAreas(int currentRow) //single row saving (because it is on text change(location code need to be constantly update))
        {
            if (isLoading()) return;
            bool isNewRecord;
            var dgAreas = Helpers.ConvertDataGridViewToDataTable(dg_areas);
            //convert to this if have time
            var dgAreasData = Helpers.ConvertDataTableToDictionary(dgAreas);

            //often may err d2
            isNewRecord = 
                currentRow < 0 
                || currentRow >= dgAreasData.Count 
                || !dgAreasData[currentRow]?.ContainsKey("id") == true;

            ApiResponseModel response;
            while (dgAreasData.Count <= currentRow)
                dgAreasData.Add(new Dictionary<string, object>());

            if (isNewRecord)
            {
                dgAreasData[currentRow]["warehouse_name_id"] = int.Parse(txt_id.Text);
                response = await WarehouseAreaServices.Insert(dgAreasData[currentRow]);
                await GetData();
                //currentIndex = headDataTable.Rows.Count;
            }
            else
            {
                //response = await WarehouseAreaServices.Update(data);
                response = await WarehouseAreaServices.Update(dgAreasData[currentRow]);
            }

            currentWarehouseName = $"'{txt_name.Text}'";
            if (response != null)
            {
                string message = response.Success
                    ? (isNewRecord ? $"{currentWarehouseName} Added Successfully" : $"{currentWarehouseName} Updated Successfully")
                    : (isNewRecord ? $"Failed To Add DG Row\n{response.message}" : $"Failed To Update {currentWarehouseName}\n{response.message}");
            }

            //NextAndPrevBtn();
            //MessageBox.Show(
            //jsonContentVisualizer(JsonConvert.SerializeObject(data)),
            //  "Debug Popup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //FetchDataGridViewAreas();
            DG_eventsuppressor = false;
        } 
        private void dg_areas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dg_areas.Columns[e.ColumnIndex].Name == "dg_location_code" && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.SelectionForeColor = Color.LightPink;
            }
            else return; //no need specific formating (auto gen = red) for other cell yet
        }

        private void dg_areas_DataError(object sender, DataGridViewDataErrorEventArgs e) //because the content of binded data in UseType is not in the items list., I think
        { //pucha para walang tigil tigil di nmn fatal error
            try
            {
                if (dg_areas.Columns[e.ColumnIndex].Name == "dg_use_type")
                    return;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Column access error: " + ex.Message);
                return;
            }
            e.ThrowException = false; //no error throw :(
        }

        private async void PopulateUsetype(DataGridViewCellEventArgs e, int rowIndex)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DataGridViewComboBoxCell cell;

                int colIndex = dg_areas.Columns["dg_use_type"].Index;
                var useTypeTable = await WarehouseUseTypeServices.GetDataTable();

                foreach (DataGridViewRow row in dg_areas.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Get use_type value from the grid
                    string useTypeValue = row.Cells["dg_use_type"]?.Value?.ToString();
                    if (string.IsNullOrWhiteSpace(useTypeValue))
                        continue;

                    // Find matching row in useTypeTable
                    var matches = useTypeTable.AsEnumerable()
                        .Where(r => string.Equals(r["name"]?.ToString(), useTypeValue, StringComparison.OrdinalIgnoreCase));

                    foreach (var match in matches)
                    {
                        string colorName = match["bg_color"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(colorName))
                        {
                            Color bg = Color.FromName(colorName);
                            row.Cells["dg_use_type"].Style.BackColor = bg;
                            row.Cells["dg_use_type"].Style.ForeColor =
                                bg.GetBrightness() < 0.5f ? Color.White : Color.Black;
                        }
                    }
                }


                if (rowIndex < 0 || colIndex < 0 || rowIndex >= dg_areas.Rows.Count || dg_areas.Rows[rowIndex].IsNewRow)
                    return;

                cell = dg_areas.Rows[rowIndex].Cells[colIndex] as DataGridViewComboBoxCell;

                if (cell == null || cell.Items.Count > 0)
                    return;

                isPopulatingUsetype = true;


                var useTypeList = useTypeTable
                    .AsEnumerable()
                    .Select(r => r.Field<string>("name"))
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Distinct()
                    .ToArray();

                var currentValue = cell.Value?.ToString();

                cell.Items.Clear();
                //cell.Items.Add("n/a"); // blank default ??
                cell.Items.AddRange(useTypeList);

                if (!string.IsNullOrWhiteSpace(currentValue) && useTypeList.Contains(currentValue))
                    cell.Value = currentValue;
                else
                    cell.Value = "";
            } 
            finally
            {
                isPopulatingUsetype = false;
                Cursor.Current = Cursors.Default;
            }
        }

        string beforeEditCellValue = "";
        private void dg_areas_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 
                || e.RowIndex > dg_areas.Rows.Count 
                //|| eventsuppressor
                || isLoading()) return;
            beforeEditCellValue = dg_areas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); 
            if (dg_areas.Columns[e.ColumnIndex].Name == "dg_use_type" /*&& e.RowIndex >= 0*/)
            {
                if (!btn_cancel.Visible) return; //to detect if edit or adding new 
                PopulateUsetype(e, e.RowIndex);

                var value = dg_areas.CurrentCell.Value?.ToString();

                Rectangle cellDisplayRect = dg_areas.GetCellDisplayRectangle(
                    dg_areas.CurrentCell.ColumnIndex,
                    dg_areas.CurrentCell.RowIndex,
                    false
                );

                Point cellLocationOnForm = dg_areas.PointToScreen(cellDisplayRect.Location);
                Point locationRelativeToForm = this.PointToClient(cellLocationOnForm);
                ToolTip tip = new ToolTip();

                if (!string.IsNullOrEmpty(value))
                {
                    tip.Show("Double Click to Change Value", this,
                        locationRelativeToForm.X + 10,
                        locationRelativeToForm.Y - 15,
                        750);
                }
                else
                { 
                    tip.Show("Double Click to Add Value", this,
                        locationRelativeToForm.X + 10,
                        locationRelativeToForm.Y - 15,
                        750);
                }
            }
        }

        static bool eventsuppressor = false;
        static bool isRowDeletionCommittable = false;
        private void dg_areas_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isLoading() || !isRowDeletionCommittable) return;
            var dgAreas = Helpers.ConvertDataGridViewToDataTable(dg_areas);
            if (dgAreas.Rows.Count < 0) return;

            DeleteWarehouseAreaRow(e.RowIndex); 
        }
        
        private async void DeleteWarehouseAreaRow(int currentRow)
        {
            if (idForDeletion < 0) return;
            DG_eventsuppressor = true;
            string prevDeletedRow = currentRowDetails;
            try
            {
                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
                Dictionary<string, dynamic> innerJson = new Dictionary<string, dynamic>();
                //int.TryParse(dg_areas.Rows[currentRow].Cells["id"]?.Value?.ToString(), out int idForDeletion);

                if (idForDeletion == 0 || currentRow < 1 || currentRow > dg_areas.Rows.Count - 1)
                {
                    return;
                } 
                data["id"] = idForDeletion;

                bool isSuccess = await WarehouseAreaServices.Delete(data);

                Helpers.ShowDialogMessage((isSuccess ? "success" : "error")
                    , $"row of {prevDeletedRow.Replace("details:\n", "")} was deleted");
                Console.WriteLine("Row deletion is success: " + isSuccess);
            }
            finally
            {
                FetchWarehouseAreas();
                //await GetData();
                idForDeletion = 0;
                DG_eventsuppressor = false;
                isRowDeletionCommittable = false;
            } 
        }
        static string currentRowDetails = "";
        static int idForDeletion = 0;
        private bool isLoading()
        {
            Console.WriteLine("\nisLoading sleep is triggered\nValue: " +
                (DG_eventsuppressor || eventsuppressor || isPopulatingManager || isPopulatingUsetype));
            return (DG_eventsuppressor || eventsuppressor || isPopulatingManager || isPopulatingUsetype);
        }

        private void dg_areas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= 0 
                || e.ColumnIndex < 0
                || e.RowIndex >= dg_areas.Rows.Count
                || e.ColumnIndex >= dg_areas.Columns.Count
                || dg_areas.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly
                || e.ColumnIndex >= dg_areas.Columns.Count
                ) return;

            columnName = dg_areas.Columns[e.ColumnIndex].Name.ToString();
            if (columnName == "dg_location_code" && e.RowIndex >= 0)
            {
                var value = dg_areas.CurrentCell.Value?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    Clipboard.SetText(value); // Copy text

                    Rectangle cellDisplayRect = dg_areas.GetCellDisplayRectangle(
                        dg_areas.CurrentCell.ColumnIndex,
                        dg_areas.CurrentCell.RowIndex,
                        false
                    );

                    Point cellLocationOnForm = dg_areas.PointToScreen(cellDisplayRect.Location);
                    Point locationRelativeToForm = this.PointToClient(cellLocationOnForm);

                    ToolTip tip = new ToolTip();
                    tip.Show("Text copied to clipboard!", this,
                        locationRelativeToForm.X + 10,
                        locationRelativeToForm.Y - 15,
                        1000);
                }
            }
            var dg = sender as DataGridView;
            int rowIndex = e.RowIndex;
            if ((columnName == "dg_zone" 
                || columnName == "dg_area"
                || columnName == "dg_rack"
                || columnName == "dg_level")
                && e.RowIndex >= 0)
            {

                for (int colIndex = e.ColumnIndex; colIndex > 0; colIndex--)
                {
                    var currentCell = dg.Rows[rowIndex].Cells[colIndex];
                    var aboveCell = dg.Rows[rowIndex - 1].Cells[colIndex];

                    // Check if the first cell (use type) is empty
                    if (dg.Rows[rowIndex].Cells[0].Value == null ||
                        string.IsNullOrWhiteSpace(dg.Rows[rowIndex].Cells[0].Value.ToString()))
                    {
                        // Set focus to first column and open ComboBox edit mode
                        dg.CurrentCell = dg.Rows[rowIndex].Cells[0];
                        dg.BeginEdit(true); // Tries to open dropdown

                        var toolTip = new ToolTip();
                        var cellDisplayRect = dg.GetCellDisplayRectangle(0, rowIndex, false);
                        Point toolTipLocation = dg.PointToScreen(new Point(cellDisplayRect.X, cellDisplayRect.Y));

                        toolTip.Show("usetype must have value", dg, dg.PointToClient(toolTipLocation), 10000); //1k = 1sec
                         
                        dg.CurrentCell.Value = null;
                         
                        dg.CurrentCell = dg.CurrentCell;
                        dg.BeginEdit(true);

                        return; // Exit the loop early since requirement isn't met
                    }

                    // Copy regular cell value from above if empty
                    if (currentCell.Value == null || string.IsNullOrWhiteSpace(currentCell.Value.ToString()))
                    {
                        currentCell.Value = aboveCell.Value;
                        UpdateWarehouseAreas(e.RowIndex);
                    } 
                }

            }
        }

        private Dictionary<string, Color> mergeGroupColors = new Dictionary<string, Color>();
        private Random rand = new Random();
        private void dg_areas_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.ColumnIndex == 0 && e.RowIndex >= 0 && e.RowIndex < dg_areas.Rows.Count /*- 1*/)
            //{
            //    var currentValue = dg_areas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";

            //    if (string.IsNullOrEmpty(currentValue)) return;

            //    Color bgColor = Color.FromArgb(255, rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
            //    //Color textColor = Color.FromArgb(255, bgColor.R - 130, bgColor.G - 130, bgColor.B - 130);

            //    // Assign a consistent random color per unique value
            //    if (!mergeGroupColors.ContainsKey(currentValue))
            //    {
            //        mergeGroupColors[currentValue] = bgColor;
            //    }

            //    Color groupColor = mergeGroupColors[currentValue];

            //    using (SolidBrush backColorBrush = new SolidBrush(groupColor))
            //    using (SolidBrush gridBrush = new SolidBrush(dg_areas.GridColor))
            //    using (Pen gridLinePen = new Pen(gridBrush))
            //    {
            //        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

            //        bool drawValue = true;

            //        if (e.RowIndex > 0)
            //        {
            //            string aboveValue = dg_areas.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value?.ToString();
            //            if (currentValue == aboveValue)
            //            {
            //                drawValue = false; // simulate merged row
            //            }
            //            if (e.RowIndex == dg_areas.Rows.Count - 1)
            //            {

            //            }
            //        }

            //        int bgColorTotal = bgColor.R + bgColor.G + bgColor.B;
            //        if (drawValue)
            //        {
            //            TextRenderer.DrawText(
            //                e.Graphics,
            //                currentValue,
            //                e.CellStyle.Font,
            //                e.CellBounds,
            //                //textColor,
            //                bgColorTotal <= 400 ? Color.White : Color.Black,
            //                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            //        }

            //        e.Handled = true;
            //    }
            //}
        }
        private void dg_areas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dg_areas.Rows.Count) return;
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dg_areas.Columns.Count) return;

        } 
        private void dg_areas_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //idForDeletion = int.Parse(dg_areas.Rows[e.RowIndex].Cells["dg_location_code"].Value?.ToString());
            currentRowDetails = "details:\n";
            currentRowDetails += dg_areas.Columns["dg_use_type"].HeaderText 
                                + ": "
                                + (string.IsNullOrEmpty(dg_areas.Rows[e.RowIndex].Cells["dg_use_type"].Value?.ToString())
                                    ? "n/a"
                                    :   "'" + dg_areas.Rows[e.RowIndex].Cells[0].Value.ToString() + "'"
                                    + " and "
                                    + dg_areas.Columns["dg_location_code"].HeaderText
                                    + "  "
                                    + (string.IsNullOrEmpty(dg_areas.Rows[e.RowIndex].Cells["dg_location_code"].Value?.ToString())
                                        ? "n/a"
                                        : "'" + dg_areas.Rows[e.RowIndex].Cells[6].Value.ToString() + "'"));

            //nag ka error out of nowhere - bandaid
            string value = null;
            if (e.RowIndex >= 0 && e.RowIndex < dg_areas.Rows.Count)
            {
                var row = dg_areas.Rows[e.RowIndex];   
                if (dg_areas.Columns.Contains("id") && row.Cells["id"].Value != null)
                { //this isnt supposedly like this but omewhere it is changed
                    value = row.Cells["id"].Value.ToString();
                } 
                else if (dg_areas.Columns.Contains("dg_id_areas") && row.Cells["dg_id_areas"].Value != null)
                { //this the initial name and change somewhere later(unintentionally)
                    value = row.Cells["dg_id_areas"].Value.ToString();
                }
            }

            idForDeletion = int.TryParse(value, out int id) ? id : 0;
        } 
        private void dg_areas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isPopulatingUsetype 
                || isSavingRow
                || e.RowIndex < 0 
                || e.ColumnIndex < 0
                ) return;

            dg_areas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dg_areas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString().ToUpper();

            try
            {
                isSavingRow = true;

                //await Task.Delay(50); 
                for (int colIndex = e.ColumnIndex; colIndex < 5; colIndex++)
                {
                    var currentCell = dg_areas.Rows[e.RowIndex].Cells[colIndex];
                    var toClearCell = dg_areas.Rows[e.RowIndex].Cells[colIndex + 1];

                    if (currentCell.Value == null || string.IsNullOrWhiteSpace(currentCell.Value.ToString().Trim()))
                    {
                        toClearCell.Value = ""/*toClearCell.Value*/;
                    }
                }
                for (int i = 0; i < 6/*dg_areas.Columns.Count*/; i++)
                {
                    string tmpString = dg_areas.Rows[e.RowIndex].Cells[i].Value?
                        .ToString()
                        .Trim()
                        .Replace("--", "-") ?? string.Empty;
                    while (tmpString.EndsWith("-"))
                    {
                        tmpString = tmpString.TrimEnd('-').Trim();
                    }

                    dg_areas.Rows[e.RowIndex].Cells[i].Value = tmpString;
                }

                UpdateLocationCodeInRow(dg_areas.Rows[e.RowIndex]);
                UpdateWarehouseAreas(e.RowIndex);
            }
            finally
            {
                isSavingRow = false;
            }
        } 
        private void dg_areas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (!btn_cancel.Visible) return; //to detect if edit or creation mode
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
                    e.SuppressKeyPress = true; // cancel the Delete key action
                    e.Handled = true;       
                }
                else
                {
                    isRowDeletionCommittable = true; //to make the edeletion doable
                }
            }
        }




        //search cmb feat here
        private void cmb_search_Leave(object sender, EventArgs e)
        {
            if (cmb_search.Items.Count > 0)
            {
                cmb_search.Text = cmb_search.Text.ToUpper();
            }
            else
            {
                cmb_search.Text = tmpString;
                // Handle the case where ComboBox is empty (e.g., show a message to the user)
            } 
        }//just makes it uppercase
        private void cmb_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
            {
                //you.Focus(); focuses on you
                this.ActiveControl = null;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);  
                cmb_search.DroppedDown = false; 
                e.Handled = true;
            }
        }
        // Fields
        static int dashCounter = 0;
        static string tmpString = "";
        static List<string> wh_areasColumnList = new List<string>
            {
                "use_type", "zone", "area", "rack", "level", "bins", "location_code"
            };
        static bool isPopulatingSearch = false;//static changed
        private async Task PopulateSearch() 
        {
            try
            {
                isPopulatingSearch = true;
                bool shouldDroppedDown = true;

                dashCounter = tmpString.Count(c => c == '-');

                if (dashCounter >= wh_areasColumnList.Count - 1)
                {
                    //MessageBox.Show("Maximum dash depth reached.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    tmpString = tmpString.TrimEnd('-');

                    cmb_search.Text = tmpString;
                    cmb_search.SelectionStart = cmb_search.Text.Length;
                    cmb_search.DroppedDown = false;
                     
                    dashCounter = tmpString.Count(c => c == '-');
                     
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    shouldDroppedDown = false;
                }


                cmb_search.Items.Clear();

                // Fetch data
                string currentText = cmb_search.Text; 
                var warehouseDictionary = await WarehouseNameServices.GetWarehouseInfos(); 

                DataTable warehouseAreaTable = JsonHelper.ToDataTable(warehouseDictionary.warehouse_area);

                string[] segments = tmpString.Split('-'); 
                bool isLastOpen =
                    string.Equals(segments
                        .Reverse()
                        .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s))
                        , "OPEN"
                        , StringComparison.OrdinalIgnoreCase);


                string nextColumn = wh_areasColumnList[dashCounter];

                // Filter rows based on previous segments
                var filtered = warehouseAreaTable.AsEnumerable();
                for (int i = 0; i < dashCounter; i++)
                {
                    string filterColumn = wh_areasColumnList[i];
                    string filterValue = segments[i];
                    filtered = filtered.Where(row => string.Equals(row.Field<string>(filterColumn), filterValue, StringComparison.OrdinalIgnoreCase));
                } 

                string lastColumn = wh_areasColumnList.Last();
                bool shouldAddOpen = false;
                var filteredList = filtered.ToList();
                //
                foreach (var row in filteredList)
                {
                    bool allEmptyTillLast = true;

                    for (int i = dashCounter; i < wh_areasColumnList.Count - 1; i++)
                    {
                        string value = row[wh_areasColumnList[i]]?.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            allEmptyTillLast = false;
                            break;
                        }
                    }

                    string locationCode = row[lastColumn]?.ToString();
                    if (allEmptyTillLast && !string.IsNullOrEmpty(locationCode) &&
                        locationCode.IndexOf("OPEN", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        shouldAddOpen = true;
                        break;
                    }
                }
                 
                if ((isLastOpen || !filtered.Any())
                    && cmb_search.Text.EndsWith("-"))
                {
                    tmpString = tmpString.TrimEnd('-');
                    cmb_search.DroppedDown = false;
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    return;
                }

                // Get distinct next options = .equals
                var nextOptions = filteredList
                    .Select(row => row.Field<string>(nextColumn))
                    .Where(val => !string.IsNullOrWhiteSpace(val))
                    .Distinct()
                    .OrderBy(val => val) // Sort alphabetically
                    .ToList();

                //cmb_search.Items.Clear();

                //filter for OPEN option
                foreach (var row in filteredList)
                {
                    // Check if all intermediate columns (from current to last) are empty
                    bool allEmptyTillLast = true;

                    for (int i = dashCounter; i < wh_areasColumnList.Count - 1; i++)
                    {
                        string value = row[wh_areasColumnList[i]]?.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            allEmptyTillLast = false;
                            break;
                        }
                    }

                    string locationCode = row[lastColumn]?.ToString();
                    if (allEmptyTillLast && !string.IsNullOrEmpty(locationCode) &&
                        locationCode.IndexOf("OPEN", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        shouldAddOpen = true;
                        break;
                    }
                }

                if (shouldAddOpen && !cmb_search.Items.Contains("OPEN"))
                {
                    cmb_search.Items.Add("OPEN");
                }

                foreach (var item in nextOptions)
                {
                    if (int.TryParse(item, out int binRange))
                    {
                        // Add numeric range items (1..binRange)
                        for (int i = 1; i <= binRange; i++)
                            cmb_search.Items.Add(i);
                    }
                    else
                    {
                        cmb_search.Items.Add(item.ToUpper());
                    }
                }
                 
                if (cmb_search.Items.Count <= 0)
                {
                    string rawText = cmb_search.Text?.Trim() ?? string.Empty;

                    string tmpString = rawText.EndsWith("-")
                        ? rawText.TrimEnd('-')
                        : rawText;

                    if (!string.IsNullOrEmpty(tmpString) && !cmb_search.Items.Contains(tmpString))
                    {
                        cmb_search.Items.Add(tmpString);
                    }
                }

                cmb_search.MaxDropDownItems = 10; 

                cmb_search.DroppedDown = shouldDroppedDown;
                cmb_search.Text = currentText;  
                cmb_search.SelectionStart = cmb_search.Text.Length;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("index"))
                { 
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    isPopulatingSearch = false;
                    return;
                }
                MessageBox.Show("Error loading search options: " + ex.Message);
            }
            finally
            {
                isPopulatingSearch = false;

            }
        }
        private void cmb_search_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_search.SelectedItem != null)
            {
                string selectedText = cmb_search.SelectedItem.ToString();
                var segments = tmpString.Split('-').ToList();

                // Replace last segment if it's empty or being edited
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

                tmpString = string.Join("-", segments);

                // Ensure trailing dash if more selection expected
                if (dashCounter < wh_areasColumnList.Count - 1 && !tmpString.EndsWith("-"))
                    tmpString += "-";

                if (tmpString.EndsWith("-") && cmb_search.Items.Count <= 0) tmpString.TrimEnd('-');
                cmb_search.Text = tmpString;
            }
        } 
        private async void cmb_search_TextChanged(object sender, EventArgs e)
        {
            if (isPopulatingSearch) return;
            isPopulatingSearch = true;

            try
            {
                if (cmb_search.DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    var currentText = cmb_search.Text;
                    var upperText = currentText.ToUpper();

                    if (currentText != upperText)
                    {
                        var cursorPos = cmb_search.SelectionStart;
                        cmb_search.Text = upperText;
                        cmb_search.SelectionStart = cursorPos;
                    }
                }

                tmpString = cmb_search.Text;

                if (tmpString.EndsWith("-") 
                    || tmpString.Length <= 1
                    || dashCounter != tmpString.Count(c => c == '-'))
                {
                    dashCounter = tmpString.Count(c => c == '-');
                    await PopulateSearch();
                }
                else
                {
                    cmb_search.SelectionStart = cmb_search.Text.Length;
                }
            }
            finally
            {
                isPopulatingSearch = false;
            }


        } 
        private void cmb_search_DropDown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default; // Fix cursor visibility issue 
        }
        private async void cmb_search_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmb_search.Text))
            {
                tmpString = "";  // reset any temp data
                dashCounter = 0;
                await PopulateSearch();
            }
        }


        private void btn_search_Click(object sender, EventArgs e)
        {
            cmb_search.Text = "";
            tmpString = "";
            dashCounter = 0;

            BtnToggleEnabillity("search");
            cmb_search.Focus();
        }
        //not used. yet
        private string RemoveDuplicateSegments(string input)
        { //remove friking double -- fron inputted string
            if (string.IsNullOrEmpty(input)) return "";

            // Split string by dash
            string[] parts = input.Split('-');
            List<string> distinctParts = new List<string>();

            foreach (string part in parts)
            {
                // Avoid empty or duplicate entries
                if (!string.IsNullOrWhiteSpace(part) && !distinctParts.Contains(part))
                {
                    distinctParts.Add(part);
                }
            }

            return string.Join("-", distinctParts);
        }

        static bool isInitialLoad;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedTab = tabControl1.SelectedIndex;
            if (tabControl1.SelectedIndex == 0)
            {
                dg_areas.EndEdit();
            }
            if (btn_cancel.Visible)
            {
                BtnToggleEnabillity("edit");
                if (string.IsNullOrEmpty(txt_id.Text))
                {
                    dg_areas.Enabled = false;
                }
            }
            else
            {
                BtnToggleEnabillity("cancel");
            }

            if (isInitialLoad)
            {
                DataView dataViewWarehouseAreas = new DataView(areas);
                for (int i = 0; i < dataViewWarehouseAreas.Count; i++)
                {
                    PopulateUsetype(new DataGridViewCellEventArgs(1, i), i);
                }
                isInitialLoad = false;
            }
        }

        private void txt_name_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_name.Text)
                && string.IsNullOrEmpty(txt_id.Text)
                && btn_cancel.Visible)
            {
                BtnToggleEnabillity("edit");
            }
        }
         
    }
} 
