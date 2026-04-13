using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    public class ReceivingReportModel
    {
        public int? id { get; set; }
        public string supplier { get; set; }
        public string supplier_code { get; set; }
        public int supplier_id { get; set; }
        public string date_received { get; set; }
        public int doc_no { get; set; }
        public string ref_doc { get; set; }
        public string prepared_by { get; set; }
        public int purchase_order_id { get; set; }
        public string warehouse { get; set; }
        public string warehouse_address { get; set; }
        public int warehouse_id { get; set; }
    }

    public class ReceivingReportDetailsModel
    {
        public int id { get; set; }
        public int receiving_report_id { get; set; }
        public int purchase_order_details_id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_desc { get; set; }
        public int ordered_qty { get; set; }
        public string ordered_uom { get; set; }
        public int? received_qty { get; set; }
        public string received_uom { get; set; }
        public int? remaining_qty { get; set; }
        public string remaining_uom { get; set; }
        public string serial_number { get; set; }
        public int? warehouse_id { get; set; }
        public string bin_location { get; set; }
        public int? rejected_qty { get; set; }
        public string rejected_uom { get; set; }
        public string reason_for_rejection { get; set; }
    }

    public class ReceivingReportList
    {
        public List<ReceivingReportModel> receiving_report { get; set; }
        public List<ReceivingReportDetailsModel> receiving_report_details { get; set; }
    }

    public class ReceivingReportPayload
    {
        public ReceivingReportModel receiving_report { get; set; }
        public List<ReceivingReportDetailsModel> receiving_report_details { get; set; }
    }
}
