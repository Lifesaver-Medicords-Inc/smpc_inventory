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
            tabContainer.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabContainer.SizeMode = TabSizeMode.Fixed;
            tabContainer.ItemSize = new Size(180, 20);

            tabContainer.SelectedIndexChanged += (s, e) => RecalculateContentWidth();
            // Phase 4.6 (UI uniformity): set the initial capped/centered width before
            // the form is ever shown - the Resize event alone would leave tabContainer
            // at its Designer-time placeholder size for one frame on startup.
            RecalculateContentWidth();
        }

        // Phase 4.6 (UI uniformity): the main content area (tabContainer - everything
        // left of the sidebar and right of RedBox's panel1) caps at 1280px and stays
        // centered on wide/ultrawide monitors. RedBox's own panel (panel1) is left
        // uncapped/full-width on purpose - it's persistent utility chrome, not the
        // "page" being viewed.
        //
        // Individual pages hardcode their own size in their own code and are never
        // resized to fit whatever tabContainer happens to be (same as
        // smpc_sales_system's Quotation.cs - see that app's Layout.cs for the full
        // history of what was tried and why this shape won). tabContainer never
        // shrinks narrower than the ACTIVE tab's own page needs; container's own
        // AutoScroll (Designer) scrolls the whole work area - tab strip included -
        // into view when it doesn't fit, rather than the page clipping inside a
        // too-small TabPage.
        private const int MaxContentWidth = 1280;

        private void container_Resize(object sender, EventArgs e)
        {
            RecalculateContentWidth();
        }

        private Control GetActiveTabPageControl()
        {
            if (tabContainer == null) return null;
            TabPage selected = tabContainer.SelectedTab;
            return selected != null && selected.Controls.Count > 0 ? selected.Controls[0] : null;
        }

        // Live crash found in smpc_sales_system: NullReferenceException on
        // tabContainer.SelectedTab. container's Resize event can fire mid-
        // InitializeComponent() - e.g. the moment it's docked into its own parent -
        // which is *before* every field this method touches is necessarily assigned
        // yet, regardless of how early each one's own "new" line appears in the
        // Designer file. Guard against both being null rather than relying on Designer
        // code-generation order to save us.
        private void RecalculateContentWidth()
        {
            if (container == null || tabContainer == null) return;

            int availableWidth = container.ClientSize.Width;
            int cappedWidth = Math.Min(MaxContentWidth, availableWidth);

            Control activePage = GetActiveTabPageControl();
            int neededWidth = activePage != null ? Math.Max(cappedWidth, activePage.Width) : cappedWidth;

            tabContainer.Width = neededWidth;
            tabContainer.Height = container.ClientSize.Height;
            // Centers only when everything actually fits (neededWidth == cappedWidth);
            // once the active page needs more room than's available, flush-left is the
            // only position that makes sense for something you're about to scroll to
            // see the rest of.
            tabContainer.Left = neededWidth <= availableWidth ? (availableWidth - neededWidth) / 2 : 0;
            tabContainer.Top = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void ShowForm(string tabTitle, Control control)
        {
            try
            {
                tabCount++;

                TabPage newTab = new TabPage(tabTitle);

                //control.Width = this.Width - 235;
                container.Height = this.Height * 2;
                //control.Height = this.Height;
                // Phase 4.6 (UI uniformity): was "control.Width = this.Width - 570" (a
                // magic-number approximation of the available content width) - removed
                // entirely. The page keeps its own Designer-authored/hardcoded size;
                // container's own AutoScroll (Designer) and RecalculateContentWidth
                // (above) handle showing all of it, scrolled if needed, instead of
                // clipping it to a forced width.
                newTab.Controls.Add(control);
                tabContainer.TabPages.Add(newTab);
                tabContainer.SelectTab(newTab);
                // SelectTab above should already raise SelectedIndexChanged and trigger
                // this, but calling it directly here too is cheap and removes any doubt
                // that a freshly-added tab's own width need is accounted for immediately.
                RecalculateContentWidth();

                if (control is smpc_inventory_app.Pages.Purchasing.NewPurchasingList purchasingListControl)
                {
                    purchasingListControl.TriggerNewForm += ShowForm;
                }
            }
            catch (Exception )
            {
                throw;
            }




        }

        private void Sidebar_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            try
            {
                if (e.Node.Name.Contains("DASHBOARD") || e.Node.Name.Contains("PURCHASE REQUISITION"))
                {
                    Helpers.ShowDialogMessage("error", "This module is not available at the moment!");
                    return;
                }

                if (!e.Node.Name.Contains("parent"))
                {
                    RouteServices route = new RouteServices(e.Node.Name);
                    ShowForm(route.GetTitle(), route.GetForm());
                }
            }
            catch (Exception)
            {

                throw;
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
                ShowForm(route.GetTitle(), route.GetForm());
            }
        }

        void SMPC_Load(object sender, EventArgs e)
        {
            Login login = new smpc_inventory_app.Pages.Login();
            if (DialogResult.OK == login.ShowDialog())
            {
                lbl_name.Text = CacheData.CurrentUser.first_name + " " + CacheData.CurrentUser.last_name;
                lbl_position.Text = CacheData.CurrentUser.position.name;
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
            WebSocketServices.OnConnected -= OnConnectedHandler;
            WebSocketServices.OnError -= OnErrorHandler;
            WebSocketServices.OnDisconnected -= OnDisconnectedHandler;

            WebSocketServices.OnConnected += OnConnectedHandler;
            WebSocketServices.OnError += OnErrorHandler;
            WebSocketServices.OnDisconnected += OnDisconnectedHandler;

            await WebSocketServices.ConnectAndDeserialize<RedboxPurchasingList>(
                ENUM_ENDPOINT.WSPURCHASINGREDBOXLIST,
                (data) => Invoke((Action)(() => LoadOrders(data)))
            );
        }

        private void OnConnectedHandler() =>
            Invoke((Action)(() => lbl_status.Text = "Connected"));

        private void OnErrorHandler(string msg) =>
            Invoke((Action)(() => { lbl_status.Text = "Error: " + msg; }));

        private void OnDisconnectedHandler() =>
            Invoke((Action)(() => lbl_status.Text = "Disconnected"));

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

                        Control existingCard = FindCardById(id);
                        if (orderType == "SO")
                        {
                            if (existingCard != null)
                            {
                                if (existingCard is SalesOrderCard)
                                {
                                    var card = (SalesOrderCard)existingCard;
                                    card.LoadCardDetails(orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                }
                            }
                            else
                            {
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
                                if (existingCard is PurchaseRequisitionCard)
                                {
                                    var card = (PurchaseRequisitionCard)existingCard;
                                    card.UpdateDetails(orderNo, projectName, commitmentDate, purchaser, numberOfItems, itemNames, customer);
                                }
                            }
                            else
                            {

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
                    RecalculateContentWidth();
                    break;
                }
            }
        }

        private void tabContainer_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = tabContainer.TabPages[e.Index];
            var tabRect = tabContainer.GetTabRect(e.Index);
            bool isSelected = (e.Index == tabContainer.SelectedIndex);

            // Draw the tab title
            string title = tabPage.Text;
            Font font = isSelected ? new Font(e.Font, FontStyle.Bold) : e.Font;
            using (Brush textBrush = new SolidBrush(tabPage.ForeColor))
            {
                e.Graphics.DrawString(title, font, textBrush, tabRect.X + 2, tabRect.Y + 4);
            }

            // Define close button size and position
            int closeButtonSize = 16;
            Rectangle closeButton = new Rectangle(
                tabRect.Right - closeButtonSize - 5,
                tabRect.Top + (tabRect.Height - 16) / 2,
                closeButtonSize,
                closeButtonSize
            );

            using (Font closeFont = new Font("Arial", 9, FontStyle.Bold))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString("x", closeFont, Brushes.Black, closeButton, sf);
            }
        }

        private void tabContainer_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabContainer.TabPages.Count; i++)
            {
                Rectangle tabRect = tabContainer.GetTabRect(i);
                int closeButtonSize = 16;
                Rectangle closeButton = new Rectangle(
                    tabRect.Right - closeButtonSize - 5,  
                    tabRect.Top + (tabRect.Height - 16) / 2,
                    closeButtonSize,
                    closeButtonSize
                );

                bool isSelected = (i == tabContainer.SelectedIndex);
                if (isSelected && closeButton.Contains(e.Location))
                {
                    TabPage tabToRemove = tabContainer.TabPages[i];
                    tabContainer.TabPages.Remove(tabToRemove);
                    RecalculateContentWidth();
                    break;
                }
            }
            return;
        }
    }
}
