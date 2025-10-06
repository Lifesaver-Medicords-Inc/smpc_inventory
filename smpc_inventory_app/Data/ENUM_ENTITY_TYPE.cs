using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{

    static class ENUM_ENTITY_TYPE
    {
        public static string Customer = "CUSTOMER";
        public static string Non_Affiliated = "NON-AFFILIATED";
        public static string Affiliated = "AFFILIATED";
        public static string Supplier = "SUPPLIER";
        public static string Blacklisted = "BLACKLISTED";
        public static string TempSupplier = "TEMP SUPPLIER";




        public static DataTable ENTITYLIST()
        {

            IEnumerable<string> data= new List<string>()
            {
                "SUPPLIER",
                "CUSTOMER",
                "BLACKLISTED",
                "NON-AFFILIATED",
                "AFFILIATED",
                "CLOSED"
            };
            DataTable table = new DataTable();
            table.Columns.Add("code");
            table.Columns.Add("name");
           

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["name"] = item;
                dr["code"] = item;
            
                table.Rows.Add(dr);

            }

            return table;
        }
    }
}
