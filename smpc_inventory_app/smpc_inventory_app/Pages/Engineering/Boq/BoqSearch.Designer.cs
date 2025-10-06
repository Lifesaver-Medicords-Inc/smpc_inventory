
namespace smpc_inventory_app.Pages.Engineering.Boq
{
    partial class BoqSearch
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
            this.dgv_boqsearch = new System.Windows.Forms.DataGridView();
            this.finalize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.project_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_set_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.set_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_boqsearch)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_boqsearch
            // 
            this.dgv_boqsearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_boqsearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.finalize,
            this.customer_name,
            this.project_name,
            this.item_set_name,
            this.set_id});
            this.dgv_boqsearch.Location = new System.Drawing.Point(-1, 32);
            this.dgv_boqsearch.Name = "dgv_boqsearch";
            this.dgv_boqsearch.Size = new System.Drawing.Size(802, 406);
            this.dgv_boqsearch.TabIndex = 0;
            this.dgv_boqsearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_items_CellClick);
            // 
            // finalize
            // 
            this.finalize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.finalize.DataPropertyName = "finalize";
            this.finalize.HeaderText = "FINALIZE QUOTE";
            this.finalize.Name = "finalize";
            this.finalize.ReadOnly = true;
            // 
            // customer_name
            // 
            this.customer_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customer_name.DataPropertyName = "customer_name";
            this.customer_name.HeaderText = "CLIENT NAME";
            this.customer_name.Name = "customer_name";
            this.customer_name.ReadOnly = true;
            // 
            // project_name
            // 
            this.project_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.project_name.DataPropertyName = "project_name";
            this.project_name.HeaderText = "PROJECT NAME";
            this.project_name.Name = "project_name";
            this.project_name.ReadOnly = true;
            // 
            // item_set_name
            // 
            this.item_set_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_set_name.DataPropertyName = "item_set_name";
            this.item_set_name.HeaderText = "ITEM/SET NAME";
            this.item_set_name.Name = "item_set_name";
            this.item_set_name.ReadOnly = true;
            // 
            // set_id
            // 
            this.set_id.DataPropertyName = "set_id";
            this.set_id.HeaderText = "Column3";
            this.set_id.Name = "set_id";
            this.set_id.Visible = false;
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(252, 32);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(100, 20);
            this.txt_search.TabIndex = 1;
            // 
            // BoqSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv_boqsearch);
            this.Controls.Add(this.txt_search);
            this.Name = "BoqSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BoqSearch";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_boqsearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_boqsearch;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn finalize;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn project_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_set_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn set_id;
    }
}