
namespace smpc_inventory_app.Pages.Inventory.ReceivingReportModals
{
    partial class ReceivingReportSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_rr_search = new System.Windows.Forms.DataGridView();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_received = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prepared_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ref_doc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rr_search)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_rr_search
            // 
            this.dgv_rr_search.AllowUserToAddRows = false;
            this.dgv_rr_search.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_rr_search.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_rr_search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_rr_search.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.supplier_name,
            this.supplier_code,
            this.date_received,
            this.warehouse_name,
            this.prepared_by,
            this.ref_doc});
            this.dgv_rr_search.Location = new System.Drawing.Point(-1, 31);
            this.dgv_rr_search.Name = "dgv_rr_search";
            this.dgv_rr_search.Size = new System.Drawing.Size(802, 389);
            this.dgv_rr_search.TabIndex = 2;
            this.dgv_rr_search.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_rr_search_CellClick);
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(350, 215);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(100, 20);
            this.txt_search.TabIndex = 3;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 80;
            // 
            // supplier_name
            // 
            this.supplier_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.supplier_name.DataPropertyName = "supplier_name";
            this.supplier_name.HeaderText = "SUPPLIER NAME";
            this.supplier_name.Name = "supplier_name";
            this.supplier_name.ReadOnly = true;
            this.supplier_name.Width = 140;
            // 
            // supplier_code
            // 
            this.supplier_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.supplier_code.DataPropertyName = "supplier_code";
            this.supplier_code.HeaderText = "SUPPLIER CODE";
            this.supplier_code.Name = "supplier_code";
            this.supplier_code.ReadOnly = true;
            this.supplier_code.Width = 140;
            // 
            // date_received
            // 
            this.date_received.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.date_received.DataPropertyName = "date_received";
            this.date_received.HeaderText = "DATE RECEIVED";
            this.date_received.MinimumWidth = 150;
            this.date_received.Name = "date_received";
            this.date_received.ReadOnly = true;
            // 
            // warehouse_name
            // 
            this.warehouse_name.DataPropertyName = "warehouse_name";
            this.warehouse_name.HeaderText = "WAREHOUSE NAME";
            this.warehouse_name.Name = "warehouse_name";
            this.warehouse_name.ReadOnly = true;
            this.warehouse_name.Width = 140;
            // 
            // prepared_by
            // 
            this.prepared_by.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.prepared_by.DataPropertyName = "prepared_by";
            this.prepared_by.HeaderText = "PREPARED BY";
            this.prepared_by.Name = "prepared_by";
            this.prepared_by.ReadOnly = true;
            this.prepared_by.Width = 140;
            // 
            // ref_doc
            // 
            this.ref_doc.DataPropertyName = "ref_doc";
            this.ref_doc.HeaderText = "REFERENCE DOC";
            this.ref_doc.Name = "ref_doc";
            this.ref_doc.ReadOnly = true;
            this.ref_doc.Width = 140;
            // 
            // ReceivingReportSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv_rr_search);
            this.Controls.Add(this.txt_search);
            this.Name = "ReceivingReportSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receiving Report Search";
            this.Load += new System.EventHandler(this.ReceivingReportSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rr_search)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_rr_search;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_received;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn prepared_by;
        private System.Windows.Forms.DataGridViewTextBoxColumn ref_doc;
    }
}