
namespace smpc_inventory_app.Pages.Purchasing
{
    partial class PurchaseItemCard
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_order_qty_uom = new System.Windows.Forms.TextBox();
            this.txt_req_qty_uom = new System.Windows.Forms.TextBox();
            this.btn_view_details = new System.Windows.Forms.Button();
            this.txt_order_qty = new System.Windows.Forms.TextBox();
            this.txt_req_qty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txt_stock = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_date = new System.Windows.Forms.DateTimePicker();
            this.btn_canvass = new System.Windows.Forms.Button();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_item_description = new System.Windows.Forms.TextBox();
            this.txt_brand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnl_canvass = new System.Windows.Forms.Panel();
            this.dgv_canvass = new System.Windows.Forms.DataGridView();
            this.bindingSourcePaymentTerms = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetPaymentTerms = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_add_supplier = new System.Windows.Forms.Button();
            this.bindingSourceSupplier = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetSupplier = new System.Data.DataSet();
            this.dataTable2 = new System.Data.DataTable();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.dataColumn13 = new System.Data.DataColumn();
            this.dataColumn14 = new System.Data.DataColumn();
            this.dataColumn15 = new System.Data.DataColumn();
            this.dataColumn16 = new System.Data.DataColumn();
            this.dataColumn17 = new System.Data.DataColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier = new System.Windows.Forms.DataGridViewButtonColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.current_list_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.new_list_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.net_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_validity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payment_terms = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lead_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.pnl_canvass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_canvass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePaymentTerms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPaymentTerms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetSupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_id);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_order_qty_uom);
            this.panel1.Controls.Add(this.txt_req_qty_uom);
            this.panel1.Controls.Add(this.btn_view_details);
            this.panel1.Controls.Add(this.txt_order_qty);
            this.panel1.Controls.Add(this.txt_req_qty);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.txt_stock);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtp_date);
            this.panel1.Controls.Add(this.btn_canvass);
            this.panel1.Controls.Add(this.label37);
            this.panel1.Controls.Add(this.txt_item_description);
            this.panel1.Controls.Add(this.txt_brand);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1120, 130);
            this.panel1.TabIndex = 0;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(492, 92);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(85, 20);
            this.txt_id.TabIndex = 83;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(465, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 82;
            this.label6.Text = "ID:";
            // 
            // txt_order_qty_uom
            // 
            this.txt_order_qty_uom.Location = new System.Drawing.Point(583, 53);
            this.txt_order_qty_uom.Name = "txt_order_qty_uom";
            this.txt_order_qty_uom.Size = new System.Drawing.Size(50, 20);
            this.txt_order_qty_uom.TabIndex = 81;
            // 
            // txt_req_qty_uom
            // 
            this.txt_req_qty_uom.Location = new System.Drawing.Point(583, 32);
            this.txt_req_qty_uom.Name = "txt_req_qty_uom";
            this.txt_req_qty_uom.Size = new System.Drawing.Size(50, 20);
            this.txt_req_qty_uom.TabIndex = 80;
            // 
            // btn_view_details
            // 
            this.btn_view_details.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_view_details.Location = new System.Drawing.Point(639, 35);
            this.btn_view_details.Name = "btn_view_details";
            this.btn_view_details.Size = new System.Drawing.Size(69, 17);
            this.btn_view_details.TabIndex = 79;
            this.btn_view_details.Text = "VIEW DETAILS";
            this.btn_view_details.UseVisualStyleBackColor = true;
            this.btn_view_details.Click += new System.EventHandler(this.btn_view_details_Click);
            // 
            // txt_order_qty
            // 
            this.txt_order_qty.Location = new System.Drawing.Point(492, 53);
            this.txt_order_qty.Name = "txt_order_qty";
            this.txt_order_qty.Size = new System.Drawing.Size(90, 20);
            this.txt_order_qty.TabIndex = 78;
            // 
            // txt_req_qty
            // 
            this.txt_req_qty.Location = new System.Drawing.Point(492, 32);
            this.txt_req_qty.Name = "txt_req_qty";
            this.txt_req_qty.Size = new System.Drawing.Size(90, 20);
            this.txt_req_qty.TabIndex = 77;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(412, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 76;
            this.label4.Text = "ORDER QTY:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(428, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 75;
            this.label5.Text = "REQ QTY:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(4, 32);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 74;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txt_stock
            // 
            this.txt_stock.Location = new System.Drawing.Point(492, 10);
            this.txt_stock.Name = "txt_stock";
            this.txt_stock.Size = new System.Drawing.Size(141, 20);
            this.txt_stock.TabIndex = 73;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(440, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 72;
            this.label2.Text = "STOCK:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 71;
            this.label1.Text = "DATE:";
            // 
            // dtp_date
            // 
            this.dtp_date.Location = new System.Drawing.Point(165, 53);
            this.dtp_date.Name = "dtp_date";
            this.dtp_date.Size = new System.Drawing.Size(200, 20);
            this.dtp_date.TabIndex = 70;
            // 
            // btn_canvass
            // 
            this.btn_canvass.Location = new System.Drawing.Point(42, 93);
            this.btn_canvass.Name = "btn_canvass";
            this.btn_canvass.Size = new System.Drawing.Size(75, 23);
            this.btn_canvass.TabIndex = 69;
            this.btn_canvass.Text = "CANVASS";
            this.btn_canvass.UseVisualStyleBackColor = true;
            this.btn_canvass.Click += new System.EventHandler(this.btn_canvass_Click);
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
            // pnl_canvass
            // 
            this.pnl_canvass.Controls.Add(this.dgv_canvass);
            this.pnl_canvass.Controls.Add(this.panel2);
            this.pnl_canvass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_canvass.Location = new System.Drawing.Point(0, 130);
            this.pnl_canvass.Name = "pnl_canvass";
            this.pnl_canvass.Size = new System.Drawing.Size(1120, 393);
            this.pnl_canvass.TabIndex = 1;
            // 
            // dgv_canvass
            // 
            this.dgv_canvass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_canvass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.supplier_id,
            this.supplier,
            this.supplier_name,
            this.contact_no,
            this.order_size,
            this.supplier_stock,
            this.current_list_price,
            this.new_list_price,
            this.discount,
            this.net_price,
            this.price_validity,
            this.payment_terms,
            this.lead_time});
            this.dgv_canvass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_canvass.Location = new System.Drawing.Point(0, 0);
            this.dgv_canvass.Name = "dgv_canvass";
            this.dgv_canvass.Size = new System.Drawing.Size(1120, 353);
            this.dgv_canvass.TabIndex = 2;
            this.dgv_canvass.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_canvass_CellClick);
            this.dgv_canvass.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_canvass_CellEndEdit);
            // 
            // bindingSourcePaymentTerms
            // 
            this.bindingSourcePaymentTerms.DataMember = "Table1";
            this.bindingSourcePaymentTerms.DataSource = this.dataSetPaymentTerms;
            // 
            // dataSetPaymentTerms
            // 
            this.dataSetPaymentTerms.DataSetName = "NewDataSet";
            this.dataSetPaymentTerms.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3});
            this.dataTable1.TableName = "Table1";
            // 
            // dataColumn1
            // 
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
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_add_supplier);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 353);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1120, 40);
            this.panel2.TabIndex = 3;
            // 
            // btn_add_supplier
            // 
            this.btn_add_supplier.Location = new System.Drawing.Point(34, 8);
            this.btn_add_supplier.Name = "btn_add_supplier";
            this.btn_add_supplier.Size = new System.Drawing.Size(117, 23);
            this.btn_add_supplier.TabIndex = 2;
            this.btn_add_supplier.Text = "ADD SUPPLIER";
            this.btn_add_supplier.UseVisualStyleBackColor = true;
            this.btn_add_supplier.Click += new System.EventHandler(this.btn_add_supplier_Click);
            // 
            // bindingSourceSupplier
            // 
            this.bindingSourceSupplier.DataMember = "Table1";
            this.bindingSourceSupplier.DataSource = this.dataSetSupplier;
            // 
            // dataSetSupplier
            // 
            this.dataSetSupplier.DataSetName = "NewDataSet";
            this.dataSetSupplier.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable2});
            // 
            // dataTable2
            // 
            this.dataTable2.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10,
            this.dataColumn11,
            this.dataColumn12,
            this.dataColumn13,
            this.dataColumn14,
            this.dataColumn15,
            this.dataColumn16,
            this.dataColumn17});
            this.dataTable2.TableName = "Table1";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "id";
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "supplier_id";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "supplier";
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "contact_no";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "order_size";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "supplier_stock";
            // 
            // dataColumn10
            // 
            this.dataColumn10.ColumnName = "current_list_price";
            // 
            // dataColumn11
            // 
            this.dataColumn11.ColumnName = "new_list_price";
            // 
            // dataColumn12
            // 
            this.dataColumn12.ColumnName = "discount";
            // 
            // dataColumn13
            // 
            this.dataColumn13.ColumnName = "net_price";
            // 
            // dataColumn14
            // 
            this.dataColumn14.ColumnName = "price_validity";
            // 
            // dataColumn15
            // 
            this.dataColumn15.ColumnName = "payment_terms";
            // 
            // dataColumn16
            // 
            this.dataColumn16.ColumnName = "lead_time";
            // 
            // dataColumn17
            // 
            this.dataColumn17.ColumnName = "item_id";
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Width = 50;
            // 
            // supplier_id
            // 
            this.supplier_id.DataPropertyName = "supplier_id";
            this.supplier_id.HeaderText = "SUPPLIER ID";
            this.supplier_id.Name = "supplier_id";
            // 
            // supplier
            // 
            this.supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier.DataPropertyName = "bpi_id";
            this.supplier.HeaderText = "SUPPLIER";
            this.supplier.Name = "supplier";
            this.supplier.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.supplier.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // supplier_name
            // 
            this.supplier_name.HeaderText = "SUPPLIER NAME";
            this.supplier_name.Name = "supplier_name";
            // 
            // contact_no
            // 
            this.contact_no.DataPropertyName = "contact_no";
            this.contact_no.HeaderText = "CONTACT NO";
            this.contact_no.Name = "contact_no";
            // 
            // order_size
            // 
            this.order_size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.order_size.DataPropertyName = "order_size";
            this.order_size.HeaderText = "ORDER SIZE";
            this.order_size.Name = "order_size";
            // 
            // supplier_stock
            // 
            this.supplier_stock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier_stock.DataPropertyName = "supplier_stock";
            this.supplier_stock.HeaderText = "SUPPLIER STOCK";
            this.supplier_stock.Name = "supplier_stock";
            // 
            // current_list_price
            // 
            this.current_list_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.current_list_price.DataPropertyName = "current_list_price";
            this.current_list_price.HeaderText = "CURRENT LIST PRICE";
            this.current_list_price.Name = "current_list_price";
            // 
            // new_list_price
            // 
            this.new_list_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.new_list_price.DataPropertyName = "new_list_price";
            this.new_list_price.HeaderText = "NEW LIST PRICE";
            this.new_list_price.Name = "new_list_price";
            // 
            // discount
            // 
            this.discount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.discount.DataPropertyName = "discount";
            this.discount.HeaderText = "DISCOUNT";
            this.discount.Name = "discount";
            // 
            // net_price
            // 
            this.net_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.net_price.DataPropertyName = "net_price";
            this.net_price.HeaderText = "NET PRICE";
            this.net_price.Name = "net_price";
            // 
            // price_validity
            // 
            this.price_validity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.price_validity.DataPropertyName = "price_validity";
            this.price_validity.HeaderText = "PRICE VALIDITY";
            this.price_validity.Name = "price_validity";
            // 
            // payment_terms
            // 
            this.payment_terms.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.payment_terms.DataPropertyName = "payment_terms";
            this.payment_terms.DataSource = this.bindingSourcePaymentTerms;
            this.payment_terms.DisplayMember = "name";
            this.payment_terms.HeaderText = "PAYMENT TERMS";
            this.payment_terms.Name = "payment_terms";
            this.payment_terms.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.payment_terms.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.payment_terms.ValueMember = "id";
            // 
            // lead_time
            // 
            this.lead_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lead_time.DataPropertyName = "lead_time";
            this.lead_time.HeaderText = "LEAD TIME";
            this.lead_time.Name = "lead_time";
            // 
            // PurchaseItemCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnl_canvass);
            this.Controls.Add(this.panel1);
            this.Name = "PurchaseItemCard";
            this.Size = new System.Drawing.Size(1120, 523);
            this.Load += new System.EventHandler(this.PurchaseItemCard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnl_canvass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_canvass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePaymentTerms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPaymentTerms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetSupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_canvass;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txt_item_description;
        private System.Windows.Forms.TextBox txt_brand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnl_canvass;
        private System.Windows.Forms.DataGridView dgv_canvass;
        private System.Windows.Forms.TextBox txt_req_qty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txt_stock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_date;
        private System.Windows.Forms.TextBox txt_order_qty;
        private System.Windows.Forms.Button btn_view_details;
        private System.Windows.Forms.TextBox txt_order_qty_uom;
        private System.Windows.Forms.TextBox txt_req_qty_uom;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_add_supplier;
        private System.Windows.Forms.BindingSource bindingSourcePaymentTerms;
        private System.Data.DataSet dataSetPaymentTerms;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Windows.Forms.BindingSource bindingSourceSupplier;
        private System.Data.DataSet dataSetSupplier;
        private System.Data.DataTable dataTable2;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Data.DataColumn dataColumn9;
        private System.Data.DataColumn dataColumn10;
        private System.Data.DataColumn dataColumn11;
        private System.Data.DataColumn dataColumn12;
        private System.Data.DataColumn dataColumn13;
        private System.Data.DataColumn dataColumn14;
        private System.Data.DataColumn dataColumn15;
        private System.Data.DataColumn dataColumn16;
        private System.Data.DataColumn dataColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_id;
        private System.Windows.Forms.DataGridViewButtonColumn supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_size;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn current_list_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn new_list_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn net_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_validity;
        private System.Windows.Forms.DataGridViewComboBoxColumn payment_terms;
        private System.Windows.Forms.DataGridViewTextBoxColumn lead_time;
    }
}
