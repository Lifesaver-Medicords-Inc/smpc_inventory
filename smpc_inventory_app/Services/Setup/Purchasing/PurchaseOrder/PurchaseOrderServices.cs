using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Purchasing;
using smpc_inventory_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing
{
    class PurchaseOrderServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {

            var response = await RequestToApi<ApiResponseModel<List<PurchaseOrder>>>.Get(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);

            DataTable purchaseOrder = JsonHelper.ToDataTable(response.Data);

            return purchaseOrder;

        }

        public static async Task<PurchaseOrder[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<PurchaseOrder[]>>.Get(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);;
            
            var purchaseOrder = response.Data;

            return purchaseOrder;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER, data);
            string sasda = response.Success.ToString();
            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER, data);

            return response;
        }

        public static async Task<ApiResponseModel> UpdateOrderDetails(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(Data.ENUM_ENDPOINT.SALES_ORDER_DETAILS, data);

            return response;
        }
        public static async Task<ApiResponseModel> UpdatePurchaseRequisition(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.PURCHASE_REQUISITION_DETAILS, data);

            return response;
        }
        public static async Task<ApiResponseModel<List<string>>> UploadImages(Dictionary<string, object> data)
        {
            return await RequestToApi<ApiResponseModel<List<string>>>.Post(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER, data);
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<PurchaseOrder>>.Delete(Data.ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
