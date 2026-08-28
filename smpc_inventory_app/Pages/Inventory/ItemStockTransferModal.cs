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
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Services.Setup.Warehouse;

namespace smpc_inventory_app.Pages.Inventory
{
    // "Transfer Stock" modal for ItemStocksPage - §10.6's Transfer function. Moves some
    // or all of the selected row's quantity to a different bin, warehouse-to-warehouse
    // moves included. Deliberately no reference-document field anywhere here - Stock
    // Transfer is the one stock movement in Lightspeed with no document behind it.
    public partial class ItemStockTransferModal : Form
    {
        private readonly ItemStockModel _current;

        public int TransferQty { get; private set; }
        public int DestWarehouseId { get; private set; }
        public string DestBinLocation { get; private set; }
        public string Remarks { get; private set; }

        // Cascading zone->area->rack->level->bins picker, same staged behavior as
        // Receiving Report's bin_location column (BinLocationComboOverlay) - this
        // used to flatten every combination into one long dropdown instead.
        private readonly CascadingBinLocationCombo _destBinLocationPicker;

        public ItemStockTransferModal(ItemStockModel current)
        {
            InitializeComponent();
            _destBinLocationPicker = new CascadingBinLocationCombo(cmb_dest_bin_location);
            _current = current;

            lbl_info.Text =
                $"Item: {current.item_code} - {current.item_name}\n" +
                $"From Warehouse: {current.warehouse_name}\n" +
                $"From Bin: {current.bin_location}\n" +
                $"Available Stock Qty: {current.stock_qty} {current.stock_uom}";

            num_transfer_qty.Maximum = current.stock_qty > 0 ? current.stock_qty : 1;
        }

        private async void ItemStockTransferModal_Load(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                var warehouseData = await WarehouseNameServices.GetWarehouseInfos();
                cmb_dest_warehouse.DataSource = warehouseData?.warehouse_name ?? new List<WarehouseNameModel>();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error loading warehouses: {ex.Message}");
            }
            finally
            {
                this.Enabled = true;
            }
        }

        // Reloads the destination picker's area data, same zone->area->rack->level->bins
        // staged flow Receiving Report uses (§10.6) - matches ItemStockAddModal's own
        // cmb_warehouse_SelectedIndexChanged handling.
        private async void cmb_dest_warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            _destBinLocationPicker.Clear();

            if (!(cmb_dest_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse)) return;

            try
            {
                var areaService = new GeneralService<ReceivingWarehouseAreaView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE_AREA + selectedWarehouse.id);
                var areas = await areaService.GetAsList() ?? new List<ReceivingWarehouseAreaView>();
                _destBinLocationPicker.SetData(areas);
            }
            catch (Exception)
            {
                // A warehouse with no bin-area setup yet, or a transient API hiccup, just
                // leaves the picker showing no options, same as Receiving Report's own
                // behavior for an unconfigured warehouse.
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!(cmb_dest_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse))
            {
                MessageBox.Show("Please select a destination warehouse.", "Destination Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_destBinLocationPicker.Value))
            {
                MessageBox.Show("Please select a destination bin location.", "Destination Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_dest_bin_location.Focus();
                return;
            }

            string destBin = _destBinLocationPicker.Value.Trim().ToUpper();

            if (selectedWarehouse.id == _current.warehouse_id
                && string.Equals(destBin, (_current.bin_location ?? "").Trim().ToUpper(), StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Source and destination are the same location - nothing to transfer.",
                    "Same Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (num_transfer_qty.Value <= 0 || num_transfer_qty.Value > _current.stock_qty)
            {
                MessageBox.Show($"Quantity must be between 1 and the available {_current.stock_qty} unit(s).",
                    "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_remarks.Text))
            {
                MessageBox.Show("Please provide a reason for this transfer.",
                    "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_remarks.Focus();
                return;
            }

            TransferQty = (int)num_transfer_qty.Value;
            DestWarehouseId = selectedWarehouse.id;
            DestBinLocation = destBin;
            Remarks = txt_remarks.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
