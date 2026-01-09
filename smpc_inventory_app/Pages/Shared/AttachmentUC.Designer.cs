
namespace smpc_inventory_app.Pages.Shared
{
    partial class AttachmentUC
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
            this.pnl_attachments = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.pnl_Receiving = new System.Windows.Forms.Panel();
            this.label43 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.RECEIVING_LV = new System.Windows.Forms.ListView();
            this.panel16 = new System.Windows.Forms.Panel();
            this.TV1_preview = new System.Windows.Forms.Panel();
            this.label45 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.RECEIVING_TV = new System.Windows.Forms.TreeView();
            this.pnl_attachments.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel15.SuspendLayout();
            this.pnl_Receiving.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel16.SuspendLayout();
            this.TV1_preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_attachments
            // 
            this.pnl_attachments.Controls.Add(this.panel8);
            this.pnl_attachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_attachments.Location = new System.Drawing.Point(0, 0);
            this.pnl_attachments.Name = "pnl_attachments";
            this.pnl_attachments.Size = new System.Drawing.Size(945, 545);
            this.pnl_attachments.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel15);
            this.panel8.Controls.Add(this.panel16);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(945, 545);
            this.panel8.TabIndex = 1;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.pnl_Receiving);
            this.panel15.Controls.Add(this.btnUpload);
            this.panel15.Controls.Add(this.RECEIVING_LV);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel15.Location = new System.Drawing.Point(295, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(650, 545);
            this.panel15.TabIndex = 1;
            // 
            // pnl_Receiving
            // 
            this.pnl_Receiving.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Receiving.Controls.Add(this.label43);
            this.pnl_Receiving.Controls.Add(this.pictureBox1);
            this.pnl_Receiving.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Receiving.Location = new System.Drawing.Point(0, 0);
            this.pnl_Receiving.Name = "pnl_Receiving";
            this.pnl_Receiving.Size = new System.Drawing.Size(650, 545);
            this.pnl_Receiving.TabIndex = 29;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(243, 165);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(179, 23);
            this.label43.TabIndex = 1;
            this.label43.Text = "Please select a folder";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::smpc_inventory_app.Properties.Resources.FolderIcon;
            this.pictureBox1.Location = new System.Drawing.Point(263, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnUpload.Location = new System.Drawing.Point(552, 508);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = false;
            // 
            // RECEIVING_LV
            // 
            this.RECEIVING_LV.AllowDrop = true;
            this.RECEIVING_LV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RECEIVING_LV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RECEIVING_LV.FullRowSelect = true;
            this.RECEIVING_LV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.RECEIVING_LV.HideSelection = false;
            this.RECEIVING_LV.HoverSelection = true;
            this.RECEIVING_LV.Location = new System.Drawing.Point(0, 0);
            this.RECEIVING_LV.MultiSelect = false;
            this.RECEIVING_LV.Name = "RECEIVING_LV";
            this.RECEIVING_LV.Size = new System.Drawing.Size(650, 545);
            this.RECEIVING_LV.TabIndex = 2;
            this.RECEIVING_LV.UseCompatibleStateImageBehavior = false;
            this.RECEIVING_LV.View = System.Windows.Forms.View.Details;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.TV1_preview);
            this.panel16.Controls.Add(this.RECEIVING_TV);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(0, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(598, 545);
            this.panel16.TabIndex = 0;
            // 
            // TV1_preview
            // 
            this.TV1_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TV1_preview.Controls.Add(this.label45);
            this.TV1_preview.Controls.Add(this.pictureBox3);
            this.TV1_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TV1_preview.Location = new System.Drawing.Point(0, 0);
            this.TV1_preview.Name = "TV1_preview";
            this.TV1_preview.Size = new System.Drawing.Size(598, 545);
            this.TV1_preview.TabIndex = 26;
            this.TV1_preview.Visible = false;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(169, 165);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(260, 23);
            this.label45.TabIndex = 4;
            this.label45.Text = "Directory will open when saved";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::smpc_inventory_app.Properties.Resources.search;
            this.pictureBox3.Location = new System.Drawing.Point(213, 73);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(151, 79);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // RECEIVING_TV
            // 
            this.RECEIVING_TV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RECEIVING_TV.Location = new System.Drawing.Point(0, 0);
            this.RECEIVING_TV.Name = "RECEIVING_TV";
            this.RECEIVING_TV.Size = new System.Drawing.Size(598, 545);
            this.RECEIVING_TV.TabIndex = 3;
            // 
            // AttachmentUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_attachments);
            this.Name = "AttachmentUC";
            this.Size = new System.Drawing.Size(945, 545);
            this.pnl_attachments.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.pnl_Receiving.ResumeLayout(false);
            this.pnl_Receiving.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel16.ResumeLayout(false);
            this.TV1_preview.ResumeLayout(false);
            this.TV1_preview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_attachments;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel pnl_Receiving;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ListView RECEIVING_LV;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel TV1_preview;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TreeView RECEIVING_TV;
    }
}
