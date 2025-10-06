using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Inventory.InventoryTrackerModals
{
    public partial class InventoryTrackerLocation : Form
    {
        public InventoryTrackerLocation(IEnumerable<object> details)
        {
            InitializeComponent();

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;

            dgv_inventory_location.DataSource = details.ToList();
        }
    }
}
