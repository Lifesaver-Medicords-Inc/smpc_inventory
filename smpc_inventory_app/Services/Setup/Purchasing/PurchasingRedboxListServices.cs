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
    class PurchasingRedboxListServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<RedboxPurchasingList>>>.Get(ENUM_ENDPOINT.PURCHASINGREDBOXPURCHASELIST);

            DataTable purchasingredboxlist = JsonHelper.ToDataTable(response.Data);

            return purchasingredboxlist;
        }
        public static async Task<ApiResponseModel> UpdateSalesOrder(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.SALES_ORDER, data);

            return response;
        }
        public static async Task<ApiResponseModel> UpdatePurchaseRequisition(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.PURCHASE_REQUISITION, data);

            return response;
        }
    }

}
