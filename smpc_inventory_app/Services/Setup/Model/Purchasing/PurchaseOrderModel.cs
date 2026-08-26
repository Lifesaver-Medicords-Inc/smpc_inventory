using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class PurchaseOrderModel
    {
        public int id { get; set; }
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string supplier_code { get; set; }
        public string address { get; set; }
        public string tin_no { get; set; }
        public string fax_no { get; set; }
        public string main_tel_no { get; set; }
        public int ship_type_id { get; set; }
        public string deliver_to { get; set; }
        public string deliver_via { get; set; }
        public string doc_no { get; set; }
        public string date { get; set; }
        public string order_type { get; set; }
        public string tax_code { get; set; }
        public string payment_terms_id { get; set; }
        public string bill_to { get; set; }
        public string ref_doc_no { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public decimal net_of_vat { get; set; }
        public decimal vat { get; set; }
        public decimal total_amount_due { get; set; }
    }
    class PurchaseOrderDetailsModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_description{ get; set; }
        public int req_qty { get; set; }
        public int order_qty { get; set; }
        public string unit_of_measure { get; set; }
        public decimal unit_price { get; set; }
        public string discount { get; set; }
        public decimal discounted_price { get; set; }
        public decimal total_price { get; set; }
        public string order_detail_ids { get; set; }
        public string allocated_qtys { get; set; }
        public string qtys { get; set; }
        //public string item_description { get; set; }

        // §7.1 (Phase 2 item 2.7) - "WAITING FOR DELIVERY" until this line's own
        // order_qty is fully received, then "IN STOCK". Deliberately not the same
        // vocabulary tbl_trans_sales_order_details.status uses - this PO line can
        // consolidate the same item across several SOs (order_detail_ids above),
        // each possibly at a different §7.1 state, so there's no single correct
        // SO-line status to show here. Server-computed; never sent back on save.
        public string delivery_status { get; set; }
    }

    class PurchaseOrderViewModel
    {
        public int id { get; set; }
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string supplier_code { get; set; }
        public string doc_no { get; set; }
    }

    class PurchaseOrderDetailsViewModel
    {
        public int pod_id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string ordered_qty { get; set; }
        public string ordered_uom { get; set; }
        public bool? is_complete { get; set; }
    }

    class PurchaseOrders
    {
        public List<PurchaseOrderModel> purchaseorder { get; set; }
    }
    class PurchaseOrdersWithDetails
    {
        public List<PurchaseOrderModel> purchaseorder { get; set; }
        public List<PurchaseOrderDetailsModel> purchaseorderdetails { get; set; }
    }
    class PurchaseOrderPrintModel
    {
        public PurchaseOrderModel poDoc { get; set; }
        public List<PurchaseOrderDetailsModel> poDocDetails { get; set; }
    }

}
