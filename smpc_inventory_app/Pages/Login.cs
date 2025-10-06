using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Auth;
using smpc_inventory_app.Services.Setup.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages
{
    internal partial class Login : Form
    {
        public Login()
        { 
            InitializeComponent();
        }

        private void frm_login_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            //admin
            txt_employee_id.Text = "IT-WD-1";
            txt_password.Text = "IT-WD-1";

            //inv manager
            //txt_employee_id.Text = "IM-IM-25";
            //txt_password.Text = "IM-IM-25";

            //IT dept
            //txt_employee_id.Text = "IT-WD-21";
            //txt_password.Text = "IT-WD-21";

            //btn_login.PerformClick(); //remove this line
        }

        private void txt_password_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btn_login_Click_1(object sender, EventArgs e)
        {


            var data = Helpers.GetControlsValues(pnl_auth);
            data.Add("motherboard_serial_no", Helpers.GetSerialNumber());
            data.Add("machine_name", Environment.MachineName);

            var currentUser = await AuthServices.Login(data);


            if (currentUser.Success)
            {
                CacheData.CurrentUser = currentUser.Data;
                //smpc_sales_system.Services.Sales.Models
                //CacheData.ShipTypeSetup = await ShipService.GetAsDatatable();s
                CacheData.PaymentTerms = await PaymentTermsServices.GetAsDatatable();
                //CacheData.ApplicationSetup = await ApplicationService.GetAsDatatable();
                //CacheData.UoM = await UnitOfMeasurementServices.GetAsDatatable();
                //smpc_sales_app.Pages.Login cacheData = new smpc_sales_app.Pages.Login();
                //cacheData.CacheDataSales();

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                Helpers.ShowDialogMessage("error", "Invalid Credentials");
            }


        }

        private void pnl_auth_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
