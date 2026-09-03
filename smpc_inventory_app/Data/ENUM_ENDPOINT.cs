using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{
   internal static class ENUM_ENDPOINT
    {
        private static string setupItem = "/setup/item/";

        public static string ITEM = setupItem;
        public static string ITEM_IMAGE = setupItem + "item_image";
        public static string ITEM_BRAND = setupItem + "brand";
        public static string ITEM_TYPE = setupItem + "type";
        public static string ITEM_CLASS = setupItem + "class";
        public static string ITEM_NAME = setupItem + "name";
        public static string ITEM_MODEL = setupItem + "model";
        public static string ITEM_IMPELLER = setupItem + "material";
        public static string ITEM_MATERIAL = setupItem + "material";
        public static string ITEM_PUMP_COUNT = setupItem + "pump_count";
        public static string ITEM_PUMP_TYPE= setupItem + "pump_type";
        public static string VALUATIONMETHOD = setupItem + "valuation_method";
        public static string UNIT_OF_MEASUREMENT = "/setup/unit_measurement";
        public static string PAYMENT_TERMS  = "/setup/payment_terms";
        public static string ENTITY = "/setup/entity";
        public static string INDUSTRIES = "/setup/industries";
        public static string SOCIALS = "/setup/social";
        public static string POSITION = "/setup/position";
        public static string BOM = "/setup/bom";
        public static string BomItemList = "/setup/bom/item_list";
        public static string BomAllItemList = "/setup/all_bom/item_list";
        public static string BOQ = "/setup/boq";
        public static string BOQ_NOTES = "/setup/boq_notes";
        public static string SHIPTYPE = "/setup/shiptype";

        //REPORTS
        public static string REPORTS = "/setup/report";

        //inventory
        public static string INVENTORY = "/setup/inv";

        //inventory tracker
        public static string INVENTORYTRACKER = INVENTORY + "/tracker";
        public static string WAREHOUSENAME = INVENTORY + "/warehouse_name";

        //inventory logbook
        public static string INVENTORYLOGBOOK = INVENTORY + "/logbook";

        //warehouse
        //private static string setupWarehouse = "/setup/warehouse/";
        public static string WAREHOUSE = "/setup/warehouse/name"; //parent
        public static string USE_TYPE = "/setup/warehouse/usetype";
        //public static string WAREHOUSE_ADDRESS = "/setup/warehouse/manager"; //unwanted child
        public static string WAREHOUSE_AREAS = "/setup/warehouse/area";

        //Receiving Report 2
        public static string REPORTS2 = "/setup/report2";
        public static string RECEIVING_REPORT2 = REPORTS2 + "/receiving2";
        public static string PURCHASE_ORDER_VIEW = REPORTS2 + "/purchase_filter";
        public static string RECEIVING_REPORT_DETAILS2 = REPORTS2 + "/receiving_details2";
        public static string RECEIVING_REPORT_HISTORY = REPORTS2 + "/history";
        public static string PURCHASING_POD_VIEW = REPORTS2 + "/purchase_order";

        //Item Stocks (Inventory Item Stocks module)
        public static string ITEM_STOCKS = "/inventory/item_stocks";
        public static string ITEM_STOCKS_TRANSFER = "/inventory/item_stocks/transfer";
        // Physical stock minus reservations, per item - spec 8.11's "TOTAL STOCK
        // (tracker) = Sum of zone units, EXCLUDING reserved". No item_id returns every item.
        public static string ITEM_STOCKS_AVAILABLE = "/inventory/item_stocks/available";

        //Receiving Report
        public static string RECEIVING_REPORT = "/inventory/receiving_report";
        public static string RECEIVING_REPORT_WAREHOUSE = "/inventory/receiving_report/warehouse";
        public static string RECEIVING_REPORT_WAREHOUSE_AREA = "/inventory/receiving_report/warehouse_area/";
        public static string RECEIVING_REPORT_PURCHASE_DOC = "/inventory/receiving_report/purchase_order_doc";
        public static string RECEIVING_REPORT_PURCHASE = "/inventory/receiving_report/purchase_order/";

        //Users 
        public static string EmployeeUsers = "/employee_users/";

        public static string USERS = "/setup/warehouse/manager";




        //BPI
        public static string BPI = "/bpi";
        public static string BpiItemList = "/bpi/list";
        public static string BpiEntity = "/bpi/entity";
        public static string CreateBPI = "/bpi/createbpi";
        public static string BPIMain = "/bpi/main";

        //PURCHASING
        public static string PURCHASING = "/purchasing";

        public static string PURCHASINGREDBOXPURCHASELIST = PURCHASING + "/purchase_redbox_list";
        public static string SOPURCHASINGLIST = PURCHASING + "/so_purchase_list";
        public static string PRPURCHASINGLIST = PURCHASING + "/pr_purchase_list";
        public static string PURCHASINGLISTSUPPLIER = PURCHASING + "/purchase_list_supplier";
        public static string PURCHASE_REQUISITION = PURCHASING + "/purchase_requisition";
        public static string PURCHASE_REQUISITION_DETAILS = PURCHASING + "/purchase_requisition_details";

        // Purchase Return (PRT#, spec section 5.8) - NOT nested under PURCHASING;
        // the Go route is its own top-level group ("/api/purchase-returns"), same
        // convention as the other new documents built this phase.
        public static string PURCHASE_RETURN = "/purchase-returns";

        // Invoice Receipt - read-only reuse of the existing Accounting-app
        // endpoint. Purchase Return references an IR, never a PO (spec 5.8), and
        // this app has no IR data of its own - it's purely an Accounting
        // document - so this just points at the same API the Accounting client
        // already calls.
        public static string INVOICE_RECEIPT = "/accounting/invoice_receipt";
        public static string PURCHASING_CANVASS_SHEET = PURCHASING + "/purchase_canvass_sheet_so";
        public static string PURCHASING_PURCHASE_ORDER = PURCHASING + "/purchase_order";
        public static string PURCHASING_GUIDING_PRICE = PURCHASING + "/purchase_guiding_price";
        public static string PURCHASE_ORDER_ACTIVE = PURCHASING + "/purchase_active_po";
        public static string PURCHASE_ORDER_CLOSED = PURCHASING + "/purchase_closed_po";

        //PURCHASING WS
        public static string WSPURCHASINGREDBOXLIST = PURCHASING + "/redboxlist";

        // Production Report (spec §5.23) - read-only reuse of the Engineering app's
        // own Job Order API. This app has no Job Order data of its own; the
        // Warehouse Manager's acknowledgement queue and the acknowledge action both
        // live on Engineering's existing endpoints, same "point at the owning
        // feature's endpoint" precedent as Purchase Return's Invoice Receipt above.
        public static string PRODUCTION_PENDING_REPORTS = "/engineering/job_order/pending_production_reports";
        public static string PRODUCTION_ACKNOWLEDGE = "/engineering/job_order/"; // + "{id}/acknowledge"

        //SALES 
        public static string SALES = "/sales";
        public static string SALES_ORDER = SALES + "/order";
        public static string SALES_ORDER_DETAILS = SALES + "/order_details";

    }
}
