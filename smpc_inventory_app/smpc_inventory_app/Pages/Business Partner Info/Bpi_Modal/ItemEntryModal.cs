using Inventory_SMPC.Pages.Item;
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

        private void ItemEntryModal_Load(object sender, EventArgs e)
        {
            frm_Item_Entry itemEntry = new frm_Item_Entry();
            itemEntry.Dock = DockStyle.Fill;
            this.Controls.Add(itemEntry);

            itemEntry.HideButton();
        }
    }
}
