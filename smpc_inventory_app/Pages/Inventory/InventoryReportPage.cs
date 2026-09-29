using smpc_inventory_app.Pages.Inventory.InventoryLogbookModals;
using System.Drawing;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Inventory
{
    // The Inventory Report builder (spec 10.3) as a page in a tab (user 2026-09-29: "Inventory
    // report not suppose a modal"). It opened as a dialog over everything, from the sidebar's
    // Inventory Report List and from the Logbook's MAKE REPORT.
    //
    // The builder itself is unchanged - it is a Form, shown here as a child window without its
    // frame, so its checkboxes, filters, PREVIEW and CREATE behave exactly as they did. Only the
    // Excel preview it opens is still a dialog: that one looks at a generated file, not at the
    // builder.
    public class InventoryReportPage : UserControl
    {
        private readonly InventoryReport _builder;

        // year and month preselect the period, as the Logbook's MAKE REPORT does; the sidebar
        // entry leaves both for the user to pick.
        public InventoryReportPage(string year = null, string month = null)
        {
            _builder = new InventoryReport
            {
                _selectedYear = year,
                _selectedMonth = month,
                TopLevel = false,
            };

            // Kept at the size it was designed at, top left, not docked to fill the tab: its
            // PREVIEW and CREATE buttons are anchored to all four edges and stretched into
            // half-page blocks when the builder was made as large as the tab.
            Size designed = _builder.ClientSize;
            _builder.FormBorderStyle = FormBorderStyle.None;
            _builder.Location = new Point(0, 0);
            _builder.Size = designed;

            // The page's natural size is the builder's, so a smaller window scrolls it rather
            // than squeezing it (Helpers.PageFit).
            Size = designed;
            BackColor = _builder.BackColor;

            Controls.Add(_builder);
            _builder.Show();
        }

        public InventoryReport Builder => _builder;
    }
}
