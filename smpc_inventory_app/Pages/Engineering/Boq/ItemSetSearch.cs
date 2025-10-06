using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Boq
{

    public partial class ItemSetSearch : Form
    {
        private DataTable Dt { get; set; }
        private string ProjectName { get; set; }

        public string SelectedItemSetName { get; private set; }

        public ItemSetSearch(string projectName, DataTable data)
        {
            InitializeComponent();
            this.ProjectName = projectName;
            this.Dt = data;  
            LoadItemSets();
        }

        private void LoadItemSets()
        {
            if (Dt == null || Dt.Rows.Count == 0)
            {
                MessageBox.Show("No data available for the selected project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var filteredRows = Dt.AsEnumerable()
                .Where(row => row.Field<string>("project_name") == ProjectName)
                .Select(row => row.Field<string>("item_set_name"))
                .Distinct()
                .ToList();

            if (filteredRows.Count == 0)
            {
                MessageBox.Show($"No item sets found for project: {ProjectName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            listBoxItemSets.DataSource = filteredRows;
        }

        private void listBoxItemSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItemSets.SelectedItem != null)
            {
                SelectedItemSetName = listBoxItemSets.SelectedItem.ToString();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedItemSetName))
            {
                this.DialogResult = DialogResult.OK;  
                this.Close();  
            }
            else
            {
                MessageBox.Show("Please select an item set name.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
