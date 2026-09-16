using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Item;


namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class ItemServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {

            var response = await RequestToApi<ApiResponseModel<List<Items>>>.Get(Data.ENUM_ENDPOINT.ITEM);

            DataTable items = JsonHelper.ToDataTable(response.Data);

            return items;

        }

        // One page of items (20) with only that page's specs, images, purchasing, sales,
        // inventory and production rows - the same nine lists GetAsDataTable returns for the
        // whole catalogue. Pass the last id of the current page as `after` for NEXT, the first
        // as `before` for PREV, or an id as `at` to open the page holding that item.
        public static async Task<PaginatedResult<Items>> GetPaged(int? after = null, int? before = null, int? at = null)
        {
            var queryParams = new List<string>();
            if (after.HasValue) queryParams.Add($"after={after}");
            if (before.HasValue) queryParams.Add($"before={before}");
            if (at.HasValue) queryParams.Add($"at={at}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var response = await RequestToApi<ApiResponseModel<Items>>.Get(Data.ENUM_ENDPOINT.ITEM_PAGED + queryString);

            return new PaginatedResult<Items>
            {
                Data = response?.Data,
                Pagination = response?.pagination
            };
        }

        // Matching items only, no children - the search modal shows five columns. 20 to a page,
        // with page/total_pages in the pagination for its "Page x of y" counter.
        public static async Task<PaginatedResult<List<ItemModel>>> Search(string search, int page = 1)
        {
            var queryParams = new List<string> { $"page={page}" };
            if (!string.IsNullOrWhiteSpace(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");

            var response = await RequestToApi<ApiResponseModel<List<ItemModel>>>.Get(
                Data.ENUM_ENDPOINT.ITEM_SEARCH + "?" + string.Join("&", queryParams));

            return new PaginatedResult<List<ItemModel>>
            {
                Data = response?.Data ?? new List<ItemModel>(),
                Pagination = response?.pagination
            };
        }

        public static async Task<ItemModel[]> GetName()
        {
            var response = await RequestToApi<ApiResponseModel<ItemModel[]>>.Get(Data.ENUM_ENDPOINT.ITEM);
            ;
            var item = response.Data;

            return item;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(Data.ENUM_ENDPOINT.ITEM, data);

            return response;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data) 
        {
            var response = await RequestToApi<ApiResponseModel>.Put(Data.ENUM_ENDPOINT.ITEM, data);

            return response;
        }
        public static async Task<ApiResponseModel<List<string>>> UploadImages(Dictionary<string, object> data)
        {
            return await RequestToApi<ApiResponseModel<List<string>>>.Post(Data.ENUM_ENDPOINT.ITEM, data);
        }

        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ItemModel>>.Delete(Data.ENUM_ENDPOINT.ITEM, data);
            bool isSuccess = response.Success;
            return isSuccess;
        }
    }
}
