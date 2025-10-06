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
    class ItemImageServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {

            var response = await RequestToApi<ApiResponseModel<List<ItemImage>>>.Get(Data.ENUM_ENDPOINT.ITEM_IMAGE);

            DataTable items = JsonHelper.ToDataTable(response.Data);

            return items;

        }

        //public static async Task<ItemModel[]> GetName()
        //{
        //    var response = await RequestToApi<ApiResponseModel<ItemImageModel[]>>.Get(Data.ENUM_ENDPOINT.ITEM);
        //    ;
        //    var item = response.Data;

        //    return item;
        //}

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(Data.ENUM_ENDPOINT.ITEM_IMAGE, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(Data.ENUM_ENDPOINT.ITEM_IMAGE, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemModel>>.Delete(Data.ENUM_ENDPOINT.ITEM_IMAGE, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
