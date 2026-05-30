
namespace smpc_inventory_app.Pages.Inventory.ReceivingReport2.ReceivingReport2Modals
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_rr_search = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_received = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ref_doc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.doc_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prepared_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rr_search)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_rr_search
            // 
            this.dgv_rr_search.AllowUserToAddRows = false;
            this.dgv_rr_search.AllowUserToDeleteRows = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_rr_search.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_rr_search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_rr_search.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.supplier,
            this.supplier_code,
            this.date_received,
            this.warehouse,
            this.ref_doc,
            this.doc_no,
            this.prepared_by});
            this.dgv_rr_search.Location = new System.Drawing.Point(-1, 31);
            this.dgv_rr_search.Name = "dgv_rr_search";
            this.dgv_rr_search.Size = new System.Drawing.Size(802, 389);
            this.dgv_rr_search.TabIndex = 11;
            this.dgv_rr_search.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_rr_search_CellClick);
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
            // supplier
            // 
            this.supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier.DataPropertyName = "supplier";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Gainsboro;
            this.supplier.DefaultCellStyle = dataGridViewCellStyle10;
            this.supplier.HeaderText = "SUPPLIER";
            this.supplier.Name = "supplier";
            this.supplier.ReadOnly = true;
            // 
            // supplier_code
            // 
            this.supplier_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier_code.DataPropertyName = "supplier_code";
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Gainsboro;
            this.supplier_code.DefaultCellStyle = dataGridViewCellStyle11;
            this.supplier_code.HeaderText = "SUPPLIER CODE";
            this.supplier_code.Name = "supplier_code";
            this.supplier_code.ReadOnly = true;
            // 
            // date_received
            // 
            this.date_received.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.date_received.DataPropertyName = "date_received";
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Gainsboro;
            this.date_received.DefaultCellStyle = dataGridViewCellStyle12;
            this.date_received.HeaderText = "DATE RECEIVED";
            this.date_received.Name = "date_received";
            this.date_received.ReadOnly = true;
            // 
            // warehouse
            // 
            this.warehouse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.warehouse.DataPropertyName = "warehouse";
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Gainsboro;
            this.warehouse.DefaultCellStyle = dataGridViewCellStyle13;
            this.warehouse.HeaderText = "WAREHOUSE";
            this.warehouse.MinimumWidth = 150;
            this.warehouse.Name = "warehouse";
            this.warehouse.ReadOnly = true;
            // 
            // ref_doc
            // 
            this.ref_doc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ref_doc.DataPropertyName = "ref_doc";
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Gainsboro;
            this.ref_doc.DefaultCellStyle = dataGridViewCellStyle14;
            this.ref_doc.HeaderText = "REFERENCE DOC";
            this.ref_doc.Name = "ref_doc";
            this.ref_doc.ReadOnly = true;
            // 
            // doc_no
            // 
            this.doc_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.doc_no.DataPropertyName = "doc_no";
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.Gainsboro;
            this.doc_no.DefaultCellStyle = dataGridViewCellStyle15;
            this.doc_no.HeaderText = "DOC NO";
            this.doc_no.Name = "doc_no";
            this.doc_no.ReadOnly = true;
            // 
            // prepared_by
            // 
            this.prepared_by.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.prepared_by.DataPropertyName = "prepared_by";
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Gainsboro;
            this.prepared_by.DefaultCellStyle = dataGridViewCellStyle16;
            this.prepared_by.HeaderText = "PREPARED BY";
            this.prepared_by.Name = "prepared_by";
            this.prepared_by.ReadOnly = true;
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(350, 215);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(100, 20);
            this.txt_search.TabIndex = 12;
            // 
            // ReceivingReportSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv_rr_search);
            this.Controls.Add(this.txt_search);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ReceivingReportSearch";
            this.Text = "ReceivingReportSearch";
            this.Load += new System.EventHandler(this.ReceivingReportSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rr_search)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_rr_search;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_received;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn ref_doc;
        private System.Windows.Forms.DataGridViewTextBoxColumn doc_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn prepared_by;
    }
}