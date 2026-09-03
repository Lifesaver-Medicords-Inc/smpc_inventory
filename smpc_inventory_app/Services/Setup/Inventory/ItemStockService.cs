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

        // Spec §8.11: "TOTAL STOCK (tracker) = Σ zone units, EXCLUDING reserved". The API
        // does that sum and the reservation subtraction in one query for every item at
        // once, so callers showing stock for a list of items (the Purchasing List's item
        // cards) fetch this once rather than per row.
        public async Task<List<ItemAvailableStockModel>> GetAvailableStock()
        {
            var response = await RequestToApi<ApiResponseModel<List<ItemAvailableStockModel>>>
                .Get(ENUM_ENDPOINT.ITEM_STOCKS_AVAILABLE);

            return response?.Data ?? new List<ItemAvailableStockModel>();
        }

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

        // §10.6's Transfer function - move some or all of one bin's stock to a different
        // bin, warehouse-to-warehouse moves included. No reference document field, by
        // design (see StockTransferBody on the Go side).
        public async Task<ApiResponseModel> TransferStock(int sourceStockId, int transferQty, int destWarehouseId, string destBinLocation, string remarks)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.ITEM_STOCKS_TRANSFER, new Dictionary<string, dynamic>
            {
                { "source_stock_id", sourceStockId },
                { "transfer_qty", transferQty },
                { "dest_warehouse_id", destWarehouseId },
                { "dest_bin_location", destBinLocation },
                { "remarks", remarks }
            });

            return response;
        }
    }
}
