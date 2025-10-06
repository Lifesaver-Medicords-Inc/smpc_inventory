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

namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class ItemNameServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemClassModel>>>.Get(ENUM_ENDPOINT.ITEM_NAME);

            DataTable itemNames = JsonHelper.ToDataTable(response.Data);

            return itemNames;

        }

        public static async Task<ItemNameModel[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<ItemNameModel[]>>.Get(ENUM_ENDPOINT.ITEM_NAME);
            var itemName = response.Data;

            return itemName;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_NAME, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_NAME, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemNameModel>>.Delete(ENUM_ENDPOINT.ITEM_NAME, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }

    }
}
