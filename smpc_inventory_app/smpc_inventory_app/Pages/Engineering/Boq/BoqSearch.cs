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
        public int SelectedIndex { get; private set; }
        public string SelectedItemSetName { get; private set; }
        public string SelectedItemSetId { get; set; }

        private string placeHolderText = "BOQ Search...";
        public string QuotationId { get; set; }
        public string ProjectName { get; set; }
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

        }

        //private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0) SelectItem();
        //}
        public int selectedIndex { get; private set; }
        private void SelectItem()
        {
            if (dgv_boqsearch.SelectedRows.Count > 0)
            {
                SelectedItem = ((DataRowView)dgv_boqsearch.SelectedRows[0].DataBoundItem).Row;

                object projectNameObj = SelectedItem["project_name"];

                if (projectNameObj == DBNull.Value || string.IsNullOrWhiteSpace(projectNameObj.ToString()))
                {
                    QuotationId = SelectedItem["id"].ToString();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    int projectName = (int)SelectedItem["id"];

                    using (ItemSetSearch itemSetSearchForm = new ItemSetSearch(projectName, Dt))
                    {
                        if (itemSetSearchForm.ShowDialog() == DialogResult.OK)
                        {
                            QuotationId = SelectedItem["id"].ToString();
                            ProjectName = SelectedItem["project_name"].ToString();
                            SelectedItemSetName = itemSetSearchForm.SelectedItemSetName;
                            SelectedItemSetId = itemSetSearchForm.SelectedItemSetId;
                            SelectedIndex = itemSetSearchForm.selectedIndex;

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
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

            var searchedData = Helpers.FilterDataTable(Dt, searchText, "id", "project_name", "date");

            dgv_boqsearch.DataSource = searchedData;
        }

        private void dgv_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectItem();
            }
        }

    }
}

