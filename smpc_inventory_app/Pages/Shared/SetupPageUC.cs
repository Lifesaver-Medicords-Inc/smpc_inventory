using smpc_inventory_app.Services.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Shared
{
    public partial class SetupPageUC: UserControl
    {
        private DataTable _setupDt;
        private readonly object _setupService;

        public SetupPageUC(string pageTitle, DataTable setupDt, object setupService)
        {
            InitializeComponent();
            
            _setupDt = setupDt;
            _setupService = setupService;
        }

        private async void GetData()
        {
          
        }
        private void BtnToggle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_edit.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            pnl_input.Enabled = isEdit;
            dg_items.Enabled = !isEdit;
        }
    }
}
