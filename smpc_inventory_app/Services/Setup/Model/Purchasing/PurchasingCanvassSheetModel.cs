using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class PurchasingCanvassSheetModel
    {
        public int id { get; set; }
        public int supplier_id  { get; set; }
        public string supplier_name { get; set; }
        public string contact_no { get; set; }
        public int order_size { get; set; }
        public int supplier_stock { get; set; }
        public int reserved_stock { get; set; }
        public decimal previous_list_price { get; set; }
        public decimal current_list_price { get; set; }
        public decimal new_list_price { get; set; }
        public string discount { get; set; }
        public decimal net_price { get; set; }
        public string price_trend { get; set; }
        public string price_validity { get; set; }
        public int payment_terms { get; set; }
        public string lead_time { get; set; }
        public int item_id { get; set; }
        public string start_date { get; set; }
        
    }
    
}
