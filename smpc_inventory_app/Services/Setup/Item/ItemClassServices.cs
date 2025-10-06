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
    internal static class ItemClassServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemClassModel>>>.Get(ENUM_ENDPOINT.ITEM_CLASS);
         
            DataTable itemClass = JsonHelper.ToDataTable(response.Data);

            return itemClass;
        }

        public static async Task<ItemClassModel[]> GetClass()
        {
            var response = await RequestToApi<ApiResponseModel<ItemClassModel[]>>.Get(ENUM_ENDPOINT.ITEM_CLASS);
            var itemClass = response.Data;

            return itemClass;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string,dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_CLASS, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_CLASS, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemClassModel>>.Delete(ENUM_ENDPOINT.ITEM, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }

    }
}
