using smpc_inventory_app.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_inventory_app.Services.Setup.Model.Bpi;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using System.Text.RegularExpressions;
using smpc_inventory_app.Services.Setup.Item;
using smpc_sales_app.Pages;
using System.IO;
using smpc_inventory_app.Pages.Business_Partner_Info.Bpi_Modal;
using smpc_inventory_app.Model;
using System.Diagnostics;

namespace smpc_inventory_app.Pages.Business_Partner_Info
{
    public partial class BpiBranchUC : UserControl
    {
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
        Bpi_Class Records;


        SetupModal modalSetup;
        SetupSelectionModal modalSelection;
        GeneralSetupServices serviceSetup;

        List<int> currentSelectedBranchIndustryIds = new List<int>();
        List<int> currentSelectedEntityIds = new List<int>();
        List<int> currentSelectedIndustryIds = new List<int>();
        List<BpiEntityRecords> entityCount;
        List<string> selectedPreferenceNames = new List<string>();
        List<CurrentUserModel> Users;

        string REGEXPATTERN = @"^\d{4}-\d{3}-\d{4}$";

        TabPage tabItemPages;
        TabPage tabFinancePages;

        int SelectedRecord;
        string ParentId;
        string SalesId;
        string TabTitle;
        string CanvassForm;
        bool IsExisting;
        bool IsMain;
        private bool isUpdatingText = false;
        private bool isUpdatingTin = false;

        public bool isUpdate { get; set; }

