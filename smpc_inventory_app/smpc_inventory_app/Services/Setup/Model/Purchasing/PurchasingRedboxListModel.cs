using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Purchasing
{
    class PurchasingRedboxListModel
    {
        public int id { get; set; }
        public string doc_no { get; set; }
        public string project_name { get; set; }
        public string commitment_date { get; set; }
        public string purchaser { get; set; }
        public string detail_ids { get; set; }
        public string item_ids { get; set; }
        public string item_names { get; set; }
        public string customer { get; set; }
        public string order_type { get; set; }
    }


    class RedboxSalesOrderListModel
    {
        public int id { get; set; }
        public string doc_no { get; set; }
        public string project_name { get; set; }
        public string commitment_date { get; set; }
        public string purchaser { get; set; }
        public string detail_ids { get; set; }
        public string item_ids { get; set; }
        public string item_names { get; set; }
        public string customer { get; set; }
        public string order_type { get; set; }
    }
    class RedboxPurchaseRequisitionListModel
    {
        public int id { get; set; }
        public string doc_no { get; set; }
        public string project_name { get; set; }
        public string commitment_date { get; set; }
        public string purchaser { get; set; }
        public string detail_ids { get; set; }
        public string item_ids { get; set; }
        public string item_names { get; set; }
        public string customer { get; set; }
        public string order_type { get; set; }
    }
    class RedboxPurchasingList
    {
        public List<PurchasingRedboxListModel> purchaselist { get; set; }
    }
}