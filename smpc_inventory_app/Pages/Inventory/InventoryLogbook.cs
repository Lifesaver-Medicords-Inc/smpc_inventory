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
namespace smpc_inventory_app.Pages.Inventory
{
    public partial class InventoryLogbook : UserControl
    {
        Dictionary<string, string[]> columnGroups = new Dictionary<string, string[]>()
        {
            { "TOTAL", new string[] { "in_total", "out_total" } },
        };

        private DataTable _rawData;
        private Dictionary<(int, string), (string rrNo, string poNo, string date, string supplierName)> _cellMetaData = new Dictionary<(int, string), (string rrNo, string poNo, string date, string supplierName)>();


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

            // Disable auto column generation
            dgv_inventory_item.AutoGenerateColumns = false;

            cmb_year.SelectedItem = DateTime.Now.Year.ToString();
            cmb_month.SelectedItem = DateTime.Now.ToString("MMMM");

            // Populate filters before grouping
            PopulateYearAndMonthFilters();

            // Filter the DataGridView using the selected filters
            FilterByYearAndMonth();

            // Group rows by pod_id
            DataTable groupedData = GroupByPodId(_rawData);

            //Bind grouped data
            dgv_inventory_item.DataSource = groupedData;

            //Add dynamic "IN" and "OUT" columns per day without clearing existing columns
            AddInOutColumnsWithGroupHeaders();
        }

        private void AddInOutColumnsWithGroupHeaders()
        {
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            // Add new IN/OUT columns and track their names
            for (int day = 1; day <= daysInMonth; day++)
            {
                string inColumnName = $"IN_{day}";
                string outColumnName = $"OUT_{day}";

                // --- Add IN column ---
                if (!dgv_inventory_item.Columns.Contains(inColumnName))
                {
                    DataGridViewTextBoxColumn inCol = new DataGridViewTextBoxColumn
                    {
                        Name = inColumnName,
                        DataPropertyName = inColumnName,
                        HeaderText = "IN",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    dgv_inventory_item.Columns.Add(inCol);
                }

                // --- Add OUT column ---
                if (!dgv_inventory_item.Columns.Contains(outColumnName))
                {
                    DataGridViewTextBoxColumn outCol = new DataGridViewTextBoxColumn
                    {
                        Name = outColumnName,
                        DataPropertyName = outColumnName,
                        HeaderText = "OUT",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    dgv_inventory_item.Columns.Add(outCol);
                }

                // Ensure OUT column is right beside its corresponding IN column
                int inIndex = dgv_inventory_item.Columns[inColumnName].DisplayIndex;
                dgv_inventory_item.Columns[outColumnName].DisplayIndex = inIndex + 1;
            }
        }

        private DataTable GroupByPodId(DataTable rawData)
        {
            if (rawData == null || !rawData.Columns.Contains("pod_id"))
                return rawData;

            DataTable grouped = rawData.Clone(); // start with base structure

            // Convert lbl_month to month number (e.g. "October" → 10)
            int targetMonth = DateTime.ParseExact(cmb_month.SelectedItem.ToString(), "MMMM", CultureInfo.InvariantCulture).Month;
            int targetYear = int.Parse(cmb_year.SelectedItem.ToString());
            int daysInMonth = DateTime.DaysInMonth(targetYear, targetMonth);

            // Add dynamic IN and OUT columns if they don't exist in the grouped table
            for (int day = 1; day <= daysInMonth; day++)
            {
                string inColName = $"IN_{day}";
                string outColName = $"OUT_{day}";

                if (!grouped.Columns.Contains(inColName))
                    grouped.Columns.Add(inColName, typeof(int)); // for qty_in

                if (!grouped.Columns.Contains(outColName))
                    grouped.Columns.Add(outColName, typeof(int)); // for qty_out
            }

            var groups = rawData.AsEnumerable().GroupBy(r => r["pod_id"]);

            foreach (var group in groups)
            {
                // Filter only rows that match selected month/year
                var filteredRows = group.Where(row =>
                {
                    string dateStr = row["date"]?.ToString();
                    if (string.IsNullOrWhiteSpace(dateStr)) return false;

                    if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out DateTime parsedDate))
                    {
                        return parsedDate.Month == targetMonth && parsedDate.Year == targetYear;
                    }

                    return false;
                }).ToList();

                if (!filteredRows.Any())
                    continue; // skip groups without valid rows

                Console.WriteLine($"\n=== POD ID: {group.Key} ===");

                // Create new grouped row
                DataRow newRow = grouped.NewRow();

                // Copy static fields from first record
                var first = group.First();
                newRow["pod_id"] = first["pod_id"];
                if (rawData.Columns.Contains("item_code")) newRow["item_code"] = first["item_code"];
                if (rawData.Columns.Contains("general_name")) newRow["general_name"] = first["general_name"];
                if (rawData.Columns.Contains("brand")) newRow["brand"] = first["brand"];
                if (rawData.Columns.Contains("item_desc")) newRow["item_desc"] = first["item_desc"];
                if (rawData.Columns.Contains("details")) newRow["details"] = first["details"];
                if (rawData.Columns.Contains("uom")) newRow["uom"] = first["uom"];

                // --- Process each filtered row ---
                foreach (var row in filteredRows)
                {
                    string dateStr = row["date"]?.ToString();
                    int qtyIn = 0;
                    int qtyOut = 0;
                    int.TryParse(row["qty_in"]?.ToString(), out qtyIn);
                    int.TryParse(row["qty_out"]?.ToString(), out qtyOut);

                    if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out DateTime parsedDate))
                    {
                        int day = parsedDate.Day;
                        string inCol = $"IN_{day}";
                        string outCol = $"OUT_{day}";

                        // --- Handle IN ---
                        int currentIn = newRow[inCol] == DBNull.Value ? 0 : Convert.ToInt32(newRow[inCol]);
                        newRow[inCol] = currentIn + qtyIn;

                        // --- Handle OUT ---
                        int currentOut = newRow[outCol] == DBNull.Value ? 0 : Convert.ToInt32(newRow[outCol]);
                        newRow[outCol] = currentOut + qtyOut;

                        Console.WriteLine($"Day {day}: IN +{qtyIn}, OUT +{qtyOut}");

                        // Save rr_id, po_id and supplier name metadata for cells that have data
                        string rrNo = row["rr_no"]?.ToString() ?? "";
                        string poNo = row["po_no"]?.ToString() ?? "";
                        string supplierName = row["supplier_name"]?.ToString() ?? "";

                        // We'll add the mapping *after* adding newRow to grouped.Rows below
                        // So we temporarily keep these in a local list
                        if (!_cellMetaData.ContainsKey((grouped.Rows.Count, inCol)) && qtyIn > 0)
                            _cellMetaData[(grouped.Rows.Count, inCol)] = (rrNo, poNo, dateStr, supplierName);

                        if (!_cellMetaData.ContainsKey((grouped.Rows.Count, outCol)) && qtyOut > 0)
                            _cellMetaData[(grouped.Rows.Count, outCol)] = (rrNo, poNo, dateStr, supplierName);
                    }
                }

