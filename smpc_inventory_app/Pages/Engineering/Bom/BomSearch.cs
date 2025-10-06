using smpc_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    public partial class BomSearch : Form
    {
        private DataTable Dt { get; set; }

        public DataRow SelectedItem { get; private set; }
        public int SelectedIndex { get; private set; } = -1;
        public string SelectedItemSetName { get; private set; }
        private string placeHolderText = "BOM Search...";

        public BomSearch(string title, DataTable items, string[] columnsToShow)
        {
            InitializeComponent();
            InitializeSearchBox();
            this.Dt = items;
            LoadData(columnsToShow);
        }

        private void LoadData(string[] columnsToShow)
        {
            Helpers.Loading.ShowLoading(dgv_bomsearch, "Fetching data...");

            if (Dt == null || Dt.Rows.Count == 0)
            {
                MessageBox.Show("No items available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            dgv_bomsearch.AutoGenerateColumns = true;
            dgv_bomsearch.DataSource = Dt;

            foreach (DataGridViewColumn column in dgv_bomsearch.Columns)
            {
                column.Visible = columnsToShow.Contains(column.Name);
            }

            // Hide the item_id column explicitly
            if (dgv_bomsearch.Columns.Contains("item_id"))
            {
                dgv_bomsearch.Columns["item_id"].Visible = false;
            }

            // Set DataGridView properties
            dgv_bomsearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_bomsearch.ReadOnly = true;
            dgv_bomsearch.AllowUserToAddRows = false;
            dgv_bomsearch.MultiSelect = false;

            // Attach event handlers
            //dgv_bomsearch.CellClick += dgv_items_CellClick;
            dgv_bomsearch.CellDoubleClick += dgv_items_CellDoubleClick;

            Helpers.Loading.HideLoading(dgv_bomsearch);
        }

        //private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0) SelectItem();
        //}

        private void dgv_items_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) SelectItem();
        }

        private void SelectItem()
        {
            if (dgv_bomsearch.SelectedRows.Count > 0)
            {
                var row = dgv_bomsearch.SelectedRows[0];
                var dataRow = ((DataRowView)row.DataBoundItem).Row;

                // Get the item_id instead of index
                SelectedItem = dataRow;
                SelectedIndex = Convert.ToInt32(dataRow["item_id"]);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(placeHolderText, txt_search_TextChanged);
            this.Controls.Add(txt_search);
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dgv_bomsearch.DataSource = Dt;
            }
            else
            {
                var searchedData = Helpers.FilterDataTable(Dt, searchText, "item_id", "general_name", "item_code", "item_model", "short_desc");
                dgv_bomsearch.DataSource = searchedData;
            }
        }

        //private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0)
        //    {
        //        SelectItem();

        //        if (SelectedItem != null)
        //        {
        //            string projectName = SelectedItem["project_name"].ToString();

        //            // Only show the ItemSetSearch form if no item set has been selected yet
        //            if (string.IsNullOrEmpty(SelectedItemSetName))
        //            {
        //                ShowItemSetSearchForm(projectName);
        //            }
        //        }
        //    }
        //}

        //private void ShowItemSetSearchForm(string projectName)
        //{
        //    using (ItemSetSearch itemSetSearchForm = new ItemSetSearch(projectName, Dt))
        //    {
        //        if (itemSetSearchForm.ShowDialog() == DialogResult.OK)
        //        {
        //            string selectedItemSetName = itemSetSearchForm.SelectedItemSetName;

        //            // Only update the selected item set if a valid selection is made
        //            if (!string.IsNullOrEmpty(selectedItemSetName))
        //            {
        //                SelectedItemSetName = selectedItemSetName; // Save the selected item set name
        //                MessageBox.Show($"Selected Item Set: {selectedItemSetName}");
        //            }
        //        }
        //    }
        //}
    }
}