        public BpiBranchUC(string parentId, string salesId, string tabTitle, string canvassForm, bool isExisting)
        {
            InitializeComponent();
 
            this.ParentId = parentId;
            this.SalesId = salesId;
            this.TabTitle = tabTitle;
            this.CanvassForm = canvassForm;
            this.IsExisting = isExisting;

            if (!string.IsNullOrEmpty(canvassForm))
                ShowCanvassTabPage();

            tabItemPages = tabControl2.TabPages["ITEMS"];
            tabFinancePages = tabControl2.TabPages["FINANCE"];

            CheckPanelsInTabPage(GENERAL, panel_general);



        }
        private async void BpiBranchUC_Load(object sender, EventArgs e)
        {

            txt_branch_name.Text = TabTitle;
            //GetIndustriesSetup();
            GetPositionSetup();
            GetTaxCode();
            await GetPayments();
            await GetSocialMediaSetup();
            //GetEntity();
            //GetBranchIndustries();
            //GetPayments();
            GetEntityCount();
            //BindBpiGeneral(true);
            //GetPositionSetup();
            LoadBpidData();
          

        }
        private async void LoadBpidData()
        {

            var response = await RequestToApi<ApiResponseModel<Bpi_Class>>.Get(ENUM_ENDPOINT.BPI);
            Records = response.Data;

            bpi = JsonHelper.ToDataTable(Records.bpi);
            general = JsonHelper.ToDataTable(Records.general);
            contacts = JsonHelper.ToDataTable(Records.contacts);
            address = JsonHelper.ToDataTable(Records.address);
            items = JsonHelper.ToDataTable(Records.items);
            finance = JsonHelper.ToDataTable(Records.finance);
            finance_pending = JsonHelper.ToDataTable(Records.finance_pending);
            accreditations = JsonHelper.ToDataTable(Records.accreditations);
            history = JsonHelper.ToDataTable(Records.history);

            if (IsExisting)
            {
                if (Records.bpi.Count != 0 && Records.general.Count != 0 && Records.contacts.Count != 0 && Records.address.Count != 0)
                {
                    BindGeneral(true);
                }
                else
                {
                    MessageBox.Show("No records found.");
                }
            }
            // 

            if (IsMain)
            {
                chk_is_main.Checked = true;
            }
           

        }
        private void LoadAllBpiChild()
        {
            //Fetch Bpi Contacts 
            DataView dataViewContact = new DataView(contacts);

            if (dataViewContact.Count != 0)
            {
                dataViewContact.RowFilter = "contacts_based_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";

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
                dataViewAddress.RowFilter = "address_based_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";

                if (dataViewAddress.Count > 0)
                {
                    DataRowView filteredRow = dataViewAddress[0];
                    string filteredBasedId = filteredRow["address_branch_id"].ToString();
                    string addressBasedId = filteredRow["address_based_id"].ToString();
                    dataViewAddress.RowFilter = $"address_branch_id = {filteredBasedId} AND address_based_id = {addressBasedId} AND address_is_deleted = {false}";
                }

                DataTable filteredAddress = dataViewAddress.ToTable();
                dataBindingAddress.DataSource = filteredAddress;
            }

            //Fetch Bpi Items
            DataView dataViewItems = new DataView(items);
            if (dataViewItems.Count != 0)
            {
                dataViewItems.RowFilter = "bpi_item_based_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";
                
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
                dataViewFinancePending.RowFilter = "customer_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";

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

            //Fetch Bpi Accreditation
            DataView dataViewAccreditation = new DataView(accreditations);
            if (dataViewAccreditation.Count != 0)
            {
                dataViewAccreditation.RowFilter = "bpi_accreditation_based_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";

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
                dataViewHistory.RowFilter = "branch_id = '" + bpi.Rows[this.SelectedRecord]["id"].ToString() + "'";

                DataTable filteredHistory = dataViewHistory.ToTable();
                dataBindingHistory.DataSource = filteredHistory;
            }
        }
        private void BindGeneral(bool isBind = false)
        {
            if (isBind)
            {
                var isSelectedSales = GetSelectedSales();

                BpiBranchToggle(isSelectedSales);
                LoadAllBpiChild();
                BindDataToPanel();
                BindDataToTable();
                BindDataToComboBox();
                BindMultiSelectField(Records.general);
                //BindDataToComboBox();

                //MessageBox.Show("Entity Type: " + txt_entity_type.Text);
                bool isItemShow = ToogleItemPages(txt_entity_type.Text);
                GetPaymentItemTerms(isItemShow);
                ShowTypeOfEntity(txt_entity_type.Text);
            }
        }
        private void BindDataToPanel()
        {
            //  INIT DATAVIEW
            DataView dataViewGeneral = new DataView(general);
            DataView dataViewFinance = new DataView(finance);
            DataView dataViewItems = new DataView(items);

            // INIT PANEL LIST
            Panel[] pnlGeneralPanel = { panel_general };
            Panel[] pnlFinancePanel = { panel_finance };
            Panel[] pnlItemPanel = { panel_item };

            // FILTER DATA FOR USING GENERAL ID
            if (dataViewGeneral.Count != 0)
            {
                dataViewGeneral.RowFilter = $"general_id = '{ParentId}'";
            }
            if (dataViewFinance.Count != 0)
            {
                dataViewFinance.RowFilter = $"finance_branch_id = '{ParentId}'";
            }
            if (dataViewItems.Count != 0)
            {
                dataViewItems.RowFilter = $"bpi_item_branch_id = '{ParentId}'";
            }

            // CREATE LIST OF PANEL
            DataTable filteredGeneral = dataViewGeneral.ToTable();
            DataTable filteredFinance = dataViewFinance.ToTable();
            DataTable filteredItems = dataViewItems.ToTable();

            //// BIND DATA TO PANEL
            if (filteredGeneral != null && filteredGeneral.Rows.Count > 0)
            {
                Helpers.BindControls(pnlGeneralPanel, filteredGeneral);
            }
               
            if (filteredFinance != null && filteredFinance.Rows.Count > 0)
            {
                Helpers.BindControls(pnlFinancePanel, filteredFinance);
            }
            if (filteredItems != null && filteredItems.Rows.Count > 0)
            {
                Helpers.BindControls(pnlItemPanel, filteredItems);
            }
        }
        private void BindDataToTable()
        {
            List<DataGridView> DgvList = new List<DataGridView>()
            {
                dg_contacts,
                dg_address,
                dg_finance_pending,
                dg_items,
                dg_accreditations,
                dg_history
            };

            DisbleAutoColumnGeneration(DgvList);
            string parentId = ParentId;

            // ---- Contacts ----
            var contactView = new DataView(contacts)
            {
                RowFilter = $"branch_id = '{parentId}'"
            };

            if (contactView.Count > 0)
            {
                var filteredRow = contactView[0];
                string filteredBranchId = filteredRow["branch_id"].ToString();
                contactView.RowFilter = $"branch_id = '{filteredBranchId}'";
            }
            DataTable filteredContacts = contactView.ToTable();
            // Ensure default position
            foreach (DataRow contactRow in filteredContacts.Rows)
            {
                int positionValue = Convert.ToInt32(contactRow["position"]);
            }
            Debug.WriteLine($"Before binding: {dg_contacts.AutoGenerateColumns}");
            
            dataBindingContacts.DataSource = filteredContacts;
            Debug.WriteLine($"After binding: {dg_contacts.AutoGenerateColumns}");

            foreach (DataGridViewColumn col in dg_contacts.Columns)
            {
                Debug.WriteLine($"{col.Index} | Name={col.Name} | DataProperty={col.DataPropertyName} | Visible={col.Visible}");
            }

            // ---- Address ----
            var addressView = new DataView(address)
            {
                RowFilter = $"address_branch_id = '{parentId}'"
            };

            if (addressView.Count > 0)
            {
                var filteredRow = addressView[0];
                string filteredBranchId = filteredRow["address_branch_id"].ToString();
                string addressBasedId = filteredRow["address_based_id"].ToString();
                // Check if based and branchid and isnotdeleted
                addressView.RowFilter = $"address_branch_id = {filteredBranchId} AND address_based_id = {addressBasedId} AND address_is_deleted = False";
            }
            dataBindingAddress.DataSource = addressView.ToTable();

            // ---- Items ----
            var itemsView = new DataView(items)
            {
                RowFilter = $"bpi_item_branch_id = '{parentId}'"
            };

            if (itemsView.Count > 0)
            {
                var filteredRow = itemsView[0];
                string filteredBranchId = filteredRow["bpi_item_branch_id"].ToString();
                string itemBasedId = filteredRow["bpi_item_based_id"].ToString();
                itemsView.RowFilter = $"bpi_item_branch_id = {filteredBranchId} AND bpi_item_based_id = {itemBasedId} AND item_is_deleted = False";
            }
            dataBindingItems.DataSource = itemsView.ToTable();

            // ---- Finance Pending ----
            var financePendingView = new DataView(finance_pending)
            {
                RowFilter = $"customer_id = '{parentId}'"
            };

            if (financePendingView.Count > 0)
            {
                var filteredRow = financePendingView[0];
                string filteredBranchId = filteredRow["finance_pending_branch_id"].ToString();
                string financeCustomerId = filteredRow["customer_id"].ToString();
                financePendingView.RowFilter = $"finance_pending_branch_id = '{filteredBranchId}' AND customer_id = '{financeCustomerId}'";
            }
            dataBindingFinancePending.DataSource = financePendingView.ToTable();

            // ---- Accreditations ----
            var accreditationView = new DataView(accreditations)
            {
                RowFilter = $"bpi_accreditation_branch_id = '{parentId}'"
            };

            if (accreditationView.Count > 0)
            {
                var filteredRow = accreditationView[0];
                string filteredBranchId = filteredRow["bpi_accreditation_branch_id"].ToString();
                string bpiAccreditationBasedId = filteredRow["bpi_accreditation_branch_id"].ToString();
                accreditationView.RowFilter = $"bpi_accreditation_branch_id = '{filteredBranchId}' AND bpi_accreditation_branch_id = '{bpiAccreditationBasedId}'";
            }
            databindingAccreditation.DataSource = accreditationView.ToTable();

            // ---- History ----
            var historyView = new DataView(history)
            {
                RowFilter = $"branch_id = '{parentId}'"
            };
            
            dataBindingHistory.DataSource = historyView.ToTable();
        }
        private void BindDataToComboBox()
        {
            if (string.IsNullOrEmpty(ParentId))
                return;

            int parentId = int.Parse(ParentId);


            // GENERAL TAB
            SetComboBoxValue(general, "general_id", parentId, cmb_social, "social_id");

            // ITEMS TAB
            SetComboBoxValue(items, "bpi_item_branch_id", parentId, cmb_payment_terms, "payment_terms_id");
            SetComboBoxValue(items, "bpi_item_branch_id", parentId, cmb_item_account, "item_account_id");
            //SetComboBoxValue(items, "bpi_item_branch_id", parentId, cmb_tax_code, "item_account_id");

            //// FINANCE TAB
            SetComboBoxValue(finance, "finance_branch_id", parentId, cmb_finance_account, "finance_account_id");
            SetComboBoxValue(finance, "finance_branch_id", parentId, cmb_finance_payment_terms, "finance_payment_terms_id");

        }
        private void BindMultiSelectField(List<BpiGeneral> general)
        {

            var matchSelectedEntity = general.FirstOrDefault(f => f.general_id == int.Parse(ParentId));

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
        private void SetComboBoxValue(DataTable table, string filterColumn, int filterValue, ComboBox combo, string valueColumn)
        {
            var row = table.AsEnumerable()
                .FirstOrDefault(r => Convert.ToInt32(r[filterColumn]) == filterValue);


            if (row != null && !row.IsNull(valueColumn))
            {
                int value = row.Field<int>(valueColumn);
                bool exists = false;

                foreach (DataRowView drv in combo.Items)
                {
                    if (Convert.ToInt32(drv[combo.ValueMember]) == value)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    combo.SelectedValue = value;
                }
                else
                {
                    combo.SelectedIndex = 0; 
                }
            }
            else
            {
                combo.SelectedIndex = 0;
            }

        }
        private bool GetSelectedSales()
        {
            string currentUser = CacheData.CurrentUser.employee_id;
            //MessageBox.Show($"current: {currentUser}, owner: {SalesId}");
            // BUG -- SETS OWNER AS THE FIRST RECORD SALES ID
            bool isSalesOwner = currentUser == SalesId;
            return isSalesOwner;
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


            AddOwnedByOtherSalesLabel();

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

            //foreach (var btn in flowLayout_panel.Controls.OfType<Button>().ToList())
            //{
            //    flowLayout_panel.Controls.Remove(btn);
            //    btn.Dispose();
            //}

            foreach (DataRow row in data.Rows)
            {
                string branchName = row["branch_name"].ToString();
                var salesName = bpi.Rows[this.SelectedRecord]["sales_id"].ToString();
                //    var matchSelectedSaless = Users.FirstOrDefault(salesUser => salesUser.employee_id == bpi.Rows[this.selectedRecord]["sales_id"].ToString());

                var salesOwner = CacheData.CurrentUser.employee_id == row["branch_sales_id"].ToString();
                var matchSelectedSales = Users.FirstOrDefault(salesUser => salesUser.employee_id == row["branch_sales_id"].ToString());
                string selectedSalesNames = "";
                if (matchSelectedSales != null)
                {
                    selectedSalesNames = $"({matchSelectedSales.first_name.Substring(0, 1).ToUpper()}. {matchSelectedSales.last_name})";
                    //selectedSalesNames = txt_sales_id.Text;
                }
                string selectedSalesName = salesOwner ? "PURCH-PO-8" : selectedSalesNames;

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


                //if (salesOwner)
                //{
                //    dynamicButton.Click += DynamicButton_Clicks; // Attach the click event
                //}
                //flowLayout_panel.Controls.Add(dynamicButton);

            }

        }


        public void SetBranchName(string name)
        {
            txt_branch_name.Text = name;
        }
        public void SetMainBranch(bool isMain)
        {
            IsMain = isMain;
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

        private void btn_get_branch_Click(object sender, EventArgs e)
        {
            DataTable branchData;

            if (string.IsNullOrEmpty(ParentId))
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
        private void CopyToMainBranchField(string fieldName, string value)
        {
            //string mainBpi_ID = ParentId;
            //if (string.IsNullOrEmpty(mainBpi_ID))
            //{
            //    switch (fieldName.ToLower())
            //    {
            //        case "main_website":
            //            txt_branch_website.Text = value;
            //            break;
            //        case "main_tel_no":
            //            txt_branch_tel_no.Text = value;
            //            break;
            //        case "industries":
            //            txt_branch_industry.Text = value;
            //            txt_branch_industry.Tag = txt_industries.Tag;



            //            var values = txt_industries.Tag as List<int>;

            //            foreach (int newValue in values)
            //            {
            //                copyBranchIds.Add(newValue);
            //            }
            //            txt_industries.Tag = copyBranchIds;
            //            currentSelectedBranchIndustryIds = txt_industries.Tag as List<int>;

            //            var selectedIndustriesID = CopySelectedIndustries(txt_industries);
            //            currentSelectedBranchIndustryIds = selectedIndustriesID;

            //            break;
            //        case "branch_industries":
            //            txt_industries.Text = value;

            //            txt_industries.Tag = txt_branch_industry.Tag;

            //            var values3 = txt_industries.Tag as List<int>;


            //            foreach (int newValue in values3)
            //            {
            //                copyBranchIds.Add(newValue);
            //            }
            //            txt_branch_industry.Tag = copyBranchIds;

            //            currentSelectedIndustryIds = txt_branch_industry.Tag as List<int>;

            //            var selectedIndustries = CopySelectedIndustries(txt_branch_industry);
            //            currentSelectedIndustryIds = selectedIndustries;

            //            break;
            //        case "branch_tel_no":
            //            txt_main_tel_no.Text = value;
            //            break;
            //        case "branch_website":
            //            txt_main_website.Text = value;
            //            break;
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
        private void txt_branch_website_Validating(object sender, CancelEventArgs e)
        {
            string branch_website = txt_branch_website.Text.Trim();
            CopyToMainBranchField("branch_website", branch_website);
        }

        private void btn_finance_payment_terms_Click(object sender, EventArgs e)
        {
            bool showSelectColumn = true;

            SetupModal finance_modal = new SetupModal("Payment Terms Setup", ENUM_ENDPOINT.PAYMENT_TERMS, CacheData.PaymentTerms, showSelectColumn);
            DialogResult r = finance_modal.ShowDialog();
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
       
        private void btn_upload_image_Click(object sender, EventArgs e)
        {
            string fname = CacheData.CurrentUser.first_name;
            string lname = CacheData.CurrentUser.last_name;
            var userAdded = $"{fname[0].ToString().ToUpper()}. {lname}";
            DateTime now = DateTime.Now;
            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_accreditations);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:Downloads\\";
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
        
        private void btn_get_entity_Click(object sender, EventArgs e)
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

        private void dg_contacts_CellClick(object sender, DataGridViewCellEventArgs e)
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
                nameCell.Style.ForeColor = Color.Black;
            }

            if (dg_contacts.Columns[e.ColumnIndex].Name == "position")
            {
                var value = dg_contacts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                MessageBox.Show(CacheData.Positions.Columns["id"].DataType.ToString());

                MessageBox.Show(
                    $"Value: {value}\n" +
                    $"Type: {value?.GetType()}"
                );
            }


        }
        private readonly string[] PhAreaCodes = new string[]
        {
            // Philippine area codes
            "02", "32", "33", "34", "35", "36", "38", "42", "43", "44", "45",
            "46", "47", "48", "49", "82", "83", "84", "85", "86", "87", "88", "89"
        };

        private bool IsValidMobileNumber(string number)
        {

            return number.Length == 11 && (number.StartsWith("09") || number.StartsWith("08"));
        }

        private bool IsValidLandlineNumber(string number)
        {
            if (number.Length != 10)
                return false;

            foreach (var code in PhAreaCodes)
            {
                if (number.StartsWith(code))
                    return true;
            }

            return false;
        }

        private string FormatMobileNumber(string number)
        {
            // 09XX-XXX-XXXX
            return string.Format("{0}-{1}-{2}",
                number.Substring(0, 4), number.Substring(4, 3), number.Substring(7, 4));
        }

        private string FormatLandlineNumber(string number)
        {
            return string.Format("({0}) {1}-{2}",
                number.Substring(0, 2), number.Substring(2, 4), number.Substring(6, 4));
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

            // Contact or Name must be filled
            if (isContactEmpty && isNameEmpty && isPositioNameEmpty && (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 6))
            {
                row.Cells[e.ColumnIndex].ErrorText = "Either Contact Number , Email and Name is required.";
                return;
            }

            // Columns 3, 4, 5 must each be filled
            if ((e.ColumnIndex == 4 && string.IsNullOrEmpty(col4) || (e.ColumnIndex == 5 && string.IsNullOrEmpty(col5))))

            {
                row.Cells[e.ColumnIndex].ErrorText = "This field is required.";
                return;
            }

            row.Cells[e.ColumnIndex].ErrorText = "";
        }

        private void dg_contacts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= NumericOnly_KeyPress;
                tb.TextChanged -= ContactNumberTextChanged;

                if (dg_contacts.CurrentCell.ColumnIndex == 1)
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
        private async Task GetSocialMediaSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.SOCIALS);
            CacheData.SocialMedia = await serviceSetup.GetAsDatatable();

            AddCmbDefaultVal(CacheData.SocialMedia);
            BindCmbValues(cmb_social, CacheData.SocialMedia);

        }
        private async Task GetPayments()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();

            AddCmbDefaultVal(CacheData.PaymentTerms);
            
            var financeDt = CacheData.PaymentTerms.Copy();
            var financeAccountDt = CacheData.PaymentTerms.Copy();
            var itemAccountDt = CacheData.PaymentTerms.Copy();

            // finance tab
            BindCmbValues(cmb_finance_payment_terms, financeDt);
            BindCmbValues(cmb_finance_account, financeAccountDt);
            // item tab
            BindCmbValues(cmb_payment_terms, CacheData.PaymentTerms);
            BindCmbValues(cmb_item_account, itemAccountDt);

        }

        private void cmb_payment_terms_Click(object sender, EventArgs e)
        {

        }

        private void btn_items_payment_terms_Click(object sender, EventArgs e)
        {
            bool showSelectColumn = true;

            SetupModal finance_modal = new SetupModal("Payment Terms Setup", ENUM_ENDPOINT.PAYMENT_TERMS, CacheData.PaymentTerms, showSelectColumn);
            DialogResult r = finance_modal.ShowDialog();
        }
        private void GetPaymentItemTerms(bool isItem)
        {
            if (!isItem) return;

            int bpiId = int.Parse(bpi.Rows[this.SelectedRecord]["id"].ToString());

            var matchedRow = items.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("bpi_item_based_id") == bpiId);

            if (matchedRow != null)
            {
                cmb_payment_terms.SelectedValue = matchedRow.Field<int>("payment_terms_id");
                cmb_payment_terms.SelectedItem = matchedRow;
                cmb_tax_code.Text = matchedRow.Field<int>("payment_terms_id").ToString();
            }
        }

