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
            dg_brands.AutoGenerateColumns = false;
        }
       

        private  void frm_item_brand_setup_Load(object sender, EventArgs e)
        {
            // Was only being set via panel_records_Paint (see note below) - moved here so it
            // runs once at load instead of on every repaint.
            BtnToggle(false);
            GetBrand();
        }
        private void BtnToggle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_edit.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_cancel.Visible = isEdit;
            pnl_input.Enabled = isEdit;
            dg_brands.Enabled = !isEdit;

            // Bug #253: re-enabling the grid (Edit -> Cancel) left it visually blank until the
            // user resized the window or otherwise forced a repaint - a known WinForms quirk
            // where a DataGridView doesn't always redraw its rows right after Enabled flips
            // back to true. Force the redraw explicitly instead of leaving it to chance.
            if (!isEdit)
            {
                dg_brands.Invalidate();
                dg_brands.Refresh();
            }
        }


        private async void GetBrand()
        {
            var data = await ItemBrandServices.GetAsDatatable();
            //dg_brands.DataSource = data;
            this.dataSource.DataSource = data;
        }

        private void panel_records_Paint(object sender, PaintEventArgs e)
        {
            // Previously called BtnToggle(false) on every single repaint of this panel (which
            // fires constantly - on focus changes, other dialogs closing, etc). That forced
            // dg_brands.Enabled back to true and reset the New/Edit/Delete/Save/Cancel button
            // visibility mid-edit, which is what produced the "data disappears" glitch in bug
            // #253: toggling Enabled on a DataGridView from inside a Paint handler re-triggers
            // painting before the grid finishes redrawing its rows. BtnToggle(false) now only
            // runs once, from the Load event.
        }
        private bool ValidateField(out string messages) 
        {
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
            BtnToggle(true);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
        }
        private void btn_new_Click_1(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_input);
            dg_brands.ClearSelection(); 
            BtnToggle(true);

        }

        private void btn_cancel_Click_1(object sender, EventArgs e)
        {

            Helpers.ResetControls(pnl_input);
            BtnToggle(false);
        }

        private void btn_edit_Click_1(object sender, EventArgs e)
        {
            BtnToggle(true);
            dg_brands.ClearSelection();
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            // Get input values
            var data = Helpers.GetControlsValues(pnl_input);
            ApiResponseModel response = new ApiResponseModel();

            // Validate required fields
            string errorMessage =
                string.IsNullOrWhiteSpace(txt_code.Text) && string.IsNullOrWhiteSpace(txt_name.Text) ? "Code and Name cannot be empty." :
                string.IsNullOrWhiteSpace(txt_code.Text) ? "Code cannot be empty." :
                string.IsNullOrWhiteSpace(txt_name.Text) ? "Name cannot be empty." : null;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Helpers.ShowDialogMessage("error", errorMessage);
                return;
            }

            // Insert or update data
            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                data.Remove("id");
            }

            response = isNewRecord
                ? await ItemBrandServices.Insert(data)
                : await ItemBrandServices.Update(data);

            // Handle result
            if (response.Success)
            {
                Helpers.ResetControls(pnl_input);
                GetBrand();
                BtnToggle(false);
            }

            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);

        }

        private async void btn_delete_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                 "Are you sure you want to delete this item?",
                 "Confirm Deletion",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question
             );

            if (result == DialogResult.Yes)
            {
                var data = Helpers.GetControlsValues(pnl_input);

                bool isSuccess = await ItemBrandServices.Delete(data);

                if (isSuccess)
                {
                    Helpers.ResetControls(pnl_input);
                    Helpers.ShowDialogMessage("success", "Item deleted successfully.");
                    GetBrand();
                    BtnToggle(false);
                }
                else
                {
                    Helpers.ShowDialogMessage("error", "Failed to delete item.");
                }
            }
        }

        private void SortDataGridView(int columnIndex)
        {
            // Add your sorting logic based on the column index
            // Example: Sort the column by ascending order
            var column = dg_brands.Columns[columnIndex];
            dg_brands.Sort(column, ListSortDirection.Ascending);
        }

        private void dg_brands_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                // Logic for handling column header click, e.g., sorting or filtering
                SortDataGridView(e.ColumnIndex);
            }
        }

        private void dg_brands_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Panel[] pnlList = { pnl_input };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_brands);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
            btn_edit.Enabled = true;
            btn_delete.Enabled = true;
        }
    }
}
