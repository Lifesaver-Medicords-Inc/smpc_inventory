using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Data;
using System.Data;

namespace smpc_inventory_app.Services.Setup.Warehouse
{
    internal static class WarehouseNameServices
    {
        public static async Task<WarehouseList> GetWarehouseInfos()
        {
            var response = await RequestToApi<ApiResponseModel<WarehouseList>>.Get(ENUM_ENDPOINT.WAREHOUSE);
            WarehouseList warehouseData = response.Data;
            return warehouseData;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.WAREHOUSE, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.WAREHOUSE, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<WarehouseNameModel>>.Delete(ENUM_ENDPOINT.WAREHOUSE, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }
    }
}
