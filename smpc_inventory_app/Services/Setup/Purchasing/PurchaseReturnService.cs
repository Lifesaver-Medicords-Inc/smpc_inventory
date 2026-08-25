using System.Collections.Generic;
using System.Threading.Tasks;
using smpc_inventory_app.Model;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Services.Setup.Purchasing
{
    // Purchase Return (PRT#), spec section 5.8. GetAsModel() (inherited from
    // ServiceBase<PurchaseReturnList>) already matches ERP_API's response shape
    // directly ({"purchase_return": [...], "purchase_return_details": [...]}),
    // so only Create and Approve need custom methods here.
    class PurchaseReturnService : ServiceBase<PurchaseReturnList>
    {
        public PurchaseReturnService() : base(ENUM_ENDPOINT.PURCHASE_RETURN) { }

        public async Task<ApiResponseModel<object>> CreatePurchaseReturn(PurchaseReturnPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Post(ENUM_ENDPOINT.PURCHASE_RETURN, new Dictionary<string, dynamic>
                {
                    { "purchase_return", payload.purchase_return },
                    { "purchase_return_details", payload.purchase_return_details }
                }
            );

            return response;
        }

        // CBDO only - the server enforces this (PURCHASE_RETURN_APPROVAL access
        // code); this just calls the endpoint.
        public async Task<ApiResponseModel<object>> ApprovePurchaseReturn(int purchaseReturnId)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Post(
                $"{ENUM_ENDPOINT.PURCHASE_RETURN}/{purchaseReturnId}/approve",
                new Dictionary<string, dynamic>()
            );

            return response;
        }
    }
}
