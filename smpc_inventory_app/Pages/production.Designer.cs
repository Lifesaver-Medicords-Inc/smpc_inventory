namespace smpc_inventory_app.Pages
{
    partial class production
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnl_top = new System.Windows.Forms.Panel();
            this.btn_acknowledge = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.lbl_search = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.dgv_production = new System.Windows.Forms.DataGridView();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_sales_order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_item_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_due = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_serial_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_so_item_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_production)).BeginInit();
            this.SuspendLayout();
            //
            // pnl_top
            //
            this.pnl_top.Controls.Add(this.btn_acknowledge);
            this.pnl_top.Controls.Add(this.btn_refresh);
            this.pnl_top.Controls.Add(this.txt_search);
            this.pnl_top.Controls.Add(this.lbl_search);
            this.pnl_top.Controls.Add(this.lbl_title);
            this.pnl_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_top.Location = new System.Drawing.Point(0, 0);
            this.pnl_top.Name = "pnl_top";
            this.pnl_top.Size = new System.Drawing.Size(1000, 42);
            this.pnl_top.TabIndex = 0;
            //
            // lbl_title
            //
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(10, 10);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(300, 20);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "PRODUCTION REPORT - PENDING ACKNOWLEDGEMENT";
            //
            // lbl_search
            //
            this.lbl_search.AutoSize = true;
            this.lbl_search.Location = new System.Drawing.Point(400, 14);
            this.lbl_search.Name = "lbl_search";
            this.lbl_search.Size = new System.Drawing.Size(44, 13);
            this.lbl_search.TabIndex = 1;
            this.lbl_search.Text = "Search:";
            //
            // txt_search
            //
            this.txt_search.Location = new System.Drawing.Point(450, 11);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(220, 20);
            this.txt_search.TabIndex = 2;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            //
            // btn_refresh
            //
            this.btn_refresh.Location = new System.Drawing.Point(680, 9);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 24);
            this.btn_refresh.TabIndex = 3;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            //
            // btn_acknowledge
            //
            this.btn_acknowledge.Location = new System.Drawing.Point(765, 9);
            this.btn_acknowledge.Name = "btn_acknowledge";
            this.btn_acknowledge.Size = new System.Drawing.Size(130, 24);
            this.btn_acknowledge.TabIndex = 4;
            this.btn_acknowledge.Text = "Acknowledge";
            this.btn_acknowledge.UseVisualStyleBackColor = true;
            this.btn_acknowledge.Click += new System.EventHandler(this.btn_acknowledge_Click);
            //
            // dgv_production
            //
            this.dgv_production.AllowUserToAddRows = false;
            this.dgv_production.AllowUserToDeleteRows = false;
            this.dgv_production.AutoGenerateColumns = false;
            this.dgv_production.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_production.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_id,
            this.col_sales_order,
            this.col_item_desc,
            this.col_type,
            this.col_quantity,
            this.col_due,
            this.col_serial_no,
            this.col_so_item_status});
            this.dgv_production.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_production.Location = new System.Drawing.Point(0, 42);
            this.dgv_production.Name = "dgv_production";
            this.dgv_production.ReadOnly = true;
            this.dgv_production.RowHeadersWidth = 25;
            this.dgv_production.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_production.MultiSelect = false;
            this.dgv_production.Size = new System.Drawing.Size(1000, 458);
            this.dgv_production.TabIndex = 1;
            this.dgv_production.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_production_CellDoubleClick);
            //
            // col_id
            //
            this.col_id.DataPropertyName = "id";
            this.col_id.HeaderText = "id";
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Visible = false;
            //
            // col_sales_order
            //
            this.col_sales_order.DataPropertyName = "sales_order";
            this.col_sales_order.HeaderText = "SALES ORDER";
            this.col_sales_order.Name = "col_sales_order";
            this.col_sales_order.ReadOnly = true;
            //
            // col_item_desc
            //
            this.col_item_desc.DataPropertyName = "item_desc";
            this.col_item_desc.HeaderText = "ITEM DESCRIPTION";
            this.col_item_desc.Name = "col_item_desc";
            this.col_item_desc.ReadOnly = true;
            //
            // col_type
            //
            this.col_type.DataPropertyName = "type";
            this.col_type.HeaderText = "MODEL";
            this.col_type.Name = "col_type";
            this.col_type.ReadOnly = true;
            //
            // col_quantity
            //
            this.col_quantity.DataPropertyName = "quantity";
            this.col_quantity.HeaderText = "QTY";
            this.col_quantity.Name = "col_quantity";
            this.col_quantity.ReadOnly = true;
            //
            // col_due
            //
            this.col_due.DataPropertyName = "due";
            this.col_due.HeaderText = "DUE";
            this.col_due.Name = "col_due";
            this.col_due.ReadOnly = true;
            //
            // col_serial_no
            //
            this.col_serial_no.DataPropertyName = "serial_no";
            this.col_serial_no.HeaderText = "SERIAL NO.";
            this.col_serial_no.Name = "col_serial_no";
            this.col_serial_no.ReadOnly = true;
            //
            // col_so_item_status
            //
            this.col_so_item_status.DataPropertyName = "so_item_status";
            this.col_so_item_status.HeaderText = "ITEM STATUS";
            this.col_so_item_status.Name = "col_so_item_status";
            this.col_so_item_status.ReadOnly = true;
            //
            // production
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgv_production);
            this.Controls.Add(this.pnl_top);
            this.Name = "production";
            this.Size = new System.Drawing.Size(1000, 500);
            this.Load += new System.EventHandler(this.production_Load);
            this.pnl_top.ResumeLayout(false);
            this.pnl_top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_production)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_top;
        private System.Windows.Forms.Button btn_acknowledge;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Label lbl_search;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.DataGridView dgv_production;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_sales_order;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_due;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_serial_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_so_item_status;
    }
}
