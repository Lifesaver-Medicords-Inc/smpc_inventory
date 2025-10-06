
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_entity_type
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_entity_type));
            this.lbl_id = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dg_entity_type = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_grid = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.lbl_industries = new System.Windows.Forms.Label();
            this.panel_records = new System.Windows.Forms.Panel();
            this.panel_header = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dg_entity_type)).BeginInit();
            this.panel_grid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel_records.SuspendLayout();
            this.panel_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(331, 30);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(16, 13);
            this.lbl_id.TabIndex = 18;
            this.lbl_id.Text = "Id";
            this.lbl_id.Visible = false;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(353, 27);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 17;
            this.txt_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(75, 45);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 16;
            this.txt_name.Tag = "";
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(75, 24);
            this.txt_code.Name = "txt_code";
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 15;
            this.txt_code.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Name";
            // 
            // dg_entity_type
            // 
            this.dg_entity_type.AllowUserToAddRows = false;
            this.dg_entity_type.AllowUserToDeleteRows = false;
            this.dg_entity_type.AllowUserToResizeColumns = false;
            this.dg_entity_type.AllowUserToResizeRows = false;
            this.dg_entity_type.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_entity_type.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name});
            this.dg_entity_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_entity_type.Location = new System.Drawing.Point(0, 0);
            this.dg_entity_type.Name = "dg_entity_type";
            this.dg_entity_type.ReadOnly = true;
            this.dg_entity_type.RowHeadersVisible = false;
            this.dg_entity_type.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_entity_type.Size = new System.Drawing.Size(634, 473);
            this.dg_entity_type.TabIndex = 0;
            this.dg_entity_type.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_entity_type_CellClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Code";
            // 
            // panel_grid
            // 
            this.panel_grid.Controls.Add(this.dg_entity_type);
            this.panel_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_grid.Location = new System.Drawing.Point(0, 174);
            this.panel_grid.Name = "panel_grid";
            this.panel_grid.Size = new System.Drawing.Size(634, 473);
            this.panel_grid.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 25);
            this.panel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_save,
            this.btn_cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 23;
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
            // btn_edit
            // 
            this.btn_edit.Image = ((System.Drawing.Image)(resources.GetObject("btn_edit.Image")));
            this.btn_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(47, 22);
            this.btn_edit.Text = "Edit";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
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
            // lbl_industries
            // 
            this.lbl_industries.AutoSize = true;
            this.lbl_industries.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_industries.Location = new System.Drawing.Point(15, 24);
            this.lbl_industries.Name = "lbl_industries";
            this.lbl_industries.Size = new System.Drawing.Size(157, 24);
            this.lbl_industries.TabIndex = 3;
            this.lbl_industries.Text = "Entity Type Setup";
            // 
            // panel_records
            // 
            this.panel_records.Controls.Add(this.lbl_id);
            this.panel_records.Controls.Add(this.txt_id);
            this.panel_records.Controls.Add(this.txt_name);
            this.panel_records.Controls.Add(this.txt_code);
            this.panel_records.Controls.Add(this.label4);
            this.panel_records.Controls.Add(this.label5);
            this.panel_records.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_records.Enabled = false;
            this.panel_records.Location = new System.Drawing.Point(0, 88);
            this.panel_records.Name = "panel_records";
            this.panel_records.Size = new System.Drawing.Size(634, 86);
            this.panel_records.TabIndex = 6;
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_industries);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(634, 63);
            this.panel_header.TabIndex = 4;
            // 
            // frm_entity_type
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_grid);
            this.Controls.Add(this.panel_records);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_entity_type";
            this.Size = new System.Drawing.Size(634, 647);
            this.Load += new System.EventHandler(this.frm_entity_type_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_entity_type)).EndInit();
            this.panel_grid.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_records.ResumeLayout(false);
            this.panel_records.PerformLayout();
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dg_entity_type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_industries;
        private System.Windows.Forms.Panel panel_records;
        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
    }
}
