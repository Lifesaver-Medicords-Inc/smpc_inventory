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

namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_social_media_setup : UserControl
    {
        
     
        public frm_social_media_setup()
        {
            InitializeComponent();
        }
        private void frm_social_media_setup_Load(object sender, EventArgs e)
        {
            GetSocialMedia();
        }


        private void BtnToogle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_delete.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_cancel.Visible = isEdit;
            panel_records.Enabled = isEdit;
        }
        private async void GetSocialMedia()
        {
           
            var data = await SocialMediaServices.GetAsDatatable();
            dg_social_media.DataSource = data;

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            dg_social_media.ClearSelection();
            BtnToogle(true);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
           BtnToogle(true);
           dg_social_media.ClearSelection();
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(panel_records);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this data? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool isSuccess = await SocialMediaServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Social Media Name Succesfully");
                GetSocialMedia();
                BtnToogle(false);
            }
        }


        private async void btn_save_Click(object sender, EventArgs e)
        {
            string message;
            //  bool isSuccess;
            string errorFieldMessage;
            ApiResponseModel response = new ApiResponseModel();

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
                response = await SocialMediaServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add social media name \n" + response.message;
            }
            else
            {
                response = await SocialMediaServices.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update social media name";
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }

            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetSocialMedia();
            BtnToogle(false);

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

        private void dg_social_media_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Panel[] pnlList = { panel_records };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_social_media);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
