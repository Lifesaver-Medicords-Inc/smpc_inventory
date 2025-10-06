using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class ReceivingReportServices
    {
        public static async Task<ReceivingReportList> GetReceivingReportInfos()
        { 
            var response = await RequestToApi<ApiResponseModel<ReceivingReportList>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT);
            ReceivingReportList receivingReportData = response.Data;
            return receivingReportData;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.RECEIVING_REPORT, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.RECEIVING_REPORT, data);

            return response;
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ReceivingReportModel>>.Delete(ENUM_ENDPOINT.RECEIVING_REPORT, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }

        public static async Task<bool> DeleteDetailsSpecificRow(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ReceivingReportModel>>.Delete(ENUM_ENDPOINT.RECEIVING_REPORT_DETAILS, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }

        public static async Task<bool> DeleteInventorySpecificRow(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ReceivingReportModel>>.Delete(ENUM_ENDPOINT.RECEIVING_REPORT_INVENTORY, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }
    }
}
