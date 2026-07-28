using System.Windows.Forms;
using smpc_inventory_app.Pages.Item;

namespace smpc_inventory_app.Pages.Inventory
{
    // frm_Item_Entry is a UserControl (it's normally opened as a tab inside the main Layout
    // via RouteServices), not a standalone dialog. This just hosts it inside a plain,
    // resizable window so it can be opened as a "New Item" popup from ItemStockAddModal
    // (and anywhere else a quick standalone Item Entry window is useful) without having to
    // reach into Layout's tab system from a modal Form.
    public class ItemEntryHostForm : Form
    {
        public ItemEntryHostForm()
        {
            var itemEntry = new frm_Item_Entry
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(itemEntry);
            this.Text = "Item Entry";
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new System.Drawing.Size(1000, 700);
        }
    }
}
