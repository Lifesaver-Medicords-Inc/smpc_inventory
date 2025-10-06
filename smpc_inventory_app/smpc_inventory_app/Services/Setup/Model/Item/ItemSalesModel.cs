using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemSalesModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public string sales_order_no { get; set; }
        public string date { get; set; }
        public string item_code { get; set; }
        public string customer_name { get; set; }
    }
}
