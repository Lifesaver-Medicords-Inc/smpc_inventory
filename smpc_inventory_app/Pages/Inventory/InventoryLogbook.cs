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

            // Add day-based columns (IN/OUT)
            AddDailyColumnsToDataTable(_rawData);

            //Bind grouped data
            dgv_inventory_item.DataSource = _rawData;

            //Move total_stock right after details
            if (dgv_inventory_item.Columns.Contains("total_stock") && dgv_inventory_item.Columns.Contains("details"))
            {
                int detailsIndex = dgv_inventory_item.Columns["details"].DisplayIndex;
                dgv_inventory_item.Columns["total_stock"].DisplayIndex = detailsIndex + 1;
            }
        }

        private void AddDailyColumnsToDataTable(DataTable data)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Create columns for each day: in_1, out_1, in_2, out_2, ...
            for (int day = 1; day <= daysInMonth; day++)
            {
                string inCol = $"in_{day}";
                string outCol = $"out_{day}";

                if (!data.Columns.Contains(inCol))
                    data.Columns.Add(inCol, typeof(string));

                if (!data.Columns.Contains(outCol))
                    data.Columns.Add(outCol, typeof(string));
            }

            // Ensure totals exist
            if (!data.Columns.Contains("in_total"))
                data.Columns.Add("in_total", typeof(string));
            if (!data.Columns.Contains("out_total"))
                data.Columns.Add("out_total", typeof(string));
        }

        private void AddDailyColumnsToGrid()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Remove old day columns
            var dayColumns = dgv_inventory_item.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("in_") || c.Name.StartsWith("out_"))
                .ToList();

            foreach (var col in dayColumns)
                dgv_inventory_item.Columns.Remove(col);

            int insertAfter = 5; // Adjust based on your fixed columns
            var newColumnGroups = new Dictionary<string, string[]>(baseColumnGroups);

            // Track columns per group for grouped headers
            List<string> inColumns = new List<string>();
            List<string> outColumns = new List<string>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                string inCol = $"in_{day}";
                string outCol = $"out_{day}";

                // IN columns
                var inColumn = new DataGridViewTextBoxColumn
                {
                    Name = inCol,
                    HeaderText = day.ToString(),
                    DataPropertyName = inCol,
                    ReadOnly = true
                };
                dgv_inventory_item.Columns.Add(inColumn);
                dgv_inventory_item.Columns[inCol].DisplayIndex = insertAfter++;
                inColumns.Add(inCol);

                // OUT columns
                var outColumn = new DataGridViewTextBoxColumn
                {
                    Name = outCol,
                    HeaderText = day.ToString(),
                    DataPropertyName = outCol,
                    ReadOnly = true
                };
                dgv_inventory_item.Columns.Add(outColumn);
                dgv_inventory_item.Columns[outCol].DisplayIndex = insertAfter++;
                outColumns.Add(outCol);
            }

            // Add grouped headers for IN and OUT
            newColumnGroups["IN"] = inColumns.ToArray();
            newColumnGroups["OUT"] = outColumns.ToArray();

            // Add TOTAL at the end if missing
            if (!dgv_inventory_item.Columns.Contains("in_total"))
            {
                dgv_inventory_item.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "in_total",
                    HeaderText = "IN TOTAL",
                    DataPropertyName = "in_total",
                    ReadOnly = true
                });
            }

            if (!dgv_inventory_item.Columns.Contains("out_total"))
            {
                dgv_inventory_item.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "out_total",
                    HeaderText = "OUT TOTAL",
                    DataPropertyName = "out_total",
                    ReadOnly = true
                });
            }

            // Apply grouped headers
            Helpers.EnableGroupHeaders(dgv_inventory_item, newColumnGroups);
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
