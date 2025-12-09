using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Bpi
{
    class ItemBpiList
    {
        public int id { get; set; }
        public string item_type { get; set; }
        public string item_code { get; set; }
        public string general_name { get; set; }
        public string item_model_name { get; set; }
        public string item_brand_name { get; set; }
        public string long_description { get; set; }
        public float item_price { get; set; }

    }
}
