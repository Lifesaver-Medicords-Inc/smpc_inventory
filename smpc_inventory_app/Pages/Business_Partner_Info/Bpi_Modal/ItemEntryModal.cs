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

            itemEntry.Dock = DockStyle.Fill;
            this.Controls.Add(itemEntry);

           

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
