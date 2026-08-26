using System.Collections.Generic;
using System.Threading.Tasks;
using smpc_inventory_app.Model;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Services.Setup.Item
{
    // §5.23 Production Report - the Warehouse Manager's acknowledgement queue.
    // Read-only reuse of the Engineering app's own Job Order API (see
    // ENUM_ENDPOINT.PRODUCTION_PENDING_REPORTS/PRODUCTION_ACKNOWLEDGE) - this app
    // has no Job Order data of its own.
    class ProductionService : ServiceBase<ProductionReportModel>
    {
        public ProductionService() : base(ENUM_ENDPOINT.PRODUCTION_PENDING_REPORTS) { }

        // Acknowledges a completed Job Order and tells the server where the produced
        // units go into stock (warehouse/bin) - required, since there's no implicit
        // "production output" location anywhere in this codebase (see
        // JobOrderService.AcknowledgeJobOrder's own comment on the Go side).
        public async Task<ApiResponseModel> AcknowledgeAsync(int jobOrderId, int warehouseId, string binLocation)
        {
            var url = ENUM_ENDPOINT.PRODUCTION_ACKNOWLEDGE + jobOrderId + "/acknowledge";

            var response = await RequestToApi<ApiResponseModel>.Post(url, new Dictionary<string, dynamic>
            {
                { "warehouse_id", warehouseId },
                { "bin_location", binLocation }
            });

            return response;
        }
    }
}
