
namespace smpc_inventory_app.Pages.Purchasing
{
    partial class NewPurchasingList
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_sales_order = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_search_so = new System.Windows.Forms.TextBox();
            this.tab_purchase_requisition = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnl_header_pr = new System.Windows.Forms.Panel();
            this.lbl_search = new System.Windows.Forms.Label();
            this.txt_search_pr = new System.Windows.Forms.TextBox();
            this.tab_active = new System.Windows.Forms.TabPage();
            this.pnl_body = new System.Windows.Forms.Panel();
            this.dgv_ative_po = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docnoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suppliernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalamountdueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leadtimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource_active_po = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_po_list = new System.Data.DataSet();
            this.active_po = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.closed_po = new System.Data.DataTable();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.pnl_header_active = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_seach_active = new System.Windows.Forms.TextBox();
            this.tab_closed = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgv_closed_po = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docnoDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suppliernameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalamountdueDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leadtimeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivingreportidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivingreportnoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource_closed_po = new System.Windows.Forms.BindingSource(this.components);
            this.pnl_header_closed = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_seach_closed = new System.Windows.Forms.TextBox();
            this.footer_panel = new System.Windows.Forms.Panel();
            this.btn_create_po = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_sales_order.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tab_purchase_requisition.SuspendLayout();
            this.pnl_header_pr.SuspendLayout();
            this.tab_active.SuspendLayout();
            this.pnl_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ative_po)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_active_po)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_po_list)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.active_po)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closed_po)).BeginInit();
            this.pnl_header_active.SuspendLayout();
            this.tab_closed.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_closed_po)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_closed_po)).BeginInit();
            this.pnl_header_closed.SuspendLayout();
            this.footer_panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1237, 820);
            this.panel3.TabIndex = 11;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_sales_order);
            this.tabControl1.Controls.Add(this.tab_purchase_requisition);
            this.tabControl1.Controls.Add(this.tab_active);
            this.tabControl1.Controls.Add(this.tab_closed);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1237, 820);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tab_sales_order
            // 
            this.tab_sales_order.Controls.Add(this.flowLayoutPanel1);
            this.tab_sales_order.Controls.Add(this.panel2);
            this.tab_sales_order.Location = new System.Drawing.Point(4, 22);
            this.tab_sales_order.Name = "tab_sales_order";
            this.tab_sales_order.Padding = new System.Windows.Forms.Padding(3);
            this.tab_sales_order.Size = new System.Drawing.Size(1229, 794);
            this.tab_sales_order.TabIndex = 0;
            this.tab_sales_order.Text = "SALES ORDER";
            this.tab_sales_order.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 52);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1223, 739);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txt_search_so);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1223, 49);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(1139, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "SEARCH";
            // 
            // txt_search_so
            // 
            this.txt_search_so.Location = new System.Drawing.Point(933, 10);
            this.txt_search_so.Name = "txt_search_so";
            this.txt_search_so.Size = new System.Drawing.Size(200, 20);
            this.txt_search_so.TabIndex = 0;
            this.txt_search_so.TextChanged += new System.EventHandler(this.txt_search_so_TextChanged);
            // 
            // tab_purchase_requisition
            // 
            this.tab_purchase_requisition.Controls.Add(this.flowLayoutPanel2);
            this.tab_purchase_requisition.Controls.Add(this.pnl_header_pr);
            this.tab_purchase_requisition.Location = new System.Drawing.Point(4, 22);
            this.tab_purchase_requisition.Name = "tab_purchase_requisition";
            this.tab_purchase_requisition.Padding = new System.Windows.Forms.Padding(3);
            this.tab_purchase_requisition.Size = new System.Drawing.Size(1229, 794);
            this.tab_purchase_requisition.TabIndex = 1;
            this.tab_purchase_requisition.Text = "PURCHASE REQUISITION";
            this.tab_purchase_requisition.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 52);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1223, 739);
            this.flowLayoutPanel2.TabIndex = 2;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // pnl_header_pr
            // 
            this.pnl_header_pr.Controls.Add(this.lbl_search);
            this.pnl_header_pr.Controls.Add(this.txt_search_pr);
            this.pnl_header_pr.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_header_pr.Location = new System.Drawing.Point(3, 3);
            this.pnl_header_pr.Name = "pnl_header_pr";
            this.pnl_header_pr.Size = new System.Drawing.Size(1223, 49);
            this.pnl_header_pr.TabIndex = 4;
            // 
            // lbl_search
            // 
            this.lbl_search.AutoSize = true;
            this.lbl_search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_search.Location = new System.Drawing.Point(1139, 15);
            this.lbl_search.Name = "lbl_search";
            this.lbl_search.Size = new System.Drawing.Size(53, 15);
            this.lbl_search.TabIndex = 3;
            this.lbl_search.Text = "SEARCH";
            // 
            // txt_search_pr
            // 
            this.txt_search_pr.Location = new System.Drawing.Point(933, 10);
            this.txt_search_pr.Name = "txt_search_pr";
            this.txt_search_pr.Size = new System.Drawing.Size(200, 20);
            this.txt_search_pr.TabIndex = 0;
            this.txt_search_pr.TextChanged += new System.EventHandler(this.txt_search_pr_TextChanged);
            // 
            // tab_active
            // 
            this.tab_active.Controls.Add(this.pnl_body);
            this.tab_active.Controls.Add(this.pnl_header_active);
            this.tab_active.Location = new System.Drawing.Point(4, 22);
            this.tab_active.Name = "tab_active";
            this.tab_active.Padding = new System.Windows.Forms.Padding(3);
            this.tab_active.Size = new System.Drawing.Size(1229, 794);
            this.tab_active.TabIndex = 2;
            this.tab_active.Text = "ACTIVE PO";
            this.tab_active.UseVisualStyleBackColor = true;
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.dgv_ative_po);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(3, 52);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Size = new System.Drawing.Size(1223, 739);
            this.pnl_body.TabIndex = 2;
            // 
            // dgv_ative_po
            // 
            this.dgv_ative_po.AllowUserToAddRows = false;
            this.dgv_ative_po.AutoGenerateColumns = false;
            this.dgv_ative_po.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ative_po.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.docnoDataGridViewTextBoxColumn,
            this.suppliernameDataGridViewTextBoxColumn,
            this.totalamountdueDataGridViewTextBoxColumn,
            this.leadtimeDataGridViewTextBoxColumn});
            this.dgv_ative_po.DataSource = this.bindingSource_active_po;
            this.dgv_ative_po.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_ative_po.Location = new System.Drawing.Point(0, 0);
            this.dgv_ative_po.Name = "dgv_ative_po";
            this.dgv_ative_po.Size = new System.Drawing.Size(1223, 739);
            this.dgv_ative_po.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // docnoDataGridViewTextBoxColumn
            // 
            this.docnoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.docnoDataGridViewTextBoxColumn.DataPropertyName = "doc_no";
            this.docnoDataGridViewTextBoxColumn.HeaderText = "DOC NO";
            this.docnoDataGridViewTextBoxColumn.Name = "docnoDataGridViewTextBoxColumn";
            // 
            // suppliernameDataGridViewTextBoxColumn
            // 
            this.suppliernameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.suppliernameDataGridViewTextBoxColumn.DataPropertyName = "supplier_name";
            this.suppliernameDataGridViewTextBoxColumn.HeaderText = "SUPPLIER NAME";
            this.suppliernameDataGridViewTextBoxColumn.Name = "suppliernameDataGridViewTextBoxColumn";
            // 
            // totalamountdueDataGridViewTextBoxColumn
            // 
            this.totalamountdueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.totalamountdueDataGridViewTextBoxColumn.DataPropertyName = "total_amount_due";
            this.totalamountdueDataGridViewTextBoxColumn.HeaderText = "PRICE";
            this.totalamountdueDataGridViewTextBoxColumn.Name = "totalamountdueDataGridViewTextBoxColumn";
            // 
            // leadtimeDataGridViewTextBoxColumn
            // 
            this.leadtimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.leadtimeDataGridViewTextBoxColumn.DataPropertyName = "lead_time";
            this.leadtimeDataGridViewTextBoxColumn.HeaderText = "EXPECTED ARRIVAL";
            this.leadtimeDataGridViewTextBoxColumn.Name = "leadtimeDataGridViewTextBoxColumn";
            // 
            // bindingSource_active_po
            // 
            this.bindingSource_active_po.DataMember = "active_po";
            this.bindingSource_active_po.DataSource = this.dataSet_po_list;
            // 
            // dataSet_po_list
            // 
            this.dataSet_po_list.DataSetName = "NewDataSet";
            this.dataSet_po_list.Tables.AddRange(new System.Data.DataTable[] {
            this.active_po,
            this.closed_po});
            // 
            // active_po
            // 
            this.active_po.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5});
            this.active_po.TableName = "active_po";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "id";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "supplier_name";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "total_amount_due";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "lead_time";
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "doc_no";
            // 
            // closed_po
            // 
            this.closed_po.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10,
            this.dataColumn11,
            this.dataColumn12});
            this.closed_po.TableName = "closed_po";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "id";
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "doc_no";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "supplier_name";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "total_amount_due";
            // 
            // dataColumn10
            // 
            this.dataColumn10.ColumnName = "lead_time";
            // 
            // dataColumn11
            // 
            this.dataColumn11.ColumnName = "receiving_report_id";
            // 
            // dataColumn12
            // 
            this.dataColumn12.ColumnName = "receiving_report_no";
            // 
            // pnl_header_active
            // 
            this.pnl_header_active.Controls.Add(this.label3);
            this.pnl_header_active.Controls.Add(this.txt_seach_active);
            this.pnl_header_active.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_header_active.Location = new System.Drawing.Point(3, 3);
            this.pnl_header_active.Name = "pnl_header_active";
            this.pnl_header_active.Size = new System.Drawing.Size(1223, 49);
            this.pnl_header_active.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(1139, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "SEARCH";
            // 
            // txt_seach_active
            // 
            this.txt_seach_active.Location = new System.Drawing.Point(933, 10);
            this.txt_seach_active.Name = "txt_seach_active";
            this.txt_seach_active.Size = new System.Drawing.Size(200, 20);
            this.txt_seach_active.TabIndex = 0;
            // 
            // tab_closed
            // 
            this.tab_closed.Controls.Add(this.panel4);
            this.tab_closed.Controls.Add(this.pnl_header_closed);
            this.tab_closed.Location = new System.Drawing.Point(4, 22);
            this.tab_closed.Name = "tab_closed";
            this.tab_closed.Padding = new System.Windows.Forms.Padding(3);
            this.tab_closed.Size = new System.Drawing.Size(1229, 794);
            this.tab_closed.TabIndex = 3;
            this.tab_closed.Text = "CLOSED PO";
            this.tab_closed.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgv_closed_po);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 52);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1223, 739);
            this.panel4.TabIndex = 3;
            // 
            // dgv_closed_po
            // 
            this.dgv_closed_po.AllowUserToAddRows = false;
            this.dgv_closed_po.AutoGenerateColumns = false;
            this.dgv_closed_po.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_closed_po.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.docnoDataGridViewTextBoxColumn1,
            this.suppliernameDataGridViewTextBoxColumn1,
            this.totalamountdueDataGridViewTextBoxColumn1,
            this.leadtimeDataGridViewTextBoxColumn1,
            this.receivingreportidDataGridViewTextBoxColumn,
            this.receivingreportnoDataGridViewTextBoxColumn});
            this.dgv_closed_po.DataSource = this.bindingSource_closed_po;
            this.dgv_closed_po.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_closed_po.Location = new System.Drawing.Point(0, 0);
            this.dgv_closed_po.Name = "dgv_closed_po";
            this.dgv_closed_po.Size = new System.Drawing.Size(1223, 739);
            this.dgv_closed_po.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "id";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // docnoDataGridViewTextBoxColumn1
            // 
            this.docnoDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.docnoDataGridViewTextBoxColumn1.DataPropertyName = "doc_no";
            this.docnoDataGridViewTextBoxColumn1.HeaderText = "DOC NO";
            this.docnoDataGridViewTextBoxColumn1.Name = "docnoDataGridViewTextBoxColumn1";
            // 
            // suppliernameDataGridViewTextBoxColumn1
            // 
            this.suppliernameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.suppliernameDataGridViewTextBoxColumn1.DataPropertyName = "supplier_name";
            this.suppliernameDataGridViewTextBoxColumn1.HeaderText = "SUPPLIER NAME";
            this.suppliernameDataGridViewTextBoxColumn1.Name = "suppliernameDataGridViewTextBoxColumn1";
            // 
            // totalamountdueDataGridViewTextBoxColumn1
            // 
            this.totalamountdueDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.totalamountdueDataGridViewTextBoxColumn1.DataPropertyName = "total_amount_due";
            this.totalamountdueDataGridViewTextBoxColumn1.HeaderText = "RPICE";
            this.totalamountdueDataGridViewTextBoxColumn1.Name = "totalamountdueDataGridViewTextBoxColumn1";
            // 
            // leadtimeDataGridViewTextBoxColumn1
            // 
            this.leadtimeDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.leadtimeDataGridViewTextBoxColumn1.DataPropertyName = "lead_time";
            this.leadtimeDataGridViewTextBoxColumn1.HeaderText = "DATE OF ARRIVAL";
            this.leadtimeDataGridViewTextBoxColumn1.Name = "leadtimeDataGridViewTextBoxColumn1";
            // 
            // receivingreportidDataGridViewTextBoxColumn
            // 
            this.receivingreportidDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.receivingreportidDataGridViewTextBoxColumn.DataPropertyName = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn.HeaderText = "receiving_report_id";
            this.receivingreportidDataGridViewTextBoxColumn.Name = "receivingreportidDataGridViewTextBoxColumn";
            this.receivingreportidDataGridViewTextBoxColumn.Visible = false;
            // 
            // receivingreportnoDataGridViewTextBoxColumn
            // 
            this.receivingreportnoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.receivingreportnoDataGridViewTextBoxColumn.DataPropertyName = "receiving_report_no";
            this.receivingreportnoDataGridViewTextBoxColumn.HeaderText = "RECEIVING REPORT";
            this.receivingreportnoDataGridViewTextBoxColumn.Name = "receivingreportnoDataGridViewTextBoxColumn";
            // 
            // bindingSource_closed_po
            // 
            this.bindingSource_closed_po.DataMember = "closed_po";
            this.bindingSource_closed_po.DataSource = this.dataSet_po_list;
            // 
            // pnl_header_closed
            // 
            this.pnl_header_closed.Controls.Add(this.label2);
            this.pnl_header_closed.Controls.Add(this.txt_seach_closed);
            this.pnl_header_closed.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_header_closed.Location = new System.Drawing.Point(3, 3);
            this.pnl_header_closed.Name = "pnl_header_closed";
            this.pnl_header_closed.Size = new System.Drawing.Size(1223, 49);
            this.pnl_header_closed.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(1139, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "SEARCH";
            // 
            // txt_seach_closed
            // 
            this.txt_seach_closed.Location = new System.Drawing.Point(933, 10);
            this.txt_seach_closed.Name = "txt_seach_closed";
            this.txt_seach_closed.Size = new System.Drawing.Size(200, 20);
            this.txt_seach_closed.TabIndex = 0;
            // 
            // footer_panel
            // 
            this.footer_panel.Controls.Add(this.btn_create_po);
            this.footer_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer_panel.Location = new System.Drawing.Point(0, 883);
            this.footer_panel.Name = "footer_panel";
            this.footer_panel.Size = new System.Drawing.Size(1237, 67);
            this.footer_panel.TabIndex = 10;
            // 
            // btn_create_po
            // 
            this.btn_create_po.Location = new System.Drawing.Point(1018, 23);
            this.btn_create_po.Name = "btn_create_po";
            this.btn_create_po.Size = new System.Drawing.Size(136, 26);
            this.btn_create_po.TabIndex = 15;
            this.btn_create_po.Text = "CREATE PO";
            this.btn_create_po.UseVisualStyleBackColor = true;
            this.btn_create_po.Click += new System.EventHandler(this.btn_create_po_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1237, 63);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Purchasing List";
            // 
            // NewPurchasingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.footer_panel);
            this.Controls.Add(this.panel1);
            this.Name = "NewPurchasingList";
            this.Size = new System.Drawing.Size(1237, 950);
            this.Load += new System.EventHandler(this.NewPurchasingList_Load);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_sales_order.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tab_purchase_requisition.ResumeLayout(false);
            this.pnl_header_pr.ResumeLayout(false);
            this.pnl_header_pr.PerformLayout();
            this.tab_active.ResumeLayout(false);
            this.pnl_body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ative_po)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_active_po)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_po_list)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.active_po)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closed_po)).EndInit();
            this.pnl_header_active.ResumeLayout(false);
            this.pnl_header_active.PerformLayout();
            this.tab_closed.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_closed_po)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_closed_po)).EndInit();
            this.pnl_header_closed.ResumeLayout(false);
            this.pnl_header_closed.PerformLayout();
            this.footer_panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel footer_panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_sales_order;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabPage tab_purchase_requisition;
        private System.Windows.Forms.TabPage tab_active;
        private System.Windows.Forms.TabPage tab_closed;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btn_create_po;
        private System.Windows.Forms.DataGridView dgv_ative_po;
        private System.Windows.Forms.BindingSource bindingSource_active_po;
        private System.Windows.Forms.Panel pnl_body;
        private System.Data.DataSet dataSet_po_list;
        private System.Data.DataTable active_po;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgv_closed_po;
        private System.Windows.Forms.Panel pnl_header_closed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_seach_closed;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn docnoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn suppliernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalamountdueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadtimeDataGridViewTextBoxColumn;
        private System.Data.DataTable closed_po;
        private System.Windows.Forms.BindingSource bindingSource_closed_po;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Data.DataColumn dataColumn9;
        private System.Data.DataColumn dataColumn10;
        private System.Data.DataColumn dataColumn11;
        private System.Data.DataColumn dataColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn docnoDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn suppliernameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalamountdueDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadtimeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivingreportnoDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel pnl_header_active;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_seach_active;
        private System.Windows.Forms.Panel pnl_header_pr;
        private System.Windows.Forms.Label lbl_search;
        private System.Windows.Forms.TextBox txt_search_pr;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_search_so;
    }
}
