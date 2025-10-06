using Inventory_SMPC.Pages.Item;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Model;
using smpc_inventory_app.Pages.Business_Partner_Info.Bpi_Modal;
using smpc_inventory_app.Pages.Setup;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Bpi;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Bpi;
using smpc_sales_app.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ApiResponseModel = smpc_inventory_app.Services.Setup.ApiResponseModel;

namespace smpc_inventory_app.Pages.Business_Partner_Info
{
    public partial class BusinessPartnerInfo : UserControl
    {
        string REGEXPATTERN = @"^\d{4}-\d{3}-\d{4}$";
        DataTable bpi;
        DataTable general;
        DataTable contacts;
        DataTable address;
        DataTable items;
        DataTable finance;
        DataTable finance_pending;
        DataTable history;
        DataTable accreditations;
        DataTable fullAddressRecords;
        DataTable fullItemsRecords;

        List<BpiEntityRecords> entityCount;
        List<CurrentUserModel> Users;
        GeneralSetupServices serviceSetup;
        SetupModal modalSetup;
        SetupSelectionModal modalSelection;
        Bpi_Class records;
        Button dynamicButton = new Button();

        List<int> currentSelectedIndustryIds = new List<int>();
        List<int> currentSelectedBranchIndustryIds = new List<int>();
        List<int> currentSelectedEntityIds = new List<int>();
        int selectedRecord = 0;
        int groupCount = 0;
        TabPage tabItemPages;
        TabPage tabFinancePages;
        ApiResponseModel response;
        string CanvassForm;
        List<string> selectedPreferenceNames = new List<string>();
        public BusinessPartnerInfo(string canvassForm)
        {
            this.CanvassForm = canvassForm;
            InitializeComponent();
            tabItemPages = tabControl2.TabPages["ITEMS"];
            tabFinancePages = tabControl2.TabPages["FINANCE"];

            if (CanvassForm != "")
            {
                ShowCanvassTabPage();
            }

        }
        public void HideButton()
        {
            btn_add_new_item.Visible = false;
        }


        private void ShowCanvassTabPage()
        {
            string[] tabPageList = { "FINANCE", "ACCREDITATION", "HISTORY" };
            foreach (TabPage tabPage in tabControl2.TabPages)

            { 


                if (tabPageList.Contains(tabPage.Text))
                {
                    RemoveTabPages(tabPage);
                }
            }
        }
        private void BusinessPartnerInfo_Load(object sender, EventArgs e)
        {

            GetBpiUser();
            GetIndustriesSetup();

            GetSocialMediaSetup();
            GetEntity();
            GetBranchIndustries();
            GetPayments();
            GetEntityCount();
            GetPositionSetup();

            GetBpi();
            BtnToogle(false);
        }

