
namespace smpc_inventory_app.Pages.Shared
{
    partial class PrintDocumentUC
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
            this.pnl_print = new System.Windows.Forms.Panel();
            this.pnl_dgv = new System.Windows.Forms.Panel();
            this.pnl_print.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_print
            // 
            this.pnl_print.Controls.Add(this.pnl_dgv);
            this.pnl_print.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_print.Location = new System.Drawing.Point(0, 0);
            this.pnl_print.Name = "pnl_print";
            this.pnl_print.Size = new System.Drawing.Size(816, 797);
            this.pnl_print.TabIndex = 16;
            // 
            // pnl_dgv
            // 
            this.pnl_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_dgv.Location = new System.Drawing.Point(0, 0);
            this.pnl_dgv.Name = "pnl_dgv";
            this.pnl_dgv.Size = new System.Drawing.Size(816, 797);
            this.pnl_dgv.TabIndex = 1;
            // 
            // PrintDocumentUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_print);
            this.Name = "PrintDocumentUC";
            this.Size = new System.Drawing.Size(816, 797);
            this.pnl_print.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_print;
        private System.Windows.Forms.Panel pnl_dgv;
    }
}
