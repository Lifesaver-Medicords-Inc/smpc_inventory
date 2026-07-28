using System;
using System.Windows.Forms;
using smpc_inventory_app.Model;

namespace smpc_inventory_app.Pages.Inventory
{
    // Small prompt used by ItemStocksPage's Adjust Stock action - asks for the new counted
    // quantity and a required reason, since AdjustItemStock (Go side) sets stock_qty
    // directly rather than adding/subtracting a delta, and the audit trail's only record of
    // "why" is whatever gets typed here.
    public partial class ItemStockAdjustmentModal : Form
    {
        private readonly ItemStockModel _current;

        public int NewQty { get; private set; }
        public string Remarks { get; private set; }

        public ItemStockAdjustmentModal(ItemStockModel current)
        {
            InitializeComponent();
            _current = current;

            lbl_info.Text =
                $"Item: {current.item_code} - {current.item_name}\n" +
                $"Warehouse: {current.warehouse_name}\n" +
                $"Bin: {current.bin_location}\n" +
                $"Current Stock Qty: {current.stock_qty} {current.stock_uom}";

            num_new_qty.Value = current.stock_qty >= 0 && current.stock_qty <= num_new_qty.Maximum
                ? current.stock_qty
                : 0;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_remarks.Text))
            {
                MessageBox.Show("Please provide a reason for this adjustment (e.g. physical count, damaged goods).",
                    "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_remarks.Focus();
                return;
            }

            if (num_new_qty.Value == _current.stock_qty)
            {
                MessageBox.Show("New quantity is the same as the current stock - nothing to adjust.",
                    "No Change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewQty = (int)num_new_qty.Value;
            Remarks = txt_remarks.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
