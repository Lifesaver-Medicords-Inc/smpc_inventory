using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Data;

namespace smpc_inventory_app.Services.Setup.Warehouse
{
    internal static class WarehouseUseTypeServices
    {
        public static async Task<DataTable> GetDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<WarehouseUseTypeModel>>>.Get(ENUM_ENDPOINT.USE_TYPE);

            DataTable Usetypes = JsonHelper.ToDataTable(response.Data);

            return Usetypes;
        } 

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.USE_TYPE, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.USE_TYPE, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<WarehouseUseTypeModel>>.Delete(ENUM_ENDPOINT.USE_TYPE, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }
    }

}
