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

namespace Inventory_SMPC.Pages.Setup
{
    public partial class frm_unit_of_measure_setup : UserControl
    {
        public frm_unit_of_measure_setup()
        {
            InitializeComponent();
        }

        private async void GetUnitOfMeasurement()
        {
            var data = await UnitOfMeasurementServices.GetAsDatatable();
            dg_unit_of_measurement.DataSource = data;

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
            BtnToogle(true);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(panel_records);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this data? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool isSuccess = await UnitOfMeasurementServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Unit of Measurement Succesfully");
                GetUnitOfMeasurement();
                BtnToogle(false);
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {

            string errorFieldMessage;
            string message;
            bool isErrorField;
            ApiResponseModel response = new ApiResponseModel();

            isErrorField = ValidateField(out errorFieldMessage);

            // Check if  all field has error
            if (isErrorField) {

                Helpers.ShowDialogMessage("error", errorFieldMessage);
                return;
            }

            // Get all data of textfield from panel
            var data = Helpers.GetControlsValues(panel_records);

            //Conditions for Add and Update executions of Api
            if (txt_id.Text.Equals("")) {
                data.Remove("id");
                response = await UnitOfMeasurementServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add unit of measurement\n" + response.message;
            }
            else {
                response = await UnitOfMeasurementServices.Update(data);  
                message = response.Success ? "Update Data Succesfully" : "Failed to update unit of measurement";
            }

            if (!response.Success) {
                Helpers.ShowDialogMessage("error", message);
                return;
            }
            //Message  after success 
            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetUnitOfMeasurement();
            BtnToogle(false);

            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

 

        private void frm_unit_of_measure_setup_Load(object sender, EventArgs e)
        {
            GetUnitOfMeasurement();
        }


       
        private void dg_unit_of_measurement_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Panel[] pnlList = { panel_records };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_unit_of_measurement);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
