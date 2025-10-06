
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_social_media_setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_social_media_setup));
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_records = new System.Windows.Forms.Panel();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_header = new System.Windows.Forms.Panel();
            this.pnl_page_title = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_social_media = new System.Windows.Forms.DataGridView();
            this.panel_records.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel_header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_social_media)).BeginInit();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "NAME";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(351, 27);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 12;
            this.txt_id.Visible = false;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(332, 30);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(16, 13);
            this.lbl_id.TabIndex = 11;
            this.lbl_id.Text = "Id";
            this.lbl_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(73, 45);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 10;
            this.txt_name.Tag = "";
            this.txt_name.TextChanged += new System.EventHandler(this.txt_name_TextChanged);
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(73, 24);
            this.txt_code.Name = "txt_code";
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 9;
            this.txt_code.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Code";
            // 
            // panel_records
            // 
            this.panel_records.Controls.Add(this.txt_id);
            this.panel_records.Controls.Add(this.lbl_id);
            this.panel_records.Controls.Add(this.txt_name);
            this.panel_records.Controls.Add(this.txt_code);
            this.panel_records.Controls.Add(this.label4);
            this.panel_records.Controls.Add(this.label5);
            this.panel_records.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_records.Enabled = false;
            this.panel_records.Location = new System.Drawing.Point(0, 88);
            this.panel_records.Name = "panel_records";
            this.panel_records.Size = new System.Drawing.Size(634, 86);
            this.panel_records.TabIndex = 33;
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
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_save,
            this.btn_cancel});
            this.toolStrip2.Location = new System.Drawing.Point(0, 63);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(634, 25);
            this.toolStrip2.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 24);
            this.label6.TabIndex = 2;
            this.label6.Text = "Social Media Setup";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.label6);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(634, 63);
            this.panel_header.TabIndex = 31;
            // 
            // pnl_page_title
            // 
            this.pnl_page_title.AutoScroll = true;
            this.pnl_page_title.AutoSize = true;
            this.pnl_page_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_page_title.Location = new System.Drawing.Point(0, 0);
            this.pnl_page_title.Name = "pnl_page_title";
            this.pnl_page_title.Size = new System.Drawing.Size(634, 0);
            this.pnl_page_title.TabIndex = 28;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 0);
            this.panel1.TabIndex = 29;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.AutoSize = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(634, 0);
            this.panel2.TabIndex = 30;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "CODE";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // dg_social_media
            // 
            this.dg_social_media.AllowUserToAddRows = false;
            this.dg_social_media.AllowUserToDeleteRows = false;
            this.dg_social_media.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_social_media.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name});
            this.dg_social_media.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_social_media.Location = new System.Drawing.Point(0, 174);
            this.dg_social_media.Name = "dg_social_media";
            this.dg_social_media.ReadOnly = true;
            this.dg_social_media.RowHeadersVisible = false;
            this.dg_social_media.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_social_media.Size = new System.Drawing.Size(634, 473);
            this.dg_social_media.TabIndex = 34;
            this.dg_social_media.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_social_media_CellClick);
            // 
            // frm_social_media_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dg_social_media);
            this.Controls.Add(this.panel_records);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.panel_header);
            this.Controls.Add(this.pnl_page_title);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frm_social_media_setup";
            this.Size = new System.Drawing.Size(634, 647);
            this.Load += new System.EventHandler(this.frm_social_media_setup_Load);
            this.panel_records.ResumeLayout(false);
            this.panel_records.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_social_media)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_records;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Panel pnl_page_title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridView dg_social_media;
    }
}
