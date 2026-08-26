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

        public ItemStockTransferModal(ItemStockModel current)
        {
            InitializeComponent();
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

        // Same "zone-area-rack-level-bins" assembly as ItemStockAddModal's
        // cmb_warehouse_SelectedIndexChanged, so destination values here match the format
        // already written everywhere else.
        private async void cmb_dest_warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_dest_bin_location.Items.Clear();
            cmb_dest_bin_location.Text = string.Empty;

            if (!(cmb_dest_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse)) return;

            try
            {
                var areaService = new GeneralService<ReceivingWarehouseAreaView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE_AREA + selectedWarehouse.id);
                var areas = await areaService.GetAsList() ?? new List<ReceivingWarehouseAreaView>();

                var binOptions = areas
                    .Select(a => string.Join("-", new[] { a.zone, a.area, a.rack, a.level, a.bins }.Where(p => !string.IsNullOrWhiteSpace(p))))
                    .Where(b => !string.IsNullOrWhiteSpace(b))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(b => b, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                cmb_dest_bin_location.Items.AddRange(binOptions);
            }
            catch (Exception)
            {
                // A warehouse with no bin-area setup yet, or a transient API hiccup, just
                // leaves this empty - the user can still type a bin location manually
                // since it's an editable combo.
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!(cmb_dest_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse))
            {
                MessageBox.Show("Please select a destination warehouse.", "Destination Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cmb_dest_bin_location.Text))
            {
                MessageBox.Show("Please enter a destination bin location.", "Destination Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_dest_bin_location.Focus();
                return;
            }

            string destBin = cmb_dest_bin_location.Text.Trim().ToUpper();

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
