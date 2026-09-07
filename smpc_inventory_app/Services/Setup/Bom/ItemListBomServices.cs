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

namespace smpc_inventory_app.Services.Setup.Bom
{
    class ItemListBomServices
    {
        // response?.Data, not response.Data: both callers in bom.cs are async void and
        // are NOT awaited (GetBomItemList/GetAllBomItemList), so anything thrown here
        // surfaces as an unhandled exception rather than as a failed await. A request
        // that failed - API restarting, connection dropped - returns no payload, and
        // dereferencing it crashed the whole BOM screen on open.
        //
        // ToDataTable now tolerates a null list and returns an empty table with the
        // right columns, so a failed fetch degrades to "no items" instead of a crash.
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemBomList>>>.Get(ENUM_ENDPOINT.BomItemList);
            DataTable entityType = JsonHelper.ToDataTable(response?.Data);
            return entityType;
        }

        public static async Task<DataTable> GetAllAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemBomList>>>.Get(ENUM_ENDPOINT.BomAllItemList);
            DataTable entityType = JsonHelper.ToDataTable(response?.Data);
            return entityType;
        }
    }
}
