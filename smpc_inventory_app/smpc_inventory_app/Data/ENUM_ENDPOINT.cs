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
        public static string BOQ = "/setup/boq";
        public static string BOQ_NOTES = "/setup/boq_notes";
        public static string WIRING_NOTES = "/setup/wiringnotes";
        public static string QQ_NOTES = "/setup/boq/qq";



        //BPI
        public static string BPI = "/bpi";
        public static string BpiItemList = "/bpi/list";
        public static string BpiEntity = "/bpi/entity";
        public static string BpiUsers = "/bpi/users";
        //PURCHASING
        public static string PURCHASINGREDBOXPURCHASELIST = "/purchasing/purchase_redbox_list";
        public static string PURCHASINGLIST = "/purchasing/purchase_list";
        public static string PURCHASINGLISTSUPPLIER = "/purchasing/purchase_list_supplier";
        public static string SALES_REQUISITION = "/purchasing/purchase_requisition";

        //SALES 
        public static string SALES = "/sales";
        public static string SALES_ORDER = SALES + "/order";

    }
}
