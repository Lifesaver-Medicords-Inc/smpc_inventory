using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Model;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Inventory;

namespace smpc_inventory_app.Pages.Inventory
{
    // Lists tbl_inv_item_stocks (one row per item+warehouse+bin, via GET /inventory/item_stocks)
    // and lets an authorized user manually correct a bin's quantity (PUT /inventory/item_stocks).
    // This is the authoritative current-stock table Receiving Report writes to - not the
    // separate legacy tbl_inv_stocks_location table used by vw_get_inventory_tracker/logbook.
    public partial class ItemStocksPage : UserControl
    {
        private readonly ItemStockService _service = new ItemStockService();
        private List<ItemStockModel> _allStocks = new List<ItemStockModel>();

        // Manual adjustment is gated the same way frm_warehouse_name_setup gates edits -
        // Inventory Manager level or above - since a bad correction here silently breaks the
        // running balance that Receiving Report, Pick Activity, and (eventually) Sales
        // Order's stock check all depend on. Viewing the list has no such restriction.
        private bool HasAdjustAuthority
        {
            get
            {
                string currentUserPosition = CacheData.CurrentUser.position.name.ToString().ToLower();
                return currentUserPosition.Contains("admin") || currentUserPosition.Contains("manager");
            }
        }

        public ItemStocksPage()
        {
            InitializeComponent();
        }

        private async void ItemStocksPage_Load(object sender, EventArgs e)
        {
            btn_adjust.Enabled = HasAdjustAuthority;
            btn_add.Enabled = HasAdjustAuthority;
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_item_stocks, "Fetching stock levels...");
                _allStocks = await _service.GetAsList();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error fetching item stocks: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_item_stocks);
            }
        }

        private void ApplyFilter()
        {
            string term = txt_search.Text?.Trim().ToLower() ?? "";

            IEnumerable<ItemStockModel> filtered = _allStocks;

            if (!string.IsNullOrEmpty(term))
            {
                filtered = _allStocks.Where(s =>
                    (s.item_code ?? "").ToLower().Contains(term) ||
                    (s.item_name ?? "").ToLower().Contains(term) ||
                    (s.brand ?? "").ToLower().Contains(term) ||
                    (s.warehouse_name ?? "").ToLower().Contains(term) ||
                    (s.bin_location ?? "").ToLower().Contains(term));
            }

            dgv_item_stocks.DataSource = filtered.ToList();
        }

        private async void btn_refresh_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dgv_item_stocks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenAdjustmentModal();
        }

        private void btn_adjust_Click(object sender, EventArgs e)
        {
            OpenAdjustmentModal();
        }

        private async void btn_add_Click(object sender, EventArgs e)
        {
            if (!HasAdjustAuthority)
            {
                Helpers.ShowDialogMessage("error", "Please use an account that is at the Inventory Manager level or above to add stock.");
                return;
            }

            using (var modal = new ItemStockAddModal())
            {
                if (modal.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Helpers.Loading.ShowLoading(dgv_item_stocks, "Adding stock...");
                    ApiResponseModel response = await _service.AddStock(modal.ItemId, modal.WarehouseId, modal.BinLocation, modal.Qty, modal.Uom);

                    if (response != null && response.Success)
                    {
                        Helpers.ShowDialogMessage("success", "Stock added successfully.");
                        await LoadData();
                    }
                    else
                    {
                        Helpers.ShowDialogMessage("error", $"Failed to add stock.\n{response?.message}");
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ShowDialogMessage("error", $"Error adding stock: {ex.Message}");
                }
                finally
                {
                    Helpers.Loading.HideLoading(dgv_item_stocks);
                }
            }
        }

        private async void OpenAdjustmentModal()
        {
            if (!HasAdjustAuthority)
            {
                Helpers.ShowDialogMessage("error", "Please use an account that is at the Inventory Manager level or above to adjust stock.");
                return;
            }

            if (!(dgv_item_stocks.CurrentRow?.DataBoundItem is ItemStockModel selected))
            {
                Helpers.ShowDialogMessage("error", "Please select a row first.");
                return;
            }

            using (var modal = new ItemStockAdjustmentModal(selected))
            {
                if (modal.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Helpers.Loading.ShowLoading(dgv_item_stocks, "Saving adjustment...");
                    ApiResponseModel response = await _service.AdjustStock(selected.id, modal.NewQty, modal.Remarks);

                    if (response != null && response.Success)
                    {
                        Helpers.ShowDialogMessage("success", "Stock adjusted successfully.");
                        await LoadData();
                    }
                    else
                    {
                        Helpers.ShowDialogMessage("error", $"Failed to adjust stock.\n{response?.message}");
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ShowDialogMessage("error", $"Error adjusting stock: {ex.Message}");
                }
                finally
                {
                    Helpers.Loading.HideLoading(dgv_item_stocks);
                }
            }
        }
    }
}
