
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_warehouse_usetype_setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_warehouse_usetype_setup));
            this.lbl_industries = new System.Windows.Forms.Label();
            this.panel_header = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.pnl_records = new System.Windows.Forms.Panel();
            this.lblColor = new System.Windows.Forms.Label();
            this.cmb_bg_color = new System.Windows.Forms.ComboBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dg_warehouse_usetype = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bg_color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_header.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnl_records.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_warehouse_usetype)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_industries
            // 
            this.lbl_industries.AutoSize = true;
            this.lbl_industries.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_industries.Location = new System.Drawing.Point(5, 16);
            this.lbl_industries.Name = "lbl_industries";
            this.lbl_industries.Size = new System.Drawing.Size(181, 24);
            this.lbl_industries.TabIndex = 3;
            this.lbl_industries.Text = "Warehouse Usetype";
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_industries);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(634, 63);
            this.panel_header.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_save,
            this.btn_cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 63);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 24;
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
            this.btn_save.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
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
            // pnl_records
            // 
            this.pnl_records.Controls.Add(this.lblColor);
            this.pnl_records.Controls.Add(this.cmb_bg_color);
            this.pnl_records.Controls.Add(this.lbl_id);
            this.pnl_records.Controls.Add(this.txt_id);
            this.pnl_records.Controls.Add(this.txt_name);
            this.pnl_records.Controls.Add(this.txt_code);
            this.pnl_records.Controls.Add(this.label4);
            this.pnl_records.Controls.Add(this.label5);
            this.pnl_records.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_records.Location = new System.Drawing.Point(0, 88);
            this.pnl_records.Name = "pnl_records";
            this.pnl_records.Size = new System.Drawing.Size(634, 122);
            this.pnl_records.TabIndex = 25;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(26, 71);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(44, 13);
            this.lblColor.TabIndex = 20;
            this.lblColor.Text = "COLOR";
            // 
            // cmb_bg_color
            // 
            this.cmb_bg_color.Enabled = false;
            this.cmb_bg_color.FormattingEnabled = true;
            this.cmb_bg_color.Location = new System.Drawing.Point(82, 68);
            this.cmb_bg_color.Name = "cmb_bg_color";
            this.cmb_bg_color.Size = new System.Drawing.Size(200, 21);
            this.cmb_bg_color.TabIndex = 19;
            this.cmb_bg_color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmb_bg_color_DrawItem);
            this.cmb_bg_color.SelectedIndexChanged += new System.EventHandler(this.cmb_bg_color_SelectedIndexChanged);
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(331, 30);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(16, 13);
            this.lbl_id.TabIndex = 18;
            this.lbl_id.Text = "Id";
            this.lbl_id.Visible = false;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(353, 27);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 17;
            this.txt_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(82, 45);
            this.txt_name.Name = "txt_name";
            this.txt_name.ReadOnly = true;
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 16;
            this.txt_name.Tag = "";
            this.txt_name.MouseLeave += new System.EventHandler(this.txt_name_MouseLeave);
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(82, 23);
            this.txt_code.Name = "txt_code";
            this.txt_code.ReadOnly = true;
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 15;
            this.txt_code.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "NAME";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "CODE";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "tbl_usetype";
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
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn4,
            this.dataColumn3});
            this.dataTable1.TableName = "tbl_usetype";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "id";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "code";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "name";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "bg_color";
            // 
            // dg_warehouse_usetype
            // 
            this.dg_warehouse_usetype.AllowUserToAddRows = false;
            this.dg_warehouse_usetype.AllowUserToDeleteRows = false;
            this.dg_warehouse_usetype.AllowUserToResizeColumns = false;
            this.dg_warehouse_usetype.AllowUserToResizeRows = false;
            this.dg_warehouse_usetype.AutoGenerateColumns = false;
            this.dg_warehouse_usetype.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_warehouse_usetype.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_warehouse_usetype.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.bg_color});
            this.dg_warehouse_usetype.DataSource = this.bindingSource1;
            this.dg_warehouse_usetype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_warehouse_usetype.Location = new System.Drawing.Point(0, 210);
            this.dg_warehouse_usetype.MultiSelect = false;
            this.dg_warehouse_usetype.Name = "dg_warehouse_usetype";
            this.dg_warehouse_usetype.ReadOnly = true;
            this.dg_warehouse_usetype.RowHeadersVisible = false;
            this.dg_warehouse_usetype.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_warehouse_usetype.Size = new System.Drawing.Size(634, 437);
            this.dg_warehouse_usetype.TabIndex = 26;
            this.dg_warehouse_usetype.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_warehouse_usetype_CellClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 2;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "CODE";
            this.code.MinimumWidth = 3;
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "NAME";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // bg_color
            // 
            this.bg_color.DataPropertyName = "bg_color";
            this.bg_color.HeaderText = "bg_color";
            this.bg_color.Name = "bg_color";
            this.bg_color.ReadOnly = true;
            this.bg_color.Visible = false;
            // 
            // frm_warehouse_usetype_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dg_warehouse_usetype);
            this.Controls.Add(this.pnl_records);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_warehouse_usetype_setup";
            this.Size = new System.Drawing.Size(634, 647);
            this.Load += new System.EventHandler(this.frm_warehouse_usetype_setup_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnl_records.ResumeLayout(false);
            this.pnl_records.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_warehouse_usetype)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_industries;
        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.Panel pnl_records;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Data.DataSet dataSet1;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn4;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dg_warehouse_usetype;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cmb_bg_color;
        private System.Data.DataColumn dataColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn bg_color;
    }
}
