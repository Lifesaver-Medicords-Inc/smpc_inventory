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
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<InventoryLogbookView>>>.Get(ENUM_ENDPOINT.INVENTORYLOGBOOK);
             
            if (response?.Data == null || !response.Data.Any())
                return new DataTable();

            DataTable invTracker = JsonHelper.ToDataTable(response.Data);
            return invTracker;
        }
    }
}
