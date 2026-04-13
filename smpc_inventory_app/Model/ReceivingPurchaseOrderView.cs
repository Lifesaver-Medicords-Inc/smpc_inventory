using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    public class ReceivingPurchaseOrderView
    {
        public int purchase_order_id { get; set; }
        public int supplier_id { get; set; }
        public string supplier { get; set; }
        public string supplier_code { get; set; }
    }

    public class ReceivingPurchaseOrderDetailsView
    {
        public int purchase_order_details_id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_desc { get; set; }
        public int ordered_qty { get; set; }
        public string ordered_uom { get; set; }
        public int remaining_qty { get; set; }
        public string remaining_uom { get; set; }
    }

    public class ReceivingPurchaseOrderDocView
    {
        public int purchase_order_id { get; set; }
        public string po_doc_no { get; set; }
    }

    public class PurchaseOrderReceivingViewList
    {
        public List<ReceivingPurchaseOrderView> purchase_order_view { get; set; }
        public List<ReceivingPurchaseOrderDetailsView> purchase_order_details_view { get; set; }
    }
}
