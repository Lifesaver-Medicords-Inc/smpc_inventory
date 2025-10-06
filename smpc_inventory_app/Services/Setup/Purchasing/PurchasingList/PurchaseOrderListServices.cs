using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing.PurchasingList
{
    class PurchaseOrderListServices
    {
        public static async Task<DataTable> GetActivePOAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchaseOrderListModel>>>.Get(Data.ENUM_ENDPOINT.PURCHASE_ORDER_ACTIVE);

            DataTable activePurchaseOrder = JsonHelper.ToDataTable(response.Data);

            return activePurchaseOrder;

        }
        public static async Task<DataTable> GetClosedPOAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchaseOrderListModel>>>.Get(Data.ENUM_ENDPOINT.PURCHASE_ORDER_CLOSED);

            DataTable closedPurchaseOrder = JsonHelper.ToDataTable(response.Data);

            return closedPurchaseOrder;

        }
    }
}
