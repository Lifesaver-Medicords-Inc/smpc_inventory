using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_inventory_app.Data;
using System.Data;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    class InventoryLogbookService
    {
        public static async Task<DataTable> GetAsDatatable(int warehouseNameId)
        {
            var response = await RequestToApi<ApiResponseModel<List<InventoryLogbookView>>>.Get(ENUM_ENDPOINT.INVENTORYLOGBOOK);

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
    }
}
