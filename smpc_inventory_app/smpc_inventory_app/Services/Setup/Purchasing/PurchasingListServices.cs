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
    class PurchasingListServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchasingListModel>>>.Get(ENUM_ENDPOINT.PURCHASINGLIST);

            DataTable purchasinglist = JsonHelper.ToDataTable(response.Data);

            return purchasinglist;
        }

        public static async Task<PurchasingListModel[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<PurchasingListModel[]>>.Get(ENUM_ENDPOINT.PURCHASINGLIST);
            ;
            var purchasinglist = response.Data;

            return purchasinglist;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.PURCHASINGLIST, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.PURCHASINGLIST, data);

            return response;
        }

        public static async Task<ApiResponseModel> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Delete(ENUM_ENDPOINT.PURCHASINGLIST, data);

            return response;
        }
    }

    class PurchasingListSupplierServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchasingListSupplierModel>>>.Get(ENUM_ENDPOINT.PURCHASINGLISTSUPPLIER);

            DataTable purchasinglist = JsonHelper.ToDataTable(response.Data);

            return purchasinglist;
        }

    }
}
