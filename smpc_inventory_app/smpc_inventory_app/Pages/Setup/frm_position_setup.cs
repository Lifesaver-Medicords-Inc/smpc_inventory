using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
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
namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_position_setup : UserControl
    {
        public frm_position_setup()
        {
            InitializeComponent();
        }

        private void frm_position_setup_Load(object sender, EventArgs e)
        {
            GetPosition();
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

        private async void GetPosition()
        {   
                var data = await PositionServices.GetAsDatatable();
                dg_position.DataSource = data;        
        }

        private void panel_records_Paint(object sender, PaintEventArgs e)
        {
            BtnToogle(false);
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
                messages += "Position cannot be empty \n";
                isValid = true;
            }

            return isValid;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            BtnToogle(false);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            BtnToogle(false);
        }


        private void btn_new_Click_1(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            dg_position.ClearSelection();
            BtnToogle(true);
        }

        private void btn_cancel_Click_1(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToogle(false);
        }

        private void btn_edit_Click_1(object sender, EventArgs e)
        {
            BtnToogle(true);
            dg_position.ClearSelection();
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
    
            string message;
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
                response = await PositionServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add position\n" + response.message;
            }
            else
            {
                response = await PositionServices.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update Position";
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }

            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetPosition();
            BtnToogle(false);

        }

        private async void btn_delete_Click_1(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(panel_records);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool isSuccess = await PositionServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Position Succesfully");
                GetPosition();
                BtnToogle(false);
            }
        }

        private void SortDataGridView(int columnIndex)
        {
            var column = dg_position.Columns[columnIndex];
            dg_position.Sort(column, ListSortDirection.Ascending);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dg_position_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                SortDataGridView(e.ColumnIndex);
            }
        }

        private void dg_position_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void panel_records_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void dg_position_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Panel[] pnlList = { panel_records };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_position);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
        }

        private void txt_code_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_id_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

