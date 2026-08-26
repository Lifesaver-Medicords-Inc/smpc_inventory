using System;
using System.Linq;
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
    // §5.23's Warehouse Manager acknowledgement step - only asks for the destination
    // (item and quantity are already fixed by the Job Order being acknowledged, unlike
    // ItemStockAddModal which picks all of item/warehouse/bin/qty/uom).
    public partial class ProductionAcknowledgeModal : Form
    {
        private readonly ProductionReportModel _report;

        public int WarehouseId { get; private set; }
        public string BinLocation { get; private set; }

        public ProductionAcknowledgeModal(ProductionReportModel report)
        {
            InitializeComponent();
            _report = report;
        }

        private async void ProductionAcknowledgeModal_Load(object sender, EventArgs e)
        {
            lbl_summary.Text = $"{_report.sales_order}  -  {_report.item_desc}  -  Qty {_report.quantity}"
                + (string.IsNullOrWhiteSpace(_report.serial_no) ? "" : $"\nSerial No.: {_report.serial_no}");

            try
            {
                this.Enabled = false;
                var warehouseData = await WarehouseNameServices.GetWarehouseInfos();
                cmb_warehouse.DataSource = warehouseData?.warehouse_name ?? new System.Collections.Generic.List<WarehouseNameModel>();
                cmb_warehouse.DisplayMember = "name";
                cmb_warehouse.SelectedIndex = -1;
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

        // Same "zone-area-rack-level-bins" assembly ItemStockAddModal already uses, so
        // values picked here match the format written elsewhere.
        private async void cmb_warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_bin_location.Items.Clear();
            cmb_bin_location.Text = string.Empty;

            if (!(cmb_warehouse.SelectedItem is WarehouseNameModel selectedWarehouse)) return;

            try
            {
                var areaService = new GeneralService<ReceivingWarehouseAreaView>(ENUM_ENDPOINT.RECEIVING_REPORT_WAREHOUSE_AREA + selectedWarehouse.id);
                var areas = await areaService.GetAsList() ?? new System.Collections.Generic.List<ReceivingWarehouseAreaView>();

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
                // Same tolerance as ItemStockAddModal - a warehouse with no bin-area setup
                // yet just leaves the combo empty; it's editable so the user can still type.
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
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

            WarehouseId = selectedWarehouse.id;
            BinLocation = cmb_bin_location.Text.Trim().ToUpper();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
