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
using smpc_inventory_app.Services.Setup.Bpi;
using System.Diagnostics;
using System.Globalization;

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
        private Dictionary<string, List<ComboBox>> _endpointCmbMap;
        // Company Setup's VAT rate; null when it could not be read.
        private double? _vatRatePercent;
        // Set while stored values are put on screen, so the tax-code handler keeps the stored
        // rate instead of clearing it as it does when the user changes the code.
        private bool _bindingTaxCodes;
        private bool _isReadOnly;
        public bool isUpdate { get; set; }
        private Bpi_Class _preloadedData;
        public BpiBranchUC(string parentId, string salesId, string tabTitle,
                   string canvassForm, bool isExisting,
                   Bpi_Class preloadedData = null)
        {
            InitializeComponent();

            // Bug #290: Branch Tel No. field had no placeholder, giving no hint of the
            // expected format - same fix already applied to the main branch's tel no field.
            Helpers.Placeholder.SetPlaceholder(txt_branch_tel_no, "09XX-XXX-XXXX / (0XX) XXXX-XXXX");

            this.ParentId = parentId;
            this.SalesId = salesId;
            this.TabTitle = tabTitle;
            this.CanvassForm = canvassForm;
            this.IsExisting = isExisting;
            this._preloadedData = preloadedData;

            if (!string.IsNullOrEmpty(canvassForm))
                ShowCanvassTabPage();

            tabItemPages = tabControl2.TabPages["ITEMS"];
            tabFinancePages = tabControl2.TabPages["FINANCE"];

            if (!isExisting)
            {
                tabControl2.TabPages.Remove(tabItemPages);
                tabControl2.TabPages.Remove(tabFinancePages);
            }
            CheckPanelsInTabPage(GENERAL, panel_general);

        }

        private async void BpiBranchUC_Load(object sender, EventArgs e)
        {
            txt_branch_name.Text = TabTitle;
            InitializeCmbMap();

            // Always initialize combos first — both paths need them
            await Task.WhenAll(
                GetSocialMediaSetup(),
                GetPayments(),
                GetPositionSetup(),
                GetAccounts(),
                GetVatRate()
            );
            GetTaxCode();

            if (_preloadedData != null)
                BindFromPreloadedData(_preloadedData);
            else
                await LoadBpidData();
        }
        private void BindFromPreloadedData(Bpi_Class data)
        {
            try
            {
                Records = data;
                // In BindFromPreloadedData, after setting the tables:
                // bpi is the full table — child UC always filters by ParentId, never by SelectedRecord index
                bpi = Helpers.SafeTable(data.bpi);
                general = Helpers.SafeTable(data.general);
                contacts = Helpers.SafeTable(data.contacts);
                address = Helpers.SafeTable(data.address);
                items = Helpers.SafeTable(data.items);
                finance = Helpers.SafeTable(data.finance);
                finance_pending = Helpers.SafeTable(data.finance_pending);
                accreditations = Helpers.SafeTable(data.accreditations);
                history = Helpers.SafeTable(data.history);

                // Filter only this branch's general row using ParentId (= general_id)
                DataView dv = new DataView(general)
                {
                    RowFilter = $"general_id = '{ParentId}'"
                };

                if (dv.Count == 0)
                {
                    // No matching row yet (new unsaved branch) — nothing to bind
                    return;
                }

                if (IsExisting)
                {
                    if (this.InvokeRequired)
                        this.BeginInvoke(new Action(() => BindGeneral(true)));
                    else
                        BindGeneral(true);
                }

                if (IsMain)
                    chk_is_main.Checked = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BindFromPreloadedData] {ex.Message}");
                MessageBox.Show("Failed to bind branch data.");
            }
        }
        private async Task LoadBpidData()
        {
            try
            {
                var response = await RequestToApi<ApiResponseModel<Bpi_Class>>.Get(ENUM_ENDPOINT.BPI);
                if (response?.Data == null || response.Data.bpi == null)
                {
                    MessageBox.Show("No records found.");
                    return;
                }

                Records = response.Data;

                // heavy work off the UI thread
                var tables = await Task.Run(() => new
                {
                    Bpi = Helpers.SafeTable(Records.bpi),
                    General = Helpers.SafeTable(Records.general),
                    Contacts = Helpers.SafeTable(Records.contacts),
                    Address = Helpers.SafeTable(Records.address),
                    Items = Helpers.SafeTable(Records.items),
                    Finance = Helpers.SafeTable(Records.finance),
                    FinancePending = Helpers.SafeTable(Records.finance_pending),
                    Accreditations = Helpers.SafeTable(Records.accreditations),
                    History = Helpers.SafeTable(Records.history),
                });

                if (tables == null) return;

                bpi = tables.Bpi;
                general = tables.General;
                contacts = tables.Contacts;
                address = tables.Address;
                items = tables.Items;
                finance = tables.Finance;
                finance_pending = tables.FinancePending;
                accreditations = tables.Accreditations;
                history = tables.History;

                if (Records.bpi.Count > 0 && Records.general.Count > 0
                    && Records.contacts.Count > 0 && Records.address.Count > 0)
                {
                    if (IsExisting)
                    {
                        if (this.InvokeRequired)
                            this.BeginInvoke(new Action(() => BindGeneral(true)));
                        else
                            BindGeneral(true);
                    }

                    if (IsMain)
                        chk_is_main.Checked = true;
                }
                else
                {
                    MessageBox.Show("No records found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoadBpidData] {ex.Message}");
                MessageBox.Show("Failed to load BPI data. Please try again.");
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
            if (!isBind) return;

            var isSelectedSales = GetSelectedSales();
            BpiBranchToggle(isSelectedSales);

            // Use BindDataToTable (filters by ParentId) instead of LoadAllBpiChild
            // LoadAllBpiChild uses SelectedRecord index which is wrong in child UC
            BindDataToPanel();
            BindDataToTable();
            BindDataToComboBox();
            BindMultiSelectField(Records.general);

            bool isItemShow = ToggleItemPages(txt_entity_type.Text);
            GetPaymentItemTerms(isItemShow);
            ShowTypeOfEntity(txt_entity_type.Text);
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

            _bindingTaxCodes = true;
            try
            {
                if (filteredFinance != null && filteredFinance.Rows.Count > 0)
                {
                    Helpers.BindControls(pnlFinancePanel, filteredFinance);
                }
                if (filteredItems != null && filteredItems.Rows.Count > 0)
                {
                    Helpers.BindControls(pnlItemPanel, filteredItems);
                }

                // BindControls matches a combo by Name.Contains(column), so the finance tax
                // code combo also receives finance_tax (the rate). Put both tax codes on
                // screen explicitly, after it. No row yet means VAT, the standard default
                // (4.5.3); a row with no code shows as unset.
                BindTaxCode(cmb_finance_tax_code, txt_finance_tax, filteredFinance, "finance_tax_code", "finance_tax");
                BindTaxCode(cmb_tax_code, txt_item_tax_code, filteredItems, "tax_code", "item_tax_code");
            }
            finally
            {
                _bindingTaxCodes = false;
            }
        }

        private void BindTaxCode(ComboBox code, TextBox rate, DataTable rows, string codeColumn, string rateColumn)
        {
            DataRow row = rows?.AsEnumerable().FirstOrDefault(r =>
                !rows.Columns.Contains("item_is_deleted") || !string.Equals(r["item_is_deleted"]?.ToString(), "True", StringComparison.OrdinalIgnoreCase))
                ?? rows?.AsEnumerable().FirstOrDefault();

            if (row == null)
            {
                rate.Text = string.Empty;
                SetTaxCode(code, rate, ENUM_TAX_CODE.VAT);
                return;
            }

            rate.Text = row.Table.Columns.Contains(rateColumn) ? row[rateColumn]?.ToString() : string.Empty;
            SetTaxCode(code, rate, row.Table.Columns.Contains(codeColumn) ? row[codeColumn]?.ToString() : string.Empty);
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
            // customer_id on a quotation is the partner's BPI id, not a branch id, so filtering
            // it by this branch's id matched nothing - or another partner's quotes whenever the
            // two numbers happened to coincide. The view already resolves each quotation to its
            // partner's main branch as finance_pending_branch_id; that is the branch to match.
            var financePendingView = new DataView(finance_pending)
            {
                RowFilter = $"finance_pending_branch_id = '{parentId}'"
            };
            DataTable filteredPending = financePendingView.ToTable();
            dataBindingFinancePending.DataSource = filteredPending;
            ShowAccountBalance(filteredPending);

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
            txt_branch_industry.Tag = new List<int>(currentSelectedBranchIndustryIds);
        }
        private void SetComboBoxValue(DataTable table, string filterColumn, int filterValue, ComboBox combo, string valueColumn)
        {
            if (combo.Items.Count == 0) return; // ← guard: nothing to select

            var row = table.AsEnumerable()
                .FirstOrDefault(r => Convert.ToInt32(r[filterColumn]) == filterValue);

            if (row != null && !row.IsNull(valueColumn))
            {
                int value = row.Field<int>(valueColumn);
                bool exists = combo.Items.Cast<DataRowView>()
                    .Any(drv => Convert.ToInt32(drv[combo.ValueMember]) == value);

                combo.SelectedValue = exists ? (object)value : (combo.Items.Count > 0 ? combo.Items[0] : null);
                if (!exists && combo.Items.Count > 0)
                    combo.SelectedIndex = 0;
            }
            else if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }
        private bool GetSelectedSales()
        {
            // Whether this branch's CONTENTS may be shown - its name always is
            // (spec 4.1.10). SalesId is this branch's own owner (branch_sales_id);
            // the role rules live in BpiAccess.
            return smpc_inventory_app.Model.BpiAccess.CanView(CacheData.CurrentUser, SalesId, BranchEntityCodes());
        }

        // entity_names of this branch, e.g. "SUP,CUS" - purchasing views every
        // supplier branch.
        private string BranchEntityCodes()
        {
            int id;
            if (Records?.general == null || !int.TryParse(ParentId, out id)) return string.Empty;
            return Records.general.FirstOrDefault(x => x.general_id == id)?.entity_names ?? string.Empty;
        }
        private void ShowTypeOfEntity(string txt)
        {
            switch (txt)
            {
                case "NON-AFFILIATED":
                    ToggleEntityField(true);
                    ToggleCustomerAndSupplier(false);

                    break;
                case "AFFILIATED":
                    ToggleEntityField(false);
                    ToggleCustomerAndSupplier(false);

                    break;


                default:
                    ShowAffiliatedAndNon(false);

                    break;
            }
        }
        private bool ToggleItemPages(string text)
        {
            bool item = false;
            string[] valuesToCheck = { "SUP", "CUS" };
            var viewData = String.Join("", text);
            bool containsBoth = valuesToCheck.All(value => viewData.Contains(value));
            if (containsBoth)
            {
                ShowTabPages(tabItemPages);
                item = true;
            }
            else if (text.Contains("SUP"))
            {
                ShowTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                item = true;
            }
            else if (text.Contains("EA"))
            {
                RemoveTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                item = false;
            }

            else if (text.Contains("EN"))
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


            ShowOtherSalesNotice(!isVisible);

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

                var salesOwner = smpc_inventory_app.Model.SalesOwner.OwnedBy(row["branch_sales_id"].ToString(), CacheData.CurrentUser);
                var matchSelectedSales = Users.FirstOrDefault(salesUser => smpc_inventory_app.Model.SalesOwner.Same(salesUser.full_name, row["branch_sales_id"].ToString()));
                string selectedSalesNames = "";
                if (matchSelectedSales != null)
                {
                    selectedSalesNames = $"({matchSelectedSales.first_name.Substring(0, 1).ToUpper()}. {matchSelectedSales.last_name})";
                    //selectedSalesNames = txt_sales_id.Text;
                }
                // Was: salesOwner ? "PURCH-PO-8" : selectedSalesNames - a leftover document
                // number shown to the OWNER as their own tooltip. Now everyone sees the
                // owner's name, falling back to the stored name when there is no user record.
                string storedOwner = row["branch_sales_id"].ToString();
                string selectedSalesName = !string.IsNullOrEmpty(selectedSalesNames) ? selectedSalesNames
                    : (string.IsNullOrWhiteSpace(storedOwner) ? "" : "(" + storedOwner + ")");

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
        private void OpenSetupModal(string title, string api, DataTable cacheData)
        {
            if (cacheData == null) return;

            DataTable dt = cacheData.Copy();
            if (dt.Columns["select"] != null)
                dt.Columns.Remove("select");

            modalSetup = new SetupModal(title, api, dt);
            modalSetup.OnDataChanged += async () => await RefreshCache(api);
            modalSetup.ShowDialog();
        }
        private async Task RefreshCache(string api)
        {
            serviceSetup = new GeneralSetupServices(api);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            switch (api)
            {
                case var _ when api == ENUM_ENDPOINT.INDUSTRIES:
                    CacheData.Industries = result;
                    // Same setup list; the branch picker of a saved partner reads this copy,
                    // so an industry added with + showed in the header picker only.
                    CacheData.BranchIndustries = result.Copy();
                    break;
                case var _ when api == ENUM_ENDPOINT.ENTITY:
                    CacheData.Entity = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.SOCIALS:
                    CacheData.SocialMedia = result;
                    break;

                default:
                    return;
            }

            // Bind the corresponding ComboBox after cache update
            if (_endpointCmbMap.TryGetValue(api, out List<ComboBox> cmbs))
                foreach (var cmb in cmbs)
                    BindCmbValues(cmb, result);
        }
        private void btn_add_entity_Click(object sender, EventArgs e) =>
             OpenSetupModal("Entity", ENUM_ENDPOINT.ENTITY, CacheData.Entity);

        private void btn_branch_industry_Click(object sender, EventArgs e) =>
            OpenSetupModal("INDUSTRIES", ENUM_ENDPOINT.INDUSTRIES, CacheData.Industries);

        private void btn_social_links_Click(object sender, EventArgs e) =>
            OpenSetupModal("SOCIAL MEDIA", ENUM_ENDPOINT.SOCIALS, CacheData.SocialMedia);
        // Raised when the user picks this branch's industries. The form copies them to the header
        // INDUSTRIES when this is the main branch (spec 4.1.3: MAIN mirrors the header both ways).
        public event Action<BpiBranchUC, string, List<int>> BranchIndustriesPicked;

        // The industry picker behind the header INDUSTRIES and every BRANCH INDUSTRY. False when
        // cancelled. The picked industries are shown by CODE, joined the way the saved list comes
        // back from the API ("RET,AUTO,EDU,HW"), so a field reads the same before and after a
        // pick and the header and the main branch read alike (user instruction 2026-09-17: codes,
        // not names). It used to write names, so a picked field and a loaded one never matched.
        internal static bool PickIndustries(string title, DataTable source, List<int> current, out string codes, out List<int> ids)
        {
            codes = string.Empty;
            ids = new List<int>();
            current = current ?? new List<int>();

            // The picker keeps its ticks in the table between uses and only resets them when it
            // is given ids. With none, clear what is left over, or an earlier pick shows ticked.
            if (current.Count == 0 && source != null && source.Columns.Contains("select"))
                foreach (DataRow row in source.Rows)
                    row["select"] = false;

            using (var modal = new SetupSelectionModal(title, ENUM_ENDPOINT.INDUSTRIES, source, current, new List<string>(), 0))
            {
                if (modal.ShowDialog() != DialogResult.OK) return false;

                var picked = new List<string>();
                foreach (DataRowView row in modal.GetResult())
                {
                    if (int.TryParse(row["id"]?.ToString(), out int id)) ids.Add(id);
                    picked.Add(row["code"]?.ToString());
                }
                codes = string.Join(",", picked);
                return true;
            }
        }

        // The ids now shown in BRANCH INDUSTRY. Kept current instead of being cleared after a
        // pick: GetGeneralData writes this list over the field's Tag, so a stale copy saved the
        // old industries, and the picker opened with the wrong ones ticked.
        private List<int> ShownBranchIndustryIds() =>
            txt_branch_industry.Tag as List<int> ?? new List<int>(currentSelectedBranchIndustryIds);

        private void btn_get_branch_Click(object sender, EventArgs e)
        {
            var branchData = string.IsNullOrEmpty(ParentId) ? CacheData.Industries : CacheData.BranchIndustries;
            if (!PickIndustries("Branch Industries", branchData, ShownBranchIndustryIds(), out string codes, out List<int> ids))
                return;

            SetBranchIndustries(codes, ids);
            BranchIndustriesPicked?.Invoke(this, codes, ids);
        }

        // Puts industries on this branch without raising BranchIndustriesPicked, so a copy from
        // the header never bounces back.
        public void SetBranchIndustries(string codes, List<int> ids)
        {
            txt_branch_industry.Text = codes;
            txt_branch_industry.Tag = new List<int>(ids);
            currentSelectedBranchIndustryIds = new List<int>(ids);
        }
        private void InitializeCmbMap()
        {
            _endpointCmbMap = new Dictionary<string, List<ComboBox>>
            {
                { ENUM_ENDPOINT.SOCIALS,         new List<ComboBox> { cmb_social } },
            };
        }
        public void SetMainBranchFields(string telNo, string website, string industryText, List<int> industryIds)
        {
            txt_branch_tel_no.Text = telNo;
            txt_branch_website.Text = website;
            SetBranchIndustries(industryText, industryIds ?? new List<int>());
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

        private void btn_finance_payment_terms_Click(object sender, EventArgs e) =>
            OpenSetupModal("PAYMENT TERMS", ENUM_ENDPOINT.PAYMENT_TERMS, CacheData.PaymentTerms);

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
            // Added to the table the grid is actually bound to. This used to rebuild a fresh
            // table from the grid's columns and rebind to it: the copy had no short_desc /
            // status_* columns, so the next ADD ITEM or multi-pick threw part-way, and the
            // new item lived in a table the rest of the tab was no longer reading.
            DataTable table = CurrentItemsTable();
            DataRow addedRow = table.NewRow();
            SetItemFields(addedRow, value);
            table.Rows.Add(addedRow);
            dataBindingItems.EndEdit();
        }

        // Every column the Items tab writes when an item is picked or added.
        private static readonly string[] ItemFieldColumns =
        {
            "item_id", "item_code", "item_type", "long_description", "short_desc",
            "status_tangible", "status_trade", "price", "notes", "item_is_deleted",
        };

        // The table the Items grid is bound to, with every column in ItemFieldColumns present.
        // A new partner starts from items.Clone() and a loaded one from a filtered copy of
        // vw_bpi_items, and neither is guaranteed to carry them all - writing a column that is
        // not there throws "Column ... does not belong to table".
        private DataTable CurrentItemsTable()
        {
            if (!(dataBindingItems.DataSource is DataTable table))
            {
                table = Helpers.ConvertDataGridViewToDataTable(dg_items);
                dataBindingItems.DataSource = table;
            }

            foreach (string column in ItemFieldColumns)
            {
                if (!table.Columns.Contains(column))
                    table.Columns.Add(column);
            }

            return table;
        }

        // Copies a picked or newly created item onto a row. The keys are the Item List modal's
        // and the Item Entry callback's; a key the source does not send leaves its column alone.
        private static void SetItemFields(DataRow row, Dictionary<string, dynamic> item)
        {
            void Set(string column, string key)
            {
                if (item.TryGetValue(key, out dynamic value) && row.Table.Columns.Contains(column))
                    row[column] = (object)value ?? DBNull.Value;
            }

            Set("item_id", "item_id");
            Set("item_code", "item_code");
            Set("long_description", "long_description");
            Set("short_desc", "short_desc");
            Set("status_tangible", "status_tangible");
            Set("status_trade", "status_trade");
            Set("price", "item_price");

            // TYPE is the item's tangibility. Item Entry's callback sends it only as
            // status_tangible.
            Set("item_type", item.ContainsKey("item_type") ? "item_type" : "status_tangible");
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
                openFileDialog.InitialDirectory = @"C:\Downloads\";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;
                openFileDialog.Filter =
                    "All Supported Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp;" +
                                       "*.docx;*.doc;*.xlsx;*.xls;*.pptx;*.ppt|" +
                    "Images|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp|" +
                    "Word Documents|*.docx;*.doc|" +
                    "Excel Spreadsheets|*.xlsx;*.xls|" +
                    "PowerPoint Presentations|*.pptx;*.ppt|" +
                    "All Files|*.*";
                openFileDialog.FilterIndex = 1;

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
            modalSelection = new SetupSelectionModal("ENTITY", ENUM_ENDPOINT.ENTITY, CacheData.Entity, currentSelectedEntityIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();
            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();
                Helpers.GetModalData(txt_entity_type, result);
                ProcessEntitySelection();
                currentSelectedEntityIds.Clear();
            }
        }
        // The picked entity types as setup codes (SUP, CUS, TSP, NAF, AFF...), read from the
        // ids the picker stored. The picker shows names, and names differ by database -
        // "Supplier" on test_fresh, "SUPPLIER" on the rehearsal DB - so the old upper-case
        // name comparison sent a supplier down the customer branch on test_fresh, which
        // disabled its Supplier Code box.
        private HashSet<string> SelectedEntityCodes()
        {
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            DataTable entities = CacheData.Entity;
            if (entities == null || !entities.Columns.Contains("code")) return codes;

            if (txt_entity_type.Tag is List<int> ids && ids.Count > 0)
            {
                foreach (DataRow row in entities.Rows)
                    if (int.TryParse(row["id"]?.ToString(), out int id) && ids.Contains(id))
                        codes.Add(row["code"]?.ToString().Trim() ?? string.Empty);
                return codes;
            }

            // No ids to go on: match the listed names, ignoring case.
            var names = new HashSet<string>(
                txt_entity_type.Text.Split(',').Select(n => n.Trim()).Where(n => n.Length > 0),
                StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in entities.Rows)
                if (names.Contains(row["name"]?.ToString().Trim() ?? string.Empty))
                    codes.Add(row["code"]?.ToString().Trim() ?? string.Empty);
            return codes;
        }

        // C# and S# are issued by the API when the partner is saved (spec 4.1.3) - it ignores
        // any code the form sends, and two users saving at once take turns there - so a new
        // partner's code box stays blank until the save returns (user instruction 2026-09-17).
        // The form used to preview "C#" + a count that was never fetched here, so it always
        // read C#1 / S#1. This returns the code the branch was saved with, if any: an existing
        // customer that gains the Supplier type keeps showing its own C#.
        private string SavedCode(bool customer)
        {
            if (Records?.general == null || !int.TryParse(ParentId, out int id)) return null;
            var saved = Records.general.FirstOrDefault(x => x.general_id == id);
            string code = customer ? saved?.customer_code : saved?.supplier_code;
            return string.IsNullOrWhiteSpace(code) ? null : code;
        }

        private void ProcessEntitySelection()
        {
            txt_customer_code.Text = "";
            txt_supplier_code.Text = "";
            txt_non_affiliated.Text = "";
            txt_affiliated.Text = "";

            var data = txt_entity_type.Text;
            string[] entities = data.Split(',');
            HashSet<string> codes = SelectedEntityCodes();
            bool isCustomer = codes.Contains("CUS");
            bool isSupplier = codes.Contains("SUP");
            // Temporary Supplier is TSP on test_fresh and "SUP (TEMP)" on the rehearsal DB.
            // It gets no S# (the API issues one for SUP only) but it is a supplier for the tabs.
            bool isSupplierType = isSupplier || codes.Contains("TSP") || codes.Contains("SUP (TEMP)");

            bool hasBlackListed = entities.Any(n => n.Trim() == ENUM_ENTITY_TYPE.Blacklisted);
            bool hasTempSupplier = entities.Any(n => n.Trim() == ENUM_ENTITY_TYPE.TempSupplier);

            if (hasBlackListed)
            {
                txt_entity_type.Text = "";
                txt_entity_type.Tag = null;
                currentSelectedEntityIds.Clear();
                Helpers.ShowDialogMessage("warning", "Cannot select BLACKLISTED based on your position");
                return;
            }

            if (CanvassForm == "" && hasTempSupplier)
            {
                txt_entity_type.Text = "";
                txt_entity_type.Tag = null;
                currentSelectedEntityIds.Clear();
                Helpers.ShowDialogMessage("warning","You Cannot Select Temporary Supplier");
                return;
            }

            if (isCustomer && isSupplierType)
            {
                DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Customer);
                if (isSupplier) DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Supplier);
                ToggleCustomerAndSupplier(true);
                ShowTabPages(tabItemPages);
                ShowTabPages(tabFinancePages);
            }
            else if (isSupplierType)
            {
                if (isSupplier) DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Supplier);
                ToggleCustomerAndSupplier(true);
                ShowAffiliatedAndNon(false);
                ShowTabPages(tabItemPages);
                RemoveTabPages(tabFinancePages);
                txt_customer_code.Enabled = false;
            }
            else if (codes.Contains("NAF"))
            {
                DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Non_Affiliated);
                ToggleEntityField(true);
                ToggleCustomerAndSupplier(false);
                RemoveTabPages(tabFinancePages);
                RemoveTabPages(tabItemPages);
            }
            else if (codes.Contains("AFF"))
            {
                DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Affiliated);
                ToggleEntityField(false);
                ToggleCustomerAndSupplier(false);
                RemoveTabPages(tabFinancePages);
            }
            else if (isCustomer)
            {
                DocumentCodeIncrementor(ENUM_ENTITY_TYPE.Customer);
                ShowAffiliatedAndNon(false);
                ShowTabPages(tabFinancePages);
                ToggleCustomerAndSupplier(true);
                RemoveTabPages(tabItemPages);
                txt_supplier_code.Enabled = false;
                btn_finance_payment_terms.Visible = CacheData.CurrentUser.position_id.Equals("Web Developer");
            }
            else
            {
                // Neither customer nor supplier (e.g. Closed only, or nothing picked): no C#
                // or S# is issued (4.1.3), and neither Finance nor Items applies (4.1).
                // This used to fall into the customer branch.
                ShowAffiliatedAndNon(false);
                ToggleCustomerAndSupplier(false);
                RemoveTabPages(tabFinancePages);
                RemoveTabPages(tabItemPages);
            }
        }
        private void ToggleEntityField(bool isShow)
        {
            txt_non_affiliated.Visible = isShow;
            lbl_non_affiliated.Visible = isShow;

            lbl_affiliated.Visible = !isShow;
            txt_affiliated.Visible = !isShow;
        }
        private void ToggleCustomerAndSupplier(bool isEnabled)
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

                    txt_supplier_code.Text = SavedCode(customer: false) ?? string.Empty;

                    break;

                case "CUSTOMER":
                    txt_customer_code.Text = SavedCode(customer: true) ?? string.Empty;

                    break;

                case "NON-AFFILIATED":

                    txt_non_affiliated.Text = "EN#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Non_Affiliated) + 1);

                    break;
                case "AFFILIATED":

                    txt_affiliated.Text = "EA#" + (GetEntityRecordCount(ENUM_ENTITY_TYPE.Affiliated) + 1);
                    break;

                case "BOTH":

                    txt_customer_code.Text = SavedCode(customer: true) ?? string.Empty;
                    txt_supplier_code.Text = SavedCode(customer: false) ?? string.Empty;

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
            var record = entityCount?.FirstOrDefault(records => records.code == code);
            return record?.entity_count ?? 0;
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
            // was index 1 (contacts_based_id) — fixed to 4 (number)
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                var cell = dg_contacts.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string currentValue = cell.Value?.ToString();

                if (!string.IsNullOrEmpty(currentValue))
                {
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
                    DataTable filterSocialMedia = CacheData.SocialMedia.AsEnumerable()
                        .Where(row => !row.Field<string>("name").Contains("-"))
                        .CopyToDataTable();

                    modalSelection = new SetupSelectionModal("Preferences", ENUM_ENDPOINT.SOCIALS, filterSocialMedia, new List<int> { }, selectedPreferenceNames, index);

                    DialogResult modalResult = modalSelection.ShowDialog();

                    if (modalResult == DialogResult.OK)
                    {
                        DataView result = modalSelection.GetResult();
                        var selectedPreferences = result.Cast<DataRowView>()
                            .Select(row => row["code"].ToString())
                            .ToList();

                        if (selectedPreferenceNames.Count != 0)
                        {
                            selectedPreferenceNames[index] = string.Join(",", selectedPreferences);
                        }

                        dg_contacts.Rows[e.RowIndex].Cells["preferences"].Value = string.Join(",", selectedPreferences);
                    }
                }
            }
        }
        private void dg_contacts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string currentColumn = dg_contacts.Columns[e.ColumnIndex].Name;

            // Only process known editable columns
            string[] handledColumns = { "number", "email", "position", "name" };
            if (!handledColumns.Contains(currentColumn)) return;

            var row = dg_contacts.Rows[e.RowIndex];
            var numberCell = row.Cells["number"];
            var nameCell = row.Cells["name"];
            var positionCell = row.Cells["position"];
            var emailCell = row.Cells["email"];

            string numberRaw = numberCell.Value?.ToString()?.Trim() ?? "";
            string nameRaw = nameCell.Value?.ToString()?.Trim() ?? "";
            string positionRawName = positionCell.Value?.ToString()?.Trim() ?? "";
            string emailRaw = emailCell.Value?.ToString()?.Trim() ?? "";

            string numberUnformatted = Regex.Replace(numberRaw, @"[\s\-\(\)]", "");

            // Check if all required fields are empty
            if (string.IsNullOrEmpty(numberUnformatted) &&
                string.IsNullOrEmpty(nameRaw) &&
                string.IsNullOrEmpty(positionRawName) &&
                string.IsNullOrEmpty(emailRaw))
            {
                string errorMessage = "Input number, email or name to proceed.";
                if (currentColumn == "number") numberCell.ErrorText = errorMessage;
                if (currentColumn == "name") nameCell.ErrorText = errorMessage;
                if (currentColumn == "position") positionCell.ErrorText = errorMessage;
                if (currentColumn == "email") emailCell.ErrorText = errorMessage;
                return;
            }

            // Clear all errors
            numberCell.ErrorText = "";
            nameCell.ErrorText = "";
            positionCell.ErrorText = "";
            emailCell.ErrorText = "";

            // Number column validation
            if (currentColumn == "number")
            {
                if (!string.IsNullOrEmpty(numberUnformatted))
                {
                    if (IsValidMobileNumber(numberUnformatted))
                    {
                        numberCell.Style.ForeColor = Color.Black;
                        numberCell.Value = FormatMobileNumber(numberUnformatted);
                    }
                    else if (IsValidLandlineNumber(numberUnformatted))
                    {
                        numberCell.Style.ForeColor = Color.Black;
                        numberCell.Value = FormatLandlineNumber(numberUnformatted);
                    }
                    else
                    {
                        numberCell.Style.ForeColor = Color.Red;
                        numberCell.Value = numberUnformatted;
                        numberCell.ErrorText = "Invalid telephone number.";
                    }
                }
                else
                {
                    numberCell.Style.ForeColor = Color.Black;
                }
            }

            // Name column
            if (currentColumn == "name")
            {
                nameCell.Style.ForeColor = Color.Black;
            }

            // Position column
            if (currentColumn == "position")
            {
                var value = positionCell.Value;
            }

            // Email column validation
            if (currentColumn == "email")
            {
                if (!string.IsNullOrEmpty(emailRaw))
                {
                    if (IsValidEmail(emailRaw))
                    {
                        emailCell.Style.ForeColor = Color.Black;
                        emailCell.Value = emailRaw.ToLowerInvariant();
                        emailCell.ErrorText = "";
                    }
                    else
                    {
                        emailCell.Style.ForeColor = Color.Red;
                        emailCell.ErrorText = "Invalid email address.";
                    }
                }
                else
                {
                    emailCell.Style.ForeColor = Color.Black;
                    emailCell.ErrorText = "";
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email &&
                       email.Contains('@') &&
                       email.LastIndexOf('.') > email.IndexOf('@');
            }
            catch
            {
                return false;
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
            if (serviceSetup == null) return;

            CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();

            AddCmbDefaultVal(CacheData.PaymentTerms);

            var financeDt = CacheData.PaymentTerms.Copy();

            // finance tab
            BindCmbValues(cmb_finance_payment_terms, financeDt);
            // item tab
            BindCmbValues(cmb_payment_terms, CacheData.PaymentTerms);

        }

        // ACCOUNT is a GL account (4.1.7), so both ACCOUNT dropdowns list the Accounting chart
        // of accounts. They were bound to the payment terms, which is why they offered CASH,
        // GCASH, Net 30... Each combo gets its own copy: two combos on one table share a
        // position, and picking on one tab would move the other.
        private async Task GetAccounts()
        {
            DataTable accounts = await BpiAccountingSetupServices.GetAccounts();

            BindCmbValues(cmb_finance_account, accounts);
            BindCmbValues(cmb_item_account, accounts.Copy());
        }

        private async Task GetVatRate()
        {
            _vatRatePercent = await BpiAccountingSetupServices.GetVatRatePercent();
        }

        private void cmb_payment_terms_Click(object sender, EventArgs e)
        {

        }

        private void btn_items_payment_terms_Click(object sender, EventArgs e) =>
            OpenSetupModal("PAYMENT TERMS", ENUM_ENDPOINT.PAYMENT_TERMS, CacheData.PaymentTerms);

        private void GetPaymentItemTerms(bool isItem)
        {
            if (!isItem) return;

            // Filter items by this branch's ParentId instead of using SelectedRecord index
            var matchedRow = items.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("bpi_item_branch_id").ToString() == ParentId);

            if (matchedRow != null)
            {
                cmb_payment_terms.SelectedValue = matchedRow.Field<int>("payment_terms_id");
                // This also wrote the payment term's id into TAX CODE. The tax code is put
                // on screen by BindTaxCode.
            }
        }

        // Both tax code dropdowns - Finance and Items - offer the same list, headed by
        // "-- SELECT --" because a tax code is optional (4.1.7). VAT is preselected for a new
        // partner; stored codes replace it when an existing one is bound.
        private void GetTaxCode()
        {
            BindTaxCodeList(cmb_finance_tax_code);
            BindTaxCodeList(cmb_tax_code);

            cmb_finance_tax_code.SelectedIndexChanged += (s, e) => ApplyTaxRate(cmb_finance_tax_code, txt_finance_tax);
            cmb_tax_code.SelectedIndexChanged += (s, e) => ApplyTaxRate(cmb_tax_code, txt_item_tax_code);

            _bindingTaxCodes = true;
            try
            {
                SetTaxCode(cmb_finance_tax_code, txt_finance_tax, ENUM_TAX_CODE.VAT);
                SetTaxCode(cmb_tax_code, txt_item_tax_code, ENUM_TAX_CODE.VAT);
            }
            finally
            {
                _bindingTaxCodes = false;
            }
        }

        // Its own copy per combo, for the same shared-position reason as the accounts.
        private static void BindTaxCodeList(ComboBox cmb)
        {
            DataTable codes = ENUM_TAX_CODE.LIST();
            DataRow select = codes.NewRow();
            select["title"] = BpiAccountingSetupServices.SELECT_TEXT;
            select["value"] = string.Empty;
            codes.Rows.InsertAt(select, 0);

            cmb.DataSource = codes;
            cmb.DisplayMember = "title";
            cmb.ValueMember = "value";
        }

        // Shows a stored tax code. A code that is not on the list - a few older records hold
        // one - is added to this combo instead of being dropped, so opening and saving a
        // record never rewrites its code.
        private void SetTaxCode(ComboBox cmb, TextBox rate, string code)
        {
            code = (code ?? string.Empty).Trim();
            int index = 0;

            if (code.Length > 0 && cmb.DataSource is DataTable codes)
            {
                index = cmb.FindStringExact(code);
                if (index < 0)
                {
                    codes.Rows.Add(code, code);
                    index = cmb.FindStringExact(code);
                }
            }

            // Through -1 so the change always registers - BindControls may already have
            // pushed text into this combo - and ApplyTaxRate runs for the code shown.
            cmb.SelectedIndex = -1;
            cmb.SelectedIndex = Math.Max(index, 0);
            ApplyTaxRate(cmb, rate);
        }

        // The code as saved: "" for "-- SELECT --".
        private static string GetTaxCodeValue(ComboBox cmb) =>
            cmb.SelectedIndex > 0 ? cmb.GetItemText(cmb.SelectedItem) : string.Empty;

        // Tax code VAT takes its rate from Company Setup, and the rate cannot be typed over
        // (user instruction 2026-09-17). Any other code's rate is typed: 4.5.3 keeps rates out
        // of code, and Tax Setup does not hold these codes yet. Changing the code by hand
        // clears the previous rate, so a VAT 12 never stays behind as a NON-VAT 12.
        // Provisional standard for the non-VAT codes; may change with the client's wishes.
        private void ApplyTaxRate(ComboBox code, TextBox rate)
        {
            bool isVat = string.Equals(GetTaxCodeValue(code), ENUM_TAX_CODE.VAT, StringComparison.OrdinalIgnoreCase);

            if (isVat && _vatRatePercent.HasValue)
                rate.Text = _vatRatePercent.Value.ToString("0.##", CultureInfo.InvariantCulture);
            else if (!_bindingTaxCodes)
                rate.Text = string.Empty;

            ApplyTaxRateLock(code, rate);
        }

        // ACCOUNT BALANCE is the total of the quotations listed under PENDING (user
        // instruction 2026-09-17). It is computed, never typed: 4.1.7 leaves only ACCOUNT,
        // PAYMENT TERMS and TAX CODE editable.
        private void ShowAccountBalance(DataTable pending)
        {
            decimal total = 0;
            if (pending != null && pending.Columns.Contains("total_price"))
            {
                foreach (DataRow row in pending.Rows)
                {
                    if (!row.IsNull("total_price"))
                        total += Convert.ToDecimal(row["total_price"], CultureInfo.InvariantCulture);
                }
            }

            txt_account_balance.Text = total.ToString("N2", CultureInfo.GetCultureInfo("en-PH"));
        }

        private void btn_add_item_Click(object sender, EventArgs e)
        {
            // CurrentItemsTable guarantees every column written below exists. Writing short_desc /
            // status_* straight into the bound table threw once "add new item" had rebound it.
            DataTable currentTable = CurrentItemsTable();
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
            //if (!isUpdate) return;
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
                        // Multi-select (requested): ItemModal now returns every checked
                        // item, not just one. The first fills the row that was clicked
                        // (unchanged behavior); any further items get appended as new
                        // rows, using btn_add_item_Click's own column list so a picked
                        // item ends up shaped exactly like a manually-added one would.
                        List<Dictionary<string, dynamic>> results = modal.GetResult();
                        if (results == null || results.Count == 0) return;

                        // Written to the bound table's rows rather than to grid cells. TYPE and
                        // DESCRIPTION had no column in that table, so values put into those cells
                        // were never kept, and the extra picks wrote columns the table did not
                        // always have and threw part-way (see CurrentItemsTable).
                        DataTable table = CurrentItemsTable();

                        var clicked = dg_items.Rows[e.RowIndex].DataBoundItem as DataRowView;
                        if (clicked != null && clicked.Row.Table == table)
                        {
                            SetItemFields(clicked.Row, results[0]);
                            // The blank "new row" line: commit it so the pick becomes a real row.
                            if (clicked.IsNew)
                                clicked.EndEdit();
                        }
                        else
                        {
                            DataRow firstRow = table.NewRow();
                            SetItemFields(firstRow, results[0]);
                            table.Rows.Add(firstRow);
                        }

                        for (int i = 1; i < results.Count; i++)
                        {
                            DataRow newRow = table.NewRow();
                            SetItemFields(newRow, results[i]);
                            table.Rows.Add(newRow);
                        }

                        dataBindingItems.EndEdit();
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
            BtnToggle(true);

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
            if (cmb_finance_account.Items.Count > 0) cmb_finance_account.SelectedIndex = 0;
            if (cmb_item_account.Items.Count > 0) cmb_item_account.SelectedIndex = 0;

            _bindingTaxCodes = true;
            try
            {
                SetTaxCode(cmb_finance_tax_code, txt_finance_tax, ENUM_TAX_CODE.VAT);
                SetTaxCode(cmb_tax_code, txt_item_tax_code, ENUM_TAX_CODE.VAT);
            }
            finally
            {
                _bindingTaxCodes = false;
            }
            cmb_payment_terms.Text = "COD";
            cmb_finance_payment_terms.Text = "COD";

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

            ToggleCustomerAndSupplier(true);
            ShowAffiliatedAndNon(false);


        }
        private void BtnToggle(bool isEdit)
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
            foreach (DataRow row in dataSource.Rows)
            {
                // Per row. This used to be declared outside the loop and only ever set to
                // true, so every contact after the default one was sent as a default too.
                bool is_default_contact = false;

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
            foreach (DataRow row in allAddress.Rows)
            {
                // Per row: a new address listed after a deleted one inherited its
                // deleted flag and was saved as deleted.
                bool isDeleted = false;

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

            // The combo's text would send "-- SELECT --" as a tax code.
            FinanceData["finance_tax_code"] = GetTaxCodeValue(cmb_finance_tax_code);
            // Shown, never saved - it is the total of the PENDING quotations.
            FinanceData.Remove("account_balance");

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
                    file_path = ConvertFileToBase64(row["file_path"].ToString());
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

            // The grid is what the user sees now, so the grid is what gets saved - including
            // items picked or edited after a row was deleted. This used to read
            // fullItemsRecords whenever it was set, and only a delete sets it, as a snapshot
            // taken at that moment: every item picked or edited after any delete was left out
            // of the save, so its item_id never reached the database (user-reported
            // 2026-09-14). The one thing the snapshot holds that the grid may not is the rows
            // the user deleted, flagged item_is_deleted - those are carried across so the API
            // still marks them deleted.
            var allItems = dataItemSource;
            if (fullItemsRecords != null
                && fullItemsRecords.Columns.Contains("bpi_item_id")
                && fullItemsRecords.Columns.Contains("item_is_deleted")
                && allItems.Columns.Contains("bpi_item_id"))
            {
                var onScreen = new HashSet<string>(allItems.AsEnumerable()
                    .Select(r => r["bpi_item_id"]?.ToString() ?? "")
                    .Where(id => id.Length > 0 && id != "0"));

                foreach (DataRow snapshotRow in fullItemsRecords.Rows)
                {
                    string id = snapshotRow["bpi_item_id"]?.ToString() ?? "";
                    bool.TryParse(snapshotRow["item_is_deleted"]?.ToString(), out bool deleted);
                    if (!deleted || id.Length == 0 || id == "0" || onScreen.Contains(id))
                        continue;

                    allItems.ImportRow(snapshotRow);
                }
            }
            var items = Helpers.GetControlsValues(panel_item);
            List<BpiItems> listItem = new List<BpiItems>();

            string taxCode = GetTaxCodeValue(cmb_tax_code);
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


                //if (row.IsNull("item_id") || string.IsNullOrWhiteSpace(row["item_id"].ToString()) || row.IsNull("bpi_item_based_id") || string.IsNullOrWhiteSpace(row["bpi_item_based_id"].ToString()) || row.IsNull("bpi_item_id") || string.IsNullOrWhiteSpace(row["bpi_item_id"].ToString()))
                //{
                //    itemId = 0;
                //    basedItemId = 0;
                //    bpiItemId = 0;
                //    bpiItemBranchId = 0;

                //}
                //else
                //{

                itemId = string.IsNullOrEmpty(row["item_id"].ToString()) ? 0 : int.Parse(row["item_id"].ToString());
                basedItemId = string.IsNullOrEmpty(row["bpi_item_based_id"].ToString()) ? 0 : int.Parse(row["bpi_item_based_id"].ToString());
                bpiItemId = string.IsNullOrEmpty(row["bpi_item_id"].ToString()) ? 0 : int.Parse(row["bpi_item_id"].ToString());
                bpiItemBranchId = string.IsNullOrEmpty(row["bpi_item_branch_id"].ToString()) ? 0 : int.Parse(row["bpi_item_branch_id"].ToString());
                isDeleted = string.IsNullOrEmpty(row["item_is_deleted"].ToString()) ? false : bool.Parse(row["item_is_deleted"].ToString());
                //}
                //itemId = ;
                string notes = row["notes"].ToString();

                unitPriceValid = float.TryParse(row["price"].ToString(), out unitPrice);
                item = new BpiItems(bpiItemId, basedItemId, paymentTermsId, itemId, taxCode, itemTaxCode, unitPrice, notes, itemAccountId, isDeleted);

                listItem.Add(item);

            }

            return listItem;
        }
        private string ConvertFileToBase64(string imagePath)
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
        private static void BindCmbValues(ComboBox cmb, DataTable dt)
        {
            cmb.DataSource = dt;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }

        private static void BindCmbValues(ComboBox cmb, DataView dv)
        {
            cmb.DataSource = dv;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            if (cmb.Items.Count > 0)
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
        private void ShowOtherSalesNotice(bool isVisible)
        {
            // Find existing notice panel to avoid duplicates
            Panel existingNotice = this.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.Name == "pnl_other_sales_notice");

            if (existingNotice != null)
            {
                existingNotice.Visible = isVisible;
                return;
            }

            // Only create if needed
            if (!isVisible) return;

            // Look up the owner's name from Users if available
            // SalesId holds the owner NAME. This used to look the owner up by
            // employee_id, which after the switch to names never matched - every notice
            // would have read "another sales representative". Prefer the matching user
            // record; otherwise the stored name itself is the owner name.
            string ownerName = "another sales representative";
            var owner = Users?.FirstOrDefault(u => smpc_inventory_app.Model.SalesOwner.Same(u.full_name, SalesId));
            if (owner != null)
                ownerName = owner.full_name;
            else if (!string.IsNullOrWhiteSpace(SalesId))
                ownerName = SalesId;

            Panel noticePanel = new Panel
            {
                Name = "pnl_other_sales_notice",
                BackColor = Color.FromArgb(255, 243, 205), // soft yellow
                Dock = DockStyle.Top,
                Height = 50,
                Visible = true
            };

            Label lbl = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(133, 77, 14), // dark amber
                Padding = new Padding(10, 0, 0, 0),
                Text = $"⚠  This record belongs to {ownerName}. Details are restricted."
            };

            noticePanel.Controls.Add(lbl);
            this.Controls.Add(noticePanel);
            this.Controls.SetChildIndex(noticePanel, 0); // place it at the top
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
            HideSystemColumns((DataGridView)sender, "finance");
        }

        private void dg_accreditations_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "accreditation");
        }

        private void dg_items_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //HideSystemColumns((DataGridView)sender, "items");
        }

        private void dg_history_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            HideSystemColumns((DataGridView)sender, "history");
        }
        // The branch's stored owner (branch_sales_id).
        public string OwnerName => SalesId;

        // Whether this user may change this branch. A branch not yet saved is
        // the current user's own.
        public bool CanEditThis => !IsExisting || smpc_inventory_app.Model.BpiAccess.CanEdit(CacheData.CurrentUser, SalesId);

        public void SetReadOnly(bool isReadOnly)
        {
            // Edit mode never unlocks a branch this user may not edit: another
            // executive's branch stays read-only beside the user's own (4.1.10).
            isReadOnly = isReadOnly || !CanEditThis;

            Helpers.SetTabControlReadOnly(tabControl2, isReadOnly);
            // Set all DGVs to read-only
            List<DataGridView> dgvList = new List<DataGridView>
            {
                dg_contacts,
                dg_address,
                dg_items,
                dg_finance_pending,
                dg_accreditations,
                dg_history
            };

            foreach (var dgv in dgvList)
            {
                dgv.ReadOnly = isReadOnly;
                dgv.AllowUserToAddRows = !isReadOnly;
                dgv.AllowUserToDeleteRows = !isReadOnly;
            }

            // What the loops above must not unlock. PENDING is generated from quotations and
            // is view-only (4.1.7) - in edit mode it used to offer a blank row to type into.
            dg_finance_pending.ReadOnly = true;
            dg_finance_pending.AllowUserToAddRows = false;
            dg_finance_pending.AllowUserToDeleteRows = false;
            txt_account_balance.ReadOnly = true;
            // System-issued codes (4.1.3): Edit mode used to make them look typeable, and the
            // API throws away whatever is typed there.
            txt_customer_code.ReadOnly = true;
            txt_supplier_code.ReadOnly = true;
            txt_affiliated.ReadOnly = true;
            txt_non_affiliated.ReadOnly = true;

            _isReadOnly = isReadOnly;
            ApplyTaxRateLock(cmb_finance_tax_code, txt_finance_tax);
            ApplyTaxRateLock(cmb_tax_code, txt_item_tax_code);
        }

        // Read-only state of a rate box without touching its value.
        private void ApplyTaxRateLock(ComboBox code, TextBox rate)
        {
            bool isVat = string.Equals(GetTaxCodeValue(code), ENUM_TAX_CODE.VAT, StringComparison.OrdinalIgnoreCase);
            rate.ReadOnly = _isReadOnly || (isVat && _vatRatePercent.HasValue);
        }
    }
}
