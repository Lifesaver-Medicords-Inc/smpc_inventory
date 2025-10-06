
namespace smpc_inventory_app.Pages
{
    partial class SetupModal
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
            this.dg_setup = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_setup_title = new System.Windows.Forms.Label();
            this.panel_button = new System.Windows.Forms.Panel();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_edit = new System.Windows.Forms.Button();
            this.btn_new = new System.Windows.Forms.Button();
            this.panel_records = new System.Windows.Forms.Panel();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.lbl_code = new System.Windows.Forms.Label();
            this.panel_dg = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dg_setup)).BeginInit();
            this.panel_header.SuspendLayout();
            this.panel_button.SuspendLayout();
            this.panel_records.SuspendLayout();
            this.panel_dg.SuspendLayout();
            this.SuspendLayout();
            // 
            // dg_setup
            // 
            this.dg_setup.AllowUserToAddRows = false;
            this.dg_setup.AllowUserToDeleteRows = false;
            this.dg_setup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_setup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name});
            this.dg_setup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_setup.Location = new System.Drawing.Point(0, 0);
            this.dg_setup.Name = "dg_setup";
            this.dg_setup.ReadOnly = true;
            this.dg_setup.RowHeadersVisible = false;
            this.dg_setup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_setup.Size = new System.Drawing.Size(400, 459);
            this.dg_setup.TabIndex = 0;
            this.dg_setup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_setup_CellClick);
            this.dg_setup.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_setup_CellContentClick);
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
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_setup_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(400, 47);
            this.panel_header.TabIndex = 4;
            this.panel_header.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_header_Paint);
            // 
            // lbl_setup_title
            // 
            this.lbl_setup_title.AutoSize = true;
            this.lbl_setup_title.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_setup_title.Location = new System.Drawing.Point(12, 12);
            this.lbl_setup_title.Name = "lbl_setup_title";
            this.lbl_setup_title.Size = new System.Drawing.Size(55, 23);
            this.lbl_setup_title.TabIndex = 1;
            this.lbl_setup_title.Text = "Label";
            // 
            // panel_button
            // 
            this.panel_button.Controls.Add(this.btn_cancel);
            this.panel_button.Controls.Add(this.btn_save);
            this.panel_button.Controls.Add(this.btn_edit);
            this.panel_button.Controls.Add(this.btn_new);
            this.panel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_button.Location = new System.Drawing.Point(0, 398);
            this.panel_button.Name = "panel_button";
            this.panel_button.Size = new System.Drawing.Size(400, 37);
            this.panel_button.TabIndex = 5;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(82, 8);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(73, 22);
            this.btn_cancel.TabIndex = 115;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(11, 8);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(73, 22);
            this.btn_save.TabIndex = 114;
            this.btn_save.Text = "SAVE";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Visible = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Location = new System.Drawing.Point(79, 8);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(73, 22);
            this.btn_edit.TabIndex = 113;
            this.btn_edit.Text = "EDIT";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_new
            // 
            this.btn_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_new.BackColor = System.Drawing.Color.LimeGreen;
            this.btn_new.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_new.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_new.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_new.ForeColor = System.Drawing.Color.Black;
            this.btn_new.Location = new System.Drawing.Point(11, 8);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(73, 22);
            this.btn_new.TabIndex = 112;
            this.btn_new.Text = "NEW";
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel_records
            // 
            this.panel_records.Controls.Add(this.txt_id);
            this.panel_records.Controls.Add(this.lbl_id);
            this.panel_records.Controls.Add(this.txt_name);
            this.panel_records.Controls.Add(this.txt_code);
            this.panel_records.Controls.Add(this.label27);
            this.panel_records.Controls.Add(this.lbl_code);
            this.panel_records.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_records.Enabled = false;
            this.panel_records.Location = new System.Drawing.Point(0, 435);
            this.panel_records.Name = "panel_records";
            this.panel_records.Size = new System.Drawing.Size(400, 71);
            this.panel_records.TabIndex = 6;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(293, 17);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(38, 20);
            this.txt_id.TabIndex = 97;
            this.txt_id.Visible = false;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(271, 21);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(16, 13);
            this.lbl_id.TabIndex = 96;
            this.lbl_id.Text = "Id";
            this.lbl_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(55, 35);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 95;
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(55, 14);
            this.txt_code.Name = "txt_code";
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 94;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(14, 38);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(35, 13);
            this.label27.TabIndex = 93;
            this.label27.Text = "Name";
            // 
            // lbl_code
            // 
            this.lbl_code.AutoSize = true;
            this.lbl_code.Location = new System.Drawing.Point(17, 17);
            this.lbl_code.Name = "lbl_code";
            this.lbl_code.Size = new System.Drawing.Size(32, 13);
            this.lbl_code.TabIndex = 92;
            this.lbl_code.Text = "Code";
            // 
            // panel_dg
            // 
            this.panel_dg.Controls.Add(this.dg_setup);
            this.panel_dg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_dg.Location = new System.Drawing.Point(0, 47);
            this.panel_dg.Name = "panel_dg";
            this.panel_dg.Size = new System.Drawing.Size(400, 459);
            this.panel_dg.TabIndex = 7;
            // 
            // SetupModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 506);
            this.Controls.Add(this.panel_button);
            this.Controls.Add(this.panel_records);
            this.Controls.Add(this.panel_dg);
            this.Controls.Add(this.panel_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetupModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.SetupModal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_setup)).EndInit();
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.panel_button.ResumeLayout(false);
            this.panel_records.ResumeLayout(false);
            this.panel_records.PerformLayout();
            this.panel_dg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_setup;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_setup_title;
        private System.Windows.Forms.Panel panel_button;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Panel panel_records;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lbl_code;
        private System.Windows.Forms.Panel panel_dg;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_save;
    }
}