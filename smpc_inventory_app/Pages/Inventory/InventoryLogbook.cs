using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smpc_app.Services.Helpers;
using System.Windows.Forms;
using smpc_inventory_app.Services.Setup.Inventory;

namespace smpc_inventory_app.Pages.Inventory
{
    public partial class InventoryLogbook : UserControl
    {
        Dictionary<string, string[]> columnGroups = new Dictionary<string, string[]>()
        {
            { "TOTAL", new string[] { "in_total", "out_total" } },
        };

        private List<WarehouseName> _warehouseName = new List<WarehouseName>();
        private int currentWarehouseIndex = -1;
        private DataTable _rawData;

        public InventoryLogbook()
        {
            InitializeComponent();

            Helpers.EnableGroupHeaders(dgv_inventory_item, columnGroups);
        }

        private async void InventoryLogbook_Load(object sender, EventArgs e)
        {
            try
            {
                lbl_month.Text = DateTime.Now.ToString("MMMM");
                lbl_year.Text = DateTime.Now.Year.ToString();

                Helpers.Loading.ShowLoading(dgv_inventory_item, "Fetching data...");
                await LoadData();
            }
            catch (Exception ex)
            {

                Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_inventory_item);
            }
        }

        private async Task LoadData()
        {
            try
            {
                Helpers.Loading.ShowLoading(dgv_inventory_item, "Fetching data...");

                await LoadWarehouseNames();

                if (currentWarehouseIndex >= 0)
                {
                    await BindWarehouseData();
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_inventory_item);
            }
        }

        private async Task LoadWarehouseNames()
        {
            _warehouseName = await InventoryLogbookService.GetWarehouseName();

            if (_warehouseName.Count > 0 && currentWarehouseIndex == -1)
            {
                currentWarehouseIndex = 0;
                lbl_warehouse.Text = _warehouseName[currentWarehouseIndex].name;
            }
        }

        private async Task BindWarehouseData()
        {
            if (currentWarehouseIndex < 0 || currentWarehouseIndex >= _warehouseName.Count)
                return;

            int currentWarehouseId = _warehouseName[currentWarehouseIndex].id;

            //Get inventory data
            _rawData = await InventoryLogbookService.GetAsDatatable(currentWarehouseId);

            //Bind grouped data
            dgv_inventory_item.DataSource = _rawData;

            //Move total_stock right after details
            if (dgv_inventory_item.Columns.Contains("total_stock") && dgv_inventory_item.Columns.Contains("details"))
            {
                int detailsIndex = dgv_inventory_item.Columns["details"].DisplayIndex;
                dgv_inventory_item.Columns["total_stock"].DisplayIndex = detailsIndex + 1;
            }
        }

        private async void btn_next_Click(object sender, EventArgs e)
        {
            if (_warehouseName == null || _warehouseName.Count == 0)
                return;

            currentWarehouseIndex = (currentWarehouseIndex + 1) % _warehouseName.Count;
            lbl_warehouse.Text = _warehouseName[currentWarehouseIndex].name;

            try
            {
                Helpers.Loading.ShowLoading(dgv_inventory_item, "Fetching data...");
                btn_next.Enabled = false;
                await BindWarehouseData();
                txt_search.Clear();
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_inventory_item);
                btn_next.Enabled = true;
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            Helpers.ApplySearchingFilter(dgv_inventory_item, txt_search.Text, "general_name", "brand", "item_desc");
        }
    }
}
