using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class PurchasingListModel
    {
        public string order_detail_ids { get; set; }
        public string based_ids { get; set; }
        public string sales_order_nos { get; set; }
        public string purchaser { get; set; }
        public int item_id { get; set; }
        public string item_code{ get; set; }
        public string item_name { get; set; }
        public string item_brand { get; set; }
        public string item_description { get; set; }
        public string unit_prices { get; set; }
        public string qtys { get; set; }
        public string unit_of_measures { get; set; }
        public string total_qty { get; set; }
    }
    class PurchasingListSupplierModel
    {
        public int supplier_id { get; set; }
        public string supplier { get; set; }
        public string contact_no { get; set; }
    }
    class PurchasingListCanvassModel
    {
        public int id { get; set; } 
        public int supplier_id { get; set; }
        public string contact_no { get; set; }
        public double order_size { get; set; }
        public double supplier_stock{ get; set; }
        public decimal current_list_price { get; set; }
        public decimal new_list_price { get; set; }
        public decimal discount { get; set; }
        public decimal net_price { get; set; }
        public string price_validity { get; set; }
        public int payment_terms { get; set; }
        public string lead_time{ get; set; }
    }
    class PurchasingList
    {
        public List<PurchasingListModel> purchasinglist { get; set; }
    }
    class PurchasingListSupplier
    {
        public List<PurchasingListSupplierModel> purchasinglistsupplier { get; set; }
    }
}
