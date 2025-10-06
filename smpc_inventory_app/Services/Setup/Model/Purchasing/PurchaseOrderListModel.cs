using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class PurchaseOrderListModel
    {
        public int id { get; set; }
        public string doc_no { get; set; }
        public string supplier_name { get; set; }
        public string total_amount_due  { get; set; }
        public string lead_time { get; set; }
        public string receiving_report_id { get; set; }
        public string receiving_report_no { get; set; }

    }
}
