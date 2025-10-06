using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Warehouse;
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
    public partial class frm_warehouse_usetype_setup : UserControl
    {
        public frm_warehouse_usetype_setup()
        {
            InitializeComponent();
            cmb_bg_color.DrawMode = DrawMode.OwnerDrawFixed; 
            cmb_bg_color.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                Color color = Color.FromKnownColor(knownColor);
                cmb_bg_color.Items.Add(color); // ARGB info is stored in each Color
            }
        }

        private async void GetData()
        {
            var data = await WarehouseUseTypeServices.GetDataTable();

            if (data.Rows.Count <= 0)
            {
                BtnToggle("empty");
                return;
            }

            dg_warehouse_usetype.DataSource = data;
             
            DataTable dataTable = Helpers.ConvertDataGridViewToDataTable(dg_warehouse_usetype);
            Panel[] panelList = { pnl_records };
            Helpers.BindControls(panelList, dataTable, 0);
             
            colorDictionary.Clear();

            //add color based on bg_color column
            foreach (DataGridViewRow row in dg_warehouse_usetype.Rows)
            { 
                string nameValue = row.Cells["name"]?.Value?.ToString();
                string bgColorName = row.Cells["bg_color"]?.Value?.ToString();

                if (!string.IsNullOrWhiteSpace(nameValue) && !string.IsNullOrWhiteSpace(bgColorName))
                {
                    Color color = Color.FromName(bgColorName);
                     
                    if (!colorDictionary.ContainsKey(nameValue))
                        colorDictionary[nameValue] = color;
                     
                    var nameCell = row.Cells["name"];
                    nameCell.Style.BackColor = color;
                    nameCell.Style.ForeColor = color.GetBrightness() < 0.6f ? Color.White : Color.Black;
                }
            }

        }

        private void frm_warehouse_usetype_setup_Load(object sender, EventArgs e)
        {
            GetData();
        } 

        private async void btn_save_Click(object sender, EventArgs e)
        {
            var data = Helpers.GetControlsValues(pnl_records);
            ApiResponseModel response = new ApiResponseModel();

            string ErrorMessage =
                string.IsNullOrWhiteSpace(txt_code.Text) && string.IsNullOrWhiteSpace(txt_name.Text) ? "Code and Name cannot be empty" :
                string.IsNullOrWhiteSpace(txt_code.Text) ? "Code cannot be empty" :
                string.IsNullOrWhiteSpace(txt_name.Text) ? "Name cannot be empty" : null;

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                Helpers.ShowDialogMessage("error", ErrorMessage);
                return;
            }

            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                data.Remove("id");
            }

            response = isNewRecord ? 
                await WarehouseUseTypeServices.Insert(data) : 
                await WarehouseUseTypeServices.Update(data);

            if (response.Success)
            {
                Helpers.ResetControls(pnl_records);
                GetData();
                BtnToggle("save");
                TableContentChanged.WarehouseUseType = true;
            }
            
            string message = response.Success ?
                (isNewRecord ? "Usetype Added Successfully" : "Usetype Updated Succesfully") :
                (isNewRecord ? "Failed to add usetype\n" + response.message : "Failed update usetype\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);
        }

        private void dg_warehouse_usetype_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dg_warehouse_usetype.Rows.Count) return;

            Panel[] panelList = { pnl_records };
            DataTable dataTable = Helpers.ConvertDataGridViewToDataTable(dg_warehouse_usetype);
            Helpers.BindControls(panelList, dataTable, e.RowIndex); 
        }

        private void BtnToggle(string action)
        { 
            if (action == "new" || action == "edit")
            {
                btn_save.Visible = true;
                btn_cancel.Visible = true;
                btn_delete.Visible = false;
                dg_warehouse_usetype.Enabled = false; 

                Helpers.SetPanelToReadOnly(pnl_records, false);
            }
            if (action == "edit")
            {
                btn_new.Visible = false;
                btn_edit.Visible = true;
            }
            else if (action == "new")
            {
                btn_new.Visible = true;
                btn_edit.Visible = false;
            }
            else if (action == "cancel" || action == "save" || action == "delete")
            { 
                dg_warehouse_usetype.Enabled = true;
                btn_new.Visible = true;
                btn_edit.Visible = true;
                btn_save.Visible = false;
                btn_cancel.Visible = false;
                btn_delete.Visible = true;

                Helpers.SetPanelToReadOnly(pnl_records, true);
            } 
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle("edit");  

            //dg_warehouse_usetype.ClearSelection();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_records);
            BtnToggle("new");
            dg_warehouse_usetype.ClearSelection();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        { 
            BtnToggle("cancel"); 
            Helpers.SetPanelToReadOnly(pnl_records, true); 
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this usetype", 
                "Confirm Deletion", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                var data = Helpers.GetControlsValues(pnl_records);

                bool isSuccess = await WarehouseUseTypeServices.Delete(data);

                if (!isSuccess) //idk why it has to be !isSuccess errors if aint
                {
                    BtnToggle("delete");
                    GetData();
                    Helpers.ShowDialogMessage("success", "Usetype deleted successfully");
                    Helpers.ResetControls(pnl_records);
                }
                else
                {
                    Helpers.ShowDialogMessage("error", "Failed to delete usetype" + isSuccess);
                }
            }
        }


        private Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>(); 
        private void cmb_bg_color_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dg_warehouse_usetype != null && dg_warehouse_usetype.Rows.Count > 0)
            {
                Color selectedColor = (Color)cmb_bg_color.SelectedItem;

                for (int i = 0; i < dg_warehouse_usetype.Rows.Count; i++)
                {
                    if (dg_warehouse_usetype.Rows[i].Cells[2].Value?.ToString() == txt_name.Text)
                    {
                        // Set background color
                        dg_warehouse_usetype.Rows[i].Cells[2].Style.BackColor = selectedColor;

                        // Use brightness to determine appropriate text color
                        float brightness = selectedColor.GetBrightness();

                        dg_warehouse_usetype.Rows[i].Cells[2].Style.ForeColor =
                            (brightness < 0.5f) ? Color.White : Color.Black;
                    }
                }
            }
        } 
         
        private void cmb_bg_color_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = (ComboBox)sender;
            Color itemColor = (Color)combo.Items[e.Index];
            Rectangle fullRect = e.Bounds;

            // right side swatch with 10% kain
            int swatchWidth = (int)(fullRect.Width * 0.10);
            Rectangle textRect = new Rectangle(fullRect.X, fullRect.Y, fullRect.Width - swatchWidth, fullRect.Height);
            Rectangle colorRect = new Rectangle(fullRect.Right - swatchWidth, fullRect.Y, swatchWidth, fullRect.Height);

            // selected item
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // for background
            Color backgroundColor = isSelected ? SystemColors.Highlight : SystemColors.Window;
            using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, fullRect);
            }

            // for color swatch
            using (Brush colorBrush = new SolidBrush(itemColor))
            {
                e.Graphics.FillRectangle(colorBrush, colorRect);
                e.Graphics.DrawRectangle(Pens.Black, colorRect); 
            }
             
            string colorName = itemColor.Name;
            Color textColor = isSelected ? SystemColors.HighlightText : SystemColors.ControlText;
            using (Brush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(colorName, e.Font, textBrush, textRect, StringFormat.GenericDefault);
            }
             
            e.DrawFocusRectangle();
        }

        private void txt_name_MouseLeave(object sender, EventArgs e)
        {
            txt_name.Text = txt_name.Text.ToUpper();
        }
    }
}
