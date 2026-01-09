using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemInventoryModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public int warehouse_id { get; set; }
        public string default_zone { get; set; }
        public string storage_type { get; set; }
        public string default_bin_location { get; set; }
        public int valuation_method_id{ get; set; }
        public int minimum_inventory { get; set; }
        public int maximum_inventory { get; set; }
    }
    class ItemAvailableInvModel
    {
        public int item_id { get; set; }
        public int warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public string location { get; set; }
        public double stock_qty { get; set; }
        public string stock_uom { get; set; }
    }
}
