using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing
{
    internal static class PurchasingCanvassSheetServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchasingCanvassSheetModel>>>.Get(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET);

            DataTable canvassSheetData = JsonHelper.ToDataTable(response.Data);

            return canvassSheetData;

        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemNameModel>>.Delete(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }

    }
}
