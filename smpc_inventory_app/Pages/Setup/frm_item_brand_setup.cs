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
namespace Inventory_SMPC.Pages.Setup
{
    public partial class frm_item_brand_setup : UserControl
    {
       
        public frm_item_brand_setup()
        {
            InitializeComponent();
        }
       

        private  void frm_item_brand_setup_Load(object sender, EventArgs e)
        {
            GetBrand();
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


        private async void GetBrand()
        {
            var data = await ItemBrandServices.GetAsDatatable();
            //dg_brands.DataSource = data;
            this.dataSource.DataSource = data;
        }

        private void panel_records_Paint(object sender, PaintEventArgs e)
        {
            BtnToogle(false);

        }


          private bool ValidateField(out string messages) {

            bool isValid = false ;
            messages= string.Empty;

            if (string.IsNullOrEmpty(txt_code.Text)) {
                messages += "Code cannot be empty \n";
                isValid = true;
            }

            if (string.IsNullOrEmpty(txt_name.Text)) {
                messages += "Name cannot be empty \n";
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

        private void dg_brands_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
        }




        private void btn_new_Click_1(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            dg_brands.ClearSelection(); 
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
            dg_brands.ClearSelection();
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
                response = await ItemBrandServices.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add brand\n" +  response.message ;
            }
            else
            {
                response = await ItemBrandServices.Update(data);
                message = response.Success ? "Update Data Succesfully" : "Failed to update Brand";
            }

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", message);
                return;
            }
         
            Helpers.ShowDialogMessage("success", message);
            Helpers.ResetControls(panel_records);
            GetBrand();
            BtnToogle(false);

        }

        private async void btn_delete_Click_1(object sender, EventArgs e)
        {
           var data = Helpers.GetControlsValues(panel_records);

           DialogResult result =  MessageBox.Show("Are you sure you want to delete this data? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ;
           
            if(result == DialogResult.Yes) {
                bool isSuccess = await ItemBrandServices.Delete(data);

                if (!isSuccess)
                {
                    Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    return;
                }
                Helpers.ResetControls(panel_records);
                Helpers.ShowDialogMessage("success", "Delete Brand Succesfully");
                GetBrand();
                BtnToogle(false);
            }


        }

        private void SortDataGridView(int columnIndex)
        {
            // Add your sorting logic based on the column index
            // Example: Sort the column by ascending order
            var column = dg_brands.Columns[columnIndex];
            dg_brands.Sort(column, ListSortDirection.Ascending);
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dg_brands_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                // Logic for handling column header click, e.g., sorting or filtering
                SortDataGridView(e.ColumnIndex);
            }
        }

        private void dg_brands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel_records_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
