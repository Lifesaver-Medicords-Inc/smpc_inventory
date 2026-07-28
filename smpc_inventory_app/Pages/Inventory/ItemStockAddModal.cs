using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Model;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Services.Setup.Warehouse;

namespace smpc_inventory_app.Pages.Inventory
{
    // "Add Stock" modal for ItemStocksPage. Unlike ItemStockAdjustmentModal (which corrects
    // an EXISTING bin's counted quantity), this adds a quantity for an item+warehouse+bin -
    // the Go side (InsertItemStock/UpsertStockWithTx) already handles the case where that
    // combination already has stock by adding onto it instead of creating a duplicate row,
    // so this form doesn't need to check for that itself.
    public partial class ItemStockAddModal : Form
    {
        private class ItemPickerOption
        {
            public ItemModel Model;
            public string Display => string.IsNullOrWhiteSpace(Model.item_model)
                ? $"{Model.item_code} - {Model.item_name}"
                : $"{Model.item_code} - {Model.item_name} - {Model.item_model}";
        }

        public int ItemId { get; private set; }
        public int WarehouseId { get; private set; }
        public string BinLocation { get; private set; }
        public int Qty { get; private set; }
        public string Uom { get; private set; }

        // Full, unfiltered list - cmb_item.DataSource itself gets swapped to a filtered
        // subset as the user types (see cmb_item_TextChanged), so this is the only place
        // the complete set of items is kept.
        private List<ItemPickerOption> _allItemOptions = new List<ItemPickerOption>();

        // Guards against the DataSource swap inside cmb_item_TextChanged re-triggering
        // TextChanged/SelectedIndexChanged while we're still in the middle of applying it.
        private bool _suppressItemComboEvents = false;

        public ItemStockAddModal()
        {
            InitializeComponent();
        }

        private async void ItemStockAddModal_Load(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                await LoadItems();

                var warehouseData = await WarehouseNameServices.GetWarehouseInfos();
                cmb_warehouse.DataSource = warehouseData?.warehouse_name ?? new List<WarehouseNameModel>();

                var uomTable = await UnitOfMeasurementServices.GetAsDatatable();
                cmb_uom.DisplayMember = "name";
                cmb_uom.ValueMember = "id";
                cmb_uom.DataSource = uomTable;
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error loading items/warehouses: {ex.Message}");
            }
            finally
            {
                this.Enabled = true;
            }
        }

        // Bin locations are defined per-warehouse in the Warehouse Setup module
        // (tbl_inv_warehouse_area, edited via frm_warehouse_name_setup), not typed freely -
        // whenever the warehouse changes, reload that warehouse's bins into cmb_bin_location.
        private async void cmb_warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_bin_location.Items.Clear();
            cmb_bin_location.Text = string.Empty;

            if (!(cmb_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse)) return;

            try
            {
                var areaService = new GeneralService<ReceivingWarehouseAreaView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE_AREA + selectedWarehouse.id);
                var areas = await areaService.GetAsList() ?? new List<ReceivingWarehouseAreaView>();

