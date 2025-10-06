using smpc_app.Services.Helpers;
using System;
using System.Collections.Generic;
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

        private Dictionary<string, string> columnMappings;
        public SearchModal(string title, DataTable data, Dictionary<string, string> columnMappings)
        {
            InitializeComponent();
            this.Text = title;
            this.Dt = data;
            this.columnMappings = columnMappings;

            LoadData(data);
        }

        private void LoadData(DataTable data)
        {
            DataTable filteredTable = new DataTable();

            // Add only selected columns with display text
            foreach (var pair in columnMappings)
            {
                if (data.Columns.Contains(pair.Key))
                {
                    filteredTable.Columns.Add(pair.Value, data.Columns[pair.Key].DataType);
                }
            }

            foreach (DataRow row in data.Rows)
            {
                DataRow newRow = filteredTable.NewRow();
                foreach (var pair in columnMappings)
                {
                    if (data.Columns.Contains(pair.Key))
                    {
                        newRow[pair.Value] = row[pair.Key];
                    }
                }
                filteredTable.Rows.Add(newRow);
            }

            dgv_items.DataSource = filteredTable;
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

            var filteredData = Helpers.FilterDataTable(Dt, searchval, columnMappings.Keys.ToArray());

            LoadData(filteredData); // Re-apply mapping and refresh view
        }


    }
}
