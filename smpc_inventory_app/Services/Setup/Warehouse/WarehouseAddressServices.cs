using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Warehouse
{
    class WarehouseAddressServices //not used (becomes a child)
    {
        //public static async Task<DataTable> GetDataTable()
        //{
        //    var response = await RequestToApi<ApiResponseModel<List<WarehouseAddressServices>>>.Get(ENUM_ENDPOINT.WAREHOUSE_ADDRESS);

        //    DataTable WarehouseAddress = JsonHelper.ToDataTable(response.Data);

        //    return WarehouseAddress;
        //}

        //public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        //{
        //    var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.WAREHOUSE_ADDRESS, data);

        //    return response;
        //}

        //public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        //{
        //    var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.WAREHOUSE_ADDRESS, data);

        //    return response;
        //}

        //public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        //{
        //    var response = await RequestToApi<ApiResponseModel<WarehouseAddressServices>>.Delete(ENUM_ENDPOINT.WAREHOUSE_ADDRESS, data);
        //    bool isSuccess = response.Success;

        //    return isSuccess;
        //}
    }
}
