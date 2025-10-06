using Microsoft.Reporting.WinForms;
using Newtonsoft.Json.Linq;
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

namespace smpc_inventory_app.Pages.Engineering
{
    public partial class EngineeringPrintModal : Form
    {
        ApiResponseModel response;
        public DataTable quotations { get; set; } = new DataTable();
        DataTable ProjectComponent;
        ProjectComponentClass ProjectComponentResponse;
        public DataTable WiringNotes { get; set; } = new DataTable();
        public DataTable QQData { get; set; } = new DataTable();
        int ID = 0;
        bool isQQ = false;
        public EngineeringPrintModal(int ID = 0, bool isQQ = false)
        {
            InitializeComponent();
            this.ID = ID;
            this.isQQ = isQQ;
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
        public DataTable FilteredProject { get; set; } = new DataTable();
        public DataTable FilteredWiring { get; set; } = new DataTable();
        private void fetchBOQ(int selectedItemSetId)
        {
            var filteredRows = from row in ProjectComponent.AsEnumerable()
                               where row.Field<int>("set_id") == selectedItemSetId
                               select row;

            if (filteredRows.Any())
            {
                DataTable filteredtable = filteredRows.CopyToDataTable();
                double bomCounter = 0;
                int bomDetailIndex = 1;
                int secondbomDetailIndex = 1;
                FilteredProject = filteredtable.Clone();
                FilteredProject.Columns.Add("number", typeof(string));
                foreach (DataRow row in filteredtable.Rows)
                {
                    DataRow newRow = FilteredProject.NewRow();
                    foreach (DataColumn col in filteredtable.Columns)
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
                        FilteredProject.Rows.Add(newRow);
                        if (bomDetailIndex > 1)
                        {
                            bomDetailIndex = 1;
                            secondbomDetailIndex = 1;
                        }
                    }
                    if (itemId == 0 && bomId == 0 && !string.IsNullOrEmpty(model))
                    {
                        newRow["number"] = $"{bomCounter}.{bomDetailIndex}";
                        FilteredProject.Rows.Add(newRow);
                        bomDetailIndex += 1;
                    }
                    // CHECKER IF THE ROW IS AN ITEM / ACCESSORIES
                    if (itemId > 0 && bomId == 0)
                    {
                        bomCounter += 1;
                        newRow["number"] = bomCounter;
                        FilteredProject.Rows.Add(newRow);
                    }
                    // CHECKER IF THE ROW IS AN ITEM OF A BOM
                    else if (bomId > 0 && itemId > 0)
                    {
                        newRow["number"] = $"{bomCounter}.{bomDetailIndex}.{secondbomDetailIndex}";
                        FilteredProject.Rows.Add(newRow);
                        secondbomDetailIndex += 1;
                    }
                }
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
                DataTable FilteredTable = filteredRows.CopyToDataTable();
                FilteredWiring = FilteredTable.Clone();
                FilteredWiring.Columns.Add("number", typeof(string));
                FilteredWiring.Columns.Add("uom1", typeof(string));
                FilteredWiring.Columns.Add("uom2", typeof(string));

                List<string> uomValues = new List<string> { "m", "pcs", "pcs", "pcs", "m", "pcs", "m", "m" };
                int counter = 1;
                foreach (DataRow row in FilteredTable.Rows)
                {
                    DataRow newRow = FilteredWiring.NewRow();
                    foreach (DataColumn col in FilteredTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName];
                    }
                    newRow["number"] = counter;
                    int index = (counter - 1) % uomValues.Count;
                    newRow["uom1"] = uomValues[index];
                    newRow["uom2"] = uomValues[index];
                    counter++;
                    FilteredWiring.Rows.Add(newRow);
                }
            }
        }
        public DataTable FilteredQQ { get; set; } = new DataTable();
        private void fetchQQ(string quotationid)
        {
            int.TryParse(quotationid, out int id);
            var filteredRows = from row in QQData.AsEnumerable()
                               where row["based_id"] != DBNull.Value && Convert.ToInt32(row["based_id"]) == id
                               select row;

            DataTable filteredTable = filteredRows.CopyToDataTable();
            FilteredQQ = filteredTable.Clone();
            FilteredQQ.Columns.Add("number", typeof(string));
            double bomCounter = 0;
            int bomDetailIndex = 1;

            foreach (DataRow row in filteredTable.Rows)
            {
                DataRow newRow = FilteredQQ.NewRow();
                foreach (DataColumn col in filteredTable.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName];
                }

                bool ischild = Convert.ToBoolean(row["is_child"]);

                if (ischild)
                {
                    newRow["number"] = $"{bomCounter}.{bomDetailIndex}";
                    FilteredQQ.Rows.Add(newRow);
                    bomDetailIndex += 1;
                }
                else
                {
                    bomCounter += 1;
                    newRow["number"] = bomCounter;
                    FilteredQQ.Rows.Add(newRow);
                    if (bomDetailIndex > 1)
                    {
                        bomDetailIndex = 1;
                    }
                }
            }
        }
        public bool AutoExport { get; set; } = false;
        public string ExportPath { get; set; } = "";
        private async void EngineeringPrintModal_Load(object sender, EventArgs e)
        {
            await fetchData();
            
            if (isQQ)
            {
                fetchQQ(ID.ToString());
                if (FilteredQQ.Rows.Count > 0)
                {
                    var parameters = new List<ReportParameter>();
                    parameters.Add(new ReportParameter("Customer", FilteredQQ.Rows[0]["customer_name"].ToString()));
                    parameters.Add(new ReportParameter("Reference", "Q#" + ID));

                    var numberValues = FilteredQQ.AsEnumerable()
                    .Select(row => row["number"].ToString())
                    .Distinct()
                    .ToArray();

                    parameters.Add(new ReportParameter("NumberValues", numberValues));

                    ReportDataSource headerReportDataSource = new ReportDataSource("DataSet1", FilteredQQ);
                    reportViewer1.LocalReport.ReportPath = Path.Combine(Settings.Default.REPORTPATH, "BOQQuickQuote.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(headerReportDataSource);
                    reportViewer1.LocalReport.SetParameters(parameters);
                    this.reportViewer1.RefreshReport();
                    if (AutoExport && !string.IsNullOrWhiteSpace(ExportPath))
                    {
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType, encoding, extension;

                        byte[] pdfBytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        File.WriteAllBytes(ExportPath, pdfBytes);

                        // Optionally close the form after exporting if shown manually
                        this.Close();
                    }
                }
            }
            else {
                fetchBOQ(ID);
                if (FilteredProject.Rows.Count > 0)
                {
                    var parameters = new List<ReportParameter>();

                    // Add single value parameters
                    parameters.Add(new ReportParameter("ProjectName", FilteredProject.Rows[0]["project_name"].ToString()));
                    parameters.Add(new ReportParameter("ItemName", FilteredProject.Rows[0]["item_set_name"].ToString()));
                    parameters.Add(new ReportParameter("Customer", FilteredProject.Rows[0]["customer_name"].ToString()));
                    parameters.Add(new ReportParameter("Reference", "Q#" + FilteredProject.Rows[0]["quotation_id"].ToString()));

                    var numberValues = FilteredProject.AsEnumerable()
                        .Select(row => row["number"].ToString())
                        .Distinct()
                        .ToArray();

                    parameters.Add(new ReportParameter("NumberValues", numberValues));

                    // Similarly for uom values from wiring notes
                    if (FilteredWiring != null && FilteredWiring.Columns.Contains("uom1"))
                    {
                        var uomValues = FilteredWiring.AsEnumerable()
                            .Select(row => row["uom1"].ToString())
                            .ToArray();

                        parameters.Add(new ReportParameter("UOMValues", uomValues));
                    }

                    if (FilteredWiring != null && FilteredWiring.Columns.Contains("number"))
                    {
                        var wiringnumberValues = FilteredWiring.AsEnumerable()
                            .Select(row => row["number"].ToString())
                            .Distinct()
                            .ToArray();

                        parameters.Add(new ReportParameter("WiringNumberValues", wiringnumberValues));
                    }

                    ReportDataSource headerReportDataSource = new ReportDataSource("DataSet1", FilteredProject);
                    ReportDataSource childReportDataSource = new ReportDataSource("DataSet2", FilteredWiring);
                    reportViewer1.LocalReport.ReportPath = Path.Combine(Settings.Default.REPORTPATH, "BOQProject.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(headerReportDataSource);
                    reportViewer1.LocalReport.DataSources.Add(childReportDataSource);
                    reportViewer1.LocalReport.SetParameters(parameters);
                    this.reportViewer1.RefreshReport();

                    if (AutoExport && !string.IsNullOrWhiteSpace(ExportPath))
                    {
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType, encoding, extension;

                        byte[] pdfBytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        File.WriteAllBytes(ExportPath, pdfBytes);

                        // Optionally close the form after exporting if shown manually
                        this.Close();
                    }
                }
            }
        }
        private void DisposeTables()
        {
            quotations?.Dispose(); quotations = null;
            WiringNotes?.Dispose(); WiringNotes = null;
            QQData?.Dispose(); QQData = null;
        }
        private void EngineeringPrintModal_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisposeTables();

            if (reportViewer1 != null)
            {
                reportViewer1.LocalReport.ReleaseSandboxAppDomain();
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.Dispose();
            }

            GC.Collect(); // optional: force immediate cleanup
            GC.WaitForPendingFinalizers();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
