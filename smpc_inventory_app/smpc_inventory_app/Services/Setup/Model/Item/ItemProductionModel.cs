using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemProductionModel
    {
        public int item_id { get; set; }
        public int bom_id { get; set; }
        public int bom_item_id { get; set; }
        public string item_code { get; set; }
        public string item_model { get; set; }
        public int bom_qty { get; set; }
    }
}
