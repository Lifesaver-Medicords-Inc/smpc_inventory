using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Engineering.Bom;
using smpc_inventory_app.Pages.Engineering.Boq;
using smpc_inventory_app.Pages.Setup;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Bom;
using smpc_inventory_app.Services.Setup.Boq;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Bom;
using smpc_inventory_app.Services.Setup.Model.Boq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages
{
    public partial class boq_wiring : UserControl
    {
        DataTable ProjectComponent;
        int selectedRecord;
        DataTable SalesProjectWiring;
        ProjectComponentClass ProjectComponentResponse;

        public dynamic remarks { get; private set; }
        public dynamic notes { get; private set; }

        public boq_wiring()
        {
            InitializeComponent();
        }

        private void BtnToogle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;
            btn_search.Visible = !isEdit;

            pnl_header.Enabled = isEdit;
            pnl_result.Enabled = isEdit;
            dg_boq.Enabled = isEdit;
        }


        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            Helpers.ResetControls(pnl_result);
            dg_boq.ClearSelection();
            BtnToogle(true);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            Helpers.ResetControls(pnl_result);
            BtnToogle(false);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
        }

        private async void boq_Load(object sender, EventArgs e)
        {

            await LoadDataAsync();
            UpdateUI();
            FilterComponentsByItemSetName();
            BtnToogle(false);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Call service that returns both lists
                ProjectComponentResponse = await BoqServices.GetAsDatatable();

                if (ProjectComponentResponse != null)
                {
                    if (ProjectComponentResponse.project_component != null)
                        ProjectComponent = JsonHelper.ToDataTable(ProjectComponentResponse.project_component);

                    if (ProjectComponentResponse.sales_project_wiring != null)
                        SalesProjectWiring = JsonHelper.ToDataTable(ProjectComponentResponse.sales_project_wiring);
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
            //try
            //{
            //    var ProjectComponentResponse = await BoqServices.GetAsDatatable();

            //    if (ProjectComponentResponse != null && ProjectComponentResponse.project_component != null)
            //    {
            //        ProjectComponent = JsonHelper.ToDataTable(ProjectComponentResponse.project_component);
            //    }
            //    else
            //    {
            //        MessageBox.Show("No ProjectComponent data available.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error fetching data: {ex.Message}");
            //}
        }



        private void UpdateUI()
        {
            if (ProjectComponent != null && ProjectComponent.Rows.Count > 0)
            {
                BindDataToPanel();

                TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;

                if (txtProjectName != null && txtItemName != null && txtCustomer != null)
                {
                    txtProjectName.Text = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
                    txtItemName.Text = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
                    txtCustomer.Text = ProjectComponent.Rows[selectedRecord]["customer_name"].ToString();
                }

                FilterComponentsByItemSetName();
            }
            else
            {
                MessageBox.Show("ProjectComponent data is not available.");
            }
        }



        //private void FilterComponentsByItemSetName()
        //{
        //    //try
        //    //{
        //    //    string itemSetName = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
        //    //    string projectName = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
        //    //    string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();

        //    //    DataView filteredDataView = new DataView(ProjectComponent);
        //    //    string filterExpression = $"project_name = '{projectName}' AND quotation_id = '{quotationId}' AND item_set_name = '{itemSetName}'";

        //    //    filteredDataView.RowFilter = filterExpression;
        //    //    DataTable filteredTable = filteredDataView.ToTable();
        //    //    dg_boq.DataSource = filteredTable;
        //    //    dg_boq.Refresh();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show($"Error while filtering data: {ex.Message}");
        //    //}
        //    try
        //    {
        //        string itemSetName = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
        //        string projectName = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
        //        string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();

        //        DataView filteredDataView = new DataView(ProjectComponent);

        //        string filterExpression = $"project_name = '{projectName}' AND quotation_id = '{quotationId}' AND item_set_name = '{itemSetName}'";

        //        filterExpression += " AND (bom_id = 0 AND item_id = 0 AND model <> ''"
        //                                  + " OR (bom_id > 0 AND item_id > 0)"
        //                                  + " OR (item_id > 0 AND bom_id = 0))";



        //        filteredDataView.RowFilter = filterExpression;

        //        DataTable filteredTable = filteredDataView.ToTable();

        //        dg_boq.DataSource = filteredTable;
        //        dg_boq.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error while filtering data: {ex.Message}");
        //    }
        //}

        //private void FilterComponentsByItemSetName()
        //{
        //    try
        //    {
        //        string itemSetName = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
        //        string projectName = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
        //        string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();

        //        DataView filteredDataView = new DataView(ProjectComponent);

        //        string filterExpression = $"project_name = '{projectName}' AND quotation_id = '{quotationId}' AND item_set_name = '{itemSetName}'";

        //        filterExpression += " AND ( (bom_id = 0 AND item_id = 0 AND (node_type = 'Parent' OR model <> ''))"
        //                            + " OR (bom_id > 0 AND item_id > 0 AND model = 0) )";

        //        filteredDataView.RowFilter = filterExpression;

        //        DataTable filteredTable = filteredDataView.ToTable();


        //        foreach (DataRow row in filteredTable.Rows)
        //        {
        //            if (row["bom_id"].ToString() == "0" && row["item_id"].ToString() == "0" && row["node_type"].ToString() == "Parent")
        //            {
        //                if (filteredTable.Columns.Contains("size"))
        //                    row["size"] = DBNull.Value;
        //                if (filteredTable.Columns.Contains("qty"))
        //                    row["qty"] = DBNull.Value;
        //                if (filteredTable.Columns.Contains("component_total"))
        //                    row["component_total"] = DBNull.Value;
        //                if (filteredTable.Columns.Contains("unit_of_measure"))
        //                    row["unit_of_measure"] = DBNull.Value;
        //            }

        //        }

        //        dg_boq.DataSource = filteredTable;
        //        dg_boq.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error while filtering data: {ex.Message}");
        //    }
        //}






        private void FilterComponentsByItemSetName()
        {
            try
            {
                string itemSetName = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
                string projectName = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
                string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();

                DataView filteredDataView = new DataView(ProjectComponent);

                string filterExpression = $"project_name = '{projectName}' AND quotation_id = '{quotationId}' AND item_set_name = '{itemSetName}'";

                filterExpression += " AND ( (bom_id = 0 AND item_id = 0 AND (node_type = 'Parent' OR model <> ''))"
                                    + " OR (bom_id > 0 AND item_id > 0 AND model = 0) )";

                filteredDataView.RowFilter = filterExpression;

                DataTable filteredTable = filteredDataView.ToTable();

                if (!filteredTable.Columns.Contains("number"))
                {
                    filteredTable.Columns.Add("number", typeof(string));
                }


                int bomCounter = 0;
                int bomDetailIndex = 1;


                foreach (DataRow row in filteredTable.Rows)
                {
                    if (row["item_id"].ToString() == "0" && row["bom_id"].ToString() == "0" && !string.IsNullOrEmpty(row["model"].ToString()))
                    {
                        bomCounter += 1;
                        row["number"] = bomCounter.ToString();
                        if (filteredTable.Columns.Contains("item_code"))
                            row["item_code"] = row["short_desc"].ToString();

                        bomDetailIndex = 1;
                    }
                    else if (Convert.ToInt32(row["bom_id"]) > 0 && Convert.ToInt32(row["item_id"]) > 0)
                    {
                        row["number"] = $"{bomCounter}.{bomDetailIndex}";
                        bomDetailIndex += 1;
                        if (filteredTable.Columns.Contains("item_code"))
                        {
                            row["item_code"] = "Item " + row["item_id"].ToString();
                        }
                    }

                    if (row["bom_id"].ToString() == "0" && row["item_id"].ToString() == "0" && row["node_type"].ToString() == "Parent")
                    {
                        if (filteredTable.Columns.Contains("size"))
                            row["size"] = DBNull.Value;
                        if (filteredTable.Columns.Contains("qty"))
                            row["qty"] = DBNull.Value;
                        if (filteredTable.Columns.Contains("component_total"))
                            row["component_total"] = DBNull.Value;
                        if (filteredTable.Columns.Contains("unit_of_measure"))
                            row["unit_of_measure"] = DBNull.Value;
                    }
                }
                //dg_boq.DataSource = filteredTable;
                //dg_wiring.DataSource = filteredTable;

                //dg_boq.Refresh();
                //dg_wiring.Refresh();
                dg_boq.DataSource = filteredTable;

                if (SalesProjectWiring != null)
                    dg_wiring.DataSource = SalesProjectWiring;

                dg_boq.Refresh();
                dg_wiring.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while filtering data: {ex.Message}");
            }
        }



        private void BindDataToPanel()
        {
            if (ProjectComponent != null && ProjectComponent.Rows.Count > 0)
            {
                TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;

                if (txtProjectName != null && txtItemName != null && txtCustomer != null && ProjectComponent.Rows.Count > 0)
                {
                    txtProjectName.Text = ProjectComponent.Rows[selectedRecord]["project_name"].ToString();
                    txtItemName.Text = ProjectComponent.Rows[selectedRecord]["item_set_name"].ToString();
                    txtCustomer.Text = ProjectComponent.Rows[selectedRecord]["customer_name"].ToString();
                }
            }
        }

        private void dg_boq_SelectionChanged(object sender, EventArgs e)
        {
            if (dg_boq.SelectedRows.Count > 0)
            {
                var selectedRow = dg_boq.SelectedRows[0];
                int selectedIndex = selectedRow.Index;
                this.selectedRecord = selectedIndex;

                BindDataToPanel();

                TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;

                if (txtProjectName != null && txtItemName != null && txtCustomer != null)
                {
                    txtProjectName.Text = ProjectComponent.Rows[selectedIndex]["project_name"].ToString();
                    txtItemName.Text = ProjectComponent.Rows[selectedIndex]["item_set_name"].ToString();
                    txtCustomer.Text = ProjectComponent.Rows[selectedIndex]["customer_name"].ToString();
                }

                //UpdateUI();

            }


        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (ProjectComponent == null || ProjectComponent.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] columnsToShow = { "project_name", "item_set_name", "customer_name" };

            using (BoqSearch boqSearchModal = new BoqSearch("BOQ Search", ProjectComponent, columnsToShow))
            {
                if (boqSearchModal.ShowDialog() == DialogResult.OK)
                {
                    int selectedIndex = boqSearchModal.SelectedIndex;
                    if (selectedIndex >= 0)
                    {
                        selectedRecord = selectedIndex;

                        string selectedProjectName = ProjectComponent.Rows[selectedIndex]["project_name"].ToString();
                        string selectedItemName = ProjectComponent.Rows[selectedIndex]["item_set_name"].ToString();
                        string selectedCustomerName = ProjectComponent.Rows[selectedIndex]["customer_name"].ToString();

                        BindDataToPanel();
                        FilterComponentsByItemSetName();

                        TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                        TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                        TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;

                        if (txtProjectName != null && txtItemName != null && txtCustomer != null)
                        {
                            txtProjectName.Text = selectedProjectName;
                            txtItemName.Text = selectedItemName;
                            txtCustomer.Text = selectedCustomerName;
                        }
                    }
                }
            }
        }





        //private void btn_next_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string itemSetBasedOnQuotationId = ProjectComponent.Rows[selectedRecord]["item_set_based_on_quotation_id"].ToString();
        //        string currentSetId = ProjectComponent.Rows[selectedRecord]["set_id"].ToString(); // Get the current set_id

        //        // Filter rows that match the current item_set_based_on_quotation_id
        //        DataView filteredDataView = new DataView(ProjectComponent);
        //        filteredDataView.RowFilter = $"item_set_based_on_quotation_id = '{itemSetBasedOnQuotationId}'";
        //        DataTable filteredTable = filteredDataView.ToTable();

        //        // Find the next set_id in the filtered set
        //        bool foundNext = false;
        //        for (int i = selectedRecord + 1; i < filteredTable.Rows.Count; i++)
        //        {
        //            if (filteredTable.Rows[i]["set_id"].ToString() != currentSetId)
        //            {
        //                selectedRecord = i; // Move to the next set_id
        //                foundNext = true;
        //                break;
        //            }
        //        }

        //        if (foundNext)
        //        {
        //            UpdateUI(filteredTable);
        //        }
        //        else
        //        {
        //            MessageBox.Show("You are already on the last set.", "End of Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error during navigation: {ex.Message}");
        //    }

        //}

        //private void btn_prev_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string itemSetBasedOnQuotationId = ProjectComponent.Rows[selectedRecord]["item_set_based_on_quotation_id"].ToString();
        //        string currentSetId = ProjectComponent.Rows[selectedRecord]["set_id"].ToString(); // Get the current set_id

        //        // Filter rows that match the current item_set_based_on_quotation_id
        //        DataView filteredDataView = new DataView(ProjectComponent);
        //        filteredDataView.RowFilter = $"item_set_based_on_quotation_id = '{itemSetBasedOnQuotationId}'";
        //        DataTable filteredTable = filteredDataView.ToTable();

        //        // Find the previous set_id in the filtered set
        //        bool foundPrev = false;
        //        for (int i = selectedRecord - 1; i >= 0; i--)
        //        {
        //            if (filteredTable.Rows[i]["set_id"].ToString() != currentSetId)
        //            {
        //                selectedRecord = i; // Move to the previous set_id
        //                foundPrev = true;
        //                break;
        //            }
        //        }

        //        if (foundPrev)
        //        {
        //            UpdateUI(filteredTable);
        //        }
        //        else
        //        {
        //            MessageBox.Show("You are already on the first set.", "Start of Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error during navigation: {ex.Message}");
        //    }
        //}    

        private void btn_next_Click(object sender, EventArgs e)
        {
            try
            {
                string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();
                string currentSetId = ProjectComponent.Rows[selectedRecord]["set_id"].ToString();

                // Filter rows based on the current quotation_id
                DataView filteredDataView = new DataView(ProjectComponent);
                filteredDataView.RowFilter = $"quotation_id = '{quotationId}'";
                DataTable filteredTable = filteredDataView.ToTable();

                bool foundNext = false;

                // Loop through the filtered rows starting from the current index to find the next different set_id
                for (int i = selectedRecord + 1; i < filteredTable.Rows.Count; i++)
                {
                    if (filteredTable.Rows[i]["set_id"].ToString() != currentSetId)
                    {
                        selectedRecord = i;  // Move to the next different set_id
                        foundNext = true;
                        break;
                    }
                }

                if (foundNext)
                {
                    UpdateUI(filteredTable);
                }
                else
                {
                    MessageBox.Show("You are already on the last set.", "End of Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during navigation: {ex.Message}");
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            try
            {
                string quotationId = ProjectComponent.Rows[selectedRecord]["quotation_id"].ToString();
                string currentSetId = ProjectComponent.Rows[selectedRecord]["set_id"].ToString();

                // Filter rows based on the current quotation_id
                DataView filteredDataView = new DataView(ProjectComponent);
                filteredDataView.RowFilter = $"quotation_id = '{quotationId}'";
                DataTable filteredTable = filteredDataView.ToTable();

                bool foundPrev = false;

                // Loop through the filtered rows starting from the current index to find the previous different set_id
                for (int i = selectedRecord - 1; i >= 0; i--)
                {
                    if (filteredTable.Rows[i]["set_id"].ToString() != currentSetId)
                    {
                        selectedRecord = i;  // Move to the previous different set_id
                        foundPrev = true;
                        break;
                    }
                }

                if (foundPrev)
                {
                    UpdateUI(filteredTable);
                }
                else
                {
                    MessageBox.Show("You are already on the first set.", "Start of Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during navigation: {ex.Message}");
            }
        }


        private void UpdateUI(DataTable filteredTable)
        {
            if (filteredTable.Rows.Count > 0)
            {
                // Bind data to the panel controls (txt_project_name, txt_item_name, txt_customer)
                BindDataToPanel();

                // Update text fields for the selected record
                TextBox txtProjectName = pnl_header.Controls["txt_project_name"] as TextBox;
                TextBox txtItemName = pnl_header.Controls["txt_item_name"] as TextBox;
                TextBox txtCustomer = pnl_header.Controls["txt_customer"] as TextBox;

                if (txtProjectName != null && txtItemName != null && txtCustomer != null)
                {
                    // Get the project details of the selected record and update the UI
                    txtProjectName.Text = filteredTable.Rows[selectedRecord]["project_name"].ToString();
                    txtItemName.Text = filteredTable.Rows[selectedRecord]["item_set_name"].ToString();
                    txtCustomer.Text = filteredTable.Rows[selectedRecord]["customer_name"].ToString();
                }
            }
            else
            {
                MessageBox.Show("No records found for the selected set.", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }



        private void dg_boq_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dg_boq.Rows[e.RowIndex].DataBoundItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;

                if (row["node_type"].ToString() == "Parent")
                {
                    dg_boq.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dg_boq.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(dg_boq.Font, FontStyle.Bold);
                    //dg_boq.Rows[e.RowIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center align text

                }
            }
        }

        private void dg_boq_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dg_boq.Rows[e.RowIndex].DataBoundItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;

                if (row["node_type"].ToString() == "Parent")
                {
                    e.Handled = true;
                    e.PaintBackground(e.ClipBounds, true);

                    using (Brush textBrush = new SolidBrush(dg_boq.ForeColor))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;

                        RectangleF textRect = new RectangleF(
                            dg_boq.GetCellDisplayRectangle(0, e.RowIndex, true).Left + 5,
                            e.CellBounds.Top,
                            dg_boq.ClientSize.Width,
                            e.CellBounds.Height
                        );

                        e.Graphics.DrawString(row["components"].ToString(), dg_boq.Font, textBrush, textRect, sf);
                    }

                    using (Pen rowBorderPen = new Pen(dg_boq.GridColor))
                    using (Brush backgroundBrush = new SolidBrush(dg_boq.Rows[e.RowIndex].DefaultCellStyle.BackColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.CellBounds);
                        e.Graphics.DrawLine(rowBorderPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    }
                }
            }
        }



        private async void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dg_boq.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.DataBoundItem is BoqDetailModel model)
                    {
                        if (string.IsNullOrWhiteSpace(model.remarks) && string.IsNullOrWhiteSpace(model.notes))
                            continue;

                        var data = new Dictionary<string, dynamic>
                {
                    { "remarks", model.remarks ?? "" },
                    { "notes", model.notes ?? "" }
                };

                        var response = await BoqNotesServices.Insert(data);

                        Console.WriteLine($"Response: Success={response.Success}, Message={response.Message}");

                        if (!response.Success)
                        {
                            MessageBox.Show($"Save failed: {response.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}