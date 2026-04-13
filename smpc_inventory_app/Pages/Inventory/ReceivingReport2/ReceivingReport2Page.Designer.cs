
namespace smpc_inventory_app.Pages.Inventory.ReceivingReport2
{
    partial class ReceivingReport2Page
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
            if (disposing)
            {
                _binLocationOverlay?.Dispose();  // Add this line
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceivingReport2Page));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_print = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.pnl_main = new System.Windows.Forms.Panel();
            this.txt_ref_doc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_warehouse_id = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_ref_doc = new System.Windows.Forms.ComboBox();
            this.txt_doc_no = new System.Windows.Forms.TextBox();
            this.txt_prepared_by = new System.Windows.Forms.TextBox();
            this.txt_warehouse_address = new System.Windows.Forms.TextBox();
            this.txt_supplier_code = new System.Windows.Forms.TextBox();
            this.txt_supplier = new System.Windows.Forms.TextBox();
            this.dtp_date_received = new System.Windows.Forms.DateTimePicker();
            this.cmb_warehouse = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_purchase_order_id = new System.Windows.Forms.TextBox();
            this.lbl_po_id = new System.Windows.Forms.Label();
            this.txt_supplier_id = new System.Windows.Forms.TextBox();
            this.lbl_supplier_id = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_rr_id = new System.Windows.Forms.Label();
            this.ref_doclbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.suppliercodelbl = new System.Windows.Forms.Label();
            this.supplierlbl = new System.Windows.Forms.Label();
            this.pnl_purchase_return = new System.Windows.Forms.Panel();
            this.btn_purchase_return = new System.Windows.Forms.Button();
            this.tbc_main = new System.Windows.Forms.TabControl();
            this.main = new System.Windows.Forms.TabPage();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.attachment = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnl_Receiving = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.RECEIVING_LV = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TV1_preview = new System.Windows.Forms.Panel();
            this.label29 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.RECEIVING_TV = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.treeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameFileItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiving_report_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_order_details_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordered_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordered_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remaining_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remaining_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.received_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.received_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serial_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bin_location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejected_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rejected_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reason_for_rejection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnl_main.SuspendLayout();
            this.pnl_purchase_return.SuspendLayout();
            this.tbc_main.SuspendLayout();
            this.main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            this.attachment.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnl_Receiving.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.TV1_preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1285, 47);
            this.panel6.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(18, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Receiving Report";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_search,
            this.btn_edit,
            this.btn_delete,
            this.btn_print,
            this.btn_save,
            this.btn_cancel,
            this.btn_next,
            this.btn_prev});
            this.toolStrip1.Location = new System.Drawing.Point(0, 47);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1285, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn_new
            // 
            this.btn_new.Image = ((System.Drawing.Image)(resources.GetObject("btn_new.Image")));
            this.btn_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(51, 22);
            this.btn_new.Text = "New";
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_search
            // 
            this.btn_search.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.Image")));
            this.btn_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(62, 22);
            this.btn_search.Text = "Search";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Image = ((System.Drawing.Image)(resources.GetObject("btn_edit.Image")));
            this.btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(47, 22);
            this.btn_edit.Text = "Edit";
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
            // btn_print
            // 
            this.btn_print.Image = ((System.Drawing.Image)(resources.GetObject("btn_print.Image")));
            this.btn_print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(52, 22);
            this.btn_print.Text = "Print";
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(51, 22);
            this.btn_save.Text = "Save";
            this.btn_save.Visible = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(63, 22);
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_next
            // 
            this.btn_next.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_next.Image = ((System.Drawing.Image)(resources.GetObject("btn_next.Image")));
            this.btn_next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(52, 22);
            this.btn_next.Text = "Next";
            this.btn_next.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_prev
            // 
            this.btn_prev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_prev.Image = ((System.Drawing.Image)(resources.GetObject("btn_prev.Image")));
            this.btn_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(72, 22);
            this.btn_prev.Text = "Previous";
            this.btn_prev.Click += new System.EventHandler(this.btn_prev_Click);
            // 
            // pnl_main
            // 
            this.pnl_main.Controls.Add(this.txt_ref_doc);
            this.pnl_main.Controls.Add(this.label8);
            this.pnl_main.Controls.Add(this.txt_warehouse_id);
            this.pnl_main.Controls.Add(this.label6);
            this.pnl_main.Controls.Add(this.cmb_ref_doc);
            this.pnl_main.Controls.Add(this.txt_doc_no);
            this.pnl_main.Controls.Add(this.txt_prepared_by);
            this.pnl_main.Controls.Add(this.txt_warehouse_address);
            this.pnl_main.Controls.Add(this.txt_supplier_code);
            this.pnl_main.Controls.Add(this.txt_supplier);
            this.pnl_main.Controls.Add(this.dtp_date_received);
            this.pnl_main.Controls.Add(this.cmb_warehouse);
            this.pnl_main.Controls.Add(this.label2);
            this.pnl_main.Controls.Add(this.txt_purchase_order_id);
            this.pnl_main.Controls.Add(this.lbl_po_id);
            this.pnl_main.Controls.Add(this.txt_supplier_id);
            this.pnl_main.Controls.Add(this.lbl_supplier_id);
            this.pnl_main.Controls.Add(this.label5);
            this.pnl_main.Controls.Add(this.label7);
            this.pnl_main.Controls.Add(this.txt_id);
            this.pnl_main.Controls.Add(this.lbl_rr_id);
            this.pnl_main.Controls.Add(this.ref_doclbl);
            this.pnl_main.Controls.Add(this.label4);
            this.pnl_main.Controls.Add(this.label3);
            this.pnl_main.Controls.Add(this.suppliercodelbl);
            this.pnl_main.Controls.Add(this.supplierlbl);
            this.pnl_main.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_main.Location = new System.Drawing.Point(0, 72);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Size = new System.Drawing.Size(1285, 132);
            this.pnl_main.TabIndex = 29;
            // 
            // txt_ref_doc
            // 
            this.txt_ref_doc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_ref_doc.Enabled = false;
            this.txt_ref_doc.Location = new System.Drawing.Point(615, 4);
            this.txt_ref_doc.Name = "txt_ref_doc";
            this.txt_ref_doc.Size = new System.Drawing.Size(200, 20);
            this.txt_ref_doc.TabIndex = 316;
            this.txt_ref_doc.TabStop = false;
            this.txt_ref_doc.Tag = "";
            this.txt_ref_doc.Visible = false;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(548, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 315;
            this.label8.Text = "REF DOC :";
            this.label8.Visible = false;
            // 
            // txt_warehouse_id
            // 
            this.txt_warehouse_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_warehouse_id.Enabled = false;
            this.txt_warehouse_id.Location = new System.Drawing.Point(615, 88);
            this.txt_warehouse_id.Name = "txt_warehouse_id";
            this.txt_warehouse_id.Size = new System.Drawing.Size(200, 20);
            this.txt_warehouse_id.TabIndex = 314;
            this.txt_warehouse_id.TabStop = false;
            this.txt_warehouse_id.Tag = "";
            this.txt_warehouse_id.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(510, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 313;
            this.label6.Text = "WAREHOUSE ID :";
            this.label6.Visible = false;
            // 
            // cmb_ref_doc
            // 
            this.cmb_ref_doc.BackColor = System.Drawing.Color.White;
            this.cmb_ref_doc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ref_doc.Enabled = false;
            this.cmb_ref_doc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_ref_doc.FormattingEnabled = true;
            this.cmb_ref_doc.Location = new System.Drawing.Point(959, 29);
            this.cmb_ref_doc.MaxLength = 50;
            this.cmb_ref_doc.MinimumSize = new System.Drawing.Size(200, 0);
            this.cmb_ref_doc.Name = "cmb_ref_doc";
            this.cmb_ref_doc.Size = new System.Drawing.Size(289, 21);
            this.cmb_ref_doc.TabIndex = 310;
            this.cmb_ref_doc.TabStop = false;
            this.cmb_ref_doc.Tag = "REQUIRED";
            this.cmb_ref_doc.SelectedIndexChanged += new System.EventHandler(this.cmb_ref_doc_SelectedIndexChanged);
            // 
            // txt_doc_no
            // 
            this.txt_doc_no.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.txt_doc_no.Location = new System.Drawing.Point(959, 7);
            this.txt_doc_no.Name = "txt_doc_no";
            this.txt_doc_no.ReadOnly = true;
            this.txt_doc_no.Size = new System.Drawing.Size(289, 20);
            this.txt_doc_no.TabIndex = 309;
            this.txt_doc_no.Tag = "DOCUMENTRR";
            // 
            // txt_prepared_by
            // 
            this.txt_prepared_by.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.txt_prepared_by.Location = new System.Drawing.Point(959, 73);
            this.txt_prepared_by.Name = "txt_prepared_by";
            this.txt_prepared_by.ReadOnly = true;
            this.txt_prepared_by.Size = new System.Drawing.Size(289, 20);
            this.txt_prepared_by.TabIndex = 308;
            this.txt_prepared_by.Tag = "";
            // 
            // txt_warehouse_address
            // 
            this.txt_warehouse_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.txt_warehouse_address.Location = new System.Drawing.Point(167, 94);
            this.txt_warehouse_address.Name = "txt_warehouse_address";
            this.txt_warehouse_address.ReadOnly = true;
            this.txt_warehouse_address.Size = new System.Drawing.Size(289, 20);
            this.txt_warehouse_address.TabIndex = 307;
            this.txt_warehouse_address.Tag = "REQUIRED";
            // 
            // txt_supplier_code
            // 
            this.txt_supplier_code.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.txt_supplier_code.Location = new System.Drawing.Point(167, 29);
            this.txt_supplier_code.Name = "txt_supplier_code";
            this.txt_supplier_code.ReadOnly = true;
            this.txt_supplier_code.Size = new System.Drawing.Size(289, 20);
            this.txt_supplier_code.TabIndex = 306;
            this.txt_supplier_code.Tag = "REQUIRED";
            // 
            // txt_supplier
            // 
            this.txt_supplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.txt_supplier.Location = new System.Drawing.Point(167, 8);
            this.txt_supplier.Name = "txt_supplier";
            this.txt_supplier.ReadOnly = true;
            this.txt_supplier.Size = new System.Drawing.Size(289, 20);
            this.txt_supplier.TabIndex = 305;
            this.txt_supplier.Tag = "REQUIRED";
            // 
            // dtp_date_received
            // 
            this.dtp_date_received.Enabled = false;
            this.dtp_date_received.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_date_received.Location = new System.Drawing.Point(167, 50);
            this.dtp_date_received.Name = "dtp_date_received";
            this.dtp_date_received.Size = new System.Drawing.Size(289, 20);
            this.dtp_date_received.TabIndex = 304;
            this.dtp_date_received.Tag = "REQUIRED";
            // 
            // cmb_warehouse
            // 
            this.cmb_warehouse.BackColor = System.Drawing.Color.White;
            this.cmb_warehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_warehouse.Enabled = false;
            this.cmb_warehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_warehouse.FormattingEnabled = true;
            this.cmb_warehouse.Location = new System.Drawing.Point(167, 72);
            this.cmb_warehouse.MaxLength = 50;
            this.cmb_warehouse.MinimumSize = new System.Drawing.Size(200, 0);
            this.cmb_warehouse.Name = "cmb_warehouse";
            this.cmb_warehouse.Size = new System.Drawing.Size(289, 21);
            this.cmb_warehouse.TabIndex = 303;
            this.cmb_warehouse.TabStop = false;
            this.cmb_warehouse.Tag = "REQUIRED";
            this.cmb_warehouse.SelectedIndexChanged += new System.EventHandler(this.cmb_warehouse_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "WAREHOUSE :";
            // 
            // txt_purchase_order_id
            // 
            this.txt_purchase_order_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_purchase_order_id.Enabled = false;
            this.txt_purchase_order_id.Location = new System.Drawing.Point(615, 46);
            this.txt_purchase_order_id.Name = "txt_purchase_order_id";
            this.txt_purchase_order_id.Size = new System.Drawing.Size(200, 20);
            this.txt_purchase_order_id.TabIndex = 40;
            this.txt_purchase_order_id.TabStop = false;
            this.txt_purchase_order_id.Tag = "";
            this.txt_purchase_order_id.Visible = false;
            // 
            // lbl_po_id
            // 
            this.lbl_po_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_po_id.AutoSize = true;
            this.lbl_po_id.Location = new System.Drawing.Point(567, 53);
            this.lbl_po_id.Name = "lbl_po_id";
            this.lbl_po_id.Size = new System.Drawing.Size(42, 13);
            this.lbl_po_id.TabIndex = 39;
            this.lbl_po_id.Text = "PO ID :";
            this.lbl_po_id.Visible = false;
            // 
            // txt_supplier_id
            // 
            this.txt_supplier_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_supplier_id.Enabled = false;
            this.txt_supplier_id.Location = new System.Drawing.Point(615, 67);
            this.txt_supplier_id.Name = "txt_supplier_id";
            this.txt_supplier_id.Size = new System.Drawing.Size(200, 20);
            this.txt_supplier_id.TabIndex = 35;
            this.txt_supplier_id.TabStop = false;
            this.txt_supplier_id.Tag = "";
            this.txt_supplier_id.Visible = false;
            // 
            // lbl_supplier_id
            // 
            this.lbl_supplier_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_supplier_id.AutoSize = true;
            this.lbl_supplier_id.Location = new System.Drawing.Point(529, 73);
            this.lbl_supplier_id.Name = "lbl_supplier_id";
            this.lbl_supplier_id.Size = new System.Drawing.Size(80, 13);
            this.lbl_supplier_id.TabIndex = 34;
            this.lbl_supplier_id.Text = "SUPPLIER ID :";
            this.lbl_supplier_id.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(898, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "DOC NO :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(864, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "PREPARED BY :";
            // 
            // txt_id
            // 
            this.txt_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_id.Enabled = false;
            this.txt_id.Location = new System.Drawing.Point(615, 25);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 30;
            this.txt_id.TabStop = false;
            this.txt_id.Tag = "";
            this.txt_id.Visible = false;
            // 
            // lbl_rr_id
            // 
            this.lbl_rr_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_rr_id.AutoSize = true;
            this.lbl_rr_id.Location = new System.Drawing.Point(585, 28);
            this.lbl_rr_id.Name = "lbl_rr_id";
            this.lbl_rr_id.Size = new System.Drawing.Size(24, 13);
            this.lbl_rr_id.TabIndex = 29;
            this.lbl_rr_id.Text = "ID :";
            this.lbl_rr_id.Visible = false;
            // 
            // ref_doclbl
            // 
            this.ref_doclbl.AutoSize = true;
            this.ref_doclbl.Location = new System.Drawing.Point(850, 32);
            this.ref_doclbl.Name = "ref_doclbl";
            this.ref_doclbl.Size = new System.Drawing.Size(104, 13);
            this.ref_doclbl.TabIndex = 28;
            this.ref_doclbl.Text = "REFERENCE DOC :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "WAREHOUSE ADDRESS :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "DATE RECEIVED :";
            // 
            // suppliercodelbl
            // 
            this.suppliercodelbl.AutoSize = true;
            this.suppliercodelbl.Location = new System.Drawing.Point(118, 32);
            this.suppliercodelbl.Name = "suppliercodelbl";
            this.suppliercodelbl.Size = new System.Drawing.Size(43, 13);
            this.suppliercodelbl.TabIndex = 21;
            this.suppliercodelbl.Text = "CODE :";
            // 
            // supplierlbl
            // 
            this.supplierlbl.AutoSize = true;
            this.supplierlbl.Location = new System.Drawing.Point(95, 11);
            this.supplierlbl.Name = "supplierlbl";
            this.supplierlbl.Size = new System.Drawing.Size(66, 13);
            this.supplierlbl.TabIndex = 14;
            this.supplierlbl.Text = "SUPPLIER :";
            // 
            // pnl_purchase_return
            // 
            this.pnl_purchase_return.Controls.Add(this.btn_purchase_return);
            this.pnl_purchase_return.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_purchase_return.Location = new System.Drawing.Point(0, 520);
            this.pnl_purchase_return.Name = "pnl_purchase_return";
            this.pnl_purchase_return.Size = new System.Drawing.Size(1285, 95);
            this.pnl_purchase_return.TabIndex = 38;
            // 
            // btn_purchase_return
            // 
            this.btn_purchase_return.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_purchase_return.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_purchase_return.Location = new System.Drawing.Point(1151, 30);
            this.btn_purchase_return.Name = "btn_purchase_return";
            this.btn_purchase_return.Size = new System.Drawing.Size(104, 23);
            this.btn_purchase_return.TabIndex = 35;
            this.btn_purchase_return.Text = "Purchase Return";
            this.btn_purchase_return.UseVisualStyleBackColor = false;
            this.btn_purchase_return.Visible = false;
            // 
            // tbc_main
            // 
            this.tbc_main.Controls.Add(this.main);
            this.tbc_main.Controls.Add(this.attachment);
            this.tbc_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbc_main.Location = new System.Drawing.Point(0, 204);
            this.tbc_main.Name = "tbc_main";
            this.tbc_main.SelectedIndex = 0;
            this.tbc_main.Size = new System.Drawing.Size(1285, 316);
            this.tbc_main.TabIndex = 39;
            // 
            // main
            // 
            this.main.Controls.Add(this.dgv_main);
            this.main.Location = new System.Drawing.Point(4, 22);
            this.main.Name = "main";
            this.main.Padding = new System.Windows.Forms.Padding(3);
            this.main.Size = new System.Drawing.Size(1277, 290);
            this.main.TabIndex = 0;
            this.main.Text = "MAIN";
            this.main.UseVisualStyleBackColor = true;
            // 
            // dgv_main
            // 
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AllowUserToDeleteRows = false;
            this.dgv_main.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_main.ColumnHeadersHeight = 50;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.number,
            this.receiving_report_id,
            this.id,
            this.purchase_order_details_id,
            this.warehouse_id,
            this.item_id,
            this.item_code,
            this.item_desc,
            this.ordered_qty,
            this.ordered_uom,
            this.remaining_qty,
            this.remaining_uom,
            this.received_qty,
            this.received_uom,
            this.serial_number,
            this.bin_location,
            this.rejected_qty,
            this.rejected_uom,
            this.reason_for_rejection});
            this.dgv_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_main.EnableHeadersVisualStyles = false;
            this.dgv_main.Location = new System.Drawing.Point(3, 3);
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.Size = new System.Drawing.Size(1271, 284);
            this.dgv_main.TabIndex = 0;
            this.dgv_main.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellValueChanged);
            this.dgv_main.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_main_DataError);
            this.dgv_main.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgv_main_EditingControlShowing);
            this.dgv_main.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_main_RowPostPaint);
            // 
            // attachment
            // 
            this.attachment.Controls.Add(this.panel1);
            this.attachment.Location = new System.Drawing.Point(4, 22);
            this.attachment.Name = "attachment";
            this.attachment.Padding = new System.Windows.Forms.Padding(3);
            this.attachment.Size = new System.Drawing.Size(1277, 290);
            this.attachment.TabIndex = 2;
            this.attachment.Text = "ATTACHMENT";
            this.attachment.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1271, 284);
            this.panel1.TabIndex = 0;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnl_Receiving);
            this.panel3.Controls.Add(this.btnUpload);
            this.panel3.Controls.Add(this.RECEIVING_LV);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(621, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(650, 284);
            this.panel3.TabIndex = 1;
            // 
            // pnl_Receiving
            // 
            this.pnl_Receiving.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Receiving.Controls.Add(this.label27);
            this.pnl_Receiving.Controls.Add(this.pictureBox1);
            this.pnl_Receiving.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Receiving.Location = new System.Drawing.Point(0, 0);
            this.pnl_Receiving.Name = "pnl_Receiving";
            this.pnl_Receiving.Size = new System.Drawing.Size(650, 284);
            this.pnl_Receiving.TabIndex = 29;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(243, 165);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(179, 23);
            this.label27.TabIndex = 1;
            this.label27.Text = "Please select a folder";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::smpc_inventory_app.Properties.Resources.FolderIcon;
            this.pictureBox1.Location = new System.Drawing.Point(263, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnUpload.Location = new System.Drawing.Point(552, 223);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // RECEIVING_LV
            // 
            this.RECEIVING_LV.AllowDrop = true;
            this.RECEIVING_LV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RECEIVING_LV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RECEIVING_LV.FullRowSelect = true;
            this.RECEIVING_LV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.RECEIVING_LV.HideSelection = false;
            this.RECEIVING_LV.HoverSelection = true;
            this.RECEIVING_LV.Location = new System.Drawing.Point(0, 0);
            this.RECEIVING_LV.MultiSelect = false;
            this.RECEIVING_LV.Name = "RECEIVING_LV";
            this.RECEIVING_LV.Size = new System.Drawing.Size(650, 284);
            this.RECEIVING_LV.TabIndex = 2;
            this.RECEIVING_LV.UseCompatibleStateImageBehavior = false;
            this.RECEIVING_LV.View = System.Windows.Forms.View.Details;
            this.RECEIVING_LV.DoubleClick += new System.EventHandler(this.RECEIVING_LV_DoubleClick);
            this.RECEIVING_LV.MouseEnter += new System.EventHandler(this.RECEIVING_LV_MouseEnter);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TV1_preview);
            this.panel2.Controls.Add(this.RECEIVING_TV);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(598, 284);
            this.panel2.TabIndex = 0;
            // 
            // TV1_preview
            // 
            this.TV1_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TV1_preview.Controls.Add(this.label29);
            this.TV1_preview.Controls.Add(this.pictureBox3);
            this.TV1_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TV1_preview.Location = new System.Drawing.Point(0, 0);
            this.TV1_preview.Name = "TV1_preview";
            this.TV1_preview.Size = new System.Drawing.Size(598, 284);
            this.TV1_preview.TabIndex = 26;
            this.TV1_preview.Visible = false;
            this.TV1_preview.Resize += new System.EventHandler(this.TV1_preview_Resize);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(169, 165);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(260, 23);
            this.label29.TabIndex = 4;
            this.label29.Text = "Directory will open when saved";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::smpc_inventory_app.Properties.Resources.search;
            this.pictureBox3.Location = new System.Drawing.Point(213, 73);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(151, 79);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // RECEIVING_TV
            // 
            this.RECEIVING_TV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RECEIVING_TV.Location = new System.Drawing.Point(0, 0);
            this.RECEIVING_TV.Name = "RECEIVING_TV";
            this.RECEIVING_TV.Size = new System.Drawing.Size(598, 284);
            this.RECEIVING_TV.TabIndex = 3;
            this.RECEIVING_TV.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RECEIVING_TV_AfterSelect);
            this.RECEIVING_TV.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RECEIVING_TV_MouseDown);
            // 
            // treeViewContextMenu
            // 
            this.treeViewContextMenu.Name = "treeViewContextMenu";
            this.treeViewContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // renameFileItem
            // 
            this.renameFileItem.Name = "renameFileItem";
            this.renameFileItem.Size = new System.Drawing.Size(61, 4);
            // 
            // number
            // 
            this.number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.number.DataPropertyName = "number";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.number.DefaultCellStyle = dataGridViewCellStyle2;
            this.number.HeaderText = "#";
            this.number.Name = "number";
            this.number.ReadOnly = true;
            this.number.Width = 50;
            // 
            // receiving_report_id
            // 
            this.receiving_report_id.DataPropertyName = "receiving_report_id";
            this.receiving_report_id.HeaderText = "RR ID";
            this.receiving_report_id.Name = "receiving_report_id";
            this.receiving_report_id.ReadOnly = true;
            this.receiving_report_id.Visible = false;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // purchase_order_details_id
            // 
            this.purchase_order_details_id.DataPropertyName = "purchase_order_details_id";
            this.purchase_order_details_id.HeaderText = "POD ID";
            this.purchase_order_details_id.Name = "purchase_order_details_id";
            this.purchase_order_details_id.ReadOnly = true;
            this.purchase_order_details_id.Visible = false;
            // 
            // warehouse_id
            // 
            this.warehouse_id.DataPropertyName = "warehouse_id";
            this.warehouse_id.HeaderText = "WAREHOUSE ID";
            this.warehouse_id.Name = "warehouse_id";
            this.warehouse_id.ReadOnly = true;
            this.warehouse_id.Visible = false;
            // 
            // item_id
            // 
            this.item_id.DataPropertyName = "item_id";
            this.item_id.HeaderText = "ITEM ID";
            this.item_id.Name = "item_id";
            this.item_id.ReadOnly = true;
            this.item_id.Visible = false;
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "item_code";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            this.item_code.DefaultCellStyle = dataGridViewCellStyle3;
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // item_desc
            // 
            this.item_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_desc.DataPropertyName = "item_desc";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            this.item_desc.DefaultCellStyle = dataGridViewCellStyle4;
            this.item_desc.HeaderText = "ITEM DESCRIPTION";
            this.item_desc.Name = "item_desc";
            this.item_desc.ReadOnly = true;
            // 
            // ordered_qty
            // 
            this.ordered_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ordered_qty.DataPropertyName = "ordered_qty";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            this.ordered_qty.DefaultCellStyle = dataGridViewCellStyle5;
            this.ordered_qty.HeaderText = "QTY";
            this.ordered_qty.Name = "ordered_qty";
            this.ordered_qty.ReadOnly = true;
            this.ordered_qty.Width = 60;
            // 
            // ordered_uom
            // 
            this.ordered_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ordered_uom.DataPropertyName = "ordered_uom";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro;
            this.ordered_uom.DefaultCellStyle = dataGridViewCellStyle6;
            this.ordered_uom.HeaderText = "UOM";
            this.ordered_uom.Name = "ordered_uom";
            this.ordered_uom.ReadOnly = true;
            this.ordered_uom.Width = 60;
            // 
            // remaining_qty
            // 
            this.remaining_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remaining_qty.DataPropertyName = "remaining_qty";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Gainsboro;
            this.remaining_qty.DefaultCellStyle = dataGridViewCellStyle7;
            this.remaining_qty.HeaderText = "QTY";
            this.remaining_qty.Name = "remaining_qty";
            this.remaining_qty.ReadOnly = true;
            this.remaining_qty.Width = 60;
            // 
            // remaining_uom
            // 
            this.remaining_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remaining_uom.DataPropertyName = "remaining_uom";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Gainsboro;
            this.remaining_uom.DefaultCellStyle = dataGridViewCellStyle8;
            this.remaining_uom.HeaderText = "UOM";
            this.remaining_uom.Name = "remaining_uom";
            this.remaining_uom.ReadOnly = true;
            this.remaining_uom.Width = 60;
            // 
            // received_qty
            // 
            this.received_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.received_qty.DataPropertyName = "received_qty";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Gainsboro;
            this.received_qty.DefaultCellStyle = dataGridViewCellStyle9;
            this.received_qty.HeaderText = "QTY";
            this.received_qty.Name = "received_qty";
            this.received_qty.ReadOnly = true;
            this.received_qty.Width = 60;
            // 
            // received_uom
            // 
            this.received_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.received_uom.DataPropertyName = "received_uom";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Gainsboro;
            this.received_uom.DefaultCellStyle = dataGridViewCellStyle10;
            this.received_uom.HeaderText = "UOM";
            this.received_uom.Name = "received_uom";
            this.received_uom.ReadOnly = true;
            this.received_uom.Width = 60;
            // 
            // serial_number
            // 
            this.serial_number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serial_number.DataPropertyName = "serial_number";
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Gainsboro;
            this.serial_number.DefaultCellStyle = dataGridViewCellStyle11;
            this.serial_number.HeaderText = "SERIAL NUMBER/S";
            this.serial_number.Name = "serial_number";
            this.serial_number.ReadOnly = true;
            // 
            // bin_location
            // 
            this.bin_location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.bin_location.DataPropertyName = "bin_location";
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Gainsboro;
            this.bin_location.DefaultCellStyle = dataGridViewCellStyle12;
            this.bin_location.HeaderText = "BIN LOCATION";
            this.bin_location.Name = "bin_location";
            this.bin_location.ReadOnly = true;
            // 
            // rejected_qty
            // 
            this.rejected_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.rejected_qty.DataPropertyName = "rejected_qty";
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Gainsboro;
            this.rejected_qty.DefaultCellStyle = dataGridViewCellStyle13;
            this.rejected_qty.HeaderText = "QTY";
            this.rejected_qty.Name = "rejected_qty";
            this.rejected_qty.ReadOnly = true;
            this.rejected_qty.Width = 60;
            // 
            // rejected_uom
            // 
            this.rejected_uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.rejected_uom.DataPropertyName = "rejected_uom";
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Gainsboro;
            this.rejected_uom.DefaultCellStyle = dataGridViewCellStyle14;
            this.rejected_uom.HeaderText = "UOM";
            this.rejected_uom.Name = "rejected_uom";
            this.rejected_uom.ReadOnly = true;
            this.rejected_uom.Width = 60;
            // 
            // reason_for_rejection
            // 
            this.reason_for_rejection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.reason_for_rejection.DataPropertyName = "reason_for_rejection";
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.Gainsboro;
            this.reason_for_rejection.DefaultCellStyle = dataGridViewCellStyle15;
            this.reason_for_rejection.HeaderText = "REASON FOR REJECTION";
            this.reason_for_rejection.Name = "reason_for_rejection";
            this.reason_for_rejection.ReadOnly = true;
            // 
            // ReceivingReport2Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbc_main);
            this.Controls.Add(this.pnl_purchase_return);
            this.Controls.Add(this.pnl_main);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel6);
            this.Name = "ReceivingReport2Page";
            this.Size = new System.Drawing.Size(1285, 615);
            this.Load += new System.EventHandler(this.ReceivingReport2Page_Load);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnl_main.ResumeLayout(false);
            this.pnl_main.PerformLayout();
            this.pnl_purchase_return.ResumeLayout(false);
            this.tbc_main.ResumeLayout(false);
            this.main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            this.attachment.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnl_Receiving.ResumeLayout(false);
            this.pnl_Receiving.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.TV1_preview.ResumeLayout(false);
            this.TV1_preview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_print;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ToolStripButton btn_next;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.Panel pnl_main;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_purchase_order_id;
        private System.Windows.Forms.Label lbl_po_id;
        private System.Windows.Forms.TextBox txt_supplier_id;
        private System.Windows.Forms.Label lbl_supplier_id;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_rr_id;
        private System.Windows.Forms.Label ref_doclbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label suppliercodelbl;
        private System.Windows.Forms.Label supplierlbl;
        private System.Windows.Forms.Panel pnl_purchase_return;
        private System.Windows.Forms.Button btn_purchase_return;
        private System.Windows.Forms.TabControl tbc_main;
        private System.Windows.Forms.TabPage main;
        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.TabPage attachment;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnl_Receiving;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ListView RECEIVING_LV;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel TV1_preview;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TreeView RECEIVING_TV;
        private System.Windows.Forms.ComboBox cmb_warehouse;
        private System.Windows.Forms.TextBox txt_supplier_code;
        private System.Windows.Forms.TextBox txt_supplier;
        private System.Windows.Forms.DateTimePicker dtp_date_received;
        private System.Windows.Forms.ComboBox cmb_ref_doc;
        private System.Windows.Forms.TextBox txt_doc_no;
        private System.Windows.Forms.TextBox txt_prepared_by;
        private System.Windows.Forms.TextBox txt_warehouse_address;
        private System.Windows.Forms.TextBox txt_warehouse_id;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip treeViewContextMenu;
        private System.Windows.Forms.ContextMenuStrip renameFileItem;
        private System.Windows.Forms.TextBox txt_ref_doc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiving_report_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_order_details_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordered_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordered_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn remaining_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn remaining_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn received_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn received_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn serial_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn bin_location;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejected_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn rejected_uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn reason_for_rejection;
    }
}
