
namespace smpc_inventory_app.Pages
{
    partial class InventoryTracker
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
            this.btn_next = new System.Windows.Forms.Button();
            this.lbl_warehouse = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnl_fill = new System.Windows.Forms.Panel();
            this.dgv_inventory_item = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pod_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.units_reserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.units_outbound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rem_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_title.SuspendLayout();
            this.pnl_fill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_inventory_item)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_title
            // 
            this.pnl_title.Controls.Add(this.btn_next);
            this.pnl_title.Controls.Add(this.lbl_warehouse);
            this.pnl_title.Controls.Add(this.label1);
            this.pnl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_title.Location = new System.Drawing.Point(0, 0);
            this.pnl_title.Name = "pnl_title";
            this.pnl_title.Size = new System.Drawing.Size(1285, 77);
            this.pnl_title.TabIndex = 2;
            // 
            // btn_next
            // 
            this.btn_next.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_next.ForeColor = System.Drawing.Color.Black;
            this.btn_next.Location = new System.Drawing.Point(792, 24);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 23);
            this.btn_next.TabIndex = 7;
            this.btn_next.Text = "<";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // lbl_warehouse
            // 
            this.lbl_warehouse.AutoSize = true;
            this.lbl_warehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_warehouse.Location = new System.Drawing.Point(319, 21);
            this.lbl_warehouse.Name = "lbl_warehouse";
            this.lbl_warehouse.Size = new System.Drawing.Size(15, 24);
            this.lbl_warehouse.TabIndex = 1;
            this.lbl_warehouse.Text = " ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inventory Tracker";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 561);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1285, 54);
            this.panel1.TabIndex = 3;
            // 
            // pnl_fill
            // 
            this.pnl_fill.Controls.Add(this.dgv_inventory_item);
            this.pnl_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_fill.Location = new System.Drawing.Point(0, 77);
            this.pnl_fill.Name = "pnl_fill";
            this.pnl_fill.Size = new System.Drawing.Size(1285, 484);
            this.pnl_fill.TabIndex = 4;
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
            this.item_code,
            this.general_name,
            this.brand,
            this.item_desc,
            this.units_reserved,
            this.details,
            this.uom,
            this.zone,
            this.units_outbound,
            this.remarks,
            this.rem_id,
            this.warehouse_id,
            this.warehouse_name});
            this.dgv_inventory_item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_inventory_item.EnableHeadersVisualStyles = false;
            this.dgv_inventory_item.Location = new System.Drawing.Point(0, 0);
            this.dgv_inventory_item.Name = "dgv_inventory_item";
            this.dgv_inventory_item.Size = new System.Drawing.Size(1285, 484);
            this.dgv_inventory_item.TabIndex = 5;
            this.dgv_inventory_item.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgv_inventory_item_CellBeginEdit);
            this.dgv_inventory_item.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_inventory_item_CellClick);
            this.dgv_inventory_item.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_inventory_item_CellEndEdit);
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
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
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
            this.brand.Name = "brand";
            this.brand.ReadOnly = true;
            this.brand.Width = 200;
            // 
            // item_desc
            // 
            this.item_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.item_desc.DataPropertyName = "item_desc";
            this.item_desc.HeaderText = "ITEM DESCRIPTION";
            this.item_desc.Name = "item_desc";
            this.item_desc.ReadOnly = true;
            this.item_desc.Width = 200;
            // 
            // units_reserved
            // 
            this.units_reserved.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.units_reserved.DataPropertyName = "units_reserved";
            this.units_reserved.HeaderText = "UNITS";
            this.units_reserved.Name = "units_reserved";
            this.units_reserved.ReadOnly = true;
            this.units_reserved.Width = 80;
            // 
            // details
            // 
            this.details.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.details.DataPropertyName = "details";
            this.details.HeaderText = "DETAILS";
            this.details.Name = "details";
            this.details.ReadOnly = true;
            // 
            // uom
            // 
            this.uom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.uom.DataPropertyName = "uom";
            this.uom.HeaderText = "UOM";
            this.uom.Name = "uom";
            this.uom.ReadOnly = true;
            this.uom.Width = 80;
            // 
            // zone
            // 
            this.zone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.zone.DataPropertyName = "zone";
            this.zone.HeaderText = "ZONE";
            this.zone.Name = "zone";
            this.zone.ReadOnly = true;
            // 
            // units_outbound
            // 
            this.units_outbound.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.units_outbound.DataPropertyName = "units_outbound";
            this.units_outbound.HeaderText = "UNITS";
            this.units_outbound.Name = "units_outbound";
            this.units_outbound.ReadOnly = true;
            this.units_outbound.Width = 80;
            // 
            // remarks
            // 
            this.remarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remarks.DataPropertyName = "remarks";
            this.remarks.HeaderText = "REMARKS";
            this.remarks.Name = "remarks";
            this.remarks.Width = 200;
            // 
            // rem_id
            // 
            this.rem_id.DataPropertyName = "rem_id";
            this.rem_id.HeaderText = "REM ID";
            this.rem_id.Name = "rem_id";
            this.rem_id.ReadOnly = true;
            this.rem_id.Visible = false;
            // 
            // warehouse_id
            // 
            this.warehouse_id.DataPropertyName = "warehouse_id";
            this.warehouse_id.HeaderText = "WAREHOUSE ID";
            this.warehouse_id.Name = "warehouse_id";
            this.warehouse_id.ReadOnly = true;
            this.warehouse_id.Visible = false;
            // 
            // warehouse_name
            // 
            this.warehouse_name.DataPropertyName = "warehouse_name";
            this.warehouse_name.HeaderText = "WAREHOUSE NAME";
            this.warehouse_name.Name = "warehouse_name";
            this.warehouse_name.ReadOnly = true;
            this.warehouse_name.Visible = false;
            // 
            // InventoryTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_fill);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnl_title);
            this.Name = "InventoryTracker";
            this.Size = new System.Drawing.Size(1285, 615);
            this.Load += new System.EventHandler(this.InventoryTracker_Load);
            this.pnl_title.ResumeLayout(false);
            this.pnl_title.PerformLayout();
            this.pnl_fill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_inventory_item)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_warehouse;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnl_fill;
        private System.Windows.Forms.DataGridView dgv_inventory_item;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn pod_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn units_reserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn details;
        private System.Windows.Forms.DataGridViewTextBoxColumn uom;
        private System.Windows.Forms.DataGridViewTextBoxColumn zone;
        private System.Windows.Forms.DataGridViewTextBoxColumn units_outbound;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn rem_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_name;
    }
}
