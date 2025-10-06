using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.WebSockets;
using Login = smpc_inventory_app.Pages.Login;

namespace Inventory_SMPC.Pages
{
    public partial class SMPC : Form
    {
        private WebSocket ws;
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

            control.Dock = DockStyle.Fill;


            if (control is smpc_inventory_app.Pages.Purchasing.NewPurchasingList)
            {
                smpc_inventory_app.Pages.Purchasing.NewPurchasingList purchasingListControl = (smpc_inventory_app.Pages.Purchasing.NewPurchasingList)control;
                purchasingListControl.TriggerNewForm += showForm;

            }

            //control.Width = this.Width - 235; 
            tabContainer.Height = this.Height * 2;
            //control.Height = this.Height;

            control.Width = this.Width - 600;

            newTab.Controls.Add(control);
            newTab.AutoScroll = true;

            Console.WriteLine("Parent of control: " + newTab.Parent + ", control: " + control);

            tabContainer.TabPages.Add(newTab);
            tabContainer.SelectTab(newTab);

        }
        private void removeTab(object sender, EventArgs e)
        {
            int currentIndex = tabContainer.SelectedIndex;

            // Remove the current selected tab
            if (currentIndex >= 0)
            {
                tabContainer.TabPages.RemoveAt(currentIndex);

                // Navigate to the previous tab if it exists, otherwise to the first tab if any
                if (tabContainer.TabCount > 0)
                {
                    int newIndex = currentIndex - 1;
                    if (newIndex < 0) newIndex = 0;
                    tabContainer.SelectedIndex = newIndex;
                }
            }
        }

