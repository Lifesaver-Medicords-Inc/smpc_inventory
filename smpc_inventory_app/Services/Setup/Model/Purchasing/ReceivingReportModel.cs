using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    public class ReceivingReportModel
    {
        public int id { get; set; }
        public string supplier_name { get; set; }
        public string supplier_code { get; set; }
        public string date_received { get; set; }
        public string address { get; set; }
        public int supplier_id { get; set; }
        public string doc { get; set; } //backend generated
        public string ref_doc { get; set; }
        public string prepared_by { get; set; }
        public int purchase_order_id { get; set; }
        public string warehouse_name { get; set; }
        public int warehouse_id { get; set; }
    }

    //children
    public class ReceivingReportDetailsModel
    {
        public string id { get; set; }
        public string receiving_report_id { get; set; }
        public string item_code { get; set; } 
        public string item_description { get; set; }
        public string ordered_qty { get; set; }
        public string ordered_uom { get; set; }
        public string received_qty { get; set; }
        public string received_uom { get; set; }
        public string rejected_qty { get; set; }
        public string rejected_uom { get; set; }
        public string reason_for_rejection { get; set; }
        public string ref_id { get; set; } // PO's ID: unnecessary I think removable
    }
     
    public class ReceivingReportInventoryModel
    {
        public string id { get; set; }
        public string receiving_report_id { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string ordered_qty { get; set; }
        public string ordered_uom { get; set; } 
        public string serial_number { get; set; }
        public string bin_location { get; set; }
        public string ref_id { get; set; } // PO's ID
    }

    public class ReceivingReportDetailsModel2
    {
        public int id { get; set; }
        public int pod_id { get; set; }
        public int receiving_report_id { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string ordered_qty { get; set; }
        public string ordered_uom { get; set; }
        public string received_qty { get; set; }
        public string received_uom { get; set; }
        public string rejected_qty { get; set; }
        public string rejected_uom { get; set; }
        public string reason_for_rejection { get; set; }
        public string serial_number { get; set; }
        public string bin_location { get; set; }
        public int ref_id { get; set; }
    }

    public class ReceivingReportList
    {
        public List<ReceivingReportModel> receiving_report { get; set; }
        public List<ReceivingReportDetailsModel> receiving_report_details { get; set; }
        public List<ReceivingReportInventoryModel> receiving_report_inventory { get; set; }
        //public List<childmodel> main tab somethin { get; set; }
    }

    public class ReceivingReportList2
    {
        public List<ReceivingReportModel> receiving_report { get; set; }
        public List<ReceivingReportDetailsModel2> receiving_report_details { get; set; }
        //public List<childmodel> main tab somethin { get; set; }
    }

    public class ReceivingReportPayload
    {
        public ReceivingReportModel receiving_report { get; set; }
        public List<ReceivingReportDetailsModel2> receiving_report_details { get; set; }
    }

    public class ReceivingReportHistory
    {
        public int id { get; set; }
        public int purchase_order_id { get; set; }
        public int receiving_report_id { get; set; }
        public int receiving_report_details_id { get; set; }
        public int ordered_qty { get; set; }
        public int received_qty { get; set; }
        public string date_received { get; set; }
    }
}
