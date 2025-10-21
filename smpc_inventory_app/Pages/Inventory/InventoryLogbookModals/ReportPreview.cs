using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace smpc_inventory_app.Pages.Inventory.InventoryLogbookModals
{
    public partial class ReportPreview : Form
    {
        public string FileName { get; set; }
        public string ReportCode { get; set; }
        public string TempFilePath { get; set; }
        public List<string> ColumnList { get; set; }
        public List<string> BrandList { get; set; }
        public List<string> ItemCategoryList { get; set; }
        public List<string> GeneralNameList { get; set; }

        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        //Add these Win32 constants
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;
        public ReportPreview()
        {
            InitializeComponent();

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportPreview_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TempFilePath) || !File.Exists(TempFilePath))
                {
                    MessageBox.Show("No file found to preview.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                // Start Excel
                excelApp = new Excel.Application();
                excelApp.Visible = false;

                // Open the file in read-only mode
                workbook = excelApp.Workbooks.Open(TempFilePath, ReadOnly: true);

                // Configure Excel for embedding
                excelApp.DisplayFormulaBar = false;
                excelApp.DisplayStatusBar = false;

                // Get Excel window handle
                IntPtr excelHandle = (IntPtr)excelApp.Hwnd;

                // Reparent Excel window to our panel
                SetParent(excelHandle, pnl_preview.Handle);

                // Simple resize to fit panel
                SetWindowPos(excelHandle, IntPtr.Zero, 0, 0, pnl_preview.Width, pnl_preview.Height,
                    SWP_NOZORDER | SWP_NOACTIVATE);

                // Set zoom to fit
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                int zoomLevel = Math.Max(50, Math.Min(120, pnl_preview.Width / 10));
                worksheet.Application.ActiveWindow.Zoom = zoomLevel;

                // Show Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying Excel preview: " + ex.Message, "Preview Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            PopulatePanelWithCheckboxes(pnl_column, ColumnList);

            // Combine the lists
            var combinedFilterList = new List<string>();
            if (GeneralNameList != null) combinedFilterList.AddRange(GeneralNameList);
            if (ItemCategoryList != null) combinedFilterList.AddRange(ItemCategoryList);
            if (BrandList != null) combinedFilterList.AddRange(BrandList);

            PopulatePanelWithCheckboxes(pnl_filter, combinedFilterList);

            txt_doc_no.Text = ReportCode;
            chb_report_title.Text = FileName;
        }

        private void ReportPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                workbook?.Close(false);
                excelApp?.Quit();

                // Clean up temp file
                if (File.Exists(TempFilePath))
                {
                    File.SetAttributes(TempFilePath, FileAttributes.Normal);
                    File.Delete(TempFilePath);
                }
            }
            catch { }
        }

        private void PopulatePanelWithCheckboxes(Panel targetPanel, IEnumerable<string> values)
        {
            if (values == null || !values.Any())
                return;

            targetPanel.Controls.Clear(); // clear any previous items

            int yPos = 5; // starting Y position
            int spacing = 5;

            foreach (var name in values)
            {
                Panel itemPanel = new Panel
                {
                    Width = targetPanel.Width - 40,
                    Height = 25,
                    BackColor = Color.White,
                    Tag = name
                };

                Label lbl = new Label
                {
                    Text = name,
                    AutoSize = false,
                    Location = new Point(5, 3),
                    Width = itemPanel.Width - 60,
                    Height = 20,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                itemPanel.Controls.Add(lbl);

                itemPanel.Location = new Point(5, yPos);
                yPos += itemPanel.Height + spacing;

                targetPanel.Controls.Add(itemPanel);
            }
        }

        private void pnl_preview_Resize(object sender, EventArgs e)
        {
            if (excelApp != null)
            {
                IntPtr excelHandle = (IntPtr)excelApp.Hwnd;
                SetWindowPos(excelHandle, IntPtr.Zero, 0, 0, pnl_preview.Width, pnl_preview.Height,
                    SWP_NOZORDER | SWP_NOACTIVATE);

                try
                {
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    int zoomLevel = Math.Max(50, Math.Min(120, pnl_preview.Width / 10));
                    worksheet.Application.ActiveWindow.Zoom = zoomLevel;
                }
                catch { }
            }
        }
    }
}