        private void Sidebar_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name.Contains("DASHBOARD") || e.Node.Name.Contains("PURCHASE RETURN") || e.Node.Name.Contains("PURCHASE REQUISITION"))
            {
                Helpers.ShowDialogMessage("error", "This module is not available at the moment!");
                return;
            }
            if (!e.Node.Name.Contains("parent"))
            {
                RouteServices route = new RouteServices(e.Node.Name);
                showForm(route.GetTitle(), route.GetForm());
            }
            else //pwedeng if (e.Node.Text.toLower().Contains("parent")) //to filter parents nodes
            {
                if (e.Node.Nodes.Count > 0)
                {
                    if (e.Node.IsExpanded)
                    {
                        e.Node.Collapse();
                    }
                    else
                    {
                        e.Node.Expand();
                    }
                }
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

        void SMPC_Load(object sender, EventArgs e)
        {
            Login login = new smpc_inventory_app.Pages.Login();
            if (DialogResult.OK == login.ShowDialog())
            {
                lbl_name.Text = CacheData.CurrentUser.first_name + " " + CacheData.CurrentUser.last_name;
                lbl_position.Text = CacheData.CurrentUser.position_id;
                lbl_department.Text = CacheData.CurrentUser.department;
                this.Enabled = true;


                ConnectWebSocket();
            }
            else
            {
                Application.Exit();
            }
        }
        private async void ConnectWebSocket()
        {

            WebSocketServices.OnConnected += () =>
            {
                Invoke((Action)(() => lbl_status.Text = "Connected"));
            };

            WebSocketServices.OnError += (msg) =>
            {
                Invoke((Action)(() => MessageBox.Show("WebSocket Error: " + msg)));
            };

            WebSocketServices.OnDisconnected += () =>
            {
                Invoke((Action)(() => lbl_status.Text = "Disconnected"));
            };

            await WebSocketServices.ConnectAndDeserialize<RedboxPurchasingList>(
                ENUM_ENDPOINT.WSPURCHASINGREDBOXLIST,
                (data) => Invoke((Action)(() => LoadOrders(data)))
            );
        }

        private void LoadOrders(RedboxPurchasingList response)
        {
            try
            {
                records = response;
                purchasinglist = JsonHelper.ToDataTable(records.purchaselist);

                flowPanelRedBox.Invoke(new Action(() =>
                {
                    flowPanelRedBox.SuspendLayout();

                    var validIds = new HashSet<int>(purchasinglist.AsEnumerable().Select(row => Convert.ToInt32(row["id"])));

                    var cardsToRemove = flowPanelRedBox.Controls.Cast<Control>()
                        .Where(c => c.Tag is int id && !validIds.Contains(id))
                        .ToList();

                    foreach (var card in cardsToRemove)
                    {
                        flowPanelRedBox.Controls.Remove(card);
                        card.Dispose();
                    }
                    // Loop through the incoming data
                    foreach (DataRow row in purchasinglist.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string orderNo = row["doc_no"].ToString();
                        string projectName = row["project_name"].ToString();
                        string commitmentDate = row["commitment_date"].ToString();
                        string purchaser = row["purchaser"].ToString();
                        string itemNames = row["item_names"].ToString();
                        int numberOfItems = itemNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        string customer = row["customer"].ToString();
                        string orderType = row["order_type"].ToString();


                        // Try to find the existing card by ID
                        Control existingCard = FindCardById(id);
                        if (orderType == "SO")
                        {
                            if (existingCard != null)
                            {
                                // Update the existing card
                                if (existingCard is SalesOrderCard)
                                {
                                    var card = (SalesOrderCard)existingCard;
                                    card.LoadCardDetails(orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                }
                            }
                            else
                            {
                                // If no card found, create a new one and add it
                                Control card = new SalesOrderCard(id, orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                card.Size = new Size(flowPanelRedBox.ClientSize.Width - flowPanelRedBox.Padding.Horizontal, 150);
                                card.Margin = new Padding(0, 0, 0, 10);
                                card.Tag = id;

                                flowPanelRedBox.Controls.Add(card);
                            }
                        }
                        else
                        {
                            if (existingCard != null)
                            {
                                // Update the existing card
                                if (existingCard is PurchaseRequisitionCard)
                                {
                                    var card = (PurchaseRequisitionCard)existingCard;
                                    card.UpdateDetails(orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                }
                            }
                            else
                            {
                                // If no card found, create a new one and add it
                                Control card = new PurchaseRequisitionCard(id, orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                card.Size = new Size(flowPanelRedBox.ClientSize.Width - flowPanelRedBox.Padding.Horizontal, 150);
                                card.Margin = new Padding(0, 0, 0, 10);
                                card.Tag = id;

                                flowPanelRedBox.Controls.Add(card);
                            }
                        }
                    }

                    flowPanelRedBox.ResumeLayout();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SMPC_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (ws != null && ws.)
            //{
            //    ws.Close();
            //}
        }
        private Control FindCardById(int id)
        {
            foreach (Control ctrl in flowPanelRedBox.Controls)
            {
                if (ctrl.Tag is int existingId && existingId == id)
                {
                    return ctrl;
                }
            }
            return null;
        }
        public void RemoveTabContaining(Control control)
        {
            foreach (TabPage tab in tabContainer.TabPages)
            {
                if (tab.Controls.Contains(control))
                {
                    tabContainer.TabPages.Remove(tab);
                    break;
                }
            }
        }

        private void tabContainer_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabBounds = tabControl.GetTabRect(e.Index);

            // Use ellipsis trimming
            StringFormat stringFlags = new StringFormat();
            stringFlags.Trimming = StringTrimming.EllipsisCharacter;
            stringFlags.FormatFlags = StringFormatFlags.NoWrap;

            // Set alignment
            stringFlags.Alignment = StringAlignment.Near;
            stringFlags.LineAlignment = StringAlignment.Center;

            // Draw background
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, tabBounds);
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, tabBounds);
            }

            // Draw the text with ellipsis
            using (Brush textBrush = new SolidBrush(tabPage.ForeColor))
            {
                e.Graphics.DrawString(tabPage.Text, e.Font, textBrush, tabBounds, stringFlags);
            }
        }
    }
}
