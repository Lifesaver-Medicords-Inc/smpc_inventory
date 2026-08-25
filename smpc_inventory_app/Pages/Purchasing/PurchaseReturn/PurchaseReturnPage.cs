using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Model;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Purchasing;

namespace smpc_inventory_app.Pages.Purchasing.PurchaseReturn
{
    // Purchase Return (PRT#), spec section 5.8. References an Invoice Receipt,
    // never a PO - this app has no IR data of its own, so the whole
    // invoice_receipt list is fetched once (ERP_API's GetInvoiceReceipt takes
    // no filter conditions) and reused both for the picker and for populating
    // the line grid from whichever IR gets picked.
    //
    // NOT YET WIRED: btn_search is a no-op (Prev/Next already page through
    // everything loaded), and the actual stock-decrease-on-release effect -
    // approving here only flips the flag server-side (see
    // PurchaseReturnService.ApprovePurchaseReturn's own comment on the Go
    // side) pending its own pass against the live accounting/inventory code.
    public partial class PurchaseReturnPage : UserControl
    {
        private readonly PurchaseReturnService _service = new PurchaseReturnService();
        private List<PurchaseReturnModel> _records = new List<PurchaseReturnModel>();
        private List<PurchaseReturnDetailsModel> _allDetails = new List<PurchaseReturnDetailsModel>();
        private int _currentIndex = -1;

        private List<InvoiceReceiptModel> _invoiceReceipts = new List<InvoiceReceiptModel>();
        private List<InvoiceReceiptDetailsModel> _invoiceReceiptDetails = new List<InvoiceReceiptDetailsModel>();

        private static readonly string[] AlwaysReadOnlyFields = {
            "txt_document_no", "txt_supplier_code", "txt_supplier_name",
            "txt_doc_date", "txt_ref_ir_no", "txt_ref_dm_no",
            "txt_approved_by", "txt_approval_date"
        };

        public PurchaseReturnPage()
        {
            InitializeComponent();

            dgv_main.AutoGenerateColumns = false;

            btn_new.Click += btn_new_Click;
            btn_search.Click += btn_search_Click;
            btn_prev.Click += btn_prev_Click;
            btn_next.Click += btn_next_Click;
            btn_edit.Click += btn_edit_Click;
            btn_save.Click += btn_save_Click;
            btn_cancel.Click += btn_cancel_Click;
            btn_approve.Click += btn_approve_Click;
            txt_ref_ir_no.Click += txt_ref_ir_no_Click;

            SetEditMode(false);
            this.Load += PurchaseReturnPage_Load;
        }

        private async void PurchaseReturnPage_Load(object sender, EventArgs e)
        {
            await LoadInvoiceReceiptsAsync();
            await LoadRecordsAsync();
        }

        private async Task LoadInvoiceReceiptsAsync()
        {
            try
            {
                var irService = new GeneralService<InvoiceReceiptList>(ENUM_ENDPOINT.INVOICE_RECEIPT);
                var data = await irService.GetAsModel();
                _invoiceReceipts = data?.invoice_receipt ?? new List<InvoiceReceiptModel>();
                _invoiceReceiptDetails = data?.invoice_receipt_details ?? new List<InvoiceReceiptDetailsModel>();
            }
            catch (Exception)
            {
                _invoiceReceipts = new List<InvoiceReceiptModel>();
                _invoiceReceiptDetails = new List<InvoiceReceiptDetailsModel>();
            }
        }

        private async Task LoadRecordsAsync()
        {
            Helpers.Loading.ShowLoading(pnl_main, "Fetching data...");
            try
            {
                var data = await _service.GetAsModel();
                _records = (data?.purchase_return ?? new List<PurchaseReturnModel>())
                    .OrderByDescending(r => r.doc_no)
                    .ToList();
                _allDetails = data?.purchase_return_details ?? new List<PurchaseReturnDetailsModel>();
                _currentIndex = _records.Count > 0 ? 0 : -1;
                ShowCurrentRecord();
            }
            catch (Exception)
            {
                Helpers.ShowDialogMessage("error", "Failed to load purchase returns.");
            }
            finally
            {
                Helpers.Loading.HideLoading(pnl_main);
            }
        }

