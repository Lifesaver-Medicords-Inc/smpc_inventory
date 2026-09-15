using Inventory_SMPC.Pages.Item;
using smpc_inventory_app.Pages.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Business_Partner_Info.Bpi_Modal
{
    public partial class ItemEntryModal : Form
    {
        public ItemEntryModal()
        {
            InitializeComponent();


        }



        public delegate void getBpiAddedItem(Dictionary<string,dynamic> value);

        public event getBpiAddedItem OnAddItem;

        private void ItemEntryModal_Load(object sender, EventArgs e)
        {
            frm_Item_Entry itemEntry = new frm_Item_Entry();
            itemEntry.OnItem += GetAddedItem;

            // Hosted the way the Inventory sidebar hosts it (Layout.ShowForm): the page keeps
            // its own designed size and the window scrolls when it is smaller. It used to be
            // docked to fill a fixed 1221x711 window, about 150px shorter than the page, so
            // every control anchored to the bottom was squeezed - the long description box
            // collapsed to nothing - and the page looked unlike the Item Entry opened from the
            // sidebar (user-reported 2026-09-14). It is the same page and the same class; only
            // the window around it was wrong.
            itemEntry.Location = new Point(0, 0);
            this.Controls.Add(itemEntry);

            // As large as the page needs, within the screen the dialog opens on.
            Rectangle screen = Screen.FromControl(this).WorkingArea;
            Size chrome = this.Size - this.ClientSize;
            int width = Math.Min(itemEntry.Width + chrome.Width + SystemInformation.VerticalScrollBarWidth, screen.Width);
            int height = Math.Min(itemEntry.Height + chrome.Height + SystemInformation.HorizontalScrollBarHeight, screen.Height);
            this.Size = new Size(width, height);
            this.Location = new Point(screen.Left + (screen.Width - width) / 2, screen.Top + (screen.Height - height) / 2);

            // ADD SUPPLIER stays hidden, as before: in this flow the supplier is the partner
            // whose Items tab opened the dialog.
            itemEntry.HideButton();
        }
     

        public void btnItemAdd_Click(object sender, EventArgs e)
        {

        }

        public void GetAddedItem(Dictionary<string,dynamic> value)
        {
            this.Close();
            OnAddItem.Invoke(value);


        }
    }
}
