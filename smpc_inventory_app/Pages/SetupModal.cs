using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
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
    public partial class SetupModal : Form
    {

        GeneralSetupServices serviceSetup;
        private string url { get; }
        private string title { get; }
        private bool showSelectedField; 
        private DataTable dataTable { get; set; }

        public SetupModal(string setupTitle, string api, DataTable dt, bool isVisible = false)
        {
            InitializeComponent();
            lbl_setup_title.Text = setupTitle;
            this.url = api;
            this.title = setupTitle;
            this.showSelectedField = isVisible;
            //if (dt != null){
            //    if (dt.Columns["select"] != null) {    // Check if select column already exist
            //        dt.Columns.Remove("select");            // Remove select column if exist 
            //        return;
            //    }
            //}
            this.dataTable = dt;


        }


        //Load of Data
        private void SetupModal_Load(object sender, EventArgs e)
        {
            dg_setup.DataSource = this.dataTable;
            dg_setup.Columns["is_selected"].Visible = this.showSelectedField;

        }


        // Fetch Setup
        private async void GetSetup()
        {
            serviceSetup = new GeneralSetupServices(this.url);
            var data = await serviceSetup.GetAsDatatable();
            dg_setup.DataSource = data;

        }
        private void  BtnToogle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_save.Visible = isEdit;
            btn_cancel.Visible = isEdit;
            panel_records.Enabled = isEdit;
        }
        private bool ValidateField(out string messages)
        {
            bool isValid = false;
            messages = string.Empty;

            if (string.IsNullOrEmpty(txt_code.Text))
            {
                messages += "Code cannot be empty \n";
                isValid = true;
            }

            if (string.IsNullOrEmpty(txt_name.Text))
            {
                messages += "Name cannot be empty \n";
                isValid = true;
            }

            return isValid;
        }

        private void dg_setup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Panel[] pnlList = { panel_records };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_setup);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
            btn_edit.Enabled = true;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(true);
            dg_setup.ClearSelection();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            string message;
           
            string errorFieldMessage;
            ApiResponseModel response = new ApiResponseModel();
            serviceSetup = new GeneralSetupServices(this.url);


            bool isErrorField = ValidateField(out errorFieldMessage);

            if (isErrorField)
            {
                Helpers.ShowDialogMessage("error", errorFieldMessage);
                return;
            }

            var data = Helpers.GetControlsValues(panel_records);

            if (txt_id.Text.Equals(""))
            {
                data.Remove("id");
                response = await serviceSetup.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add" + this.title +"\n" + response.message;
            }
            else
            {
                response = await serviceSetup.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update " + this.title;
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }
            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetSetup();
            BtnToogle(false);

        }

        private void panel_header_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dg_setup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
