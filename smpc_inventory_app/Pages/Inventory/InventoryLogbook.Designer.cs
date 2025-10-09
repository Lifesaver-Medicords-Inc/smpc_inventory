
namespace smpc_inventory_app.Pages.Inventory
{
    partial class InventoryLogbook
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnl_title = new System.Windows.Forms.Panel();
            this.cmb_month = new System.Windows.Forms.ComboBox();
            this.cmb_year = new System.Windows.Forms.ComboBox();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_make_report = new System.Windows.Forms.Button();
            this.lbl_warehouse = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnl_fill = new System.Windows.Forms.Panel();
            this.dgv_inventory_item = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pod_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.beg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.in_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.out_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_title.SuspendLayout();
            this.pnl_fill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_inventory_item)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_title
            // 
            this.pnl_title.Controls.Add(this.cmb_month);
            this.pnl_title.Controls.Add(this.cmb_year);
            this.pnl_title.Controls.Add(this.txt_search);
            this.pnl_title.Controls.Add(this.label3);
            this.pnl_title.Controls.Add(this.label5);
            this.pnl_title.Controls.Add(this.label2);
            this.pnl_title.Controls.Add(this.btn_make_report);
            this.pnl_title.Controls.Add(this.lbl_warehouse);
            this.pnl_title.Controls.Add(this.label1);
            this.pnl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_title.Location = new System.Drawing.Point(0, 0);
            this.pnl_title.Name = "pnl_title";
            this.pnl_title.Size = new System.Drawing.Size(1285, 77);
            this.pnl_title.TabIndex = 3;
            // 
            // cmb_month
            // 
            this.cmb_month.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_month.FormattingEnabled = true;
            this.cmb_month.Location = new System.Drawing.Point(65, 50);
            this.cmb_month.Name = "cmb_month";
            this.cmb_month.Size = new System.Drawing.Size(121, 21);
            this.cmb_month.TabIndex = 16;
            this.cmb_month.SelectedIndexChanged += new System.EventHandler(this.cmb_month_SelectedIndexChanged);
            // 
            // cmb_year
            // 
            this.cmb_year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_year.FormattingEnabled = true;
            this.cmb_year.Location = new System.Drawing.Point(270, 50);
            this.cmb_year.Name = "cmb_year";
            this.cmb_year.Size = new System.Drawing.Size(121, 21);
            this.cmb_year.TabIndex = 15;
            this.cmb_year.SelectedIndexChanged += new System.EventHandler(this.cmb_year_SelectedIndexChanged);
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(497, 51);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(284, 20);
            this.txt_search.TabIndex = 14;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(433, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "FILTER:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "MONTH";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "YEAR";
            // 
            // btn_make_report
            // 
            this.btn_make_report.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_make_report.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.btn_make_report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_make_report.Location = new System.Drawing.Point(1099, 11);
            this.btn_make_report.Name = "btn_make_report";
            this.btn_make_report.Size = new System.Drawing.Size(169, 23);
            this.btn_make_report.TabIndex = 8;
            this.btn_make_report.Text = "MAKE REPORT";
            this.btn_make_report.UseVisualStyleBackColor = false;
            this.btn_make_report.Click += new System.EventHandler(this.btn_make_report_Click);
            // 
            // lbl_warehouse
            // 
            this.lbl_warehouse.AutoSize = true;
            this.lbl_warehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_warehouse.Location = new System.Drawing.Point(362, 12);
            this.lbl_warehouse.Name = "lbl_warehouse";
            this.lbl_warehouse.Size = new System.Drawing.Size(0, 24);
            this.lbl_warehouse.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inventory Logbook";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 561);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1285, 54);
            this.panel1.TabIndex = 4;
            // 
            // pnl_fill
            // 
            this.pnl_fill.Controls.Add(this.dgv_inventory_item);
            this.pnl_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_fill.Location = new System.Drawing.Point(0, 77);
            this.pnl_fill.Name = "pnl_fill";
            this.pnl_fill.Size = new System.Drawing.Size(1285, 484);
            this.pnl_fill.TabIndex = 5;
            // 
            // dgv_inventory_item
            // 
            this.dgv_inventory_item.AllowUserToAddRows = false;
            this.dgv_inventory_item.AllowUserToDeleteRows = false;
            this.dgv_inventory_item.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_inventory_item.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_inventory_item.ColumnHeadersHeight = 50;
            this.dgv_inventory_item.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.pod_id,
            this.general_name,
            this.brand,
            this.item_desc,
            this.beg,
            this.end,
            this.in_total,
            this.out_total,
            this.location});
            this.dgv_inventory_item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_inventory_item.EnableHeadersVisualStyles = false;
            this.dgv_inventory_item.Location = new System.Drawing.Point(0, 0);
            this.dgv_inventory_item.Name = "dgv_inventory_item";
            this.dgv_inventory_item.Size = new System.Drawing.Size(1285, 484);
            this.dgv_inventory_item.TabIndex = 5;
            this.dgv_inventory_item.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_inventory_item_CellClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // pod_id
            // 
            this.pod_id.DataPropertyName = "pod_id";
            this.pod_id.HeaderText = "POD ID";
            this.pod_id.Name = "pod_id";
            this.pod_id.ReadOnly = true;
            this.pod_id.Visible = false;
            // 
            // general_name
            // 
            this.general_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.general_name.DataPropertyName = "general_name";
            this.general_name.HeaderText = "GENERAL NAME";
            this.general_name.Name = "general_name";
            this.general_name.ReadOnly = true;
            this.general_name.Width = 200;
            // 
            // brand
            // 
            this.brand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.brand.DataPropertyName = "brand";
            this.brand.HeaderText = "BRAND";
            this.brand.MinimumWidth = 150;
            this.brand.Name = "brand";
            this.brand.ReadOnly = true;
            this.brand.Width = 150;
            // 
            // item_desc
            // 
            this.item_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.item_desc.DataPropertyName = "item_desc";
            this.item_desc.HeaderText = "ITEM DESCRIPTION";
            this.item_desc.MinimumWidth = 200;
            this.item_desc.Name = "item_desc";
            this.item_desc.ReadOnly = true;
            this.item_desc.Width = 200;
            // 
            // beg
            // 
            this.beg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.beg.DataPropertyName = "beg";
            this.beg.HeaderText = "BEG";
            this.beg.Name = "beg";
            this.beg.ReadOnly = true;
            this.beg.Width = 80;
            // 
            // end
            // 
            this.end.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.end.DataPropertyName = "end";
            this.end.HeaderText = "END";
            this.end.Name = "end";
            this.end.ReadOnly = true;
            this.end.Width = 80;
            // 
            // in_total
            // 
            this.in_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.in_total.DataPropertyName = "in_total";
            this.in_total.HeaderText = "IN";
            this.in_total.Name = "in_total";
            this.in_total.ReadOnly = true;
            this.in_total.Width = 80;
            // 
            // out_total
            // 
            this.out_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.out_total.DataPropertyName = "out_total";
            this.out_total.HeaderText = "OUT";
            this.out_total.Name = "out_total";
            this.out_total.ReadOnly = true;
            this.out_total.Width = 80;
            // 
            // location
            // 
            this.location.DataPropertyName = "location";
            this.location.HeaderText = "LOCATION";
            this.location.Name = "location";
            this.location.ReadOnly = true;
            this.location.Visible = false;
            // 
            // InventoryLogbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_fill);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnl_title);
            this.Name = "InventoryLogbook";
            this.Size = new System.Drawing.Size(1285, 615);
            this.Load += new System.EventHandler(this.InventoryLogbook_Load);
            this.pnl_title.ResumeLayout(false);
            this.pnl_title.PerformLayout();
            this.pnl_fill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_inventory_item)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_title;
        private System.Windows.Forms.Label lbl_warehouse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnl_fill;
        private System.Windows.Forms.DataGridView dgv_inventory_item;
        private System.Windows.Forms.Button btn_make_report;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn pod_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn beg;
        private System.Windows.Forms.DataGridViewTextBoxColumn end;
        private System.Windows.Forms.DataGridViewTextBoxColumn in_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn out_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn location;
        private System.Windows.Forms.ComboBox cmb_month;
        private System.Windows.Forms.ComboBox cmb_year;
    }
}
