using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    // Matches models.PendingProductionReportView on the Go side - one row per Job
    // Order marked COMPLETE that the Warehouse Manager hasn't acknowledged yet
    // (spec §5.23), via GET /engineering/job_order/pending_production_reports.
    public class ProductionReportModel
    {
        public int id { get; set; } // job order id - what AcknowledgeAsync targets
        public int so_id { get; set; }
        public int order_details_id { get; set; }
        public int item_id { get; set; }
        public string date { get; set; }
        public string sales_order { get; set; }
        public string general_name { get; set; }
        public string item_desc { get; set; }
        public string type { get; set; }
        public string materials { get; set; }
        public int quantity { get; set; }
        public string due { get; set; }
        public int engr_id { get; set; }
        public string a_engr { get; set; }
        public string status { get; set; }
        public string so_item_status { get; set; }
        public string serial_no { get; set; }
        public string report { get; set; }
        public string report_base { get; set; }
    }
}
