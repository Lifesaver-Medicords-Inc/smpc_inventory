
namespace smpc_inventory_app.Pages.Purchasing.PurchaseReturn
{
    partial class PurchaseReturnPage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnl_main = new System.Windows.Forms.Panel();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.col_details_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_ref_ir_details_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_req_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_unit_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_top = new System.Windows.Forms.Panel();
            this.pnl_header = new System.Windows.Forms.Panel();
            this.lbl_supplier_code = new System.Windows.Forms.Label();
            this.txt_supplier_code = new System.Windows.Forms.TextBox();
            this.txt_supplier_id = new System.Windows.Forms.TextBox();
            this.lbl_doc_no = new System.Windows.Forms.Label();
            this.txt_document_no = new System.Windows.Forms.TextBox();
            this.lbl_supplier_name = new System.Windows.Forms.Label();
            this.txt_supplier_name = new System.Windows.Forms.TextBox();
            this.lbl_ref_ir_no = new System.Windows.Forms.Label();
            this.txt_ref_ir_no = new System.Windows.Forms.TextBox();
            this.txt_ref_ir_id = new System.Windows.Forms.TextBox();
            this.lbl_doc_date = new System.Windows.Forms.Label();
            this.txt_doc_date = new System.Windows.Forms.TextBox();
            this.lbl_return_type = new System.Windows.Forms.Label();
            this.cmb_return_type = new System.Windows.Forms.ComboBox();
            this.lbl_ref_dm_no = new System.Windows.Forms.Label();
            this.txt_ref_dm_no = new System.Windows.Forms.TextBox();
            this.lbl_approved_by = new System.Windows.Forms.Label();
            this.txt_approved_by = new System.Windows.Forms.TextBox();
            this.lbl_approval_date = new System.Windows.Forms.Label();
            this.txt_approval_date = new System.Windows.Forms.TextBox();
            this.lbl_remarks = new System.Windows.Forms.Label();
            this.txt_remarks = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.btn_approve = new System.Windows.Forms.ToolStripButton();
            this.lbl_title = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnl_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            this.panel_top.SuspendLayout();
            this.pnl_header.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.pnl_main);
            this.panel1.Controls.Add(this.panel_top);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1229, 900);
            this.panel1.TabIndex = 0;
            //
            // pnl_main
            //
            this.pnl_main.Controls.Add(this.dgv_main);
            this.pnl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_main.Location = new System.Drawing.Point(0, 258);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Padding = new System.Windows.Forms.Padding(8);
            this.pnl_main.Size = new System.Drawing.Size(1229, 642);
            this.pnl_main.TabIndex = 1;
            //
            // dgv_main
            //
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AutoGenerateColumns = false;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_details_id,
            this.col_ref_ir_details_id,
            this.col_item_id,
            this.col_item_code,
            this.col_description,
            this.col_uom,
            this.col_req_qty,
            this.col_qty,
            this.col_unit_cost,
            this.col_reason});
            this.dgv_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_main.Location = new System.Drawing.Point(8, 8);
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgv_main.Size = new System.Drawing.Size(1213, 626);
            this.dgv_main.TabIndex = 0;
            //
            // col_details_id
            //
            this.col_details_id.HeaderText = "id";
            this.col_details_id.Name = "col_details_id";
            this.col_details_id.Visible = false;
            //
            // col_ref_ir_details_id
            //
            // Pins to a specific Invoice Receipt LINE, not the header - spec 5.8's own
            // warning: matching at the header/PO level is "where the mistake happens".
            this.col_ref_ir_details_id.HeaderText = "ref_ir_details_id";
            this.col_ref_ir_details_id.Name = "col_ref_ir_details_id";
            this.col_ref_ir_details_id.Visible = false;
            //
            // col_item_id
            //
            this.col_item_id.HeaderText = "item_id";
            this.col_item_id.Name = "col_item_id";
            this.col_item_id.Visible = false;
            //
            // col_item_code
            //
            this.col_item_code.HeaderText = "ITEM CODE";
            this.col_item_code.Name = "col_item_code";
            this.col_item_code.ReadOnly = true;
            //
            // col_description
            //
            this.col_description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_description.HeaderText = "DESCRIPTION";
            this.col_description.Name = "col_description";
            this.col_description.ReadOnly = true;
            //
            // col_uom
            //
            this.col_uom.HeaderText = "UOM";
            this.col_uom.Name = "col_uom";
            this.col_uom.ReadOnly = true;
            //
            // col_req_qty
            //
            // Informational - the qty originally received on the referenced IR line.
            this.col_req_qty.HeaderText = "RECEIVED QTY";
            this.col_req_qty.Name = "col_req_qty";
            this.col_req_qty.ReadOnly = true;
            //
            // col_qty
            //
            // Qty being returned on this line - 0 (the default) means this line isn't
            // part of the return; only lines with a qty &gt; 0 get sent to the API.
            this.col_qty.HeaderText = "QTY TO RETURN";
            this.col_qty.Name = "col_qty";
            //
            // col_unit_cost
            //
            // Auto-filled from the matched IR line, never user-entered.
            this.col_unit_cost.HeaderText = "UNIT COST";
            this.col_unit_cost.Name = "col_unit_cost";
            this.col_unit_cost.ReadOnly = true;
            //
            // col_reason
            //
            this.col_reason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_reason.HeaderText = "REASON";
            this.col_reason.Name = "col_reason";
            //
            // panel_top
            //
            this.panel_top.Controls.Add(this.pnl_header);
            this.panel_top.Controls.Add(this.toolStrip1);
            this.panel_top.Controls.Add(this.lbl_title);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1229, 258);
            this.panel_top.TabIndex = 0;
            //
            // pnl_header
            //
            this.pnl_header.Controls.Add(this.lbl_supplier_code);
            this.pnl_header.Controls.Add(this.txt_supplier_code);
            this.pnl_header.Controls.Add(this.txt_supplier_id);
            this.pnl_header.Controls.Add(this.lbl_doc_no);
            this.pnl_header.Controls.Add(this.txt_document_no);
            this.pnl_header.Controls.Add(this.lbl_supplier_name);
            this.pnl_header.Controls.Add(this.txt_supplier_name);
            this.pnl_header.Controls.Add(this.lbl_ref_ir_no);
            this.pnl_header.Controls.Add(this.txt_ref_ir_no);
            this.pnl_header.Controls.Add(this.txt_ref_ir_id);
            this.pnl_header.Controls.Add(this.lbl_doc_date);
            this.pnl_header.Controls.Add(this.txt_doc_date);
            this.pnl_header.Controls.Add(this.lbl_return_type);
            this.pnl_header.Controls.Add(this.cmb_return_type);
            this.pnl_header.Controls.Add(this.lbl_ref_dm_no);
            this.pnl_header.Controls.Add(this.txt_ref_dm_no);
            this.pnl_header.Controls.Add(this.lbl_approved_by);
            this.pnl_header.Controls.Add(this.txt_approved_by);
            this.pnl_header.Controls.Add(this.lbl_approval_date);
            this.pnl_header.Controls.Add(this.txt_approval_date);
            this.pnl_header.Controls.Add(this.lbl_remarks);
            this.pnl_header.Controls.Add(this.txt_remarks);
            this.pnl_header.Location = new System.Drawing.Point(0, 47);
            this.pnl_header.Name = "pnl_header";
            this.pnl_header.Size = new System.Drawing.Size(1229, 211);
            this.pnl_header.TabIndex = 1;
            //
            // lbl_supplier_code
            //
            this.lbl_supplier_code.AutoSize = true;
            this.lbl_supplier_code.Location = new System.Drawing.Point(12, 15);
            this.lbl_supplier_code.Name = "lbl_supplier_code";
            this.lbl_supplier_code.Size = new System.Drawing.Size(78, 13);
            this.lbl_supplier_code.TabIndex = 0;
            this.lbl_supplier_code.Text = "SUPPLIER CODE";
            //
            // txt_supplier_code
            //
            // Read-only - populated from whichever Invoice Receipt is picked via
            // txt_ref_ir_no, never typed directly (spec 5.8 - a return is always tied
            // to a specific IR/supplier).
            this.txt_supplier_code.Location = new System.Drawing.Point(140, 12);
            this.txt_supplier_code.Name = "txt_supplier_code";
            this.txt_supplier_code.ReadOnly = true;
            this.txt_supplier_code.Size = new System.Drawing.Size(180, 20);
            this.txt_supplier_code.TabIndex = 1;
            //
            // txt_supplier_id
            //
            this.txt_supplier_id.Location = new System.Drawing.Point(140, 12);
            this.txt_supplier_id.Name = "txt_supplier_id";
            this.txt_supplier_id.Size = new System.Drawing.Size(180, 20);
            this.txt_supplier_id.TabIndex = 2;
            this.txt_supplier_id.Visible = false;
            //
            // lbl_doc_no
            //
            this.lbl_doc_no.AutoSize = true;
            this.lbl_doc_no.Location = new System.Drawing.Point(668, 15);
            this.lbl_doc_no.Name = "lbl_doc_no";
            this.lbl_doc_no.Size = new System.Drawing.Size(48, 13);
            this.lbl_doc_no.TabIndex = 3;
            this.lbl_doc_no.Text = "PRT#";
            //
            // txt_document_no
            //
            this.txt_document_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txt_document_no.Location = new System.Drawing.Point(796, 12);
            this.txt_document_no.Name = "txt_document_no";
            this.txt_document_no.ReadOnly = true;
            this.txt_document_no.Size = new System.Drawing.Size(180, 20);
            this.txt_document_no.TabIndex = 4;
            this.txt_document_no.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // lbl_supplier_name
            //
            this.lbl_supplier_name.AutoSize = true;
            this.lbl_supplier_name.Location = new System.Drawing.Point(12, 41);
            this.lbl_supplier_name.Name = "lbl_supplier_name";
            this.lbl_supplier_name.Size = new System.Drawing.Size(80, 13);
            this.lbl_supplier_name.TabIndex = 5;
            this.lbl_supplier_name.Text = "SUPPLIER NAME";
            //
            // txt_supplier_name
            //
            this.txt_supplier_name.Location = new System.Drawing.Point(140, 38);
            this.txt_supplier_name.Name = "txt_supplier_name";
            this.txt_supplier_name.ReadOnly = true;
            this.txt_supplier_name.Size = new System.Drawing.Size(180, 20);
            this.txt_supplier_name.TabIndex = 6;
            //
            // lbl_ref_ir_no
            //
            // Spec 5.8 - references an Invoice Receipt, never a PO.
            this.lbl_ref_ir_no.AutoSize = true;
            this.lbl_ref_ir_no.Location = new System.Drawing.Point(340, 41);
            this.lbl_ref_ir_no.Name = "lbl_ref_ir_no";
            this.lbl_ref_ir_no.Size = new System.Drawing.Size(65, 13);
            this.lbl_ref_ir_no.TabIndex = 7;
            this.lbl_ref_ir_no.Text = "REF. IR NO.";
            //
            // txt_ref_ir_no
            //
            // Click opens the Invoice Receipt picker - see PurchaseReturnPage.cs.
            this.txt_ref_ir_no.Location = new System.Drawing.Point(468, 38);
            this.txt_ref_ir_no.Name = "txt_ref_ir_no";
            this.txt_ref_ir_no.ReadOnly = true;
            this.txt_ref_ir_no.Size = new System.Drawing.Size(180, 20);
            this.txt_ref_ir_no.TabIndex = 8;
            //
            // txt_ref_ir_id
            //
            this.txt_ref_ir_id.Location = new System.Drawing.Point(468, 38);
            this.txt_ref_ir_id.Name = "txt_ref_ir_id";
            this.txt_ref_ir_id.Size = new System.Drawing.Size(180, 20);
            this.txt_ref_ir_id.TabIndex = 9;
            this.txt_ref_ir_id.Visible = false;
            //
            // lbl_doc_date
            //
            this.lbl_doc_date.AutoSize = true;
            this.lbl_doc_date.Location = new System.Drawing.Point(668, 41);
            this.lbl_doc_date.Name = "lbl_doc_date";
            this.lbl_doc_date.Size = new System.Drawing.Size(64, 13);
            this.lbl_doc_date.TabIndex = 10;
            this.lbl_doc_date.Text = "DOC DATE";
            //
            // txt_doc_date
            //
            this.txt_doc_date.Location = new System.Drawing.Point(796, 38);
            this.txt_doc_date.Name = "txt_doc_date";
            this.txt_doc_date.ReadOnly = true;
            this.txt_doc_date.Size = new System.Drawing.Size(180, 20);
            this.txt_doc_date.TabIndex = 11;
            //
            // lbl_return_type
            //
            this.lbl_return_type.AutoSize = true;
            this.lbl_return_type.Location = new System.Drawing.Point(12, 67);
            this.lbl_return_type.Name = "lbl_return_type";
            this.lbl_return_type.Size = new System.Drawing.Size(38, 13);
            this.lbl_return_type.TabIndex = 12;
            this.lbl_return_type.Text = "TYPE";
            //
            // cmb_return_type
            //
            this.cmb_return_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_return_type.FormattingEnabled = true;
            this.cmb_return_type.Items.AddRange(new object[] {
            "--Select--",
            "Return with Debit Memo",
            "Return without Debit Memo"});
            this.cmb_return_type.Location = new System.Drawing.Point(140, 64);
            this.cmb_return_type.Name = "cmb_return_type";
            this.cmb_return_type.Size = new System.Drawing.Size(180, 21);
            this.cmb_return_type.TabIndex = 13;
            //
            // lbl_ref_dm_no
            //
            // Populated only once "with Debit Memo" actually produces one - not user
            // typed.
            this.lbl_ref_dm_no.AutoSize = true;
            this.lbl_ref_dm_no.Location = new System.Drawing.Point(340, 67);
            this.lbl_ref_dm_no.Name = "lbl_ref_dm_no";
            this.lbl_ref_dm_no.Size = new System.Drawing.Size(62, 13);
            this.lbl_ref_dm_no.TabIndex = 14;
            this.lbl_ref_dm_no.Text = "REF. DM NO.";
            //
            // txt_ref_dm_no
            //
            this.txt_ref_dm_no.Location = new System.Drawing.Point(468, 64);
            this.txt_ref_dm_no.Name = "txt_ref_dm_no";
            this.txt_ref_dm_no.ReadOnly = true;
            this.txt_ref_dm_no.Size = new System.Drawing.Size(180, 20);
            this.txt_ref_dm_no.TabIndex = 15;
            //
            // lbl_approved_by
            //
            this.lbl_approved_by.AutoSize = true;
            this.lbl_approved_by.Location = new System.Drawing.Point(668, 67);
            this.lbl_approved_by.Name = "lbl_approved_by";
            this.lbl_approved_by.Size = new System.Drawing.Size(66, 13);
            this.lbl_approved_by.TabIndex = 16;
            this.lbl_approved_by.Text = "APPROVED BY";
            //
            // txt_approved_by
            //
            this.txt_approved_by.Location = new System.Drawing.Point(796, 64);
            this.txt_approved_by.Name = "txt_approved_by";
            this.txt_approved_by.ReadOnly = true;
            this.txt_approved_by.Size = new System.Drawing.Size(180, 20);
            this.txt_approved_by.TabIndex = 17;
            //
            // lbl_approval_date
            //
            this.lbl_approval_date.AutoSize = true;
            this.lbl_approval_date.Location = new System.Drawing.Point(12, 93);
            this.lbl_approval_date.Name = "lbl_approval_date";
            this.lbl_approval_date.Size = new System.Drawing.Size(78, 13);
            this.lbl_approval_date.TabIndex = 18;
            this.lbl_approval_date.Text = "APPROVAL DATE";
            //
            // txt_approval_date
            //
            this.txt_approval_date.Location = new System.Drawing.Point(140, 90);
            this.txt_approval_date.Name = "txt_approval_date";
            this.txt_approval_date.ReadOnly = true;
            this.txt_approval_date.Size = new System.Drawing.Size(180, 20);
            this.txt_approval_date.TabIndex = 19;
            //
            // lbl_remarks
            //
            this.lbl_remarks.AutoSize = true;
            this.lbl_remarks.Location = new System.Drawing.Point(340, 93);
            this.lbl_remarks.Name = "lbl_remarks";
            this.lbl_remarks.Size = new System.Drawing.Size(50, 13);
            this.lbl_remarks.TabIndex = 20;
            this.lbl_remarks.Text = "REMARKS";
            //
            // txt_remarks
            //
            this.txt_remarks.Location = new System.Drawing.Point(468, 90);
            this.txt_remarks.Multiline = true;
            this.txt_remarks.Name = "txt_remarks";
            this.txt_remarks.Size = new System.Drawing.Size(508, 50);
            this.txt_remarks.TabIndex = 21;
            //
            // toolStrip1
            //
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_search,
            this.btn_prev,
            this.btn_next,
            this.btn_edit,
            this.btn_save,
            this.btn_cancel,
            this.btn_approve});
            this.toolStrip1.Location = new System.Drawing.Point(0, 22);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1229, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            //
            // btn_new
            //
            this.btn_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(35, 22);
            this.btn_new.Text = "New";
            //
            // btn_search
            //
            this.btn_search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(46, 22);
            this.btn_search.Text = "Search";
            //
            // btn_prev
            //
            this.btn_prev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(45, 22);
            this.btn_prev.Text = "<< PREV";
            //
            // btn_next
            //
            this.btn_next.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(45, 22);
            this.btn_next.Text = "NEXT >>";
            //
            // btn_edit
            //
            this.btn_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(31, 22);
            this.btn_edit.Text = "Edit";
            //
            // btn_save
            //
            this.btn_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(35, 22);
            this.btn_save.Text = "Save";
            this.btn_save.Visible = false;
            //
            // btn_cancel
            //
            this.btn_cancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(46, 22);
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Visible = false;
            //
            // btn_approve
            //
            // CBDO only (confirmed with the user - spec 5.8 itself never describes this
            // gate, but section 3.2/section 16 imply CBDO approves purchase returns).
            this.btn_approve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_approve.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_approve.Name = "btn_approve";
            this.btn_approve.Size = new System.Drawing.Size(55, 22);
            this.btn_approve.Text = "Approve";
            this.btn_approve.Visible = false;
            //
            // lbl_title
            //
            this.lbl_title.AutoSize = true;
            this.lbl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Padding = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.lbl_title.Size = new System.Drawing.Size(140, 22);
            this.lbl_title.TabIndex = 3;
            this.lbl_title.Text = "PURCHASE RETURN";
            //
            // PurchaseReturnPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "PurchaseReturnPage";
            this.Size = new System.Drawing.Size(1229, 900);
            this.panel1.ResumeLayout(false);
            this.pnl_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.pnl_header.ResumeLayout(false);
            this.pnl_header.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnl_main;
        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_details_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_ref_ir_details_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_req_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_unit_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_reason;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel pnl_header;
        private System.Windows.Forms.Label lbl_supplier_code;
        private System.Windows.Forms.TextBox txt_supplier_code;
        private System.Windows.Forms.TextBox txt_supplier_id;
        private System.Windows.Forms.Label lbl_doc_no;
        private System.Windows.Forms.TextBox txt_document_no;
        private System.Windows.Forms.Label lbl_supplier_name;
        private System.Windows.Forms.TextBox txt_supplier_name;
        private System.Windows.Forms.Label lbl_ref_ir_no;
        private System.Windows.Forms.TextBox txt_ref_ir_no;
        private System.Windows.Forms.TextBox txt_ref_ir_id;
        private System.Windows.Forms.Label lbl_doc_date;
        private System.Windows.Forms.TextBox txt_doc_date;
        private System.Windows.Forms.Label lbl_return_type;
        private System.Windows.Forms.ComboBox cmb_return_type;
        private System.Windows.Forms.Label lbl_ref_dm_no;
        private System.Windows.Forms.TextBox txt_ref_dm_no;
        private System.Windows.Forms.Label lbl_approved_by;
        private System.Windows.Forms.TextBox txt_approved_by;
        private System.Windows.Forms.Label lbl_approval_date;
        private System.Windows.Forms.TextBox txt_approval_date;
        private System.Windows.Forms.Label lbl_remarks;
        private System.Windows.Forms.TextBox txt_remarks;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.ToolStripButton btn_next;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ToolStripButton btn_approve;
        private System.Windows.Forms.Label lbl_title;
    }
}
