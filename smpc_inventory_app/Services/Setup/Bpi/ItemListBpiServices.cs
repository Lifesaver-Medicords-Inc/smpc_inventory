using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Bpi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Bpi
{
    internal static class ItemListBpiServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemBpiList>>>.Get(ENUM_ENDPOINT.BpiItemList);
            DataTable entityType = JsonHelper.ToDataTable(response.Data);

            return entityType;

        }


    }
}
