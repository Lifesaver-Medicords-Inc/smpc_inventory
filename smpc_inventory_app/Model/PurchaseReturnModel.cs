using System.Collections.Generic;

namespace smpc_inventory_app.Model
{
    // Mirrors ERP_API's models.PurchaseReturnContent (spec section 5.8) field for
    // field - snake_case to match the Go JSON tags directly, same convention as
    // ReceivingReportModel.
    public class PurchaseReturnModel
    {
        public int? id { get; set; }
        public int doc_no { get; set; }
        public string doc_date { get; set; }

        public int supplier_id { get; set; }
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }

        // References an Invoice Receipt, never a PO (spec 5.8) - the IR records
        // what actually arrived and what will be paid.
        public int ref_ir_id { get; set; }
        public string ref_ir_no { get; set; }

        // "Return with Debit Memo" | "Return without Debit Memo".
        public string return_type { get; set; }

        public int ref_dm_id { get; set; }
        public string ref_dm_no { get; set; }

        public int source_sales_return_id { get; set; }
        public int source_sales_return_details_id { get; set; }

        public string remarks { get; set; }

        // CBDO approval gate.
        public bool is_approved { get; set; }
        public int approved_by_id { get; set; }
        public string approved_by_name { get; set; }
        public string approval_date { get; set; }
    }

    public class PurchaseReturnDetailsModel
    {
        public int? id { get; set; }
        public int purchase_return_id { get; set; }

        // Pins to a specific Invoice Receipt LINE, never the header - one IR can
        // span several POs, so matching at the header level could apply a return
        // against the wrong PO/supplier (spec 5.8's own warning).
        public int ref_ir_details_id { get; set; }

        public int item_id { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string unit_of_measure { get; set; }

        public int qty { get; set; }

        // Auto-filled from the matched IR line, not user-entered.
        public double unit_cost { get; set; }

        public string reason { get; set; }
    }

    public class PurchaseReturnList
    {
        public List<PurchaseReturnModel> purchase_return { get; set; }
        public List<PurchaseReturnDetailsModel> purchase_return_details { get; set; }
    }

    public class PurchaseReturnPayload
    {
        public PurchaseReturnModel purchase_return { get; set; }
        public List<PurchaseReturnDetailsModel> purchase_return_details { get; set; }
    }
}
