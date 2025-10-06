using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing
{
    public partial class SalesOrderCard : UserControl
    {
        private ToolTip toolTip;

        public int Id { get; private set; }
        public string OrderNo { get; private set; }
        public string CommitmentDate { get; private set; }
        public string Purchaser { get; private set; }
        public int NumberOfItems { get; private set; }
        public string ProjectName { get; private set; }
        public string ItemNames { get; private set; }
        public string Customer { get; private set; }




        public SalesOrderCard(int id, string orderNo, string projectName, string commitmentDate, string purchaser, int numberOfItems, string itemNames, string customer)
        {
            InitializeComponent();

            Id = id;
            OrderNo = orderNo;
            ProjectName = projectName;
            CommitmentDate = commitmentDate;
            NumberOfItems = numberOfItems;
            Purchaser = purchaser;
            ItemNames = itemNames;
            Customer = customer;

            toolTip = new ToolTip();
        }

        public void LoadCardDetails(string orderNo, string projectName, string commitmentDate, string purchaser, int numberOfItems, string itemNames, string customer)
        {
            // Update the properties of the card
            OrderNo = orderNo;
            ProjectName = projectName;
            CommitmentDate = commitmentDate;
            Purchaser = purchaser;
            NumberOfItems = numberOfItems;
            ItemNames = itemNames;
            Customer = customer;

            // Update the labels/textboxes or other controls
            lbl_document_no.Text = "SO#" + OrderNo;
            lbl_project_name.Text = TruncateText(ProjectName, 10);
            lbl_commitment_date.Text = CommitmentDate;
            lbl_items_to_order.Text = NumberOfItems.ToString();
            lbl_client_name.Text = TruncateText(Customer, 10);
            lbl_assigned_purchaser.Text = TruncateText(Purchaser, 10);

            // Update the checkbox and label colors based on purchaser
            checkBox1.CheckedChanged -= checkBox1_CheckedChanged;

            if (Purchaser == CacheData.CurrentUser.employee_id)
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = true;
                lbl_assigned_purchaser.ForeColor = Color.Green;
            }
            else if (!string.IsNullOrEmpty(Purchaser) && Purchaser != "~")
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
                lbl_assigned_purchaser.ForeColor = Color.Gray;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox1.Enabled = true;
                lbl_assigned_purchaser.ForeColor = Color.Black;
            }

            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }
        private void SalesOrderCard_Load(object sender, EventArgs e)
        {
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 200;
        }
        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }

        private  void lbl_items_to_order_MouseHover(object sender, EventArgs e)
        {
            string[] itemArray = ItemNames.Split(',');
            string itemList = string.Join("\n", itemArray);

            toolTip.SetToolTip(lbl_items_to_order, itemList);
        }

        private async Task checkBox1_CheckedChangedAsync()
        {
            checkBox1.Enabled = false;
            var newPurchaser = checkBox1.Checked ? CacheData.CurrentUser.employee_id : "~";
            var message = checkBox1.Checked ? "Sales Order has been reserved" : "Sales Order reservation has been removed";

            var data = new Dictionary<string, dynamic>
                {
                    { "order_id", Id },
                    { "doc", OrderNo },
                    { "purchaser", newPurchaser }
                };

            try
            {
                ApiResponseModel response = await PurchasingRedboxListServices.UpdateSalesOrder(data);

                if (response != null && response.Success)
                {
                    Purchaser = newPurchaser;
                    LoadCardDetails(OrderNo, ProjectName, CommitmentDate, Purchaser, NumberOfItems, ItemNames, Customer);

                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Reservation update failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            checkBox1.Enabled = true;
        }

        // Event handler method
        private async void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            await checkBox1_CheckedChangedAsync();
        }
        private void btn_view_details_Click(object sender, EventArgs e)
        {
            string[] orderNos = OrderNo.Split(',');
            string[] itemNames = ItemNames.Split(',');
            
            DataTable dt = new DataTable();
            dt.Columns.Add("order_no", typeof(string));
            
            dt.Columns.Add("item_names", typeof(string));

            int maxLength = Math.Max(orderNos.Length, itemNames.Length);


            for (int i = 0; i < maxLength; i++)
            {
                string orderNo = !string.IsNullOrWhiteSpace(OrderNo) ? "SO#" + OrderNo.Split(',')[0] : "N/A";
                string itemName = i < itemNames.Length ? itemNames[i] : "N/A";

                dt.Rows.Add(orderNo, itemName);
            }
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "order_no", "DOC NO" },
                { "item_names", "ITEM NAME" }
            };
            ViewDetailsModal modal = new ViewDetailsModal(dt);
            modal.SetData(dt, headers);
            modal.ShowDialog();
        }
    }

}