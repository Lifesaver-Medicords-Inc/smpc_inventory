using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Model;
using smpc_inventory_app.Pages.Inventory;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;

namespace smpc_inventory_app.Pages
{
    // §5.23 Production Report - the Warehouse Manager's acknowledgement queue. Lists
    // every Job Order the engineer has marked COMPLETE that hasn't been WH-acknowledged
    // yet (GET /engineering/job_order/pending_production_reports), company-wide - not
    // scoped to any one engineer, unlike the Engineering app's own Job Order tabs.
    // Acknowledging here (a) flips the job order's is_wh_acknowledged flag, (b) increases
    // item stock at the destination the Warehouse Manager picks, and (c) recomputes the
    // SO item's §7.1 status to IN STOCK - all done server-side in one call.
    public partial class production : UserControl
    {
        private readonly ProductionService _service = new ProductionService();
        private List<ProductionReportModel> _allReports = new List<ProductionReportModel>();

        // §5.23: "acknowledged by the Warehouse Manager" - same position-name gate
        // convention already used for Stock Transfer/Adjust (§14.87) in this app; the
        // real gate is server-side (JobOrderWhAckAccessCode, checked in
        // AcknowledgeJobOrder) - this is a UX nicety only.
        private bool HasAcknowledgeAuthority
        {
            get
            {
                string currentUserPosition = CacheData.CurrentUser.position.name.ToString().ToLower();
                return currentUserPosition.Contains("admin") || currentUserPosition.Contains("warehouse");
            }
        }

        public production()
        {
            InitializeComponent();
        }

        private async void production_Load(object sender, EventArgs e)
        {
            btn_acknowledge.Enabled = HasAcknowledgeAuthority;
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_production, "Fetching pending production reports...");
                _allReports = await _service.GetAsList() ?? new List<ProductionReportModel>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error fetching production reports: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_production);
            }
        }

        private void ApplyFilter()
        {
            string term = txt_search.Text?.Trim().ToLower() ?? "";

            IEnumerable<ProductionReportModel> filtered = _allReports;

            if (!string.IsNullOrEmpty(term))
            {
                filtered = _allReports.Where(r =>
                    (r.sales_order ?? "").ToLower().Contains(term) ||
                    (r.item_desc ?? "").ToLower().Contains(term) ||
                    (r.type ?? "").ToLower().Contains(term) ||
                    (r.serial_no ?? "").ToLower().Contains(term));
            }

            dgv_production.DataSource = filtered.ToList();
        }

        private async void btn_refresh_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dgv_production_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenAcknowledgeModal();
        }

        private void btn_acknowledge_Click(object sender, EventArgs e)
        {
            OpenAcknowledgeModal();
        }

        private async void OpenAcknowledgeModal()
        {
            if (!HasAcknowledgeAuthority)
            {
                Helpers.ShowDialogMessage("error", "Please use an account that is at the Warehouse Manager level or above to acknowledge production.");
                return;
            }

            if (!(dgv_production.CurrentRow?.DataBoundItem is ProductionReportModel selected))
            {
                Helpers.ShowDialogMessage("error", "Please select a row first.");
                return;
            }

            using (var modal = new ProductionAcknowledgeModal(selected))
            {
                if (modal.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Helpers.Loading.ShowLoading(dgv_production, "Acknowledging production...");
                    ApiResponseModel response = await _service.AcknowledgeAsync(selected.id, modal.WarehouseId, modal.BinLocation);

                    if (response != null && response.Success)
                    {
                        Helpers.ShowDialogMessage("success", "Production acknowledged - item stock updated.");
                        await LoadData();
                    }
                    else
                    {
                        Helpers.ShowDialogMessage("error", $"Failed to acknowledge production.\n{response?.message}");
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ShowDialogMessage("error", $"Error acknowledging production: {ex.Message}");
                }
                finally
                {
                    Helpers.Loading.HideLoading(dgv_production);
                }
            }
        }
    }
}
