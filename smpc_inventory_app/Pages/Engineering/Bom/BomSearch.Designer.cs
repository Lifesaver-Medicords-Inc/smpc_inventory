
namespace smpc_inventory_app.Pages.Engineering.Bom
{
    partial class BomSearch
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
            this.dgv_bomsearch = new System.Windows.Forms.DataGridView();
            this.item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.general_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.short_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bomsearch)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_bomsearch
            // 
            this.dgv_bomsearch.AllowUserToAddRows = false;
            this.dgv_bomsearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bomsearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.item_id,
            this.general_name,
            this.short_desc,
            this.item_code,
            this.item_model});
            this.dgv_bomsearch.Location = new System.Drawing.Point(-1, 39);
            this.dgv_bomsearch.Name = "dgv_bomsearch";
            this.dgv_bomsearch.Size = new System.Drawing.Size(802, 389);
            this.dgv_bomsearch.TabIndex = 1;
            this.dgv_bomsearch.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_items_CellDoubleClick);
            // 
            // item_id
            // 
            this.item_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_id.DataPropertyName = "item_id";
            this.item_id.HeaderText = "ITEM ID";
            this.item_id.Name = "item_id";
            this.item_id.ReadOnly = true;
            this.item_id.Visible = false;
            // 
            // general_name
            // 
            this.general_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.general_name.DataPropertyName = "general_name";
            this.general_name.HeaderText = "GENERAL NAME";
            this.general_name.Name = "general_name";
            this.general_name.ReadOnly = true;
            // 
            // short_desc
            // 
            this.short_desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.short_desc.DataPropertyName = "short_desc";
            this.short_desc.HeaderText = "SHORT DESCRIPTION";
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
            // item_model
            // 
            this.item_model.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_model.DataPropertyName = "item_model";
            this.item_model.HeaderText = "ITEM MODEL";
            this.item_model.Name = "item_model";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(351, 49);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(100, 20);
            this.txt_search.TabIndex = 2;
            // 
            // BomSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv_bomsearch);
            this.Controls.Add(this.txt_search);
            this.Name = "BomSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bom Search";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bomsearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_bomsearch;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn general_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn short_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_model;
    }
}