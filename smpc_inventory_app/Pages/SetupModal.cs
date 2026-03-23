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
        public event Action OnDataChanged;
        private GeneralSetupServices _serviceSetup;
        private string url { get; }
        private string title { get; }
        private bool showSelectedField;
        private string placeHolderText = "Search...";
        private DataTable _data;

        public SetupModal(string setupTitle, string api, DataTable dt, bool isVisible = false)
        {
            InitializeComponent();
            this.url = api;
            this.title = setupTitle;
            this.showSelectedField = isVisible;
            this._data = dt;

            txt_search.Text = placeHolderText;
            lbl_setup_title.Text = setupTitle;
            _serviceSetup = new GeneralSetupServices(this.url);

        }


        //Load of Data
        private void SetupModal_Load(object sender, EventArgs e)
        {
            dg_setup.DataSource = this._data;
            Console.WriteLine("datatable" + _data);

            if (dg_setup.Columns["is_selected"] != null)
                dg_setup.Columns["is_selected"].Visible = this.showSelectedField;

        }


        // Fetch Setup
        private async void GetSetup()
        {
            _data = await _serviceSetup.GetAsDatatable();
            dg_setup.DataSource = _data;

        }
        private void  BtnToggle(bool isEdit)
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

        private void dg_setup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            Panel[] pnlList = { panel_records };

            // Get the actual DataRow from the clicked row (works with filters too)
            DataRowView rowView = dg_setup.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null) return;

            DataTable singleRow = rowView.Row.Table.Clone();
            singleRow.ImportRow(rowView.Row);

            Helpers.BindControls(pnlList, singleRow, 0); // always row 0 since it's a single row
            btn_edit.Enabled = true;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToggle(true);
            dg_setup.ClearSelection();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(panel_records);
            BtnToggle(false);
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
                response = await _serviceSetup.Insert(data);
                message = response.Success ? "Insert Data Succesfully" : "Failed to add" + this.title +"\n" + response.message;
            }
            else
            {
                response = await _serviceSetup.Update(data);
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
            BtnToggle(false);

            OnDataChanged?.Invoke();

        }
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dg_setup.DataSource = _data;
            }
            else
            {
                ApplySearchFilter(searchText.ToLower());
            }
        }
        private void ApplySearchFilter(string searchText)
        {
            _data.DefaultView.RowFilter =
                $"CONVERT(code, 'System.String') LIKE '%{searchText}%' OR " +
                $"CONVERT(name, 'System.String') LIKE '%{searchText}%'";

            dg_setup.DataSource = _data.DefaultView;
        }

        private void txt_search_Enter(object sender, EventArgs e)
        {
            if (txt_search.Text == placeHolderText)
            {
                txt_search.Text = "";
                txt_search.ForeColor = Color.Black;
            }
        }

        private void txt_search_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = placeHolderText;
                txt_search.ForeColor = Color.Gray;
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_name.Text) && string.IsNullOrEmpty(txt_code.Text))
                {
                    Helpers.ShowDialogMessage("warning", "Select items to delete first.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

                if (result == DialogResult.Yes)
                {
                    var data = Helpers.GetControlsValues(panel_records);

                    bool isSuccess = await _serviceSetup.Delete(data);

                    if (isSuccess)
                    {
                        Helpers.ResetControls(panel_records);
                        Helpers.ShowDialogMessage("success", "Item deleted successfully.");
                        GetSetup();
                        BtnToggle(false);

                        OnDataChanged?.Invoke();
                    }
                    else
                    {
                        Helpers.ShowDialogMessage("error", "Failed to delete item.");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", $"An error occurred: {ex.Message}");
            }
           
        }
    }
}
