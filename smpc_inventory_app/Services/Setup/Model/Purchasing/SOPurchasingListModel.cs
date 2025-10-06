using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    public class SOPurchasingListModel
    {
        public int item_id { get; set; }
        public string purchaser { get; set; }
        public string order_detail_ids { get; set; }
        public string order_ids { get; set; }
        public string sales_order_nos { get; set; }
        public string project_names { get; set; }
        public string sales_executives { get; set; }
        public string unit_prices { get; set; }
        public string discounts { get; set; }
        public string quote_supplier { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string unit_of_measure { get; set; }
        public string item_name { get; set; }
        public string item_brand { get; set; }
        public string commitment_dates { get; set; }
        public string qtys { get; set; }
        public int total_qty { get; set; }
    }

    class PurchasingListSupplierModel
    {
        public int supplier_id { get; set; }
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string address { get; set; }
        public string tin_no { get; set; }
        public string fax_no { get; set; }
        public string main_tel_no { get; set; }
        public string tax_code { get; set; }
        public int payment_terms_id { get; set; }
        public string item_ids { get; set; }
        public string order_detail_id { get; set; }
        public string qty { get; set; }
    }
    class PurchasingListGuidingPriceModel
    {
         public int item_id { get; set; }
         public string last_price { get; set; }
         public string last_supplier_id { get; set; }
         public string last_supplier_name { get; set; }
         public string second_last_price { get; set; }
         public string second_last_supplier_id { get; set; }
         public string second_last_supplier_name { get; set; }
         public string third_last_price { get; set; }
         public string third_last_supplier_id { get; set; }
         public string third_last_supplier_name { get; set; }
         public string lowest_1yr { get; set; }
         public string lowest_3yr { get; set; }
         public string lowest_alltime { get; set; }
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
        public List<SOPurchasingListModel> purchasinglist { get; set; }
    }
    class PurchasingListSupplier
    {
        public List<PurchasingListSupplierModel> purchasinglistsupplier { get; set; }
    }
}
