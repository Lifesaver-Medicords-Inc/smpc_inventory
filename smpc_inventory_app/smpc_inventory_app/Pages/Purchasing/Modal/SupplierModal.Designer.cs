
namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    partial class SupplierModal
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
            this.BPI = new System.Windows.Forms.Label();
            this.dgv_supplier = new System.Windows.Forms.DataGridView();
            this.supplier_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supplier)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BPI);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 47);
            this.panel1.TabIndex = 0;
            // 
            // BPI
            // 
            this.BPI.AutoSize = true;
            this.BPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BPI.Location = new System.Drawing.Point(12, 9);
            this.BPI.Name = "BPI";
            this.BPI.Size = new System.Drawing.Size(142, 24);
            this.BPI.TabIndex = 1;
            this.BPI.Text = "SUPPLIER LIST";
            // 
            // dgv_supplier
            // 
            this.dgv_supplier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_supplier.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.supplier_id,
            this.supplier_name,
            this.contact_no});
            this.dgv_supplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_supplier.Location = new System.Drawing.Point(0, 47);
            this.dgv_supplier.Name = "dgv_supplier";
            this.dgv_supplier.Size = new System.Drawing.Size(372, 305);
            this.dgv_supplier.TabIndex = 1;
            this.dgv_supplier.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_supplier_CellClick);
            // 
            // supplier_id
            // 
            this.supplier_id.DataPropertyName = "supplier_id";
            this.supplier_id.HeaderText = "SUPPLIER ID";
            this.supplier_id.Name = "supplier_id";
            // 
            // supplier_name
            // 
            this.supplier_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier_name.DataPropertyName = "supplier";
            this.supplier_name.HeaderText = "SUPPLIER";
            this.supplier_name.Name = "supplier_name";
            // 
            // contact_no
            // 
            this.contact_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.contact_no.DataPropertyName = "contact_no";
            this.contact_no.HeaderText = "CONTACT NO.";
            this.contact_no.Name = "contact_no";
            // 
            // SupplierModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 352);
            this.Controls.Add(this.dgv_supplier);
            this.Controls.Add(this.panel1);
            this.Name = "SupplierModal";
            this.Text = "SupplierModal";
            this.Load += new System.EventHandler(this.ItemModal_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_supplier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label BPI;
        private System.Windows.Forms.DataGridView dgv_supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_no;
    }
}