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
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Pages.Inventory.InventoryTrackerModals;

namespace smpc_inventory_app.Pages
{
    public partial class InventoryTracker : UserControl
    {
        private List<WarehouseName> _warehouseName = new List<WarehouseName>();
        private int currentWarehouseIndex = -1;
        private List<WarehouseAreaModel> _warehouseAreas = new List<WarehouseAreaModel>();
        private DataTable _rawData;
        private Dictionary<string, List<DataRow>> _zoneDetails = new Dictionary<string, List<DataRow>>();
        private string _oldRemarksValue = null;

        Dictionary<string, string[]> columnGroups = new Dictionary<string, string[]>()
        {
            { "RESERVED", new string[] { "units_reserved", "details" } },
            { "OUTBOUND", new string[] { "zone", "units_outbound"} },
        };

        public InventoryTracker()
        {
            InitializeComponent();

            dgv_inventory_item.AutoGenerateColumns = false;
            Helpers.EnableGroupHeaders(dgv_inventory_item, columnGroups);
            Helpers.FreezeVisibleColumns(dgv_inventory_item, 4);
        }

        private async void InventoryTracker_Load(object sender, EventArgs e)
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
            _warehouseName = await InventoryTrackerService.GetWarehouseName();

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

            //Get warehouse zones
            _warehouseAreas = await InventoryTrackerService.GetWarehouseArea(currentWarehouseId);

            //Get inventory data
            _rawData = await InventoryTrackerService.GetAsDatatable(currentWarehouseId);

            //Add dynamic zone columns
            AddZoneColumnsToDataTable(_rawData);

            // Group rows by pod_id
            var groupedData = GroupByPodId(_rawData);

            //Bind grouped data
            dgv_inventory_item.DataSource = groupedData;

            //Add zone columns to grid
            AddZoneColumnsToGrid();

            //Move total_stock right after details
            if (dgv_inventory_item.Columns.Contains("total_stock") && dgv_inventory_item.Columns.Contains("details"))
            {
                int detailsIndex = dgv_inventory_item.Columns["details"].DisplayIndex;
                dgv_inventory_item.Columns["total_stock"].DisplayIndex = detailsIndex + 1;
            }
        }

        private void AddZoneColumnsToDataTable(DataTable data)
        {
            if (_warehouseAreas == null || _warehouseAreas.Count == 0)
                return;

            foreach (var zone in _warehouseAreas.Select(a => a.zone).Where(z => !string.IsNullOrEmpty(z)).Distinct())
            {
                string qtyColumnName = $"zone_{zone}";
                string uomColumnName = $"zone_{zone}_uom";

                if (!data.Columns.Contains(qtyColumnName))
                {
                    data.Columns.Add(qtyColumnName, typeof(string));
                }

                if (!data.Columns.Contains(uomColumnName))
                {
                    data.Columns.Add(uomColumnName, typeof(string));
                }
            }

            // Add total_stock column if missing
            if (!data.Columns.Contains("total_stock"))
            {
                data.Columns.Add("total_stock", typeof(string));
            }
        }

