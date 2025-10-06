using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Data;

namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class ItemTypeServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemTypeModel>>>.Get(ENUM_ENDPOINT.ITEM_TYPE);

            DataTable itemType = JsonHelper.ToDataTable(response.Data);

            return itemType;
        }

        public static async Task<ItemTypeModel[]> GetType()
        {
            var response = await RequestToApi<ApiResponseModel<ItemTypeModel[]>>.Get(ENUM_ENDPOINT.ITEM_TYPE);
            var itemType = response.Data;

            return itemType;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_TYPE, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_TYPE, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemTypeModel>>.Delete(ENUM_ENDPOINT.ITEM_TYPE, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
