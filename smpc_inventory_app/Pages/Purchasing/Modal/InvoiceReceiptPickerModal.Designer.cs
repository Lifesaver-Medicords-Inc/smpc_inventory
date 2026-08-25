
namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    partial class InvoiceReceiptPickerModal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbl_title = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.dgv_ir = new System.Windows.Forms.DataGridView();
            this.col_ir_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_doc_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_supplier_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_doc_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_select = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ir)).BeginInit();
            this.SuspendLayout();
            //
            // lbl_title
            //
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(12, 12);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(180, 21);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Select Invoice Receipt";
            //
            // txt_search
            //
            this.txt_search.Location = new System.Drawing.Point(12, 44);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(560, 20);
            this.txt_search.TabIndex = 1;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            //
            // dgv_ir
            //
            this.dgv_ir.AllowUserToAddRows = false;
            this.dgv_ir.AutoGenerateColumns = false;
            this.dgv_ir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ir.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_ir_id,
            this.col_doc_no,
            this.col_supplier_code,
            this.col_supplier,
            this.col_doc_date});
            this.dgv_ir.Location = new System.Drawing.Point(12, 70);
            this.dgv_ir.MultiSelect = false;
            this.dgv_ir.Name = "dgv_ir";
            this.dgv_ir.ReadOnly = true;
            this.dgv_ir.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgv_ir.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_ir.Size = new System.Drawing.Size(560, 340);
            this.dgv_ir.TabIndex = 2;
            this.dgv_ir.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_ir_CellDoubleClick);
            //
            // col_ir_id
            //
            this.col_ir_id.HeaderText = "id";
            this.col_ir_id.Name = "col_ir_id";
            this.col_ir_id.Visible = false;
            //
            // col_doc_no
            //
            this.col_doc_no.HeaderText = "IR#";
            this.col_doc_no.Name = "col_doc_no";
            //
            // col_supplier_code
            //
            this.col_supplier_code.HeaderText = "SUPPLIER CODE";
            this.col_supplier_code.Name = "col_supplier_code";
            //
            // col_supplier
            //
            this.col_supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_supplier.HeaderText = "SUPPLIER";
            this.col_supplier.Name = "col_supplier";
            //
            // col_doc_date
            //
            this.col_doc_date.HeaderText = "DOC DATE";
            this.col_doc_date.Name = "col_doc_date";
            //
            // btn_select
            //
            this.btn_select.Location = new System.Drawing.Point(416, 418);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(75, 27);
            this.btn_select.TabIndex = 3;
            this.btn_select.Text = "Select";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            //
            // btn_cancel
            //
            this.btn_cancel.Location = new System.Drawing.Point(497, 418);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 27);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            //
            // InvoiceReceiptPickerModal
            //
            this.AcceptButton = this.btn_select;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.dgv_ir);
            this.Controls.Add(this.txt_search);
            this.Controls.Add(this.lbl_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvoiceReceiptPickerModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Invoice Receipt";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ir)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridView dgv_ir;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_ir_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_doc_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_supplier_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_doc_date;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.Button btn_cancel;
    }
}
