using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_inventory_app.Model;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    class ReceivingReport2Service : ServiceBase<ReceivingReportList>
    {
        public ReceivingReport2Service() : base(ENUM_ENDPOINT.RECEIVING_REPORT) { }

        // CREATE
        public async Task<ApiResponseModel<object>> CreateReceivingReport(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Post(ENUM_ENDPOINT.RECEIVING_REPORT, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response;
        }

        // UPDATE
        public async Task<ApiResponseModel<object>> UpdateReceivingReport(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Put(ENUM_ENDPOINT.RECEIVING_REPORT, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response;
        }

        // DELETE
        public async Task<ApiResponseModel<object>> DeleteReceivingReport(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Delete(ENUM_ENDPOINT.RECEIVING_REPORT, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response;
        }
    }
}
