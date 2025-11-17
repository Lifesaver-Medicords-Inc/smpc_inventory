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

            if (dgv_items.Columns.Contains("ID"))
            {
                dgv_items.Columns["ID"].Visible = false;
            }
        }

        private void dgv_items_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) SelectItem();
        }

        private void SelectItem()
        {
            if (dgv_items.SelectedRows.Count > 0)
            {
                var selectedRow = (DataRowView)dgv_items.SelectedRows[0].DataBoundItem;
                var id = selectedRow["id"]; 

                SelectedItem = Dt.AsEnumerable().FirstOrDefault(r => r["id"].Equals(id));
                SelectedIndex = Dt.Rows.IndexOf(SelectedItem);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchval = txt_search.Text;

            var filterColumns = columnMappings.Keys.Where(k => k != "id").ToArray();
            var filteredData = Helpers.FilterDataTable(Dt, searchval, filterColumns);

            LoadData(filteredData); 
        }


    }
}
