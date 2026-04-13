using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_inventory_app.Services.Setup.Inventory;
using smpc_inventory_app.Model;
using smpc_app.Services.Helpers;

namespace smpc_inventory_app.Pages.Inventory.ReceivingReport2.ReceivingReport2Modals
{
    public partial class ReceivingReportSearch : Form
    {
        public string SelectedRRId { get; private set; } = null;
        private string placeHolderText = "Receiving Report Search...";
        private ReceivingReportList ReceivingReport;
        readonly ReceivingReport2Service receivingReportService = new ReceivingReport2Service();
        private DataTable rrTable;

        public ReceivingReportSearch()
        {
            InitializeComponent();

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;

            dgv_rr_search.AutoGenerateColumns = false;
            Helpers.DataGridViewDocumentFormatter.DataGridViewDocumentFormat(dgv_rr_search, "doc_no", "RR");
            InitializeSearchBox();
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
                var searchedData = Helpers.FilterDataTable(rrTable, searchText,
                    "supplier", "supplier_code", "date_received", "warehouse", "ref_doc", "doc_no", "prepared_by");
                dgv_rr_search.DataSource = searchedData;
            }
        }

        private async void ReceivingReportSearch_Load(object sender, EventArgs e)
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_rr_search, "Fetching data...");
                await ReceivingReports();
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

        private async Task ReceivingReports()
        {
            try
            {
                ReceivingReport = await receivingReportService.GetAsModel();

                // Convert receiving report list to DataTable using helper
                rrTable = Helpers.ToDataTable(ReceivingReport.receiving_report);

                if (rrTable?.Rows.Count > 0)
                {
                    dgv_rr_search.DataSource = rrTable;
                }
                else
                {
                    dgv_rr_search.DataSource = null;
                    Helpers.ShowDialogMessage("error", "No receiving report found.");
                }
            }
            catch (NullReferenceException)
            {
                Helpers.ShowDialogMessage("error", "No receiving report found.");
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Failed to load: {ex.Message}");
            }
        }

        private void dgv_rr_search_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgv_rr_search.Rows[e.RowIndex];

            // Always get the id value from the row, regardless of which column was clicked
            var idValue = row.Cells["id"].Value;

            if (idValue != null)
            {
                SelectedRRId = idValue.ToString();

                this.DialogResult = DialogResult.OK; // close the modal with OK
                this.Close();
            }
        }
    }
}
