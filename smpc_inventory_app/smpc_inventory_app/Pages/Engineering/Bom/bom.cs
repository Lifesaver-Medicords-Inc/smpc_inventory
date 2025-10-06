using Newtonsoft.Json;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Engineering.Bom;
using smpc_inventory_app.Pages.Setup;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Bom;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Bom;
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
    public partial class bom : UserControl
    {
        BomClass records;
        DataTable BomHead;
        DataTable BomDetail;
        DataTable bomItemList;

        public bom()
        {

            InitializeComponent();

            dg_bom.CellValueChanged += dg_bom_CellValueChanged;
            txt_man_days.TextChanged += txt_man_days_TextChanged;
            txt_labor_rate.TextChanged += txt_labor_rate_TextChanged;
        }

        int selectedRecord = 0;

        private string daysPlaceholder = "[DAYS]";
        private string ratePlaceholder = "[RATE]";

        private async void GetBomItemList()
        {
            var data = await ItemListBomServices.GetAsDatatable();
            bomItemList = data;
        }


        private void BtnToogle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;
            btn_search.Visible = !isEdit;


            pnl_header.Enabled = isEdit;
            pnl_components.Enabled = isEdit;
            dg_bom.Enabled = isEdit;


        }


        private bool ValidateFields(out string errorFieldMessage)
        {
            bool isValid = true;
            errorFieldMessage = string.Empty;

            if (string.IsNullOrEmpty(txt_general_name.Text))
            {
                isValid = false;
                errorFieldMessage += "General Name cannot be empty.\n";
            }
            if (string.IsNullOrEmpty(txt_item_model.Text))
            {
                isValid = false;
                errorFieldMessage += "Model cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_item_code.Text))
            {
                isValid = false;
                errorFieldMessage += "Item Code cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_production_qty.Text) || !int.TryParse(txt_production_qty.Text, out _))
            {
                isValid = false;
                errorFieldMessage += "Production Quantity must be a valid number.\n";
            }

            if (string.IsNullOrEmpty(txt_production_cost.Text))
            {
                isValid = false;
                errorFieldMessage += "Production Cost cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_man_days.Text))
            {
                isValid = false;
                errorFieldMessage += "Days cannot be empty.\n";
            }

            if (string.IsNullOrEmpty(txt_labor_rate.Text))
            {
                isValid = false;
                errorFieldMessage += "Rate cannot be empty.\n";
            }

            if (cmb_production_type.SelectedItem == null)
            {
                isValid = false;
                errorFieldMessage += "Production Type must be selected.\n";
            }

            return isValid;

        }






        //private void txt_item_name_TextChanged(object sender, EventArgs e)
        //{
        //    Helpers.ResetControls(pnl_header);
        //    Helpers.ResetControls(pnl_components);

        //    dg_bom.DataSource = null;
        //    dg_bom.Rows.Clear();

        //    BtnToogle(true);

        //    txt_general_name.Clear();
        //    txt_item_model.Clear();
        //    txt_item_code.Clear();
        //    txt_production_qty.Clear();
        //    txt_labor.Clear();
        //    cmb_production_type.SelectedIndex = 0;
        //}



        private void dg_bom_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                if (dg_bom.Columns[e.ColumnIndex].Name == "item_code")
                {


                    BomItemModal modal = new BomItemModal();
                    DialogResult r = modal.ShowDialog();

                    if (r == DialogResult.OK)
                    {

                        Dictionary<string, dynamic> result = modal.GetResult();
                        this.dg_bom.Rows[e.RowIndex].Cells[0].Value = result["item_code"];
                        this.dg_bom.Rows[e.RowIndex].Cells[1].Value = result["short_desc"];
                        this.dg_bom.Rows[e.RowIndex].Cells[2].Value = result["item_id"];
                        this.dg_bom.Rows[e.RowIndex].Cells[4].Value = result["size"];
                        this.dg_bom.Rows[e.RowIndex].Cells[6].Value = result["uom_name"];
                        

                    }


                }
            }


        }

        private void btn_get_item_Click(object sender, EventArgs e)
        {


            List<int> t1 = new List<int>();
            List<string> s1 = new List<string>();
            string Title = "Item";

            SetupItemModal item_bom = new SetupItemModal(Title, ENUM_ENDPOINT.BomItemList, bomItemList, t1, s1, 0);
            DialogResult r = item_bom.ShowDialog();

            if (r == DialogResult.OK)
            {
                int result = item_bom.GetResult();

                if (result != null)
                {

                    Panel[] pnl_list = { pnl_header };

                    Helpers.BindControls(pnl_list, bomItemList, result);

                }
            }


        }



        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            Helpers.ResetControls(pnl_components);
            dg_bom.ClearSelection();
            BtnToogle(true);

            dataBindingBomComponents.DataSource = BomDetail.Clone();


        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            Helpers.ResetControls(pnl_components);
            BtnToogle(false);
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {

                this.selectedRecord--;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(pnl_header);

            if (data.ContainsKey("item_id") && data["item_id"] != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this data?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool isSuccess = await BomServices.DeleteBom(data);

                    if (isSuccess)
                    {
                        Helpers.ShowDialogMessage("success", "BOM deleted successfully!");

                        GetBomItemList();

                        Helpers.ResetControls(pnl_header);
                        BtnToogle(false);
                    }
                    else
                    {
                        Helpers.ShowDialogMessage("error", "Operation failed. Please try again later.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No BOM data selected for deletion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToogle(true);

        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            string message;

            string errorMessage;
            bool isValid = ValidateFields(out errorMessage);
            ApiResponseModel response = new ApiResponseModel();

            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            


            var data = Helpers.GetControlsValues(pnl_header);
            data["item_id"] = int.Parse(data["item_id"].ToString());
            data["production_qty"] = int.Parse(data["production_qty"].ToString());
            data["production_cost"] = decimal.TryParse(data["production_cost"].ToString(), out decimal productionCost) ? (float)productionCost : 0f;
            data["man_days"] = decimal.TryParse(data["man_days"].ToString(), out decimal manDays) ? (int)manDays : 0;
            data["labor_rate"] = decimal.TryParse(data["labor_rate"].ToString(), out decimal laborRate) ? (float)laborRate : 0f; 




            data.Remove("general_name");
            data.Remove("item_model");
            data.Remove("item_code");

            var isUpdate = !txt_id.Text.Equals("");
            var bomDetail = SaveBomDetails(isUpdate);



            data.Add("bom_details", bomDetail);
            data.Remove("item_code");
            data.Remove("short_desc");
            data.Remove("uom");

            try
            {

                if (txt_id.Text.Equals(""))
                {
                    data.Remove("id");
                    response = await BomServices.Insert(data);
                    message = response.Success ? "Insert Bom Data Succesfully" : "Failed to add Bom\n" + response.message;

                    MessageBox.Show(response.Success ? "BOM saved successfully!" : "Failed to save BOM.",
                        response.Success ? "Success" : "Error",
                        MessageBoxButtons.OK,
                        response.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }
                else
                {

                    response = await BomServices.UpdateBom(data);
                    message = response.Success ? "BOM updated successfully!" : "Failed to update BOM.";

                    MessageBox.Show(response.Success ? "BOM updated successfully!" : "Failed to update BOM.",
                        response.Success ? "Success" : "Error",
                        MessageBoxButtons.OK,
                        response.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }


                if (!response.Success)
                {
                    Helpers.ShowDialogMessage("error", message);
                    return;
                }

                Helpers.ResetControls(pnl_header);
                BtnToogle(false);
                dataBindingBomComponents.DataSource = BomDetail.Clone();

            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the BOM: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.BomHead.Rows.Count - 1 > this.selectedRecord)
            {
                this.selectedRecord++;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }





        private List<ItemBomDetails> SaveBomDetails(bool isUpdate)
        {

            var dataSource = Helpers.ConvertDataGridViewToDataTable(dg_bom);
            List<ItemBomDetails> bomDetails = new List<ItemBomDetails>();
            ItemBomDetails bomChild = null;

            int item_bom_id = 0;
            int id = 0;
            foreach (DataRow row in dataSource.Rows)
            {



                if (isUpdate)
                {
                    if (row.IsNull("item_bom_id") || string.IsNullOrWhiteSpace(row["item_bom_id"].ToString()) || row.IsNull("id") || string.IsNullOrWhiteSpace(row["id"].ToString()))
                    {
                        item_bom_id = 0;
                        id = 0;
                    }
                    else
                    {
                        item_bom_id = int.Parse(row["item_bom_id"].ToString());
                        id = int.Parse(row["id"].ToString());

                    }
                }

                int item_id = 0;
                int bom_qty = 0;
                int unit_price = 0;
                float net_price = 0;


                int.TryParse(row["item_id"].ToString(), out item_id);
                int.TryParse(row["bom_qty"].ToString(), out bom_qty);
                int.TryParse(row["unit_price"].ToString(), out unit_price);
                float.TryParse(row["net_price"].ToString(), out net_price);




                bomChild = new ItemBomDetails(id, item_bom_id, item_id, bom_qty, unit_price, net_price);
                bomDetails.Add(bomChild);

            }

            return bomDetails;
        }
         
        private void txt_item_id_TextChanged(object sender, EventArgs e)
        {

        }

        private async void bom_Load(object sender, EventArgs e)
        {
            GetBomItemList();
            BtnToogle(false);

            txt_man_days.Text = daysPlaceholder;
            txt_labor_rate.Text = ratePlaceholder;

            txt_man_days.ForeColor = Color.Gray;
            txt_labor_rate.ForeColor = Color.Gray;

            var response = await RequestToApi<ApiResponseModel<BomClass>>.Get(ENUM_ENDPOINT.BOM);
            records = response.Data;

            BomHead = JsonHelper.ToDataTable(records.bom_head);
            BomDetail = JsonHelper.ToDataTable(records.bom_details);

            if (records.bom_head.Count != 0 && records.bom_details.Count != 0)
            {
                Bind(true);
            }


        }




        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                BindDataToPanel();
                FetchAllChilds();
            }
        }

        private void BindDataToPanel()
        {
            Panel[] pnlHeader = { pnl_header };

            Helpers.BindControls(pnlHeader, BomHead, this.selectedRecord);
        }

        private void FetchAllChilds()
        {
            DataView dataViewItemDetails = new DataView(BomDetail);

            if (dataViewItemDetails.Count != 0)
            {
                dataViewItemDetails.RowFilter = "item_bom_id = '" + BomHead.Rows[this.selectedRecord]["id"].ToString() + "'";

            }
            DataTable filtedTable = dataViewItemDetails.ToTable();
            dataBindingBomComponents.DataSource = filtedTable;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (bomItemList == null || bomItemList.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] columnsToShow = { "item_id", "general_name", "item_code","item_model", "short_desc" };
            using (BomSearch bomSearch = new BomSearch("Search Items", bomItemList, columnsToShow))
            {
                if (bomSearch.ShowDialog() == DialogResult.OK)
                {
                    int selectedIndex = bomSearch.SelectedIndex;
                    if (selectedIndex >= 0)
                    {
                        this.selectedRecord = selectedIndex;
                        Bind(true);
                    }
                }
            }
        }

        private void txt_days_Enter(object sender, EventArgs e)
        {
            if (txt_man_days.Text == daysPlaceholder)
            {
                txt_man_days.Text = "";
                txt_man_days.ForeColor = Color.Black;
            }
        }

        private void txt_days_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_man_days.Text))
            {
                txt_man_days.Text = daysPlaceholder;
                txt_man_days.ForeColor = Color.Gray; 
            }
        }

        private void txt_rate_Enter(object sender, EventArgs e)
        {
            if (txt_labor_rate.Text == ratePlaceholder)
            {
                txt_labor_rate.Text = "";
                txt_labor_rate.ForeColor = Color.Black; 
            }
        }

        private void txt_rate_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_labor_rate.Text))
            {
                txt_labor_rate.Text = ratePlaceholder;
                txt_labor_rate.ForeColor = Color.Gray; 
            }
        }

        private void pnl_header_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CalculateProductionCost()
        {
            decimal totalNetPrice = 0;
            foreach(DataGridViewRow row in dg_bom.Rows)
            {
                if (row.Cells["bom_qty"].Value != null && decimal.TryParse(row.Cells["bom_qty"].Value.ToString(), out decimal qty) &&
                    row.Cells["unit_price"].Value != null && decimal.TryParse(row.Cells["unit_price"].Value.ToString(), out decimal unitPrice))
                {
                    decimal netPrice = qty * unitPrice;
                    row.Cells["net_price"].Value = netPrice;
                    totalNetPrice += netPrice;

                }
            }

            decimal ManDays = 0;
            decimal LaborRate = 0;

            if (decimal.TryParse(txt_man_days.Text, out ManDays) && decimal.TryParse(txt_labor_rate.Text, out LaborRate))
            {
                decimal laborCost = ManDays * LaborRate;
                decimal A = totalNetPrice  + laborCost;
                decimal markUp = 1.186m;
                decimal productionCost = A * markUp;

                txt_production_cost.Text = productionCost.ToString("F2");
            }
            else
            {
                txt_production_cost.Text = "0.00";
            }
        }

        private void dg_bom_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && e.ColumnIndex == dg_bom.Columns["unit_price"].Index) 
            //{
            //    CalculateProductionCost();
            //}
        }

        private void txt_man_days_TextChanged(object sender, EventArgs e)
        {
            //CalculateProductionCost();

        }

        private void txt_labor_rate_TextChanged(object sender, EventArgs e)
        {
            //CalculateProductionCost();
        }

        private void dg_bom_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CalculateProductionCost();
        }
    }
}





