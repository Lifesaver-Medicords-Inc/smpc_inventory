using System.Collections.Generic;
using System.Threading.Tasks;
using smpc_inventory_app.Model;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    // Backs the Inventory Item Stocks module. GET (list) and general dictionary-based
    // Insert/Update/Delete already come from ServiceBase<ItemStockModel> - AddStock and
    // AdjustStock are the two operations specific to this module: AddStock adds a quantity
    // for an item+warehouse+bin (upserted server-side, so it's safe even if that
    // combination already has stock), while AdjustStock is a manual correction that SETS
    // stock_qty directly (not a delta) and always carries a Remarks value for the audit trail.
    class ItemStockService : ServiceBase<ItemStockModel>
    {
        public ItemStockService() : base(ENUM_ENDPOINT.ITEM_STOCKS) { }

        public async Task<ApiResponseModel> AddStock(int itemId, int warehouseId, string binLocation, int qty, string uom)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_STOCKS, new Dictionary<string, dynamic>
            {
                { "item_id", itemId },
                { "warehouse_id", warehouseId },
                { "bin_location", binLocation },
                { "stock_qty", qty },
                { "stock_uom", uom }
            });

            return response;
        }

        public async Task<ApiResponseModel> AdjustStock(int id, int newQty, string remarks)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.ITEM_STOCKS, new Dictionary<string, dynamic>
            {
                { "id", id },
                { "new_qty", newQty },
                { "remarks", remarks }
            });

            return response;
        }
    }
}
