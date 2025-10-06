using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages;
using smpc_inventory_app.Pages.Purchasing;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing;
using smpc_sales_app.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_SMPC.Pages
{
    public partial class SMPC : Form
    {
        RedboxPurchasingList records;
        DataTable purchasinglist;
        private static SMPC _instance;

            public static SMPC Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new SMPC();
                    }
                    return _instance;
                }
            }


            private int tabCount = 0;
        public SMPC()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void showForm(string tabTitle, Control control)
        {
            tabCount++;
            Button closeButton = new Button();
            closeButton.Text = "X";
            closeButton.Size = new Size(20, 20);
            closeButton.Click += removeTab;
            closeButton.ForeColor = Color.Red;

            TabPage newTab = new TabPage(tabTitle);
            newTab.Controls.Add(closeButton);
            closeButton.Location = new Point(newTab.Width, 10); // Adjust position as needed

            //control.Width = this.Width - 235; 
            tabContainer.Height = this.Height * 2;
            //control.Height = this.Height;
            control.Width = this.Width - 550;
            newTab.Controls.Add(control);
            newTab.AutoScroll = true;
            tabContainer.TabPages.Add(newTab);
            tabContainer.SelectTab(newTab);
        }
        private void removeTab(object sender, EventArgs e)
        {
            tabContainer.TabPages.Remove(tabContainer.SelectedTab);
            //tabControl1.SelectTab();
        }

        private void Sidebar_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // || e.Node.Name.Contains("Sales Order") || e.Node.Name.Contains("Ship Type Setup") e.Node.Name.Contains("PURCHASE ORDER") ||
            if (e.Node.Name.Contains("DASHBOARD") ||  e.Node.Name.Contains("PURCHASE RETURN"))
            {
                Helpers.ShowDialogMessage("error", "This module is not available at the moment!");
                return;
            }
            if (!e.Node.Name.Contains("parent"))
            {
                RouteServices route = new RouteServices(e.Node.Name);
                showForm(route.GetTitle(), route.GetForm());
            }
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!e.Node.Name.Contains("parent"))
            {
                RouteServices route = new RouteServices(e.Node.Name);
                showForm(route.GetTitle(), route.GetForm());
            }
        }

        private void SMPC_Load(object sender, EventArgs e)
        {
            Login login = new Login();
            if (DialogResult.OK == login.ShowDialog())
            {
                lbl_name.Text = CacheData.CurrentUser.first_name + " " + CacheData.CurrentUser.last_name;
                lbl_position.Text = CacheData.CurrentUser.position;
                lbl_department.Text = CacheData.CurrentUser.department;
                ///LoadSalesOrders();
                this.Enabled = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private async void LoadSalesOrders()
        {
            try
            {
                var response = await RequestToApi<ApiResponseModel<RedboxPurchasingList>>.Get(ENUM_ENDPOINT.PURCHASINGREDBOXPURCHASELIST);

                records = response.Data;
                purchasinglist = JsonHelper.ToDataTable(records.purchaselist);

               
                flowPanelRedBox.Invoke(new Action(() =>
                {
                    flowPanelRedBox.Controls.Clear();
                    flowPanelRedBox.AutoScroll = true;
                    flowPanelRedBox.WrapContents = false;
                    flowPanelRedBox.Padding = new Padding(5);

                    foreach (DataRow row in purchasinglist.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string orderNo = row["doc_no"].ToString();
                        string projectName = row["project_name"].ToString();
                        string commitmentDate = row["commitment_date"].ToString();
                        string purchaser = row["purchaser"].ToString();
                        string itemNames = row["item_names"].ToString();
                        int numberOfItems = row["item_names"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        string customer = row["customer"].ToString();
                        string orderType = row["order_type"].ToString();

                        // Create the appropriate card based on order type
                        Control card;
                        if (orderType == "SO")
                        {

                            card = new SalesOrderCard(id, orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer)
                            {
                                Size = new Size(flowPanelRedBox.ClientSize.Width - flowPanelRedBox.Padding.Horizontal, 150),
                                Margin = new Padding(0, 0, 0, 10)
                            };
                        }
                        else
                        {
                            card = new PurchaseRequisitionCard(id, orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer)
                            {
                                Size = new Size(flowPanelRedBox.ClientSize.Width - flowPanelRedBox.Padding.Horizontal, 150),
                                Margin = new Padding(0, 0, 0, 10)
                            };
                        }
                        
                        flowPanelRedBox.Controls.Add(card);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
