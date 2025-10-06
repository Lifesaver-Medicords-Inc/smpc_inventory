using smpc_app.Services.Helpers;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class SearchModal : Form
    {
        private DataTable Dt { get; set; }
        public DataRow SelectedItem { get; private set; }
        public int SelectedIndex { get; private set; } = -1;

        public SearchModal(string title, DataTable items, string[] columnsToShow)
        {
            InitializeComponent();
            lbl_title.Text = title; // Assuming you have a Label for the title
            this.Dt = items;

            LoadData(columnsToShow);
        }

        private void LoadData(string[] columnsToShow)
        {
            if (Dt == null || Dt.Rows.Count == 0)
            {
                MessageBox.Show("No items available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            dgv_items.DataSource = Dt;

            // Hide all columns first, then show only specified columns
            foreach (DataGridViewColumn column in dgv_items.Columns)
            {
                column.Visible = columnsToShow.Contains(column.Name);
            }

            // Set DataGridView properties
            dgv_items.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_items.ReadOnly = true;
            dgv_items.AllowUserToAddRows = false;
            dgv_items.MultiSelect = false;

            // Attach event handlers
            dgv_items.CellClick += dgv_items_CellClick;
            dgv_items.CellDoubleClick += dgv_items_CellDoubleClick;
        }

        private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) SelectItem();
        }

        private void dgv_items_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) SelectItem();
        }

        private void SelectItem()
        {
            if (dgv_items.SelectedRows.Count > 0)
            {
                SelectedIndex = dgv_items.SelectedRows[0].Index;
                SelectedItem = ((DataRowView)dgv_items.SelectedRows[0].DataBoundItem).Row;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchval = txt_search.Text;
            var data = Helpers.FilterDataTable(Dt, searchval, "item_name", "short_desc");
            dgv_items.DataSource = data;
        }

    }
}
