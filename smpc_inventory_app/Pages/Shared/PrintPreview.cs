using Microsoft.Reporting.WinForms;
using smpc_inventory_app.Printing.Core;
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

namespace smpc_inventory_app.Pages.Shared
{
    public partial class PrintPreview : Form
    {
        public PrintPreview(IReportProvider provider, bool autoExport = false, string exportPath = null)
        {
            InitializeComponent();
            LoadReport(provider, autoExport, exportPath);
        }
        private void LoadReport(IReportProvider provider, bool autoExport, string exportPath)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (!File.Exists(provider.ReportPath))
                    throw new FileNotFoundException("RDLC not found", provider.ReportPath);

                reportViewer1.LocalReport.ReportPath = provider.ReportPath;
                reportViewer1.LocalReport.DataSources.Clear();

                foreach (var ds in provider.GetDataSources())
                    reportViewer1.LocalReport.DataSources.Add(ds);

                var parameters = provider.GetParameters()?.ToList();
                if (parameters != null && parameters.Any())
                    reportViewer1.LocalReport.SetParameters(parameters);

                reportViewer1.RefreshReport();

                if (autoExport && !string.IsNullOrWhiteSpace(exportPath))
                {
                    ExportPdf(exportPath);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "RDLC ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void ExportPdf(string path)
        {
            byte[] pdf = reportViewer1.LocalReport.Render("PDF");
            File.WriteAllBytes(path, pdf);
        }
    }
}