        private void ShowCurrentRecord()
        {
            ClearForm();
            UpdateNavButtons();

            if (_currentIndex < 0 || _currentIndex >= _records.Count) return;

            var prt = _records[_currentIndex];
            txt_document_no.Text = prt.doc_no.ToString();
            txt_supplier_id.Text = prt.supplier_id.ToString();
            txt_supplier_code.Text = prt.supplier_code;
            txt_supplier_name.Text = prt.supplier_name;
            txt_ref_ir_id.Text = prt.ref_ir_id.ToString();
            txt_ref_ir_no.Text = prt.ref_ir_no;
            txt_doc_date.Text = prt.doc_date;
            cmb_return_type.Text = prt.return_type;
            txt_ref_dm_no.Text = prt.ref_dm_no;
            txt_remarks.Text = prt.remarks;
            txt_approved_by.Text = prt.approved_by_name;
            txt_approval_date.Text = prt.approval_date;
            btn_approve.Visible = !prt.is_approved;

            dgv_main.Rows.Clear();
            foreach (var d in _allDetails.Where(x => x.purchase_return_id == prt.id))
            {
                AddDetailRow(d.id, d.ref_ir_details_id, d.item_id, d.item_code, d.description, d.unit_of_measure, null, d.qty, d.unit_cost, d.reason);
            }
        }

        private void AddDetailRow(int? id, int refIrDetailsId, int itemId, string itemCode, string description, string uom, int? receivedQty, int qty, double unitCost, string reason)
        {
            int rowIndex = dgv_main.Rows.Add();
            var row = dgv_main.Rows[rowIndex];
            row.Cells["col_details_id"].Value = id;
            row.Cells["col_ref_ir_details_id"].Value = refIrDetailsId;
            row.Cells["col_item_id"].Value = itemId;
            row.Cells["col_item_code"].Value = itemCode;
            row.Cells["col_description"].Value = description;
            row.Cells["col_uom"].Value = uom;
            row.Cells["col_req_qty"].Value = receivedQty;
            row.Cells["col_qty"].Value = qty;
            row.Cells["col_unit_cost"].Value = unitCost;
            row.Cells["col_reason"].Value = reason;
        }

        private void ClearForm()
        {
            txt_document_no.Text = "";
            txt_supplier_id.Text = "";
            txt_supplier_code.Text = "";
            txt_supplier_name.Text = "";
            txt_ref_ir_id.Text = "";
            txt_ref_ir_no.Text = "";
            txt_doc_date.Text = "";
            cmb_return_type.SelectedIndex = -1;
            txt_ref_dm_no.Text = "";
            txt_remarks.Text = "";
            txt_approved_by.Text = "";
            txt_approval_date.Text = "";
            dgv_main.Rows.Clear();
            btn_approve.Visible = false;
        }

        private void UpdateNavButtons()
        {
            btn_prev.Enabled = _currentIndex > 0;
            btn_next.Enabled = _currentIndex >= 0 && _currentIndex < _records.Count - 1;
        }

        private void SetEditMode(bool enable)
        {
            Helpers.SetButtonVisibility(toolStrip1, panel_top,
                visibleButtons: enable
                    ? new[] { "btn_save", "btn_cancel" }
                    : new[] { "btn_new", "btn_search", "btn_prev", "btn_next", "btn_edit" },
                hiddenButtons: enable
                    ? new[] { "btn_new", "btn_search", "btn_prev", "btn_next", "btn_edit", "btn_approve" }
                    : new[] { "btn_save", "btn_cancel" });

            Helpers.SetChildControlsEnabled2(new System.Windows.Forms.Control[] { pnl_header }, !enable, AlwaysReadOnlyFields);
            dgv_main.ReadOnly = !enable;

            if (!enable && _currentIndex >= 0 && _currentIndex < _records.Count)
                btn_approve.Visible = !_records[_currentIndex].is_approved;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            _currentIndex = -1;
            ClearForm();
            SetEditMode(true);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (_currentIndex < 0) return;
            SetEditMode(true);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            if (_currentIndex < 0 && _records.Count > 0) _currentIndex = 0;
            ShowCurrentRecord();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (_currentIndex > 0) { _currentIndex--; ShowCurrentRecord(); }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (_currentIndex < _records.Count - 1) { _currentIndex++; ShowCurrentRecord(); }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            // A dedicated search modal (by supplier/doc no) is the natural next
            // step - not wired in this pass; PREV/NEXT already page through every
            // record loaded.
        }

