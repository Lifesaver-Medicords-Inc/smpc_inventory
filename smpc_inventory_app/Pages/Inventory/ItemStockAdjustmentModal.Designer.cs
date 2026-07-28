namespace smpc_inventory_app.Pages.Inventory
{
    partial class ItemStockAdjustmentModal
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
            this.lbl_new_qty = new System.Windows.Forms.Label();
            this.num_new_qty = new System.Windows.Forms.NumericUpDown();
            this.lbl_remarks = new System.Windows.Forms.Label();
            this.txt_remarks = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_new_qty)).BeginInit();
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
            // lbl_new_qty
            //
            this.lbl_new_qty.AutoSize = true;
            this.lbl_new_qty.Location = new System.Drawing.Point(12, 82);
            this.lbl_new_qty.Name = "lbl_new_qty";
            this.lbl_new_qty.Size = new System.Drawing.Size(89, 13);
            this.lbl_new_qty.TabIndex = 1;
            this.lbl_new_qty.Text = "New Stock Qty:";
            //
            // num_new_qty
            //
            this.num_new_qty.Location = new System.Drawing.Point(150, 80);
            this.num_new_qty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.num_new_qty.Name = "num_new_qty";
            this.num_new_qty.Size = new System.Drawing.Size(100, 20);
            this.num_new_qty.TabIndex = 2;
            //
            // lbl_remarks
            //
            this.lbl_remarks.AutoSize = true;
            this.lbl_remarks.Location = new System.Drawing.Point(12, 115);
            this.lbl_remarks.Name = "lbl_remarks";
            this.lbl_remarks.Size = new System.Drawing.Size(140, 13);
            this.lbl_remarks.TabIndex = 3;
            this.lbl_remarks.Text = "Reason for adjustment:";
            //
            // txt_remarks
            //
            this.txt_remarks.Location = new System.Drawing.Point(12, 132);
            this.txt_remarks.Multiline = true;
            this.txt_remarks.Name = "txt_remarks";
            this.txt_remarks.Size = new System.Drawing.Size(340, 60);
            this.txt_remarks.TabIndex = 4;
            //
            // btn_ok
            //
            this.btn_ok.Location = new System.Drawing.Point(196, 205);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 26);
            this.btn_ok.TabIndex = 5;
            this.btn_ok.Text = "Save";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            //
            // btn_cancel
            //
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(277, 205);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 26);
            this.btn_cancel.TabIndex = 6;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            //
            // ItemStockAdjustmentModal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 243);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_remarks);
            this.Controls.Add(this.lbl_remarks);
            this.Controls.Add(this.num_new_qty);
            this.Controls.Add(this.lbl_new_qty);
            this.Controls.Add(this.lbl_info);
            this.CancelButton = this.btn_cancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemStockAdjustmentModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjust Stock";
            ((System.ComponentModel.ISupportInitialize)(this.num_new_qty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.Label lbl_new_qty;
        private System.Windows.Forms.NumericUpDown num_new_qty;
        private System.Windows.Forms.Label lbl_remarks;
        private System.Windows.Forms.TextBox txt_remarks;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
    }
}
