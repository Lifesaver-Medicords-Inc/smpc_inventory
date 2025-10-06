using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemPurchasingModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public int supplier_name_id { get; set; }
        public int payment_terms_id { get; set; }
        public float price { get; set; }
        public string supplier_name { get; set; }
        public string supplier_type_name { get; set; }
        public string payment_terms_name { get; set; }
        public string validity_period { get; set; }
    }
    class ItemPurchasing 
    {
        public List<ItemPurchasingModel> itempurchasing { get; set; }
    }
}
