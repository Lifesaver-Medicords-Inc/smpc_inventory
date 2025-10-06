using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
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
    public partial class NewPurchasingList : UserControl
    {
        public NewPurchasingList()
        {
            InitializeComponent();

        }

        private void NewPurchasingList_Load(object sender, EventArgs e)
        {
            GetPurchasingList();
        }
        private async void GetPurchasingList()
        {
            var records = await PurchasingListServices.GetAsDataTable();
            string user = CacheData.CurrentUser.employee_id;

            DataView dv = new DataView(records);
            dv.RowFilter = $"Purchaser = '{user}'";

            flowLayoutPanel1.Controls.Clear();

            flowLayoutPanel1.Invoke(new Action(() =>
            {
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.Padding = new Padding(5);

                foreach (DataRowView row in dv)
                {
                    string orderId = row["order_detail_ids"].ToString();
                    string basedId = row["based_ids"].ToString();
                    string orderNo = row["sales_order_nos"].ToString();
                    string purchaser = row["purchaser"].ToString();
                    string itemId = row["item_id"].ToString();
                    string itemCode = row["item_code"].ToString();
                    string itemName = row["item_name"].ToString();
                    string itemBrand= row["item_brand"].ToString();
                    string itemDescription = row["item_description"].ToString();
                    string unitPrice = row["unit_prices"].ToString();
                    string qty = row["qtys"].ToString();
                    string unitOfMeasures = row["unit_of_measures"].ToString();
                    string totalQty = row["total_qty"].ToString();
                    
                  
                    Control card = new PurchaseItemCard(orderId, basedId, orderNo, purchaser, itemId, itemCode, itemName, itemBrand, itemDescription, unitPrice, qty, unitOfMeasures, totalQty);
                    card.Size = new Size(1120, 130);
                    flowLayoutPanel1.Controls.Add(card);
                }
            }));
        }
    }
}
