using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing
{
    class PRPurchasingListServices
    {
        //public static async Task<DataTable> GetAsDataTable()
        //{
        //    var response = await RequestToApi<ApiResponseModel<List<PRPurchasingListModel>>>.Get(ENUM_ENDPOINT.PRPURCHASINGLIST);

        //    DataTable orderpurchasinglist = JsonHelper.ToDataTable(response.Data);

        //    return orderpurchasinglist;
        //}
        public static async Task<List<PRPurchasingListModel>> GetList()
        {
            var response = await RequestToApi<ApiResponseModel<List<PRPurchasingListModel>>>.Get(ENUM_ENDPOINT.PRPURCHASINGLIST);
            return response.Data;
        }

        public static async Task<DataTable> GetAsDataTable()
        {
            var list = await GetList();
            return JsonHelper.ToDataTable(list);
        }
    }
}