        private async void GetBpiUser()
        {
            var response = await BpiServices.GetBpiUsers("IT");
            Users = response;

        }
        private async void GetIndustriesSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.INDUSTRIES);
            CacheData.Industries = await serviceSetup.GetAsDatatable();
        }
        private async void GetSocialMediaSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.SOCIALS);
            CacheData.SocialMedia = await serviceSetup.GetAsDatatable();


            DataRow newRow = CacheData.SocialMedia.NewRow();
            newRow["id"] = DBNull.Value;
            newRow["name"] = "-- Select --";

            CacheData.SocialMedia.Rows.InsertAt(newRow, 0);
            cmb_social.DataSource = CacheData.SocialMedia;
            cmb_social.ValueMember = "id";
            cmb_social.DisplayMember = "name";
        }

        private async void GetBranchIndustries()
        {

            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.INDUSTRIES);
            CacheData.BranchIndustries = await serviceSetup.GetAsDatatable();
        }


        private async void GetEntity()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ENTITY);
            CacheData.Entity = await serviceSetup.GetAsDatatable();
        }

        private async void GetPayments()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();

            cmb_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_account.DataSource = CacheData.PaymentTerms;
            cmb_item_account.DataSource = CacheData.PaymentTerms;

            cmb_payment_terms.ValueMember = "id";
            cmb_payment_terms.DisplayMember = "code";

            cmb_finance_payment_terms.ValueMember = "id";
            cmb_finance_payment_terms.DisplayMember = "code";

            cmb_finance_account.ValueMember = "id";
            cmb_finance_account.DisplayMember = "code";

            cmb_item_account.ValueMember = "id";
            cmb_item_account.DisplayMember = "code";
        }

       
        private async void GetEntityCount()
        {

            var response = await RequestToApi<ApiResponseModel<List<BpiEntityRecords>>>.Get(ENUM_ENDPOINT.BpiEntity);
            entityCount = response.Data;

        }

        private int GetEntityRecordCount(string code)
        {
            var record = entityCount.FirstOrDefault(records => records.code == code);
            return record != null ? record.entity_count : 0;

        }
        private async void GetPositionSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.POSITION);
            CacheData.Positions = await serviceSetup.GetAsDatatable();

            DataRow newRow = CacheData.Positions.NewRow();
            newRow["id"] = DBNull.Value;
            newRow["name"] = "--Select--";

            CacheData.Positions.Rows.InsertAt(newRow, 0);


            var combobox = (DataGridViewComboBoxColumn)dg_contacts.Columns["position"];
            combobox.DataSource = CacheData.Positions;
            combobox.DisplayMember = "name";
            combobox.ValueMember = "id";

        }
        private void GetPaymentItemTerms(bool isItem)
        {
            if (isItem)
            {
                //serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
                //CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();

                var matchedRecord = records.items.FirstOrDefault(f => f.bpi_item_based_id == int.Parse(bpi.Rows[this.selectedRecord]["id"].ToString()));

                if (matchedRecord != null)
                {
                    cmb_payment_terms.SelectedValue = matchedRecord.payment_terms_id;
                    cmb_payment_terms.SelectedItem = matchedRecord;
                    cmb_tax_code.Text = matchedRecord.payment_terms_id.ToString();
                }

                //Add tax code 
                cmb_tax_code.DataSource = ENUM_TAX_CODE.LIST();
                cmb_tax_code.DisplayMember = "title";
            }

        }
        private async void GetBpi()
        {
            var response = await RequestToApi<ApiResponseModel<Bpi_Class>>.Get(ENUM_ENDPOINT.BPI);
            records = response.Data;

            bpi = JsonHelper.ToDataTable(records.bpi);
            general = JsonHelper.ToDataTable(records.general);
            contacts = JsonHelper.ToDataTable(records.contacts);
            address = JsonHelper.ToDataTable(records.address);
            items = JsonHelper.ToDataTable(records.items);
            finance = JsonHelper.ToDataTable(records.finance);
            finance_pending = JsonHelper.ToDataTable(records.finance_pending);
            accreditations = JsonHelper.ToDataTable(records.accreditations);
            history = JsonHelper.ToDataTable(records.history);
            if (records.bpi.Count != 0 && records.general.Count != 0 && records.contacts.Count != 0 && records.address.Count != 0)
            {

                BindBpiRecords(true);
            }
            else
            {

                button5.Enabled = false;
                RemoveTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);

                Button dynamicButton = new Button();

                dynamicButton.Text = "MAIN";
                dynamicButton.Size = new Size(100, 50);
                dynamicButton.Location = new Point(50, 50); // X=50, Y=50
                dynamicButton.BackColor = Color.LightBlue;

            }

        }

        private void GetAllBpiChild()
        {
            //Fetch Bpi Contacts
            DataView dataViewContact = new DataView(contacts);

            if (dataViewContact.Count != 0)
            {
                dataViewContact.RowFilter = "contacts_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = null;
                if (dataViewContact.Count > 0)
                {

                    filteredRow = dataViewContact[0];
                    string filteredBasedId = filteredRow["branch_id"].ToString();

                    dataViewContact.RowFilter = $"branch_id = '{filteredBasedId}'";

                }

                DataTable filteredContacts = dataViewContact.ToTable();

                foreach (DataRow contactRow in filteredContacts.Rows)
                {
                    int positionValue = Convert.ToInt32(contactRow["position"]);

                    if (positionValue == 0)
                    {
                        CacheData.Positions.Rows.Add(0, "", "");
                        contactRow["position"] = 0;
                    }
                }

                dataBindingContacts.DataSource = filteredContacts;
            }

            //Fetch Bpi Address
            DataView dataViewAddress = new DataView(address);
            if (dataViewAddress.Count != 0)
            {
                //   var sampleTest = bpi.Rows[this.selectedRecord]["id"].ToString();
                dataViewAddress.RowFilter = "address_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = dataViewAddress[0];
                string filteredBasedId = filteredRow["address_branch_id"].ToString();
                string addressBasedId = filteredRow["address_based_id"].ToString();
                dataViewAddress.RowFilter = $"address_branch_id = {filteredBasedId} AND address_based_id = {addressBasedId} AND address_is_deleted = {false}";
                //  dataViewAddress.RowFilter = $"address_branch_id = {filteredBasedId} AND address_based_id = {addressBasedId} AND address_is_deleted = 'False'";
                DataTable filteredAddress = dataViewAddress.ToTable();
                dataBindingAddress.DataSource = filteredAddress;
            }

            //Fetch Bpi Address
            DataView dataViewItems = new DataView(items);
            if (dataViewItems.Count != 0)
            {
                dataViewItems.RowFilter = "bpi_item_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = null;
                if (dataViewItems.Count > 0)
                {
                    filteredRow = dataViewItems[0];
                    string filteredBasedId = filteredRow["bpi_item_branch_id"].ToString();
                    string itemBasedId = filteredRow["bpi_item_based_id"].ToString();
                    dataViewItems.RowFilter = $"bpi_item_branch_id = {filteredBasedId} AND bpi_item_based_id = {itemBasedId} AND item_is_deleted = False";

                }

                DataTable filteredItems = dataViewItems.ToTable();
                dataBindingItems.DataSource = filteredItems;
            }

            // Fetch Bpi Finance Pending 
            DataView dataViewFinancePending = new DataView(finance_pending);
            if (dataViewFinancePending.Count != 0)
            {
                dataViewFinancePending.RowFilter = "customer_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = null;
                if (dataViewFinancePending.Count > 0)
                {
                    filteredRow = dataViewFinancePending[0];
                    string filteredBranchId = filteredRow["finance_pending_branch_id"].ToString();
                    string financeCustomerId = filteredRow["customer_id"].ToString();
                    dataViewFinancePending.RowFilter = $"finance_pending_branch_id = '{filteredBranchId}' AND customer_id = '{financeCustomerId}'";

                }
                dataBindingFinancePending.DataSource = dataViewFinancePending;
            }

            //Fetch Bpi Accreditations
            DataView dataViewAccreditation = new DataView(accreditations);
            if (dataViewAccreditation.Count != 0)
            {
                dataViewAccreditation.RowFilter = "bpi_accreditation_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = null;
                if (dataViewAccreditation.Count > 0)
                {
                    filteredRow = dataViewAccreditation[0];
                    string filteredBranchId = filteredRow["bpi_accreditation_branch_id"].ToString();
                    string bpiAccreditationBasedId = filteredRow["bpi_accreditation_based_id"].ToString();
                    dataViewAccreditation.RowFilter = $"bpi_accreditation_branch_id = '{filteredBranchId}' AND bpi_accreditation_based_id = '{bpiAccreditationBasedId}'";

                }

                DataTable filteredItems = dataViewAccreditation.ToTable();
                databindingAccreditation.DataSource = filteredItems;
            }

            DataView dataViewHistory = new DataView(history);
            if (dataViewHistory.Count != 0)
            {
                dataViewHistory.RowFilter = "based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                DataRowView filteredRow = null;
                if (dataViewHistory.Count > 0)
                {
                    filteredRow = dataViewHistory[0];
                    string filteredBranchId = filteredRow["branch_id"].ToString();
                    string historyBasedId = filteredRow["based_id"].ToString();
                    dataViewHistory.RowFilter = $"branch_id = {filteredBranchId} AND based_id = {historyBasedId} ";

                }

                DataTable filteredHistory = dataViewHistory.ToTable();
                dataBindingHistory.DataSource = filteredHistory;
            }



        }

        private void GetAllBpiBranch(DataTable data)
        {
            var fetchBranch = data;

            //var buttonsToRemove = flowLayout_panel.Controls
            //    .OfType<Button>()
            //    .Where(ctrl => !data.AsEnumerable().Any(row => row["branch_name"].ToString() == ctrl.Text))
            //    .ToList();

            //// Remove buttons safely
            //foreach (var btn in buttonsToRemove)
            //{
            //    flowLayout_panel.Controls.Remove(btn);
            //    btn.Dispose();
            //}

            foreach (var btn in flowLayout_panel.Controls.OfType<Button>().ToList())
            {
                flowLayout_panel.Controls.Remove(btn);
                btn.Dispose();
            }

            foreach (DataRow row in data.Rows)
            {
                string branchName = row["branch_name"].ToString();
                var salesName = bpi.Rows[this.selectedRecord]["sales_id"].ToString();
                //    var matchSelectedSaless = Users.FirstOrDefault(salesUser => salesUser.employee_id == bpi.Rows[this.selectedRecord]["sales_id"].ToString());

                var salesOwner = CacheData.CurrentUser.employee_id == row["branch_sales_id"].ToString();
                var matchSelectedSales = Users.FirstOrDefault(salesUser => salesUser.employee_id == row["branch_sales_id"].ToString());
                string selectedSalesNames = "";
                if (matchSelectedSales != null)
                {
                    selectedSalesNames = $"({matchSelectedSales.first_name.Substring(0, 1).ToUpper()}. {matchSelectedSales.last_name})";
                    //selectedSalesNames = txt_sales_id.Text;
                }
                string selectedSalesName = salesOwner ? txt_sales_id.Text : selectedSalesNames;

                Button dynamicButton = new Button
                {
                    Text = branchName,
                    Size = new Size(100, 50),
                    BackColor = Color.LightBlue,
                    Tag = row,
                    //    Enabled = selectedSalesName != ""
                };
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(dynamicButton, selectedSalesName);


                if (salesOwner)
                {
                    dynamicButton.Click += DynamicButton_Clicks; // Attach the click event
                }
                flowLayout_panel.Controls.Add(dynamicButton);

            }

        }

        private bool GetSelectedSales()
        {
            bool isSalesOwner = CacheData.CurrentUser.employee_id == bpi.Rows[this.selectedRecord]["sales_id"].ToString();
            return isSalesOwner;
        }
        public void GetFilteredTabIds(DataTable table, string filterColumn, string filterValue, string column1, string column2, out int basedId, out int id)
        {
            basedId = 0;
            id = 0;

            if (table == null || table.Rows.Count == 0)
                return;

            DataView tabView = new DataView(table)
            {
                RowFilter = $"{filterColumn} = '{filterValue}'"
            };

            if (tabView.Count == 0)
                return;

            int.TryParse(tabView[0][column1].ToString(), out basedId);
            int.TryParse(tabView[0][column2].ToString(), out id);
        }

        private void BindBpiRecords(bool isBind = false)
        {

            if (isBind)
            {
                var isSelectedSales = GetSelectedSales();
                BpiBranchToggle(isSelectedSales);
                BindDataToPanel();
                GetAllBpiChild();

                // For List of Industry, General Entity and General Branch Names

                txt_industries.Text = string.IsNullOrEmpty(records.bpi[this.selectedRecord].industry_names) ? "" : (records.bpi[this.selectedRecord].industry_names);
                string industryIds = string.IsNullOrEmpty(records.bpi[this.selectedRecord].industry_ids) ? "" : (records.bpi[this.selectedRecord].industry_ids);

                BindMultiSelectField(records.general); // Bind Id and Names in Modal

                // Show Item pages

                bool isItemShow = ToogleItemPages(txt_entity_type.Text);
            
                GetPaymentItemTerms(isItemShow);
                ShowTypeOfEntity(txt_entity_type.Text);



                var matchSelectedSales = Users.FirstOrDefault(salesUser => salesUser.employee_id == bpi.Rows[this.selectedRecord]["sales_id"].ToString());
                if (matchSelectedSales != null)
                {
                    txt_sales_id.Text = $"({matchSelectedSales.first_name.Substring(0, 1).ToUpper()}. {matchSelectedSales.last_name})";
                }


                var listOfNames = records.bpi.Select(item => item.name).ToList();
                cmb_name.DataSource = listOfNames;
                var matchedFinanceRecord = records.finance.FirstOrDefault(f => f.finance_based_id == int.Parse(bpi.Rows[this.selectedRecord]["id"].ToString()));

                if (matchedFinanceRecord != null)
                {
                    cmb_finance_payment_terms.SelectedValue = matchedFinanceRecord.finance_payment_terms_id;
                    cmb_finance_payment_terms.SelectedItem = matchedFinanceRecord;
                    cmb_finance_account.SelectedValue = matchedFinanceRecord.finance_account_id;
                    cmb_finance_account.SelectedItem = matchedFinanceRecord;

                    
                    btn_finance_payment_terms.Visible = matchSelectedSales == null ? false :  matchSelectedSales.position_id.Equals("Web Developer");

                }


                cmb_transaction_type.SelectedValue = records.general[this.selectedRecord].transaction_type;
                cmb_class_name.SelectedItem = records.general[this.selectedRecord].class_name;

                cmb_name.SelectedItem = listOfNames[this.selectedRecord];
                cmb_social.SelectedValue = records.general[this.selectedRecord].social_id;
                cmb_social.SelectedItem = records.general[this.selectedRecord].social_id;

                //Getting the List of Ids to match in my getmodal

                currentSelectedIndustryIds = industryIds.Split(',')
                                           .Where(val => int.TryParse(val, out _))
                                           .Select(int.Parse)
                                           .ToList();

                DataView dataViewGeneral = new DataView(general);
                if (dataViewGeneral.Count != 0)
                {
                    dataViewGeneral.RowFilter = "general_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";

                    txt_branch_name.Visible = dataViewGeneral.Count > 1;
                    lbl_branch_name.Visible = dataViewGeneral.Count > 1;
                }

                DataTable filteredGeneral = dataViewGeneral.ToTable();


                Panel[] pnlGeneralPanel = { panel_general };
                txt_branch_name.Visible = false;
                lbl_branch_name.Visible = false;
                Helpers.BindControls(pnlGeneralPanel, filteredGeneral);
                GetAllBpiBranch(filteredGeneral);


            }
            else
            {
                this.bpi.Rows.Clear();
                this.contacts.Rows.Clear();
                this.general.Rows.Clear();
                this.address.Rows.Clear();
                this.items.Rows.Clear();
                this.finance.Rows.Clear();
                this.finance_pending.Rows.Clear();
            }
        }


        private void BindDataToPanel()
        {
            Panel[] pnlList = { panel_header_records };
            Panel[] pnlGeneralPanel = { panel_general };
            Panel[] pnlItems = { panel_item };
            Panel[] pnlFinance = { panel_finance };

            Helpers.BindControls(pnlList, bpi, this.selectedRecord);
            //   Helpers.BindControls(pnlGeneralPanel, general, this.selectedRecord);
            //   Helpers.BindControls(pnlItems, items, this.selectedRecord);
            Helpers.BindControls(pnlFinance, finance, this.selectedRecord);

        }

        private void BindMultiSelectField(List<BpiGeneral> general)
        {

            var matchSelectedEntity = general.FirstOrDefault(f => f.general_based_id == int.Parse(bpi.Rows[this.selectedRecord]["id"].ToString()));

            string selectedEntity = "";
            string branchIndustryIds = "";
            if (matchSelectedEntity != null)
            {
                txt_entity_type.Text = matchSelectedEntity.entity_names;
                selectedEntity = matchSelectedEntity.entity_ids;
                txt_branch_industry.Text = matchSelectedEntity.branch_industry_names;
                branchIndustryIds = matchSelectedEntity.branch_industry_ids;

            }

            currentSelectedEntityIds = selectedEntity.Split(',')
                                               .Where(val => int.TryParse(val, out _))
                                               .Select(int.Parse)
                                               .ToList();

            currentSelectedBranchIndustryIds = branchIndustryIds.Split(',')
                                                 .Where(val => int.TryParse(val, out _))
                                                 .Select(int.Parse)
                                                 .ToList();
        }


        private void BindMultiSelectField(DataView dt, string ids = "0")
        {

            int selectedId;
            string rows = "";
            if (ids == "0")
            {
                rows = bpi.Rows[this.selectedRecord]["id"]?.ToString();
            }
            else
            {
                rows = ids;
            }
            if (int.TryParse(rows, out selectedId))
            {
                var matchSelectedEntity = dt.Cast<DataRowView>()
                    .FirstOrDefault(f => Convert.ToInt32(f["general_based_id"]) == selectedId);

                string selectedEntity = "";
                string branchIndustryIds = "";
                if (matchSelectedEntity != null)
                {

                    txt_entity_type.Text = matchSelectedEntity["entity_names"].ToString();
                    selectedEntity = matchSelectedEntity["entity_ids"].ToString();
                    txt_branch_industry.Text = matchSelectedEntity["branch_industry_names"].ToString();
                    branchIndustryIds = matchSelectedEntity["branch_industry_ids"].ToString();
                }


                // Convert selectedEntity to List<int>, filtering out invalid values
                currentSelectedEntityIds = selectedEntity.Split(',')
                    .Where(val => int.TryParse(val, out _))
                    .Select(int.Parse)
                    .ToList();

                currentSelectedBranchIndustryIds = branchIndustryIds.Split(',')
                    .Where(val => int.TryParse(val, out _))
                    .Select(int.Parse)
                    .ToList();
            }

        }

        private void BpiBranchToggle(bool isVisible = true)
        {

            panel_general.Visible = isVisible;
            pnl_new_added_item.Visible = isVisible;
            panel_accreditation.Visible = isVisible;
            panel_item.Visible = isVisible;
            panel_finance.Visible = isVisible;
            dg_contacts.Visible = isVisible;
            dg_address.Visible = isVisible;
            dg_items.Visible = isVisible;
            dg_accreditations.Visible = isVisible;
            tabControl_Finance.Visible = isVisible;
        }

        private void BindBranchToChildTab(int branchId)
        {
            DataView dataViewContact = new DataView(contacts);

            if (dataViewContact.Count != 0)
            {
                dataViewContact.RowFilter = $"branch_id = '{branchId}'";

                DataTable filteredContacts = dataViewContact.ToTable();
                dataBindingContacts.DataSource = filteredContacts;

            }

            DataView dataViewAddress = new DataView(address);

            if (dataViewAddress.Count != 0)
            {
                dataViewAddress.RowFilter = $"address_branch_id = '{branchId}'";

                DataTable filteredAddress = dataViewAddress.ToTable();
                dataBindingAddress.DataSource = filteredAddress;

            }


            DataView dataViewItems = new DataView(items);
            if (dataViewItems.Count != 0)
            {
                dataViewItems.RowFilter = $"bpi_item_branch_id = '{branchId}'";

                DataTable filteredItems = dataViewItems.ToTable();
                dataBindingItems.DataSource = filteredItems;
            }

        }

        private void BindBranchToChildTabs(int branchId, DataTable dt, string filterName)
        {
            DataView dataViewRecords = new DataView(dt);
            if (dataViewRecords.Count != 0)
            {
                dataViewRecords.RowFilter = $"{filterName} = '{branchId}'";

                DataTable filteredContacts = dataViewRecords.ToTable();
                dataBindingContacts.DataSource = filteredContacts;

            }

        }

        private void BtnBindData()
        {
            general.Rows.Add(0, 0, 0);
            DataRow lastRow = general.Rows[general.Rows.Count - 1]; // Get last 
            Button newDynamicButton = new Button();

            newDynamicButton.Text = "MAIN";
            newDynamicButton.Size = new Size(100, 50);
            newDynamicButton.Location = new Point(50, 50);
            newDynamicButton.BackColor = Color.LightBlue;

            newDynamicButton.Tag = general;


            newDynamicButton.Click += DynamicButton_Clicks;

            flowLayout_panel.Controls.Add(newDynamicButton);

        }

        private void BtnToogle(bool isEdit)
        {

            btn_new.Visible = !isEdit;
            btn_search.Visible = !isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;
            btn_edit.Visible = !isEdit;

            btn_cancel.Visible = isEdit;

            panel_header_records.Enabled = isEdit;
            buttonPanel.Enabled = isEdit;
            panel_general.Enabled = isEdit;
            pnl_new_added_item.Enabled = isEdit;

            panel_finance.Enabled = isEdit;
            dg_contacts.Enabled = isEdit;
            dg_address.Enabled = isEdit;
            dg_items.Enabled = isEdit;
            dg_accreditations.Enabled = isEdit;

        }

        List<int> copyBranchIds = new List<int>();
       
        private void CopyToMainBranchField(string fieldName, string value)
        {
            string mainBpi_ID = txt_id.Text;
            if (string.IsNullOrEmpty(mainBpi_ID))
            {
                switch (fieldName.ToLower())
                {
                    case "main_website":
                        txt_branch_website.Text = value;
                        break;
                    case "main_tel_no":
                        txt_branch_tel_no.Text = value;
                        break;
                    case "industries":
                        txt_branch_industry.Text = value;
                        txt_branch_industry.Tag = txt_industries.Tag;



                        //var values = txt_industries.Tag as List<int>;

                        //foreach (int newValue in values)
                        //{
                        //    copyBranchIds.Add(newValue);
                        //}
                        //txt_industries.Tag = copyBranchIds;
                        //currentSelectedBranchIndustryIds = txt_industries.Tag as List<int>;

                        var selectedIndustriesID = CopySelectedIndustries(txt_industries);
                        currentSelectedBranchIndustryIds = selectedIndustriesID;

                        break;
                    case "branch_industries":
                        txt_industries.Text = value;

                        txt_industries.Tag = txt_branch_industry.Tag;

                        //var values3 = txt_industries.Tag as List<int>;


                        //foreach (int newValue in values3)
                        //{
                        //    copyBranchIds.Add(newValue);
                        //}
                        //txt_branch_industry.Tag = copyBranchIds;

                        //currentSelectedIndustryIds = txt_branch_industry.Tag as List<int>;

                        var selectedIndustries = CopySelectedIndustries(txt_branch_industry);
                        currentSelectedIndustryIds = selectedIndustries;

                        break;
                    case "branch_tel_no":
                        txt_main_tel_no.Text = value;
                        break;
                    case "branch_website":
                        txt_main_website.Text = value;
                        break;
                }
            }
        }
       
        private List<int> CopySelectedIndustries(TextBox txtBox)
        {
            var selectedIndustriesID = txtBox.Tag as List<int>;
            foreach (int newIndustryIds in selectedIndustriesID)
            {
                copyBranchIds.Add(newIndustryIds);
            }
            txtBox.Tag = copyBranchIds;

            return copyBranchIds;
        }
       
        private void DynamicButton_Clicks(object sender, EventArgs e)
        {

            Button clickedButton = sender as Button;

            // Retrieve the associated data from the Tag property (could be the row or just the branch name)
            if (clickedButton != null && clickedButton.Tag != null)
            {
                DataRow row = clickedButton.Tag as DataRow;

                if (row != null)
                {
                    string branchName = row["branch_name"].ToString();
                    int branchId = int.Parse(row["general_id"].ToString());


                    DataTable generalRowTable = row.Table.Copy();


                    //Check to find the index of general rows
                    int index = generalRowTable.AsEnumerable()
                          .Select((r, i) => new { Row = r, Index = i })
                          .FirstOrDefault(x => x.Row["branch_name"].ToString() == branchName)?.Index ?? -1;

                    // filter the general based on branch name when btn click is process
                    DataView dataViewGeneral = new DataView(generalRowTable);
                    if (dataViewGeneral.Count != 0)
                    {
                        dataViewGeneral.RowFilter = $"branch_name = '{branchName}'";

                        txt_branch_name.Visible = index > 0;
                        lbl_branch_name.Visible = index > 0;
                    }

                    DataTable filteredGeneral = dataViewGeneral.ToTable();
                    DataView dataViewGenerals = new DataView(filteredGeneral);
                    string value = "0";  // Default value

                    if (dataViewGenerals.Count > 0)  // Ensure there is at least one row
                    {
                        object idValue = dataViewGenerals[0]["general_based_id"];
                        value = (idValue != DBNull.Value) ? idValue.ToString() : "0";
                    }

                    // Now, value is always defined and can be used

                    BindMultiSelectField(dataViewGenerals, value); // bind entity type to text field based on branch selected

                    Panel[] pnlGeneralPanel = { panel_general };

                    if (branchId == 0)
                    {
                        Helpers.ResetControls(panel_general);
                        Helpers.ResetControls(panel_item);
                        Helpers.ResetControls(panel_finance);
                        ResetSelectedData();
                    }
                    if (value != "0")
                    {
                        var isSelectedSales = CacheData.CurrentUser.employee_id == row["branch_sales_id"].ToString();
                        BpiBranchToggle(isSelectedSales);

                        Helpers.BindControls(pnlGeneralPanel, filteredGeneral);
                    }
                    else
                    {
                        BpiBranchToggle();
                    }

                    BindBranchToChildTab(branchId);

                }

            }
        }

        private void DocumentCodeIncrementor(string entity)
        {

            switch (entity)
            {

                case "SUPPLIER":

                    txt_supplier_code.Text = "S#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Supplier) + 1);

                    break;

                case "CUSTOMER":
                    txt_customer_code.Text = "C#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Customer) + 1);

                    break;

                case "NON-AFFILIATED":

                    txt_non_affiliated.Text = "EN#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Non_Affiliated) + 1);

                    break;
                case "AFFILIATED":

                    txt_affiliated.Text = "EA#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Affiliated) + 1);
                    break;

                case "BOTH":

                    txt_customer_code.Text = "C#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Customer) + 1);
                    txt_supplier_code.Text = "S#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Supplier) + 1);

                    break;

                default:

                    break;
            }

        }
        //private void FetchAllBranchSample(DataTable data)
        //{
        //    var existingButtons = flowLayout_panel.Controls.OfType<Button>()
        //                        .Select(btn => btn.Text)
        //                        .ToList();

        //    foreach (Control ctrl in flowLayout_panel.Controls.OfType<Button>().ToList())
        //    {

        //        if (!data.AsEnumerable().Any(row => row["branch_name"].ToString() == ctrl.Text))
        //        {
        //            flowLayout_panel.Controls.Remove(ctrl);
        //            ctrl.Dispose();
        //        }


        //    }

        //    foreach (DataRow row in data.Rows)
        //    {
        //        string branchName = row["branch_name"].ToString();

        //        if (existingButtons.Contains(branchName)) // Only add if not already present
        //        {
        //            Button dynamicButton = new Button
        //            {
        //                Text = branchName,
        //                Size = new Size(100, 50),
        //                BackColor = Color.LightBlue,
        //                Tag = row

        //            };

        //            dynamicButton.Click += DynamicButton_Clicks; // Attach the click event
        //            flowLayout_panel.Controls.Add(dynamicButton);
        //        }
        //    }
        //}


        private void EnableDisabledChildPanel(bool isEnabled)
        {
            panel_header_records.Enabled = isEnabled;
            panel_general.Enabled = isEnabled;
            panel_finance.Enabled = isEnabled;
            dg_contacts.Enabled = isEnabled;
            dg_address.Enabled = isEnabled;
            dg_items.Enabled = isEnabled;
            dg_accreditations.Enabled = isEnabled;
        }

     
        private void ShowTabPages(TabPage tabpage)
        {
            if (!tabControl2.TabPages.Contains(tabpage))
            {
                if (tabpage.Equals("ITEMS"))
                {
                    tabControl2.TabPages.Insert(4, tabpage);

                }
                else
                {
                    tabControl2.TabPages.Insert(3, tabpage);

                }
            }
        }

        private void ShowTypeOfEntity(string txt)
        {
            switch (txt)
            {
                case "NON-AFFILIATED":
                    ToogleEntityField(true);
                    ToogleCustomerAndSupplier(false);

                    break;
                case "AFFILIATED":
                    ToogleEntityField(false);
                    ToogleCustomerAndSupplier(false);

                    break;


                default:
                    ShowAffiliatedAndNon(false);

                    break;
            }
        }

        private void ShowAffiliatedAndNon(bool isShow)
        {
            lbl_affiliated.Visible = isShow;
            txt_affiliated.Visible = isShow;
            lbl_non_affiliated.Visible = isShow;
            txt_non_affiliated.Visible = isShow;

        }

        private void RemoveTabPages(TabPage tabpage)
        {
            if (tabControl2.TabPages.Contains(tabpage))
            {
                tabControl2.TabPages.Remove(tabpage);
            }
        }



        private bool ToogleItemPages(string text)
        {
            bool item = false;
            string[] valuesToCheck = { "SUPPLIER", "CUSTOMER" };
            var viewData = String.Join("", text);
            bool containsBoth = valuesToCheck.All(value => viewData.Contains(value));
            if (containsBoth)
            {
                ShowTabPages(tabItemPages);
                item = true;
            }
            else if (text.Contains("SUPPLIER"))
            {
                ShowTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                item = true;
            }
            else if (text.Contains("AFFILIATED"))
            {
                RemoveTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                item = false;
            }

            else if (text.Contains("NON-AFFILIATED"))
            {
                RemoveTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                item = false;
            }
            else
            {

                ShowTabPages(tabFinancePages);
                RemoveTabPages(tabItemPages);
                item = false;
            }
            return item;
        }


        
        private void ToogleEntityField(bool isShow)
        {
            txt_non_affiliated.Visible = isShow;
            lbl_non_affiliated.Visible = isShow;

            lbl_affiliated.Visible = !isShow;
            txt_affiliated.Visible = !isShow;
        }
        private void ToogleCustomerAndSupplier(bool isEnabled)
        {
            lbl_customer_code.Enabled = isEnabled;
            lbl_supplier_code.Enabled = isEnabled;
            txt_supplier_code.Enabled = isEnabled;
            txt_customer_code.Enabled = isEnabled;
        }
      
        private void RemoveAllId(Dictionary<string, dynamic> bpi, Dictionary<string, dynamic> general, Dictionary<string, dynamic> finance)
        {

            bpi.Remove("industries");
            general.Remove("general_id");
            general.Remove("general_based_id");
            general.Remove("branch_sales_id");
            finance.Remove("finance_id");
            finance.Remove("finance_based_id");
            finance.Remove("finance_branch_id");


        }

        private List<BpiContacts> SaveContacts(bool isUpdate)
        {

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_contacts);
            List<BpiContacts> listContacts = new List<BpiContacts>();


            BpiContacts contacts = null;
            int contacts_id = 0;
            int contacts_based_id = 0;
            int branch_id = 0;
            bool is_default_contact= false ;
            foreach (DataRow row in dataSource.Rows)
            {

                if (isUpdate)
                {

                    if (row.IsNull("contacts_id") || string.IsNullOrWhiteSpace(row["contacts_id"].ToString()) || row.IsNull("contacts_based_id") || string.IsNullOrWhiteSpace(row["contacts_based_id"].ToString()))
                    {
                        contacts_id = 0;
                        contacts_based_id = 0;
                        branch_id = 0;
                    }
                    else
                    {
                        contacts_id = int.Parse(row["contacts_id"].ToString());
                        contacts_based_id = int.Parse(row["contacts_based_id"].ToString());
                        branch_id = int.Parse(row["contacts_based_id"].ToString());
                    }

                }

                string number = row["number"].ToString();
                string email = row["email"].ToString();
                string name = row["name"].ToString();
                string preferences = row["preferences"].ToString();
                string notes = row["contact_notes"].ToString();

                if (row["is_default_contact"] != DBNull.Value && bool.TryParse(row["is_default_contact"].ToString(), out bool result))
                {
                    is_default_contact = result;
                }
                int contactPositionId;



                if (!int.TryParse(row["position"]?.ToString(), out contactPositionId))
                {
                    contactPositionId = 0;
                }

                contacts = new BpiContacts(contacts_id, contacts_based_id, number, name, email, preferences, contactPositionId, branch_id,notes,is_default_contact);
                listContacts.Add(contacts);

            }


            return listContacts;
        }

        private List<BpiAddress> SaveAddress(bool isUpdate)
        {
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_address);

            var allAddress = fullAddressRecords == null ? dataSource : fullAddressRecords;
            List<BpiAddress> listAddress = new List<BpiAddress>();

            BpiAddress address = null;
            int address_id = 0;
            int adrress_based_id = 0;
            int branch_id = 0;
            bool isDeleted = false;
            foreach (DataRow row in allAddress.Rows)
            {

                if (row.IsNull("address_ids") || string.IsNullOrWhiteSpace(row["address_ids"].ToString()) || row.IsNull("address_based_id") || string.IsNullOrWhiteSpace(row["address_based_id"].ToString()))
                {
                    address_id = 0;
                    adrress_based_id = 0;
                    branch_id = 0;
                }
                else
                {
                    address_id = int.Parse(row["address_ids"].ToString());
                    adrress_based_id = int.Parse(row["address_based_id"].ToString());

                    branch_id = int.Parse(row["address_branch_id"].ToString());
                    isDeleted = bool.Parse(row["address_is_deleted"].ToString());

                }

                string location = row["location"].ToString();
                address = new BpiAddress(address_id, adrress_based_id, location, branch_id, isDeleted);
                listAddress.Add(address);

            }

            return listAddress;
        }
        private List<BpiItems> SaveItems(bool isUpdate)
        {

            var dataItemSource = Helpers.ConvertDataGridViewToDataTable(dg_items);
            var allItems = fullItemsRecords == null ? dataItemSource : fullItemsRecords;
            var items = Helpers.GetControlsValues(panel_item);
            List<BpiItems> listItem = new List<BpiItems>();

            string taxCode = items["tax_code"].ToString();
            string itemTaxCode = items["item_tax_code"].ToString();
            int itemAccountId;
            int paymentTermsId;
            if (!int.TryParse(items["payment_terms_id"]?.ToString(), out paymentTermsId))
            {
                paymentTermsId = 0;
            }
            if (!int.TryParse(items["item_account_id"]?.ToString(), out itemAccountId))
            {
                itemAccountId = 0;
            }
            int itemId = 0;
            int basedItemId = 0;
            int bpiItemId = 0;
            int bpiItemBranchId = 0;
            BpiItems item = null;
            float unitPrice;
            bool unitPriceValid;
            bool isDeleted = false;
            foreach (DataRow row in allItems.Rows)
            {


                if (row.IsNull("item_id") || string.IsNullOrWhiteSpace(row["item_id"].ToString()) || row.IsNull("bpi_item_based_id") || string.IsNullOrWhiteSpace(row["bpi_item_based_id"].ToString()) || row.IsNull("bpi_item_id") || string.IsNullOrWhiteSpace(row["bpi_item_id"].ToString()))
                {
                    itemId = 0;
                    basedItemId = 0;
                    bpiItemId = 0;
                    bpiItemBranchId = 0;

                }
                else
                {
                    itemId = int.Parse(row["item_id"].ToString());
                    basedItemId = int.Parse(row["bpi_item_based_id"].ToString());
                    bpiItemId = int.Parse(row["bpi_item_id"].ToString());
                    bpiItemBranchId = int.Parse(row["bpi_item_branch_id"].ToString());
                    isDeleted = bool.Parse(row["item_is_deleted"].ToString());

                }
                //itemId = ;
                string notes = row["notes"].ToString();

                unitPriceValid = float.TryParse(row["price"].ToString(), out unitPrice);
                item = new BpiItems(bpiItemId, basedItemId, paymentTermsId, itemId, taxCode, itemTaxCode, unitPrice, notes, itemAccountId, isDeleted);

                listItem.Add(item);

            }

            return listItem;
        }


        private List<BpiAccreditation> SaveAccreditations(bool isUpdate)
        {
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_accreditations);
            List<BpiAccreditation> listAccreditation = new List<BpiAccreditation>();

            BpiAccreditation accreditations = null;
            int bpi_accreditation_id = 0;
            int branch_id = 0;
            int bpi_accreditation_based_id = 0;

            string file_path;
            foreach (DataRow row in dataSource.Rows)
            {

                if (isUpdate)
                {

                    if (row.IsNull("bpi_accreditation_id") || string.IsNullOrWhiteSpace(row["bpi_accreditation_id"].ToString()) || row.IsNull("bpi_accreditation_branch_id") || string.IsNullOrWhiteSpace(row["bpi_accreditation_branch_id"].ToString()))
                    {
                        bpi_accreditation_id = 0;
                        branch_id = 0;
                        bpi_accreditation_based_id = 0;
                    }
                    else
                    {
                        bpi_accreditation_id = int.Parse(row["bpi_accreditation_id"].ToString());
                        bpi_accreditation_based_id = int.Parse(row["bpi_accreditation_based_id"].ToString());
                        branch_id = int.Parse(row["bpi_accreditation_branch_id"].ToString());

                    }

                }

                string addedBy = row["accreditation_added_by"].ToString();
                string date_added = row["date_added"].ToString();
                if (!row["file_path"].ToString().StartsWith("./"))
                {
                    file_path = ConvertImageToBase64(row["file_path"].ToString());
                }
                else
                {
                    file_path = row["file_path"].ToString();
                }

                string file_name = row["file_name"].ToString();

                //  int accreditation_added_by_id = int.Parse(row["accreditation_added_by_id"].ToString());
                accreditations = new BpiAccreditation(bpi_accreditation_id, branch_id, date_added, file_path, bpi_accreditation_based_id, file_name, addedBy);
                listAccreditation.Add(accreditations);

            }

            return listAccreditation;


        }

        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

  
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.bpi.Rows.Count - 1 > this.selectedRecord)
            {
                //RemoveSelectedDataTable(CacheData.BranchIndustries);
                //RemoveSelectedDataTable(CacheData.Entity);
                this.selectedRecord++;
                BindBpiRecords(true);

            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {
                //RemoveSelectedDataTable(CacheData.BranchIndustries);
                //RemoveSelectedDataTable(CacheData.Entity);
                this.selectedRecord--;
                BindBpiRecords(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btn_update_Click(object sender, EventArgs e)
        {

            int generalBasedId = 0, generalId = 0;
            int financeBasedId, financeId,
                financePaymentTerms = 0,
                financeAccountId = 0,
                financeBranchId = 0;

            string selectedId = bpi.Rows[this.selectedRecord]["id"].ToString();

            //GetFilteredTabIds(general,"general_based_id", selectedId, "general_based_id","general_id",out generalBasedId, out generalId);
            GetFilteredTabIds(finance, "finance_based_id", selectedId, "finance_based_id", "finance_id", out financeBasedId, out financeId);



            DataView dataViewFinance = new DataView(finance);
            if (dataViewFinance.Count != 0)
            {
                dataViewFinance.RowFilter = "finance_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";
                financePaymentTerms = int.Parse(dataViewFinance[0]["finance_payment_terms_id"].ToString());
                financeAccountId = int.Parse(dataViewFinance[0]["finance_account_id"].ToString());
                financeBranchId = int.Parse(dataViewFinance[0]["finance_branch_id"].ToString());

            }

            if (currentSelectedEntityIds.Count != 0 && currentSelectedIndustryIds.Count != 0 && currentSelectedBranchIndustryIds.Count != 0)
            {
                txt_entity_type.Tag = currentSelectedEntityIds;
                txt_industries.Tag = currentSelectedIndustryIds;
                txt_branch_industry.Tag = currentSelectedBranchIndustryIds;
            }



            var Bpi = Helpers.GetControlsValues(panel_header_records);
            var Generals = Helpers.GetControlsValues(panel_general);


            var Contacts = SaveContacts(true);

            var Accreditations = SaveAccreditations(true);
            var Address = SaveAddress(true);
            var Items = SaveItems(true);

            dg_contacts.EndEdit();
            dg_contacts.CommitEdit(DataGridViewDataErrorContexts.Commit);




            var modifiedContact = Contacts.Select(c => new
            {
                id = c.contacts_id,  // Renaming contacts_id to id
                based_id = c.contacts_based_id,  // Renaming contacts_based_id to base_id
                number = c.number,
                name = c.name,
                email = c.email,
                preferences = c.preferences,
                position = c.position
            }).ToList();

            var modifiedAddress = Address.Select(c => new
            {
                id = c.address_ids,
                based_id = c.address_based_id,
                location = c.location,
                is_deleted = c.address_is_deleted
            }).ToList();

            var modifiedItems = Items.Select(c => new
            {
                id = c.bpi_item_id,
                based_id = c.bpi_item_based_id,
                payment_terms_id = c.payment_terms_id,
                item_account_id = c.item_account_id,
                item_id = c.item_id,
                tax_code = c.tax_code,
                item_tax_code = c.item_tax_code,
                price = c.price,
                notes = c.notes,
                is_deleted = c.item_is_deleted
            }).ToList();

            var modifiedAccreditations = Accreditations.Select(c => new
            {
                id = c.bpi_accreditation_id,
                based_id = c.bpi_accreditation_based_id,
                branch_id = c.bpi_accreditation_branch_id,
                date_added = c.date_added,
                file_name = c.file_name,
                file_path = c.file_path,
                accreditation_added_by = c.accreditation_added_by,
            }).ToList();

            //   Bpi["sales_id"] = CacheData.CurrentUser.employee_id;
            Generals["social_id"] = int.Parse(Generals["social_id"].ToString());
            Generals["id"] = int.TryParse(Generals["general_id"]?.ToString(), out generalId) ? generalId : 0;
            Generals["based_id"] = int.TryParse(Generals["general_based_id"]?.ToString(), out generalBasedId) ? generalBasedId : 0;


            // Generals.Add("id", int.TryParse(Generals["general_id"].ToString(), out generalId));
            // Generals.Add("based_id", int.TryParse(Generals["general_based_id"].ToString(), out generalBasedId));

            Generals.Add("sales_id", CacheData.CurrentUser.employee_id);

            Bpi.Remove("industries");
            Bpi.Remove("sales_id");
            Generals.Remove("general_id");
            Generals.Remove("general_based_id");
            Generals.Remove("branch_sales_id");

            if (txt_entity_type.Text.Contains("SUPPLIER"))
            {

                Bpi.Add("items", modifiedItems);
            }
            else if (txt_entity_type.Text.Contains("CUSTOMER"))
            {
                var Finance = Helpers.GetControlsValues(panel_finance);
                Finance["finance_id"] = financeId;
                Finance["finance_based_id"] = financeBasedId;
                Finance["finance_branch_id"] = financeBranchId;
                Finance["finance_payment_terms_id"] = financePaymentTerms;
                Finance["finance_account_id"] = financeAccountId;

                Bpi.Add("finance", Finance);

            }

            Bpi.Add("general", Generals);
            Bpi.Add("contacts", modifiedContact);
            Bpi.Add("address", modifiedAddress);
            Bpi.Add("accreditations", modifiedAccreditations);

            bool response = await BpiServices.Update(Bpi);

            if (response)
            {

                MessageBox.Show("SHOW SENIOR LEM");
            }
            else
            {
                MessageBox.Show("SHOW SENIOR JOSH");
            }
        }
        private void ResetData(bool isIncluded)
        {

            // panel_general.Visible = true;

            BpiBranchToggle();




            BtnToogle(true);
            btn_add.Visible = true;
            btn_add.Location = new System.Drawing.Point(1069, 28);

            if (isIncluded)
            {
                Helpers.ResetControls(panel_header_records);
                cmb_name.Text = "";
                button5.Enabled = false;

                Helpers.ResetControls(panel_general);
                Helpers.ResetControls(panel_item);
                Helpers.ResetControls(panel_finance);


            }
            cmb_social.SelectedIndex = 0;
            cmb_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_account.DataSource = CacheData.PaymentTerms;


            cmb_tax_code.DataSource = ENUM_TAX_CODE.LIST();
            cmb_tax_code.DisplayMember = "title";
            cmb_tax_code.Text = "VAT";
            cmb_payment_terms.Text = "COD";
            cmb_finance_payment_terms.Text = "COD";
            cmb_finance_account.Text = "COD";

            if (txt_entity_type.Text.Contains("Supplier"))
            {
                cmb_payment_terms.SelectedIndex = 0;
            }

            currentSelectedIndustryIds.Clear();
            currentSelectedEntityIds.Clear();
            currentSelectedBranchIndustryIds.Clear();
            selectedPreferenceNames.Clear();

            DataTable clonedContacts = contacts.Clone();

            DataRow newRow = clonedContacts.NewRow();   // Create a new row
            newRow["position"] = DBNull.Value;
            clonedContacts.Rows.Add(newRow);

            dataBindingContacts.DataSource = clonedContacts;
            dataBindingAddress.DataSource = address.Clone();



    
            dataBindingItems.DataSource = items.Clone();

            dataBindingFinancePending.DataSource = finance_pending.Clone();
            databindingAccreditation.DataSource = accreditations.Clone();

            RemoveSelectedDataTable(CacheData.Industries);
            RemoveSelectedDataTable(CacheData.BranchIndustries);
            RemoveSelectedDataTable(CacheData.Entity);
            RemoveTabPages(tabItemPages);
            RemoveTabPages(tabFinancePages);

            ToogleCustomerAndSupplier(true);
            ShowAffiliatedAndNon(false);


        }
        private void ResetSelectedData()
        {

            txt_branch_tel_no.Text = "";
            txt_branch_website.Text = "";
            txt_notes.Text = "";
            cmb_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_finance_account.DataSource = CacheData.PaymentTerms;

            cmb_social.SelectedIndex = 0;
            cmb_tax_code.DataSource = ENUM_TAX_CODE.LIST();
            cmb_tax_code.DisplayMember = "title";
            cmb_tax_code.Text = "VAT";
            cmb_payment_terms.Text = "COD";
            cmb_finance_payment_terms.Text = "COD";
            cmb_finance_account.Text = "COD";

            if (txt_entity_type.Text.Contains("Supplier"))
            {
                cmb_payment_terms.SelectedIndex = 0;
            }

            currentSelectedIndustryIds.Clear();
            currentSelectedEntityIds.Clear();
            currentSelectedBranchIndustryIds.Clear();
            selectedPreferenceNames.Clear();

            DataTable clonedContacts = contacts.Clone();
            DataRow newRow = clonedContacts.NewRow();   // Create a new row
            newRow["position"] = 1;
            clonedContacts.Rows.Add(newRow);

            dataBindingContacts.DataSource = clonedContacts;
            dataBindingAddress.DataSource = address.Clone();
            dataBindingItems.DataSource = items.Clone();
            dataBindingFinancePending.DataSource = finance_pending.Clone();
            databindingAccreditation.DataSource = accreditations.Clone();

            RemoveSelectedDataTable(CacheData.Industries);
            RemoveSelectedDataTable(CacheData.BranchIndustries);
            RemoveSelectedDataTable(CacheData.Entity);
            RemoveTabPages(tabItemPages);
            RemoveTabPages(tabFinancePages);

            ToogleCustomerAndSupplier(true);
            ShowAffiliatedAndNon(false);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            ResetData(true);
            txt_branch_industry.Tag = "MULTI";
            txt_entity_type.Tag = "MULTI";
            flowLayout_panel.Controls.Clear();
            txt_sales_id.Text = $"({CacheData.CurrentUser.first_name.Substring(0, 1).ToUpper()}. {CacheData.CurrentUser.last_name})";
            txt_branch_sales_id.Text = CacheData.CurrentUser.employee_id;
            Panel[] pnlList = { panel_header_records };
            var Bpi = Helpers.GetControlsValues(panel_header_records);


            if (Bpi["id"] == "")
            {

                BtnBindData();

                txt_branch_name.Visible = false;
                lbl_branch_name.Visible = false;
            }
            else
            {
                txt_branch_name.Visible = true;
                lbl_branch_name.Visible = true;
            }



        }


      

        private void RemoveSelectedDataTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {

                if (dt.Columns.Contains("select"))
                {
                    row["select"] = false;

                }

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            modalSetup = new SetupModal("Branch Industries", ENUM_ENDPOINT.INDUSTRIES, CacheData.Industries);
            DialogResult r = modalSetup.ShowDialog();
        }

        private void btn_add_setup_Click(object sender, EventArgs e)
        {
           
            DataTable dt = CacheData.Industries.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Industries", ENUM_ENDPOINT.INDUSTRIES, dt);
            DialogResult r = modalSetup.ShowDialog();
        }

        private void btn_branch_industry_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.BranchIndustries.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }
            modalSetup = new SetupModal("Branch Industries", ENUM_ENDPOINT.INDUSTRIES, dt);
            modalSetup.ShowDialog();

        }

        private void btn_social_links_Click(object sender, EventArgs e)
        {
            modalSetup = new SetupModal("Social Media ", ENUM_ENDPOINT.SOCIALS, CacheData.SocialMedia);
            modalSetup.ShowDialog();
        }


        private void btn_add_industries_Click(object sender, EventArgs e)
        {
            //DataTable branchData;

            //if (string.IsNullOrEmpty(txt_id.Text))
            //{
            //    branchData = CacheData.Industries;
            //}
            //else
            //{
            //    branchData = CacheData.BranchIndustries;
            //}


            modalSelection = new SetupSelectionModal("Industries", ENUM_ENDPOINT.INDUSTRIES, CacheData.Industries, currentSelectedIndustryIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();

            if (modalResult == DialogResult.OK)
            {

                var result = modalSelection.GetResult();

                Helpers.GetModalData(txt_industries, result);
                CopyToMainBranchField("industries", txt_industries.Text);
                currentSelectedIndustryIds.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_customer_code.Text = "";
            txt_supplier_code.Text = "";
            txt_non_affiliated.Text = "";
            txt_affiliated.Text = "";

            modalSelection = new SetupSelectionModal("ENTITY", ENUM_ENDPOINT.ENTITY, CacheData.Entity, currentSelectedEntityIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();

            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();

                Helpers.GetModalData(txt_entity_type, result);
                var data = txt_entity_type.Text;

                string[] entities = data.Split(',');
                bool hasBlackListed = entities.Any(n => n.Trim() == ENUM_ENTITY_TYPE.Blacklisted);
                bool hasTempSupplier = entities.Any(n => n.Trim() == ENUM_ENTITY_TYPE.TempSupplier);

                if (hasBlackListed)
                {
                    txt_entity_type.Text = "";
                    txt_entity_type.Tag = null;
                    currentSelectedEntityIds.Clear();
                    MessageBox.Show("Cannot select BLACKLISTED based on your position", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (CanvassForm == "" && hasTempSupplier)
                {
                    txt_entity_type.Text = "";
                    txt_entity_type.Tag = null;
                    currentSelectedEntityIds.Clear();
                    MessageBox.Show("You Cannot Select Temporary Supplier", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    currentSelectedEntityIds.Clear();
                    string[] valuesToCheck = { "SUPPLIER", "CUSTOMER" };
                    var viewData = String.Join("", txt_entity_type.Text);
                    bool containsBoth = valuesToCheck.All(value => viewData.Contains(value));

                    if (containsBoth)
                    {
                        DocumentCodeIncrementor("BOTH");
                        ToogleCustomerAndSupplier(true);
                        ShowTabPages(tabItemPages);
                        ShowTabPages(tabFinancePages);
                    }
                    else if (txt_entity_type.Text.Contains(ENUM_ENTITY_TYPE.Supplier))
                    {
                        DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Supplier);
                        ToogleCustomerAndSupplier(true);
                        ShowAffiliatedAndNon(false);
                        ShowTabPages(tabItemPages);
                        RemoveTabPages(tabFinancePages);

                        txt_customer_code.Enabled = false;

                        //  txt_supplier_code.Text = "S#" + number.ToString();

                    }

                    else if (txt_entity_type.Text.Contains(ENUM_ENTITY_TYPE.Non_Affiliated))
                    {
                        // txt_non_affiliated.Text = "EN#" + number.ToString();
                        DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Non_Affiliated);
                        ToogleEntityField(true);
                        ToogleCustomerAndSupplier(false);
                        RemoveTabPages(tabFinancePages);
                        RemoveTabPages(tabItemPages);

                    }
                    else if (txt_entity_type.Text.Contains(ENUM_ENTITY_TYPE.Affiliated))
                    {
                        //   txt_affiliated.Text = "EA#" + number.ToString();
                        DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Affiliated);
                        ToogleEntityField(false);
                        ToogleCustomerAndSupplier(false);
                        RemoveTabPages(tabFinancePages);
                    }
                    else
                    {
                        DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Customer);
                        ShowAffiliatedAndNon(false);
                        ShowTabPages(tabFinancePages);
                        ToogleCustomerAndSupplier(true);
                        RemoveTabPages(tabItemPages);
                        txt_supplier_code.Enabled = false;
                        btn_finance_payment_terms.Visible = CacheData.CurrentUser.position_id.Equals("Web Developer"); // Parameter is ready for manager position only

                        //    tabControl2.TabPages.Remove(tabItemPages);
                    }

                }

            }

            //string[] valuesToCheck = { "SUPPLIER", "CUSTOMER" };
            //var viewData = String.Join("", txt_entity_type.Text);
            //bool containsBoth = valuesToCheck.All(value => viewData.Contains(value));


            //if (containsBoth)
            //{

            //    DocumentCodeIncrementor("BOTH");
            //    ToogleCustomerAndSupplier(true);
            //    ShowTabPages(tabItemPages);
            //    ShowTabPages(tabFinancePages);

            //}
            //else if (txt_entity_type.Text.Contains("SUPPLIER"))
            //{
            //    DocumentCodeIncrementor("SUPPLIER");
            //    ToogleCustomerAndSupplier(true);
            //    ShowAffiliatedAndNon(false);
            //    ShowTabPages(tabItemPages);
            //    RemoveTabPages(tabFinancePages);


            //    txt_customer_code.Enabled = false;


            //    //  txt_supplier_code.Text = "S#" + number.ToString();

            //}

            //else if (txt_entity_type.Text.Contains("NON-AFFILIATED"))
            //{
            //    // txt_non_affiliated.Text = "EN#" + number.ToString();
            //    DocumentCodeIncrementor("NON-AFFILIATED");
            //    ToogleEntityField(true);
            //    ToogleCustomerAndSupplier(false);
            //    RemoveTabPages(tabFinancePages);
            //    RemoveTabPages(tabItemPages);

            //}
            //else if (txt_entity_type.Text.Contains("AFFILIATED"))
            //{
            //    //   txt_affiliated.Text = "EA#" + number.ToString();
            //    DocumentCodeIncrementor("AFFILIATED");
            //    ToogleEntityField(false);
            //    ToogleCustomerAndSupplier(false);
            //    RemoveTabPages(tabFinancePages);

            //}
            //else
            //{
            //    DocumentCodeIncrementor("CUSTOMER");
            //    ShowAffiliatedAndNon(false);
            //    ShowTabPages(tabFinancePages);
            //    ToogleCustomerAndSupplier(true);
            //    RemoveTabPages(tabItemPages);


            //    txt_supplier_code.Enabled = false;

            //    //    tabControl2.TabPages.Remove(tabItemPages);
            //}


        }

       

       
      


        private void btn_get_branch_Click(object sender, EventArgs e)

        {
            DataTable branchData;

            if (string.IsNullOrEmpty(txt_id.Text))
            {
                branchData = CacheData.Industries;
            }
            else
            {
                branchData = CacheData.BranchIndustries;
            }


            modalSelection = new SetupSelectionModal("Branch Industries", ENUM_ENDPOINT.INDUSTRIES, branchData, currentSelectedBranchIndustryIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();


            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();
                Helpers.GetModalData(txt_branch_industry, result);

                CopyToMainBranchField("branch_industries", txt_branch_industry.Text);
                currentSelectedBranchIndustryIds.Clear();

            }

        }
      

        private void btn_add_entity_Click(object sender, EventArgs e)
        {

            DataTable dt = CacheData.Entity.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }
            modalSetup = new SetupModal("Entity", ENUM_ENDPOINT.ENTITY, dt);
            modalSetup.ShowDialog();
        }

        private void cmb_found_us_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_customer_code_TextChanged(object sender, EventArgs e)
        {

        }


        private bool AddBpiIdentificationType(Dictionary<string, dynamic> records, out string bpiMesssages)
        {


            bpiMesssages = string.Empty;
            int temporaryId = 0;

            string industries = txt_industries.Text;
            string mainTelNo = txt_main_tel_no.Text;
            string tin = txt_tin.Text;
            if (string.IsNullOrEmpty(industries))
            {
                bpiMesssages += "Industries is required \n";
                return false;
            }

            if (string.IsNullOrEmpty(mainTelNo))
            {
                bpiMesssages += "Main Tel No. is required \n";
                return false;

            }
            if (string.IsNullOrEmpty(tin))
            {
                bpiMesssages += "Tin No. is required \n";
                return false;

            }

            if (!Regex.IsMatch(mainTelNo, REGEXPATTERN))
            {
                bpiMesssages += "Main Tel No. is Invalid \n";
                return false;
            }

            else
            {
                if (records["id"] is string strId && string.IsNullOrEmpty(strId))
                {
                    temporaryId = 0;
                    records.Remove("id");
                    records.Add("id", temporaryId);
                }
                else if (records["id"] is int intId && intId == 0)
                {
                    temporaryId = 0;
                    records.Add("id", temporaryId);
                }
                return true;
            }




        }

        private bool GeneralValidations(Dictionary<string, dynamic> records, out string generalMessage)
        {
            generalMessage = string.Empty;

            string entityType = txt_entity_type.Text;
            string branchIndustries = txt_branch_industry.Text;
            string branchTelNo = txt_branch_tel_no.Text;
            if (string.IsNullOrEmpty(entityType) || string.IsNullOrEmpty(branchIndustries))
            {
                generalMessage += "Entity Type and Branch Industries is required \n";
                return false;
            }
            else if (!Regex.IsMatch(branchTelNo, REGEXPATTERN))
            {
                generalMessage += "Branch Tel No. is Invalid \n";
                return false;
            }
            return true;


        }
        private bool ContactsValidations(List<BpiContacts> records, out string contactsMessages)
        {
            bool checkContactError= false;
            contactsMessages = string.Empty;
            if (records.Count == 1 && records[0].number == "")
            {
                contactsMessages += "Contacts is required";
                return false;
            }


            foreach(BpiContacts contact in records)
            {
                string cleanedNumber = contact.number.Trim().Replace(" ", "").Replace("-", "");
                if (checkContactError)
                {
                    break;
                }
                else
                {
                    if (string.IsNullOrEmpty(contact.email) && string.IsNullOrEmpty(contact.number) && string.IsNullOrEmpty(contact.name) )
                    {

                        contactsMessages += "Input email,number or name to proceed";
                        checkContactError = true;
                    }
                    else if (!contact.is_default_contact)
                    {

                        contactsMessages += "You need atleast 1 default selected contact to proceed";
                        checkContactError = true;
                    }


                    else if (!IsValidLandlineNumber(cleanedNumber) && !IsValidMobileNumber(cleanedNumber) )
                    {
                        contactsMessages += "Invalid Contact Number";
                        checkContactError = true;
                    }
                   
                    else
                    {
                        checkContactError = false;
                    }
                }
                

            }
            if (checkContactError)
            {
                return false ;
            }



            return true;
        }
        private bool AddressValidation(List<BpiAddress> records, out string addressMessages)
        {
            addressMessages = string.Empty;
            if (records.Count == 0 || records[0].location == "")
            {
                addressMessages += "Address is required";
                return false;
            }
            return true;
        }
        private async void btn_add_Click(object sender, EventArgs e)
        {
            string generalMessage = "";
            string bpiGeneralMessage = "";
            string contactMessage = "";
            string addressMessage = "";

            var Bpi = Helpers.GetControlsValues(panel_header_records);
            var Generals = Helpers.GetControlsValues(panel_general);
            var Contacts = SaveContacts(false);
            var Address = SaveAddress(false);

            bool isBpiValidated = AddBpiIdentificationType(Bpi, out bpiGeneralMessage);
            bool isGeneralValidated = GeneralValidations(Generals, out generalMessage);
            bool isContactValidated = ContactsValidations(Contacts, out contactMessage);
            bool isAddressValidated = AddressValidation(Address, out addressMessage);


            List<string> errorMessages = new List<string>();

            if (!isBpiValidated) errorMessages.Add(bpiGeneralMessage);
            if (!isGeneralValidated) errorMessages.Add(generalMessage);
            if (!isContactValidated) errorMessages.Add(contactMessage);
            if (!isAddressValidated) errorMessages.Add(addressMessage);

            if (errorMessages.Count > 0)
            {
                string fullMessage = string.Join("\n• ", errorMessages.Where(m => !string.IsNullOrWhiteSpace(m)));
                MessageBox.Show("Please address the following issues:\n\n• " + fullMessage, "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            if (Bpi["id"] == 0)
            {
                Generals["branch_name"] = Bpi["name"];
                Generals["branch_industry_id"] = Bpi["industries_id"];
            }


            Bpi["sales_id"] = CacheData.CurrentUser.employee_id;
            Generals["sales_id"] = CacheData.CurrentUser.employee_id;
            int social_id = 0;
            
       
            if (!int.TryParse(Generals["social_id"]?.ToString(),out social_id))
            {
                social_id = 0;
            }

            Generals["social_id"] = social_id;


            var Finance = Helpers.GetControlsValues(panel_finance);
            var Accreditations = SaveAccreditations(false);
          
            if (txt_entity_type.Text.Contains(ENUM_ENTITY_TYPE.Supplier))
            {
                var Items = SaveItems(false);
                Bpi.Add("items", Items);
            }
          
            else if (txt_entity_type.Text.Contains(ENUM_ENTITY_TYPE.Customer))
            {
                Bpi.Add("finance", Finance);
            }

            Bpi.Add("general", Generals);
            Bpi.Add("contacts", Contacts);
            Bpi.Add("address", Address);
            Bpi.Add("accreditations", Accreditations);
            RemoveAllId(Bpi, Generals, Finance);

            var response = await BpiServices.Insert(Bpi);

            var data = response.Data;

            if (response.Success)
            {

                MessageBox.Show("Bpi record added Succesfully", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                button5.Enabled = true;
                txt_id.Text = data["id"];

                Generals.Add("general_id", data["general"]["id"].ToString());
                Generals.Add("general_based_id", data["general"]["based_id"].ToString());
                Generals["general_id"] = int.Parse(Generals["general_id"].ToString());
                Generals["general_based_id"] = int.Parse(Generals["general_based_id"].ToString());
                Generals.Add("branch_industry_ids", Generals["branch_industry_id"]);
                Generals.Add("entity_ids", Generals["entity_type_id"]);
                Generals.Add("branch_sales_id", Generals["sales_id"]);


                // Ensure entity_type_id exists and is a list of IDs
                GeneralMultiSelectText("entity_type_id", "entity_names", Generals, CacheData.Entity);
                GeneralMultiSelectText("branch_industry_id", "branch_industry_names", Generals, CacheData.BranchIndustries);

                DataRow rows = AddDictionaryToDataTable(general, Generals);
                general.Rows.Add(rows);

                DataView dataViewGeneral = new DataView(general);

                if (dataViewGeneral.Count != 0)
                {
                    dataViewGeneral.RowFilter = "general_based_id = '" + txt_id.Text + "'";

                    txt_branch_name.Visible = dataViewGeneral.Count > 1;
                    lbl_branch_name.Visible = dataViewGeneral.Count > 1;
                }

                DataTable filteredGeneral = dataViewGeneral.ToTable();
                GetAllBpiBranch(filteredGeneral);

                btn_new.Visible = true;
                btn_search.Visible = true;
                btn_prev.Visible = true;
                btn_next.Visible = true;
                btn_edit.Visible = true;
                EnableDisabledChildPanel(false);

            }
            else
            {
                Helpers.ShowDialogMessage("error", string.IsNullOrEmpty(response.message) ? "Operation Fail" : response.message);
            }

        }
        private void GeneralMultiSelectText(string identity, string field, Dictionary<string, dynamic> dt, DataTable modalSetup)
        {

            if (dt.TryGetValue(identity, out var FieldObj) && FieldObj is List<int> SelectedIds)
            {
                if (modalSetup != null)
                {
                    var matchedNames = modalSetup.AsEnumerable()
                        .Where(ee => SelectedIds.Contains(ee.Field<int>("id")))  // Match by Id column
                        .Select(ee => ee.Field<string>("name"))  // Select Name column
                        .ToList();

                    dt[field] = string.Join(", ", matchedNames);
                }
            }

        }

        private DataRow AddDictionaryToDataTable(DataTable dt, Dictionary<string, dynamic> model)
        {
            DataRow row = dt.NewRow();
            foreach (var key in model.Keys)
            {
                if (dt.Columns.Contains(key)) // Ensure the column exists
                {
                    var value = model[key];

                    // Check if the value is a List<int>
                    if (value is List<int> list)
                    {
                        row[key] = string.Join(",", list); // Convert list to a comma-separated string
                    }
                    else
                    {
                        row[key] = value; // Assign normally if not a list
                    }
                }
            }
            return row;

        }


        private async void Refetch()
        {
            var response = await RequestToApi<ApiResponseModel<Bpi_Class>>.Get("/bpi");
            records = response.Data;

            bpi = JsonHelper.ToDataTable(records.bpi);

        }

      

        private void txt_main_website_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_industry_TextChanged(object sender, EventArgs e)
        {

        }

        private void footer_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_header_records_Paint(object sender, PaintEventArgs e)
        {

        }
        private void cmb_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_id.Text == "")
            {

                if (cmb_name.SelectedIndex != -1)
                {
                    string selectedValue = cmb_name.SelectedItem.ToString();
                    var listOfNames = records.bpi.Select(item => item.name).ToList();
                    bool exists = listOfNames.Contains(selectedValue, StringComparer.OrdinalIgnoreCase);
                    if (exists)
                    {
                        MessageBox.Show("Cannot select existing client", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_name.SelectedIndex = -1;
                    }

                }

            }


        }

        private void txt_entity_type_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                if (dg_contacts.Columns[e.ColumnIndex].Name == "ADD_PREF")
                {

                    int index = e.RowIndex;
                    DataTable filterSocialMedia = CacheData.SocialMedia.AsEnumerable()  // it doesnt include the select field
                        .Where(row => !row.Field<string>("name").Contains("-"))
                        .CopyToDataTable();



                    modalSelection = new SetupSelectionModal("Preferences", ENUM_ENDPOINT.SOCIALS, filterSocialMedia, new List<int> { }, selectedPreferenceNames, index);
                    DialogResult modalResult = modalSelection.ShowDialog();

                    if (modalResult == DialogResult.OK)
                    {
                        DataView result = modalSelection.GetResult(); // Get the DataView
                        var selectedPreferences = result.Cast<DataRowView>()
                       .Select(row => row["code"].ToString())
                       .ToList();

                        if (selectedPreferenceNames.Count != 0)
                        {
                            selectedPreferenceNames[index] = string.Join(",", selectedPreferences); // to change the value  selectedPreferenceNames when it adds 
                        }

                        dg_contacts.Rows[e.RowIndex].Cells["preferences"].Value = string.Join(",", selectedPreferences);

                    }


                }
            }
        }

        private void dataGridView1_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }

        private void dataBindingPosition_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataBindingContacts_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }


        private void cmb_item_payment_terms_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dg_items.Columns[e.ColumnIndex].Name == "item_code")
                {
                    dg_items.EndEdit();
                    dataBindingItems.EndEdit();
                    ItemModal modal = new ItemModal();
                    DialogResult r = modal.ShowDialog();

                    if (r == DialogResult.OK)
                    {
                        Dictionary<string, dynamic> result = modal.GetResult();

                        // Update current row in DataGridView
                        DataGridViewRow selectedRow = dg_items.Rows[e.RowIndex];

                        selectedRow.Cells["item_id"].Value = result["item_id"];
                        selectedRow.Cells["item_code"].Value = result["item_code"];
                        selectedRow.Cells["short_desc"].Value = result["short_desc"];
                        selectedRow.Cells["status_tangible"].Value = result["status_tangible"];
                        selectedRow.Cells["status_trade"].Value = result["status_trade"];
                        selectedRow.Cells["price"].Value = result["item_price"];

                        // Add new empty row to data source (assumes DataTable binding)

                    }


                  
                }
            }

        }
        //private void dg_items_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        if (dg_items.Columns[e.ColumnIndex].Name == "item_code")
        //        {
        //            ItemModal modal = new ItemModal();
        //            DialogResult r = modal.ShowDialog();

        //            if (r == DialogResult.OK)
        //            {
        //                Dictionary<string, dynamic> result = modal.GetResult();

        //                // Get the bound DataTable
        //                DataTable currentTable = dataBindingItems.DataSource as DataTable;
        //                if (currentTable != null)
        //                {


        //                    // Insert modal-selected item at the top
        //                    DataRow filledRow = currentTable.NewRow();
        //                    filledRow["item_id"] = result["item_id"];
        //                    filledRow["item_code"] = result["item_code"];
        //                    filledRow["short_desc"] = result["short_desc"];
        //                    filledRow["status_tangible"] = result["status_tangible"];
        //                    filledRow["status_trade"] = result["status_trade"];
        //                    filledRow["price"] = result["item_price"]; // or 0 if price comes from user

        //                    currentTable.Rows.InsertAt(filledRow, 0);

        //                    // Then insert a blank row after it
        //                    DataRow emptyRow = currentTable.NewRow();
        //                    emptyRow["item_id"] = DBNull.Value;
        //                    emptyRow["item_code"] = "";
        //                    emptyRow["short_desc"] = "";
        //                    emptyRow["status_tangible"] = "";
        //                    emptyRow["status_trade"] = "";
        //                    emptyRow["price"] = 0;

        //                    currentTable.Rows.InsertAt(emptyRow, 1);

        //                    // Optional: focus the empty row’s item_code cell
        //                 //   dg_items.CurrentCell = dg_items.Rows[1].Cells["item_code"];
        //                }
        //            }
        //        }
        //    }
        //}

        private void dg_items_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

         

        }

        private void dg_items_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_items_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmb_tax_code_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {



            general.Rows.Add(0, 0, 0);
            DataRow lastRow = general.Rows[general.Rows.Count - 1]; // Get last row
            Button dynamicButton = new Button();
            groupCount++;
            // Set properties
            dynamicButton.Text = "Group" + groupCount;
            dynamicButton.Size = new Size(100, 50);
            dynamicButton.Location = new Point(50, 50);
            dynamicButton.BackColor = Color.LightBlue;
            dynamicButton.Tag = lastRow;

            // Add the button to the form
            dynamicButton.Click += DynamicButton_Clicks;
            flowLayout_panel.Controls.Add(dynamicButton);

            Panel[] pnlGeneralPanel = { panel_general };

            Helpers.BindControls(pnlGeneralPanel, general, general.Rows.Count - 1);
            if (!string.IsNullOrEmpty(txt_id.Text))
            {
                txt_entity_type.Tag = "MULTI";
                txt_industries.Tag = "MULTI";
                txt_branch_industry.Tag = "MULTI";
                txt_branch_name.Visible = true;
                lbl_branch_name.Visible = true;
            }
            button5.Enabled = false;
            //btn_update.Visible = false;
            ResetData(false);

            Helpers.ResetControls(panel_general);
            Helpers.ResetControls(panel_item);
            Helpers.ResetControls(panel_finance);
            EnableDisabledChildPanel(true);
        }





        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void panel_header_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_add_new_item_Click(object sender, EventArgs e)
        {

        }

       

        private void cmb_tax_code_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

    

        private void txt_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void dg_contacts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {


            var row = dg_contacts.Rows[e.RowIndex];
            var contactCell = row.Cells[1];
            var nameCell = row.Cells[2];
            var positionNameCell = row.Cells[6];

            string contactRaw = contactCell.Value?.ToString()?.Trim() ?? "";
            string nameRaw = nameCell.Value?.ToString()?.Trim() ?? "";
            string positionRawName = positionNameCell.Value?.ToString()?.Trim() ?? "";

            string contactUnformatted = Regex.Replace(contactRaw, @"[\s\-\(\)]", "");

            // Check if both are empty
            if (string.IsNullOrEmpty(contactUnformatted) && string.IsNullOrEmpty(nameRaw) && string.IsNullOrEmpty(positionRawName))
            {
                string errorMessage = "Input email , number or name to proceed.";
                if (e.ColumnIndex == 1)
                    contactCell.ErrorText = errorMessage;
                if (e.ColumnIndex == 2)
                    nameCell.ErrorText = errorMessage;

                if (e.ColumnIndex == 6)
                    positionNameCell.ErrorText = errorMessage;
                return;
            }

            // Clear error if either is valid
            contactCell.ErrorText = "";
            nameCell.ErrorText = "";
            positionNameCell.ErrorText = "";

            if (e.ColumnIndex == 1)
            {
                if (!string.IsNullOrEmpty(contactUnformatted))
                {
                    if (IsValidMobileNumber(contactUnformatted))
                    {
                        contactCell.Style.ForeColor = Color.Black;
                        contactCell.Value = FormatMobileNumber(contactUnformatted);
                    }
                    else if (IsValidLandlineNumber(contactUnformatted))
                    {
                        contactCell.Style.ForeColor = Color.Black;
                        contactCell.Value = FormatLandlineNumber(contactUnformatted);
                    }
                    else
                    {
                        contactCell.Style.ForeColor = Color.Red;
                        contactCell.Value = contactUnformatted;
                        contactCell.ErrorText = "Invalid contact number.";
                    }
                }
            }

            if (e.ColumnIndex == 2)
            {
                // You may also add optional email format validation here
                nameCell.Style.ForeColor = Color.Black;
            }

        }

        private void cmb_finance_payment_terms_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                if (dg_accreditations.Columns[e.ColumnIndex].Name == "btn_upload")
                {

                    MessageBox.Show("Hi");
                    // openFileDialog1
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Get the path of specified file
                            string filePath = openFileDialog.FileName;
                            string fileName = Path.GetFileName(filePath);
                            DateTime now = DateTime.Now;

                            this.dg_accreditations.Rows[e.RowIndex].Cells[0].Value = now.ToString("yyyy-MM-dd HH:mm:ss");
                            this.dg_accreditations.Rows[e.RowIndex].Cells[1].Value = fileName;
                            this.dg_accreditations.Rows[e.RowIndex].Cells[3].Value = "JEROME OBOGNE ";
                        }
                    }

                }
            }
        }

        private void dg_accreditations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_upload_image_Click(object sender, EventArgs e)
        {
            var userAdded = txt_sales_id.Text;
            DateTime now = DateTime.Now;
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_accreditations);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    foreach (string files in openFileDialog.FileNames)
                    {

                        string fileName = Path.GetFileName(files);

                        dataSource.Rows.Add(now, fileName, userAdded, files);
                        databindingAccreditation.DataSource = dataSource;
                    }
                }
            }


        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            // var Accreditations = SaveAccreditations(false); ;
            BtnToogle(false);
            btn_add.Visible = false;
            btn_update.Visible = false;
            GetBpi();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            var isSelectedSales = GetSelectedSales();
            BpiBranchToggle(isSelectedSales);
            BtnToogle(true);
            btn_update.Visible = true;
            btn_add.Visible = false;
        }

        private void btn_add_new_item_Click_1(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ItemEntryModal>().Any())
            {
                return; // Prevent opening if already open
            }
            ItemEntryModal itemModal = new ItemEntryModal();

            itemModal.OnAddItem += AddNewBpiItem;
            itemModal.StartPosition = FormStartPosition.CenterParent;
            itemModal.ShowDialog();

        }
        public void AddNewBpiItem(Dictionary<string, dynamic> value)
        {

            Dictionary<string, dynamic> Bpi_Item = value;
            DataTable itemList = Helpers.ConvertDataGridViewToDataTable(dg_items);

            DataRow addedRow = itemList.NewRow();
            foreach (var item in Bpi_Item)
            {
                if (itemList.Columns.Contains(item.Key))
                {
                    addedRow[item.Key] = item.Value ?? DBNull.Value;
                }
            }
            itemList.Rows.Add(addedRow);
            dataBindingItems.DataSource = itemList;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (bpi == null || bpi.Rows.Count == 0)
            {
                MessageBox.Show("No Bpi available  for selections", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
            {
                 { "name", "NAME" },
                    { "tin", "TIN" },
                    { "main_website", "MAIN WEBSITE" },
                    { "main_tel_no", "MAIN TEL NO" }
            };

            var count = bpi.Rows.Count;
            using (SearchModal searchModal = new SearchModal("Search Bpi", bpi, columnMappings))
            {

                if (searchModal.ShowDialog() == DialogResult.OK)
                {
                    int selectedBpiIndex = searchModal.SelectedIndex;
                    if (selectedBpiIndex >= 0)
                    {
                        this.selectedRecord = selectedBpiIndex;
                        BindBpiRecords(true);

                    }
                }
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void dg_contacts_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            var value = e.Row.DataBoundItem;
        }

        private void dg_address_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void dg_address_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var selectedAddress = e.Row.DataBoundItem;
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_address);

            if (selectedAddress != null)
            {

                if (selectedAddress is DataRowView address)
                {
                    if (address.Row.Table.Columns.Contains("address_is_deleted"))
                    {
                        address["address_is_deleted"] = true;
                    }
                    string selectedId = address["address_ids"].ToString();

                    var addressRow = dataSource.AsEnumerable().FirstOrDefault(r => r.Field<string>("address_ids") == selectedId);

                    if (addressRow != null)
                    {
                        addressRow.SetField("address_is_deleted", address["address_is_deleted"]); // Change the Name column

                    }
                    fullAddressRecords = dataSource;

                    DataView dataViewAddress = new DataView(dataSource);
                    DataTable filteredAddress = dataViewAddress.ToTable();
                    dataBindingAddress.DataSource = filteredAddress;

                }

            }
        }

        private void dg_items_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

            var selectedItem = e.Row.DataBoundItem;
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_items);

            if (selectedItem != null)
            {

                if (selectedItem is DataRowView itemVIew)
                {
                    if (itemVIew.Row.Table.Columns.Contains("item_is_deleted"))
                    {
                        itemVIew["item_is_deleted"] = true;
                    }
                    string selectedId = itemVIew["bpi_item_id"].ToString();

                    var itemRow = dataSource.AsEnumerable().FirstOrDefault(r => r.Field<string>("bpi_item_id") == selectedId);

                    if (itemRow != null)
                    {
                        itemRow.SetField("item_is_deleted", itemVIew["item_is_deleted"]); // Change the Name column

                    }

                    fullItemsRecords = dataSource;

                    DataView dataViewItems = new DataView(dataSource);
                    DataTable filteredItems = dataViewItems.ToTable();
                    dataBindingItems.DataSource = filteredItems;

                }

            }
        }

        private void panel_item_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_branch_website_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_main_tel_no_KeyPress(object sender, KeyPressEventArgs e)
        {

            string input = txt_main_tel_no.Text;
            if (input.Length > 12 && input.Contains("-"))
            {

                txt_main_tel_no.Text = input.Trim().Replace(" ", "").Replace("-", "");
                // Put cursor at the end of text
                txt_main_tel_no.SelectionStart = txt_main_tel_no.Text.Length;
                txt_main_tel_no.SelectionLength = 0;
            }
            // Allow control keys (e.g., Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb != null && tb.TextLength >= 13)
            {
                e.Handled = true;
            }

        }

        private void txt_main_tel_no_Leave(object sender, EventArgs e)
        {
          
        }

        private bool IsValidMobileNumber(string number)
        {

            return number.Length == 11 && (number.StartsWith("09") || number.StartsWith("08"));
        }

        private bool IsValidLandlineNumber(string number)
        {
            return number.Length == 10 &&
                   (number.StartsWith("02") || number.StartsWith("03") || number.StartsWith("04"));
        }

        private string FormatMobileNumber(string number)
        {
            // 09XX-XXX-XXXX
            return string.Format("{0}-{1}-{2}",
                number.Substring(0, 4), number.Substring(4, 3), number.Substring(7, 4));
        }

        private string FormatLandlineNumber(string number)
        {
            // (XXX) XXX-XXXX
            return string.Format("({0}) {1}-{2}",
                number.Substring(0, 2), number.Substring(2, 3), number.Substring(5, 5));
        }
        private void txt_tin_Leave(object sender, EventArgs e)
        {

        }

        private void txt_tin_KeyPress(object sender, KeyPressEventArgs e)
        {

            string input = txt_tin.Text;
            //if(input.Length > 9 && input.Contains("-"))
            //{

            //}
            // Allow control keys (e.g., Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits,hypen and parenthesis
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb != null && tb.TextLength >= 17)
            {
                e.Handled = true;
            }

        }

        private void txt_branch_tel_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            string input = txt_branch_tel_no.Text;
            if (input.Length > 12 && input.Contains("-"))
            {

                txt_branch_tel_no.Text = input.Trim().Replace(" ", "").Replace("-", "");
                // Put cursor at the end of text
                txt_branch_tel_no.SelectionStart = txt_branch_tel_no.Text.Length;
                txt_branch_tel_no.SelectionLength = 0;
            }

            // Allow control keys (e.g., Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb != null && tb.TextLength >= 13)
            {
                e.Handled = true;
            }
        }

        private void txt_main_tel_no_Enter(object sender, EventArgs e)
        {

        }

        private void txt_main_tel_no_TextChanged(object sender, EventArgs e)
        {
            txt_main_tel_no.ForeColor = Color.Black;

        }

        private void txt_main_tel_no_Validating(object sender, CancelEventArgs e)
        {
            string rawinput = txt_main_tel_no.Text.Trim().Replace(" ", "").Replace("-", "");
            string input = txt_main_tel_no.Text;
            txt_main_tel_no.ForeColor = Color.Black; // Reset to default

            if (IsValidMobileNumber(rawinput))
            {
                txt_main_tel_no.Text = FormatMobileNumber(rawinput);
                CopyToMainBranchField("main_tel_no", txt_main_tel_no.Text);

            }
            else if (IsValidLandlineNumber(rawinput))
            {
                txt_main_tel_no.Text = FormatLandlineNumber(rawinput);
                CopyToMainBranchField("main_tel_no", txt_main_tel_no.Text);

            }
            else
            {
                txt_main_tel_no.ForeColor = Color.Firebrick;
                txt_main_tel_no.Text = Regex.Replace(input, @"[\s\-\(\)]", "");
            }

            txt_main_tel_no.SelectionStart = txt_main_tel_no.Text.Length;    
        }





        private void txt_branch_tel_no_Validating(object sender, CancelEventArgs e)
        {
             string branch_tel_no = Regex.Replace(txt_branch_tel_no.Text, @"[\s\-\(\)]", "");
            string raw_branch_tel_no = txt_branch_tel_no.Text;

            txt_branch_tel_no.ForeColor = Color.Black;


            if (IsValidMobileNumber(branch_tel_no))
            {
                txt_branch_tel_no.Text = FormatMobileNumber(branch_tel_no);
                CopyToMainBranchField("branch_tel_no", txt_branch_tel_no.Text);
            }
            else if (IsValidLandlineNumber(branch_tel_no))
            {
                txt_branch_tel_no.Text = FormatLandlineNumber(branch_tel_no);
                CopyToMainBranchField("branch_tel_no", txt_branch_tel_no.Text);

            }
            else
            {
                txt_branch_tel_no.ForeColor = Color.Firebrick;
                txt_branch_tel_no.Text = Regex.Replace(txt_branch_tel_no.Text, @"[\s\-\(\)]", "");
            }

            txt_branch_tel_no.SelectionStart = txt_branch_tel_no.Text.Length;
        }

        private void dg_contacts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                // Always remove previous handlers to prevent duplicate or lingering ones
                tb.KeyPress -= NumericOnly_KeyPress;
                tb.TextChanged -= ContactNumberTextChanged;

                if (dg_contacts.CurrentCell.ColumnIndex == 1) // Target column
                {
                    tb.KeyPress += NumericOnly_KeyPress;
                    tb.TextChanged += ContactNumberTextChanged;
                }
            }
        }
        private void ContactNumberTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                string input = tb.Text.Trim();

                if (IsValidMobileNumber(input) || IsValidLandlineNumber(input))
                {
                    tb.ForeColor = Color.Black;
                }
                else
                {
                    tb.ForeColor = Color.Red;
                }
            }
        }
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {



            TextBox tb = sender as TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (tb != null && tb.TextLength >= 11)
            {
                e.Handled = true;
            }
        }

        private void dg_contacts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_contacts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            string GetTrimmedValue(int colIndex) =>
                row.Cells[colIndex].EditedFormattedValue?.ToString()?.Trim() ?? "";

            string contactRaw = GetTrimmedValue(1);
            string nameRaw = GetTrimmedValue(2);
            string positionRawName = GetTrimmedValue(6);
            string col4 = GetTrimmedValue(4);
            string col5 = GetTrimmedValue(5);

            string contactUnformatted = Regex.Replace(contactRaw, @"[\s\-\(\)]", "");
            bool isContactEmpty = string.IsNullOrEmpty(contactUnformatted);
            bool isNameEmpty = string.IsNullOrEmpty(nameRaw);
            bool isPositioNameEmpty = string.IsNullOrEmpty(positionRawName);

            // ✅ Rule 1: Contact or Name must be filled
            if (isContactEmpty && isNameEmpty &&  isPositioNameEmpty && (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 6))
            {
                row.Cells[e.ColumnIndex].ErrorText = "Either Contact Number , Email and Name is required.";
                return;
            }

            // ✅ Rule 2: Columns 3, 4, 5 must each be filled
            if ((e.ColumnIndex == 4 && string.IsNullOrEmpty(col4) || (e.ColumnIndex == 5 && string.IsNullOrEmpty(col5))))

            {
                row.Cells[e.ColumnIndex].ErrorText = "This field is required.";
                return;
            }

            // ✅ Clear error if validation passed
            row.Cells[e.ColumnIndex].ErrorText = "";
        }

        private void dg_contacts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var cell = dg_contacts.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string currentValue = cell.Value?.ToString();

                if (!string.IsNullOrEmpty(currentValue))
                {
                    // Remove formatting (e.g., dashes, spaces, parentheses)
                    string unformatted = Regex.Replace(currentValue, @"[\s\-\(\)]", "");
                    cell.Value = unformatted;
                }
            }
        }

        private bool isUpdatingText = false;
        private bool isUpdatingTin = false;

        private void txt_main_tel_no_TextChanged_1(object sender, EventArgs e)
        {

            if (isUpdatingText) return;

            isUpdatingText = true;

            //    string input = txt_main_tel_no.Text.Trim().Replace(" ","");
            string originalText = txt_main_tel_no.Text;
            string cleanedInput = Regex.Replace(originalText, @"[\s\-\(\)]", ""); // remove formatting


            txt_main_tel_no.ForeColor = Color.Black; // Reset to default

            if (IsValidMobileNumber(cleanedInput))
            {
                txt_main_tel_no.Text = FormatMobileNumber(cleanedInput);
            }
            else if (IsValidLandlineNumber(cleanedInput))
            {
                txt_main_tel_no.Text = FormatLandlineNumber(cleanedInput);
            }
            else
            {
                //  e.Cancel = true;
                //    txt_main_tel_no.Text = Regex.Replace(txt_main_tel_no.Text, @"[\s\-\(\)]", "");

                txt_main_tel_no.Text = originalText; // keep what user typed
                txt_main_tel_no.SelectionStart = originalText.Length;
                txt_main_tel_no.ForeColor = Color.Firebrick;

            }
            isUpdatingText = false;
        }
        private void txt_tin_Validating(object sender, CancelEventArgs e)
        {
            string input = txt_tin.Text.Trim();
            string digitsOnly = Regex.Replace(input, @"[^\d]", ""); // remove everything except digits
            txt_tin.ForeColor = Color.Black;

            if (digitsOnly.Length == 9)
            {
                // Format: XXX-XXX-XXX-00000
                string formatted = $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 3)}-00000";
                txt_tin.Text = formatted;
            }
            else if (digitsOnly.Length == 14)
            {
                // Format: XXX-XXX-XXX-XXXXX
                string formatted = $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 3)}-{digitsOnly.Substring(9, 5)}";
                txt_tin.Text = formatted;
            }
            else
            {
                // Invalid: not 9 or 14 digits
                txt_tin.ForeColor = Color.Firebrick;
                txt_tin.Text = digitsOnly;
                // Optional: Prevent leaving field
                // e.Cancel = true;
            }
        }

        private void txt_tin_TextChanged(object sender, EventArgs e)
        {
            if (isUpdatingTin) return;

            isUpdatingTin = true;

            string rawInput = txt_tin.Text;
            int selectionStart = txt_tin.SelectionStart;

            string digitsOnly = Regex.Replace(rawInput, @"[^\d]", "");

            string formatted = rawInput; // fallback to original input
            bool isValid = true;

            if (digitsOnly.Length == 9)
            {
                formatted = $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 3)}";
                txt_tin.ForeColor = Color.Black;
            }
            else if (digitsOnly.Length == 14)
            {
                formatted = $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 3)}-{digitsOnly.Substring(9, 5)}";
                txt_tin.ForeColor = Color.Black;
            }
            else
            {
                isValid = false;
                txt_tin.ForeColor = Color.Firebrick;
            }

            // Only update the text if formatting happened
            if (isValid)
                txt_tin.Text = formatted;
            else
                txt_tin.Text = rawInput;

            txt_tin.SelectionStart = Math.Min(selectionStart, txt_tin.Text.Length);

            isUpdatingTin = false;
        }

        private void txt_tin_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            string input = txt_tin.Text;
            //if(input.Length > 9 && input.Contains("-"))
            //{

            //}
            // Allow control keys (e.g., Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits,hypen and parenthesis
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb != null && tb.TextLength >= 17)
            {
                e.Handled = true;
            }
        }

        private void txt_branch_tel_no_TextChanged(object sender, EventArgs e)
        {


            if (isUpdatingText) return;

            isUpdatingText = true;

            //    string input = txt_main_tel_no.Text.Trim().Replace(" ","");
            string originalText = txt_branch_tel_no.Text;
            string cleanedInput = Regex.Replace(originalText, @"[\s\-\(\)]", ""); // remove formatting


            txt_branch_tel_no.ForeColor = Color.Black; // Reset to default

            if (IsValidMobileNumber(cleanedInput))
            {
                txt_branch_tel_no.Text = FormatMobileNumber(cleanedInput);
            }
            else if (IsValidLandlineNumber(cleanedInput))
            {
                txt_branch_tel_no.Text = FormatLandlineNumber(cleanedInput);
            }
            else
            {
          
                txt_branch_tel_no.Text = originalText; // keep what user typed
                txt_branch_tel_no.SelectionStart = originalText.Length;
                txt_branch_tel_no.ForeColor = Color.Firebrick;

            }
            isUpdatingText = false;

        }

        private void txt_main_website_Validating(object sender, CancelEventArgs e)
        {
            string main_website = txt_main_website.Text.Trim();
            CopyToMainBranchField("main_website", main_website);

        }

        private void txt_branch_website_Validating(object sender, CancelEventArgs e)
        {
            string branch_website = txt_branch_website.Text.Trim();
            CopyToMainBranchField("branch_website", branch_website);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {



            general.Rows.Add(0, 0, 0);
            DataRow lastRow = general.Rows[general.Rows.Count - 1]; // Get last row
            Button dynamicButton = new Button();
            groupCount++;
            // Set properties
            dynamicButton.Text = "Group" + groupCount;
            dynamicButton.Size = new Size(100, 50);
            dynamicButton.Location = new Point(50, 50);
            dynamicButton.BackColor = Color.LightBlue;
            dynamicButton.Tag = lastRow;

            // Add the button to the form
            dynamicButton.Click += DynamicButton_Clicks;
            flowLayout_panel.Controls.Add(dynamicButton);

            Panel[] pnlGeneralPanel = { panel_general };

            Helpers.BindControls(pnlGeneralPanel, general, general.Rows.Count - 1);
            if (!string.IsNullOrEmpty(txt_id.Text))
            {
                txt_entity_type.Tag = "MULTI";
                txt_industries.Tag = "MULTI";
                txt_branch_industry.Tag = "MULTI";
                txt_branch_name.Visible = true;
                lbl_branch_name.Visible = true;
            }
            button5.Enabled = false;
            //btn_update.Visible = false;
            ResetData(false);

            Helpers.ResetControls(panel_general);
            Helpers.ResetControls(panel_item);
            Helpers.ResetControls(panel_finance);
            EnableDisabledChildPanel(true);
            panel_header_records.Enabled = false;


        }

        private void dg_contacts_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void btn_finance_payment_terms_Click(object sender, EventArgs e)
        {

            bool showSelectColumn = true;

            SetupModal finance_modal = new SetupModal("Payment Terms Setup", ENUM_ENDPOINT.PAYMENT_TERMS, CacheData.PaymentTerms, showSelectColumn);
            DialogResult r = finance_modal.ShowDialog();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            DataTable currentTable = dataBindingItems.DataSource as DataTable;
            if (currentTable != null)
            {
                DataRow newRow = currentTable.NewRow();
                // Initialize columns with default/empty values as needed
                newRow["item_id"] = DBNull.Value;
                newRow["item_code"] = "";
                newRow["short_desc"] = "";
                newRow["status_tangible"] = "";
                newRow["status_trade"] = "";
                newRow["price"] = 0;


                currentTable.Rows.Add(newRow);

              
              
            }
        }
    }
}
