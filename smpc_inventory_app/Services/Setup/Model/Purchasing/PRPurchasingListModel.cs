using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    public class PRPurchasingListModel
    {
        public int item_id { get; set; }
        public string purchaser { get; set; }
        public string purchase_requisition_detail_ids { get; set; }
        public string purchase_requisition_ids { get; set; }
        public string purchase_requisition_nos { get; set; }
        public string requestors { get; set; }
        public string departments { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string unit_of_measure { get; set; }
        public string item_name { get; set; }
        public string item_brand { get; set; }
        public string commitment_dates { get; set; }
        public string qtys { get; set; }
        public string order_qty { get; set; }
        public string qty { get; set; }
        public int total_qty { get; set; }
        public bool order_is_checked { get; set; }
        public string lowest_selling_price { get; set; }
        public string last_purchase_price { get; set; }
        public string status { get; set; }
        public string order_type { get; set; }

    }
}
