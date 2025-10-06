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
    class ItemMaterialServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemMaterialModel>>>.Get(ENUM_ENDPOINT.ITEM_MATERIAL);

            DataTable itemMaterial = JsonHelper.ToDataTable(response.Data);

            return itemMaterial;
        }

        public static async Task<ItemMaterialModel[]> GetMaterial()
        {
            var response = await RequestToApi<ApiResponseModel<ItemMaterialModel[]>>.Get(ENUM_ENDPOINT.ITEM_MATERIAL);
            var itemMaterial = response.Data;

            return itemMaterial;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_MATERIAL, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_MATERIAL, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemMaterialModel>>.Delete(ENUM_ENDPOINT.ITEM_MATERIAL, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
