using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Item;


namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class ItemServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {

            var response = await RequestToApi<ApiResponseModel<List<Items>>>.Get(Data.ENUM_ENDPOINT.ITEM);

            DataTable items = JsonHelper.ToDataTable(response.Data);

            return items;

        }

        public static async Task<ItemModel[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<ItemModel[]>>.Get(Data.ENUM_ENDPOINT.ITEM);
            ;
            var item = response.Data;

            return item;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(Data.ENUM_ENDPOINT.ITEM, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data) 
        {
            var response = await RequestToApi<ApiResponseModel>.Put(Data.ENUM_ENDPOINT.ITEM, data);

            return response;
        }
        public static async Task<ApiResponseModel<List<string>>> UploadImages(Dictionary<string, object> data)
        {
            return await RequestToApi<ApiResponseModel<List<string>>>.Post(Data.ENUM_ENDPOINT.ITEM, data);
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemModel>>.Delete(Data.ENUM_ENDPOINT.ITEM, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
