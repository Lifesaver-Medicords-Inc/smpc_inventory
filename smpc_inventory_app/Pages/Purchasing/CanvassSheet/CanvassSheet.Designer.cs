
namespace smpc_inventory_app.Pages.Purchasing
{
    partial class CanvassSheet
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.item_code = new System.Windows.Forms.DataGridViewButtonColumn();
            this.item_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.list_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_validity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payment_terms_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lead_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.footer_panel = new System.Windows.Forms.Panel();
            this.btn_generate_report = new System.Windows.Forms.Button();
            this.btn_done = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_req_qty = new System.Windows.Forms.TextBox();
            this.txt_stock = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_doc_no = new System.Windows.Forms.TextBox();
            this.txt_date = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_item_name = new System.Windows.Forms.TextBox();
            this.txt_item_code = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.footer_panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 178);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1154, 380);
            this.panel3.TabIndex = 15;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.item_code,
            this.item_description,
            this.order_size,
            this.req,
            this.list_price,
            this.discount,
            this.price_validity,
            this.order_qty,
            this.total,
            this.payment_terms_id,
            this.lead_time});
            this.dataGridView1.Location = new System.Drawing.Point(46, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1049, 299);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.DataPropertyName = "supplier_name";
            this.item_code.HeaderText = "SUUPLIER NAME";
            this.item_code.Name = "item_code";
            this.item_code.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.item_code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // item_description
            // 
            this.item_description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_description.DataPropertyName = "contact_no";
            this.item_description.HeaderText = "CONTACT NO.";
            this.item_description.Name = "item_description";
            // 
            // order_size
            // 
            this.order_size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.order_size.HeaderText = "ORDER SIZE";
            this.order_size.Name = "order_size";
            // 
            // req
            // 
            this.req.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.req.DataPropertyName = "current_price";
            this.req.HeaderText = "CURRENT PRICE";
            this.req.Name = "req";
            // 
            // list_price
            // 
            this.list_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.list_price.DataPropertyName = "new_unit_price";
            this.list_price.HeaderText = "NEW UNIT PRICE";
            this.list_price.Name = "list_price";
            // 
            // discount
            // 
            this.discount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.discount.HeaderText = "DISCOUNT";
            this.discount.Name = "discount";
            // 
            // price_validity
            // 
            this.price_validity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.price_validity.HeaderText = "PRICE VALIDITY";
            this.price_validity.Name = "price_validity";
            // 
            // order_qty
            // 
            this.order_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.order_qty.HeaderText = "ORDER QTY";
            this.order_qty.Name = "order_qty";
            // 
            // total
            // 
            this.total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.total.HeaderText = "TOTAL";
            this.total.Name = "total";
            // 
            // payment_terms_id
            // 
            this.payment_terms_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.payment_terms_id.HeaderText = "PAYMENT TERMS";
            this.payment_terms_id.Name = "payment_terms_id";
            // 
            // lead_time
            // 
            this.lead_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lead_time.HeaderText = "LEADTIME";
            this.lead_time.Name = "lead_time";
            // 
            // footer_panel
            // 
            this.footer_panel.Controls.Add(this.btn_generate_report);
            this.footer_panel.Controls.Add(this.btn_done);
            this.footer_panel.Controls.Add(this.btn_cancel);
            this.footer_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer_panel.Location = new System.Drawing.Point(0, 558);
            this.footer_panel.Name = "footer_panel";
            this.footer_panel.Size = new System.Drawing.Size(1154, 89);
            this.footer_panel.TabIndex = 14;
            // 
            // btn_generate_report
            // 
            this.btn_generate_report.Location = new System.Drawing.Point(590, 31);
            this.btn_generate_report.Name = "btn_generate_report";
            this.btn_generate_report.Size = new System.Drawing.Size(130, 26);
            this.btn_generate_report.TabIndex = 16;
            this.btn_generate_report.Text = "GENERATE REPORT";
            this.btn_generate_report.UseVisualStyleBackColor = true;
            // 
            // btn_done
            // 
            this.btn_done.Location = new System.Drawing.Point(919, 31);
            this.btn_done.Name = "btn_done";
            this.btn_done.Size = new System.Drawing.Size(106, 26);
            this.btn_done.TabIndex = 15;
            this.btn_done.Text = "DONE";
            this.btn_done.UseVisualStyleBackColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(807, 31);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(106, 26);
            this.btn_cancel.TabIndex = 14;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txt_req_qty);
            this.panel2.Controls.Add(this.txt_stock);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txt_doc_no);
            this.panel2.Controls.Add(this.txt_date);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txt_item_name);
            this.panel2.Controls.Add(this.txt_item_code);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1154, 115);
            this.panel2.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(509, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 75;
            this.label6.Text = "REQ QTY:";
            // 
            // txt_req_qty
            // 
            this.txt_req_qty.Location = new System.Drawing.Point(590, 20);
            this.txt_req_qty.Name = "txt_req_qty";
            this.txt_req_qty.Size = new System.Drawing.Size(130, 20);
            this.txt_req_qty.TabIndex = 74;
            // 
            // txt_stock
            // 
            this.txt_stock.Location = new System.Drawing.Point(590, 42);
            this.txt_stock.Name = "txt_stock";
            this.txt_stock.Size = new System.Drawing.Size(130, 20);
            this.txt_stock.TabIndex = 73;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(509, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 72;
            this.label7.Text = "STOCK:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(270, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "DOC NO:";
            // 
            // txt_doc_no
            // 
            this.txt_doc_no.Location = new System.Drawing.Point(351, 20);
            this.txt_doc_no.Name = "txt_doc_no";
            this.txt_doc_no.Size = new System.Drawing.Size(130, 20);
            this.txt_doc_no.TabIndex = 70;
            // 
            // txt_date
            // 
            this.txt_date.Location = new System.Drawing.Point(351, 42);
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(130, 20);
            this.txt_date.TabIndex = 69;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(283, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 68;
            this.label5.Text = "DATE:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 67;
            this.label2.Text = "ITEM NAME:";
            // 
            // txt_item_name
            // 
            this.txt_item_name.Location = new System.Drawing.Point(99, 20);
            this.txt_item_name.Name = "txt_item_name";
            this.txt_item_name.Size = new System.Drawing.Size(130, 20);
            this.txt_item_name.TabIndex = 66;
            // 
            // txt_item_code
            // 
            this.txt_item_code.Location = new System.Drawing.Point(99, 42);
            this.txt_item_code.Name = "txt_item_code";
            this.txt_item_code.Size = new System.Drawing.Size(130, 20);
            this.txt_item_code.TabIndex = 65;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 64;
            this.label3.Text = "ITEM CODE:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1154, 63);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Canvass Sheet";
            // 
            // CanvassSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.footer_panel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CanvassSheet";
            this.Size = new System.Drawing.Size(1154, 647);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.footer_panel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewButtonColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_size;
        private System.Windows.Forms.DataGridViewTextBoxColumn req;
        private System.Windows.Forms.DataGridViewTextBoxColumn list_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_validity;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn payment_terms_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn lead_time;
        private System.Windows.Forms.Panel footer_panel;
        private System.Windows.Forms.Button btn_generate_report;
        private System.Windows.Forms.Button btn_done;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_req_qty;
        private System.Windows.Forms.TextBox txt_stock;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_doc_no;
        private System.Windows.Forms.TextBox txt_date;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_item_name;
        private System.Windows.Forms.TextBox txt_item_code;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
