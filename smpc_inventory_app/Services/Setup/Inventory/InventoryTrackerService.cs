using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Bom;
using smpc_inventory_app.Services.Setup.Model.Bpi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_inventory_app.Services.Setup.Model.Warehouse;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    class InventoryTrackerService
    {
        public static async Task<DataTable> GetAsDatatable(int warehouseNameId)
        {
            var response = await RequestToApi<ApiResponseModel<List<InventoryTrackerView>>>.Get(ENUM_ENDPOINT.INVENTORYTRACKER);

            if (response?.Data == null || !response.Data.Any())
                return new DataTable();

            // filter before converting to DataTable
            var filtered = response.Data.Where(item => item.warehouse_id == warehouseNameId).ToList();

            DataTable invTracker = JsonHelper.ToDataTable(filtered);
            return invTracker;
        }

        public static async Task<List<WarehouseName>> GetWarehouseName()
        {
            var response = await RequestToApi<ApiResponseModel<List<WarehouseName>>>.Get(ENUM_ENDPOINT.WAREHOUSENAME);

            return response.Data;
        }

        public static async Task<List<WarehouseAreaModel>> GetWarehouseArea(int warehouseNameId)
        {
            var response = await RequestToApi<ApiResponseModel<List<WarehouseAreaModel>>>.Get(ENUM_ENDPOINT.WAREHOUSE_AREAS);

            if (response?.Data == null || !response.Data.Any())
                return new List<WarehouseAreaModel>();

            // Filter only the warehouse areas belonging to this warehouse
            return response.Data.Where(area => area.warehouse_name_id == warehouseNameId).ToList();
        }

        public static async Task<ApiResponseModel> CreateInvTracker(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.INVENTORYTRACKER, data);

            return response;
        }

        public static async Task<ApiResponseModel> UpdateInvTracker(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.INVENTORYTRACKER, data);
            return response;
        }

        public static async Task<ApiResponseModel> DeleteInvTracker(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Delete(ENUM_ENDPOINT.INVENTORYTRACKER, data);
            return response;
        }
    }
}
