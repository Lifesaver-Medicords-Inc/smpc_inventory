using System.Collections.Generic;

namespace smpc_inventory_app.Model
{
    // Read-only client-side view of ERP_API's accounting_models.InvoiceReceipt -
    // this app has no Invoice Receipt data of its own (it's purely an Accounting
    // document); Purchase Return just needs to search/reference one (spec 5.8
    // requires an IR, never a PO). Only the fields the Purchase Return picker
    // and line grid actually need are included here, not the full Accounting
    // header (tax code, AP voucher flag, etc. - not this app's concern).
    public class InvoiceReceiptModel
    {
        public int id { get; set; }
        public int doc_no { get; set; }
        public string doc_date { get; set; }
        public int supplier_id { get; set; }
        public string supplier_code { get; set; }
        public string supplier { get; set; }
    }

    public class InvoiceReceiptDetailsModel
    {
        public int id { get; set; }
        public int invoice_receipt_id { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public int req_qty { get; set; }
        public string req_uom { get; set; }
        public double unit_price { get; set; }
    }

    public class InvoiceReceiptList
    {
        public List<InvoiceReceiptModel> invoice_receipt { get; set; }
        public List<InvoiceReceiptDetailsModel> invoice_receipt_details { get; set; }
    }
}
