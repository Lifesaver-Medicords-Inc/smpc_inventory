
namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    partial class ViewDetailsModal
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_details = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_details)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_details
            // 
            this.dgv_details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_details.Location = new System.Drawing.Point(0, 0);
            this.dgv_details.Name = "dgv_details";
            this.dgv_details.Size = new System.Drawing.Size(411, 267);
            this.dgv_details.TabIndex = 0;
            // 
            // ViewDetailsModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 267);
            this.Controls.Add(this.dgv_details);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ViewDetailsModal";
            this.Text = "ViewDetailsModal";
            this.Load += new System.EventHandler(this.ViewDetailsModal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_details)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_details;
    }
}