
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_item_model_setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_item_model_setup));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_close = new System.Windows.Forms.ToolStripButton();
            this.pnl_header = new System.Windows.Forms.Panel();
            this.txt_item_tangibility_type = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_trade_status = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_item_brand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_item_class = new System.Windows.Forms.TextBox();
            this.txt_unit_of_measure = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_item_code = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_item_model = new System.Windows.Forms.TextBox();
            this.cmb_item = new System.Windows.Forms.ComboBox();
            this.txt_short_desc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.chk_is_active = new System.Windows.Forms.CheckBox();
            this.pnl_input = new System.Windows.Forms.Panel();
            this.txt_catalogue_year = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_new_model_name = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dg_item_model = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.related_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.related_brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.catalogue_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnl_header.SuspendLayout();
            this.pnl_input.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_item_model)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(934, 63);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Item Model Setup";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_search,
            this.btn_edit,
            this.btn_prev,
            this.btn_next,
            this.btn_delete,
            this.btn_save,
            this.btn_close});
            this.toolStrip1.Location = new System.Drawing.Point(0, 63);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(934, 25);
            this.toolStrip1.TabIndex = 18;
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
            this.btn_edit.Enabled = false;
            this.btn_edit.Image = ((System.Drawing.Image)(resources.GetObject("btn_edit.Image")));
            this.btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(47, 22);
            this.btn_edit.Text = "Edit";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
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
            this.btn_next.Size = new System.Drawing.Size(51, 22);
            this.btn_next.Text = "Next";
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Enabled = false;
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
            // pnl_header
            // 
            this.pnl_header.Controls.Add(this.txt_item_tangibility_type);
            this.pnl_header.Controls.Add(this.label10);
            this.pnl_header.Controls.Add(this.txt_trade_status);
            this.pnl_header.Controls.Add(this.label9);
            this.pnl_header.Controls.Add(this.txt_item_brand);
            this.pnl_header.Controls.Add(this.label3);
            this.pnl_header.Controls.Add(this.txt_item_class);
            this.pnl_header.Controls.Add(this.txt_unit_of_measure);
            this.pnl_header.Controls.Add(this.label7);
            this.pnl_header.Controls.Add(this.label8);
            this.pnl_header.Controls.Add(this.txt_item_code);
            this.pnl_header.Controls.Add(this.label2);
            this.pnl_header.Controls.Add(this.txt_item_model);
            this.pnl_header.Controls.Add(this.cmb_item);
            this.pnl_header.Controls.Add(this.txt_short_desc);
            this.pnl_header.Controls.Add(this.label6);
            this.pnl_header.Controls.Add(this.label4);
            this.pnl_header.Controls.Add(this.label5);
            this.pnl_header.Controls.Add(this.txt_id);
            this.pnl_header.Controls.Add(this.lbl_id);
            this.pnl_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_header.Enabled = false;
            this.pnl_header.Location = new System.Drawing.Point(0, 88);
            this.pnl_header.Name = "pnl_header";
            this.pnl_header.Size = new System.Drawing.Size(934, 119);
            this.pnl_header.TabIndex = 19;
            // 
            // txt_item_tangibility_type
            // 
            this.txt_item_tangibility_type.Location = new System.Drawing.Point(715, 11);
            this.txt_item_tangibility_type.Name = "txt_item_tangibility_type";
            this.txt_item_tangibility_type.Size = new System.Drawing.Size(200, 20);
            this.txt_item_tangibility_type.TabIndex = 43;
            this.txt_item_tangibility_type.Tag = "REQUIRED";
            this.txt_item_tangibility_type.WordWrap = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(652, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Tangibility:";
            // 
            // txt_trade_status
            // 
            this.txt_trade_status.Location = new System.Drawing.Point(441, 74);
            this.txt_trade_status.Name = "txt_trade_status";
            this.txt_trade_status.Size = new System.Drawing.Size(200, 20);
            this.txt_trade_status.TabIndex = 41;
            this.txt_trade_status.Tag = "REQUIRED";
            this.txt_trade_status.WordWrap = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(392, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Type:";
            // 
            // txt_item_brand
            // 
            this.txt_item_brand.Location = new System.Drawing.Point(441, 32);
            this.txt_item_brand.Name = "txt_item_brand";
            this.txt_item_brand.Size = new System.Drawing.Size(200, 20);
            this.txt_item_brand.TabIndex = 39;
            this.txt_item_brand.Tag = "REQUIRED";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(365, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Item Brand:";
            // 
            // txt_item_class
            // 
            this.txt_item_class.Location = new System.Drawing.Point(441, 11);
            this.txt_item_class.Name = "txt_item_class";
            this.txt_item_class.Size = new System.Drawing.Size(200, 20);
            this.txt_item_class.TabIndex = 37;
            this.txt_item_class.Tag = "REQUIRED";
            this.txt_item_class.WordWrap = false;
            // 
            // txt_unit_of_measure
            // 
            this.txt_unit_of_measure.Location = new System.Drawing.Point(441, 53);
            this.txt_unit_of_measure.Name = "txt_unit_of_measure";
            this.txt_unit_of_measure.Size = new System.Drawing.Size(200, 20);
            this.txt_unit_of_measure.TabIndex = 36;
            this.txt_unit_of_measure.Tag = "REQUIRED";
            this.txt_unit_of_measure.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(368, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Item UOM:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(351, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Item Category:";
            // 
            // txt_item_code
            // 
            this.txt_item_code.Location = new System.Drawing.Point(120, 54);
            this.txt_item_code.Name = "txt_item_code";
            this.txt_item_code.Size = new System.Drawing.Size(200, 20);
            this.txt_item_code.TabIndex = 33;
            this.txt_item_code.Tag = "REQUIRED";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Item Code:";
            // 
            // txt_item_model
            // 
            this.txt_item_model.Location = new System.Drawing.Point(120, 33);
            this.txt_item_model.Name = "txt_item_model";
            this.txt_item_model.Size = new System.Drawing.Size(200, 20);
            this.txt_item_model.TabIndex = 26;
            this.txt_item_model.Tag = "REQUIRED";
            this.txt_item_model.WordWrap = false;
            // 
            // cmb_item
            // 
            this.cmb_item.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_item.FormattingEnabled = true;
            this.cmb_item.Location = new System.Drawing.Point(120, 11);
            this.cmb_item.Name = "cmb_item";
            this.cmb_item.Size = new System.Drawing.Size(200, 21);
            this.cmb_item.TabIndex = 25;
            this.cmb_item.Tag = "DYNAMIC";
            // 
            // txt_short_desc
            // 
            this.txt_short_desc.Location = new System.Drawing.Point(120, 75);
            this.txt_short_desc.Name = "txt_short_desc";
            this.txt_short_desc.Size = new System.Drawing.Size(200, 20);
            this.txt_short_desc.TabIndex = 18;
            this.txt_short_desc.Tag = "REQUIRED";
            this.txt_short_desc.WordWrap = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Short Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Model:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Item Name:";
            // 
            // txt_id
            // 
            this.txt_id.Enabled = false;
            this.txt_id.Location = new System.Drawing.Point(715, 32);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(40, 20);
            this.txt_id.TabIndex = 12;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(681, 35);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(19, 13);
            this.lbl_id.TabIndex = 11;
            this.lbl_id.Text = "Id:";
            this.lbl_id.Visible = false;
            // 
            // chk_is_active
            // 
            this.chk_is_active.AutoSize = true;
            this.chk_is_active.Location = new System.Drawing.Point(441, 17);
            this.chk_is_active.Name = "chk_is_active";
            this.chk_is_active.Size = new System.Drawing.Size(56, 17);
            this.chk_is_active.TabIndex = 19;
            this.chk_is_active.Text = "Active";
            this.chk_is_active.UseVisualStyleBackColor = true;
            // 
            // pnl_input
            // 
            this.pnl_input.Controls.Add(this.txt_catalogue_year);
            this.pnl_input.Controls.Add(this.label11);
            this.pnl_input.Controls.Add(this.txt_new_model_name);
            this.pnl_input.Controls.Add(this.label12);
            this.pnl_input.Controls.Add(this.chk_is_active);
            this.pnl_input.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_input.Location = new System.Drawing.Point(0, 207);
            this.pnl_input.Name = "pnl_input";
            this.pnl_input.Size = new System.Drawing.Size(934, 73);
            this.pnl_input.TabIndex = 20;
            // 
            // txt_catalogue_year
            // 
            this.txt_catalogue_year.Location = new System.Drawing.Point(120, 36);
            this.txt_catalogue_year.Name = "txt_catalogue_year";
            this.txt_catalogue_year.Size = new System.Drawing.Size(200, 20);
            this.txt_catalogue_year.TabIndex = 37;
            this.txt_catalogue_year.Tag = "REQUIRED";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 39);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "Catalogue Year:";
            // 
            // txt_new_model_name
            // 
            this.txt_new_model_name.Location = new System.Drawing.Point(120, 15);
            this.txt_new_model_name.Name = "txt_new_model_name";
            this.txt_new_model_name.Size = new System.Drawing.Size(200, 20);
            this.txt_new_model_name.TabIndex = 35;
            this.txt_new_model_name.Tag = "REQUIRED";
            this.txt_new_model_name.WordWrap = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "New Model:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dg_item_model);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 280);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(934, 367);
            this.panel3.TabIndex = 21;
            // 
            // dg_item_model
            // 
            this.dg_item_model.AllowUserToAddRows = false;
            this.dg_item_model.AllowUserToDeleteRows = false;
            this.dg_item_model.AllowUserToResizeColumns = false;
            this.dg_item_model.AllowUserToResizeRows = false;
            this.dg_item_model.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_item_model.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.related_name,
            this.related_brand,
            this.catalogue_year,
            this.is_active});
            this.dg_item_model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_item_model.Location = new System.Drawing.Point(0, 0);
            this.dg_item_model.MultiSelect = false;
            this.dg_item_model.Name = "dg_item_model";
            this.dg_item_model.ReadOnly = true;
            this.dg_item_model.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_item_model.Size = new System.Drawing.Size(934, 367);
            this.dg_item_model.TabIndex = 21;
            this.dg_item_model.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_item_model_CellClick);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "NAME";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // related_name
            // 
            this.related_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.related_name.DataPropertyName = "related_name";
            this.related_name.HeaderText = "ITEM NAME";
            this.related_name.Name = "related_name";
            this.related_name.ReadOnly = true;
            // 
            // related_brand
            // 
            this.related_brand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.related_brand.DataPropertyName = "related_brand";
            this.related_brand.HeaderText = "BRAND NAME";
            this.related_brand.Name = "related_brand";
            this.related_brand.ReadOnly = true;
            // 
            // catalogue_year
            // 
            this.catalogue_year.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.catalogue_year.DataPropertyName = "catalogue_year";
            this.catalogue_year.HeaderText = "CATALOGUE YEAR";
            this.catalogue_year.Name = "catalogue_year";
            this.catalogue_year.ReadOnly = true;
            // 
            // is_active
            // 
            this.is_active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.is_active.DataPropertyName = "is_active";
            this.is_active.HeaderText = "ACTIVE";
            this.is_active.Name = "is_active";
            this.is_active.ReadOnly = true;
            // 
            // frm_item_model_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnl_input);
            this.Controls.Add(this.pnl_header);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "frm_item_model_setup";
            this.Size = new System.Drawing.Size(934, 647);
            this.Load += new System.EventHandler(this.frm_item_model_setup_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnl_header.ResumeLayout(false);
            this.pnl_header.PerformLayout();
            this.pnl_input.ResumeLayout(false);
            this.pnl_input.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_item_model)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_close;
        private System.Windows.Forms.Panel pnl_header;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.TextBox txt_short_desc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chk_is_active;
        private System.Windows.Forms.ComboBox cmb_item;
        private System.Windows.Forms.TextBox txt_item_model;
        private System.Windows.Forms.TextBox txt_item_brand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_item_class;
        private System.Windows.Forms.TextBox txt_unit_of_measure;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_item_code;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_trade_status;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_item_tangibility_type;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pnl_input;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dg_item_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn related_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn related_brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn catalogue_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_active;
        private System.Windows.Forms.TextBox txt_catalogue_year;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_new_model_name;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.ToolStripButton btn_next;
    }
}
