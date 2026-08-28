
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
            this.pnl_footer = new System.Windows.Forms.Panel();
            this.btn_add_selected = new System.Windows.Forms.Button();
            this.pnl_dgv = new System.Windows.Forms.Panel();
            this.dg_ItemList = new System.Windows.Forms.DataGridView();
            this.selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_model_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_brand_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.long_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.short_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_tangible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_trade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_title.SuspendLayout();
            this.pnl_footer.SuspendLayout();
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
            // pnl_footer
            //
            this.pnl_footer.Controls.Add(this.btn_add_selected);
            this.pnl_footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_footer.Location = new System.Drawing.Point(0, 444);
            this.pnl_footer.Name = "pnl_footer";
            this.pnl_footer.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnl_footer.Size = new System.Drawing.Size(818, 46);
            this.pnl_footer.TabIndex = 2;
            //
            // btn_add_selected
            //
            this.btn_add_selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add_selected.Location = new System.Drawing.Point(636, 8);
            this.btn_add_selected.Name = "btn_add_selected";
            this.btn_add_selected.Size = new System.Drawing.Size(158, 28);
            this.btn_add_selected.TabIndex = 0;
            this.btn_add_selected.Text = "Add Selected Items";
            this.btn_add_selected.UseVisualStyleBackColor = true;
            this.btn_add_selected.Click += new System.EventHandler(this.btn_add_selected_Click);
            //
            // pnl_dgv
            //
            this.pnl_dgv.Controls.Add(this.dg_ItemList);
            this.pnl_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_dgv.Location = new System.Drawing.Point(0, 62);
            this.pnl_dgv.Name = "pnl_dgv";
            this.pnl_dgv.Size = new System.Drawing.Size(818, 382);
            this.pnl_dgv.TabIndex = 1;
            //
            // dg_ItemList
            //
            this.dg_ItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selected,
            this.id,
            this.general_name,
            this.item_type,
            this.item_model_name,
            this.item_brand_name,
            this.item_code,
            this.long_description,
            this.item_price,
            this.short_desc,
            this.status_tangible,
            this.status_trade});
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
            // selected
            //
            // Multi-select (requested): a real bool column added to the fetched
            // DataTable in code (GetItemList) rather than left typeless - a
            // DataGridViewCheckBoxColumn needs a bool-typed source to bind cleanly.
            this.selected.DataPropertyName = "Selected";
            this.selected.HeaderText = "";
            this.selected.Name = "selected";
            this.selected.Width = 30;
            //
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // general_name
            // 
            this.general_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.general_name.DataPropertyName = "general_name";
            this.general_name.HeaderText = "GENERAL NAME";
            this.general_name.Name = "general_name";
            this.general_name.ReadOnly = true;
            // 
            // item_type
            // 
            this.item_type.DataPropertyName = "item_type";
            this.item_type.HeaderText = "TYPE";
            this.item_type.Name = "item_type";
            this.item_type.ReadOnly = true;
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
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // long_description
            // 
            this.long_description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.long_description.DataPropertyName = "long_description";
            this.long_description.HeaderText = "DESCRIPTION";
            this.long_description.Name = "long_description";
            this.long_description.ReadOnly = true;
            // 
            // item_price
            // 
            this.item_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_price.DataPropertyName = "item_price";
            this.item_price.HeaderText = "PRICE";
            this.item_price.Name = "item_price";
            this.item_price.ReadOnly = true;
            //
            // short_desc / status_tangible / status_trade
            //
            // Hidden data-carrier columns, not shown in the picker grid - they exist
            // so GetResult() (btn_add_selected_Click) can read them into the returned
            // dictionary for BusinessPartnerInfo.cs's dg_items, which expects exactly
            // these three keys and previously had no source for them at all.
            this.short_desc.DataPropertyName = "short_desc";
            this.short_desc.Name = "short_desc";
            this.short_desc.ReadOnly = true;
            this.short_desc.Visible = false;
            this.status_tangible.DataPropertyName = "status_tangible";
            this.status_tangible.Name = "status_tangible";
            this.status_tangible.ReadOnly = true;
            this.status_tangible.Visible = false;
            this.status_trade.DataPropertyName = "status_trade";
            this.status_trade.Name = "status_trade";
            this.status_trade.ReadOnly = true;
            this.status_trade.Visible = false;
            //
            // ItemModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 613);
            this.Controls.Add(this.pnl_dgv);
            this.Controls.Add(this.pnl_footer);
            this.Controls.Add(this.pnl_title);
            this.Name = "ItemModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ItemModal";
            this.Load += new System.EventHandler(this.ItemModal_Load);
            this.pnl_title.ResumeLayout(false);
            this.pnl_title.PerformLayout();
            this.pnl_footer.ResumeLayout(false);
            this.pnl_dgv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ItemList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnl_footer;
        private System.Windows.Forms.Button btn_add_selected;
        private System.Windows.Forms.Panel pnl_dgv;
        private System.Windows.Forms.DataGridView dg_ItemList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_model_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_brand_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn long_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn short_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_tangible;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_trade;
    }
}