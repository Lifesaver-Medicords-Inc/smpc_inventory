using Inventory_SMPC.Pages.Business_Partner_Info;
using smpc_inventory_app.Pages.Business_Partner_Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Item
{
    public partial class BusnessPartnerInfoModal : Form
    {
        public BusnessPartnerInfoModal()
        {
            InitializeComponent();
        }

        private void BusnessPartnerInfoModal_Load(object sender, EventArgs e)
        {

            BusinessPartnerInfo BPI = new BusinessPartnerInfo("");

            BPI.Dock = DockStyle.Fill;
            this.Controls.Add(BPI);

            BPI.HideButton();
        }
    }
}
