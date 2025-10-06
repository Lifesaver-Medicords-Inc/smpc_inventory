
namespace smpc_sales_app.Pages
{
    partial class ItemModal
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
            this.pnl_title = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnl_dgv = new System.Windows.Forms.Panel();
            this.dg_ItemList = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.short_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_model_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_brand_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_trade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_tangible = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.item_price = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnl_title.SuspendLayout();
            this.pnl_dgv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_title
            // 
            this.pnl_title.Controls.Add(this.label1);
            this.pnl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_title.Location = new System.Drawing.Point(0, 0);
            this.pnl_title.Name = "pnl_title";
            this.pnl_title.Size = new System.Drawing.Size(818, 62);
            this.pnl_title.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ITEM LIST";
            // 
            // pnl_dgv
            // 
            this.pnl_dgv.Controls.Add(this.dg_ItemList);
            this.pnl_dgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_dgv.Location = new System.Drawing.Point(0, 62);
            this.pnl_dgv.Name = "pnl_dgv";
            this.pnl_dgv.Size = new System.Drawing.Size(818, 382);
            this.pnl_dgv.TabIndex = 1;
            // 
            // dg_ItemList
            // 
            this.dg_ItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.short_desc,
            this.item_code,
            this.general_name,
            this.item_model_name,
            this.item_brand_name,
            this.status_trade,

            this.status_tangible,
            this.item_price});

            this.dg_ItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_ItemList.Location = new System.Drawing.Point(0, 0);
            this.dg_ItemList.MultiSelect = false;
            this.dg_ItemList.Name = "dg_ItemList";
            this.dg_ItemList.ReadOnly = true;
            this.dg_ItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_ItemList.Size = new System.Drawing.Size(818, 382);
            this.dg_ItemList.TabIndex = 0;
            this.dg_ItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_itemList_CellClick);
            this.dg_ItemList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_ItemList_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // short_desc
            // 
            this.short_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.short_desc.DataPropertyName = "short_desc";
            this.short_desc.HeaderText = "DESCRIPTION";
            this.short_desc.Name = "short_desc";
            this.short_desc.ReadOnly = true;
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // general_name
            // 
            this.general_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.general_name.DataPropertyName = "general_name";
            this.general_name.HeaderText = "GENERAL NAME";
            this.general_name.Name = "general_name";
            this.general_name.ReadOnly = true;
            // 
            // item_model_name
            // 
            this.item_model_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_model_name.DataPropertyName = "item_model_name";
            this.item_model_name.HeaderText = "ITEM MODEL";
            this.item_model_name.Name = "item_model_name";
            this.item_model_name.ReadOnly = true;
            // 
            // item_brand_name
            // 
            this.item_brand_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_brand_name.DataPropertyName = "item_brand_name";
            this.item_brand_name.HeaderText = "ITEM BRAND";
            this.item_brand_name.Name = "item_brand_name";
            this.item_brand_name.ReadOnly = true;
            // 
            // status_trade
            // 
            this.status_trade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status_trade.DataPropertyName = "status_trade";
            this.status_trade.HeaderText = "STATUS TRADE";
            this.status_trade.Name = "status_trade";
            this.status_trade.ReadOnly = true;
            // 
            // status_tangible
            // 
            this.status_tangible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status_tangible.DataPropertyName = "status_tangible";
            this.status_tangible.HeaderText = "STATUS TANGIBLE";
            this.status_tangible.Name = "status_tangible";
            this.status_tangible.ReadOnly = true;
            // 

            // item_price
            // 
            this.item_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_price.HeaderText = "ITEM_PRICE";
            this.item_price.Name = "item_price";
            this.item_price.ReadOnly = true;
            // 

            // ItemModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 613);
            this.Controls.Add(this.pnl_dgv);
            this.Controls.Add(this.pnl_title);
            this.Name = "ItemModal";
            this.Text = "ItemModal";
            this.Load += new System.EventHandler(this.ItemModal_Load);
            this.pnl_title.ResumeLayout(false);
            this.pnl_title.PerformLayout();
            this.pnl_dgv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ItemList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnl_dgv;
        private System.Windows.Forms.DataGridView dg_ItemList;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn short_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_model_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_brand_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_trade;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_tangible;

        private System.Windows.Forms.DataGridViewTextBoxColumn item_price;

    }
}