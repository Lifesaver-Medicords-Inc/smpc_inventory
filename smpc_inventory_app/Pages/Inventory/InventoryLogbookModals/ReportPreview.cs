using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Inventory.InventoryLogbookModals
{
    public partial class ReportPreview : Form
    {
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
    }
}
