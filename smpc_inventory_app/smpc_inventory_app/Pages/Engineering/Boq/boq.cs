using Newtonsoft.Json.Linq;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Properties;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Sales;
using smpc_inventory_app.Services.Sales.Models;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Boq;
using smpc_inventory_app.Services.Setup.Model.Boq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Boq
{
    public partial class BOQ : UserControl
    {
        ApiResponseModel response;
        DataTable ProjectComponent;
        ProjectComponentClass ProjectComponentResponse;
        public DataTable quotations { get; set; } = new DataTable();
        public DataTable WiringNotes { get; set; } = new DataTable();
        public DataTable QQData { get; set; } = new DataTable();
        public BOQ()
        {
            InitializeComponent();
        }
        private async Task fetchData()
        {
            SalesQuotationList data = await QuotationService.GetQuotations();
            quotations = JsonHelper.ToDataTable(data.SalesQuotation);
            
            WiringNotes[] notes = await BoqServices.GetAsDatatableNote();
            JArray notesArray = JArray.FromObject(notes);
            WiringNotes = JsonHelper.ToDataTable(notesArray);

            QQView[] qqbody = await BoqServices.GetQQView();
            JArray qqbodyarray = JArray.FromObject(qqbody);
            QQData = JsonHelper.ToDataTable(qqbodyarray);

            try
            {
                // Call service that returns both lists
                ProjectComponentResponse = await BoqServices.GetAsDatatable();
                
                if (ProjectComponentResponse != null)
                {
                    if (ProjectComponentResponse.project_component != null)
                        ProjectComponent = JsonHelper.ToDataTable(ProjectComponentResponse.project_component);
                }
                else
                {
                    MessageBox.Show("No data available.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }
        private async void BOQ_Load(object sender, EventArgs e)
        {
            await fetchData();
            btn_search_Click(sender, e);
        }
        private List<int> currentQuotationItemSetIds = new List<int>();
        private int currentIndex = -1;
        int itemsetID = 0;
        string quotationIdStr;
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (quotations == null || quotations.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] columnsToShow = { "id", "project_name", "date" };

            using (BoqSearch boqSearchModal = new BoqSearch("BOQ Search", quotations, columnsToShow))
            {
                
                int selectedIndex = boqSearchModal.selectedIndex;
                if (boqSearchModal.ShowDialog() == DialogResult.OK)
                {
                    string selectedItemSetIdStr = boqSearchModal.SelectedItemSetId;
                    quotationIdStr = boqSearchModal.QuotationId;
                    if (int.TryParse(selectedItemSetIdStr, out int selectedItemSetId) &&
                int.TryParse(quotationIdStr, out int quotationId))
                    {
                        if (ProjectComponent != null && ProjectComponent.Rows.Count > 0)
                        {
                            currentQuotationItemSetIds = ProjectComponent.AsEnumerable()
                            .Where(row => row.Field<int>("quotation_id") == quotationId)
                            .Select(row => row.Field<int>("set_id"))
                            .Distinct()
                            .OrderBy(id => id)
                            .ToList();

                            itemsetID = selectedItemSetId;
                            currentIndex = currentQuotationItemSetIds.IndexOf(selectedItemSetId);
                            fetchBOQ(selectedItemSetId);
                            dgv_qq.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("ProjectComponent data is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else 
                    {
                        fetchQQ(quotationIdStr);
                    }
                }
            }
        }
        private void fetchQQ(string quotationid)
        {
            int.TryParse(quotationid, out int id);
            var filteredRows = from row in QQData.AsEnumerable()
                               where row["based_id"] != DBNull.Value && Convert.ToInt32(row["based_id"]) == id
                               select row;

            DataTable filteredTable = filteredRows.CopyToDataTable();
            DataTable MergedTable = filteredTable.Clone();
            MergedTable.Columns.Add("number", typeof(string));
            double bomCounter = 0;
            int bomDetailIndex = 1;

            foreach (DataRow row in filteredTable.Rows)
            {
                DataRow newRow = MergedTable.NewRow();
                foreach (DataColumn col in filteredTable.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName];
                }

                bool ischild = Convert.ToBoolean(row["is_child"]);

                if (ischild)
                {
                    newRow["number"] = $"{bomCounter}.{bomDetailIndex}";
                    MergedTable.Rows.Add(newRow);
                    bomDetailIndex += 1;
                }
                else
                {
                    bomCounter += 1;
                    newRow["number"] = bomCounter;
                    MergedTable.Rows.Add(newRow);
                    if (bomDetailIndex > 1)
                    {
                        bomDetailIndex = 1;
                    }
                }
            }
            txt_customer.Text = filteredTable.Rows[0]["customer_name"].ToString();
            txt_doc_ref.Text = "Q#" + id;
            txt_item_name.Visible = false;
            txt_project_name.Visible = false;
            pnl_preview.Visible = true;
            dgv_qq.Visible = true;
            lbl_item_set.Visible = false;
            lbl_project_name.Visible = false;
            btn_next.Visible = false;
            btn_prev.Visible = false;
            dgv_qq.DataSource = MergedTable;
        }
        string projectname;
        string itemname;
        string quotationidexport;
        private void fetchBOQ(int selectedItemSetId)
        {
            var filteredRows = from row in ProjectComponent.AsEnumerable()
                               where row.Field<int>("set_id") == selectedItemSetId
                               select row;

            if (filteredRows.Any())
            {
                DataTable filteredTable = filteredRows.CopyToDataTable();
                TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;
                TextBox txtRef = panel_header2.Controls["txt_doc_ref"] as TextBox;
                double bomCounter = 0;
                int bomDetailIndex = 1;
                int secondbomDetailIndex = 1;
                if (txtProjectName != null && txtItemName != null && txtCustomer != null && ProjectComponent.Rows.Count > 0)
                {
                    txtProjectName.Text = filteredTable.Rows[0]["project_name"].ToString();
                    txtItemName.Text = filteredTable.Rows[0]["item_set_name"].ToString();
                    txtCustomer.Text = filteredTable.Rows[0]["customer_name"].ToString();
                    txtRef.Text = "Q#"+filteredTable.Rows[0]["quotation_id"].ToString();

                    projectname = filteredTable.Rows[0]["project_name"].ToString();
                    itemname = filteredTable.Rows[0]["item_set_name"].ToString();
                    quotationidexport = filteredTable.Rows[0]["quotation_id"].ToString();
                }
                DataTable withItemListTwo = filteredTable.Clone();
                withItemListTwo.Columns.Add("number", typeof(string));
                foreach (DataRow row in filteredTable.Rows)
                {
                    DataRow newRow = withItemListTwo.NewRow();
                    foreach (DataColumn col in filteredTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName];
                    }
                    int itemId = Convert.ToInt32(row["item_id"]);
                    int bomId = Convert.ToInt32(row["bom_id"]);
                    string nodetype = (string)row["node_type"];
                    string model = row["model"].ToString();
                    // CHECKER IF THE ROW IS HEAD OF BOM THAT HAS EXISTING ITEMS
                    if (nodetype == "Parent")
                    {
                        bomCounter += 1;
                        newRow["number"] = bomCounter;
                        withItemListTwo.Rows.Add(newRow);
                        if (bomDetailIndex > 1)
                        {
                            bomDetailIndex = 1;
                            secondbomDetailIndex = 1;
                        }
                    }
                    if (itemId == 0 && bomId == 0 && !string.IsNullOrEmpty(model))
                    {
                        newRow["number"] = $"{bomCounter}.{bomDetailIndex}";
                        withItemListTwo.Rows.Add(newRow);
                        bomDetailIndex += 1;
                    }
                    // CHECKER IF THE ROW IS AN ITEM / ACCESSORIES
                    if (itemId > 0 && bomId == 0)
                    {
                        bomCounter += 1;
                        newRow["number"] = bomCounter;
                        withItemListTwo.Rows.Add(newRow);
                    }
                    // CHECKER IF THE ROW IS AN ITEM OF A BOM
                    else if (bomId > 0 && itemId > 0)
                    {
                        newRow["number"] = $"{bomCounter}.{bomDetailIndex}.{secondbomDetailIndex}";
                        withItemListTwo.Rows.Add(newRow);
                        secondbomDetailIndex += 1;
                    }
                }
                dg_boq.DataSource = withItemListTwo;
                SetNotesRemarksEditability();
            }
            else
            {
                MessageBox.Show("No records found for the selected item set.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            filteredRows = from row in WiringNotes.AsEnumerable()
                           where row["based_id"] != DBNull.Value && Convert.ToInt32(row["based_id"]) == selectedItemSetId
                           select row;


            if (filteredRows.Any())
            {
                DataTable filteredTable = filteredRows.CopyToDataTable();
                DataTable withItemListTwo = filteredTable.Clone();
                withItemListTwo.Columns.Add("number", typeof(string));
                withItemListTwo.Columns.Add("uom1", typeof(string));
                withItemListTwo.Columns.Add("uom2", typeof(string));

                List<string> uomValues = new List<string> { "m", "pcs", "pcs", "pcs", "m", "pcs", "m", "m" };
                int counter = 1;
                foreach (DataRow row in filteredTable.Rows)
                {
                    DataRow newRow = withItemListTwo.NewRow();
                    foreach (DataColumn col in filteredTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName];
                    }
                    newRow["number"] = counter;
                    int index = (counter - 1) % uomValues.Count;
                    newRow["uom1"] = uomValues[index];
                    newRow["uom2"] = uomValues[index];
                    counter++;
                    withItemListTwo.Rows.Add(newRow);
                }
                    dg_wiring.DataSource = withItemListTwo;
            }
            else
            {
                dg_wiring.DataSource = WiringNotes.Clone();
            }
            txt_item_name.Visible = true;
            txt_project_name.Visible = true;
            pnl_preview.Visible = false;
            lbl_item_set.Visible = true;
            lbl_project_name.Visible = true;
            dgv_qq.Visible = false;
            btn_next.Visible = true;
            btn_prev.Visible = true;
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (currentQuotationItemSetIds.Count == 0) return;

            // Check if there's only one item
            if (currentQuotationItemSetIds.Count == 1)
            {
                MessageBox.Show("This quotation has only one item set.", "Information",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Circular navigation - wrap around to first item if at end
            currentIndex = (currentIndex + 1) % currentQuotationItemSetIds.Count;
            fetchBOQ(currentQuotationItemSetIds[currentIndex]);
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (currentQuotationItemSetIds.Count == 0) return;

            // Check if there's only one item
            if (currentQuotationItemSetIds.Count == 1)
            {
                MessageBox.Show("This quotation has only one item set.", "Information",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Circular navigation - wrap around to last item if at beginning
            currentIndex = (currentIndex - 1 + currentQuotationItemSetIds.Count) % currentQuotationItemSetIds.Count;
            fetchBOQ(currentQuotationItemSetIds[currentIndex]);
        }
        private void SetNotesRemarksEditability()
        {
            foreach (DataGridViewRow row in dg_boq.Rows)
            {
                if (row.IsNewRow) continue;

                string number = row.Cells["number"].Value?.ToString();
                bool isWholeNumber = !string.IsNullOrWhiteSpace(number) && !number.Contains('.');

                foreach (string colName in new[] { "notes", "remarks" })
                {
                    var cell = row.Cells["notes"];
                    if (isWholeNumber)
                    {
                        cell.ReadOnly = false;
                        cell.Style.BackColor = dg_boq.DefaultCellStyle.BackColor; // normal
                    }
                    else
                    {
                        cell.ReadOnly = true;
                        cell.Style.BackColor = Color.Gainsboro; // visually gray out
                    }
                }
            }
        }

        private async void dg_boq_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_boq);
                List<Dictionary<string, dynamic>> boq = new List<Dictionary<string, dynamic>>();

                // Iterate through the DataTable and populate the opportunity list
                foreach (DataRow item in dataSource.Rows)
                {
                    Dictionary<string, object> existingData = new Dictionary<string, object>();

                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        string columnName = dataSource.Columns[i].ColumnName;
                        string columnValue = item[i].ToString();

                        existingData[columnName] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                    }
                    boq.Add(existingData);
                }
                DataRow editedItem = dataSource.Rows[e.RowIndex];
                Dictionary<string, object> data = new Dictionary<string, object>();

                string[] requiredColumns = {"notes", "remarks", "items_id", "id", "set_id" };
                foreach (var column in requiredColumns)
                {
                    string columnValue = editedItem[column].ToString();
                    data[column] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                }

                // Check and process based_id
                if (editedItem.Table.Columns.Contains("items_id"))
                {
                    var itemsIdValue = editedItem["items_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        // Convert to int (ensure the value is within int's range)
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["items_id"] = (int)itemsIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("items_id value is too large to fit into an int.");
                            return;  // Exit or handle accordingly
                        }
                    }
                    else
                    {
                        data["items_id"] = null; // Or handle the case where the value is invalid
                    }
                }
                if (editedItem.Table.Columns.Contains("set_id"))
                {
                    var itemsIdValue = editedItem["set_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        // Convert to int (ensure the value is within int's range)
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["set_id"] = (int)itemsIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("items_id value is too large to fit into an int.");
                            return;  // Exit or handle accordingly
                        }
                    }
                    else
                    {
                        data["set_id"] = null; // Or handle the case where the value is invalid
                    }
                }

                // Check and process crm_id
                if (editedItem.Table.Columns.Contains("id"))
                {
                    var boqIdValue = editedItem["id"].ToString();
                    if (uint.TryParse(boqIdValue, out uint boqIdUint))
                    {
                        if (boqIdUint <= int.MaxValue)
                        {
                            data["id"] = (int)boqIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("item_boq_id value is too large to fit into an int.");
                            return;
                        }
                    }
                    else
                    {
                        data["id"] = null;
                    }
                }

                var item_boq_id = data.ContainsKey("id") ? data["id"]?.ToString().Trim() : null;

                if (item_boq_id == "0")
                {
                    data["item_boq_id"] = 0;
                    response = await BoqServices.Insert(data);
                    if (response != null && response.Success)
                    {
                        int setId = (int)data["set_id"];

                        MessageBox.Show("Added Successfully");
                        asyncExport(quotationidexport, projectname, itemname);
                        await fetchData();
                        fetchBOQ(setId);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save quotation. Please try againsadsa.");
                    }
                }
                else
                {
                    response = await BoqServices.UpdateBoq(data);
                    if (response != null && response.Success)
                    {
                        int setId = (int)data["set_id"];
                        MessageBox.Show("Edit Successfully updated");
                        asyncExport(quotationidexport, projectname, itemname);
                        await fetchData();
                        fetchBOQ(setId);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update quotation. Please try again.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void dg_wiring_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_wiring);
                List<Dictionary<string, dynamic>> note = new List<Dictionary<string, dynamic>>();

                // Iterate through the DataTable and populate the opportunity list
                foreach (DataRow item in dataSource.Rows)
                {
                    Dictionary<string, object> existingData = new Dictionary<string, object>();

                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        string columnName = dataSource.Columns[i].ColumnName;
                        string columnValue = item[i].ToString();

                        existingData[columnName] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                    }
                    note.Add(existingData);
                }
                DataRow editedItem = dataSource.Rows[e.RowIndex];
                Dictionary<string, object> data = new Dictionary<string, object>();

                string[] requiredColumns = { "wiring_note", "wiring_id", "note_id", "based_id"};
                foreach (var column in requiredColumns)
                {
                    string columnValue = editedItem[column].ToString();
                    data[column] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                }
                
                // Check and process based_id
                if (editedItem.Table.Columns.Contains("wiring_id"))
                {
                    var itemsIdValue = editedItem["wiring_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["wiring_id"] = (int)itemsIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("wiring_id value is too large to fit into an int.");
                            return;  // Exit or handle accordingly
                        }
                    }
                    else
                    {
                        data["wiring_id"] = null; // Or handle the case where the value is invalid
                    }
                }
                if (editedItem.Table.Columns.Contains("based_id"))
                {
                    var itemsIdValue = editedItem["based_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        // Convert to int (ensure the value is within int's range)
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["based_id"] = (int)itemsIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("based_id value is too large to fit into an int.");
                            return;  // Exit or handle accordingly
                        }
                    }
                    else
                    {
                        data["based_id"] = null; // Or handle the case where the value is invalid
                    }
                }
                if (editedItem.Table.Columns.Contains("note_id"))
                {
                    var itemsIdValue = editedItem["note_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        // Convert to int (ensure the value is within int's range)
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["note_id"] = (int)itemsIdUint;  // Safely cast to int
                        }
                        else
                        {
                            MessageBox.Show("note_id value is too large to fit into an int.");
                            return;  // Exit or handle accordingly
                        }
                    }
                    else
                    {
                        data["note_id"] = null; // Or handle the case where the value is invalid
                    }
                }

                var item_note_id = data.ContainsKey("note_id") ? data["note_id"]?.ToString().Trim() : null;
                
                if (item_note_id == "0")
                {
                    data["item_note_id"] = 0;
                    response = await BoqServices.InsertNote(data);
                    if (response != null && response.Success)
                    {
                        int setId = (int)data["based_id"];
                        MessageBox.Show("Added Successfully");
                        asyncExport(quotationidexport, projectname, itemname);
                        await fetchData();
                        fetchBOQ(setId);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save quotation. Please try againsadsa.");
                    }
                }
                else
                {
                    response = await BoqServices.UpdateNote(data);
                    if (response != null && response.Success)
                    {
                        int setId = (int)data["based_id"];
                        MessageBox.Show("Edit Successfully updated");
                        asyncExport(quotationidexport, projectname, itemname);
                        await fetchData();
                        fetchBOQ(setId);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update quotation. Please try again.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            EngineeringPrintModal printPage = new EngineeringPrintModal(itemsetID); ;
            if (!dgv_qq.Visible)
            {
                printPage = new EngineeringPrintModal(itemsetID, false);
            }
            else
            {
                printPage = new EngineeringPrintModal(Convert.ToInt32(quotationIdStr), true);
            }
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            printPage.Height = (int)(screenHeight);
            printPage.StartPosition = FormStartPosition.CenterParent;
            printPage.ShowDialog();
        }

        private async void dgv_qq_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataSource = Helpers.ConvertDataGridViewToDataTable(dgv_qq);
                List<Dictionary<string, dynamic>> note = new List<Dictionary<string, dynamic>>();

                // Define a mapping of original column names to new names
                var columnNameMapping = new Dictionary<string, string>
                {
                    { "qq_note", "notes" },  // Example: original name -> new name
                    { "qq_id", "qq_id" },
                    { "qq_remarks", "remarks" },
                    { "qq_note_id", "id" }
                    // Add other mappings as needed
                };

                // Iterate through the DataTable and populate the opportunity list
                foreach (DataRow item in dataSource.Rows)
                {
                    Dictionary<string, object> existingData = new Dictionary<string, object>();

                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        string columnName = dataSource.Columns[i].ColumnName;
                        string columnValue = item[i].ToString();

                        // Use the mapped column name if it exists in the dictionary
                        string mappedColumnName = columnNameMapping.ContainsKey(columnName) ? columnNameMapping[columnName] : columnName;
                        existingData[mappedColumnName] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                    }
                    note.Add(existingData);
                }

                DataRow editedItem = dataSource.Rows[e.RowIndex];
                Dictionary<string, object> data = new Dictionary<string, object>();

                string[] requiredColumns = { "qq_note", "qq_id", "qq_remarks", "qq_note_id" };
                foreach (var column in requiredColumns)
                {
                    string columnValue = editedItem[column].ToString();
                    string mappedColumnName = columnNameMapping.ContainsKey(column) ? columnNameMapping[column] : column;  // Map the column name

                    data[mappedColumnName] = string.IsNullOrWhiteSpace(columnValue) ? null : columnValue;
                }

                if (editedItem.Table.Columns.Contains("qq_id"))
                {
                    var itemsIdValue = editedItem["qq_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["qq_id"] = (int)itemsIdUint;  // Use the mapped name
                        }
                        else
                        {
                            MessageBox.Show("qq_id value is too large to fit into an int.");
                            return;
                        }
                    }
                    else
                    {
                        data["qq_id"] = null;
                    }
                }

                if (editedItem.Table.Columns.Contains("qq_note_id"))
                {
                    var itemsIdValue = editedItem["qq_note_id"].ToString();
                    if (uint.TryParse(itemsIdValue, out uint itemsIdUint))
                    {
                        if (itemsIdUint <= int.MaxValue)
                        {
                            data["id"] = (int)itemsIdUint;  // Use the mapped name
                        }
                        else
                        {
                            MessageBox.Show("qq_note_id value is too large to fit into an int.");
                            return;
                        }
                    }
                    else
                    {
                        data["id"] = null;
                    }
                }


                var item_note_id = data.ContainsKey("id") ? data["id"]?.ToString().Trim() : null;

                if (item_note_id == "0")
                {
                    data["id"] = 0;
                    response = await BoqServices.Insert(data);
                    if (response != null && response.Success)
                    {
                        //int setId = (int)data["Based ID"];
                        MessageBox.Show("Added Successfully");
                        asyncExport(quotationIdStr);
                        await fetchData();
                        fetchQQ(quotationIdStr);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save quotation. Please try again.");
                    }
                }
                else
                {
                    response = await BoqServices.UpdateBoq(data);
                    if (response != null && response.Success)
                    {
                       // int setId = (int)data["Based ID"];
                        MessageBox.Show("Edit Successfully updated");
                        asyncExport(quotationIdStr);
                        await fetchData();
                        fetchQQ(quotationIdStr);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update quotation. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void asyncExport(string quotationid, string projectname = null, string itemsetname = null)
        {
            var modal = new EngineeringPrintModal();
            string fileName;
            string saveDirectory = Settings.Default.AFTERSALESPATH;
            string active = "ACTIVE";
            string BOQ = "Bill of Quantities & Bill of Materials";
            string fullPath = Path.Combine(saveDirectory, active);
            fullPath = Path.Combine(fullPath, BOQ);
            Directory.CreateDirectory(fullPath);
            if (!string.IsNullOrEmpty(projectname))
            {
                fileName = $"Q#{quotationid} - BOQ {projectname}_{itemsetname}.pdf";
                fullPath = Path.Combine(fullPath, fileName);
                modal = new EngineeringPrintModal(itemsetID, false)
                {
                    AutoExport = true,
                    ExportPath = fullPath
                };
            }
            else
            {
                fileName = $"Q#{quotationid} - BOQ.pdf";
                fullPath = Path.Combine(fullPath, fileName);
                modal = new EngineeringPrintModal(Convert.ToInt32(quotationIdStr), true)
                {
                    AutoExport = true,
                    ExportPath = fullPath
                };
            }
            modal.ShowInTaskbar = false;
            modal.WindowState = FormWindowState.Minimized;
            modal.Show();
        }
    }
}
