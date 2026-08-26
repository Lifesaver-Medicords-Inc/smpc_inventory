namespace smpc_inventory_app.Pages.Inventory
{
    partial class ItemStocksPage
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
            this.btn_adjust = new System.Windows.Forms.Button();
            this.btn_transfer = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.lbl_search = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.dgv_item_stocks = new System.Windows.Forms.DataGridView();
            this.col_item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_item_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_warehouse_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_bin_location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_stock_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_stock_uom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_is_active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnl_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_item_stocks)).BeginInit();
            this.SuspendLayout();
            //
            // pnl_top
            //
            this.pnl_top.Controls.Add(this.btn_adjust);
            this.pnl_top.Controls.Add(this.btn_transfer);
            this.pnl_top.Controls.Add(this.btn_add);
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
            this.lbl_title.Size = new System.Drawing.Size(255, 20);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "INVENTORY ITEM STOCKS";
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
            // btn_add
            //
            this.btn_add.Location = new System.Drawing.Point(765, 9);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(90, 24);
            this.btn_add.TabIndex = 4;
            this.btn_add.Text = "Add Stock";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            //
            // btn_adjust
            //
            this.btn_adjust.Location = new System.Drawing.Point(865, 9);
            this.btn_adjust.Name = "btn_adjust";
            this.btn_adjust.Size = new System.Drawing.Size(110, 24);
            this.btn_adjust.TabIndex = 5;
            this.btn_adjust.Text = "Adjust Stock";
            this.btn_adjust.UseVisualStyleBackColor = true;
            this.btn_adjust.Click += new System.EventHandler(this.btn_adjust_Click);
            //
            // btn_transfer
            //
            // §10.6 Transfer function - Admin/Warehouse Manager gated, same as Adjust
            // Stock (HasAdjustAuthority).
            this.btn_transfer.Location = new System.Drawing.Point(981, 9);
            this.btn_transfer.Name = "btn_transfer";
            this.btn_transfer.Size = new System.Drawing.Size(110, 24);
            this.btn_transfer.TabIndex = 6;
            this.btn_transfer.Text = "Transfer Stock";
            this.btn_transfer.UseVisualStyleBackColor = true;
            this.btn_transfer.Click += new System.EventHandler(this.btn_transfer_Click);
            //
            // dgv_item_stocks
            //
            this.dgv_item_stocks.AllowUserToAddRows = false;
            this.dgv_item_stocks.AllowUserToDeleteRows = false;
            this.dgv_item_stocks.AutoGenerateColumns = false;
            this.dgv_item_stocks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_item_stocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_item_code,
            this.col_item_name,
            this.col_brand,
            this.col_warehouse_name,
            this.col_bin_location,
            this.col_stock_qty,
            this.col_stock_uom,
            this.col_is_active});
            this.dgv_item_stocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_item_stocks.Location = new System.Drawing.Point(0, 42);
            this.dgv_item_stocks.Name = "dgv_item_stocks";
            this.dgv_item_stocks.ReadOnly = true;
            this.dgv_item_stocks.RowHeadersWidth = 25;
            this.dgv_item_stocks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_item_stocks.MultiSelect = false;
            this.dgv_item_stocks.Size = new System.Drawing.Size(1000, 458);
            this.dgv_item_stocks.TabIndex = 1;
            this.dgv_item_stocks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_item_stocks_CellDoubleClick);
            //
            // col_item_code
            //
            this.col_item_code.DataPropertyName = "item_code";
            this.col_item_code.HeaderText = "Item Code";
            this.col_item_code.Name = "col_item_code";
            this.col_item_code.ReadOnly = true;
            this.col_item_code.Width = 110;
            //
            // col_item_name
            //
            this.col_item_name.DataPropertyName = "item_name";
            this.col_item_name.HeaderText = "Item Name";
            this.col_item_name.Name = "col_item_name";
            this.col_item_name.ReadOnly = true;
            this.col_item_name.Width = 150;
            //
            // col_brand
            //
            this.col_brand.DataPropertyName = "brand";
            this.col_brand.HeaderText = "Brand";
            this.col_brand.Name = "col_brand";
            this.col_brand.ReadOnly = true;
            this.col_brand.Width = 100;
            //
            // col_warehouse_name
            //
            this.col_warehouse_name.DataPropertyName = "warehouse_name";
            this.col_warehouse_name.HeaderText = "Warehouse";
            this.col_warehouse_name.Name = "col_warehouse_name";
            this.col_warehouse_name.ReadOnly = true;
            this.col_warehouse_name.Width = 120;
            //
            // col_bin_location
            //
            this.col_bin_location.DataPropertyName = "bin_location";
            this.col_bin_location.HeaderText = "Bin Location";
            this.col_bin_location.Name = "col_bin_location";
            this.col_bin_location.ReadOnly = true;
            this.col_bin_location.Width = 130;
            //
            // col_stock_qty
            //
            this.col_stock_qty.DataPropertyName = "stock_qty";
            this.col_stock_qty.HeaderText = "Stock Qty";
            this.col_stock_qty.Name = "col_stock_qty";
            this.col_stock_qty.ReadOnly = true;
            this.col_stock_qty.Width = 80;
            //
            // col_stock_uom
            //
            this.col_stock_uom.DataPropertyName = "stock_uom";
            this.col_stock_uom.HeaderText = "UOM";
            this.col_stock_uom.Name = "col_stock_uom";
            this.col_stock_uom.ReadOnly = true;
            this.col_stock_uom.Width = 70;
            //
            // col_is_active
            //
            this.col_is_active.DataPropertyName = "is_active";
            this.col_is_active.HeaderText = "Active";
            this.col_is_active.Name = "col_is_active";
            this.col_is_active.ReadOnly = true;
            this.col_is_active.Width = 60;
            //
            // ItemStocksPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgv_item_stocks);
            this.Controls.Add(this.pnl_top);
            this.Name = "ItemStocksPage";
            this.Size = new System.Drawing.Size(1000, 500);
            this.Load += new System.EventHandler(this.ItemStocksPage_Load);
            this.pnl_top.ResumeLayout(false);
            this.pnl_top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_item_stocks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_top;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_search;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_adjust;
        private System.Windows.Forms.Button btn_transfer;
        private System.Windows.Forms.DataGridView dgv_item_stocks;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_warehouse_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_bin_location;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_stock_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_stock_uom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_is_active;
    }
}
