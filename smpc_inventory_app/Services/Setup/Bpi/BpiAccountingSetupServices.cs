using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Bpi
{
    // The two pieces of Accounting setup the BPI Finance and Items tabs read (spec 4.1.7):
    // the chart of accounts behind ACCOUNT, and Company Setup's VAT rate behind tax code VAT.
    internal static class BpiAccountingSetupServices
    {
        public const string SELECT_TEXT = "-- SELECT --";

        private class ChartOfAccountRow
        {
            public int id { get; set; }
            public string code { get; set; }
            public string name { get; set; }
        }

        private class CompanyVatRow
        {
            public double? vat_rate_percent { get; set; }
        }

        // id + "code - name", in code order, headed by a "-- SELECT --" row with id 0 so an
        // unset account reads as unset. Same id/name shape BindCmbValues expects.
        public static async Task<DataTable> GetAccounts()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Rows.Add(0, SELECT_TEXT);

            try
            {
                var response = await RequestToApi<ApiResponseModel<List<ChartOfAccountRow>>>.Get(ENUM_ENDPOINT.CHART_OF_ACCOUNTS);
                foreach (var account in (response?.Data ?? new List<ChartOfAccountRow>()).OrderBy(a => a.code))
                    table.Rows.Add(account.id, $"{account.code} - {account.name}");
            }
            catch (Exception ex)
            {
                // A failed fetch leaves only "-- SELECT --": the page still opens, and a
                // stored account shows as unset rather than as some other account.
                Debug.WriteLine($"[BpiAccountingSetupServices.GetAccounts] {ex.Message}");
            }

            return table;
        }

        // Company Setup's VAT rate as a whole-number percentage (12 means 12%), or null when
        // it could not be read - the caller then leaves the rate for the user to type.
        public static async Task<double?> GetVatRatePercent()
        {
            try
            {
                var response = await RequestToApi<ApiResponseModel<CompanyVatRow>>.Get(ENUM_ENDPOINT.COMPANY_SETUP);
                return response?.Data?.vat_rate_percent;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BpiAccountingSetupServices.GetVatRatePercent] {ex.Message}");
                return null;
            }
        }
    }
}
