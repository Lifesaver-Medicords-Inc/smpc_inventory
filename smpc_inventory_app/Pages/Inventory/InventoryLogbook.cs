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
using System.Globalization;
using smpc_inventory_app.Pages.Inventory.InventoryLogbookModals;

namespace smpc_inventory_app.Pages.Inventory
{
    public partial class InventoryLogbook : UserControl
    {
        Dictionary<string, string[]> columnGroups = new Dictionary<string, string[]>()
        {
            { "TOTAL", new string[] { "in_total", "out_total" } },
        };

        private DataTable _rawData;
        private Dictionary<(int, string), List<(int qty, string rrNo, string poNo, string date, string supplierName)>> _cellMetaData = new Dictionary<(int, string), List<(int, string, string, string, string)>>();

        public InventoryLogbook()
        {
            InitializeComponent();

            Helpers.EnableGroupHeaders(dgv_inventory_item, columnGroups);
            Helpers.FreezeVisibleColumns(dgv_inventory_item, 8);
        }

        private async void InventoryLogbook_Load(object sender, EventArgs e)
        {
            try
            {
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

                await BindWarehouseData();
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

        private async Task BindWarehouseData()
        {
            //Get inventory data
            _rawData = await InventoryLogbookService.GetAsDatatable();

            AdjustColumnsIfNoData();

            // Disable auto column generation
            dgv_inventory_item.AutoGenerateColumns = false;

            cmb_year.SelectedItem = DateTime.Now.Year.ToString();
            cmb_month.SelectedItem = DateTime.Now.ToString("MMMM");

            // Populate filters before grouping
            PopulateYearAndMonthFilters();

            // Filter the DataGridView using the selected filters
            FilterByYearAndMonth();

            // Group rows by item_id
            DataTable groupedData = GroupByItemId(_rawData);

            //Bind grouped data
            dgv_inventory_item.DataSource = groupedData;
        }

        private void AdjustColumnsIfNoData()
        {
            if (_rawData == null || _rawData.Rows.Count == 0)
            {
                if (dgv_inventory_item.Columns.Contains("general_name"))
                    dgv_inventory_item.Columns["general_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (dgv_inventory_item.Columns.Contains("brand"))
                    dgv_inventory_item.Columns["brand"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (dgv_inventory_item.Columns.Contains("item_description"))
                    dgv_inventory_item.Columns["item_description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void StyleInOutColumns()
        {
            foreach (DataGridViewColumn col in dgv_inventory_item.Columns)
            {
                if (col.Name.StartsWith("IN_") || col.Name.StartsWith("OUT_"))
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    col.MinimumWidth = 50;
                }
            }
        }

        private void FixInOutHeaders()
        {
            foreach (DataGridViewColumn col in dgv_inventory_item.Columns)
            {
                if (col.Name.StartsWith("IN_"))
                    col.HeaderText = "IN";

                if (col.Name.StartsWith("OUT_"))
                    col.HeaderText = "OUT";
            }
        }

        private void AddDynamicColumns(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                // Skip the columns you already created in designer
                if (dgv_inventory_item.Columns.Contains(col.ColumnName))
                    continue;

                // Skip non-IN/OUT columns
                if (!col.ColumnName.StartsWith("IN_") && !col.ColumnName.StartsWith("OUT_"))
                    continue;

                // Create a new DataGridView column
                var gridCol = new DataGridViewTextBoxColumn
                {
                    Name = col.ColumnName,
                    HeaderText = col.ColumnName,
                    DataPropertyName = col.ColumnName,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };

                dgv_inventory_item.Columns.Add(gridCol);
            }
        }

        private void RemoveInOutColumns()
        {
            for (int i = dgv_inventory_item.Columns.Count - 1; i >= 0; i--)
            {
                var col = dgv_inventory_item.Columns[i].Name;
                if (col.StartsWith("IN_") || col.StartsWith("OUT_"))
                    dgv_inventory_item.Columns.RemoveAt(i);
            }
        }

        private DataTable GroupByItemId(DataTable rawData)
        {
            if (rawData == null || !rawData.Columns.Contains("item_id"))
                return rawData;

            _cellMetaData.Clear();

            DataTable grouped = rawData.Clone();

            // Remove unnecessary date columns from clone
            foreach (DataColumn col in rawData.Columns)
            {
                if (!grouped.Columns.Contains(col.ColumnName))
                    grouped.Columns.Add(col.ColumnName, col.DataType);
            }

            var groups = rawData.AsEnumerable().GroupBy(r => r["item_id"]);

            foreach (var group in groups)
            {
                // Filter only rows that have data (IN or OUT)
                var filteredRows = group.Where(r =>
                {
                    int qtyIn = int.TryParse(r["qty_in"]?.ToString(), out int qIn) ? qIn : 0;
                    int qtyOut = int.TryParse(r["qty_out"]?.ToString(), out int qOut) ? qOut : 0;
                    return qtyIn >= 0 || qtyOut >= 0;
                }).ToList();

                if (!filteredRows.Any())
                    continue;

                DataRow newRow = grouped.NewRow();
                var first = group.First();

                // Basic item info
                foreach (DataColumn col in rawData.Columns)
                {
                    if (newRow.Table.Columns.Contains(col.ColumnName) &&
                        col.ColumnName != "qty_in" &&
                        col.ColumnName != "qty_out" &&
                        col.ColumnName != "date" &&
                        col.ColumnName != "supplier_name" &&
                        col.ColumnName != "rr_no" &&
                        col.ColumnName != "po_no")
                    {
                        newRow[col.ColumnName] = first[col.ColumnName];
                    }
                }

                // --- Count IN & OUT transactions ---
                int totalInCount = filteredRows.Count(r => Convert.ToInt32(r["qty_in"]) >= 0);
                int totalOutCount = filteredRows.Count(r => Convert.ToInt32(r["qty_out"]) >= 0);

                int inIndex = 1;
                int outIndex = 1;
                int currentRowIndex = grouped.Rows.Count;
                grouped.Rows.Add(newRow);

                foreach (var row in filteredRows)
                {
                    int qtyIn = int.TryParse(row["qty_in"]?.ToString(), out int qIn) ? qIn : 0;
                    int qtyOut = int.TryParse(row["qty_out"]?.ToString(), out int qOut) ? qOut : 0;

                    if (qtyIn >= 0)
                    {
                        string col = $"IN_{inIndex}";
                        if (!grouped.Columns.Contains(col))
                            grouped.Columns.Add(col, typeof(int));
                        newRow[col] = qtyIn;

                        AddCellMetadata(row, currentRowIndex, col);
                        inIndex++;
                    }

                    if (qtyOut >= 0)
                    {
                        string col = $"OUT_{outIndex}";
                        if (!grouped.Columns.Contains(col))
                            grouped.Columns.Add(col, typeof(int));
                        newRow[col] = qtyOut;

                        AddCellMetadata(row, currentRowIndex, col);
                        outIndex++;
                    }
                }

                // Compute totals
                newRow["in_total"] = Enumerable.Range(1, totalInCount)
                                               .Select(i => Convert.ToInt32(newRow[$"IN_{i}"] ?? 0))
                                               .Sum();

                newRow["out_total"] = Enumerable.Range(1, totalOutCount)
                                                .Select(i => Convert.ToInt32(newRow[$"OUT_{i}"] ?? 0))
                                                .Sum();
            }

            return grouped;
        }

        private void AddCellMetadata(DataRow src, int rowIndex, string colName)
        {
            int qty = (src["qty_in"].ToString() != "0")
                        ? Convert.ToInt32(src["qty_in"])
                        : Convert.ToInt32(src["qty_out"]);

            string rrNo = src["rr_no"]?.ToString() ?? "";
            string poNo = src["po_no"]?.ToString() ?? "";
            string date = src["date"]?.ToString() ?? "";
            string supplier = src["supplier_name"]?.ToString() ?? "";

            if (!_cellMetaData.ContainsKey((rowIndex, colName)))
                _cellMetaData[(rowIndex, colName)] = new List<(int, string, string, string, string)>();

            _cellMetaData[(rowIndex, colName)].Add((qty, rrNo, poNo, date, supplier));
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            Helpers.ApplySearchingFilter(dgv_inventory_item, txt_search.Text, "general_name", "brand", "item_description");
        }

        private void dgv_inventory_item_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = dgv_inventory_item.Columns[e.ColumnIndex].Name;

            if (!(columnName.StartsWith("IN_") || columnName.StartsWith("OUT_")))
                return;

            var cellValue = dgv_inventory_item.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                return;

            // Check if cell value is zero (assuming numeric value)
            if (decimal.TryParse(cellValue.ToString(), out decimal numericValue) && numericValue == 0)
                return;

            if (_cellMetaData.TryGetValue((e.RowIndex, columnName), out var metaList))
            {
                StringBuilder message = new StringBuilder();

                if (columnName.StartsWith("IN_"))
                {
                    message.AppendLine("📦 IN ITEM DETAILS\n");
                    int count = 1;
                    foreach (var meta in metaList)
                    {
                        message.AppendLine($"Transaction {count++}");
                        message.AppendLine($"• Quantity: {meta.qty}");
                        message.AppendLine($"• RR #: {meta.rrNo}");
                        message.AppendLine($"• PO #: {meta.poNo}");
                        message.AppendLine($"• Supplier Name: {meta.supplierName}");
                        message.AppendLine($"• Date Received: {meta.date}");
                        message.AppendLine();
                    }
                }
                else if (columnName.StartsWith("OUT_"))
                {
                    message.AppendLine("🚚 OUT ITEM DETAILS\n");
                    int count = 1;
                    foreach (var meta in metaList)
                    {
                        message.AppendLine($"Transaction {count++}");
                        message.AppendLine($"• Quantity: {meta.qty}");
                        message.AppendLine($"• IR #: {meta.rrNo}");
                        message.AppendLine($"• DR #: {meta.poNo}");
                        message.AppendLine($"• Customer Name: {meta.supplierName}");
                        message.AppendLine($"• Date Released: {meta.date}");
                        message.AppendLine();
                    }
                }

                Helpers.ShowDialogMessage("success", message.ToString());
            }
        }

        private void PopulateYearAndMonthFilters()
        {
            if (_rawData == null || !_rawData.Columns.Contains("date"))
                return;

            List<DateTime> validDates = new List<DateTime>();

            // Extract valid dates from _rawData
            foreach (DataRow row in _rawData.Rows)
            {
                string dateStr = row["date"]?.ToString();
                if (DateTime.TryParseExact(dateStr, "M/d/yyyy", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime parsedDate))
                {
                    validDates.Add(parsedDate);
                }
            }

            if (validDates.Count == 0)
                return;

            // --- Get min and max year from data ---
            int minYear = validDates.Min(d => d.Year);
            int maxYear = validDates.Max(d => d.Year);

            // --- Populate cmb_year ---
            cmb_year.Items.Clear();
            for (int year = minYear; year <= maxYear; year++)
            {
                cmb_year.Items.Add(year.ToString());
            }

            // --- Select current year by default if available ---
            if (cmb_year.Items.Contains(DateTime.Now.Year.ToString()))
                cmb_year.SelectedItem = DateTime.Now.Year.ToString();
            else
                cmb_year.SelectedIndex = 0;

            // --- Populate cmb_month based on selected year ---
            PopulateMonthChoices();
        }

        private void PopulateMonthChoices()
        {
            cmb_month.Items.Clear();

            if (_rawData == null || !_rawData.Columns.Contains("date"))
                return;

            if (!int.TryParse(cmb_year.SelectedItem?.ToString(), out int selectedYear))
                return;

            // Get all months from _rawData for the selected year
            var monthsInData = _rawData.AsEnumerable()
                .Select(row =>
                {
                    string dateStr = row["date"]?.ToString();
                    if (DateTime.TryParseExact(dateStr, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    {
                        return dt.Year == selectedYear ? dt.Month : (int?)null;
                    }
                    return null;
                })
                .Where(m => m.HasValue)
                .Select(m => m.Value)
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            // Populate ComboBox with month names from the data
            foreach (int month in monthsInData)
            {
                cmb_month.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
            }

            // Automatically select current month if available
            if (monthsInData.Contains(DateTime.Now.Month))
                cmb_month.SelectedItem = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            else if (cmb_month.Items.Count > 0)
                cmb_month.SelectedIndex = 0;
        }

        private void FilterByYearAndMonth()
        {
            if (_rawData.Rows.Count == 0) return;

            string selectedYear = cmb_year.SelectedItem?.ToString();
            string selectedMonth = cmb_month.SelectedItem?.ToString();

            DataTable filteredTable = _rawData.Clone();

            foreach (DataRow row in _rawData.Rows)
            {
                if (!DateTime.TryParseExact(row["date"].ToString(), "M/d/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime transDate))
                {
                    continue;
                }

                if (transDate.Year.ToString() == selectedYear &&
                    transDate.ToString("MMMM") == selectedMonth)
                {
                    filteredTable.Rows.Add(row.ItemArray);
                }
            }

            DataTable grouped = GroupByItemId(filteredTable);

            RemoveInOutColumns();

            AddDynamicColumns(grouped);

            FixInOutHeaders();

            StyleInOutColumns();

            dgv_inventory_item.DataSource = grouped;
        }

        private void cmb_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveInOutColumns();
            FilterByYearAndMonth();
        }

        private void cmb_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateMonthChoices();
            RemoveInOutColumns();
            FilterByYearAndMonth();
        }

        private void btn_make_report_Click(object sender, EventArgs e)
        {
            if (cmb_year.SelectedItem == null || cmb_month.SelectedItem == null)
            {
                Helpers.ShowDialogMessage("error", "Please select both Year and Month before generating a report.");
                return;
            }

            string selectedYear = cmb_year.SelectedItem.ToString();
            string selectedMonth = cmb_month.SelectedItem.ToString();

            var reportForm = new InventoryReport { _selectedYear = selectedYear, _selectedMonth = selectedMonth};

            reportForm.ShowDialog();
        }
    }
}
