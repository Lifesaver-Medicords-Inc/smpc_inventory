using Microsoft.Reporting.WinForms;
using smpc_inventory_app.Printing.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Shared
{
    public partial class PrintPreview : Form
    {
        private readonly IReportProvider _provider;
        private readonly bool _autoExport;
        private readonly string _exportPath;

        public PrintPreview(IReportProvider provider, bool autoExport = false, string exportPath = null)
        {
            InitializeComponent();
            _provider = provider;
            _autoExport = autoExport;
            _exportPath = exportPath;
        }

        // async void is required by the event signature; inner exceptions are caught explicitly
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                await LoadReportAsync();
            }
            catch (Exception ex)
            {
                // Fallback: catches anything that escapes LoadReportAsync
                MessageBox.Show(ex.Message, "Unexpected Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadReportAsync()
        {
            // Show wait cursor while data/report loads
            Cursor = Cursors.WaitCursor;

            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                await _provider.InitializeAsync();

                if (!File.Exists(_provider.ReportPath))
                    throw new FileNotFoundException("RDLC file not found.", _provider.ReportPath);

                reportViewer1.LocalReport.ReportPath = _provider.ReportPath;

                // Bind all data sources provided by the report provider
                reportViewer1.LocalReport.DataSources.Clear();
                foreach (var ds in _provider.GetDataSources())
                    reportViewer1.LocalReport.DataSources.Add(ds);

                // Only set parameters if any are provided
                var parameters = _provider.GetParameters()?.ToArray();
                if (parameters?.Length > 0)
                    reportViewer1.LocalReport.SetParameters(parameters);

                reportViewer1.RefreshReport();

                // Auto-export mode: render to PDF then close without showing the viewer
                if (_autoExport && !string.IsNullOrWhiteSpace(_exportPath))
                {
                    ExportPdf(_exportPath);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Report Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ExportPdf(string path)
        {
            try
            {
                byte[] pdf = reportViewer1.LocalReport.Render("PDF");
                File.WriteAllBytes(path, pdf);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF export failed: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}