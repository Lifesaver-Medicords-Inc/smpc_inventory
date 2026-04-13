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
using smpc_inventory_app.Services.Setup.Model.Purchasing;

namespace smpc_inventory_app.Pages.Inventory.ReceivingReportModals
{
    public partial class ReceivingReportSearch : Form
    {
        public string SelectedRRId { get; private set; } = null;
        private string placeHolderText = "Receiving Report Search...";
        private DataTable rrTable;
        private ReceivingReportList2 ReceivingReport;

        public ReceivingReportSearch()
        {
            InitializeComponent();
            InitializeSearchBox();
            dgv_rr_search.AutoGenerateColumns = false;

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(placeHolderText, txt_search_TextChanged);
            this.Controls.Add(txt_search);
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            if (rrTable == null || rrTable.Rows.Count == 0)
                return;

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

        private async void ReceivingReportSearch_Load(object sender, EventArgs e)
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_rr_search, "Fetching data...");
                await LoadReceivingReports();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_rr_search);
            }
        }

        private async Task LoadReceivingReports()
        {
            ReceivingReport = await ReceivingReportService.GetRRRecords();

            ReceivingReport.receiving_report.Reverse();

            rrTable = Helpers.ToDataTable(ReceivingReport.receiving_report);

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

        private void dgv_rr_search_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
