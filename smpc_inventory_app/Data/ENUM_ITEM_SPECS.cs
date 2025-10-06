using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{
    static class ENUM_ITEM_SPECS
    {
        public static DataTable LIST()
        {
            IEnumerable<string> data = new List<String>()
            {
                "GENERIC",
                "PUMP",
                "CONTROLLER",
                "COMMON_PACKAGE"
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data) {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = item;

                table.Rows.Add(dr);

            }
           
            return table;
        }
        //public static DataTable GENERIC()
        //{
        //    IEnumerable<string> data = new List<String>()
        //    {
        //        "MATERIAL",
        //        "HEIGHT",
        //        "LENGTH",
        //        "WIDTH",
        //        "WEIGHT",
        //    }; 

        //    DataTable table = new DataTable();
        //    table.Columns.Add("title");
        //    table.Columns.Add("value");

        //    foreach (string item in data)
        //    {

        //        DataRow dr = table.NewRow();
        //        dr["title"] = item;
        //        dr["value"] = "";

        //        table.Rows.Add(dr);

        //    }
             
        //    return table;
        //}
        public static DataTable PUMP()
        {
            IEnumerable<string> data = new List<String>()
            {
                "FLA",
                "VOLTAGE",
                "HORESEPOWER",
                "KILOWATT",
                "SUCTION SIZE",
                "DISCHARGE SIZE",
                "PHASE (1 OR 3)",
                "MIN. CAPACITY",
                "MAX. CAPACITY",
                "MIN. HEAD",
                "MAX. HEAD",
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }

            return table;
        }
        public static DataTable CONTROLLER()
        {
            IEnumerable<string> data = new List<String>()
            {
                "MODEL",
                "# OF PUMP/S",
                "VOLTAGE",
                "CURRENT",
                "START METHOD",
                "USE TYPE",
                "PHASE (1 OR 3)",
                "WIRING DIAGRAM #",
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }

            return table;
        }
        public static DataTable COMMON_PACKAGE()
        {
            IEnumerable<string> data = new List<String>()
            {
                "TYPE",
                "SUCTION TYPE (+/-)",
                "MAIN LINE SIZE",
                "SUCTION SIZE",
                "DISCHARGE SIZE",
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }

            return table;
        }
        public static DataTable VALVE()
        {
            IEnumerable<string> data = new List<String>()
            {
                "SUNCTION SIZE",
                "DISCHARGE SIZE"
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }
            return table;
        }

        public static DataTable PRESSURE_TRANSDUCER()
        {
            IEnumerable<string> data = new List<String>()
            {
                "PRESSURE",
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }
            return table;
        }
        public static DataTable WATER_METER()
        {
            IEnumerable<string> data = new List<String>()
            {
                "GPM",
                "PRESSURE",
                "SIZE",
                "TYPE"
            };

            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = "";

                table.Rows.Add(dr);

            }
            return table;
        }
    }
}
