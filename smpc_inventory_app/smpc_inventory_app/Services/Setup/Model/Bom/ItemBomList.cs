using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Bom
{
    class ItemBomList
    {
        public int item_id { get; set; }
        public string short_desc { get; set; }
        public string item_code { get; set; }
        public string general_name { get; set; }
        public string item_model { get; set; }
        public string uom_name { get; set; }
        public string size { get; set; }
    }


    class ItemBomDetails
    {

        public int id { get; set; }
        public int item_bom_id { get; set; }
        public int item_id { get; set; }
        public string size { get; set; }
        public int bom_qty { get; set; }
        public string uom_name { get; set; }
        public string item_code { get; set; }
        public string short_desc { get; set; }
        public int unit_price { get; set; }
        public float net_price { get; set; }


        public ItemBomDetails(int id, int itemBomId, int itemId, int bomQty, int unitPrice, float netPrice)
        {
            this.id = id;
            this.item_bom_id = itemBomId;
            this.item_id = itemId;
            this.unit_price = unitPrice;
            this.size = size;
            this.net_price = netPrice;
            this.bom_qty = bomQty;

        }



    }
}
