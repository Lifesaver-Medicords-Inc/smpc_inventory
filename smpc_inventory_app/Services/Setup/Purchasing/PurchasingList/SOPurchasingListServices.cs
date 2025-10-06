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
    class SOPurchasingListServices
    {
        public static async Task<List<SOPurchasingListModel>> GetList()
        {
            var response = await RequestToApi<ApiResponseModel<List<SOPurchasingListModel>>>.Get(ENUM_ENDPOINT.SOPURCHASINGLIST);
            return response.Data;
        }
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<SOPurchasingListModel>>>.Get(ENUM_ENDPOINT.SOPURCHASINGLIST);

            DataTable orderpurchasinglist = JsonHelper.ToDataTable(response.Data);

            return orderpurchasinglist;
        }

        public static async Task<SOPurchasingListModel[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<SOPurchasingListModel[]>>.Get(ENUM_ENDPOINT.SOPURCHASINGLIST);
            var purchasinglist = response.Data;

            return purchasinglist;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.SOPURCHASINGLIST, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.SOPURCHASINGLIST, data);

            return response;
        }

        public static async Task<ApiResponseModel> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Delete(ENUM_ENDPOINT.SOPURCHASINGLIST, data);

            return response;
        }
    }

   
}
