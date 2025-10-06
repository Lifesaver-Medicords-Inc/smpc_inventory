
namespace Inventory_SMPC.Pages
{
    partial class SMPC
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Dashboard");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Item Entry");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Canvass Sheet");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Business Partner Info");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Purchasing List");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("BOM");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("BOQ");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Purchase Order", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Purchase Return");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Item Brand");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Item Class");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Item Material");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Item Name");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Item Pump Count");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Item Pump Type");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Item Type");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Unit of Measure");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Payment terms");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Social Media");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Entity Type");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Industries");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Setup", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMPC));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.flowPanelRedBox = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.MainLeft_Panel = new System.Windows.Forms.Panel();
            this.Sidebar = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_name = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_department = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_date_time = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.MainLeft_Panel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.tabContainer);
            this.MainPanel.Controls.Add(this.panel1);
            this.MainPanel.Controls.Add(this.MainLeft_Panel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1288, 637);
            this.MainPanel.TabIndex = 1;
            // 
            // tabContainer
            // 
            this.tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainer.Location = new System.Drawing.Point(235, 0);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(740, 637);
            this.tabContainer.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabContainer.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCoral;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(975, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 637);
            this.panel1.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.flowPanelRedBox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 58);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(313, 579);
            this.panel4.TabIndex = 1;
            // 
            // flowPanelRedBox
            // 
            this.flowPanelRedBox.AutoScroll = true;
            this.flowPanelRedBox.BackColor = System.Drawing.Color.LightCoral;
            this.flowPanelRedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelRedBox.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanelRedBox.Location = new System.Drawing.Point(0, 0);
            this.flowPanelRedBox.Name = "flowPanelRedBox";
            this.flowPanelRedBox.Size = new System.Drawing.Size(313, 579);
            this.flowPanelRedBox.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightCoral;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(313, 58);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(101, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "RED BOX";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkRed;
            this.panel3.Location = new System.Drawing.Point(0, 53);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(313, 1);
            this.panel3.TabIndex = 3;
            // 
            // MainLeft_Panel
            // 
            this.MainLeft_Panel.Controls.Add(this.Sidebar);
            this.MainLeft_Panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainLeft_Panel.Location = new System.Drawing.Point(0, 0);
            this.MainLeft_Panel.Name = "MainLeft_Panel";
            this.MainLeft_Panel.Size = new System.Drawing.Size(235, 637);
            this.MainLeft_Panel.TabIndex = 3;
            // 
            // Sidebar
            // 
            this.Sidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Sidebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sidebar.Location = new System.Drawing.Point(0, 0);
            this.Sidebar.Name = "Sidebar";
            treeNode1.Name = "DASHBOARD";
            treeNode1.Text = "Dashboard";
            treeNode2.Name = "ITEM ENTRY";
            treeNode2.Text = "Item Entry";
            treeNode3.Name = "CANVASS SHEET";
            treeNode3.Text = "Canvass Sheet";
            treeNode4.Name = "BUSINESS PARTNER INFO";
            treeNode4.Text = "Business Partner Info";
            treeNode5.Name = "PURCHASING LIST";
            treeNode5.Text = "Purchasing List";
            treeNode6.Name = "BOM";
            treeNode6.Text = "BOM";
            treeNode7.Name = "BOQ";
            treeNode7.Text = "BOQ";
            treeNode8.Name = "parent";
            treeNode8.Text = "Purchase Order";
            treeNode9.Name = "PURCHASE RETURN";
            treeNode9.Text = "Purchase Return";
            treeNode10.Name = "ITEM BRAND";
            treeNode10.Text = "Item Brand";
            treeNode11.Name = "ITEM CLASS";
            treeNode11.Text = "Item Class";
            treeNode12.Name = "ITEM MATERIAL";
            treeNode12.Text = "Item Material";
            treeNode13.Name = "ITEM NAME";
            treeNode13.Text = "Item Name";
            treeNode14.Name = "ITEM PUMP COUNT";
            treeNode14.Text = "Item Pump Count";
            treeNode15.Name = "ITEM PUMP TYPE";
            treeNode15.Text = "Item Pump Type";
            treeNode16.Name = "ITEM TYPE";
            treeNode16.Text = "Item Type";
            treeNode17.Name = "UNIT OF MEASURE";
            treeNode17.Text = "Unit of Measure";
            treeNode18.Name = "PAYMENT TERMS";
            treeNode18.Text = "Payment terms";
            treeNode19.Name = "SOCIAL MEDIA";
            treeNode19.Text = "Social Media";
            treeNode20.Name = "ENTITY TYPE";
            treeNode20.Text = "Entity Type";
            treeNode21.Name = "INDUSTRIES";
            treeNode21.Text = "Industries";
            treeNode22.Name = "parent";
            treeNode22.Text = "Setup";
            this.Sidebar.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode8,
            treeNode9,
            treeNode22});
            this.Sidebar.Size = new System.Drawing.Size(235, 637);
            this.Sidebar.TabIndex = 0;
            this.Sidebar.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Sidebar_NodeMouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lbl_name,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.lbl_position,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.lbl_department,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.lbl_date_time});
            this.statusStrip1.Location = new System.Drawing.Point(0, 615);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1288, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(33, 17);
            this.toolStripStatusLabel1.Text = "User:";
            // 
            // lbl_name
            // 
            this.lbl_name.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(55, 17);
            this.toolStripStatusLabel2.Text = "                ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(53, 17);
            this.toolStripStatusLabel3.Text = "Position:";
            // 
            // lbl_position
            // 
            this.lbl_position.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_position.Name = "lbl_position";
            this.lbl_position.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(55, 17);
            this.toolStripStatusLabel6.Text = "                ";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel7.Text = "Department:";
            // 
            // lbl_department
            // 
            this.lbl_department.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_department.Name = "lbl_department";
            this.lbl_department.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(55, 17);
            this.toolStripStatusLabel4.Text = "                ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(84, 17);
            this.toolStripStatusLabel5.Text = "Date and time:";
            this.toolStripStatusLabel5.Visible = false;
            // 
            // lbl_date_time
            // 
            this.lbl_date_time.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date_time.Name = "lbl_date_time";
            this.lbl_date_time.Size = new System.Drawing.Size(124, 17);
            this.lbl_date_time.Text = "Feb 7, 2025 11:02AM";
            this.lbl_date_time.Visible = false;
            // 
            // SMPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1288, 637);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SMPC";
            this.Text = "SMPC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SMPC_Load);
            this.MainPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.MainLeft_Panel.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel MainLeft_Panel;
        private System.Windows.Forms.TreeView Sidebar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_name;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lbl_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel lbl_department;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel lbl_date_time;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.FlowLayoutPanel flowPanelRedBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
    }
}