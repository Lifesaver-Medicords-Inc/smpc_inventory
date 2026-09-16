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

        // One page of the BPI item picker (20 rows), searched on the server. The unpaged
        // call above is left alone for any caller that still wants the whole list.
        public static async Task<PaginatedResult<List<ItemBpiList>>> GetPaged(string search, int page = 1)
        {
            string query = $"?page={page}";
            if (!string.IsNullOrWhiteSpace(search))
                query += $"&search={Uri.EscapeDataString(search)}";

            var response = await RequestToApi<ApiResponseModel<List<ItemBpiList>>>.Get(
                ENUM_ENDPOINT.BpiItemList + "/paged" + query);

            return new PaginatedResult<List<ItemBpiList>>
            {
                Data = response?.Data ?? new List<ItemBpiList>(),
                Pagination = response?.pagination
            };
        }
    }
}
