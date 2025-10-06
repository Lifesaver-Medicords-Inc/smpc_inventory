 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class GuidingPriceModel
    {
        public int item_id { get; set; }
        public decimal last_price { get; set; }
        public string last_supplier_name { get; set; }
        public decimal second_last_price { get; set; }
        public string second_last_supplier_name { get; set; }
        public decimal third_last_price { get; set; }
        public string third_last_supplier_name { get; set; }
        public decimal lowest_1yr { get; set; }
        public string lowest_1yr_supplier_name { get; set; }
        public decimal lowest_3yr { get; set; }
        public string lowest_3yr_supplier_name { get; set; }
        public decimal lowest_alltime { get; set; }
        public string lowest_alltime_supplier_name { get; set; }
    }
}
