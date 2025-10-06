
namespace smpc_inventory_app.Pages.Engineering.Boq
{
    partial class ItemSetSearch
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
            this.listBoxItemSets = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxItemSets
            // 
            this.listBoxItemSets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxItemSets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.listBoxItemSets.FormattingEnabled = true;
            this.listBoxItemSets.ItemHeight = 20;
            this.listBoxItemSets.Location = new System.Drawing.Point(25, 21);
            this.listBoxItemSets.Name = "listBoxItemSets";
            this.listBoxItemSets.Size = new System.Drawing.Size(333, 262);
            this.listBoxItemSets.TabIndex = 1;
            this.listBoxItemSets.SelectedIndexChanged += new System.EventHandler(this.listBoxItemSets_SelectedIndexChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.LightCyan;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelect.Location = new System.Drawing.Point(25, 291);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(333, 26);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // ItemSetSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 341);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.listBoxItemSets);
            this.Name = "ItemSetSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemSetSearch";
            this.Load += new System.EventHandler(this.ItemSetSearch_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxItemSets;
        private System.Windows.Forms.Button btnSelect;
    }
}