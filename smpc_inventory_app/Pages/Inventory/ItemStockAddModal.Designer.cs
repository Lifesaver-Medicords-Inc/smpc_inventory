namespace smpc_inventory_app.Pages.Inventory
{
    partial class ItemStockAddModal
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lbl_item = new System.Windows.Forms.Label();
            this.cmb_item = new System.Windows.Forms.ComboBox();
            this.lnk_new_item = new System.Windows.Forms.LinkLabel();
            this.lbl_warehouse = new System.Windows.Forms.Label();
            this.cmb_warehouse = new System.Windows.Forms.ComboBox();
            this.lbl_bin = new System.Windows.Forms.Label();
            this.cmb_bin_location = new System.Windows.Forms.ComboBox();
            this.lbl_qty = new System.Windows.Forms.Label();
            this.num_qty = new System.Windows.Forms.NumericUpDown();
            this.lbl_uom = new System.Windows.Forms.Label();
            this.cmb_uom = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_qty)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_item
            // 
            this.lbl_item.AutoSize = true;
            this.lbl_item.Location = new System.Drawing.Point(12, 15);
            this.lbl_item.Name = "lbl_item";
            this.lbl_item.Size = new System.Drawing.Size(30, 13);
            this.lbl_item.TabIndex = 0;
            this.lbl_item.Text = "Item:";
            // 
            // cmb_item
            // 
            this.cmb_item.FormattingEnabled = true;
            this.cmb_item.Location = new System.Drawing.Point(120, 12);
            this.cmb_item.Name = "cmb_item";
            this.cmb_item.Size = new System.Drawing.Size(240, 21);
            this.cmb_item.TabIndex = 1;
            this.cmb_item.SelectedIndexChanged += new System.EventHandler(this.cmb_item_SelectedIndexChanged);
            this.cmb_item.TextChanged += new System.EventHandler(this.cmb_item_TextChanged);
            // 
            // lnk_new_item
            // 
            this.lnk_new_item.AutoSize = true;
            this.lnk_new_item.Location = new System.Drawing.Point(373, 15);
            this.lnk_new_item.Name = "lnk_new_item";
            this.lnk_new_item.Size = new System.Drawing.Size(61, 13);
            this.lnk_new_item.TabIndex = 12;
            this.lnk_new_item.TabStop = true;
            this.lnk_new_item.Text = "+ New Item";
            this.lnk_new_item.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_new_item_LinkClicked);
            // 
            // lbl_warehouse
            // 
            this.lbl_warehouse.AutoSize = true;
            this.lbl_warehouse.Location = new System.Drawing.Point(12, 48);
            this.lbl_warehouse.Name = "lbl_warehouse";
            this.lbl_warehouse.Size = new System.Drawing.Size(65, 13);
            this.lbl_warehouse.TabIndex = 2;
            this.lbl_warehouse.Text = "Warehouse:";
            // 
            // cmb_warehouse
            // 
            this.cmb_warehouse.DisplayMember = "name";
            this.cmb_warehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_warehouse.FormattingEnabled = true;
            this.cmb_warehouse.Location = new System.Drawing.Point(120, 45);
            this.cmb_warehouse.Name = "cmb_warehouse";
            this.cmb_warehouse.Size = new System.Drawing.Size(280, 21);
            this.cmb_warehouse.TabIndex = 3;
            this.cmb_warehouse.ValueMember = "id";
            this.cmb_warehouse.SelectedIndexChanged += new System.EventHandler(this.cmb_warehouse_SelectedIndexChanged);
            //
            // lbl_bin
            //
            this.lbl_bin.AutoSize = true;
            this.lbl_bin.Location = new System.Drawing.Point(12, 81);
            this.lbl_bin.Name = "lbl_bin";
            this.lbl_bin.Size = new System.Drawing.Size(69, 13);
            this.lbl_bin.TabIndex = 4;
            this.lbl_bin.Text = "Bin Location:";
            //
            // cmb_bin_location
            //
            // Populated per-warehouse from tbl_inv_warehouse_area (Warehouse Setup module)
            // via cmb_warehouse_SelectedIndexChanged - each option is the assembled
            // "zone-area-rack-level-bins" string, same format ReceivingReport2 already
            // writes into bin_location elsewhere, so values stay consistent across screens.
            // Left as editable DropDown (not DropDownList) rather than locked to the master
            // list, since not every warehouse necessarily has areas defined yet there.
            this.cmb_bin_location.FormattingEnabled = true;
            this.cmb_bin_location.Location = new System.Drawing.Point(120, 78);
            this.cmb_bin_location.Name = "cmb_bin_location";
            this.cmb_bin_location.Size = new System.Drawing.Size(280, 21);
            this.cmb_bin_location.TabIndex = 5;
            //
            // lbl_qty
            // 
            this.lbl_qty.AutoSize = true;
            this.lbl_qty.Location = new System.Drawing.Point(12, 114);
            this.lbl_qty.Name = "lbl_qty";
            this.lbl_qty.Size = new System.Drawing.Size(60, 13);
            this.lbl_qty.TabIndex = 6;
            this.lbl_qty.Text = "Qty to Add:";
            // 
            // num_qty
            // 
            this.num_qty.Location = new System.Drawing.Point(120, 112);
            this.num_qty.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.num_qty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_qty.Name = "num_qty";
            this.num_qty.Size = new System.Drawing.Size(100, 20);
            this.num_qty.TabIndex = 7;
            this.num_qty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbl_uom
            // 
            this.lbl_uom.AutoSize = true;
            this.lbl_uom.Location = new System.Drawing.Point(12, 147);
            this.lbl_uom.Name = "lbl_uom";
            this.lbl_uom.Size = new System.Drawing.Size(35, 13);
            this.lbl_uom.TabIndex = 8;
            this.lbl_uom.Text = "UOM:";
            //
            // cmb_uom
            //
            // Master-list dropdown, same convention as cmb_warehouse: bound to the
            // /setup/unit_measurement table (id/name) via UnitOfMeasurementServices,
            // ValueMember "id" / DisplayMember "name". Auto-selected to match the picked
            // item's unit_of_measure_id in cmb_item_SelectedIndexChanged, but still
            // user-changeable since some stock may legitimately be counted in another UOM.
            this.cmb_uom.DisplayMember = "name";
            this.cmb_uom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_uom.FormattingEnabled = true;
            this.cmb_uom.Location = new System.Drawing.Point(120, 144);
            this.cmb_uom.Name = "cmb_uom";
            this.cmb_uom.Size = new System.Drawing.Size(150, 21);
            this.cmb_uom.TabIndex = 9;
            this.cmb_uom.ValueMember = "id";
            //
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(287, 200);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 26);
            this.btn_ok.TabIndex = 10;
            this.btn_ok.Text = "Add";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(368, 200);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 26);
            this.btn_cancel.TabIndex = 11;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // ItemStockAddModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(446, 238);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cmb_uom);
            this.Controls.Add(this.lbl_uom);
            this.Controls.Add(this.num_qty);
            this.Controls.Add(this.lbl_qty);
            this.Controls.Add(this.cmb_bin_location);
            this.Controls.Add(this.lbl_bin);
            this.Controls.Add(this.cmb_warehouse);
            this.Controls.Add(this.lbl_warehouse);
            this.Controls.Add(this.lnk_new_item);
            this.Controls.Add(this.cmb_item);
            this.Controls.Add(this.lbl_item);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemStockAddModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Stock";
            this.Load += new System.EventHandler(this.ItemStockAddModal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_qty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_item;
        private System.Windows.Forms.ComboBox cmb_item;
        private System.Windows.Forms.LinkLabel lnk_new_item;
        private System.Windows.Forms.Label lbl_warehouse;
        private System.Windows.Forms.ComboBox cmb_warehouse;
        private System.Windows.Forms.Label lbl_bin;
        private System.Windows.Forms.ComboBox cmb_bin_location;
        private System.Windows.Forms.Label lbl_qty;
        private System.Windows.Forms.NumericUpDown num_qty;
        private System.Windows.Forms.Label lbl_uom;
        private System.Windows.Forms.ComboBox cmb_uom;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
    }
}
