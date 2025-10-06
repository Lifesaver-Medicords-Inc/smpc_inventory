
namespace smpc_inventory_app.Pages.Engineering.Bom
{
    partial class BomItemModal
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
            this.dg_BomItemList = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.short_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uom_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg_BomItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_BomItemList
            // 
            this.dg_BomItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_BomItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.short_desc,
            this.item_code,
            this.general_name,
            this.item_model,
            this.uom_name,
            this.size});
            this.dg_BomItemList.Location = new System.Drawing.Point(2, 57);
            this.dg_BomItemList.Name = "dg_BomItemList";
            this.dg_BomItemList.Size = new System.Drawing.Size(818, 382);
            this.dg_BomItemList.TabIndex = 0;
            this.dg_BomItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_BomItemList_CellClick);
            this.dg_BomItemList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_BomItemList_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // short_desc
            // 
            this.short_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.short_desc.DataPropertyName = "short_desc";
            this.short_desc.HeaderText = "DESCRIPTION";
            this.short_desc.Name = "short_desc";
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "ITEM CODE";
            this.item_code.Name = "item_code";
            // 
            // general_name
            // 
            this.general_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.general_name.DataPropertyName = "general_name";
            this.general_name.HeaderText = "GENERAL NAME";
            this.general_name.Name = "general_name";
            // 
            // item_model
            // 
            this.item_model.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_model.DataPropertyName = "item_model";
            this.item_model.HeaderText = "ITEM MODEL";
            this.item_model.Name = "item_model";
            // 
            // uom_name
            // 
            this.uom_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.uom_name.DataPropertyName = "uom_name";
            this.uom_name.HeaderText = "UOM";
            this.uom_name.Name = "uom_name";
            // 
            // size
            // 
            this.size.DataPropertyName = "size";
            this.size.HeaderText = "SIZE";
            this.size.Name = "size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "BOM ITEM LIST";
            // 
            // BomItemModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 448);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_BomItemList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BomItemModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BomItemModal";
            this.Load += new System.EventHandler(this.BomItemModal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_BomItemList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_BomItemList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn short_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn uom_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
    }
}