        private DataTable GroupByPodId(DataTable rawData)
        {
            _zoneDetails.Clear();

            DataTable grouped = rawData.Clone(); // keep same schema as your DataGridView DataSource

            var groups = rawData.AsEnumerable().GroupBy(r => r["pod_id"]);

            foreach (var group in groups)
            {
                DataRow newRow = grouped.NewRow();

                // Copy static fields (from first row in group)
                foreach (DataColumn col in rawData.Columns)
                {
                    if (!col.ColumnName.StartsWith("zone_") && col.ColumnName != "total_stock")
                    {
                        newRow[col.ColumnName] = group.First()[col];
                    }
                }

                int total = 0;

                // Fill zone columns and accumulate total
                foreach (var row in group)
                {
                    string location = row["location"]?.ToString();
                    string qtyStr = row["qty"]?.ToString();
                    string uom = row["uom"]?.ToString();

                    string zoneFromLocation = ExtractZoneFromLocation(location);
                    string qtyColumnName = $"zone_{zoneFromLocation}";
                    string uomColumnName = $"zone_{zoneFromLocation}_uom";

                    if (grouped.Columns.Contains(qtyColumnName))
                    {
                        int existingQty = 0;
                        int.TryParse(newRow[qtyColumnName]?.ToString(), out existingQty);

                        int addQty = 0;
                        int.TryParse(qtyStr, out addQty);

                        int newQty = existingQty + addQty;
                        newRow[qtyColumnName] = newQty.ToString();

                        // populate UOM column (always overwrite with the last UOM)
                        if (grouped.Columns.Contains(uomColumnName))
                        {
                            newRow[uomColumnName] = uom;
                        }

                        total += addQty;

                        string key = $"{group.Key}_{zoneFromLocation}"; // pod_id + zone

                        if (!_zoneDetails.ContainsKey(key))
                            _zoneDetails[key] = new List<DataRow>();

                        _zoneDetails[key].Add(row);
                    }
                }

                //Populate the total_stock column that already exists in your grid
                newRow["total_stock"] = total.ToString();

                grouped.Rows.Add(newRow);
            }

            return grouped;
        }

        private void AddZoneColumnsToGrid()
        {
            if (_warehouseAreas == null || _warehouseAreas.Count == 0)
                return;

            // Remove old zone columns
            var zoneColumns = dgv_inventory_item.Columns.Cast<DataGridViewColumn>().Where(c => c.Name.StartsWith("zone_")).ToList();

            foreach (var col in zoneColumns)
            {
                dgv_inventory_item.Columns.Remove(col);
            }

            int insertAfter = 6;

            // Reset column groups for zones
            var newColumnGroups = new Dictionary<string, string[]>(columnGroups); // copy existing RESERVED/OUTBOUND


            foreach (var zone in _warehouseAreas.Select(a => a.zone).Where(z => !string.IsNullOrEmpty(z)).Distinct())
            {
                string qtyColumnName = $"zone_{zone}";
                string uomColumnName = $"zone_{zone}_uom";

                if (!dgv_inventory_item.Columns.Contains(qtyColumnName))
                {
                    var qtyCol = new DataGridViewTextBoxColumn
                    {
                        Name = qtyColumnName,
                        HeaderText = "QTY",
                        DataPropertyName = qtyColumnName,
                        ReadOnly = true
                    };

                    dgv_inventory_item.Columns.Add(qtyCol);
                    dgv_inventory_item.Columns[qtyColumnName].DisplayIndex = insertAfter++;
                }

                if (!dgv_inventory_item.Columns.Contains(uomColumnName))
                {
                    var uomCol = new DataGridViewTextBoxColumn
                    {
                        Name = uomColumnName,
                        HeaderText = "UOM",
                        DataPropertyName = uomColumnName,
                        ReadOnly = true
                    };
                    dgv_inventory_item.Columns.Add(uomCol);
                    dgv_inventory_item.Columns[uomColumnName].DisplayIndex = insertAfter++;
                }

                // Add a new group header for this zone (QTY + UOM)
                newColumnGroups[zone] = new string[] { qtyColumnName, uomColumnName };
            }

            if (!dgv_inventory_item.Columns.Contains("total_stock"))
            {
                var totalCol = new DataGridViewTextBoxColumn
                {
                    Name = "total_stock",
                    HeaderText = "TOTAL STOCK",
                    DataPropertyName = "total_stock",
                    ReadOnly = true
                };

                dgv_inventory_item.Columns.Add(totalCol);
                totalCol.DisplayIndex = dgv_inventory_item.Columns.Count - 1; // put at end
            }

            // Apply the new grouped headers
            Helpers.EnableGroupHeaders(dgv_inventory_item, newColumnGroups);
        }

        private string ExtractZoneFromLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
                return string.Empty;

            int dashIndex = location.IndexOf('-');
            if (dashIndex > 0)
                return location.Substring(0, dashIndex); // take everything before '-'

