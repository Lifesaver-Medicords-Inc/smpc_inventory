using Inventory_SMPC.Pages.Item;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Business_Partner_Info.Bpi_Modal;
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
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Business_Partner_Info
{
    public partial class BusinessPartnerInfo : UserControl
    {
        DataTable bpi;
        DataTable general;
        DataTable contacts;
        DataTable address;
        DataTable items;
        DataTable finance;
        DataTable finance_pending;
        DataTable accreditations;
        List<BpiEntityRecords> entityCount;
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
        List<string> selectedPreferenceNames = new List<string>();
        public BusinessPartnerInfo()
        {
            InitializeComponent();
            tabItemPages = tabControl2.TabPages["ITEMS"];
            tabFinancePages = tabControl2.TabPages["FINANCE"];
        }
        public void HideButton()
        {
            btn_add_new_item.Visible = false;
        }

        private async void BusinessPartnerInfo_Load(object sender, EventArgs e)
        {
            GetIndustriesSetup();

            GetSocialMediaSetup();
            GetEntity();
            GetBranchIndustries();
            GetPayments();
            GetEntityCount();
            GetPositionSetup();

            FetchBpi();
            BtnToogle(false);
        }

        private async void FetchBpi()
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
            if (records.bpi.Count != 0 && records.general.Count != 0 && records.contacts.Count != 0 && records.address.Count != 0)
            {
                Bind(true);
            }
            else
            {
                button5.Enabled = false;
                RemoveTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
              //  general.Rows.Add(0,0,0);
              //  DataRow lastRow = general.Rows[general.Rows.Count - 1]; // Get last row
                Button dynamicButton = new Button();


                // Set properties
                dynamicButton.Text = "MAIN";
                dynamicButton.Size = new Size(100, 50);
                dynamicButton.Location = new Point(50, 50); // X=50, Y=50
                dynamicButton.BackColor = Color.LightBlue;
              //  dynamicButton.Tag = lastRow;

              //  dynamicButton.Click += DynamicButton_Clicks;
              //  flowLayout_panel.Controls.Add(dynamicButton);
            }

        }


        private void Bind(bool isBind = false)
        {
            

            if (isBind)
            {
               
                BindDataToPanel();
                FetchAllChilds();

                // For List of Industry, General Entity and General Branch Names
               
                txt_industries.Text = string.IsNullOrEmpty(records.bpi[this.selectedRecord].industry_names) ? "" : (records.bpi[this.selectedRecord].industry_names);
                string industryIds = string.IsNullOrEmpty(records.bpi[this.selectedRecord].industry_ids) ? "" : (records.bpi[this.selectedRecord].industry_ids);

                BindMultiSelectField(records.general); // Bind Id and Names in Modal

                // Show Item pages
                bool isItemShow = ToogleItemPages(txt_entity_type.Text);
                GetPaymentItemTerms(isItemShow);
                ShowTypeOfEntity(txt_entity_type.Text);

         

                var listOfNames = records.bpi.Select(item => item.name).ToList();
                cmb_name.DataSource = listOfNames;
                var matchedRecord = records.finance.FirstOrDefault(f => f.finance_based_id == int.Parse(bpi.Rows[this.selectedRecord]["id"].ToString()));
                 
                if (matchedRecord != null)
                {
                    cmb_finance_payment_terms.SelectedValue = matchedRecord.finance_payment_terms_id;
                    cmb_finance_payment_terms.SelectedItem = matchedRecord;
                    cmb_finance_account.SelectedValue = matchedRecord.finance_account_id;
                    cmb_finance_account.SelectedItem = matchedRecord;
                }

                //cmb_finance_payment_terms.SelectedValue = records.finance[this.selectedRecord].finance_payment_terms_id;
                //cmb_finance_account.SelectedValue = records.finance[this.selectedRecord].finance_account_id;
                //cmb_finance_account.SelectedItem = records.finance[this.selectedRecord].finance_account_id;
                // selectedPreferenceNames = records.contacts.Select(item => item.preferences).ToList();

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
                FetchAllBranch(filteredGeneral);

             
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


        private void DynamicButton_Clicks(object sender, EventArgs e)
        {
            
            MessageBox.Show("TEST DATA");
            Button clickedButton = sender as Button;

            // Retrieve the associated data from the Tag property (could be the row or just the branch name)
            if (clickedButton != null && clickedButton.Tag != null)
            {
                DataRow row = clickedButton.Tag as DataRow;
              
                if (row != null )
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

                    Helpers.BindControls(pnlGeneralPanel, filteredGeneral);
                 
                    BindBranchToChildTab(branchId);
                

                }
            }
        }


        private void FetchAllBranch1(DataTable data)
        {
            var existingButtons = flowLayout_panel.Controls.OfType<Button>()
                                .Select(btn => btn.Text)
                                .ToList();

            foreach (Control ctrl in flowLayout_panel.Controls.OfType<Button>().ToList())
            {

                if (!data.AsEnumerable().Any(row => row["branch_name"].ToString() == ctrl.Text))
                {
                    flowLayout_panel.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }


            }

            foreach (DataRow row in data.Rows)
            {
                string branchName = row["branch_name"].ToString();

                if (existingButtons.Contains(branchName)) // Only add if not already present
                {
                    Button dynamicButton = new Button
                    {
                        Text = branchName,
                        Size = new Size(100, 50),
                        BackColor = Color.LightBlue,
                        Tag = row
                    };

                    dynamicButton.Click += DynamicButton_Clicks; // Attach the click event
                    flowLayout_panel.Controls.Add(dynamicButton);
                }
            }
        }



        private void FetchAllBranch(DataTable data)
        {

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


               
                    Button dynamicButton = new Button
                    {
                        Text = branchName,
                        Size = new Size(100, 50),
                        BackColor = Color.LightBlue,
                        Tag = row
                    };

                    dynamicButton.Click += DynamicButton_Clicks; // Attach the click event
                    flowLayout_panel.Controls.Add(dynamicButton);
            }
           
        }

        private void BindBranchToChildTab(int branchId )
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


        private void BindBranchToChildTabs(int branchId , DataTable dt , string filterName)
        {

            DataView dataViewRecords = new DataView(dt);
            if (dataViewRecords.Count != 0)
            {
                dataViewRecords.RowFilter = $"{filterName} = '{branchId}'";

                DataTable filteredContacts = dataViewRecords.ToTable();
                dataBindingContacts.DataSource = filteredContacts;

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
            Helpers.BindControls(pnlItems, items, this.selectedRecord);
            Helpers.BindControls(pnlFinance, finance, this.selectedRecord);

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
            panel_general.Enabled = isEdit;
            panel_finance.Enabled = isEdit;
            dg_contacts.Enabled = isEdit;
            dg_address.Enabled = isEdit;
            dg_items.Enabled = isEdit;
            dg_accreditations.Enabled = isEdit;


            
        }
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

        private void FetchAllChilds()
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
                dataBindingContacts.DataSource = filteredContacts;
            }

            //Fetch Bpi Address
            DataView dataViewAddress = new DataView(address);
            if (dataViewAddress.Count != 0)
            {
                dataViewAddress.RowFilter = "address_based_id = '" + bpi.Rows[this.selectedRecord]["id"].ToString() + "'";
               
                DataRowView filteredRow = dataViewAddress[0];
                string filteredBasedId = filteredRow["address_branch_id"].ToString();
                string addressBasedId = filteredRow["address_based_id"].ToString();

                dataViewAddress.RowFilter = $"address_branch_id = '{filteredBasedId}' AND address_based_id = '{addressBasedId}'";
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
                    dataViewItems.RowFilter = $"bpi_item_branch_id = '{filteredBasedId}' AND bpi_item_based_id = '{itemBasedId}'";

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
            if (ids =="0")
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
          
            else if (text.Contains("NON AFFILIATED"))
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


        private void  ShowTypeOfEntity(string txt)
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
        private void ShowAffiliatedAndNon(bool isShow)
        {
            lbl_affiliated.Visible = isShow;
            txt_affiliated.Visible = isShow;
            lbl_non_affiliated.Visible = isShow;
            txt_non_affiliated.Visible = isShow;

        }
        private void RemoveAllId(Dictionary<string, dynamic> bpi, Dictionary<string, dynamic> general, Dictionary<string, dynamic> finance)
        {

            bpi.Remove("industries");
            general.Remove("general_id");
            general.Remove("general_based_id");
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

            foreach (DataRow row in dataSource.Rows)
            {

                if (isUpdate)
                {

                    if (row.IsNull("contacts_id") || string.IsNullOrWhiteSpace(row["contacts_id"].ToString())  || row.IsNull("contacts_based_id") || string.IsNullOrWhiteSpace(row["contacts_based_id"].ToString()))
                    {
                        contacts_id = 0;
                        contacts_based_id = 0;
                        branch_id = 0;
                    }
                    else
                    {
                        contacts_id = int.Parse(row["contacts_id"].ToString());
                        contacts_based_id = int.Parse(row["contacts_based_id"].ToString());
                        branch_id  = int.Parse(row["contacts_based_id"].ToString());
                    }
                    
                }

                string number = row["number"].ToString();
                string email = row["email"].ToString();
                string name = row["name"].ToString();
                string preferences = row["preferences"].ToString();
                int position = int.Parse(row["position"].ToString());

                contacts = new BpiContacts(contacts_id, contacts_based_id, number, name, email, preferences, position, branch_id);
                listContacts.Add(contacts);

            }


            return listContacts;
        }

        private List<BpiAddress> SaveAddress(bool isUpdate)
        {

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_address);
            List<BpiAddress> listAddress = new List<BpiAddress>();

            BpiAddress address = null;
            int address_id = 0;
            int adrress_based_id = 0;
            int branch_id = 0;
            foreach (DataRow row in dataSource.Rows)
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
                }

                string location = row["location"].ToString();

                address = new BpiAddress(address_id, adrress_based_id, location,branch_id);
                listAddress.Add(address);

            }

            return listAddress;
        }
        private List<BpiItems> SaveItems(bool isUpdate)
        {
          

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_items);
            var items = Helpers.GetControlsValues(panel_item);
            List<BpiItems> listItem = new List<BpiItems>();

            string taxCode = items["tax_code"].ToString();
            string itemTaxCode = items["item_tax_code"].ToString();
            int itemAccountId = int.Parse(items["item_account_id"].ToString());
            int paymentTermsId;
            if (!int.TryParse(items["payment_terms_id"]?.ToString(), out paymentTermsId))
            {
                paymentTermsId = 0;
            }
            int itemId = 0;
            int basedItemId = 0;
            int bpiItemId = 0;
            int bpiItemBranchId = 0;
            BpiItems item = null;
            float unitPrice;
            bool unitPriceValid;
            foreach (DataRow row in dataSource.Rows)
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
                }
                itemId = int.Parse(row["item_id"].ToString());
                string notes  = row["notes"].ToString();

                unitPriceValid = float.TryParse(row["price"].ToString(), out unitPrice);
                item = new BpiItems(bpiItemId, basedItemId,paymentTermsId, itemId, taxCode,itemTaxCode,unitPrice, notes, itemAccountId);

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

                int accreditation_added_by_id;
                if (!int.TryParse(row["accreditation_added_by_id"].ToString(), out accreditation_added_by_id))
                {
                    accreditation_added_by_id = 1;
                }

                //  int accreditation_added_by_id = int.Parse(row["accreditation_added_by_id"].ToString());
                accreditations = new BpiAccreditation(bpi_accreditation_id, branch_id, date_added, file_path,  accreditation_added_by_id, bpi_accreditation_based_id, file_name);
                listAccreditation.Add(accreditations);

            }

            return listAccreditation;


        }
        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private async void GetEntityCount()
        {

            var response = await RequestToApi<ApiResponseModel<List<BpiEntityRecords>>>.Get(ENUM_ENDPOINT.BpiEntity);
             entityCount = response.Data;

        }
        private async void GetIndustriesSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.INDUSTRIES);
            CacheData.Industries = await serviceSetup.GetAsDatatable();

            //var combobox = (DataGridViewComboBoxColumn)dg_contacts.Columns["position"];
            //combobox.DataSource = CacheData.Industries;
            //combobox.DisplayMember = "name";
            //combobox.ValueMember = "id";
        }

        private async void GetPositionSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.POSITION);
            CacheData.Positions = await serviceSetup.GetAsDatatable();

            var combobox = (DataGridViewComboBoxColumn)dg_contacts.Columns["position"];
            combobox.DataSource = CacheData.Positions;
            combobox.DisplayMember = "name";
            combobox.ValueMember = "id";

        }
        private async void GetBranchIndustries()
        {

            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.INDUSTRIES);
            CacheData.BranchIndustries = await serviceSetup.GetAsDatatable();
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
        private  void GetPaymentItemTerms(bool isItem)
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

      
        private async void GetSocialMediaSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.SOCIALS);
            CacheData.SocialMedia = await serviceSetup.GetAsDatatable();
            cmb_social.DataSource = CacheData.SocialMedia;
            cmb_social.ValueMember = "id";
            cmb_social.DisplayMember = "name";
        }
        private async void GetEntity()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ENTITY);
            CacheData.Entity = await serviceSetup.GetAsDatatable();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.bpi.Rows.Count - 1 > this.selectedRecord)
            {
                //RemoveSelectedDataTable(CacheData.BranchIndustries);
                //RemoveSelectedDataTable(CacheData.Entity);
                this.selectedRecord++;
                Bind(true);

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
                Bind(true);

            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btn_update_Click(object sender, EventArgs e)
        {

            if (currentSelectedEntityIds.Count != 0 && currentSelectedIndustryIds.Count != 0  && currentSelectedBranchIndustryIds.Count != 0)
            {
                txt_entity_type.Tag = currentSelectedEntityIds;
                txt_industries.Tag = currentSelectedIndustryIds;
                txt_branch_industry.Tag = currentSelectedBranchIndustryIds;
            }

         

            var Bpi = Helpers.GetControlsValues(panel_header_records);
            var Generals = Helpers.GetControlsValues(panel_general);
            var Contacts = SaveContacts(true);

            var Accreditations = SaveAccreditations(true); ;
            dg_contacts.EndEdit();
            dg_contacts.CommitEdit(DataGridViewDataErrorContexts.Commit);


            var Address = SaveAddress(true);
            var Items = SaveItems(true);

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
                location = c.location
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
                notes = c.notes   
            }).ToList();

            var modifiedAccreditations = Accreditations.Select(c => new
            {
                id = c.bpi_accreditation_id,
                based_id = c.bpi_accreditation_based_id,
                branch_id = c.bpi_accreditation_branch_id,
                date_added = c.date_added,
                file_name = c.file_name,
                file_path = c.file_path,
                accreditation_added_by_id = c.accreditation_added_by_id,        
            }).ToList();




            Bpi["sales_id"] = int.Parse(Bpi["sales_id"]);
            Generals["social_id"] = int.Parse(Generals["social_id"].ToString());

            Generals.Add("id", int.Parse(Generals["general_id"]));
            Generals.Add("based_id", int.Parse(Generals["general_based_id"]));

            Bpi.Remove("industries");
            Generals.Remove("general_id");
            Generals.Remove("general_based_id");

            if (txt_entity_type.Text.Contains("SUPPLIER"))
            {

                Bpi.Add("items", modifiedItems);
            }
            else if (txt_entity_type.Text.Contains("CUSTOMER"))
            {
                var Finance = Helpers.GetControlsValues(panel_finance);
                Finance["finance_id"] = int.Parse(Finance["finance_id"].ToString());
                Finance["finance_based_id"] = int.Parse(Finance["finance_based_id"].ToString());

                Bpi.Add("finance", Finance);

            }
            Bpi.Add("general", Generals);

            Bpi.Add("contacts", modifiedContact);

            Bpi.Add("address", modifiedAddress);
            Bpi.Add("accreditations", modifiedAccreditations);



            var dataBpi = Bpi;
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
        private void ResetSelectedData()
        {


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
         



            Panel[] pnlList = { panel_header_records };
            var Bpi = Helpers.GetControlsValues(panel_header_records);


            //general.Rows.Add(0, 0, 0);
            //DataRow lastRow = // Get last row

            if (Bpi["id"] == "")
            {

                //Button dynamicButton = new Button();

                //dynamicButton.Text = "MAIN";
                //dynamicButton.Size = new Size(100, 50);
                //dynamicButton.Location = new Point(50, 50);
                //dynamicButton.BackColor = Color.LightBlue;

                //dynamicButton.Tag = general.Rows[general.Rows.Count - 1];


                //dynamicButton.Click += DynamicButton_Clicks;

                // flowLayout_panel.Controls.Add(dynamicButton);

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



            modalSelection = new SetupSelectionModal("Industries", ENUM_ENDPOINT.INDUSTRIES, CacheData.Industries, currentSelectedIndustryIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();

            if (modalResult == DialogResult.OK)
            {

                var result = modalSelection.GetResult();

                Helpers.GetModalData(txt_industries, result);
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

                string [] entities = data.Split(',');
                bool hasBlackListed = entities.Any(n => n.Trim() =="BLACKLISTED");
          
                if (hasBlackListed )
                {

                    txt_entity_type.Text = "";
                    txt_entity_type.Tag = null;
                    currentSelectedEntityIds.Clear();
                    MessageBox.Show("Cannot select BLACKLISTED based on your position", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    else if (txt_entity_type.Text.Contains("SUPPLIER"))
                    {
                        DocumentCodeIncrementor("SUPPLIER");
                        ToogleCustomerAndSupplier(true);
                        ShowAffiliatedAndNon(false);
                        ShowTabPages(tabItemPages);
                        RemoveTabPages(tabFinancePages);

                        txt_customer_code.Enabled = false;

                        //  txt_supplier_code.Text = "S#" + number.ToString();

                    }

                    else if (txt_entity_type.Text.Contains("NON-AFFILIATED"))
                    {
                        // txt_non_affiliated.Text = "EN#" + number.ToString();
                        DocumentCodeIncrementor("NON-AFFILIATED");
                        ToogleEntityField(true);
                        ToogleCustomerAndSupplier(false);
                        RemoveTabPages(tabFinancePages);
                        RemoveTabPages(tabItemPages);

                    }
                    else if (txt_entity_type.Text.Contains("AFFILIATED"))
                    {
                        //   txt_affiliated.Text = "EA#" + number.ToString();
                        DocumentCodeIncrementor("AFFILIATED");
                        ToogleEntityField(false);
                        ToogleCustomerAndSupplier(false);
                        RemoveTabPages(tabFinancePages);

                    }
                    else
                    {
                        DocumentCodeIncrementor("CUSTOMER");
                        ShowAffiliatedAndNon(false);
                        ShowTabPages(tabFinancePages);
                        ToogleCustomerAndSupplier(true);
                        RemoveTabPages(tabItemPages);


                        txt_supplier_code.Enabled = false;



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

       

        private void DocumentCodeIncrementor(string entity)
        {

            switch (entity)
            {

                case "SUPPLIER":

                    txt_supplier_code.Text = "S#" + (GetEntityRecordCount("SUPPLIER") + 1);

                    break;

                case "CUSTOMER":
                    txt_customer_code.Text = "C#" + (GetEntityRecordCount("CUSTOMER") + 1);

                    break;

                case "NON-AFFILIATED":

                    txt_non_affiliated.Text = "EN#" + (GetEntityRecordCount("NON-AFFILIATED") + 1);

                    break;
                case "AFFILIATED":

                    txt_affiliated.Text = "EA#" + (GetEntityRecordCount("AFFILIATED") + 1);
                    break;

                case "BOTH": 

                    txt_customer_code.Text = "C#" + (GetEntityRecordCount("CUSTOMER") + 1);
                    txt_supplier_code.Text = "S#" + (GetEntityRecordCount("SUPPLIER") + 1);
          
                    break;

                default:
       

                    break;
            }

        }


        private int GetEntityRecordCount(string code)
        {
            var record = entityCount.FirstOrDefault(records => records.code == code);
            return record != null ? record.entity_count : 0;

        }
        private void General_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_get_branch_Click(object sender, EventArgs e)

        {

            modalSelection = new SetupSelectionModal("Branch Industries", ENUM_ENDPOINT.INDUSTRIES, CacheData.BranchIndustries, currentSelectedBranchIndustryIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();


            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();
                Helpers.GetModalData(txt_branch_industry, result);
                currentSelectedBranchIndustryIds.Clear();

  
            }
           
        }
        private void txt_industries_TextChanged(object sender, EventArgs e)
        {

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



        private async void btn_add_Click(object sender, EventArgs e)
        {

            int temporaryId = 0;      
            var Bpi = Helpers.GetControlsValues(panel_header_records);
            var Generals = Helpers.GetControlsValues(panel_general);

            if (Bpi["id"] is string strId && string.IsNullOrEmpty(strId))
            {
                temporaryId = 0;
                Bpi.Remove("id");
                Bpi.Add("id", temporaryId);
            }
            else if (Bpi["id"] is int intId && intId == 0)
            {
                temporaryId = 0;
                Bpi.Add("id", temporaryId);
            }

          
  
            if (Bpi["id"] == 0)
            {
               Generals["branch_name"] = Bpi["name"];
            }

            var Contacts = SaveContacts(false);
            var Address = SaveAddress(false);
            var Finance = Helpers.GetControlsValues(panel_finance);
            var Accreditations = SaveAccreditations(false); ;

            Bpi["sales_id"] = int.Parse(Bpi["sales_id"]);
            Generals["social_id"] = int.Parse(Generals["social_id"].ToString());
  
              
            if (txt_entity_type.Text.Contains("SUPPLIER"))
            {
                var Items = SaveItems(false);
                Bpi.Add("items", Items);
            }

            else if (txt_entity_type.Text.Contains("CUSTOMER"))
            {       
                Bpi.Add("finance", Finance);
            }



            Bpi.Add("general", Generals);
            Bpi.Add("contacts", Contacts);
            Bpi.Add("address", Address);
            Bpi.Add("accreditations", Accreditations);
            RemoveAllId(Bpi, Generals,Finance);


            var response = await BpiServices.Insert(Bpi);

            var data = response.Data;
            if (response.Success)
            {
                MessageBox.Show("SHOW SENIOR LEM");
                button5.Enabled = true;
                txt_id.Text = data["id"];


                Generals.Add("general_id", data["general"]["id"].ToString());
                Generals.Add("general_based_id", data["general"]["based_id"].ToString());
                Generals["general_id"] = int.Parse(Generals["general_id"].ToString());
                Generals["general_based_id"] = int.Parse(Generals["general_based_id"].ToString());
                Generals.Add("branch_industry_ids", Generals["branch_industry_id"]);
                Generals.Add("entity_ids", Generals["entity_type_id"]);


                

                // Ensure entity_type_id exists and is a list of IDs
                GeneralMultiSelectText("entity_type_id", "entity_names", Generals, CacheData.Entity);
                GeneralMultiSelectText("branch_industry_id", "branch_industry_names", Generals, CacheData.BranchIndustries);  

                DataRow rows =  AddDictionaryToDataTable(general, Generals);
                general.Rows.Add(rows);

                DataView dataViewGeneral = new DataView(general);
                if (dataViewGeneral.Count != 0)
                {
                    dataViewGeneral.RowFilter = "general_based_id = '" + txt_id.Text + "'";

                    txt_branch_name.Visible = dataViewGeneral.Count > 1;
                    lbl_branch_name.Visible = dataViewGeneral.Count > 1;
                }

                DataTable filteredGeneral = dataViewGeneral.ToTable();
                FetchAllBranch(filteredGeneral);



                btn_new.Visible = true;
                btn_search.Visible = true;
                btn_prev.Visible = true;
                btn_next.Visible = true;
                btn_edit.Visible = true;
                EnableDisabledChildPanel(false);


            }
            else
            {
                Helpers.ShowDialogMessage("error",string.IsNullOrEmpty(response.message) ? "Operation Fail" : response.message);
            }

        }

        private void GeneralMultiSelectText(string identity , string field, Dictionary <string,dynamic> dt, DataTable modalSetup)
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
         
        private DataRow AddDictionaryToDataTable(DataTable dt ,Dictionary<string,dynamic> model)
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

            //DataRow row = general.NewRow();
            // foreach (var key in Generals.Keys)
            // {
            //     if (general.Columns.Contains(key)) // Ensure the column exists
            //     {
            //         var value = Generals[key];

            //         // Check if the value is a List<int>
            //         if (value is List<int> list)
            //         {
            //             row[key] = string.Join(",", list); // Convert list to a comma-separated string
            //         }
            //         else
            //         {
            //             row[key] = value; // Assign normally if not a list
            //         }
            //     }
            // }
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



                    modalSelection = new SetupSelectionModal("Preferences", ENUM_ENDPOINT.SOCIALS, CacheData.SocialMedia, new List<int> { }, selectedPreferenceNames, index);
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

                
                    ItemModal modal = new ItemModal();
                    DialogResult r = modal.ShowDialog(); 

                    if( r == DialogResult.OK) {

                        Dictionary<string, dynamic> result = modal.GetResult();

                        this.dg_items.Rows[e.RowIndex].Cells[7].Value = result["item_id"];
                        this.dg_items.Rows[e.RowIndex].Cells[0].Value = result["item_code"];
                        this.dg_items.Rows[e.RowIndex].Cells[1].Value = result["short_desc"];
                        this.dg_items.Rows[e.RowIndex].Cells[5].Value = result["status_tangible"];
                        this.dg_items.Rows[e.RowIndex].Cells[6].Value = result["status_trade"];

                    }

                    
                }
            }

        }

        private void dg_items_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            dg_items.EndEdit();
            dg_items.CommitEdit(DataGridViewDataErrorContexts.Commit);

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

            Helpers.BindControls(pnlGeneralPanel, general, general.Rows.Count -1);
            if (!string.IsNullOrEmpty(txt_id.Text))
            {
                txt_entity_type.Tag = "MULTI";
                txt_industries.Tag = "MULTI";
                txt_branch_industry.Tag = "MULTI";
                txt_branch_name.Visible = true;
                lbl_branch_name.Visible = true;
            }
            button5.Enabled = false;
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
            if (Application.OpenForms.OfType<ItemEntryModal>().Any())
            {
                return; // Prevent opening if already open
            }
            ItemEntryModal modal = new ItemEntryModal();
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ShowDialog();
        }

        private void cmb_payment_terms_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmb_tax_code_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txt_sales_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void dg_contacts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
          

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
          
            DateTime now = DateTime.Now;
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_accreditations);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;
             
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    foreach ( string files in openFileDialog.FileNames)
                    {
                      
                        string fileName = Path.GetFileName(files);

                        dataSource.Rows.Add(now, fileName, "Jerome Obogne", files);
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
            FetchBpi();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
            btn_update.Visible = true;
            btn_add.Visible = false;



        }
    }
    }














