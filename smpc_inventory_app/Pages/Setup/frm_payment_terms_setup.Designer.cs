
namespace Inventory_SMPC.Pages.Setup
{
    partial class frm_payment_terms_setup
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_payment_terms_setup));
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_records = new System.Windows.Forms.Panel();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_header = new System.Windows.Forms.Panel();
            this.dg_payment_terms = new System.Windows.Forms.DataGridView();
            this.dataBinding_payment_terms = new System.Windows.Forms.BindingSource(this.components);
            this.ds_payment_terms = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.is_selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_records.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel_header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_payment_terms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBinding_payment_terms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_payment_terms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(73, 45);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 10;
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(73, 24);
            this.txt_code.Name = "txt_code";
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Code";
            // 
            // panel_records
            // 
            this.panel_records.Controls.Add(this.txt_id);
            this.panel_records.Controls.Add(this.lbl_id);
            this.panel_records.Controls.Add(this.txt_name);
            this.panel_records.Controls.Add(this.txt_code);
            this.panel_records.Controls.Add(this.label2);
            this.panel_records.Controls.Add(this.label3);
            this.panel_records.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_records.Enabled = false;
            this.panel_records.Location = new System.Drawing.Point(0, 88);
            this.panel_records.Name = "panel_records";
            this.panel_records.Size = new System.Drawing.Size(634, 86);
            this.panel_records.TabIndex = 22;
            // 
            // txt_id
            // 
            this.txt_id.Enabled = false;
            this.txt_id.Location = new System.Drawing.Point(390, 24);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 16;
            this.txt_id.Visible = false;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(337, 27);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(16, 13);
            this.lbl_id.TabIndex = 15;
            this.lbl_id.Text = "Id";
            this.lbl_id.Visible = false;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(63, 22);
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.Image")));
            this.btn_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(60, 22);
            this.btn_delete.Text = "Delete";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Image = ((System.Drawing.Image)(resources.GetObject("btn_edit.Image")));
            this.btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(47, 22);
            this.btn_edit.Text = "Edit";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_new
            // 
            this.btn_new.Image = ((System.Drawing.Image)(resources.GetObject("btn_new.Image")));
            this.btn_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(51, 22);
            this.btn_new.Text = "New";
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_save,
            this.btn_cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 63);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 21;
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(51, 22);
            this.btn_save.Text = "Save";
            this.btn_save.Visible = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Payment Terms Setup";
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.label1);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(634, 63);
            this.panel_header.TabIndex = 20;
            this.panel_header.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_header_Paint);
            // 
            // dg_payment_terms
            // 
            this.dg_payment_terms.AllowUserToAddRows = false;
            this.dg_payment_terms.AllowUserToDeleteRows = false;
            this.dg_payment_terms.AllowUserToResizeColumns = false;
            this.dg_payment_terms.AllowUserToResizeRows = false;
            this.dg_payment_terms.AutoGenerateColumns = false;
            this.dg_payment_terms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_payment_terms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.is_selected,
            this.id,
            this.code,
            this.name});
            this.dg_payment_terms.DataSource = this.dataBinding_payment_terms;
            this.dg_payment_terms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_payment_terms.Location = new System.Drawing.Point(0, 174);
            this.dg_payment_terms.MultiSelect = false;
            this.dg_payment_terms.Name = "dg_payment_terms";
            this.dg_payment_terms.RowHeadersVisible = false;
            this.dg_payment_terms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dg_payment_terms.Size = new System.Drawing.Size(634, 473);
            this.dg_payment_terms.TabIndex = 23;
            this.dg_payment_terms.CellBorderStyleChanged += new System.EventHandler(this.dg_payment_terms_CellBorderStyleChanged);
            this.dg_payment_terms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_payment_terms_CellClick);
            this.dg_payment_terms.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_payment_terms_CellContentClick);
            this.dg_payment_terms.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_payment_terms_CellValueChanged);
            // 
            // dataBinding_payment_terms
            // 
            this.dataBinding_payment_terms.DataMember = "Tbl_PaymentTerms";
            this.dataBinding_payment_terms.DataSource = this.ds_payment_terms;
            // 
            // ds_payment_terms
            // 
            this.ds_payment_terms.DataSetName = "NewDataSet";
            this.ds_payment_terms.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4});
            this.dataTable1.TableName = "Tbl_PaymentTerms";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "";
            this.dataColumn1.ColumnName = "id";
            this.dataColumn1.DataType = typeof(short);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "code";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "name";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "is_selected";
            this.dataColumn4.DataType = typeof(bool);
            // 
            // is_selected
            // 
            this.is_selected.DataPropertyName = "is_selected";
            this.is_selected.HeaderText = "";
            this.is_selected.Name = "is_selected";
            this.is_selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_selected.Width = 75;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "CODE";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "NAME";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // frm_payment_terms_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dg_payment_terms);
            this.Controls.Add(this.panel_records);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_payment_terms_setup";
            this.Size = new System.Drawing.Size(634, 647);
            this.Load += new System.EventHandler(this.frm_payment_terms_setup_Load);
            this.panel_records.ResumeLayout(false);
            this.panel_records.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_payment_terms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBinding_payment_terms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_payment_terms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel_records;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.DataGridView dg_payment_terms;
        private System.Windows.Forms.BindingSource dataBinding_payment_terms;
        private System.Data.DataSet ds_payment_terms;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
    }
}