        private void txt_ref_ir_no_Click(object sender, EventArgs e)
        {
            if (!btn_save.Visible) return; // only pickable while in edit mode

            using (var modal = new InvoiceReceiptPickerModal(_invoiceReceipts))
            {
                if (modal.ShowDialog(this.FindForm()) != DialogResult.OK) return;

                var ir = _invoiceReceipts.FirstOrDefault(x => x.id == modal.SelectedInvoiceReceiptId);
                if (ir == null) return;

                txt_ref_ir_id.Text = ir.id.ToString();
                txt_ref_ir_no.Text = ir.doc_no.ToString();
                txt_doc_date.Text = ir.doc_date;
                txt_supplier_id.Text = ir.supplier_id.ToString();
                txt_supplier_code.Text = ir.supplier_code;
                txt_supplier_name.Text = ir.supplier;

                // Every line on the picked IR becomes a candidate return line -
                // qty defaults to 0 (not part of the return) until the user fills
                // one in; spec 5.8 - "the item list may span multiple POs" since
                // one IR can cover several, but the line set itself is exactly
                // what that IR received.
                dgv_main.Rows.Clear();
                foreach (var d in _invoiceReceiptDetails.Where(x => x.invoice_receipt_id == ir.id))
                {
                    AddDetailRow(null, d.id, 0, d.item_code, d.item_description, d.req_uom, d.req_qty, 0, d.unit_price, "");
                }
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            dgv_main.EndEdit();

            if (!int.TryParse(txt_ref_ir_id.Text, out int refIrId) || refIrId == 0)
            {
                MessageBox.Show("Select a reference Invoice Receipt first.", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmb_return_type.SelectedIndex <= 0)
            {
                MessageBox.Show("TYPE is required.", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var details = new List<PurchaseReturnDetailsModel>();
            foreach (DataGridViewRow row in dgv_main.Rows)
            {
                if (row.IsNewRow) continue;
                int.TryParse(row.Cells["col_qty"].Value?.ToString(), out int qty);
                if (qty <= 0) continue; // 0 (the default) means this line isn't part of the return

                int.TryParse(row.Cells["col_ref_ir_details_id"].Value?.ToString(), out int refIrDetailsId);
                int.TryParse(row.Cells["col_item_id"].Value?.ToString(), out int itemId);
                double.TryParse(row.Cells["col_unit_cost"].Value?.ToString(), out double unitCost);

                details.Add(new PurchaseReturnDetailsModel
                {
                    ref_ir_details_id = refIrDetailsId,
                    item_id = itemId,
                    item_code = row.Cells["col_item_code"].Value?.ToString(),
                    description = row.Cells["col_description"].Value?.ToString(),
                    unit_of_measure = row.Cells["col_uom"].Value?.ToString(),
                    qty = qty,
                    unit_cost = unitCost,
                    reason = row.Cells["col_reason"].Value?.ToString(),
                });
            }

            if (details.Count == 0)
            {
                MessageBox.Show("Enter a qty to return on at least one line.", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = new PurchaseReturnPayload
            {
                purchase_return = new PurchaseReturnModel
                {
                    supplier_id = int.TryParse(txt_supplier_id.Text, out int supId) ? supId : 0,
                    supplier_code = txt_supplier_code.Text,
                    supplier_name = txt_supplier_name.Text,
                    ref_ir_id = refIrId,
                    return_type = cmb_return_type.Text,
                    remarks = txt_remarks.Text,
                },
                purchase_return_details = details,
            };

            Helpers.Loading.ShowLoading(pnl_main, "Saving data...");
            try
            {
                var response = await _service.CreatePurchaseReturn(payload);
                if (response == null || !response.Success)
                {
                    Helpers.ShowDialogMessage("error", response?.message ?? "Failed to save.");
                    return;
                }

                Helpers.ShowDialogMessage("success", "Purchase return saved.");
                SetEditMode(false);
                await LoadRecordsAsync();
            }
            finally
            {
                Helpers.Loading.HideLoading(pnl_main);
            }
        }

        private async void btn_approve_Click(object sender, EventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _records.Count) return;

            var confirm = MessageBox.Show(
                "Approve this Purchase Return? Only CBDO may do this.",
                "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            btn_approve.Enabled = false;
            try
            {
                var response = await _service.ApprovePurchaseReturn(_records[_currentIndex].id ?? 0);
                if (response == null || !response.Success)
                {
                    Helpers.ShowDialogMessage("error", response?.message ?? "Failed to approve.");
                    return;
                }
                await LoadRecordsAsync();
            }
            finally
            {
                btn_approve.Enabled = true;
            }
        }
    }
}
