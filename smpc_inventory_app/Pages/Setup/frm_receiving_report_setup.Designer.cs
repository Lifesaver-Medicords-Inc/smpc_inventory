
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_receiving_report_setup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_receiving_report_setup));
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_rr = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.pnl_head = new System.Windows.Forms.Panel();
            this.cmb_warehouse_name = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_address = new System.Windows.Forms.ComboBox();
            this.txt_purchase_order_id = new System.Windows.Forms.TextBox();
            this.poIDlbl = new System.Windows.Forms.Label();
            this.cmb_ref_doc = new System.Windows.Forms.ComboBox();
            this.txt_supplier_id = new System.Windows.Forms.TextBox();
            this.lbl_supplier_id = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_prepared_by = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.ref_doclbl = new System.Windows.Forms.Label();
            this.txt_supplier_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_date_received = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.suppliercodelbl = new System.Windows.Forms.Label();
            this.txt_supplier_name = new System.Windows.Forms.TextBox();
            this.txt_doc = new System.Windows.Forms.TextBox();
            this.supplierlbl = new System.Windows.Forms.Label();
            this.btn_setTime = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_main = new System.Windows.Forms.TabPage();
            this.pnl_purchase_return = new System.Windows.Forms.Panel();
            this.btn_purchase_return = new System.Windows.Forms.Button();
            this.pnl_main = new System.Windows.Forms.Panel();
            this.dg_receiving_report_details = new System.Windows.Forms.DataGridView();
            this.dg_receiving_report_details_item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_item_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_ordered_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_ordered_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_received_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_received_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_rejected_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_rejected_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_reason_for_rejection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_ref_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_details_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.receiving_report_id_receiving_report_details = new System.Data.DataColumn();
            this.item_code = new System.Data.DataColumn();
            this.item_description = new System.Data.DataColumn();
            this.ordered_qty = new System.Data.DataColumn();
            this.ordered_uom = new System.Data.DataColumn();
            this.received_qty = new System.Data.DataColumn();
            this.received_uom = new System.Data.DataColumn();
            this.rejected_qty = new System.Data.DataColumn();
            this.rejected_uom = new System.Data.DataColumn();
            this.reason_for_rejection = new System.Data.DataColumn();
            this.ref_id = new System.Data.DataColumn();
            this.id = new System.Data.DataColumn();
            this.tab_inventory = new System.Windows.Forms.TabPage();
            this.pnl_inventory = new System.Windows.Forms.Panel();
            this.dg_receiving_report_inventory = new System.Windows.Forms.DataGridView();
            this.tab_attachment = new System.Windows.Forms.TabPage();
            this.pnl_attachment_upload = new System.Windows.Forms.Panel();
            this.lst_receiving_report_attachment_files = new System.Windows.Forms.ListBox();
            this.btn_upload = new System.Windows.Forms.Button();
            this.pnl_attachment = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.receivingreportidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet2 = new System.Data.DataSet();
            this.dataTable2 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.pcb_receiving_report_attachment_images = new System.Windows.Forms.ImageList(this.components);
            this.receivingreportidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemdescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderedqtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordereduomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedqtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveduomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejectedqtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejecteduomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reasonforrejectionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivingreportidDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemcodeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemdescriptionDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderedqtyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordereduomDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedqtyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveduomDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejectedqtyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejecteduomDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reasonforrejectionDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivingreportidDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemcodeDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemdescriptionDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderedqtyDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordereduomDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedqtyDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveduomDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejectedqtyDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejecteduomDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reasonforrejectionDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refidDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_item_code1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_item_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_ordered_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_ordered_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_serial_numbers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_bin_location = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dg_receiving_report_inventory_ref_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_receiving_report_inventory_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_header.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnl_head.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_main.SuspendLayout();
            this.pnl_purchase_return.SuspendLayout();
            this.pnl_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_receiving_report_details)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.tab_inventory.SuspendLayout();
            this.pnl_inventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_receiving_report_inventory)).BeginInit();
            this.tab_attachment.SuspendLayout();
            this.pnl_attachment_upload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_rr);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(1285, 63);
            this.panel_header.TabIndex = 10;
            // 
            // lbl_rr
            // 
            this.lbl_rr.AutoSize = true;
            this.lbl_rr.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rr.Location = new System.Drawing.Point(5, 16);
            this.lbl_rr.Name = "lbl_rr";
            this.lbl_rr.Size = new System.Drawing.Size(155, 24);
            this.lbl_rr.TabIndex = 3;
            this.lbl_rr.Text = "Receiving Report";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_search,
            this.btn_save,
            this.btn_next,
            this.btn_prev,
            this.btn_cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 63);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1285, 25);
            this.toolStrip1.TabIndex = 26;
            // 
            // btn_new
            // 
            this.btn_new.AccessibleName = "";
            this.btn_new.Image = ((System.Drawing.Image)(resources.GetObject("btn_new.Image")));
            this.btn_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(51, 22);
            this.btn_new.Text = "New";
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Image = ((System.Drawing.Image)(resources.GetObject("btn_edit.Image")));
            this.btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(47, 22);
            this.btn_edit.Text = "&Edit";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.Image")));
            this.btn_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(60, 22);
            this.btn_delete.Text = "Delete";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_search
            // 
            this.btn_search.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.Image")));
            this.btn_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(62, 22);
            this.btn_search.Text = "Search";
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(51, 22);
            this.btn_save.Text = "&Save";
            this.btn_save.Visible = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_next
            // 
            this.btn_next.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_next.Enabled = false;
            this.btn_next.Image = ((System.Drawing.Image)(resources.GetObject("btn_next.Image")));
            this.btn_next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(52, 22);
            this.btn_next.Text = "Next";
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_prev
            // 
            this.btn_prev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_prev.Enabled = false;
            this.btn_prev.Image = ((System.Drawing.Image)(resources.GetObject("btn_prev.Image")));
            this.btn_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(72, 22);
            this.btn_prev.Text = "Previous";
            this.btn_prev.Click += new System.EventHandler(this.btn_prev_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(63, 22);
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.ToolTipText = "Cancel";
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // pnl_head
            // 
            this.pnl_head.Controls.Add(this.cmb_warehouse_name);
            this.pnl_head.Controls.Add(this.label1);
            this.pnl_head.Controls.Add(this.cmb_address);
            this.pnl_head.Controls.Add(this.txt_purchase_order_id);
            this.pnl_head.Controls.Add(this.poIDlbl);
            this.pnl_head.Controls.Add(this.cmb_ref_doc);
            this.pnl_head.Controls.Add(this.txt_supplier_id);
            this.pnl_head.Controls.Add(this.lbl_supplier_id);
            this.pnl_head.Controls.Add(this.label5);
            this.pnl_head.Controls.Add(this.txt_prepared_by);
            this.pnl_head.Controls.Add(this.label7);
            this.pnl_head.Controls.Add(this.txt_id);
            this.pnl_head.Controls.Add(this.lbl_id);
            this.pnl_head.Controls.Add(this.ref_doclbl);
            this.pnl_head.Controls.Add(this.txt_supplier_code);
            this.pnl_head.Controls.Add(this.label4);
            this.pnl_head.Controls.Add(this.txt_date_received);
            this.pnl_head.Controls.Add(this.label3);
            this.pnl_head.Controls.Add(this.suppliercodelbl);
            this.pnl_head.Controls.Add(this.txt_supplier_name);
            this.pnl_head.Controls.Add(this.txt_doc);
            this.pnl_head.Controls.Add(this.supplierlbl);
            this.pnl_head.Controls.Add(this.btn_setTime);
            this.pnl_head.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_head.Location = new System.Drawing.Point(0, 88);
            this.pnl_head.Name = "pnl_head";
            this.pnl_head.Size = new System.Drawing.Size(1285, 132);
            this.pnl_head.TabIndex = 27;
            // 
            // cmb_warehouse_name
            // 
            this.cmb_warehouse_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_warehouse_name.FormattingEnabled = true;
            this.cmb_warehouse_name.Location = new System.Drawing.Point(140, 76);
            this.cmb_warehouse_name.MaxLength = 50;
            this.cmb_warehouse_name.MinimumSize = new System.Drawing.Size(200, 0);
            this.cmb_warehouse_name.Name = "cmb_warehouse_name";
            this.cmb_warehouse_name.Size = new System.Drawing.Size(200, 21);
            this.cmb_warehouse_name.TabIndex = 43;
            this.cmb_warehouse_name.TabStop = false;
            this.cmb_warehouse_name.Tag = "no_edit";
            this.cmb_warehouse_name.DropDown += new System.EventHandler(this.cmb_name_DropDown);
            this.cmb_warehouse_name.SelectedIndexChanged += new System.EventHandler(this.cmb_name_SelectedIndexChanged);
            this.cmb_warehouse_name.SelectionChangeCommitted += new System.EventHandler(this.cmb_name_SelectionChangeCommitted);
            this.cmb_warehouse_name.DropDownClosed += new System.EventHandler(this.cmb_name_DropDownClosed);
            this.cmb_warehouse_name.MouseHover += new System.EventHandler(this.cmb_name_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "WAREHOUSE";
            // 
            // cmb_address
            // 
            this.cmb_address.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_address.Enabled = false;
            this.cmb_address.FormattingEnabled = true;
            this.cmb_address.Location = new System.Drawing.Point(140, 99);
            this.cmb_address.MaxLength = 50;
            this.cmb_address.MinimumSize = new System.Drawing.Size(200, 0);
            this.cmb_address.Name = "cmb_address";
            this.cmb_address.Size = new System.Drawing.Size(200, 21);
            this.cmb_address.TabIndex = 41;
            this.cmb_address.TabStop = false;
            this.cmb_address.Tag = "no_edit";
            this.cmb_address.DropDown += new System.EventHandler(this.cmb_address_DropDown);
            this.cmb_address.SelectedIndexChanged += new System.EventHandler(this.cmb_address_SelectedIndexChanged);
            this.cmb_address.SelectionChangeCommitted += new System.EventHandler(this.cmb_address_SelectionChangeCommitted);
            this.cmb_address.DropDownClosed += new System.EventHandler(this.cmb_address_DropDownClosed);
            this.cmb_address.MouseHover += new System.EventHandler(this.cmb_address_MouseHover);
            // 
            // txt_purchase_order_id
            // 
            this.txt_purchase_order_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_purchase_order_id.Location = new System.Drawing.Point(1048, 102);
            this.txt_purchase_order_id.Name = "txt_purchase_order_id";
            this.txt_purchase_order_id.ReadOnly = true;
            this.txt_purchase_order_id.Size = new System.Drawing.Size(200, 20);
            this.txt_purchase_order_id.TabIndex = 40;
            this.txt_purchase_order_id.TabStop = false;
            this.txt_purchase_order_id.Tag = "no_edit";
            this.txt_purchase_order_id.Visible = false;
            // 
            // poIDlbl
            // 
            this.poIDlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poIDlbl.AutoSize = true;
            this.poIDlbl.Location = new System.Drawing.Point(957, 106);
            this.poIDlbl.Name = "poIDlbl";
            this.poIDlbl.Size = new System.Drawing.Size(89, 13);
            this.poIDlbl.TabIndex = 39;
            this.poIDlbl.Text = "purchase order id";
            this.poIDlbl.Visible = false;
            // 
            // cmb_ref_doc
            // 
            this.cmb_ref_doc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_ref_doc.Enabled = false;
            this.cmb_ref_doc.FormattingEnabled = true;
            this.cmb_ref_doc.Items.AddRange(new object[] {
            "tmp ",
            "items",
            "here",
            "and ",
            "here"});
            this.cmb_ref_doc.Location = new System.Drawing.Point(1048, 32);
            this.cmb_ref_doc.Name = "cmb_ref_doc";
            this.cmb_ref_doc.Size = new System.Drawing.Size(199, 21);
            this.cmb_ref_doc.TabIndex = 5;
            this.cmb_ref_doc.TabStop = false;
            this.cmb_ref_doc.DropDown += new System.EventHandler(this.cmb_ref_doc_DropDown);
            this.cmb_ref_doc.SelectedIndexChanged += new System.EventHandler(this.cmb_ref_doc_SelectedIndexChanged);
            this.cmb_ref_doc.TextChanged += new System.EventHandler(this.cmb_ref_doc_TextChanged);
            this.cmb_ref_doc.Enter += new System.EventHandler(this.cmb_ref_doc_Enter);
            this.cmb_ref_doc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_ref_doc_KeyDown);
            this.cmb_ref_doc.Leave += new System.EventHandler(this.cmb_ref_doc_Leave);
            this.cmb_ref_doc.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmb_ref_doc_MouseClick);
            // 
            // txt_supplier_id
            // 
            this.txt_supplier_id.Location = new System.Drawing.Point(532, 98);
            this.txt_supplier_id.Name = "txt_supplier_id";
            this.txt_supplier_id.ReadOnly = true;
            this.txt_supplier_id.Size = new System.Drawing.Size(200, 20);
            this.txt_supplier_id.TabIndex = 35;
            this.txt_supplier_id.TabStop = false;
            this.txt_supplier_id.Tag = "no_edit";
            this.txt_supplier_id.Visible = false;
            // 
            // lbl_supplier_id
            // 
            this.lbl_supplier_id.AutoSize = true;
            this.lbl_supplier_id.Location = new System.Drawing.Point(476, 102);
            this.lbl_supplier_id.Name = "lbl_supplier_id";
            this.lbl_supplier_id.Size = new System.Drawing.Size(54, 13);
            this.lbl_supplier_id.TabIndex = 34;
            this.lbl_supplier_id.Text = "supplier id";
            this.lbl_supplier_id.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(934, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "DOC NO";
            // 
            // txt_prepared_by
            // 
            this.txt_prepared_by.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_prepared_by.Location = new System.Drawing.Point(1048, 77);
            this.txt_prepared_by.Name = "txt_prepared_by";
            this.txt_prepared_by.ReadOnly = true;
            this.txt_prepared_by.Size = new System.Drawing.Size(200, 20);
            this.txt_prepared_by.TabIndex = 33;
            this.txt_prepared_by.TabStop = false;
            this.txt_prepared_by.Tag = "no_edit";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(934, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "PREPARED BY";
            // 
            // txt_id
            // 
            this.txt_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_id.Location = new System.Drawing.Point(1048, 54);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 30;
            this.txt_id.TabStop = false;
            this.txt_id.Tag = "no_edit";
            // 
            // lbl_id
            // 
            this.lbl_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(1032, 58);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(15, 13);
            this.lbl_id.TabIndex = 29;
            this.lbl_id.Text = "id";
            this.lbl_id.Visible = false;
            // 
            // ref_doclbl
            // 
            this.ref_doclbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ref_doclbl.AutoSize = true;
            this.ref_doclbl.Location = new System.Drawing.Point(933, 36);
            this.ref_doclbl.Name = "ref_doclbl";
            this.ref_doclbl.Size = new System.Drawing.Size(98, 13);
            this.ref_doclbl.TabIndex = 28;
            this.ref_doclbl.Text = "REFERENCE DOC";
            // 
            // txt_supplier_code
            // 
            this.txt_supplier_code.Location = new System.Drawing.Point(140, 32);
            this.txt_supplier_code.Name = "txt_supplier_code";
            this.txt_supplier_code.ReadOnly = true;
            this.txt_supplier_code.Size = new System.Drawing.Size(200, 20);
            this.txt_supplier_code.TabIndex = 2;
            this.txt_supplier_code.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "ADDRESS";
            // 
            // txt_date_received
            // 
            this.txt_date_received.Location = new System.Drawing.Point(140, 54);
            this.txt_date_received.Name = "txt_date_received";
            this.txt_date_received.ReadOnly = true;
            this.txt_date_received.Size = new System.Drawing.Size(200, 20);
            this.txt_date_received.TabIndex = 3;
            this.txt_date_received.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "DATE RECEIVED";
            // 
            // suppliercodelbl
            // 
            this.suppliercodelbl.AutoSize = true;
            this.suppliercodelbl.Location = new System.Drawing.Point(37, 35);
            this.suppliercodelbl.Name = "suppliercodelbl";
            this.suppliercodelbl.Size = new System.Drawing.Size(37, 13);
            this.suppliercodelbl.TabIndex = 21;
            this.suppliercodelbl.Text = "CODE";
            // 
            // txt_supplier_name
            // 
            this.txt_supplier_name.Location = new System.Drawing.Point(140, 10);
            this.txt_supplier_name.Name = "txt_supplier_name";
            this.txt_supplier_name.ReadOnly = true;
            this.txt_supplier_name.Size = new System.Drawing.Size(200, 20);
            this.txt_supplier_name.TabIndex = 1;
            this.txt_supplier_name.Tag = "";
            // 
            // txt_doc
            // 
            this.txt_doc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_doc.Location = new System.Drawing.Point(1048, 10);
            this.txt_doc.Name = "txt_doc";
            this.txt_doc.ReadOnly = true;
            this.txt_doc.Size = new System.Drawing.Size(200, 20);
            this.txt_doc.TabIndex = 15;
            this.txt_doc.TabStop = false;
            this.txt_doc.Tag = "no_edit";
            // 
            // supplierlbl
            // 
            this.supplierlbl.AutoSize = true;
            this.supplierlbl.Location = new System.Drawing.Point(37, 13);
            this.supplierlbl.Name = "supplierlbl";
            this.supplierlbl.Size = new System.Drawing.Size(60, 13);
            this.supplierlbl.TabIndex = 14;
            this.supplierlbl.Text = "SUPPLIER";
            // 
            // btn_setTime
            // 
            this.btn_setTime.Location = new System.Drawing.Point(346, 52);
            this.btn_setTime.Name = "btn_setTime";
            this.btn_setTime.Size = new System.Drawing.Size(75, 23);
            this.btn_setTime.TabIndex = 38;
            this.btn_setTime.Text = "Set Current";
            this.btn_setTime.UseVisualStyleBackColor = true;
            this.btn_setTime.Visible = false;
            this.btn_setTime.Click += new System.EventHandler(this.btn_setTime_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_main);
            this.tabControl1.Controls.Add(this.tab_inventory);
            this.tabControl1.Controls.Add(this.tab_attachment);
            this.tabControl1.Location = new System.Drawing.Point(0, 223);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(8, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1285, 392);
            this.tabControl1.TabIndex = 33;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tab_main
            // 
            this.tab_main.Controls.Add(this.pnl_purchase_return);
            this.tab_main.Controls.Add(this.pnl_main);
            this.tab_main.Location = new System.Drawing.Point(4, 22);
            this.tab_main.Name = "tab_main";
            this.tab_main.Padding = new System.Windows.Forms.Padding(3);
            this.tab_main.Size = new System.Drawing.Size(1277, 366);
            this.tab_main.TabIndex = 1;
            this.tab_main.Text = "MAIN";
            this.tab_main.UseVisualStyleBackColor = true;
            // 
            // pnl_purchase_return
            // 
            this.pnl_purchase_return.Controls.Add(this.btn_purchase_return);
            this.pnl_purchase_return.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_purchase_return.Location = new System.Drawing.Point(3, 268);
            this.pnl_purchase_return.Name = "pnl_purchase_return";
            this.pnl_purchase_return.Size = new System.Drawing.Size(1271, 95);
            this.pnl_purchase_return.TabIndex = 36;
            // 
            // btn_purchase_return
            // 
            this.btn_purchase_return.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_purchase_return.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_purchase_return.Location = new System.Drawing.Point(1137, 30);
            this.btn_purchase_return.Name = "btn_purchase_return";
            this.btn_purchase_return.Size = new System.Drawing.Size(104, 23);
            this.btn_purchase_return.TabIndex = 35;
            this.btn_purchase_return.Text = "Purchase Return";
            this.btn_purchase_return.UseVisualStyleBackColor = false;
            // 
            // pnl_main
            // 
            this.pnl_main.Controls.Add(this.dg_receiving_report_details);
            this.pnl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_main.Location = new System.Drawing.Point(3, 3);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Padding = new System.Windows.Forms.Padding(0, 0, 0, 95);
            this.pnl_main.Size = new System.Drawing.Size(1271, 360);
            this.pnl_main.TabIndex = 1;
            // 
            // dg_receiving_report_details
            // 
            this.dg_receiving_report_details.AllowUserToAddRows = false;
            this.dg_receiving_report_details.AllowUserToDeleteRows = false;
            this.dg_receiving_report_details.AutoGenerateColumns = false;
            this.dg_receiving_report_details.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_receiving_report_details.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dg_receiving_report_details.ColumnHeadersHeight = 34;
            this.dg_receiving_report_details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_receiving_report_details.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_receiving_report_details_item_code,
            this.dg_receiving_report_details_item_description,
            this.dg_receiving_report_details_ordered_qty,
            this.dg_receiving_report_details_ordered_uom,
            this.dg_receiving_report_details_received_qty,
            this.dg_receiving_report_details_received_uom,
            this.dg_receiving_report_details_rejected_qty,
            this.dg_receiving_report_details_rejected_uom,
            this.dg_receiving_report_details_reason_for_rejection,
            this.dg_receiving_report_details_ref_id,
            this.dg_receiving_report_details_id});
            this.dg_receiving_report_details.DataSource = this.bindingSource1;
            this.dg_receiving_report_details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_receiving_report_details.Location = new System.Drawing.Point(0, 0);
            this.dg_receiving_report_details.Name = "dg_receiving_report_details";
            this.dg_receiving_report_details.RowHeadersWidth = 17;
            this.dg_receiving_report_details.ShowCellErrors = false;
            this.dg_receiving_report_details.ShowCellToolTips = false;
            this.dg_receiving_report_details.ShowEditingIcon = false;
            this.dg_receiving_report_details.Size = new System.Drawing.Size(1271, 265);
            this.dg_receiving_report_details.TabIndex = 2;
            this.dg_receiving_report_details.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_receiving_report_details_CellLeave);
            this.dg_receiving_report_details.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.datagrid_CellPainting);
            this.dg_receiving_report_details.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.datagrid_ColumnWidthChanged);
            this.dg_receiving_report_details.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.datagrid_DataBindingComplete);
            this.dg_receiving_report_details.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_RowEnter);
            this.dg_receiving_report_details.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dg_RowsRemoved);
            this.dg_receiving_report_details.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_receiving_report_details_RowValidating);
            this.dg_receiving_report_details.Scroll += new System.Windows.Forms.ScrollEventHandler(this.datagrid_Scroll);
            this.dg_receiving_report_details.Paint += new System.Windows.Forms.PaintEventHandler(this.datagrid_Paint);
            this.dg_receiving_report_details.DoubleClick += new System.EventHandler(this.dg_DoubleClick);
            this.dg_receiving_report_details.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dg_KeyDown);
            // 
            // dg_receiving_report_details_item_code
            // 
            this.dg_receiving_report_details_item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_item_code.DataPropertyName = "item_code";
            this.dg_receiving_report_details_item_code.HeaderText = "ITEM CODE";
            this.dg_receiving_report_details_item_code.Name = "dg_receiving_report_details_item_code";
            this.dg_receiving_report_details_item_code.ReadOnly = true;
            this.dg_receiving_report_details_item_code.Width = 84;
            // 
            // dg_receiving_report_details_item_description
            // 
            this.dg_receiving_report_details_item_description.DataPropertyName = "item_description";
            this.dg_receiving_report_details_item_description.HeaderText = "ITEM DESCRIPTION";
            this.dg_receiving_report_details_item_description.MinimumWidth = 150;
            this.dg_receiving_report_details_item_description.Name = "dg_receiving_report_details_item_description";
            this.dg_receiving_report_details_item_description.ReadOnly = true;
            // 
            // dg_receiving_report_details_ordered_qty
            // 
            this.dg_receiving_report_details_ordered_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_ordered_qty.DataPropertyName = "ordered_qty";
            this.dg_receiving_report_details_ordered_qty.HeaderText = "QTY";
            this.dg_receiving_report_details_ordered_qty.Name = "dg_receiving_report_details_ordered_qty";
            this.dg_receiving_report_details_ordered_qty.ReadOnly = true;
            this.dg_receiving_report_details_ordered_qty.Width = 54;
            // 
            // dg_receiving_report_details_ordered_uom
            // 
            this.dg_receiving_report_details_ordered_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_ordered_uom.DataPropertyName = "ordered_uom";
            this.dg_receiving_report_details_ordered_uom.HeaderText = "UOM";
            this.dg_receiving_report_details_ordered_uom.Name = "dg_receiving_report_details_ordered_uom";
            this.dg_receiving_report_details_ordered_uom.ReadOnly = true;
            this.dg_receiving_report_details_ordered_uom.Width = 57;
            // 
            // dg_receiving_report_details_received_qty
            // 
            this.dg_receiving_report_details_received_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_received_qty.DataPropertyName = "received_qty";
            this.dg_receiving_report_details_received_qty.HeaderText = "QTY";
            this.dg_receiving_report_details_received_qty.Name = "dg_receiving_report_details_received_qty";
            this.dg_receiving_report_details_received_qty.Width = 54;
            // 
            // dg_receiving_report_details_received_uom
            // 
            this.dg_receiving_report_details_received_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_received_uom.DataPropertyName = "received_uom";
            this.dg_receiving_report_details_received_uom.HeaderText = "UOM";
            this.dg_receiving_report_details_received_uom.Name = "dg_receiving_report_details_received_uom";
            this.dg_receiving_report_details_received_uom.Width = 57;
            // 
            // dg_receiving_report_details_rejected_qty
            // 
            this.dg_receiving_report_details_rejected_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_rejected_qty.DataPropertyName = "rejected_qty";
            this.dg_receiving_report_details_rejected_qty.HeaderText = "QTY";
            this.dg_receiving_report_details_rejected_qty.Name = "dg_receiving_report_details_rejected_qty";
            this.dg_receiving_report_details_rejected_qty.Width = 54;
            // 
            // dg_receiving_report_details_rejected_uom
            // 
            this.dg_receiving_report_details_rejected_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_rejected_uom.DataPropertyName = "rejected_uom";
            this.dg_receiving_report_details_rejected_uom.HeaderText = "UOM";
            this.dg_receiving_report_details_rejected_uom.Name = "dg_receiving_report_details_rejected_uom";
            this.dg_receiving_report_details_rejected_uom.Width = 57;
            // 
            // dg_receiving_report_details_reason_for_rejection
            // 
            this.dg_receiving_report_details_reason_for_rejection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dg_receiving_report_details_reason_for_rejection.DataPropertyName = "reason_for_rejection";
            this.dg_receiving_report_details_reason_for_rejection.HeaderText = "REASON FOR REJECTION";
            this.dg_receiving_report_details_reason_for_rejection.MinimumWidth = 150;
            this.dg_receiving_report_details_reason_for_rejection.Name = "dg_receiving_report_details_reason_for_rejection";
            this.dg_receiving_report_details_reason_for_rejection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_receiving_report_details_reason_for_rejection.Width = 150;
            // 
            // dg_receiving_report_details_ref_id
            // 
            this.dg_receiving_report_details_ref_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_ref_id.DataPropertyName = "ref_id";
            this.dg_receiving_report_details_ref_id.HeaderText = "ref_id";
            this.dg_receiving_report_details_ref_id.Name = "dg_receiving_report_details_ref_id";
            this.dg_receiving_report_details_ref_id.Visible = false;
            this.dg_receiving_report_details_ref_id.Width = 58;
            // 
            // dg_receiving_report_details_id
            // 
            this.dg_receiving_report_details_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_details_id.DataPropertyName = "id";
            this.dg_receiving_report_details_id.HeaderText = "id";
            this.dg_receiving_report_details_id.Name = "dg_receiving_report_details_id";
            this.dg_receiving_report_details_id.ReadOnly = true;
            this.dg_receiving_report_details_id.Width = 40;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "ReceivingReportDetails";
            this.bindingSource1.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.receiving_report_id_receiving_report_details,
            this.item_code,
            this.item_description,
            this.ordered_qty,
            this.ordered_uom,
            this.received_qty,
            this.received_uom,
            this.rejected_qty,
            this.rejected_uom,
            this.reason_for_rejection,
            this.ref_id,
            this.id});
            this.dataTable1.TableName = "ReceivingReportDetails";
            // 
            // receiving_report_id_receiving_report_details
            // 
            this.receiving_report_id_receiving_report_details.ColumnName = "receiving_report_id";
            // 
            // item_code
            // 
            this.item_code.ColumnName = "item_code";
            // 
            // item_description
            // 
            this.item_description.ColumnName = "item_description";
            // 
            // ordered_qty
            // 
            this.ordered_qty.ColumnName = "ordered_qty";
            // 
            // ordered_uom
            // 
            this.ordered_uom.ColumnName = "ordered_uom";
            // 
            // received_qty
            // 
            this.received_qty.ColumnName = "received_qty";
            // 
            // received_uom
            // 
            this.received_uom.ColumnName = "received_uom";
            // 
            // rejected_qty
            // 
            this.rejected_qty.ColumnName = "rejected_qty";
            // 
            // rejected_uom
            // 
            this.rejected_uom.ColumnName = "rejected_uom";
            // 
            // reason_for_rejection
            // 
            this.reason_for_rejection.ColumnName = "reason_for_rejection";
            // 
            // ref_id
            // 
            this.ref_id.ColumnName = "ref_id";
            // 
            // id
            // 
            this.id.ColumnName = "id";
            this.id.ReadOnly = true;
            // 
            // tab_inventory
            // 
            this.tab_inventory.Controls.Add(this.pnl_inventory);
            this.tab_inventory.Location = new System.Drawing.Point(4, 22);
            this.tab_inventory.Name = "tab_inventory";
            this.tab_inventory.Padding = new System.Windows.Forms.Padding(3, 3, 3, 18);
            this.tab_inventory.Size = new System.Drawing.Size(1277, 366);
            this.tab_inventory.TabIndex = 0;
            this.tab_inventory.Text = " INVENTORY";
            this.tab_inventory.UseVisualStyleBackColor = true;
            // 
            // pnl_inventory
            // 
            this.pnl_inventory.Controls.Add(this.dg_receiving_report_inventory);
            this.pnl_inventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_inventory.Location = new System.Drawing.Point(3, 3);
            this.pnl_inventory.Name = "pnl_inventory";
            this.pnl_inventory.Size = new System.Drawing.Size(1271, 345);
            this.pnl_inventory.TabIndex = 39;
            // 
            // dg_receiving_report_inventory
            // 
            this.dg_receiving_report_inventory.AllowUserToAddRows = false;
            this.dg_receiving_report_inventory.AllowUserToDeleteRows = false;
            this.dg_receiving_report_inventory.AutoGenerateColumns = false;
            this.dg_receiving_report_inventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_receiving_report_inventory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dg_receiving_report_inventory.ColumnHeadersHeight = 34;
            this.dg_receiving_report_inventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_receiving_report_inventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_receiving_report_inventory_item_code1,
            this.dg_receiving_report_inventory_item_description,
            this.dg_receiving_report_inventory_ordered_qty,
            this.dg_receiving_report_inventory_ordered_uom,
            this.dg_receiving_report_inventory_serial_numbers,
            this.dg_receiving_report_inventory_bin_location,
            this.dg_receiving_report_inventory_ref_id,
            this.dg_receiving_report_inventory_id});
            this.dg_receiving_report_inventory.DataSource = this.bindingSource1;
            this.dg_receiving_report_inventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_receiving_report_inventory.Location = new System.Drawing.Point(0, 0);
            this.dg_receiving_report_inventory.Name = "dg_receiving_report_inventory";
            this.dg_receiving_report_inventory.RowHeadersWidth = 17;
            this.dg_receiving_report_inventory.ShowCellErrors = false;
            this.dg_receiving_report_inventory.ShowCellToolTips = false;
            this.dg_receiving_report_inventory.ShowEditingIcon = false;
            this.dg_receiving_report_inventory.Size = new System.Drawing.Size(1271, 345);
            this.dg_receiving_report_inventory.TabIndex = 3;
            this.dg_receiving_report_inventory.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_receiving_report_inventory_CellBeginEdit);
            this.dg_receiving_report_inventory.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.datagrid_CellPainting);
            this.dg_receiving_report_inventory.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.datagrid_ColumnWidthChanged);
            this.dg_receiving_report_inventory.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.datagrid_DataBindingComplete);
            this.dg_receiving_report_inventory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_receiving_report_inventory_DataError);
            this.dg_receiving_report_inventory.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dg_receiving_report_inventory_EditingControlShowing);
            this.dg_receiving_report_inventory.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_RowEnter);
            this.dg_receiving_report_inventory.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dg_RowsRemoved);
            this.dg_receiving_report_inventory.Scroll += new System.Windows.Forms.ScrollEventHandler(this.datagrid_Scroll);
            this.dg_receiving_report_inventory.Paint += new System.Windows.Forms.PaintEventHandler(this.datagrid_Paint);
            this.dg_receiving_report_inventory.DoubleClick += new System.EventHandler(this.dg_DoubleClick);
            this.dg_receiving_report_inventory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dg_KeyDown);
            // 
            // tab_attachment
            // 
            this.tab_attachment.Controls.Add(this.pnl_attachment_upload);
            this.tab_attachment.Controls.Add(this.pnl_attachment);
            this.tab_attachment.Location = new System.Drawing.Point(4, 22);
            this.tab_attachment.Name = "tab_attachment";
            this.tab_attachment.Size = new System.Drawing.Size(1277, 366);
            this.tab_attachment.TabIndex = 2;
            this.tab_attachment.Text = "ATTACHMENT";
            this.tab_attachment.UseVisualStyleBackColor = true;
            // 
            // pnl_attachment_upload
            // 
            this.pnl_attachment_upload.Controls.Add(this.lst_receiving_report_attachment_files);
            this.pnl_attachment_upload.Controls.Add(this.btn_upload);
            this.pnl_attachment_upload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_attachment_upload.Location = new System.Drawing.Point(0, 0);
            this.pnl_attachment_upload.Name = "pnl_attachment_upload";
            this.pnl_attachment_upload.Padding = new System.Windows.Forms.Padding(15, 15, 15, 75);
            this.pnl_attachment_upload.Size = new System.Drawing.Size(1277, 366);
            this.pnl_attachment_upload.TabIndex = 38;
            // 
            // lst_receiving_report_attachment_files
            // 
            this.lst_receiving_report_attachment_files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lst_receiving_report_attachment_files.FormattingEnabled = true;
            this.lst_receiving_report_attachment_files.Location = new System.Drawing.Point(15, 15);
            this.lst_receiving_report_attachment_files.Margin = new System.Windows.Forms.Padding(15);
            this.lst_receiving_report_attachment_files.Name = "lst_receiving_report_attachment_files";
            this.lst_receiving_report_attachment_files.Size = new System.Drawing.Size(1247, 276);
            this.lst_receiving_report_attachment_files.TabIndex = 36;
            this.lst_receiving_report_attachment_files.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lst_receiving_report_attachment_files_DrawItem);
            this.lst_receiving_report_attachment_files.DoubleClick += new System.EventHandler(this.lst_receiving_report_attachment_DoubleClick);
            this.lst_receiving_report_attachment_files.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lst_receiving_report_attachment_files_KeyDown);
            this.lst_receiving_report_attachment_files.MouseLeave += new System.EventHandler(this.lst_receiving_report_attachment_files_MouseLeave);
            this.lst_receiving_report_attachment_files.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lst_receiving_report_attachment_files_MouseMove);
            // 
            // btn_upload
            // 
            this.btn_upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_upload.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_upload.Location = new System.Drawing.Point(1169, 304);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(75, 23);
            this.btn_upload.TabIndex = 35;
            this.btn_upload.Text = "Upload";
            this.btn_upload.UseVisualStyleBackColor = false;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // pnl_attachment
            // 
            this.pnl_attachment.Location = new System.Drawing.Point(3, 3);
            this.pnl_attachment.Name = "pnl_attachment";
            this.pnl_attachment.Size = new System.Drawing.Size(692, 392);
            this.pnl_attachment.TabIndex = 36;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // receivingreportidDataGridViewTextBoxColumn
            // 
            this.receivingreportidDataGridViewTextBoxColumn.DataPropertyName = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn.HeaderText = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn.Name = "receivingreportidDataGridViewTextBoxColumn";
            this.receivingreportidDataGridViewTextBoxColumn.Width = 633;
            // 
            // bindingSource2
            // 
            this.bindingSource2.DataSource = this.dataSet1;
            this.bindingSource2.Position = 0;
            // 
            // dataSet2
            // 
            this.dataSet2.DataSetName = "NewDataSet";
            this.dataSet2.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable2});
            // 
            // dataTable2
            // 
            this.dataTable2.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10,
            this.dataColumn11,
            this.dataColumn12});
            this.dataTable2.TableName = "ReceivingReportInventory";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "receiving_report_id";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "item_code";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "item_description";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "ordered_qty";
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "ordered_uom";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "received_qty";
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "received_uom";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "rejected_qty";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "rejected_uom";
            // 
            // dataColumn10
            // 
            this.dataColumn10.ColumnName = "reason_for_rejection";
            // 
            // dataColumn11
            // 
            this.dataColumn11.ColumnName = "ref_id";
            // 
            // dataColumn12
            // 
            this.dataColumn12.ColumnName = "id";
            this.dataColumn12.ReadOnly = true;
            // 
            // pcb_receiving_report_attachment_images
            // 
            this.pcb_receiving_report_attachment_images.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.pcb_receiving_report_attachment_images.ImageSize = new System.Drawing.Size(16, 16);
            this.pcb_receiving_report_attachment_images.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // receivingreportidDataGridViewTextBoxColumn1
            // 
            this.receivingreportidDataGridViewTextBoxColumn1.DataPropertyName = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn1.HeaderText = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn1.Name = "receivingreportidDataGridViewTextBoxColumn1";
            this.receivingreportidDataGridViewTextBoxColumn1.Width = 73;
            // 
            // itemcodeDataGridViewTextBoxColumn
            // 
            this.itemcodeDataGridViewTextBoxColumn.DataPropertyName = "item_code";
            this.itemcodeDataGridViewTextBoxColumn.HeaderText = "item_code";
            this.itemcodeDataGridViewTextBoxColumn.Name = "itemcodeDataGridViewTextBoxColumn";
            this.itemcodeDataGridViewTextBoxColumn.Width = 73;
            // 
            // itemdescriptionDataGridViewTextBoxColumn
            // 
            this.itemdescriptionDataGridViewTextBoxColumn.DataPropertyName = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn.HeaderText = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn.Name = "itemdescriptionDataGridViewTextBoxColumn";
            this.itemdescriptionDataGridViewTextBoxColumn.Width = 72;
            // 
            // orderedqtyDataGridViewTextBoxColumn
            // 
            this.orderedqtyDataGridViewTextBoxColumn.DataPropertyName = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn.HeaderText = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn.Name = "orderedqtyDataGridViewTextBoxColumn";
            this.orderedqtyDataGridViewTextBoxColumn.Width = 73;
            // 
            // ordereduomDataGridViewTextBoxColumn
            // 
            this.ordereduomDataGridViewTextBoxColumn.DataPropertyName = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn.HeaderText = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn.Name = "ordereduomDataGridViewTextBoxColumn";
            this.ordereduomDataGridViewTextBoxColumn.Width = 73;
            // 
            // receivedqtyDataGridViewTextBoxColumn
            // 
            this.receivedqtyDataGridViewTextBoxColumn.DataPropertyName = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn.HeaderText = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn.Name = "receivedqtyDataGridViewTextBoxColumn";
            this.receivedqtyDataGridViewTextBoxColumn.Width = 73;
            // 
            // receiveduomDataGridViewTextBoxColumn
            // 
            this.receiveduomDataGridViewTextBoxColumn.DataPropertyName = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn.HeaderText = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn.Name = "receiveduomDataGridViewTextBoxColumn";
            this.receiveduomDataGridViewTextBoxColumn.Width = 73;
            // 
            // rejectedqtyDataGridViewTextBoxColumn
            // 
            this.rejectedqtyDataGridViewTextBoxColumn.DataPropertyName = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn.HeaderText = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn.Name = "rejectedqtyDataGridViewTextBoxColumn";
            this.rejectedqtyDataGridViewTextBoxColumn.Width = 73;
            // 
            // rejecteduomDataGridViewTextBoxColumn
            // 
            this.rejecteduomDataGridViewTextBoxColumn.DataPropertyName = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn.HeaderText = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn.Name = "rejecteduomDataGridViewTextBoxColumn";
            this.rejecteduomDataGridViewTextBoxColumn.Width = 72;
            // 
            // reasonforrejectionDataGridViewTextBoxColumn
            // 
            this.reasonforrejectionDataGridViewTextBoxColumn.DataPropertyName = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn.HeaderText = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn.Name = "reasonforrejectionDataGridViewTextBoxColumn";
            this.reasonforrejectionDataGridViewTextBoxColumn.Width = 73;
            // 
            // refidDataGridViewTextBoxColumn
            // 
            this.refidDataGridViewTextBoxColumn.DataPropertyName = "ref_id";
            this.refidDataGridViewTextBoxColumn.HeaderText = "ref_id";
            this.refidDataGridViewTextBoxColumn.Name = "refidDataGridViewTextBoxColumn";
            this.refidDataGridViewTextBoxColumn.Width = 73;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 73;
            // 
            // receivingreportidDataGridViewTextBoxColumn2
            // 
            this.receivingreportidDataGridViewTextBoxColumn2.DataPropertyName = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn2.HeaderText = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn2.Name = "receivingreportidDataGridViewTextBoxColumn2";
            this.receivingreportidDataGridViewTextBoxColumn2.Width = 73;
            // 
            // itemcodeDataGridViewTextBoxColumn1
            // 
            this.itemcodeDataGridViewTextBoxColumn1.DataPropertyName = "item_code";
            this.itemcodeDataGridViewTextBoxColumn1.HeaderText = "item_code";
            this.itemcodeDataGridViewTextBoxColumn1.Name = "itemcodeDataGridViewTextBoxColumn1";
            this.itemcodeDataGridViewTextBoxColumn1.Width = 73;
            // 
            // itemdescriptionDataGridViewTextBoxColumn1
            // 
            this.itemdescriptionDataGridViewTextBoxColumn1.DataPropertyName = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn1.HeaderText = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn1.Name = "itemdescriptionDataGridViewTextBoxColumn1";
            this.itemdescriptionDataGridViewTextBoxColumn1.Width = 72;
            // 
            // orderedqtyDataGridViewTextBoxColumn1
            // 
            this.orderedqtyDataGridViewTextBoxColumn1.DataPropertyName = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn1.HeaderText = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn1.Name = "orderedqtyDataGridViewTextBoxColumn1";
            this.orderedqtyDataGridViewTextBoxColumn1.Width = 73;
            // 
            // ordereduomDataGridViewTextBoxColumn1
            // 
            this.ordereduomDataGridViewTextBoxColumn1.DataPropertyName = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn1.HeaderText = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn1.Name = "ordereduomDataGridViewTextBoxColumn1";
            this.ordereduomDataGridViewTextBoxColumn1.Width = 73;
            // 
            // receivedqtyDataGridViewTextBoxColumn1
            // 
            this.receivedqtyDataGridViewTextBoxColumn1.DataPropertyName = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn1.HeaderText = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn1.Name = "receivedqtyDataGridViewTextBoxColumn1";
            this.receivedqtyDataGridViewTextBoxColumn1.Width = 73;
            // 
            // receiveduomDataGridViewTextBoxColumn1
            // 
            this.receiveduomDataGridViewTextBoxColumn1.DataPropertyName = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn1.HeaderText = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn1.Name = "receiveduomDataGridViewTextBoxColumn1";
            this.receiveduomDataGridViewTextBoxColumn1.Width = 73;
            // 
            // rejectedqtyDataGridViewTextBoxColumn1
            // 
            this.rejectedqtyDataGridViewTextBoxColumn1.DataPropertyName = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn1.HeaderText = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn1.Name = "rejectedqtyDataGridViewTextBoxColumn1";
            this.rejectedqtyDataGridViewTextBoxColumn1.Width = 73;
            // 
            // rejecteduomDataGridViewTextBoxColumn1
            // 
            this.rejecteduomDataGridViewTextBoxColumn1.DataPropertyName = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn1.HeaderText = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn1.Name = "rejecteduomDataGridViewTextBoxColumn1";
            this.rejecteduomDataGridViewTextBoxColumn1.Width = 72;
            // 
            // reasonforrejectionDataGridViewTextBoxColumn1
            // 
            this.reasonforrejectionDataGridViewTextBoxColumn1.DataPropertyName = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn1.HeaderText = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn1.Name = "reasonforrejectionDataGridViewTextBoxColumn1";
            this.reasonforrejectionDataGridViewTextBoxColumn1.Width = 73;
            // 
            // refidDataGridViewTextBoxColumn1
            // 
            this.refidDataGridViewTextBoxColumn1.DataPropertyName = "ref_id";
            this.refidDataGridViewTextBoxColumn1.HeaderText = "ref_id";
            this.refidDataGridViewTextBoxColumn1.Name = "refidDataGridViewTextBoxColumn1";
            this.refidDataGridViewTextBoxColumn1.Width = 73;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "id";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.ReadOnly = true;
            this.idDataGridViewTextBoxColumn1.Width = 73;
            // 
            // receivingreportidDataGridViewTextBoxColumn3
            // 
            this.receivingreportidDataGridViewTextBoxColumn3.DataPropertyName = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn3.HeaderText = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn3.Name = "receivingreportidDataGridViewTextBoxColumn3";
            this.receivingreportidDataGridViewTextBoxColumn3.Width = 73;
            // 
            // itemcodeDataGridViewTextBoxColumn2
            // 
            this.itemcodeDataGridViewTextBoxColumn2.DataPropertyName = "item_code";
            this.itemcodeDataGridViewTextBoxColumn2.HeaderText = "item_code";
            this.itemcodeDataGridViewTextBoxColumn2.Name = "itemcodeDataGridViewTextBoxColumn2";
            this.itemcodeDataGridViewTextBoxColumn2.Width = 73;
            // 
            // itemdescriptionDataGridViewTextBoxColumn2
            // 
            this.itemdescriptionDataGridViewTextBoxColumn2.DataPropertyName = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn2.HeaderText = "item_description";
            this.itemdescriptionDataGridViewTextBoxColumn2.Name = "itemdescriptionDataGridViewTextBoxColumn2";
            this.itemdescriptionDataGridViewTextBoxColumn2.Width = 72;
            // 
            // orderedqtyDataGridViewTextBoxColumn2
            // 
            this.orderedqtyDataGridViewTextBoxColumn2.DataPropertyName = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn2.HeaderText = "ordered_qty";
            this.orderedqtyDataGridViewTextBoxColumn2.Name = "orderedqtyDataGridViewTextBoxColumn2";
            this.orderedqtyDataGridViewTextBoxColumn2.Width = 73;
            // 
            // ordereduomDataGridViewTextBoxColumn2
            // 
            this.ordereduomDataGridViewTextBoxColumn2.DataPropertyName = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn2.HeaderText = "ordered_uom";
            this.ordereduomDataGridViewTextBoxColumn2.Name = "ordereduomDataGridViewTextBoxColumn2";
            this.ordereduomDataGridViewTextBoxColumn2.Width = 73;
            // 
            // receivedqtyDataGridViewTextBoxColumn2
            // 
            this.receivedqtyDataGridViewTextBoxColumn2.DataPropertyName = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn2.HeaderText = "received_qty";
            this.receivedqtyDataGridViewTextBoxColumn2.Name = "receivedqtyDataGridViewTextBoxColumn2";
            this.receivedqtyDataGridViewTextBoxColumn2.Width = 73;
            // 
            // receiveduomDataGridViewTextBoxColumn2
            // 
            this.receiveduomDataGridViewTextBoxColumn2.DataPropertyName = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn2.HeaderText = "received_uom";
            this.receiveduomDataGridViewTextBoxColumn2.Name = "receiveduomDataGridViewTextBoxColumn2";
            this.receiveduomDataGridViewTextBoxColumn2.Width = 73;
            // 
            // rejectedqtyDataGridViewTextBoxColumn2
            // 
            this.rejectedqtyDataGridViewTextBoxColumn2.DataPropertyName = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn2.HeaderText = "rejected_qty";
            this.rejectedqtyDataGridViewTextBoxColumn2.Name = "rejectedqtyDataGridViewTextBoxColumn2";
            this.rejectedqtyDataGridViewTextBoxColumn2.Width = 73;
            // 
            // rejecteduomDataGridViewTextBoxColumn2
            // 
            this.rejecteduomDataGridViewTextBoxColumn2.DataPropertyName = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn2.HeaderText = "rejected_uom";
            this.rejecteduomDataGridViewTextBoxColumn2.Name = "rejecteduomDataGridViewTextBoxColumn2";
            this.rejecteduomDataGridViewTextBoxColumn2.Width = 72;
            // 
            // reasonforrejectionDataGridViewTextBoxColumn2
            // 
            this.reasonforrejectionDataGridViewTextBoxColumn2.DataPropertyName = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn2.HeaderText = "reason_for_rejection";
            this.reasonforrejectionDataGridViewTextBoxColumn2.Name = "reasonforrejectionDataGridViewTextBoxColumn2";
            this.reasonforrejectionDataGridViewTextBoxColumn2.Width = 73;
            // 
            // refidDataGridViewTextBoxColumn2
            // 
            this.refidDataGridViewTextBoxColumn2.DataPropertyName = "ref_id";
            this.refidDataGridViewTextBoxColumn2.HeaderText = "ref_id";
            this.refidDataGridViewTextBoxColumn2.Name = "refidDataGridViewTextBoxColumn2";
            this.refidDataGridViewTextBoxColumn2.Width = 73;
            // 
            // idDataGridViewTextBoxColumn2
            // 
            this.idDataGridViewTextBoxColumn2.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn2.HeaderText = "id";
            this.idDataGridViewTextBoxColumn2.Name = "idDataGridViewTextBoxColumn2";
            this.idDataGridViewTextBoxColumn2.ReadOnly = true;
            this.idDataGridViewTextBoxColumn2.Width = 73;
            // 
            // dg_receiving_report_inventory_item_code1
            // 
            this.dg_receiving_report_inventory_item_code1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_inventory_item_code1.DataPropertyName = "item_code";
            this.dg_receiving_report_inventory_item_code1.HeaderText = "ITEM CODE";
            this.dg_receiving_report_inventory_item_code1.Name = "dg_receiving_report_inventory_item_code1";
            this.dg_receiving_report_inventory_item_code1.ReadOnly = true;
            this.dg_receiving_report_inventory_item_code1.Width = 84;
            // 
            // dg_receiving_report_inventory_item_description
            // 
            this.dg_receiving_report_inventory_item_description.DataPropertyName = "item_description";
            this.dg_receiving_report_inventory_item_description.HeaderText = "ITEM DESCRIPTION";
            this.dg_receiving_report_inventory_item_description.MinimumWidth = 120;
            this.dg_receiving_report_inventory_item_description.Name = "dg_receiving_report_inventory_item_description";
            this.dg_receiving_report_inventory_item_description.ReadOnly = true;
            // 
            // dg_receiving_report_inventory_ordered_qty
            // 
            this.dg_receiving_report_inventory_ordered_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_inventory_ordered_qty.DataPropertyName = "ordered_qty";
            this.dg_receiving_report_inventory_ordered_qty.HeaderText = "QTY";
            this.dg_receiving_report_inventory_ordered_qty.Name = "dg_receiving_report_inventory_ordered_qty";
            this.dg_receiving_report_inventory_ordered_qty.ReadOnly = true;
            this.dg_receiving_report_inventory_ordered_qty.Width = 54;
            // 
            // dg_receiving_report_inventory_ordered_uom
            // 
            this.dg_receiving_report_inventory_ordered_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_inventory_ordered_uom.DataPropertyName = "ordered_uom";
            this.dg_receiving_report_inventory_ordered_uom.HeaderText = "UOM";
            this.dg_receiving_report_inventory_ordered_uom.Name = "dg_receiving_report_inventory_ordered_uom";
            this.dg_receiving_report_inventory_ordered_uom.ReadOnly = true;
            this.dg_receiving_report_inventory_ordered_uom.Width = 57;
            // 
            // dg_receiving_report_inventory_serial_numbers
            // 
            this.dg_receiving_report_inventory_serial_numbers.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dg_receiving_report_inventory_serial_numbers.HeaderText = "SERIAL NUMBERS";
            this.dg_receiving_report_inventory_serial_numbers.MinimumWidth = 120;
            this.dg_receiving_report_inventory_serial_numbers.Name = "dg_receiving_report_inventory_serial_numbers";
            // 
            // dg_receiving_report_inventory_bin_location
            // 
            this.dg_receiving_report_inventory_bin_location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_receiving_report_inventory_bin_location.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dg_receiving_report_inventory_bin_location.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dg_receiving_report_inventory_bin_location.HeaderText = "BIN LOCATION";
            this.dg_receiving_report_inventory_bin_location.MinimumWidth = 185;
            this.dg_receiving_report_inventory_bin_location.Name = "dg_receiving_report_inventory_bin_location";
            this.dg_receiving_report_inventory_bin_location.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_receiving_report_inventory_bin_location.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_receiving_report_inventory_bin_location.Width = 185;
            // 
            // dg_receiving_report_inventory_ref_id
            // 
            this.dg_receiving_report_inventory_ref_id.DataPropertyName = "ref_id";
            this.dg_receiving_report_inventory_ref_id.HeaderText = "ref_id";
            this.dg_receiving_report_inventory_ref_id.Name = "dg_receiving_report_inventory_ref_id";
            this.dg_receiving_report_inventory_ref_id.Visible = false;
            // 
            // dg_receiving_report_inventory_id
            // 
            this.dg_receiving_report_inventory_id.DataPropertyName = "id11111";
            this.dg_receiving_report_inventory_id.HeaderText = "id";
            this.dg_receiving_report_inventory_id.Name = "dg_receiving_report_inventory_id";
            this.dg_receiving_report_inventory_id.ReadOnly = true;
            // 
            // frm_receiving_report_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnl_head);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_header);
            this.MinimumSize = new System.Drawing.Size(706, 400);
            this.Name = "frm_receiving_report_setup";
            this.Size = new System.Drawing.Size(1285, 615);
            this.Load += new System.EventHandler(this.frm_receiving_report_setup_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnl_head.ResumeLayout(false);
            this.pnl_head.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tab_main.ResumeLayout(false);
            this.pnl_purchase_return.ResumeLayout(false);
            this.pnl_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_receiving_report_details)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.tab_inventory.ResumeLayout(false);
            this.pnl_inventory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_receiving_report_inventory)).EndInit();
            this.tab_attachment.ResumeLayout(false);
            this.pnl_attachment_upload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_rr;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_next;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.Panel pnl_head;
        private System.Windows.Forms.Label suppliercodelbl;
        private System.Windows.Forms.TextBox txt_supplier_name;
        private System.Windows.Forms.TextBox txt_doc;
        private System.Windows.Forms.Label supplierlbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_supplier_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_date_received;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_prepared_by;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.Label ref_doclbl;
        private System.Windows.Forms.TextBox txt_supplier_id;
        private System.Windows.Forms.Label lbl_supplier_id;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_inventory;
        private System.Windows.Forms.TabPage tab_main;
        private System.Windows.Forms.Panel pnl_main;
        private System.Windows.Forms.DataGridView dg_receiving_report_details;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.TabPage tab_attachment;
        private System.Windows.Forms.Panel pnl_purchase_return;
        private System.Windows.Forms.Button btn_purchase_return;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.Panel pnl_attachment_upload;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cmb_ref_doc;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.Panel pnl_attachment;
        private System.Windows.Forms.Button btn_setTime;
        private System.Windows.Forms.Panel pnl_inventory;
        private System.Windows.Forms.TextBox txt_purchase_order_id;
        private System.Windows.Forms.Label poIDlbl;
        private System.Windows.Forms.BindingSource bindingSource1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dg_item_description; //wtf are these
        //private System.Windows.Forms.DataGridViewTextBoxColumn dg_ordered_qty;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dg_ordered_uom;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn receiving_report_id_receiving_report_details;
        private System.Data.DataColumn item_code;
        private System.Data.DataColumn item_description;
        private System.Data.DataColumn ordered_qty;
        private System.Data.DataColumn ordered_uom;
        private System.Data.DataColumn received_qty;
        private System.Data.DataColumn received_uom;
        private System.Data.DataColumn rejected_qty;
        private System.Data.DataColumn rejected_uom;
        private System.Data.DataColumn reason_for_rejection;
        private System.Data.DataColumn ref_id;
        private System.Data.DataColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dg_receiving_report_inventory;
        private System.Windows.Forms.ComboBox cmb_address;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Data.DataSet dataSet2;
        private System.Data.DataTable dataTable2;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Data.DataColumn dataColumn9;
        private System.Data.DataColumn dataColumn10;
        private System.Data.DataColumn dataColumn11;
        private System.Data.DataColumn dataColumn12;
        private System.Windows.Forms.ListBox lst_receiving_report_attachment_files;
        private System.Windows.Forms.ImageList pcb_receiving_report_attachment_images;
        private System.Windows.Forms.ComboBox cmb_warehouse_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_item_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_ordered_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_ordered_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_received_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_received_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_rejected_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_rejected_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_reason_for_rejection;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_ref_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_details_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemdescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderedqtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordereduomDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedqtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveduomDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejectedqtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejecteduomDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reasonforrejectionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn refidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportidDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemcodeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemdescriptionDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderedqtyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordereduomDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedqtyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveduomDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejectedqtyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejecteduomDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn reasonforrejectionDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn refidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_item_code1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_item_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_ordered_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_ordered_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_serial_numbers;
        private System.Windows.Forms.DataGridViewComboBoxColumn dg_receiving_report_inventory_bin_location;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_ref_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_receiving_report_inventory_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportidDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemcodeDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemdescriptionDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderedqtyDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordereduomDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedqtyDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveduomDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejectedqtyDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejecteduomDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn reasonforrejectionDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn refidDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn2;
    }
}
