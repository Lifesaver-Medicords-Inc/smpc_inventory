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
    public partial class frm_entity_type : UserControl
    {
        public frm_entity_type()
        {
            InitializeComponent();
        }

        private async void GetEntityType()
        {
            var data = await EntityServices.GetAsDatatable();
            dg_entity_type.DataSource = data;

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



        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            dg_entity_type.ClearSelection();
            BtnToogle(true);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
            dg_entity_type.ClearSelection();
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(panel_records);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this data? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool isSuccess = await EntityServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Entity Type Succesfully");
                GetEntityType();
                BtnToogle(false);
            }

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

        private void frm_entity_type_Load(object sender, EventArgs e)
        {
            GetEntityType();
        }

        private void dg_entity_type_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Panel[] pnlList = { panel_records };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_entity_type);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
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
                response = await EntityServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add entity type\n" + response.message;
            }
            else
            {
                response = await EntityServices.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update entity type ";
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }

            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetEntityType();
            BtnToogle(false);
        }
    }
}