        private void GetTaxCode()
        {
            cmb_tax_code.DataSource = ENUM_TAX_CODE.LIST();
            cmb_tax_code.DisplayMember = "title";
        }

        private void btn_add_item_Click(object sender, EventArgs e)
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

        private void dg_items_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dg_items.Columns["item_graph"].Index)
            {
                e.PaintBackground(e.CellBounds, true);

                Image icon = Properties.Resources.line_chart;
                int size = 16;

                int x = e.CellBounds.Left + (e.CellBounds.Width - size) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - size) / 2;

                e.Graphics.DrawImage(icon, x, y, size, size);

                e.Handled = true;
            }
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
                        selectedRow.Cells["item_type"].Value = result["item_type"];
                        selectedRow.Cells["item_code"].Value = result["item_code"];
                        selectedRow.Cells["long_description"].Value = result["long_description"];
                        selectedRow.Cells["price"].Value = result["item_price"];

                        // Add new empty row to data source (assumes DataTable binding)

                    }



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
        private async Task GetPositionSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.POSITION);
            CacheData.Positions = await serviceSetup.GetAsDatatable();

            DataRow newRow = CacheData.Positions.NewRow();
            newRow["id"] = 0;
            newRow["name"] = "--Select--";

            CacheData.Positions.Rows.InsertAt(newRow, 0);


            var combobox = (DataGridViewComboBoxColumn)dg_contacts.Columns["position"];
            combobox.DataSource = CacheData.Positions;
            combobox.DataPropertyName = "position";
            combobox.DisplayMember = "code";
            combobox.ValueMember = "id";

        }
        private void ResetData(bool isIncluded)
        {
            MessageBox.Show("RESET FOR NEW RECORD");
            // panel_general.Visible = true;

            BpiBranchToggle();




            BtnToogle(true);

            if (isIncluded)
            {
                // Reset panels
                Helpers.ResetControls(panel_general);
                Helpers.ResetControls(panel_item);
                Helpers.ResetControls(panel_finance);
            }

            // Reset finance tab 
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

            // Reset comboboxes general tab
            currentSelectedIndustryIds.Clear();
            currentSelectedEntityIds.Clear();
            currentSelectedBranchIndustryIds.Clear();
            selectedPreferenceNames.Clear();

            // Reset Contact Tab
            DataTable clonedContacts = contacts.Clone();
            DataRow newRow = clonedContacts.NewRow();   // Create a new row
            newRow["position"] = DBNull.Value;
            clonedContacts.Rows.Add(newRow);

            // Reset binding sopurce
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
        private void BtnToogle(bool isEdit)
        {
            panel_general.Enabled = isEdit;
            pnl_new_added_item.Enabled = isEdit;

            panel_finance.Enabled = isEdit;
            dg_contacts.Enabled = isEdit;
            dg_address.Enabled = isEdit;
            dg_items.Enabled = isEdit;
            dg_accreditations.Enabled = isEdit;


            


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
        public  Dictionary<string,object> GetGeneralData()
        {
            if (currentSelectedBranchIndustryIds.Count != 0 && ParentId != "")
            {
                txt_branch_industry.Tag = currentSelectedBranchIndustryIds;
            }
            if (currentSelectedEntityIds.Count != 0 && ParentId != "")
            {
                txt_entity_type.Tag = currentSelectedEntityIds;
            }

            var GeneralData = Helpers.GetControlsValues(panel_general);
            return GeneralData;
        }

        public List<BpiContacts> GetContactData(bool isUpdate)
        {

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_contacts);
            List<BpiContacts> listContacts = new List<BpiContacts>();


            BpiContacts contacts = null;
            int contacts_id = 0;
            int contacts_based_id = 0;
            int branch_id = 0;
            bool is_default_contact = false;
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

                contacts = new BpiContacts(contacts_id, contacts_based_id, number, name, email, preferences, contactPositionId, branch_id, notes, is_default_contact);
                listContacts.Add(contacts);

            }


            return listContacts;
        }
        public List<BpiAddress> GetAdressData(bool isUpdate)
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
        public Dictionary<string, dynamic> GetFinanceData()
        {
            var FinanceData = Helpers.GetControlsValues(panel_finance);

            return FinanceData;
        }
        public List<BpiAccreditation> GetAccreditationData(bool isUpdate)
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
        public List<BpiItems> GetItemsData(bool isUpdate)
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
        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
        private void dg_contacts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        public void CommitEdits()
        {
            dg_contacts.EndEdit();
            dg_address.EndEdit();
            dg_items.EndEdit();
            dg_accreditations.EndEdit();
        }
        private static void AddCmbDefaultVal(DataTable dt)
        {
            if (dt == null) return;

            DataRow newRow = dt.NewRow();
            newRow["id"] = 0;
            newRow["name"] = "-- SELECT --";

            dt.Rows.InsertAt(newRow, 0);
        }
        private static void BindCmbValues(ComboBox cmb, DataView dv)
        {
            cmb.DataSource = dv;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            cmb.SelectedIndex = 0;
        }
        private static void BindCmbValues(ComboBox cmb, DataTable dt)
        {
            cmb.DataSource = dt;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            cmb.SelectedIndex = 0;
        }
        private void CheckPanelsInTabPage(TabPage tabPage, params Panel[] panels)
        {
            foreach (var panel in panels)
            {
                if (tabPage.Controls.Contains(panel))
                {
                    Console.WriteLine($"Panel '{panel.Name}' is added to TabPage '{tabPage.Name}'.");
                }
                else
                {
                    Console.WriteLine($"Panel '{panel.Name}' is NOT added to TabPage '{tabPage.Name}'.");
                }

                // Optional: check the panel's parent
                if (panel.Parent == tabPage)
                {
                    Console.WriteLine($"Panel '{panel.Name}' parent is correctly set to '{tabPage.Name}'.");
                }
                else
                {
                    Console.WriteLine($"Panel '{panel.Name}' parent is NOT set to '{tabPage.Name}', current parent: {panel.Parent?.Name ?? "null"}");
                }
            }
        }
        private void ListControlsInPanel(Panel panel)
        {
            Console.WriteLine($"Listing controls inside panel '{panel.Name}':");

            foreach (Control ctrl in panel.Controls)
            {
                Console.WriteLine($"- Control Name: {ctrl.Name}, Type: {ctrl.GetType().Name}, Visible: {ctrl.Visible}");
            }
        }
        private void AddOwnedByOtherSalesLabel()
        {
            // Create the label
            Label lblOwnedByOtherSales = new Label();
            lblOwnedByOtherSales.Name = "lblOwnedByOtherSales";  // optional but useful
            lblOwnedByOtherSales.Text = "Owned by other sales";
            lblOwnedByOtherSales.AutoSize = true; // adjusts size to fit text
            lblOwnedByOtherSales.ForeColor = Color.Red; // optional styling
            lblOwnedByOtherSales.Font = new Font("Segoe UI", 9, FontStyle.Bold); // optional styling

            // Set the position inside panel_general
            lblOwnedByOtherSales.Location = new Point(20, 300); // adjust X,Y as needed

            // Add to panel_general
            panel_general.Controls.Add(lblOwnedByOtherSales);

            // Make sure the panel is visible
            lblOwnedByOtherSales.Visible = true;
        }
        private void DisbleAutoColumnGeneration(List<DataGridView> dgvs)
        {
            foreach (var dgv in dgvs)
            {
                dgv.AutoGenerateColumns = false;
            }
        }

        
        private void HideSystemColumns(DataGridView dgv, string tab)
        {
            string[] hidden = GetHiddenColumns(tab);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!string.IsNullOrEmpty(col.DataPropertyName))
                    col.Visible = !hidden.Contains(col.DataPropertyName);
            }
        }

        private string[] GetHiddenColumns(string tab)
        {
            if (tab == "contacts")
                return new[] { "contacts_id", "contacts_based_id", "branch_id" };

            if (tab == "address")
                return new[] { "address_ids", "address_based_id", "address_branch_id", "address_is_deleted" };

            if (tab == "items")
                return new[] { "item_id", "bpi_item_id", "bpi_item_based_id", "bpi_item_branch_id", "item_is_deleted" };

            if (tab == "accreditation")
                return new[]
                {
                    "bpi_accreditation_id",
                    "bpi_accreditation_based_id",
                    "bpi_accreditation_branch_id",
                    "accreditation_added_by_id"
                };
            if (tab == "history")
                return new[] { "edit_history_id", "branch_id", "actions" };

            // default
            return new string[0];
        }
        private void dg_contacts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "contacts");
        }
        private void dg_address_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "address");
        }

        private void dg_finance_transactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //HideSystemColumns((DataGridView)sender, "items");
        }

        private void dg_accreditations_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "accreditation");
        }

        private void dg_items_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "items");
        }

        private void dg_history_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "history");
        }

        private void dg_contacts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }
    }
}
