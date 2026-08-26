namespace smpc_inventory_app.Pages.Inventory
{
    partial class ProductionAcknowledgeModal
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
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_summary = new System.Windows.Forms.Label();
            this.lbl_warehouse = new System.Windows.Forms.Label();
            this.cmb_warehouse = new System.Windows.Forms.ComboBox();
            this.lbl_bin_location = new System.Windows.Forms.Label();
            this.cmb_bin_location = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lbl_title
            //
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(15, 15);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(238, 20);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "ACKNOWLEDGE PRODUCTION";
            //
            // lbl_summary
            //
            this.lbl_summary.AutoSize = true;
            this.lbl_summary.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_summary.Location = new System.Drawing.Point(15, 45);
            this.lbl_summary.MaximumSize = new System.Drawing.Size(360, 0);
            this.lbl_summary.Name = "lbl_summary";
            this.lbl_summary.Size = new System.Drawing.Size(50, 13);
            this.lbl_summary.TabIndex = 1;
            this.lbl_summary.Text = "lbl_summary";
            //
            // lbl_warehouse
            //
            this.lbl_warehouse.AutoSize = true;
            this.lbl_warehouse.Location = new System.Drawing.Point(15, 90);
            this.lbl_warehouse.Name = "lbl_warehouse";
            this.lbl_warehouse.Size = new System.Drawing.Size(63, 13);
            this.lbl_warehouse.TabIndex = 2;
            this.lbl_warehouse.Text = "Warehouse:";
            //
            // cmb_warehouse
            //
            this.cmb_warehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_warehouse.FormattingEnabled = true;
            this.cmb_warehouse.Location = new System.Drawing.Point(140, 87);
            this.cmb_warehouse.Name = "cmb_warehouse";
            this.cmb_warehouse.Size = new System.Drawing.Size(235, 21);
            this.cmb_warehouse.TabIndex = 3;
            this.cmb_warehouse.SelectedIndexChanged += new System.EventHandler(this.cmb_warehouse_SelectedIndexChanged);
            //
            // lbl_bin_location
            //
            this.lbl_bin_location.AutoSize = true;
            this.lbl_bin_location.Location = new System.Drawing.Point(15, 125);
            this.lbl_bin_location.Name = "lbl_bin_location";
            this.lbl_bin_location.Size = new System.Drawing.Size(75, 13);
            this.lbl_bin_location.TabIndex = 4;
            this.lbl_bin_location.Text = "Bin Location:";
            //
            // cmb_bin_location
            //
            this.cmb_bin_location.FormattingEnabled = true;
            this.cmb_bin_location.Location = new System.Drawing.Point(140, 122);
            this.cmb_bin_location.Name = "cmb_bin_location";
            this.cmb_bin_location.Size = new System.Drawing.Size(235, 21);
            this.cmb_bin_location.TabIndex = 5;
            //
            // btn_ok
            //
            this.btn_ok.Location = new System.Drawing.Point(199, 165);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(85, 27);
            this.btn_ok.TabIndex = 6;
            this.btn_ok.Text = "Acknowledge";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            //
            // btn_cancel
            //
            this.btn_cancel.Location = new System.Drawing.Point(290, 165);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(85, 27);
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            //
            // ProductionAcknowledgeModal
            //
            this.AcceptButton = this.btn_ok;
            this.CancelButton = this.btn_cancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 210);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cmb_bin_location);
            this.Controls.Add(this.lbl_bin_location);
            this.Controls.Add(this.cmb_warehouse);
            this.Controls.Add(this.lbl_warehouse);
            this.Controls.Add(this.lbl_summary);
            this.Controls.Add(this.lbl_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductionAcknowledgeModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acknowledge Production";
            this.Load += new System.EventHandler(this.ProductionAcknowledgeModal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_summary;
        private System.Windows.Forms.Label lbl_warehouse;
        private System.Windows.Forms.ComboBox cmb_warehouse;
        private System.Windows.Forms.Label lbl_bin_location;
        private System.Windows.Forms.ComboBox cmb_bin_location;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
    }
}
