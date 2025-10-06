
namespace smpc_inventory_app.Pages.Purchasing.PurchaseList
{
    partial class PurchaseRequisitionDisbtributionCard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_distribute = new System.Windows.Forms.DataGridView();
            this.order_detail_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.doc_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.project_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commitment_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_req = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_to_give = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_header = new System.Windows.Forms.Panel();
            this.txt_item_id = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_order_qty_uom = new System.Windows.Forms.TextBox();
            this.txt_req_qty_uom = new System.Windows.Forms.TextBox();
            this.txt_order_qty = new System.Windows.Forms.TextBox();
            this.txt_req_qty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_item_description = new System.Windows.Forms.TextBox();
            this.txt_brand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_distribute)).BeginInit();
            this.pnl_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_distribute);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(980, 150);
            this.panel2.TabIndex = 4;
            // 
            // dgv_distribute
            // 
            this.dgv_distribute.AllowUserToAddRows = false;
            this.dgv_distribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_distribute.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.order_detail_id,
            this.doc_no,
            this.project_name,
            this.requestor,
            this.commitment_date,
            this.qty_req,
            this.qty_to_give});
            this.dgv_distribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_distribute.Location = new System.Drawing.Point(0, 0);
            this.dgv_distribute.Name = "dgv_distribute";
            this.dgv_distribute.Size = new System.Drawing.Size(980, 150);
            this.dgv_distribute.TabIndex = 5;
            // 
            // order_detail_id
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.order_detail_id.DefaultCellStyle = dataGridViewCellStyle1;
            this.order_detail_id.HeaderText = "ORDER DETAIL IDS";
            this.order_detail_id.Name = "order_detail_id";
            this.order_detail_id.ReadOnly = true;
            this.order_detail_id.Visible = false;
            // 
            // doc_no
            // 
            this.doc_no.DataPropertyName = "doc_no";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.doc_no.DefaultCellStyle = dataGridViewCellStyle2;
            this.doc_no.HeaderText = "DOC NO";
            this.doc_no.Name = "doc_no";
            this.doc_no.ReadOnly = true;
            // 
            // project_name
            // 
            this.project_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.project_name.DataPropertyName = "project_name";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.project_name.DefaultCellStyle = dataGridViewCellStyle3;
            this.project_name.HeaderText = "PROJECT NAME";
            this.project_name.Name = "project_name";
            this.project_name.ReadOnly = true;
            // 
            // requestor
            // 
            this.requestor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.requestor.DataPropertyName = "requestor";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.requestor.DefaultCellStyle = dataGridViewCellStyle4;
            this.requestor.HeaderText = "FOR";
            this.requestor.Name = "requestor";
            this.requestor.ReadOnly = true;
            // 
            // commitment_date
            // 
            this.commitment_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.commitment_date.DataPropertyName = "commitment_date";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.commitment_date.DefaultCellStyle = dataGridViewCellStyle5;
            this.commitment_date.HeaderText = "COMMITMENT DATE";
            this.commitment_date.Name = "commitment_date";
            this.commitment_date.ReadOnly = true;
            // 
            // qty_req
            // 
            this.qty_req.DataPropertyName = "qty_req";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.qty_req.DefaultCellStyle = dataGridViewCellStyle6;
            this.qty_req.HeaderText = "QTY REQ";
            this.qty_req.Name = "qty_req";
            this.qty_req.ReadOnly = true;
            // 
            // qty_to_give
            // 
            this.qty_to_give.DataPropertyName = "qty_to_give";
            dataGridViewCellStyle7.NullValue = "0";
            this.qty_to_give.DefaultCellStyle = dataGridViewCellStyle7;
            this.qty_to_give.HeaderText = "QTY TO GIVE";
            this.qty_to_give.Name = "qty_to_give";
            // 
            // pnl_header
            // 
            this.pnl_header.Controls.Add(this.txt_item_id);
            this.pnl_header.Controls.Add(this.label6);
            this.pnl_header.Controls.Add(this.txt_order_qty_uom);
            this.pnl_header.Controls.Add(this.txt_req_qty_uom);
            this.pnl_header.Controls.Add(this.txt_order_qty);
            this.pnl_header.Controls.Add(this.txt_req_qty);
            this.pnl_header.Controls.Add(this.label4);
            this.pnl_header.Controls.Add(this.label5);
            this.pnl_header.Controls.Add(this.label37);
            this.pnl_header.Controls.Add(this.txt_item_description);
            this.pnl_header.Controls.Add(this.txt_brand);
            this.pnl_header.Controls.Add(this.label3);
            this.pnl_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_header.Location = new System.Drawing.Point(0, 0);
            this.pnl_header.Name = "pnl_header";
            this.pnl_header.Size = new System.Drawing.Size(980, 100);
            this.pnl_header.TabIndex = 3;
            // 
            // txt_item_id
            // 
            this.txt_item_id.Location = new System.Drawing.Point(165, 74);
            this.txt_item_id.Name = "txt_item_id";
            this.txt_item_id.Size = new System.Drawing.Size(85, 20);
            this.txt_item_id.TabIndex = 85;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 84;
            this.label6.Text = "ITEM ID:";
            // 
            // txt_order_qty_uom
            // 
            this.txt_order_qty_uom.Location = new System.Drawing.Point(876, 35);
            this.txt_order_qty_uom.Name = "txt_order_qty_uom";
            this.txt_order_qty_uom.Size = new System.Drawing.Size(50, 20);
            this.txt_order_qty_uom.TabIndex = 81;
            // 
            // txt_req_qty_uom
            // 
            this.txt_req_qty_uom.Location = new System.Drawing.Point(876, 14);
            this.txt_req_qty_uom.Name = "txt_req_qty_uom";
            this.txt_req_qty_uom.Size = new System.Drawing.Size(50, 20);
            this.txt_req_qty_uom.TabIndex = 80;
            // 
            // txt_order_qty
            // 
            this.txt_order_qty.Location = new System.Drawing.Point(785, 35);
            this.txt_order_qty.Name = "txt_order_qty";
            this.txt_order_qty.Size = new System.Drawing.Size(90, 20);
            this.txt_order_qty.TabIndex = 78;
            // 
            // txt_req_qty
            // 
            this.txt_req_qty.Location = new System.Drawing.Point(785, 14);
            this.txt_req_qty.Name = "txt_req_qty";
            this.txt_req_qty.Size = new System.Drawing.Size(90, 20);
            this.txt_req_qty.TabIndex = 77;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(705, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 76;
            this.label4.Text = "ORDER QTY:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(721, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 75;
            this.label5.Text = "REQ QTY:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(39, 14);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(112, 13);
            this.label37.TabIndex = 68;
            this.label37.Text = "ITEM DESCRIPTION:";
            // 
            // txt_item_description
            // 
            this.txt_item_description.Location = new System.Drawing.Point(165, 11);
            this.txt_item_description.Name = "txt_item_description";
            this.txt_item_description.Size = new System.Drawing.Size(200, 20);
            this.txt_item_description.TabIndex = 67;
            // 
            // txt_brand
            // 
            this.txt_brand.Location = new System.Drawing.Point(165, 32);
            this.txt_brand.Name = "txt_brand";
            this.txt_brand.Size = new System.Drawing.Size(200, 20);
            this.txt_brand.TabIndex = 66;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "BRAND:";
            // 
            // PurchaseRequisitionDisbtributionCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnl_header);
            this.Name = "PurchaseRequisitionDisbtributionCard";
            this.Size = new System.Drawing.Size(980, 250);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_distribute)).EndInit();
            this.pnl_header.ResumeLayout(false);
            this.pnl_header.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_distribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_detail_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn doc_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn project_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestor;
        private System.Windows.Forms.DataGridViewTextBoxColumn commitment_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_req;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_to_give;
        private System.Windows.Forms.Panel pnl_header;
        private System.Windows.Forms.TextBox txt_item_id;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_order_qty_uom;
        private System.Windows.Forms.TextBox txt_req_qty_uom;
        private System.Windows.Forms.TextBox txt_order_qty;
        private System.Windows.Forms.TextBox txt_req_qty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txt_item_description;
        private System.Windows.Forms.TextBox txt_brand;
        private System.Windows.Forms.Label label3;
    }
}
