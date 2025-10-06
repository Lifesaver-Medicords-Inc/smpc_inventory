
namespace smpc_inventory_app.Pages.Setup
{
    partial class frm_warehouse_name_setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_warehouse_name_setup));
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_warehouse = new System.Windows.Forms.Label();
            this.pnl_head = new System.Windows.Forms.Panel();
            this.lblmanager = new System.Windows.Forms.Label();
            this.cmb_warehouse_manager = new System.Windows.Forms.ComboBox();
            this.chk_is_inactive = new System.Windows.Forms.CheckBox();
            this.lbl_id = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblcode = new System.Windows.Forms.Label();
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.use_type = new System.Data.DataColumn();
            this.zone = new System.Data.DataColumn();
            this.area = new System.Data.DataColumn();
            this.rack = new System.Data.DataColumn();
            this.level = new System.Data.DataColumn();
            this.bins = new System.Data.DataColumn();
            this.location_code = new System.Data.DataColumn();
            this.notes = new System.Data.DataColumn();
            this.warehouse_name_id = new System.Data.DataColumn();
            this.id = new System.Data.DataColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_general = new System.Windows.Forms.TabPage();
            this.pnl_address = new System.Windows.Forms.Panel();
            this.AddIdLbl = new System.Windows.Forms.Label();
            this.txt_warehouse_address_id = new System.Windows.Forms.TextBox();
            this.txt_address_id = new System.Windows.Forms.TextBox();
            this.WH_Addlbl = new System.Windows.Forms.Label();
            this.txt_contact_no = new System.Windows.Forms.TextBox();
            this.contactnolbl = new System.Windows.Forms.Label();
            this.txt_contact_person = new System.Windows.Forms.TextBox();
            this.txt_country = new System.Windows.Forms.TextBox();
            this.txt_zip_code = new System.Windows.Forms.TextBox();
            this.txt_city = new System.Windows.Forms.TextBox();
            this.txt_barangay_no = new System.Windows.Forms.TextBox();
            this.txt_street = new System.Windows.Forms.TextBox();
            this.txt_building_no = new System.Windows.Forms.TextBox();
            this.contactpersonlbl = new System.Windows.Forms.Label();
            this.countrylbl = new System.Windows.Forms.Label();
            this.zipcodelbl = new System.Windows.Forms.Label();
            this.citylbl = new System.Windows.Forms.Label();
            this.barangaylbl = new System.Windows.Forms.Label();
            this.streetlbl = new System.Windows.Forms.Label();
            this.buildingnolbl = new System.Windows.Forms.Label();
            this.tab_areas = new System.Windows.Forms.TabPage();
            this.pnl_areas = new System.Windows.Forms.Panel();
            this.dg_areas = new System.Windows.Forms.DataGridView();
            this.dg_use_type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dg_zone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_area = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_rack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_bins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_warehouse_name_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_id_areas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_new = new System.Windows.Forms.ToolStripButton();
            this.btn_edit = new System.Windows.Forms.ToolStripButton();
            this.btn_delete = new System.Windows.Forms.ToolStripButton();
            this.btn_search = new System.Windows.Forms.ToolStripButton();
            this.btn_next = new System.Windows.Forms.ToolStripButton();
            this.btn_prev = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStrip();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.btn_cancel = new System.Windows.Forms.ToolStripButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.cmb_search = new System.Windows.Forms.ComboBox();
            this.tmplbl = new System.Windows.Forms.Label();
            this.deleteMe = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_header.SuspendLayout();
            this.pnl_head.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tab_general.SuspendLayout();
            this.pnl_address.SuspendLayout();
            this.tab_areas.SuspendLayout();
            this.pnl_areas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_areas)).BeginInit();
            this.toolStripButton1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_warehouse);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(634, 63);
            this.panel_header.TabIndex = 9;
            // 
            // lbl_warehouse
            // 
            this.lbl_warehouse.AutoSize = true;
            this.lbl_warehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_warehouse.Location = new System.Drawing.Point(5, 16);
            this.lbl_warehouse.Name = "lbl_warehouse";
            this.lbl_warehouse.Size = new System.Drawing.Size(108, 24);
            this.lbl_warehouse.TabIndex = 3;
            this.lbl_warehouse.Text = "Warehouse";
            // 
            // pnl_head
            // 
            this.pnl_head.Controls.Add(this.lblmanager);
            this.pnl_head.Controls.Add(this.cmb_warehouse_manager);
            this.pnl_head.Controls.Add(this.chk_is_inactive);
            this.pnl_head.Controls.Add(this.lbl_id);
            this.pnl_head.Controls.Add(this.txt_id);
            this.pnl_head.Controls.Add(this.txt_name);
            this.pnl_head.Controls.Add(this.txt_code);
            this.pnl_head.Controls.Add(this.label4);
            this.pnl_head.Controls.Add(this.lblcode);
            this.pnl_head.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_head.Enabled = false;
            this.pnl_head.Location = new System.Drawing.Point(0, 88);
            this.pnl_head.Name = "pnl_head";
            this.pnl_head.Size = new System.Drawing.Size(634, 63);
            this.pnl_head.TabIndex = 26;
            // 
            // lblmanager
            // 
            this.lblmanager.AutoSize = true;
            this.lblmanager.Location = new System.Drawing.Point(38, 34);
            this.lblmanager.Name = "lblmanager";
            this.lblmanager.Size = new System.Drawing.Size(61, 13);
            this.lblmanager.TabIndex = 21;
            this.lblmanager.Text = "MANAGER";
            // 
            // cmb_warehouse_manager
            // 
            this.cmb_warehouse_manager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cmb_warehouse_manager.Enabled = false;
            this.cmb_warehouse_manager.FormattingEnabled = true;
            this.cmb_warehouse_manager.Location = new System.Drawing.Point(114, 32);
            this.cmb_warehouse_manager.Name = "cmb_warehouse_manager";
            this.cmb_warehouse_manager.Size = new System.Drawing.Size(200, 21);
            this.cmb_warehouse_manager.Sorted = true;
            this.cmb_warehouse_manager.TabIndex = 20;
            this.cmb_warehouse_manager.Tag = "manual";
            this.cmb_warehouse_manager.Text = "Lack of authority";
            // 
            // chk_is_inactive
            // 
            this.chk_is_inactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_is_inactive.AutoSize = true;
            this.chk_is_inactive.Location = new System.Drawing.Point(559, 8);
            this.chk_is_inactive.Name = "chk_is_inactive";
            this.chk_is_inactive.Size = new System.Drawing.Size(64, 17);
            this.chk_is_inactive.TabIndex = 19;
            this.chk_is_inactive.Text = "&Inactive";
            this.chk_is_inactive.UseVisualStyleBackColor = true;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.Location = new System.Drawing.Point(345, 38);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(15, 13);
            this.lbl_id.TabIndex = 18;
            this.lbl_id.Text = "id";
            this.lbl_id.Visible = false;
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(364, 34);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(200, 20);
            this.txt_id.TabIndex = 17;
            this.txt_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(114, 10);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(200, 20);
            this.txt_name.TabIndex = 16;
            this.txt_name.Tag = "";
            this.txt_name.Leave += new System.EventHandler(this.txt_name_Leave);
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(364, 10);
            this.txt_code.Name = "txt_code";
            this.txt_code.ReadOnly = true;
            this.txt_code.Size = new System.Drawing.Size(200, 20);
            this.txt_code.TabIndex = 15;
            this.txt_code.TabStop = false;
            this.txt_code.Tag = "no_edit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "NAME";
            // 
            // lblcode
            // 
            this.lblcode.AutoSize = true;
            this.lblcode.Location = new System.Drawing.Point(321, 14);
            this.lblcode.Name = "lblcode";
            this.lblcode.Size = new System.Drawing.Size(37, 13);
            this.lblcode.TabIndex = 13;
            this.lblcode.Text = "CODE";
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.use_type,
            this.zone,
            this.area,
            this.rack,
            this.level,
            this.bins,
            this.location_code,
            this.notes,
            this.warehouse_name_id,
            this.id});
            this.dataTable1.TableName = "areas";
            // 
            // use_type
            // 
            this.use_type.ColumnName = "use_type";
            // 
            // zone
            // 
            this.zone.ColumnName = "zone";
            // 
            // area
            // 
            this.area.ColumnName = "area";
            // 
            // rack
            // 
            this.rack.ColumnName = "rack";
            // 
            // level
            // 
            this.level.ColumnName = "level";
            // 
            // bins
            // 
            this.bins.ColumnName = "bins";
            // 
            // location_code
            // 
            this.location_code.ColumnName = "location_code";
            this.location_code.ReadOnly = true;
            // 
            // notes
            // 
            this.notes.ColumnName = "notes";
            // 
            // warehouse_name_id
            // 
            this.warehouse_name_id.ColumnName = "warehouse_name_id";
            this.warehouse_name_id.DataType = typeof(long);
            this.warehouse_name_id.ReadOnly = true;
            // 
            // id
            // 
            this.id.Caption = "id";
            this.id.ColumnName = "id";
            this.id.DataType = typeof(long);
            this.id.ReadOnly = true;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "areas";
            this.bindingSource1.DataSource = this.dataSet1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_general);
            this.tabControl1.Controls.Add(this.tab_areas);
            this.tabControl1.Location = new System.Drawing.Point(0, 157);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(634, 425);
            this.tabControl1.TabIndex = 32;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tab_general
            // 
            this.tab_general.Controls.Add(this.pnl_address);
            this.tab_general.Location = new System.Drawing.Point(4, 22);
            this.tab_general.Name = "tab_general";
            this.tab_general.Padding = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.tab_general.Size = new System.Drawing.Size(626, 399);
            this.tab_general.TabIndex = 0;
            this.tab_general.Text = "GENERAL";
            this.tab_general.UseVisualStyleBackColor = true;
            // 
            // pnl_address
            // 
            this.pnl_address.Controls.Add(this.AddIdLbl);
            this.pnl_address.Controls.Add(this.txt_warehouse_address_id);
            this.pnl_address.Controls.Add(this.txt_address_id);
            this.pnl_address.Controls.Add(this.WH_Addlbl);
            this.pnl_address.Controls.Add(this.txt_contact_no);
            this.pnl_address.Controls.Add(this.contactnolbl);
            this.pnl_address.Controls.Add(this.txt_contact_person);
            this.pnl_address.Controls.Add(this.txt_country);
            this.pnl_address.Controls.Add(this.txt_zip_code);
            this.pnl_address.Controls.Add(this.txt_city);
            this.pnl_address.Controls.Add(this.txt_barangay_no);
            this.pnl_address.Controls.Add(this.txt_street);
            this.pnl_address.Controls.Add(this.txt_building_no);
            this.pnl_address.Controls.Add(this.contactpersonlbl);
            this.pnl_address.Controls.Add(this.countrylbl);
            this.pnl_address.Controls.Add(this.zipcodelbl);
            this.pnl_address.Controls.Add(this.citylbl);
            this.pnl_address.Controls.Add(this.barangaylbl);
            this.pnl_address.Controls.Add(this.streetlbl);
            this.pnl_address.Controls.Add(this.buildingnolbl);
            this.pnl_address.Location = new System.Drawing.Point(7, 7);
            this.pnl_address.Name = "pnl_address";
            this.pnl_address.Size = new System.Drawing.Size(612, 386);
            this.pnl_address.TabIndex = 1;
            // 
            // AddIdLbl
            // 
            this.AddIdLbl.AutoSize = true;
            this.AddIdLbl.Location = new System.Drawing.Point(25, 299);
            this.AddIdLbl.Name = "AddIdLbl";
            this.AddIdLbl.Size = new System.Drawing.Size(55, 13);
            this.AddIdLbl.TabIndex = 23;
            this.AddIdLbl.Text = "address id";
            this.AddIdLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AddIdLbl.Visible = false;
            // 
            // txt_warehouse_address_id
            // 
            this.txt_warehouse_address_id.Location = new System.Drawing.Point(87, 269);
            this.txt_warehouse_address_id.Name = "txt_warehouse_address_id";
            this.txt_warehouse_address_id.Size = new System.Drawing.Size(200, 20);
            this.txt_warehouse_address_id.TabIndex = 23;
            this.txt_warehouse_address_id.Visible = false;
            // 
            // txt_address_id
            // 
            this.txt_address_id.Location = new System.Drawing.Point(87, 295);
            this.txt_address_id.Name = "txt_address_id";
            this.txt_address_id.Size = new System.Drawing.Size(200, 20);
            this.txt_address_id.TabIndex = 22;
            this.txt_address_id.Visible = false;
            // 
            // WH_Addlbl
            // 
            this.WH_Addlbl.AutoSize = true;
            this.WH_Addlbl.Location = new System.Drawing.Point(19, 272);
            this.WH_Addlbl.Name = "WH_Addlbl";
            this.WH_Addlbl.Size = new System.Drawing.Size(53, 13);
            this.WH_Addlbl.TabIndex = 22;
            this.WH_Addlbl.Text = "wh-add-id";
            this.WH_Addlbl.Visible = false;
            // 
            // txt_contact_no
            // 
            this.txt_contact_no.Location = new System.Drawing.Point(454, 172);
            this.txt_contact_no.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_contact_no.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_contact_no.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_contact_no.Name = "txt_contact_no";
            this.txt_contact_no.Size = new System.Drawing.Size(135, 20);
            this.txt_contact_no.TabIndex = 58;
            // 
            // contactnolbl
            // 
            this.contactnolbl.AutoSize = true;
            this.contactnolbl.Location = new System.Drawing.Point(358, 176);
            this.contactnolbl.Name = "contactnolbl";
            this.contactnolbl.Size = new System.Drawing.Size(83, 13);
            this.contactnolbl.TabIndex = 57;
            this.contactnolbl.Text = "CONTACT NO.:";
            // 
            // txt_contact_person
            // 
            this.txt_contact_person.Location = new System.Drawing.Point(144, 172);
            this.txt_contact_person.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_contact_person.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_contact_person.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_contact_person.Name = "txt_contact_person";
            this.txt_contact_person.Size = new System.Drawing.Size(200, 20);
            this.txt_contact_person.TabIndex = 56;
            // 
            // txt_country
            // 
            this.txt_country.Location = new System.Drawing.Point(144, 146);
            this.txt_country.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_country.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_country.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_country.Name = "txt_country";
            this.txt_country.Size = new System.Drawing.Size(445, 20);
            this.txt_country.TabIndex = 55;
            // 
            // txt_zip_code
            // 
            this.txt_zip_code.Location = new System.Drawing.Point(144, 120);
            this.txt_zip_code.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_zip_code.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_zip_code.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_zip_code.Name = "txt_zip_code";
            this.txt_zip_code.Size = new System.Drawing.Size(445, 20);
            this.txt_zip_code.TabIndex = 54;
            // 
            // txt_city
            // 
            this.txt_city.Location = new System.Drawing.Point(144, 94);
            this.txt_city.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_city.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_city.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_city.Name = "txt_city";
            this.txt_city.Size = new System.Drawing.Size(445, 20);
            this.txt_city.TabIndex = 53;
            // 
            // txt_barangay_no
            // 
            this.txt_barangay_no.Location = new System.Drawing.Point(144, 68);
            this.txt_barangay_no.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_barangay_no.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_barangay_no.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_barangay_no.Name = "txt_barangay_no";
            this.txt_barangay_no.Size = new System.Drawing.Size(445, 20);
            this.txt_barangay_no.TabIndex = 52;
            // 
            // txt_street
            // 
            this.txt_street.Location = new System.Drawing.Point(144, 43);
            this.txt_street.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_street.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_street.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_street.Name = "txt_street";
            this.txt_street.Size = new System.Drawing.Size(445, 20);
            this.txt_street.TabIndex = 51;
            // 
            // txt_building_no
            // 
            this.txt_building_no.Location = new System.Drawing.Point(144, 17);
            this.txt_building_no.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txt_building_no.MaximumSize = new System.Drawing.Size(1200, 60);
            this.txt_building_no.MinimumSize = new System.Drawing.Size(130, 20);
            this.txt_building_no.Name = "txt_building_no";
            this.txt_building_no.Size = new System.Drawing.Size(445, 20);
            this.txt_building_no.TabIndex = 50;
            // 
            // contactpersonlbl
            // 
            this.contactpersonlbl.AutoSize = true;
            this.contactpersonlbl.Location = new System.Drawing.Point(25, 176);
            this.contactpersonlbl.Name = "contactpersonlbl";
            this.contactpersonlbl.Size = new System.Drawing.Size(109, 13);
            this.contactpersonlbl.TabIndex = 49;
            this.contactpersonlbl.Text = "CONTACT PERSON:";
            // 
            // countrylbl
            // 
            this.countrylbl.AutoSize = true;
            this.countrylbl.Location = new System.Drawing.Point(25, 150);
            this.countrylbl.Name = "countrylbl";
            this.countrylbl.Size = new System.Drawing.Size(63, 13);
            this.countrylbl.TabIndex = 48;
            this.countrylbl.Text = "COUNTRY:";
            // 
            // zipcodelbl
            // 
            this.zipcodelbl.AutoSize = true;
            this.zipcodelbl.Location = new System.Drawing.Point(25, 124);
            this.zipcodelbl.Name = "zipcodelbl";
            this.zipcodelbl.Size = new System.Drawing.Size(60, 13);
            this.zipcodelbl.TabIndex = 47;
            this.zipcodelbl.Text = "ZIP CODE:";
            // 
            // citylbl
            // 
            this.citylbl.AutoSize = true;
            this.citylbl.Location = new System.Drawing.Point(25, 98);
            this.citylbl.Name = "citylbl";
            this.citylbl.Size = new System.Drawing.Size(111, 13);
            this.citylbl.TabIndex = 46;
            this.citylbl.Text = "CITY/MUNICIPALTY:";
            // 
            // barangaylbl
            // 
            this.barangaylbl.AutoSize = true;
            this.barangaylbl.Location = new System.Drawing.Point(25, 72);
            this.barangaylbl.Name = "barangaylbl";
            this.barangaylbl.Size = new System.Drawing.Size(91, 13);
            this.barangaylbl.TabIndex = 45;
            this.barangaylbl.Text = "BARANGAY NO.:";
            // 
            // streetlbl
            // 
            this.streetlbl.AutoSize = true;
            this.streetlbl.Location = new System.Drawing.Point(25, 46);
            this.streetlbl.Name = "streetlbl";
            this.streetlbl.Size = new System.Drawing.Size(53, 13);
            this.streetlbl.TabIndex = 44;
            this.streetlbl.Text = "STREET:";
            // 
            // buildingnolbl
            // 
            this.buildingnolbl.AutoSize = true;
            this.buildingnolbl.Location = new System.Drawing.Point(25, 21);
            this.buildingnolbl.Name = "buildingnolbl";
            this.buildingnolbl.Size = new System.Drawing.Size(83, 13);
            this.buildingnolbl.TabIndex = 43;
            this.buildingnolbl.Text = "BUILDING NO.:";
            // 
            // tab_areas
            // 
            this.tab_areas.Controls.Add(this.pnl_areas);
            this.tab_areas.Location = new System.Drawing.Point(4, 22);
            this.tab_areas.Name = "tab_areas";
            this.tab_areas.Padding = new System.Windows.Forms.Padding(3);
            this.tab_areas.Size = new System.Drawing.Size(626, 399);
            this.tab_areas.TabIndex = 1;
            this.tab_areas.Text = "AREAS";
            this.tab_areas.UseVisualStyleBackColor = true;
            // 
            // pnl_areas
            // 
            this.pnl_areas.Controls.Add(this.dg_areas);
            this.pnl_areas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_areas.Enabled = false;
            this.pnl_areas.Location = new System.Drawing.Point(3, 3);
            this.pnl_areas.Name = "pnl_areas";
            this.pnl_areas.Size = new System.Drawing.Size(620, 393);
            this.pnl_areas.TabIndex = 1;
            // 
            // dg_areas
            // 
            this.dg_areas.AllowUserToOrderColumns = true;
            this.dg_areas.AutoGenerateColumns = false;
            this.dg_areas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_areas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dg_areas.CausesValidation = false;
            this.dg_areas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_areas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_use_type,
            this.dg_zone,
            this.dg_area,
            this.dg_rack,
            this.dg_level,
            this.dg_bins,
            this.dg_location_code,
            this.dg_notes,
            this.dg_warehouse_name_id,
            this.dg_id_areas});
            this.dg_areas.DataSource = this.bindingSource1;
            this.dg_areas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_areas.Enabled = false;
            this.dg_areas.Location = new System.Drawing.Point(0, 0);
            this.dg_areas.MultiSelect = false;
            this.dg_areas.Name = "dg_areas";
            this.dg_areas.Size = new System.Drawing.Size(620, 393);
            this.dg_areas.TabIndex = 2;
            this.dg_areas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_CellClick);
            this.dg_areas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_CellDoubleClick);
            this.dg_areas.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_CellEndEdit);
            this.dg_areas.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_CellEnter);
            this.dg_areas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dg_areas_CellFormatting);
            this.dg_areas.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dg_areas_CellPainting);
            this.dg_areas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_CellValueChanged);
            this.dg_areas.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_areas_DataError);
            this.dg_areas.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_areas_RowEnter);
            this.dg_areas.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dg_areas_RowsRemoved);
            this.dg_areas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dg_areas_KeyDown);
            // 
            // dg_use_type
            // 
            this.dg_use_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_use_type.DataPropertyName = "use_type";
            this.dg_use_type.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dg_use_type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dg_use_type.HeaderText = "USE TYPE";
            this.dg_use_type.MinimumWidth = 150;
            this.dg_use_type.Name = "dg_use_type";
            this.dg_use_type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_use_type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_use_type.ToolTipText = "Double Click To Change Value";
            this.dg_use_type.Width = 150;
            // 
            // dg_zone
            // 
            this.dg_zone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_zone.DataPropertyName = "zone";
            this.dg_zone.HeaderText = "ZONE";
            this.dg_zone.MinimumWidth = 75;
            this.dg_zone.Name = "dg_zone";
            this.dg_zone.Width = 75;
            // 
            // dg_area
            // 
            this.dg_area.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_area.DataPropertyName = "area";
            this.dg_area.HeaderText = "AREA";
            this.dg_area.MinimumWidth = 75;
            this.dg_area.Name = "dg_area";
            this.dg_area.Width = 75;
            // 
            // dg_rack
            // 
            this.dg_rack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_rack.DataPropertyName = "rack";
            this.dg_rack.HeaderText = "RACK";
            this.dg_rack.MinimumWidth = 75;
            this.dg_rack.Name = "dg_rack";
            this.dg_rack.Width = 75;
            // 
            // dg_level
            // 
            this.dg_level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_level.DataPropertyName = "level";
            this.dg_level.HeaderText = "LEVEL";
            this.dg_level.MinimumWidth = 75;
            this.dg_level.Name = "dg_level";
            this.dg_level.Width = 75;
            // 
            // dg_bins
            // 
            this.dg_bins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_bins.DataPropertyName = "bins";
            this.dg_bins.HeaderText = "BINS";
            this.dg_bins.MinimumWidth = 75;
            this.dg_bins.Name = "dg_bins";
            this.dg_bins.Width = 75;
            // 
            // dg_location_code
            // 
            this.dg_location_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dg_location_code.DataPropertyName = "location_code";
            this.dg_location_code.HeaderText = "LOCATION CODE";
            this.dg_location_code.MinimumWidth = 200;
            this.dg_location_code.Name = "dg_location_code";
            this.dg_location_code.ReadOnly = true;
            this.dg_location_code.Width = 200;
            // 
            // dg_notes
            // 
            this.dg_notes.DataPropertyName = "notes";
            this.dg_notes.HeaderText = "NOTES";
            this.dg_notes.Name = "dg_notes";
            // 
            // dg_warehouse_name_id
            // 
            this.dg_warehouse_name_id.DataPropertyName = "warehouse_name_id";
            this.dg_warehouse_name_id.HeaderText = "warehouse_name_id";
            this.dg_warehouse_name_id.Name = "dg_warehouse_name_id";
            this.dg_warehouse_name_id.ReadOnly = true;
            this.dg_warehouse_name_id.Visible = false;
            // 
            // dg_id_areas
            // 
            this.dg_id_areas.DataPropertyName = "id";
            this.dg_id_areas.HeaderText = "id";
            this.dg_id_areas.Name = "dg_id_areas";
            this.dg_id_areas.ReadOnly = true;
            this.dg_id_areas.Visible = false;
            // 
            // btn_new
            // 
            this.btn_new.AccessibleName = "";
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
            // btn_search
            // 
            this.btn_search.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.Image")));
            this.btn_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(62, 22);
            this.btn_search.Text = "Search";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_next
            // 
            this.btn_next.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_next.Enabled = false;
            this.btn_next.Image = ((System.Drawing.Image)(resources.GetObject("btn_next.Image")));
            this.btn_next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(51, 22);
            this.btn_next.Text = "Next";
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_prev
            // 
            this.btn_prev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_prev.Enabled = false;
            this.btn_prev.Image = ((System.Drawing.Image)(resources.GetObject("btn_prev.Image")));
            this.btn_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(72, 22);
            this.btn_prev.Text = "Previous";
            this.btn_prev.Click += new System.EventHandler(this.btn_prev_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_new,
            this.btn_edit,
            this.btn_delete,
            this.btn_search,
            this.btn_next,
            this.btn_prev,
            this.btn_save,
            this.btn_cancel});
            this.toolStripButton1.Location = new System.Drawing.Point(0, 63);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.toolStripButton1.Size = new System.Drawing.Size(634, 25);
            this.toolStripButton1.TabIndex = 25;
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
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
            this.btn_cancel.Size = new System.Drawing.Size(56, 22);
            this.btn_cancel.Text = "Close";
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // cmb_search
            // 
            this.cmb_search.FormattingEnabled = true;
            this.cmb_search.Location = new System.Drawing.Point(98, 600);
            this.cmb_search.Name = "cmb_search";
            this.cmb_search.Size = new System.Drawing.Size(216, 21);
            this.cmb_search.TabIndex = 33;
            this.cmb_search.Visible = false;
            this.cmb_search.DropDown += new System.EventHandler(this.cmb_search_DropDown);
            this.cmb_search.SelectionChangeCommitted += new System.EventHandler(this.cmb_search_SelectionChangeCommitted);
            this.cmb_search.TextChanged += new System.EventHandler(this.cmb_search_TextChanged);
            this.cmb_search.Enter += new System.EventHandler(this.cmb_search_Enter);
            this.cmb_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_search_KeyDown);
            this.cmb_search.Leave += new System.EventHandler(this.cmb_search_Leave);
            // 
            // tmplbl
            // 
            this.tmplbl.AutoSize = true;
            this.tmplbl.Location = new System.Drawing.Point(22, 604);
            this.tmplbl.Name = "tmplbl";
            this.tmplbl.Size = new System.Drawing.Size(68, 13);
            this.tmplbl.TabIndex = 34;
            this.tmplbl.Text = "search mock";
            this.tmplbl.Visible = false;
            // 
            // deleteMe
            // 
            this.deleteMe.AutoSize = true;
            this.deleteMe.Location = new System.Drawing.Point(62, 624);
            this.deleteMe.Name = "deleteMe";
            this.deleteMe.Size = new System.Drawing.Size(0, 13);
            this.deleteMe.TabIndex = 35;
            this.deleteMe.Visible = false;
            // 
            // frm_warehouse_name_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteMe);
            this.Controls.Add(this.cmb_search);
            this.Controls.Add(this.tmplbl);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnl_head);
            this.Controls.Add(this.toolStripButton1);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_warehouse_name_setup";
            this.Size = new System.Drawing.Size(634, 647);
            this.Load += new System.EventHandler(this.frm_warehouse_name_setup_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.pnl_head.ResumeLayout(false);
            this.pnl_head.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tab_general.ResumeLayout(false);
            this.pnl_address.ResumeLayout(false);
            this.pnl_address.PerformLayout();
            this.tab_areas.ResumeLayout(false);
            this.pnl_areas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_areas)).EndInit();
            this.toolStripButton1.ResumeLayout(false);
            this.toolStripButton1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_warehouse;
        private System.Windows.Forms.Panel pnl_head;
        private System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.CheckBox chk_is_inactive;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblcode;
        private System.Windows.Forms.Label lblmanager;
        private System.Windows.Forms.ComboBox cmb_warehouse_manager;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripButton btn_new;
        private System.Windows.Forms.ToolStripButton btn_edit;
        private System.Windows.Forms.ToolStripButton btn_delete;
        private System.Windows.Forms.ToolStripButton btn_search;
        private System.Windows.Forms.ToolStripButton btn_next;
        private System.Windows.Forms.ToolStripButton btn_prev;
        private System.Windows.Forms.ToolStrip toolStripButton1;
        private System.Windows.Forms.TabPage tab_general;
        private System.Windows.Forms.TabPage tab_areas;
        private System.Windows.Forms.Panel pnl_areas;
        private System.Windows.Forms.DataGridView dg_areas;
        private System.Windows.Forms.Panel pnl_address;
        private System.Windows.Forms.Label AddIdLbl;
        private System.Windows.Forms.TextBox txt_warehouse_address_id;
        private System.Windows.Forms.TextBox txt_address_id;
        private System.Windows.Forms.Label WH_Addlbl;
        private System.Windows.Forms.TextBox txt_contact_no;
        private System.Windows.Forms.Label contactnolbl;
        private System.Windows.Forms.TextBox txt_contact_person;
        private System.Windows.Forms.TextBox txt_country;
        private System.Windows.Forms.TextBox txt_zip_code;
        private System.Windows.Forms.TextBox txt_city;
        private System.Windows.Forms.TextBox txt_barangay_no;
        private System.Windows.Forms.TextBox txt_street;
        private System.Windows.Forms.TextBox txt_building_no;
        private System.Windows.Forms.Label contactpersonlbl;
        private System.Windows.Forms.Label countrylbl;
        private System.Windows.Forms.Label zipcodelbl;
        private System.Windows.Forms.Label citylbl;
        private System.Windows.Forms.Label barangaylbl;
        private System.Windows.Forms.Label streetlbl;
        private System.Windows.Forms.Label buildingnolbl;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn use_type;
        private System.Data.DataColumn zone;
        private System.Data.DataColumn area;
        private System.Data.DataColumn rack;
        private System.Data.DataColumn level;
        private System.Data.DataColumn bins;
        private System.Data.DataColumn location_code;
        private System.Data.DataColumn notes;
        private System.Data.DataColumn warehouse_name_id;
        private System.Data.DataColumn id;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripButton btn_cancel;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox cmb_search;
        private System.Windows.Forms.Label tmplbl;
        private System.Windows.Forms.Label deleteMe;
        private System.Windows.Forms.DataGridViewComboBoxColumn dg_use_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_zone;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_area;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_rack;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_bins;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_warehouse_name_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_id_areas;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
