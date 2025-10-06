using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Inventory;

namespace smpc_inventory_app.Pages.Inventory.ReceivingReportModals
{
    public partial class ReceivingReportSearch : Form
    {
        public string SelectedRRId { get; private set; } = null;
        private string placeHolderText = "Receiving Report Search...";
        private DataTable rrTable;

        public ReceivingReportSearch()
        {
            InitializeComponent();
            InitializeSearchBox();
            dgv_rr_search.AutoGenerateColumns = false;
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(placeHolderText, txt_search_TextChanged);
            this.Controls.Add(txt_search);
        }

        private async void ReceivingReportSearch_Load(object sender, EventArgs e)
        {
            await LoadReceivingReports();
        }

        private async Task LoadReceivingReports()
        {
            rrTable = await ReceivingReportService.GetRRRecordsAsDataTable();

            if (rrTable.Rows.Count > 0)
            {
                dgv_rr_search.DataSource = rrTable;
            }
            else
            {
                dgv_rr_search.DataSource = null;
                MessageBox.Show("No receiving reports found.");
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dgv_rr_search.DataSource = rrTable;
            }
            else
            {
                var searchedData = Helpers.FilterDataTable(rrTable, searchText, "supplier_name", "supplier_code", "date_received", "warehouse_name", "prepared_by", "ref_doc");
                dgv_rr_search.DataSource = searchedData;
            }
        }

        private void dgv_rr_search_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure it's not the header row
            {
                // Always get the id value from the row, regardless of which column was clicked
                var idValue = dgv_rr_search.Rows[e.RowIndex].Cells["id"].Value;

                if (idValue != null)
                {
                    SelectedRRId = idValue.ToString();

                    // Log to console
                    Console.WriteLine($"Selected RR Id: {SelectedRRId}");

                    this.DialogResult = DialogResult.OK; // close the modal with OK
                    this.Close();
                }
            }
        }
    }
}
