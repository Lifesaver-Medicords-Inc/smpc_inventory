
namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    partial class SalesOrderDistributionModal
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_search = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.footer_panel = new System.Windows.Forms.Panel();
            this.btn_done = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.footer_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 63);
            this.panel1.TabIndex = 9;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(670, 21);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 89;
            this.btn_search.Text = "SEARCH";
            this.btn_search.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(751, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 20);
            this.textBox1.TabIndex = 88;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Order Quantity Distribution";
            // 
            // footer_panel
            // 
            this.footer_panel.Controls.Add(this.btn_done);
            this.footer_panel.Controls.Add(this.btn_cancel);
            this.footer_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer_panel.Location = new System.Drawing.Point(0, 594);
            this.footer_panel.Name = "footer_panel";
            this.footer_panel.Size = new System.Drawing.Size(984, 67);
            this.footer_panel.TabIndex = 11;
            // 
            // btn_done
            // 
            this.btn_done.Location = new System.Drawing.Point(822, 21);
            this.btn_done.Name = "btn_done";
            this.btn_done.Size = new System.Drawing.Size(117, 23);
            this.btn_done.TabIndex = 89;
            this.btn_done.Text = "DONE";
            this.btn_done.UseVisualStyleBackColor = true;
            this.btn_done.Click += new System.EventHandler(this.btn_done_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(670, 21);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(117, 23);
            this.btn_cancel.TabIndex = 88;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 63);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(984, 531);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // SalesOrderDistributionModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.footer_panel);
            this.Controls.Add(this.panel1);
            this.Name = "SalesOrderDistributionModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrderDistribution";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.footer_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel footer_panel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_done;
        private System.Windows.Forms.Button btn_cancel;
    }
}