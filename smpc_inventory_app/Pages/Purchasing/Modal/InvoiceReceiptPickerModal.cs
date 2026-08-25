using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using smpc_inventory_app.Model;

namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    // Purchase Return references an Invoice Receipt, never a PO (spec section
    // 5.8) - this app has no IR data of its own, so the caller fetches the
    // whole (unfiltered - ERP_API's GetInvoiceReceipt takes no conditions)
    // invoice_receipt list once and hands it here rather than this modal
    // making its own redundant round trip.
    public partial class InvoiceReceiptPickerModal : Form
    {
        public int SelectedInvoiceReceiptId { get; private set; }

        private readonly List<InvoiceReceiptModel> _allInvoiceReceipts;

        public InvoiceReceiptPickerModal(List<InvoiceReceiptModel> invoiceReceipts)
        {
            InitializeComponent();

            _allInvoiceReceipts = invoiceReceipts ?? new List<InvoiceReceiptModel>();

            foreach (var ir in _allInvoiceReceipts.OrderByDescending(x => x.doc_no))
            {
                int rowIndex = dgv_ir.Rows.Add();
                var row = dgv_ir.Rows[rowIndex];
                row.Cells["col_ir_id"].Value = ir.id;
                row.Cells["col_doc_no"].Value = ir.doc_no;
                row.Cells["col_supplier_code"].Value = ir.supplier_code;
                row.Cells["col_supplier"].Value = ir.supplier;
                row.Cells["col_doc_date"].Value = ir.doc_date;
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string search = txt_search.Text.Trim();

            foreach (DataGridViewRow row in dgv_ir.Rows)
            {
                if (string.IsNullOrEmpty(search))
                {
                    row.Visible = true;
                    continue;
                }

                string docNo = row.Cells["col_doc_no"].Value?.ToString() ?? "";
                string supplierCode = row.Cells["col_supplier_code"].Value?.ToString() ?? "";
                string supplier = row.Cells["col_supplier"].Value?.ToString() ?? "";

                row.Visible = docNo.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                           || supplierCode.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                           || supplier.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void dgv_ir_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SelectCurrentRowAndClose();
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            SelectCurrentRowAndClose();
        }

        private void SelectCurrentRowAndClose()
        {
            if (dgv_ir.CurrentRow == null)
            {
                MessageBox.Show("Select an Invoice Receipt first.", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.TryParse(dgv_ir.CurrentRow.Cells["col_ir_id"].Value?.ToString(), out int id))
            {
                SelectedInvoiceReceiptId = id;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
