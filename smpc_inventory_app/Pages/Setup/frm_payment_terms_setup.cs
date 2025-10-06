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
    public partial class frm_payment_terms_setup : UserControl
    {
        public frm_payment_terms_setup()
        {
            InitializeComponent();
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

        private async void GetPaymentTerms()
        {
            DataTable data = await PaymentTermsServices.GetAsDatatable();

            dataBinding_payment_terms.DataSource = data;

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
                bool isSuccess = await PaymentTermsServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Payment Terms Succesfully");
                GetPaymentTerms();
                BtnToogle(false);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

        private void frm_payment_terms_setup_Load(object sender, EventArgs e)
        {
            GetPaymentTerms();
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
                data.Add("is_selected", false);
                response = await PaymentTermsServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add payment terms\n" + response.message;
            }
            else
            {

                response = await PaymentTermsServices.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update payment terms";
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }

            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetPaymentTerms();
            BtnToogle(false);
        }

        private void panel_header_Paint(object sender, PaintEventArgs e)
        {

        }


        private void dg_payment_terms_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            Panel[] pnlList = { panel_records };
            DataTable paymentTermData = Helpers.ConvertDataGridViewToDataTable(dg_payment_terms);
            Helpers.BindControls(pnlList, paymentTermData, e.RowIndex);

        }

        private void dg_payment_terms_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dg_payment_terms_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var records = Helpers.GetControlsValues(panel_records);
            if (e.ColumnIndex == 0)
            {
                dg_payment_terms.CommitEdit(DataGridViewDataErrorContexts.Commit);
                int selectedRecordIndex = dg_payment_terms.CurrentRow.Index;
                var paymentTermSource = Helpers.ConvertDataGridViewToDataTable(dg_payment_terms);

                if (paymentTermSource != null && paymentTermSource.Rows.Count > 1)
                {
                    var rows = paymentTermSource.Rows[selectedRecordIndex];
                    string selected = paymentTermSource.Rows[selectedRecordIndex]["is_selected"].ToString();
                    records.Add("is_selected", bool.Parse(selected));              
                    UpdateSelectedPaymentTerms(records);
                }

            }

        }
        private async void UpdateSelectedPaymentTerms(Dictionary <string,dynamic> records)
        {
            ApiResponseModel response = new ApiResponseModel();
            string message = "";
            response = await PaymentTermsServices.Update(records);
            message = response.Success ? "Update Data Successfully" : "Failed to update as selected records"; 
            
            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }
            Helpers.ShowDialogMessage("success", message);
            GetPaymentTerms();


        }

        private void dg_payment_terms_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }
    }
}