            return location; // if no dash exists, return full location
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
            }
            catch(Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_inventory_item);
                btn_next.Enabled = true;
            }
        }

        private void dgv_inventory_item_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var col = dgv_inventory_item.Columns[e.ColumnIndex];
            if (!col.Name.StartsWith("zone_")) return;

            var cell = dgv_inventory_item.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value == null || cell.Value.ToString() == "0") return;

            string podId = dgv_inventory_item.Rows[e.RowIndex].Cells["pod_id"].Value.ToString();
            string zone = col.Name.Replace("zone_", "");

            string key = $"{podId}_{zone}";
            if (_zoneDetails.ContainsKey(key))
            {
                var matches = _zoneDetails[key].Select(r => new
                {
                    Id = r["id"].ToString(),
                    Location = r["location"].ToString(),
                    Qty = r["qty"].ToString()
                }).ToList();

                if (matches.Any())
                {
                    var frm = new InventoryTrackerLocation(matches);
                    frm.ShowDialog();
                }
            }
        }

        private void dgv_inventory_item_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgv_inventory_item.Columns[e.ColumnIndex].Name == "remarks")
            {
                var cell = dgv_inventory_item.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _oldRemarksValue = cell.Value?.ToString(); // store old value
            }
        }

        private async void dgv_inventory_item_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_inventory_item.Columns[e.ColumnIndex].Name != "remarks") return;

            var row = dgv_inventory_item.Rows[e.RowIndex];

            if (row.Cells["pod_id"].Value == null) return;

            int podId = Convert.ToInt32(row.Cells["pod_id"].Value);
            int rrdId = Convert.ToInt32(row.Cells["id"].Value);
            var newValue = row.Cells[e.ColumnIndex].Value?.ToString();

            // Prepare payload
            var data = new Dictionary<string, dynamic>
            {
                { "pod_id", podId },
                { "rrd_id", rrdId },
                { "remarks", newValue }
            };

            try
            {
                // Case 1: Initially empty → now filled → CREATE
                if (string.IsNullOrEmpty(_oldRemarksValue) && !string.IsNullOrEmpty(newValue))
                {
                    try
                    {
                        Helpers.Loading.ShowLoading(dgv_inventory_item, "Saving data...");
                        var response = await InventoryTrackerService.CreateInvTracker(data);
                        Helpers.ShowDialogMessage("success", "Remarks created successfully!");
                    }
                    catch (Exception ex)
                    {
                        Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
                    }
                    finally
                    {
                        // Reload the warehouse data after delete
                        await BindWarehouseData();
                        Helpers.Loading.HideLoading(dgv_inventory_item);
                    }
                }
                // Case 2: Initially filled → changed → UPDATE
                else if (!string.IsNullOrEmpty(_oldRemarksValue) && !string.IsNullOrEmpty(newValue) && _oldRemarksValue != newValue)
                {
                    if (row.Cells["rem_id"].Value != null)
                    {
                        int id = Convert.ToInt32(row.Cells["rem_id"].Value);
                        data.Add("id", id);
                    }

                    var response = await InventoryTrackerService.UpdateInvTracker(data);
                    Helpers.ShowDialogMessage("success", "Remarks updated successfully!");
                }
                // Case 3: Initially filled → now empty → DELETE
                else if (!string.IsNullOrEmpty(_oldRemarksValue) && string.IsNullOrEmpty(newValue))
                {
                    if (row.Cells["rem_id"].Value != null)
                    {
                        int id = Convert.ToInt32(row.Cells["rem_id"].Value);

                        try
                        {
                            Helpers.Loading.ShowLoading(dgv_inventory_item, "Deleting data...");
                            var response = await InventoryTrackerService.DeleteInvTracker(new Dictionary<string, dynamic>
                            {
                                { "id", id }
                            });

                            Helpers.ShowDialogMessage("success", "Remarks deleted successfully!");
                        }
                        catch (Exception ex)
                        {
                            Helpers.ShowDialogMessage("error", $"Error fetching data: {ex.Message}");
                        }
                        finally
                        {
                            // Reload the warehouse data after delete
                            await BindWarehouseData();
                            Helpers.Loading.HideLoading(dgv_inventory_item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"Error: {ex.Message}");
            }
        }
    }
}
