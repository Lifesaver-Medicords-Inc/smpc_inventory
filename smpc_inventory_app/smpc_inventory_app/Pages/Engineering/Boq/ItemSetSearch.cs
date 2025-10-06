using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Sales;
using smpc_inventory_app.Services.Setup.Boq;
using smpc_inventory_app.Services.Setup.Model.Boq;
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
        private int id { get; set; }
        DataTable ProjectComponent;
        public string SelectedItemSetName { get; private set; }

        public ItemSetSearch(int projectName, DataTable data)
        {
            InitializeComponent();
            this.id = projectName;
            this.Dt = data;
            
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var ProjectComponentResponse = await ProjectService.GetProjects();

                if (ProjectComponentResponse != null)
                {
                    var fullTable = JsonHelper.ToDataTable(ProjectComponentResponse.sales_project_item_set);

                    // Filter using LINQ
                    var filteredRows = fullTable.AsEnumerable()
                        .Where(row => row.Field<int>("based_id") == this.id); // Use your actual column name

                    // Convert filtered result to DataTable and assign it to ProjectComponent
                    ProjectComponent = filteredRows.Any()
                        ? filteredRows.CopyToDataTable()
                        : fullTable.Clone(); // If no match, assign an empty DataTable with same schema

                }
                else
                {
                    MessageBox.Show("No data available.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }

        private void LoadItemSets()
        {
            if (ProjectComponent == null || ProjectComponent.Rows.Count == 0)
            {
                MessageBox.Show("No item sets found for the selected project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listBoxItemSets.DataSource = ProjectComponent;
            listBoxItemSets.DisplayMember = "tab_number";  // What user sees
            listBoxItemSets.ValueMember = "itemset_id";   // What you access
        }
        public string SelectedItemSetId { get; private set; }
        public int selectedIndex { get; private set; }
        private void listBoxItemSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItemSets.SelectedItem != null)
            {
                // Access the selected DataRowView
                DataRowView selectedRowView = (DataRowView)listBoxItemSets.SelectedItem;

                // Access the columns from the DataRowView (e.g., tab_number and item_set_id)
                SelectedItemSetName = selectedRowView["tab_number"].ToString();
                SelectedItemSetId = selectedRowView["itemset_id"].ToString();
                selectedIndex = listBoxItemSets.SelectedIndex;
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

        private async void ItemSetSearch_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
            LoadItemSets();
        }
    }

}