                // Same "zone-area-rack-level-bins" assembly ReceivingReport2 already uses,
                // so values picked here match the format already written elsewhere.
                var binOptions = areas
                    .Select(a => string.Join("-", new[] { a.zone, a.area, a.rack, a.level, a.bins }.Where(p => !string.IsNullOrWhiteSpace(p))))
                    .Where(b => !string.IsNullOrWhiteSpace(b))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(b => b, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                cmb_bin_location.Items.AddRange(binOptions);
            }
            catch (Exception)
            {
                // Don't block the modal on this - a warehouse with no bin-area setup yet
                // (or a transient API hiccup) just leaves cmb_bin_location empty, and the
                // user can still type a bin location manually since it's an editable combo.
            }
        }

        private async Task LoadItems(string keepSelectedItemCode = null)
        {
            // ItemServices.GetName()/GetAsDataTable() assume the API returns a flat array,
            // but /setup/item/ actually wraps everything in an object -
            // { items: [...], itemspecs: [...], ... } (the Items class already defined in
            // ItemModel.cs) - calling this directly instead of trusting those helpers.
            var itemsResponse = await RequestToApi<ApiResponseModel<Items>>.Get(ENUM_ENDPOINT.ITEM);
            var items = itemsResponse?.Data?.items ?? new List<ItemModel>();

            _allItemOptions = items
                .Where(i => i != null)
                .Select(i => new ItemPickerOption { Model = i })
                .OrderBy(o => o.Model.item_code)
                .ToList();

            BindItemComboSource(_allItemOptions);

            if (!string.IsNullOrEmpty(keepSelectedItemCode))
            {
                var match = _allItemOptions.FirstOrDefault(o => o.Model.item_code == keepSelectedItemCode);
                if (match != null) cmb_item.SelectedItem = match;
            }
        }

        private void BindItemComboSource(List<ItemPickerOption> options)
        {
            _suppressItemComboEvents = true;
            try
            {
                var typedText = cmb_item.Text;
                var selectionStart = cmb_item.SelectionStart;

                cmb_item.DataSource = null;
                cmb_item.DisplayMember = "Display";
                cmb_item.DataSource = options;

                // Re-assigning DataSource resets/clears Text - put back exactly what the
                // user had typed so filtering doesn't fight their cursor.
                cmb_item.Text = typedText;
                cmb_item.SelectionStart = selectionStart;
                cmb_item.SelectionLength = 0;
            }
            finally
            {
                _suppressItemComboEvents = false;
            }
        }

        // Matches a typed prefix against item_code, item_name, OR item_model
        // independently, rather than only against the combined "Code - Name - Model"
        // Display string - so searching can start from whichever field the user has in
        // mind (code, item/name, or model), not just the code.
        private void cmb_item_TextChanged(object sender, EventArgs e)
        {
            if (_suppressItemComboEvents) return;

            var text = cmb_item.Text;

            // Picking an item from the dropdown fires SelectedIndexChanged first, which
            // sets cmb_item.Text to that item's full "Code - Name - Model" Display string -
            // that assignment then fires THIS handler again. Without this check, we'd
            // re-filter for a code/name/model that starts with the entire combined string,
            // which never matches anything, rebinding cmb_item to an empty list and
            // silently wiping out the selection the user just made (this is what made
            // picking by model - or really any pick - fail to "take").
            if (cmb_item.SelectedItem is ItemPickerOption selected && selected.Display == text)
            {
                return;
            }

            var filtered = string.IsNullOrWhiteSpace(text)
                ? _allItemOptions
                : _allItemOptions.Where(o =>
                    StartsWithIgnoreCase(o.Model.item_code, text) ||
                    StartsWithIgnoreCase(o.Model.item_name, text) ||
                    StartsWithIgnoreCase(o.Model.item_model, text)
                ).ToList();

            BindItemComboSource(filtered);

            if (!string.IsNullOrEmpty(text) && filtered.Count > 0)
            {
                cmb_item.DroppedDown = true;
            }
        }

        private static bool StartsWithIgnoreCase(string value, string prefix)
        {
            return !string.IsNullOrEmpty(value) && value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        private async void lnk_new_item_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var itemEntryHost = new ItemEntryHostForm())
            {
                itemEntryHost.ShowDialog(this);
            }

            // Refresh regardless of whether a new item was actually saved - cheap, and
            // guarantees a newly created item shows up immediately without a manual reopen.
            try
            {
                this.Enabled = false;
                Helpers.Loading.ShowLoading(cmb_item, "Refreshing item list...");
                await LoadItems();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error refreshing item list: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(cmb_item);
                this.Enabled = true;
            }
        }

        private void cmb_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressItemComboEvents) return;

            if (cmb_item.SelectedItem is ItemPickerOption selected)
            {
                // Auto-select the item's default UOM by id (unit_of_measure_id is the real
                // FK; unit_of_measure is just its joined display name). Falls back to no
                // selection if the id isn't in the loaded list yet or doesn't match -
                // WinForms just leaves SelectedIndex at -1 rather than throwing, so the
                // user can still pick manually.
                if (cmb_uom.DataSource != null)
                {
                    cmb_uom.SelectedValue = selected.Model.unit_of_measure_id;
                }
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!(cmb_item.SelectedItem is ItemPickerOption selectedItem))
            {
                MessageBox.Show("Please select an item.", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cmb_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse))
            {
                MessageBox.Show("Please select a warehouse.", "Warehouse Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cmb_bin_location.Text))
            {
                MessageBox.Show("Please enter a bin location.", "Bin Location Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_bin_location.Focus();
                return;
            }

            if (num_qty.Value <= 0)
            {
                MessageBox.Show("Quantity to add must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmb_uom.SelectedIndex < 0 || cmb_uom.SelectedValue == null)
            {
                MessageBox.Show("Please select a unit of measure.", "UOM Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ItemId = selectedItem.Model.id;
            WarehouseId = selectedWarehouse.id;
            BinLocation = cmb_bin_location.Text.Trim().ToUpper();
            Qty = (int)num_qty.Value;
            Uom = cmb_uom.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
