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
    class ItemModelServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemModelModel>>>.Get(ENUM_ENDPOINT.ITEM_MODEL);

            DataTable itemModel = JsonHelper.ToDataTable(response.Data);

            return itemModel;
        }

        public static async Task<ItemModelModel[]> GetClass()
        {
            var response = await RequestToApi<ApiResponseModel<ItemModelModel[]>>.Get(ENUM_ENDPOINT.ITEM_MODEL);
            var itemModel = response.Data;

            return itemModel;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_MODEL, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_MODEL, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemModelModel>>.Delete(ENUM_ENDPOINT.ITEM_MODEL, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
