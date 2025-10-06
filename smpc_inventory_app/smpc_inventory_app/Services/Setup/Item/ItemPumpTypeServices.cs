using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Item;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Item
{
    class ItemPumpTypeServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemPumpTypeModel>>>.Get(ENUM_ENDPOINT.ITEM_PUMP_TYPE);

            DataTable pumpType = JsonHelper.ToDataTable(response.Data);

            return pumpType;
        }

        public static async Task<ItemPumpTypeModel[]> GetMaterial()
        {
            var response = await RequestToApi<ApiResponseModel<ItemPumpTypeModel[]>>.Get(ENUM_ENDPOINT.ITEM_PUMP_TYPE);
            var pumpType = response.Data;

            return pumpType;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_PUMP_TYPE, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_PUMP_TYPE, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemPumpTypeModel>>.Delete(ENUM_ENDPOINT.ITEM_PUMP_TYPE, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
