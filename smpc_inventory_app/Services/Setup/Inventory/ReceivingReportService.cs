using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    class ReceivingReportService
    {
        public static async Task<List<PurchaseOrderViewModel>> GetPOWithDetails()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchaseOrderViewModel>>>.Get(ENUM_ENDPOINT.PURCHASE_ORDER_VIEW);

            return response.Data;
        }

        public static async Task<List<PurchaseOrderDetailsViewModel>> GetPODetailsView(int poId)
        {
            var endpoint = $"{ENUM_ENDPOINT.PURCHASING_POD_VIEW}/{poId}";
            var response = await RequestToApi<ApiResponseModel<List<PurchaseOrderDetailsViewModel>>>.Get(endpoint);

            return response.Data;
        }

        public static async Task<WarehouseList> GetWarehouseDetails()
        {
            var response = await RequestToApi<ApiResponseModel<WarehouseList>>.Get(ENUM_ENDPOINT.WAREHOUSE);

            return response.Data;
        }

        public static async Task<ReceivingReportList2> GetRRRecords()
        {
            var response = await RequestToApi<ApiResponseModel<ReceivingReportList2>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT2);

            return response.Data;
        }

        public static async Task<DataTable> GetRRRecordsAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<ReceivingReportList2>>.Get(ENUM_ENDPOINT.RECEIVING_REPORT2);

            // If data is missing, return an empty DataTable
            if (response?.Data == null || response.Data.receiving_report == null)
                return new DataTable();

            // Convert List<ReceivingReportModel> → DataTable
            DataTable rrDataTable = JsonHelper.ToDataTable(response.Data.receiving_report);
            return rrDataTable;
        }

        public static async Task<List<WarehouseAreaModel>> GetWarehouseArea(int warehouseNameId)
        {
            var response = await RequestToApi<ApiResponseModel<List<WarehouseAreaModel>>>.Get(ENUM_ENDPOINT.WAREHOUSE_AREAS);

            if (response?.Data == null || !response.Data.Any())
                return new List<WarehouseAreaModel>();

            // Filter only the warehouse areas belonging to this warehouse
            return response.Data.Where(area => area.warehouse_name_id == warehouseNameId).ToList();
        }

        // CREATE
        public static async Task<object> CreateRRRecord(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Post(ENUM_ENDPOINT.RECEIVING_REPORT2, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response.Data;
        }

        // UPDATE
        public static async Task<object> UpdateRRRecord(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Put(ENUM_ENDPOINT.RECEIVING_REPORT2, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response.Data;
        }

        // DELETE
        public static async Task<object> DeleteRRRecord(ReceivingReportPayload payload)
        {
            var response = await RequestToApi<ApiResponseModel<object>>.Delete(ENUM_ENDPOINT.RECEIVING_REPORT2, new Dictionary<string, dynamic>
                {
                    { "receiving_report", payload.receiving_report },
                    { "receiving_report_details", payload.receiving_report_details }
                }
            );

            return response.Data;
        }
    }
}
