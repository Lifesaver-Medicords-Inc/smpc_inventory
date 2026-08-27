using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    // Matches inventory_models.ItemStockListView on the Go side - one row per
    // item+warehouse+bin from tbl_inv_item_stocks, with item/warehouse names already
    // resolved so this module doesn't have to re-look them up client-side.
    public class ItemStockModel
    {
        public int id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_model { get; set; }
        public string brand { get; set; }
        public int warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public string bin_location { get; set; }
        public int stock_qty { get; set; }
        public string stock_uom { get; set; }
        public bool is_active { get; set; }
    }
}
