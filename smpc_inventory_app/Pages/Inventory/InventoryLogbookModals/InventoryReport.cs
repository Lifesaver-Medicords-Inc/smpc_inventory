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

namespace smpc_inventory_app.Pages.Inventory.InventoryLogbookModals
{
    public partial class InventoryReport : Form
    {
        private DataTable _rawData;

        public InventoryReport()
        {
            InitializeComponent();

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            var previewForm = new ReportPreview();

            previewForm.ShowDialog();
        }

        private async void InventoryReport_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            //Get inventory data
            _rawData = await InventoryLogbookService.GetAsDatatable();
        }
    }
}
