
namespace smpc_inventory_app.Pages
{
    partial class bom
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bom));
            this.pnl_title = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnl_label = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pnl_components = new System.Windows.Forms.Panel();
            this.dg_bom = new System.Windows.Forms.DataGridView();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.short_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_bom_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bom_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uom_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.net_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataBindingBomComponents = new System.Windows.Forms.BindingSource(this.components);
            this.ds_bom_components = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
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
            this.pnl_footer = new System.Windows.Forms.Panel();
            this.btn_production_order = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_item_model = new System.Windows.Forms.TextBox();
            this.txt_production_qty = new System.Windows.Forms.TextBox();
            this.cmb_production_type = new System.Windows.Forms.ComboBox();
            this.pnl_header = new System.Windows.Forms.Panel();
            this.txt_labor_rate = new System.Windows.Forms.TextBox();
            this.txt_man_days = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_production_cost = new System.Windows.Forms.TextBox();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.txt_item_id = new System.Windows.Forms.TextBox();
            this.txt_general_name = new System.Windows.Forms.TextBox();
            this.txt_item_code = new System.Windows.Forms.TextBox();
            this.btn_get_item = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_close = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnl_title.SuspendLayout();
            this.pnl_label.SuspendLayout();
            this.pnl_components.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_bom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBindingBomComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_bom_components)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.pnl_header.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_title
            // 
            this.pnl_title.Controls.Add(this.label1);
            this.pnl_title.Location = new System.Drawing.Point(0, 1);
            this.pnl_title.Name = "pnl_title";
            this.pnl_title.Size = new System.Drawing.Size(1282, 40);
            this.pnl_title.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "BOM";
            // 
            // pnl_label
            // 
            this.pnl_label.Controls.Add(this.label8);
            this.pnl_label.Location = new System.Drawing.Point(0, 179);
            this.pnl_label.Name = "pnl_label";
            this.pnl_label.Size = new System.Drawing.Size(1282, 38);
            this.pnl_label.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(9, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "COMPONENTS:";
            // 
            // pnl_components
            // 
            this.pnl_components.Controls.Add(this.dg_bom);
            this.pnl_components.Controls.Add(this.pnl_footer);
            this.pnl_components.Location = new System.Drawing.Point(0, 212);
            this.pnl_components.Name = "pnl_components";
            this.pnl_components.Size = new System.Drawing.Size(1282, 323);
            this.pnl_components.TabIndex = 4;
            // 
            // dg_bom
            // 
            this.dg_bom.AutoGenerateColumns = false;
            this.dg_bom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_bom.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.item_code,
            this.short_desc,
            this.item_id,
            this.item_bom_id,
            this.size,
            this.bom_qty,
            this.uom_name,
            this.unit_price,
            this.net_price,
            this.id});
            this.dg_bom.DataSource = this.dataBindingBomComponents;
            this.dg_bom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_bom.Location = new System.Drawing.Point(0, 0);
            this.dg_bom.Name = "dg_bom";
            this.dg_bom.Size = new System.Drawing.Size(1282, 323);
            this.dg_bom.TabIndex = 36;
            this.dg_bom.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_bom_CellClick);
            this.dg_bom.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_bom_CellEndEdit);
            this.dg_bom.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dg_bom_EditingControlShowing);
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // short_desc
            // 
            this.short_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.short_desc.DataPropertyName = "short_desc";
            this.short_desc.HeaderText = "ITEM DESCRIPTION";
            this.short_desc.Name = "short_desc";
            this.short_desc.ReadOnly = true;
            // 
            // item_id
            // 
            this.item_id.DataPropertyName = "item_id";
            this.item_id.HeaderText = "item_id";
            this.item_id.Name = "item_id";
            this.item_id.Visible = false;
            // 
            // item_bom_id
            // 
            this.item_bom_id.DataPropertyName = "item_bom_id";
            this.item_bom_id.HeaderText = "item_bom_id";
            this.item_bom_id.Name = "item_bom_id";
            this.item_bom_id.Visible = false;
            // 
            // size
            // 
            this.size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.size.DataPropertyName = "size";
            this.size.HeaderText = "SIZE";
            this.size.Name = "size";
            this.size.ReadOnly = true;
            // 
            // bom_qty
            // 
            this.bom_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.bom_qty.DataPropertyName = "bom_qty";
            this.bom_qty.FillWeight = 50F;
            this.bom_qty.HeaderText = "QTY";
            this.bom_qty.Name = "bom_qty";
            // 
            // uom_name
            // 
            this.uom_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.uom_name.DataPropertyName = "uom_name";
            this.uom_name.FillWeight = 60F;
            this.uom_name.HeaderText = "UOM";
            this.uom_name.Name = "uom_name";
            this.uom_name.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle3;
            this.unit_price.FillWeight = 60F;
            this.unit_price.HeaderText = "UNIT PRICE";
            this.unit_price.Name = "unit_price";
            // 
            // net_price
            // 
            this.net_price.DataPropertyName = "net_price";
            this.net_price.HeaderText = "NET PRICE";
            this.net_price.Name = "net_price";
            this.net_price.ReadOnly = true;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // dataBindingBomComponents
            // 
            this.dataBindingBomComponents.DataMember = "TblBomComponents";
            this.dataBindingBomComponents.DataSource = this.ds_bom_components;
            // 
            // ds_bom_components
            // 
            this.ds_bom_components.DataSetName = "NewDataSet";
            this.ds_bom_components.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10});
            this.dataTable1.TableName = "TblBomComponents";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "id";
            this.dataColumn1.DataType = typeof(short);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "item_bom_id";
            this.dataColumn2.DataType = typeof(short);
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "item_id";
            this.dataColumn3.DataType = typeof(short);
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "size";
            this.dataColumn4.DataType = typeof(short);
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "bom_qty";
            this.dataColumn5.DataType = typeof(short);
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "item_code";
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "short_desc";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "uom_name";
            // 
            // dataColumn9
            // 
            this.dataColumn9.Caption = "unit_price";
            this.dataColumn9.ColumnName = "unit_price";
            // 
            // dataColumn10
            // 
            this.dataColumn10.Caption = "net_price";
            this.dataColumn10.ColumnName = "net_price";
            // 
            // pnl_footer
            // 
            this.pnl_footer.Location = new System.Drawing.Point(3, 296);
            this.pnl_footer.Name = "pnl_footer";
            this.pnl_footer.Size = new System.Drawing.Size(1279, 100);
            this.pnl_footer.TabIndex = 5;
            // 
            // btn_production_order
            // 
            this.btn_production_order.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_production_order.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_production_order.Location = new System.Drawing.Point(19, 566);
            this.btn_production_order.Name = "btn_production_order";
            this.btn_production_order.Size = new System.Drawing.Size(169, 23);
            this.btn_production_order.TabIndex = 5;
            this.btn_production_order.Text = "PRODUCTION ORDER";
            this.btn_production_order.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "GENERAL NAME:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "MODEL:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "ITEM CODE:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(526, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "PRODUCTION QTY:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(526, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "PRODUCTION TYPE:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(525, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "PRODUCTION COST:";
            // 
            // txt_item_model
            // 
            this.txt_item_model.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_item_model.Location = new System.Drawing.Point(111, 25);
            this.txt_item_model.Name = "txt_item_model";
            this.txt_item_model.ReadOnly = true;
            this.txt_item_model.Size = new System.Drawing.Size(200, 20);
            this.txt_item_model.TabIndex = 7;
            // 
            // txt_production_qty
            // 
            this.txt_production_qty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_production_qty.Location = new System.Drawing.Point(644, 12);
            this.txt_production_qty.Name = "txt_production_qty";
            this.txt_production_qty.Size = new System.Drawing.Size(200, 20);
            this.txt_production_qty.TabIndex = 9;
            this.txt_production_qty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntegerOnly_KeyPress);
            // 
            // cmb_production_type
            // 
            this.cmb_production_type.FormattingEnabled = true;
            this.cmb_production_type.Items.AddRange(new object[] {
            "PRODUCTION",
            "DISASSEMBLY"});
            this.cmb_production_type.Location = new System.Drawing.Point(644, 33);
            this.cmb_production_type.Name = "cmb_production_type";
            this.cmb_production_type.Size = new System.Drawing.Size(200, 21);
            this.cmb_production_type.TabIndex = 11;
            // 
            // pnl_header
            // 
            this.pnl_header.Controls.Add(this.txt_labor_rate);
            this.pnl_header.Controls.Add(this.txt_man_days);
            this.pnl_header.Controls.Add(this.label9);
            this.pnl_header.Controls.Add(this.txt_production_cost);
            this.pnl_header.Controls.Add(this.txt_id);
            this.pnl_header.Controls.Add(this.txt_item_id);
            this.pnl_header.Controls.Add(this.txt_general_name);
            this.pnl_header.Controls.Add(this.txt_item_code);
            this.pnl_header.Controls.Add(this.btn_get_item);
            this.pnl_header.Controls.Add(this.cmb_production_type);
            this.pnl_header.Controls.Add(this.txt_production_qty);
            this.pnl_header.Controls.Add(this.txt_item_model);
            this.pnl_header.Controls.Add(this.label5);
            this.pnl_header.Controls.Add(this.label6);
            this.pnl_header.Controls.Add(this.label7);
            this.pnl_header.Controls.Add(this.label4);
            this.pnl_header.Controls.Add(this.label3);
            this.pnl_header.Controls.Add(this.label2);
            this.pnl_header.Location = new System.Drawing.Point(0, 70);
            this.pnl_header.Name = "pnl_header";
            this.pnl_header.Size = new System.Drawing.Size(1285, 108);
            this.pnl_header.TabIndex = 2;
            // 
            // txt_labor_rate
            // 
            this.txt_labor_rate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_labor_rate.Location = new System.Drawing.Point(995, 63);
            this.txt_labor_rate.Name = "txt_labor_rate";
            this.txt_labor_rate.Size = new System.Drawing.Size(72, 20);
            this.txt_labor_rate.TabIndex = 35;
            this.txt_labor_rate.TextChanged += new System.EventHandler(this.txt_labor_rate_TextChanged);
            this.txt_labor_rate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DecimalOnly_KeyPress);
            // 
            // txt_man_days
            // 
            this.txt_man_days.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_man_days.Location = new System.Drawing.Point(922, 63);
            this.txt_man_days.Name = "txt_man_days";
            this.txt_man_days.Size = new System.Drawing.Size(73, 20);
            this.txt_man_days.TabIndex = 34;
            this.txt_man_days.TextChanged += new System.EventHandler(this.txt_man_days_TextChanged);
            this.txt_man_days.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntegerOnly_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(870, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "LABOR:";
            // 
            // txt_production_cost
            // 
            this.txt_production_cost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_production_cost.Location = new System.Drawing.Point(644, 55);
            this.txt_production_cost.Name = "txt_production_cost";
            this.txt_production_cost.ReadOnly = true;
            this.txt_production_cost.Size = new System.Drawing.Size(200, 20);
            this.txt_production_cost.TabIndex = 32;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(1167, 10);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(100, 20);
            this.txt_id.TabIndex = 31;
            this.txt_id.Visible = false;
            // 
            // txt_item_id
            // 
            this.txt_item_id.Location = new System.Drawing.Point(1067, 10);
            this.txt_item_id.Name = "txt_item_id";
            this.txt_item_id.Size = new System.Drawing.Size(100, 20);
            this.txt_item_id.TabIndex = 30;
            this.txt_item_id.Visible = false;
            // 
            // txt_general_name
            // 
            this.txt_general_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_general_name.Location = new System.Drawing.Point(111, 4);
            this.txt_general_name.Name = "txt_general_name";
            this.txt_general_name.ReadOnly = true;
            this.txt_general_name.Size = new System.Drawing.Size(200, 20);
            this.txt_general_name.TabIndex = 29;
            // 
            // txt_item_code
            // 
            this.txt_item_code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_item_code.Location = new System.Drawing.Point(111, 46);
            this.txt_item_code.Name = "txt_item_code";
            this.txt_item_code.ReadOnly = true;
            this.txt_item_code.Size = new System.Drawing.Size(200, 20);
            this.txt_item_code.TabIndex = 28;
            // 
            // btn_get_item
            // 
            this.btn_get_item.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_get_item.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.btn_get_item.Location = new System.Drawing.Point(317, 4);
            this.btn_get_item.Name = "btn_get_item";
            this.btn_get_item.Size = new System.Drawing.Size(30, 22);
            this.btn_get_item.TabIndex = 27;
            this.btn_get_item.Text = "...";
            this.btn_get_item.UseVisualStyleBackColor = true;
            this.btn_get_item.Click += new System.EventHandler(this.btn_get_item_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1282, 28);
            this.panel1.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_search,
            this.btn_edit,
            this.btn_delete,
            this.btn_save,
            this.btn_close,
            this.btn_prev,
            this.btn_next});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1282, 25);
            this.toolStrip1.TabIndex = 10;
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
            // btn_close
            // 
            this.btn_close.Image = ((System.Drawing.Image)(resources.GetObject("btn_close.Image")));
            this.btn_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(56, 22);
            this.btn_close.Text = "Close";
            this.btn_close.Visible = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_prev
            // 
            this.btn_prev.Image = ((System.Drawing.Image)(resources.GetObject("btn_prev.Image")));
            this.btn_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(72, 22);
            this.btn_prev.Text = "Previous";
            this.btn_prev.Click += new System.EventHandler(this.btn_prev_Click);
            // 
            // btn_next
            // 
            this.btn_next.Image = ((System.Drawing.Image)(resources.GetObject("btn_next.Image")));
            this.btn_next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(52, 22);
            this.btn_next.Text = "Next";
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(61, 4);
            // 
            // bom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_production_order);
            this.Controls.Add(this.pnl_components);
            this.Controls.Add(this.pnl_label);
            this.Controls.Add(this.pnl_header);
            this.Controls.Add(this.pnl_title);
            this.Name = "bom";
            this.Size = new System.Drawing.Size(1285, 615);
            this.Load += new System.EventHandler(this.bom_Load);
            this.pnl_title.ResumeLayout(false);
            this.pnl_title.PerformLayout();
            this.pnl_label.ResumeLayout(false);
            this.pnl_label.PerformLayout();
            this.pnl_components.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_bom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBindingBomComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_bom_components)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.pnl_header.ResumeLayout(false);
            this.pnl_header.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnl_label;
        private System.Windows.Forms.Panel pnl_components;
        private System.Windows.Forms.Panel pnl_footer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_production_order;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_item_model;
        private System.Windows.Forms.TextBox txt_production_qty;
        private System.Windows.Forms.ComboBox cmb_production_type;
        private System.Windows.Forms.Panel pnl_header;
        private System.Windows.Forms.Panel panel1;
        private System.Data.DataSet ds_bom_components;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Windows.Forms.BindingSource dataBindingBomComponents;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Windows.Forms.Button btn_get_item;
        private System.Windows.Forms.TextBox txt_item_code;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txt_general_name;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.TextBox txt_item_id;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_production_cost;
        private System.Data.DataColumn dataColumn9;
        private System.Windows.Forms.TextBox txt_man_days;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.TextBox txt_labor_rate;
        private System.Data.DataColumn dataColumn10;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_close;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.ToolStripButton btn_next;
        private System.Windows.Forms.DataGridView dg_bom;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn short_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_bom_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn bom_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn uom_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn net_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
    }
}
