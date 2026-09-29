// BomServices.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Services.Setup.Model.Bom;

namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class BomServices
    {
        // Get the BOMs as a DataTable
        public static async Task<DataTable> GetBomsAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<BomModel>>>.Get(ENUM_ENDPOINT.BOM);
            DataTable boms = JsonHelper.ToDataTable(response.Data);
            return boms;
        }

        // One page of the BOM screen: 20 heads with their component lines, in the same
        // bom_head/bom_details shape as GET /setup/bom. after/before/at are BOM ids, not page
        // numbers - the page after the one ending at `after`, before the one starting at
        // `before`, or starting at `at` (a searched or just-saved BOM).
        public static async Task<PaginatedResult<BomClass>> GetPaged(int? after = null, int? before = null, int? at = null)
        {
            var queryParams = new List<string>();
            if (after.HasValue) queryParams.Add($"after={after}");
            if (before.HasValue) queryParams.Add($"before={before}");
            if (at.HasValue) queryParams.Add($"at={at}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var response = await RequestToApi<ApiResponseModel<BomClass>>.Get(ENUM_ENDPOINT.BOM_PAGED + queryString);

            return new PaginatedResult<BomClass>
            {
                Data = response?.Data,
                Pagination = response?.pagination
            };
        }

        // The BOM screen's Search: matching heads only, 20 to a page, with page/total_pages
        // for the modal's "Page x of y".
        public static async Task<PaginatedResult<List<BomHead>>> Search(string search, int page = 1)
        {
            var queryParams = new List<string> { $"page={page}" };
            if (!string.IsNullOrWhiteSpace(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");

            var response = await RequestToApi<ApiResponseModel<List<BomHead>>>.Get(
                ENUM_ENDPOINT.BOM_SEARCH + "?" + string.Join("&", queryParams));

            return new PaginatedResult<List<BomHead>>
            {
                Data = response?.Data ?? new List<BomHead>(),
                Pagination = response?.pagination
            };
        }

        // Insert a BOM (parent)
        public static async Task<bool> InsertBom(Dictionary<string, dynamic> data)
        {

            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BOM, data);
            bool responseData = response.Success;
            return responseData;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BOM, data);

            return response;
        }


        //Delete a BOM(parent)
        public static async Task<ApiResponseModel> DeleteBom(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Delete(ENUM_ENDPOINT.BOM, data);
            return response;
        }

        // Update a BOM (parent)
        public static async Task<ApiResponseModel> UpdateBom(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BOM, data);
            return response;
        }
    }
}

