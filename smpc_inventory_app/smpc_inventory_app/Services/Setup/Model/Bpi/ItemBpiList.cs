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
        public string short_desc { get; set; }
        public string item_code { get; set; }
        public string general_name { get; set; }
        public string item_model_name { get; set; }
        public string item_brand_name { get; set; }
        public string status_trade { get; set; }
        public string status_tangible { get; set; }

    }
}
