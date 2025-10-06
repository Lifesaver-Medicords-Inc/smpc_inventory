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
    class ItemPumpCountServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemPumpCountModel>>>.Get(ENUM_ENDPOINT.ITEM_PUMP_COUNT);

            DataTable pumpCount = JsonHelper.ToDataTable(response.Data);

            return pumpCount;
        }

        public static async Task<ItemPumpCountModel[]> GetMaterial()
        {
            var response = await RequestToApi<ApiResponseModel<ItemPumpCountModel[]>>.Get(ENUM_ENDPOINT.ITEM_PUMP_COUNT);
            var pumpCount = response.Data;

            return pumpCount;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_PUMP_COUNT, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_PUMP_COUNT, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemPumpCountModel>>.Delete(ENUM_ENDPOINT.ITEM_PUMP_COUNT, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
