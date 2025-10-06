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

namespace smpc_inventory_app.Pages.Engineering.Boq
{
    public partial class BoqSearch : Form
    {
        private DataTable Dt { get; set; }

        public DataRow SelectedItem { get; private set; }
        public int SelectedIndex { get; private set; } = -1;
        public string SelectedItemSetName { get; private set; }

        private string placeHolderText = "BOQ Search...";

        public BoqSearch(string title, DataTable items, string[] columnsToShow)
        {
            InitializeComponent();
            InitializeSearchBox();
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

            dgv_boqsearch.DataSource = Dt;

            // Hide all columns first, then show only specified columns
            foreach (DataGridViewColumn column in dgv_boqsearch.Columns)
            {
                column.Visible = columnsToShow.Contains(column.Name);
            }

            // Set DataGridView properties
            dgv_boqsearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_boqsearch.ReadOnly = true;
            dgv_boqsearch.AllowUserToAddRows = false;
            dgv_boqsearch.MultiSelect = false;

            // Attach event handlers
            dgv_boqsearch.CellClick += dgv_items_CellClick;
            dgv_boqsearch.CellDoubleClick += dgv_items_CellDoubleClick;
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
            if (dgv_boqsearch.SelectedRows.Count > 0)
            {
                SelectedIndex = dgv_boqsearch.SelectedRows[0].Index;
                SelectedItem = ((DataRowView)dgv_boqsearch.SelectedRows[0].DataBoundItem).Row;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void InitializeSearchBox()
        {
            txt_search = new TextBox
            {
                Name = "txt_search",
                Dock = DockStyle.Top,
                ForeColor = Color.Gray,
                Text = placeHolderText
            };

            txt_search.Enter += txt_search_Enter;
            txt_search.Leave += txt_search_Leave;
            txt_search.TextChanged += txt_search_TextChanged;
            this.Controls.Add(txt_search);

        }



        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dgv_boqsearch.DataSource = Dt;
            }
            else
            {
                ApplySearchFilter(searchText.ToLower());
            }
        }

        private void txt_search_Enter(object sender, EventArgs e)
        {
            if (txt_search.Text == placeHolderText)
            {
                txt_search.Text = "";
                txt_search.ForeColor = Color.Black;
            }
        }

        private void txt_search_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = placeHolderText;
                txt_search.ForeColor = Color.Gray;
            }
        }

        private void ApplySearchFilter(string searchText)
        {

            var searchedData = Helpers.FilterDataTable(Dt, searchText, "quotation_id", "project_name", "item_set_name", "customer_name");

            dgv_boqsearch.DataSource = searchedData;


        }




        private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectItem();

                if (SelectedItem != null)
                {
                    string projectName = SelectedItem["project_name"].ToString();

                    // Only show the ItemSetSearch form if no item set has been selected yet
                    if (string.IsNullOrEmpty(SelectedItemSetName))
                    {
                        ShowItemSetSearchForm(projectName);
                    }
                }
            }
        }

        private void ShowItemSetSearchForm(string projectName)
        {
            using (ItemSetSearch itemSetSearchForm = new ItemSetSearch(projectName, Dt))
            {
                if (itemSetSearchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItemSetName = itemSetSearchForm.SelectedItemSetName;

                    // Only update the selected item set if a valid selection is made
                    if (!string.IsNullOrEmpty(selectedItemSetName))
                    {
                        SelectedItemSetName = selectedItemSetName; // Save the selected item set name
                        MessageBox.Show($"Selected Item Set: {selectedItemSetName}");
                    }
                }
            }
        }






    }
}

