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
        public static string BRAND = setupItem + "brand";
        public static string ITEM_TYPE = setupItem + "type";
        public static string ITEM_CLASS = setupItem + "class";
        public static string ITEM_NAME = setupItem + "name";
        public static string ITEM_MODEL = setupItem + "model";
        public static string ITEM_MATERIAL = setupItem + "material";
        public static string ITEM_PUMP_COUNT = setupItem + "pump_count";
        public static string ITEM_PUMP_TYPE= setupItem + "pump_type";
        public static string UNIT_OF_MEASURMENT = "/setup/unit_measurement";
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

        //REPORTS
        public static string REPORTS = "/setup/report";

            //receiving
            public static string RECEIVING_REPORT = REPORTS + "/receiving"; 
            public static string RECEIVING_REPORT_DETAILS = REPORTS + "/receiving_details"; 
            public static string RECEIVING_REPORT_INVENTORY = REPORTS + "/receiving_inventory";

        //Receiving Report
        public static string REPORTS2 = "/setup/report2";
        public static string RECEIVING_REPORT2 = REPORTS2 + "/receiving2";
        public static string PURCHASE_ORDER_VIEW = REPORTS2 + "/purchase_filter";
        public static string RECEIVING_REPORT_DETAILS2 = REPORTS2 + "/receiving_details2";
        public static string RECEIVING_REPORT_HISTORY = REPORTS2 + "/history";
        public static string PURCHASING_POD_VIEW = REPORTS2 + "/purchase_order";

        //Users 
        public static string EmployeeUsers = "/employee_users/";

        public static string USERS = "/setup/warehouse/manager";




        //BPI
        public static string BPI = "/bpi";
        public static string BpiItemList = "/bpi/list";
        public static string BpiEntity = "/bpi/entity";

        //PURCHASING
        public static string PURCHASING = "/purchasing";

        public static string PURCHASINGREDBOXPURCHASELIST = PURCHASING + "/purchase_redbox_list";
        public static string SOPURCHASINGLIST = PURCHASING + "/so_purchase_list";
        public static string PRPURCHASINGLIST = PURCHASING + "/pr_purchase_list";
        public static string PURCHASINGLISTSUPPLIER = PURCHASING + "/purchase_list_supplier";
        public static string PURCHASE_REQUISITION = PURCHASING + "/purchase_requisition";
        public static string PURCHASE_REQUISITION_DETAILS = PURCHASING + "/purchase_requisition_details";
        public static string PURCHASING_CANVASS_SHEET = PURCHASING + "/purchase_canvass_sheet_so";
        public static string PURCHASING_PURCHASE_ORDER = PURCHASING + "/purchase_order";
        public static string PURCHASING_GUIDING_PRICE = PURCHASING + "/purchase_guiding_price";
        public static string PURCHASE_ORDER_ACTIVE = PURCHASING + "/purchase_active_po";
        public static string PURCHASE_ORDER_CLOSED = PURCHASING + "/purchase_closed_po";

        //PURCHASING WS
        public static string WSPURCHASINGREDBOXLIST = PURCHASING + "/redboxlist";

        

        //SALES 
        public static string SALES = "/sales";
        public static string SALES_ORDER = SALES + "/order";
        public static string SALES_ORDER_DETAILS = SALES + "/order_details";

    }
}