                grouped.Rows.Add(newRow);

                // Get the actual index of the row we just added
                int currentRowIndex = grouped.Rows.Count - 1;

                // Loop again to store metadata for each cell that has data
                foreach (var row in filteredRows)
                {
                    string dateStr = row["date"]?.ToString();
                    string supplierName = row["supplier_name"]?.ToString();
                    string rrNo = row["rr_no"]?.ToString() ?? "";
                    string poNo = row["po_no"]?.ToString() ?? "";
                    int qtyIn = int.TryParse(row["qty_in"]?.ToString(), out int tmpIn) ? tmpIn : 0;
                    int qtyOut = int.TryParse(row["qty_out"]?.ToString(), out int tmpOut) ? tmpOut : 0;

                    if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        int day = parsedDate.Day;
                        string inCol = $"IN_{day}";
                        string outCol = $"OUT_{day}";

                        if (qtyIn > 0)
                            _cellMetaData[(currentRowIndex, inCol)] = (rrNo, poNo, dateStr, supplierName);

                        if (qtyOut > 0)
                            _cellMetaData[(currentRowIndex, outCol)] = (rrNo, poNo, dateStr, supplierName);
                    }
                }
            }

            // Compute total IN per row before returning
            ComputeInTotals(grouped);
            ComputeOutTotals(grouped);
            return grouped;
        }

        private void ComputeInTotals(DataTable grouped)
        {
            if (grouped == null) return;

            foreach (DataRow row in grouped.Rows)
            {
                int totalIn = 0;

                // Go through all columns that start with "IN_"
                foreach (DataColumn col in grouped.Columns)
                {
                    if (col.ColumnName.StartsWith("IN_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (row[col] != DBNull.Value && int.TryParse(row[col].ToString(), out int val))
                        {
                            totalIn += val;
                        }
                    }
                }

                // Store the total in "in_total" column (make sure it exists)
                if (grouped.Columns.Contains("in_total"))
                    row["in_total"] = totalIn;
                else
                {
                    grouped.Columns.Add("in_total", typeof(int));
                    row["in_total"] = totalIn;
                }
            }
        }

        private void ComputeOutTotals(DataTable grouped)
        {
            if (grouped == null) return;

            foreach (DataRow row in grouped.Rows)
            {
                int totalOut = 0;

                foreach (DataColumn col in grouped.Columns)
                {
                    if (col.ColumnName.StartsWith("OUT_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (row[col] != DBNull.Value && int.TryParse(row[col].ToString(), out int val))
                        {
                            totalOut += val;
                        }
                    }
                }

                // Store total in "out_total" column
                if (grouped.Columns.Contains("out_total"))
                    row["out_total"] = totalOut;
                else
                {
                    grouped.Columns.Add("out_total", typeof(int));
                    row["out_total"] = totalOut;
                }
            }
        }


        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            Helpers.ApplySearchingFilter(dgv_inventory_item, txt_search.Text, "general_name", "brand", "item_desc");
        }

        private void dgv_inventory_item_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks on headers or invalid indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = dgv_inventory_item.Columns[e.ColumnIndex].Name;

            // Only handle IN_x or OUT_x columns
            if (!(columnName.StartsWith("IN_") || columnName.StartsWith("OUT_")))
                return;

            var cellValue = dgv_inventory_item.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            // Only respond if the cell has data
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                return;

            // Check if metadata exists for this specific cell
            if (_cellMetaData.TryGetValue((e.RowIndex, columnName), out var meta))
            {
                string message = "";

                if (columnName.StartsWith("IN_"))
                {
                    message =
                        $"📦 IN ITEM DETAILS\n\n" +
                        $"• Quantity: {cellValue}\n" +
                        $"• RR #: {meta.rrNo}\n" +
                        $"• PO #: {meta.poNo}\n" +
                        $"• Supplier Name: {meta.supplierName}\n" +
                        $"• Date Received: {meta.date}\n";

                    Helpers.ShowDialogMessage("success", message);
                }
                else if (columnName.StartsWith("OUT_"))
                {
                    message =
                        $"🚚 OUT ITEM DETAILS\n\n" +
                        $"• Quantity: {cellValue}\n" +
                        $"• IR #: {meta.rrNo}\n" +
                        $"• DR #: {meta.poNo}\n" +
                        $"• Customer Name: {meta.supplierName}\n" +
                        $"• Date Released: {meta.date}\n";

                    Helpers.ShowDialogMessage("success", message);
                }
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
                if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture,
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

            int selectedYear = 0;
            if (!int.TryParse(cmb_year.SelectedItem?.ToString(), out selectedYear))
                return;

            // Determine how many months to show
            int maxMonth = (selectedYear == DateTime.Now.Year)
                ? DateTime.Now.Month  // only up to current month
                : 12;                 // full year

            // Add months from January up to maxMonth
            for (int month = 1; month <= maxMonth; month++)
            {
                cmb_month.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
            }

            // Select the current month if it's available
            if (selectedYear == DateTime.Now.Year)
                cmb_month.SelectedItem = DateTime.Now.ToString("MMMM");
            else
                cmb_month.SelectedIndex = 0;
        }

        private void FilterByYearAndMonth()
        {
            if (_rawData == null || !_rawData.Columns.Contains("date"))
                return;

            if (cmb_year.SelectedItem == null || cmb_month.SelectedItem == null)
                return;

            int selectedYear = int.Parse(cmb_year.SelectedItem.ToString());
            int selectedMonth = DateTime.ParseExact(cmb_month.SelectedItem.ToString(), "MMMM", CultureInfo.InvariantCulture).Month;

            // Filter rows that match the selected year and month
            var filteredRows = _rawData.AsEnumerable().Where(row =>
            {
                string dateStr = row["date"]?.ToString();
                if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime parsedDate))
                {
                    return parsedDate.Year == selectedYear && parsedDate.Month == selectedMonth;
                }
                return false;
            });

            if (!filteredRows.Any())
            {
                dgv_inventory_item.DataSource = null;
                return;
            }

            DataTable filteredTable = filteredRows.CopyToDataTable();

            // Group and bind the filtered data
            DataTable grouped = GroupByPodId(filteredTable);
            dgv_inventory_item.DataSource = grouped;

            // Re-add dynamic IN/OUT columns (if needed)
            AddInOutColumnsWithGroupHeaders();
        }

        private void cmb_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterByYearAndMonth();
        }

        private void cmb_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateMonthChoices();
            FilterByYearAndMonth();
        }

        private void btn_make_report_Click(object sender, EventArgs e)
        {
        }
    }
}
