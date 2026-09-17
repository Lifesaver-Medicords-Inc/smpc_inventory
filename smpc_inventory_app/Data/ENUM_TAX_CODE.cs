using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{
    static  class ENUM_TAX_CODE
    {

        // The tax code with a rate of its own: picking it fills the rate from Company Setup.
        public const string VAT = "VAT";

        // The ten configured codes of spec 4.5.3. WV-010 was listed here and is deliberately
        // absent from that table; VAT-EXEMPT was missing, and it is one of the four VAT
        // treatments a partner can have (4.1.7). Rates are not kept here - 4.5.3 forbids it.
        public static DataTable LIST()
        {
            IEnumerable<string> data = new List<String>()
            {
                VAT,
                "NON-VAT",
                "VAT-EXEMPT",
                "ZERO-RATED",
                "2306-VAT",
                "EWT",
                "IT",
                "WC-140",
                "WC-160",
                "WC-515",
            };
            DataTable table = new DataTable();
            table.Columns.Add("title");
            table.Columns.Add("value");

            foreach (string item in data)
            {

                DataRow dr = table.NewRow();
                dr["title"] = item;
                dr["value"] = item;

                table.Rows.Add(dr);
            }

            return table;

        }



    }
}
