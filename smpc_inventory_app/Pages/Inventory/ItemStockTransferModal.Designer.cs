namespace smpc_inventory_app.Pages.Inventory
{
    partial class ItemStockTransferModal
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
            this.lbl_info = new System.Windows.Forms.Label();
            this.lbl_transfer_qty = new System.Windows.Forms.Label();
            this.num_transfer_qty = new System.Windows.Forms.NumericUpDown();
            this.lbl_dest_warehouse = new System.Windows.Forms.Label();
            this.cmb_dest_warehouse = new System.Windows.Forms.ComboBox();
            this.lbl_dest_bin = new System.Windows.Forms.Label();
            this.cmb_dest_bin_location = new System.Windows.Forms.ComboBox();
            this.lbl_remarks = new System.Windows.Forms.Label();
            this.txt_remarks = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_transfer_qty)).BeginInit();
            this.SuspendLayout();
            //
            // lbl_info
            //
            this.lbl_info.Location = new System.Drawing.Point(12, 12);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(340, 60);
            this.lbl_info.TabIndex = 0;
            this.lbl_info.Text = "lbl_info";
            //
            // lbl_transfer_qty
            //
            this.lbl_transfer_qty.AutoSize = true;
            this.lbl_transfer_qty.Location = new System.Drawing.Point(12, 82);
            this.lbl_transfer_qty.Name = "lbl_transfer_qty";
            this.lbl_transfer_qty.Size = new System.Drawing.Size(89, 13);
            this.lbl_transfer_qty.TabIndex = 1;
            this.lbl_transfer_qty.Text = "Qty to Transfer:";
            //
            // num_transfer_qty
            //
            this.num_transfer_qty.Location = new System.Drawing.Point(150, 80);
            this.num_transfer_qty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.num_transfer_qty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.num_transfer_qty.Name = "num_transfer_qty";
            this.num_transfer_qty.Size = new System.Drawing.Size(100, 20);
            this.num_transfer_qty.TabIndex = 2;
            this.num_transfer_qty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lbl_dest_warehouse
            //
            this.lbl_dest_warehouse.AutoSize = true;
            this.lbl_dest_warehouse.Location = new System.Drawing.Point(12, 115);
            this.lbl_dest_warehouse.Name = "lbl_dest_warehouse";
            this.lbl_dest_warehouse.Size = new System.Drawing.Size(115, 13);
            this.lbl_dest_warehouse.TabIndex = 3;
            this.lbl_dest_warehouse.Text = "Destination Warehouse:";
            //
            // cmb_dest_warehouse
            //
            this.cmb_dest_warehouse.DisplayMember = "name";
            this.cmb_dest_warehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_dest_warehouse.FormattingEnabled = true;
            this.cmb_dest_warehouse.Location = new System.Drawing.Point(150, 112);
            this.cmb_dest_warehouse.Name = "cmb_dest_warehouse";
            this.cmb_dest_warehouse.Size = new System.Drawing.Size(202, 21);
            this.cmb_dest_warehouse.TabIndex = 4;
            this.cmb_dest_warehouse.ValueMember = "id";
            this.cmb_dest_warehouse.SelectedIndexChanged += new System.EventHandler(this.cmb_dest_warehouse_SelectedIndexChanged);
            //
            // lbl_dest_bin
            //
            this.lbl_dest_bin.AutoSize = true;
            this.lbl_dest_bin.Location = new System.Drawing.Point(12, 148);
            this.lbl_dest_bin.Name = "lbl_dest_bin";
            this.lbl_dest_bin.Size = new System.Drawing.Size(89, 13);
            this.lbl_dest_bin.TabIndex = 5;
            this.lbl_dest_bin.Text = "Destination Bin:";
            //
            // cmb_dest_bin_location
            //
            // Same "zone-area-rack-level-bins" convention as ItemStockAddModal's
            // cmb_bin_location - loaded per-warehouse from Warehouse Setup, left editable
            // rather than locked to the master list.
            this.cmb_dest_bin_location.FormattingEnabled = true;
            this.cmb_dest_bin_location.Location = new System.Drawing.Point(150, 145);
            this.cmb_dest_bin_location.Name = "cmb_dest_bin_location";
            this.cmb_dest_bin_location.Size = new System.Drawing.Size(202, 21);
            this.cmb_dest_bin_location.TabIndex = 6;
            //
            // lbl_remarks
            //
            this.lbl_remarks.AutoSize = true;
            this.lbl_remarks.Location = new System.Drawing.Point(12, 181);
            this.lbl_remarks.Name = "lbl_remarks";
            this.lbl_remarks.Size = new System.Drawing.Size(103, 13);
            this.lbl_remarks.TabIndex = 7;
            this.lbl_remarks.Text = "Reason for transfer:";
            //
            // txt_remarks
            //
            this.txt_remarks.Location = new System.Drawing.Point(12, 198);
            this.txt_remarks.Multiline = true;
            this.txt_remarks.Name = "txt_remarks";
            this.txt_remarks.Size = new System.Drawing.Size(340, 55);
            this.txt_remarks.TabIndex = 8;
            //
            // btn_ok
            //
            this.btn_ok.Location = new System.Drawing.Point(196, 268);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 26);
            this.btn_ok.TabIndex = 9;
            this.btn_ok.Text = "Transfer";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            //
            // btn_cancel
            //
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(277, 268);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 26);
            this.btn_cancel.TabIndex = 10;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            //
            // ItemStockTransferModal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(364, 306);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_remarks);
            this.Controls.Add(this.lbl_remarks);
            this.Controls.Add(this.cmb_dest_bin_location);
            this.Controls.Add(this.lbl_dest_bin);
            this.Controls.Add(this.cmb_dest_warehouse);
            this.Controls.Add(this.lbl_dest_warehouse);
            this.Controls.Add(this.num_transfer_qty);
            this.Controls.Add(this.lbl_transfer_qty);
            this.Controls.Add(this.lbl_info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemStockTransferModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer Stock";
            this.Load += new System.EventHandler(this.ItemStockTransferModal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_transfer_qty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.Label lbl_transfer_qty;
        private System.Windows.Forms.NumericUpDown num_transfer_qty;
        private System.Windows.Forms.Label lbl_dest_warehouse;
        private System.Windows.Forms.ComboBox cmb_dest_warehouse;
        private System.Windows.Forms.Label lbl_dest_bin;
        private System.Windows.Forms.ComboBox cmb_dest_bin_location;
        private System.Windows.Forms.Label lbl_remarks;
        private System.Windows.Forms.TextBox txt_remarks;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
    }
}
