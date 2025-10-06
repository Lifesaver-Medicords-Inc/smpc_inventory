using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Warehouse
{
    class WarehouseAreaServices //for testing before having a parent
    {
        public static async Task<DataTable> GetDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<WarehouseAreaServices>>>.Get(ENUM_ENDPOINT.WAREHOUSE_AREAS);

            DataTable WarehouseArea = JsonHelper.ToDataTable(response.Data);

            return WarehouseArea;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.WAREHOUSE_AREAS, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.WAREHOUSE_AREAS, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<WarehouseAreaServices>>.Delete(ENUM_ENDPOINT.WAREHOUSE_AREAS, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }
    }
}
