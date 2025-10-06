using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Helpers;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Pages;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Pages.Setup;
using smpc_inventory_app.Pages.Item;
using System.IO;
using System.Net;
using System.Drawing.Imaging;

namespace Inventory_SMPC.Pages.Item
{
    public partial class frm_Item_Entry : UserControl
    {
        GeneralSetupServices serviceSetup;
        SetupModal modalSetup;
        TradeTypeSelectionModal tradetypemodal = new TradeTypeSelectionModal();
        DataTable items;
        DataTable itemspecs;
        DataTable additionalspecs;
        DataTable itempurchasing;
        DataTable itemsales;
        DataTable itemproduction;
        DataTable itemprice;
        DataTable itemimages;
        DataTable dt_template;
        SetupSelectionModal modalSelection;

        Items records;
        int selectedRecord = 0;
        private bool isProgrammaticChange = false;
        private List<TabPage> hiddenTabs = new List<TabPage>();
        private Dictionary<string, int> tabOrder = new Dictionary<string, int>();
        private Dictionary<string, string> item_img = new Dictionary<string, string>();
        private List<PictureBox> imageBoxes = new List<PictureBox>();
        List<int> currentSelectedPumpTypeIds = new List<int>();
        List<int> currentSelectedTradeTypeIds = new List<int>();
        private Dictionary<int, string> imageFilePaths = new Dictionary<int, string>();
        private Dictionary<string, object> imageData = new Dictionary<string, object>();
        private List<string> newbase64Images = new List<string>();
        List<Dictionary<string, object>> replaceBase64Images = new List<Dictionary<string, object>>();

        private int tempImageIdCounter = -1; 

        public frm_Item_Entry()
        {
            InitializeComponent();

        }
        public void HideButton()
        {
            btn_add_supplier.Visible = false;
        }
        private async void frm_Item_Entry_Load(object sender, EventArgs e)
        {
            BtnToggle(false);

            await GetNameSetup();
            await GetClassSetup();
            await GetBrandSetup();
            await GetUOMSetup();
            await GetItemTradeType();
            await GetMaterialSetup();
            await GetPumpTypeSetup();
            await GetPumpCountSetup();
            

            GetData();
        }
        private async void GetData()
        {
            var response = await RequestToApi<ApiResponseModel<Items>>.Get(ENUM_ENDPOINT.ITEM);
            records = response.Data;    

            items = JsonHelper.ToDataTable(records.items);
            itemspecs = JsonHelper.ToDataTable(records.itemspecs);
            additionalspecs = JsonHelper.ToDataTable(records.additionalspecs);
            itemimages = JsonHelper.ToDataTable(records.itemimages);
            itempurchasing = JsonHelper.ToDataTable(records.itempurchasing);
            itemsales = JsonHelper.ToDataTable(records.itemsales);
            itemproduction = JsonHelper.ToDataTable(records.itemproduction);

            
            Bind(true);
        }
        private void Bind(bool isBind = false)
        {
            if (isBind)
            {

                //BindMultiSelectField(records.additionalspecs);
                
                GetItemValues();

                // Bind UI panels
                Panel[] pnlItem = { pnl_header };
                Panel[] pnlAdditionalSpecs = { pnl_additional_specs };
                Panel[] pnlItemSales = { pnl_item_sales_price }; 
                Panel[] pnlItemImages = { pnl_item_image };

                Helpers.BindControls(pnlItem, items, this.selectedRecord);
                Helpers.BindControls(pnlItemSales, items, this.selectedRecord);
                Helpers.BindControls(pnlAdditionalSpecs, additionalspecs, this.selectedRecord);
                Helpers.BindControls(pnlItemImages, itemimages, this.selectedRecord);
                     
                int currentItemId = Convert.ToInt32(items.Rows[selectedRecord]["id"]);

                txt_trade_type.Text = string.IsNullOrEmpty(records.items[this.selectedRecord].trade_type_names) ? "" : (records.items[this.selectedRecord].trade_type_names);
                txt_pump_type_compatability.Text = string.IsNullOrEmpty(records.additionalspecs[this.selectedRecord].pump_type_compatability_names) ? "" : (records.additionalspecs[this.selectedRecord].pump_type_compatability_names);
                FetchDataGridViewChild();

                string tradeTypes = string.IsNullOrEmpty(records.items[this.selectedRecord].trade_type_id) ? "" : (records.items[this.selectedRecord].trade_type_id);

                //Getting the List of Ids to match in my getmodal
                currentSelectedTradeTypeIds = tradeTypes.Split(',')
                                                    .Where(val => int.TryParse(val, out _))
                                                    .Select(int.Parse)
                                                    .ToList();
                txt_trade_type.Tag = currentSelectedTradeTypeIds;

                //Check Item Type Before Binding
                ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);

                var matchingItemSpec = records.itemspecs.FirstOrDefault(spec => spec.based_id == currentItemId);

                if (matchingItemSpec != null)
                {
                    txt_item_specs_id.Text = matchingItemSpec.id.ToString();
                    txt_item_specs_based_id.Text = matchingItemSpec.based_id.ToString();
                }
                else
                {
                    ClearTextBoxes(txt_item_specs_id, txt_item_specs_based_id);
                }

                var matchingAdditionalSpec = records.additionalspecs.FirstOrDefault(spec => spec.based_id == currentItemId);

                // Bind ItemSpecs 
                if (items.Rows.Count > 0)
                {
                    DataView dataView = new DataView(itemspecs)
                    {
                        RowFilter = $"based_id = '{currentItemId}'"
                    };
                    dgv_template.DataSource = dataView;

                    HideColumns(dgv_template, "id", "based_id", "template", "manufacturer_origin");

                    cmb_template.SelectedItem = dataView.Count > 0 ? dataView[0]["template"].ToString() : null;
                    txt_manufacturer_origin.Text = dataView.Count > 0 ? dataView[0]["manufacturer_origin"].ToString() : null;
                }

                //Bind Additional Specs
                if (matchingAdditionalSpec != null)
                {
                    txt_additional_specs_id.Text = matchingAdditionalSpec.id.ToString();
                    txt_additional_specs_based_id.Text = matchingAdditionalSpec.based_id.ToString();
                }
                else
                {
                    ClearTextBoxes(txt_additional_specs_id, txt_additional_specs_based_id);
                }
                string pumpTypeIds = string.IsNullOrEmpty(records.additionalspecs[this.selectedRecord].pump_type_compatability_id) ? "" : (records.additionalspecs[this.selectedRecord].pump_type_compatability_id);

                //Getting the List of Ids to match in my getmodal
                currentSelectedPumpTypeIds = pumpTypeIds.Split(',')
                                                    .Where(val => int.TryParse(val, out _))
                                                    .Select(int.Parse)
                                                    .ToList();
                txt_pump_type_compatability.Tag = currentSelectedPumpTypeIds;

                // Bind Images

                flowLayoutPanel1.Controls.Clear();
                img_preview.Image = null;

                var filteredImages = records.itemimages.Where(image => image.based_id == currentItemId).ToList();

                if (filteredImages.Any())
                {
                    txt_item_image_id.Text = filteredImages.First().id.ToString();
                    txt_item_image_based_id.Text = filteredImages.First().based_id.ToString();
                }
                else
                {
                    ClearTextBoxes(txt_item_image_id, txt_item_image_based_id);
                }

                foreach (var imageRecord in filteredImages)
                {
                    if (!string.IsNullOrEmpty(imageRecord.image))
                    {
                        string imageUrl = imageRecord.image.Trim();
                        imageUrl = imageUrl.StartsWith("http") ? imageUrl : "http://" + imageUrl;

                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                byte[] imageData = client.DownloadData(imageUrl);
                                using (MemoryStream ms = new MemoryStream(imageData, false))
                                {
                                    Image loadedImage = Image.FromStream(ms);
                                    PictureBox pictureBox = CreatePictureBox(loadedImage, imageUrl, imageRecord.id);
                                    flowLayoutPanel1.Controls.Add(pictureBox);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //PictureBox errorPictureBox = CreatePictureBox(Properties.Resources.placeholder, imageUrl, imageRecord.id);
                            //flowLayoutPanel1.Controls.Add(errorPictureBox);
                            MessageBox.Show("Error loading image: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                this.items.Rows.Clear();
                this.itemspecs.Rows.Clear();
                this.additionalspecs.Rows.Clear();
                this.itemimages.Rows.Clear();
                this.itempurchasing.Rows.Clear();
                this.itemsales.Rows.Clear();
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            isProgrammaticChange = true;
            if (!CheckIfCalpeda())
            {
                return;
            }
            // Validate required fields
            string errorMessage =
                string.IsNullOrWhiteSpace(txt_item_code.Text) ? "Item Code cannot be empty." : null;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Helpers.ShowDialogMessage("error", errorMessage);
                return;
            }
            if (currentSelectedTradeTypeIds.Count != 0 && txt_id.Text != "")
            {
                txt_trade_type.Tag = currentSelectedTradeTypeIds;
            }
            // Get Data
            var data = Helpers.GetControlsValues(pnl_header);
            var itemprice = Helpers.GetControlsValues(pnl_item_sales_price);

            ApiResponseModel response = new ApiResponseModel();

            

            data["is_stop_selling"] = data.TryGetValue("is_stop_selling", out var value) && value.ToString() == "1";

            if (itemprice.TryGetValue("price", out var priceValue))
            {
                data["price"] = priceValue;
            }
            data["price"] = float.TryParse(data["price"]?.ToString(), out float price) ? price : 0f;
            data["itemspecs"] = GetItemSpecs();

            if (currentSelectedPumpTypeIds.Count != 0 && txt_id.Text != "")
            {
                txt_pump_type_compatability.Tag = currentSelectedPumpTypeIds;
            }
            data["additionalspecs"] = GetAdditionalSpecs();
            data["itemimages"] = imageData;


            // Determine if this is a new record    
            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                data.Remove("id"); 
            }
            else if (int.TryParse(txt_id.Text, out int recordId))
            {
                data["id"] = recordId;
                
            }
            else
            {
                Helpers.ShowDialogMessage("error", "Invalid ID format.");
                //update json for itemimages
                return;
            }
            response = isNewRecord
                ? await ItemServices.Insert(data)
                : await ItemServices.Update(data);
            
            

            isProgrammaticChange = false;
            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);

            if (response.Success)
            {
                Helpers.ResetControls(pnl_header);
                GetData();
                BtnToggle(false);
                currentSelectedTradeTypeIds.Clear();
                txt_trade_type.Tag = 0;
                currentSelectedPumpTypeIds.Clear();
                txt_pump_type_compatability.Tag = 0;
                
                imageData.Clear();
                newbase64Images.Clear();
                replaceBase64Images.Clear();
                imageData.Remove("newimages");
                imageData.Remove("replaceimages");
            }
        }
        private Dictionary<string, object> GetItemSpecs()
        {
            dgv_template.EndEdit();
            dgv_template.CommitEdit(DataGridViewDataErrorContexts.Commit);

            var fieldsList = dgv_template.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(row => new Dictionary<string, object>
                {
            { "title", row.Cells["title"].Value ?? "" },
            { "value", row.Cells["value"].Value ?? "" }
                }).ToList();

            return new Dictionary<string, object>
            {
                { "id", int.TryParse(txt_item_specs_id.Text, out int id) ? id : 0 },
                { "based_id", int.TryParse(txt_item_specs_based_id.Text, out int basedId) ? basedId : 0 },
                { "template", cmb_template.Text },
                { "fields", fieldsList },
                { "manufacturer_origin", txt_manufacturer_origin.Text }
            };
        }
        private Dictionary<string, object> GetAdditionalSpecs()
        {

            if (currentSelectedPumpTypeIds.Count != 0 && txt_id.Text != "")
            {
                txt_pump_type_compatability.Tag = currentSelectedPumpTypeIds;
            }

            var additionalspecs = Helpers.GetControlsValues(pnl_additional_specs);

            additionalspecs["weight"] = float.TryParse(additionalspecs["weight"]?.ToString(), out float weight) ? weight : 0f;
            additionalspecs["volume"] = float.TryParse(additionalspecs["volume"]?.ToString(), out float volume) ? volume : 0f;
            //additionalspecs["length"] = float.TryParse(additionalspecs["length"]?.ToString(), out float length) ? length : 0f;
            //additionalspecs["height"] = float.TryParse(additionalspecs["height"]?.ToString(), out float height) ? height : 0f;

            additionalspecs["id"] = uint.TryParse(txt_additional_specs_id.Text, out uint additionalId) ? additionalId : 0;
            additionalspecs["based_id"] = uint.TryParse(txt_additional_specs_based_id.Text, out uint additionalBasedId) ? additionalBasedId : 0;

            return additionalspecs;
        }
        //private Dictionary<string, object> GetItemImages()
        //{
        //    var itemImages = Helpers.GetControlsValues(pnl_item_image);
        //    var base64Images = flowLayoutPanel1.Controls
        //        .OfType<PictureBox>()
        //        .Where(pictureBox => pictureBox.Image != null)
        //        .Select(pictureBox => ConvertImageToBase64(pictureBox.Image))
        //        .ToList();

        //    itemImages["id"] = uint.TryParse(txt_item_image_id.Text, out uint itemImageId) ? itemImageId : 0;
        //    itemImages["based_id"] = uint.TryParse(txt_item_image_based_id.Text, out uint itemImageBasedId) ? itemImageBasedId : 0;
        //    itemImages["images"] = base64Images;

        //    return itemImages;
        //}
        private void btn_new_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
            ResetPanels(
                pnl_additional_specs,
                pnl_item_specs, pnl_header,
                pnl_item_image,
                pnl_item_sales_price
            );

            //isProgrammaticChange = false;
            ResetComboBoxes(
                cmb_template,
                cmb_item_name,
                cmb_item_class,
                cmb_item_brand,
                cmb_unit_of_measure,
                cmb_item_tangibility_type,
                cmb_material,
                cmb_connection_type,
                cmb_pump_count_compatability,
                cmb_volume_unit_of_measure,
                cmb_weight_unit_of_measure
            );
           

            dgv_template.DataSource = null;
            chk_is_stop_selling.Checked = false;

            if (dgv_template.Columns["title"] != null)
            {
                dgv_template.Columns["title"].ReadOnly = true;
            }

            flowLayoutPanel1.Controls.Clear();
            img_preview.Image = null;
            currentSelectedTradeTypeIds.Clear();
            txt_trade_type.Tag = "MULTI";
            currentSelectedPumpTypeIds.Clear();
            txt_pump_type_compatability.Tag = "MULTI";
            RemoveSelectedDataTable(CacheData.PumpType);
            RemoveSelectedDataTable(CacheData.ItemType);
            //isProgrammaticChange = false;
        }
        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
            txt_trade_type.Tag = "MULTI";
            txt_pump_type_compatability.Tag = "MULTI";
            img_preview.Image = null;
            txt_item_image_id.Text = null;
            
            if (dgv_template.Columns["title"] != null)
            {
                dgv_template.Columns["title"].ReadOnly = true;
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
            GetData();
            newbase64Images.Clear();
            replaceBase64Images.Clear();
            imageData.Remove("newimages");
            imageData.Remove("replaceimages");
            // Should clear input for fields not being save
        }
        private void GetItemValues()
        {
            // Items
            cmb_item_name.SelectedValue = records.items[this.selectedRecord].item_name_id;
            cmb_item_name.SelectedItem = records.items[this.selectedRecord].item_name_id;

            cmb_item_class.SelectedValue = records.items[this.selectedRecord].item_class_id;
            cmb_item_class.SelectedItem = records.items[this.selectedRecord].item_class_id;

            cmb_item_brand.SelectedValue = records.items[this.selectedRecord].item_brand_id;
            cmb_item_brand.SelectedItem = records.items[this.selectedRecord].item_brand_id;

            cmb_unit_of_measure.SelectedValue = records.items[this.selectedRecord].unit_of_measure_id;
            cmb_unit_of_measure.SelectedItem = records.items[this.selectedRecord].unit_of_measure_id;

            cmb_item_class.SelectedValue = records.items[this.selectedRecord].item_class_id;
            cmb_item_class.SelectedItem = records.items[this.selectedRecord].item_class_id;

            // Additional Specs
            cmb_material.SelectedValue = records.additionalspecs[this.selectedRecord].material_id;
            cmb_material.SelectedItem = records.additionalspecs[this.selectedRecord].material_id;

            cmb_pump_count_compatability.SelectedValue = records.additionalspecs[this.selectedRecord].pump_count_compatability_id;
            cmb_pump_count_compatability.SelectedItem = records.additionalspecs[this.selectedRecord].pump_count_compatability_id;

            cmb_volume_unit_of_measure.SelectedValue = records.additionalspecs[this.selectedRecord].volume_unit_of_measure_id;
            cmb_volume_unit_of_measure.SelectedItem = records.additionalspecs[this.selectedRecord].volume_unit_of_measure_id;

            cmb_weight_unit_of_measure.SelectedValue = records.additionalspecs[this.selectedRecord].weight_unit_of_measure_id;
            cmb_weight_unit_of_measure.SelectedItem = records.additionalspecs[this.selectedRecord].weight_unit_of_measure_id;
        }
        private async Task GetClassSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_CLASS);
            CacheData.ItemClass = await serviceSetup.GetAsDatatable();

            cmb_item_class.DataSource = CacheData.ItemClass;
            cmb_item_class.ValueMember = "id";
            cmb_item_class.DisplayMember = "name";
        }
        private async Task GetNameSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_NAME);
            CacheData.ItemName = await serviceSetup.GetAsDatatable();

            cmb_item_name.DataSource = CacheData.ItemName;
            cmb_item_name.ValueMember = "id";
            cmb_item_name.DisplayMember = "name";
        }
        private async Task GetBrandSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.BRAND);
            CacheData.ItemBrand = await serviceSetup.GetAsDatatable();

            DataRow newRow = CacheData.ItemBrand.NewRow();
            newRow["id"] = DBNull.Value;  // Or assign a default value like 0
            newRow["name"] = "-- No Brand --"; // Display text for empty selection

            CacheData.ItemBrand.Rows.InsertAt(newRow, 0);
            cmb_item_brand.DataSource = CacheData.ItemBrand;
            cmb_item_brand.ValueMember = "id";
            cmb_item_brand.DisplayMember = "name";
        }
        private async Task GetUOMSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.UNIT_OF_MEASURMENT);
            DataTable originalData = await serviceSetup.GetAsDatatable();

            CacheData.UnitOfMeasurement = originalData;

            DataView dvUnit = new DataView(originalData);
            DataView dvWeight = new DataView(originalData);
            DataView dvVolume = new DataView(originalData);
            DataView dvHeight = new DataView(originalData);
            DataView dvLength = new DataView(originalData);

            cmb_unit_of_measure.DataSource = dvUnit;
            cmb_unit_of_measure.ValueMember = "id";
            cmb_unit_of_measure.DisplayMember = "name";

            cmb_weight_unit_of_measure.DataSource = dvWeight;
            cmb_weight_unit_of_measure.ValueMember = "id";
            cmb_weight_unit_of_measure.DisplayMember = "name";

            cmb_volume_unit_of_measure.DataSource = dvVolume;
            cmb_volume_unit_of_measure.ValueMember = "id";
            cmb_volume_unit_of_measure.DisplayMember = "name";
        }
        private async Task GetItemTradeType()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_TYPE);
            CacheData.ItemType = await serviceSetup.GetAsDatatable();
        }
        private async Task GetMaterialSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_MATERIAL);
            CacheData.Material = await serviceSetup.GetAsDatatable();

            cmb_material.DataSource = CacheData.Material;
            cmb_material.ValueMember = "id";
            cmb_material.DisplayMember = "name";
        }
        private async Task GetPumpTypeSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_PUMP_TYPE);
            CacheData.PumpType = await serviceSetup.GetAsDatatable();
        }
        private async Task GetPumpCountSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_PUMP_COUNT);
            CacheData.PumpCount = await serviceSetup.GetAsDatatable();

            cmb_pump_count_compatability.DataSource = CacheData.PumpCount;
            cmb_pump_count_compatability.ValueMember = "id";
            cmb_pump_count_compatability.DisplayMember = "name";
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.items.Rows.Count - 1 > this.selectedRecord)
            {
                RemoveSelectedDataTable(CacheData.PumpType);
                RemoveSelectedDataTable(CacheData.ItemType);
                this.selectedRecord++;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {
                RemoveSelectedDataTable(CacheData.PumpType);
                RemoveSelectedDataTable(CacheData.ItemType);
                this.selectedRecord--;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btn_add_class_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.ItemClass.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Class", ENUM_ENDPOINT.ITEM_CLASS, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void btn_add_name_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.ItemName.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("General Name", ENUM_ENDPOINT.ITEM_NAME, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void btn_add_brand_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.ItemBrand.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Brand", ENUM_ENDPOINT.BRAND, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void btn_add_material_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.Material.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Material", ENUM_ENDPOINT.ITEM_MATERIAL, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void cmb_pump_type_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.PumpType.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Pump Types", ENUM_ENDPOINT.ITEM_PUMP_TYPE, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void cmb_pump_count_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.PumpCount.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("Brand", ENUM_ENDPOINT.ITEM_PUMP_COUNT, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void btn_add_oum_Click(object sender, EventArgs e)
        {
            AddUOM();
        }
        private void add_volume_uom_Click(object sender, EventArgs e)
        {
            AddUOM();
        }
        private void add_weight_uom_Click(object sender, EventArgs e)
        {
            AddUOM();
        }
        private void add_height_uom_Click(object sender, EventArgs e)
        {
            AddUOM();
        }
        private void add_length_uom_Click(object sender, EventArgs e)
        {
            AddUOM();
        }
        private void AddUOM()
        {
            DataTable dt = CacheData.UnitOfMeasurement.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("General Name", ENUM_ENDPOINT.UNIT_OF_MEASURMENT, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void ResetComboBoxes(params ComboBox[] comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = 0;
            }
        }
        private void ResetPanels(params Panel[] panels)
        {
            foreach (var panel in panels)
            {
                Helpers.ResetControls(panel);
            }
        }
        private void cmb_template_SelectedIndexChanged(object sender, EventArgs e)
       {
            if (!btn_new.Visible)
            {
                string selectedText = cmb_template.Text;
                var templateMapping = new Dictionary<string, Func<DataTable>>
                    {
                        { "CONTROLLER", ENUM_ITEM_SPECS.CONTROLLER },
                        { "COMMON PACKAGE", ENUM_ITEM_SPECS.COMMON_PACKAGE },
                        { "PUMP", ENUM_ITEM_SPECS.PUMP },
                        { "COMMON HEADER", ENUM_ITEM_SPECS.VALVE },
                        { "VALVE", ENUM_ITEM_SPECS.VALVE },
                        { "RUBBER BELO", ENUM_ITEM_SPECS.VALVE },
                        { "PRESSURE TRANSDUCER", ENUM_ITEM_SPECS.PRESSURE_TRANSDUCER },
                        { "PRESSURE SWITCH", ENUM_ITEM_SPECS.PRESSURE_TRANSDUCER },
                        { "WATER METER", ENUM_ITEM_SPECS.WATER_METER },
                        { "FLOW METER", ENUM_ITEM_SPECS.WATER_METER }
                    };
                if (templateMapping.TryGetValue(selectedText, out var getTemplate))
                {
                    dgv_template.DataSource = null;
                    dgv_template.Rows.Clear();
                    dt_template = getTemplate();
                    dgv_template.DataSource = dt_template;
                }
            }
        }
        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Text = string.Empty;
            }
        }
        private void HideColumns(DataGridView dgv, params string[] columnNames)
        {
            foreach (var col in columnNames)
            {
                if (dgv.Columns.Contains(col))
                {
                    dgv.Columns[col].Visible = false;
                }
            }
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (items == null || items.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] columnsToShow = { "id", "item_name", "short_desc" };

            using (SearchModal searchModal = new SearchModal("Search Items", items, columnsToShow))
            {
                if (searchModal.ShowDialog() == DialogResult.OK)
                {
                    int selectedIndex = searchModal.SelectedIndex;

                    if (selectedIndex >= 0)
                    {
                        this.selectedRecord = selectedIndex;
                        Bind(true);
                    }
                }
            }
        }
        private void cmb_item_tangibility_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);
        }
        private void txt_trade_status_TextChanged(object sender, EventArgs e)
        {
            ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);
        }
        private void FetchDataGridViewChild()
        {
            //Fetch Additional Specs
            DataView dataViewAdditionalSpecs = new DataView(additionalspecs);
            if (dataViewAdditionalSpecs.Count != 0)
            {
                dataViewAdditionalSpecs.RowFilter = "based_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
            }
            //Fetch Item Purchasing
            DataView dataViewPurchasing = new DataView(itempurchasing);

            if (dataViewPurchasing.Count != 0)
            {
                dataViewPurchasing.RowFilter = "based_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourcePurchasing.DataSource = dataViewPurchasing;
            }

            //Fetch Item Sales
            DataView dataViewSales = new DataView(itemsales);
            if (dataViewSales.Count != 0)
            {
                dataViewSales.RowFilter = "based_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourceSales.DataSource = dataViewSales;
            }

            //Fetch Item Production
            DataView dataViewProduction= new DataView(itemproduction);
            if (dataViewProduction.Count != 0)
            {
                dataViewProduction.RowFilter = "item_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourceProduction.DataSource = dataViewProduction;
            }
        }
        private void btn_add_supplier_Click(object sender, EventArgs e)
        {
            BusnessPartnerInfoModal modal = new BusnessPartnerInfoModal();
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ShowDialog();
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            if (clickedPictureBox != null)
            if (clickedPictureBox != null)
            {
                int index = imageBoxes.IndexOf(clickedPictureBox);
                if (index != -1 && item_img.ContainsKey($"img{index + 1}"))
                {
                    string filePath = item_img[$"img{index + 1}"];
                    img_preview.Image = Image.FromFile(filePath);
                    img_preview.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }
        private void btn_upload_image_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        using (var stream = new MemoryStream(File.ReadAllBytes(filePath)))
                        {
                            stream.Position = 0;
                            Image image = Image.FromStream(stream);
                            int tempImageId = tempImageIdCounter--;
                            imageFilePaths[tempImageId] = filePath;
                            txt_item_image_id.Text = tempImageId.ToString();

                            flowLayoutPanel1.Controls.Add(CreatePictureBox(image, filePath, tempImageId));

                            img_preview.Image = image;
                            img_preview.SizeMode = PictureBoxSizeMode.Zoom;

                            string base64String = ConvertImageToBase64(image, ImageFormat.Jpeg, 50L);
                            if (!string.IsNullOrEmpty(base64String))
                            {
                                newbase64Images.Add(base64String);
                            }
                        }
                    }

                    imageData["newimages"] = newbase64Images;
                }
            }
        }
        private void btn_replace_image_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_item_image_id.Text) || !int.TryParse(txt_item_image_id.Text, out int imageId))
            {
                MessageBox.Show("Select an image to replace.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    using (var stream = new MemoryStream(File.ReadAllBytes(filePath)))
                    {
                        stream.Position = 0; // Reset stream position
                        using (Image image = Image.FromStream(stream))
                        {
                            if (imageFilePaths.ContainsKey(imageId))
                            {
                                imageFilePaths[imageId] = filePath;
                            }

                            // Instead of removing and re-adding, just update the existing PictureBox image
                            PictureBox existingPictureBox = flowLayoutPanel1.Controls
                                .OfType<PictureBox>()
                                .FirstOrDefault(pb => pb.Tag is ImageTag tag && tag.Id == imageId);

                            if (existingPictureBox != null)
                            {
                                existingPictureBox.Image?.Dispose(); // Dispose old image
                                existingPictureBox.Image = (Image)image.Clone(); // Update image
                            }

                            img_preview.Image?.Dispose(); // Dispose old preview image
                            img_preview.Image = (Image)image.Clone();
                            img_preview.SizeMode = PictureBoxSizeMode.Zoom;

                            string base64String = ConvertImageToBase64(image, ImageFormat.Jpeg, 50L);
                            if (!string.IsNullOrEmpty(base64String))
                            {
                                replaceBase64Images.RemoveAll(d => Convert.ToInt32(d["imageid"]) == imageId);
                                replaceBase64Images.Add(new Dictionary<string, object>
                            {
                                { "imageid", imageId },
                                { "newimage", base64String }
                            });
                            }

                            imageData["replaceimages"] = replaceBase64Images;
                        }
                    }
                }
            }
        }
        private void btn_remove_image_Click(object sender, EventArgs e)
        {
            if (img_preview.Image == null)
            {
                MessageBox.Show("No image selected to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PictureBox pictureToRemove = null;
            int? imageId = null;

            // Find the PictureBox that matches the preview image
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Image == img_preview.Image)
                {
                    pictureToRemove = pictureBox;
                    imageId = int.TryParse(txt_item_image_id.Text, out int parsedId) ? parsedId : (int?)null;
                    break;
                }
            }

            if (pictureToRemove != null && imageId.HasValue)
            {
                // Confirm before removing
                DialogResult result = MessageBox.Show("Are you sure you want to remove this image?",
                                                      "Confirm Removal",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    flowLayoutPanel1.Controls.Remove(pictureToRemove);
                    pictureToRemove.Dispose();
                    img_preview.Image = null;
                    txt_item_image_id.Clear();

                    // Remove from `imageFilePaths`
                    if (imageFilePaths.ContainsKey(imageId.Value))
                    {
                        imageFilePaths.Remove(imageId.Value);
                    }

                    // Check if the image is from `newbase64Images` (temporary added images)
                    if (imageId.Value < 0) // Temporary images have negative IDs
                    {
                        int index = newbase64Images.Count - Math.Abs(imageId.Value);
                        if (index >= 0 && index < newbase64Images.Count)
                        {
                            newbase64Images.RemoveAt(index);
                        }
                        imageData["newimages"] = newbase64Images;
                    }

                    // Check if the image was replaced in `replaceBase64Images`
                    replaceBase64Images.RemoveAll(d => d["imageid"].Equals(imageId.Value));
                    imageData["replaceimages"] = replaceBase64Images;

                    // Add imageId to `deleteimages` if it is an existing image
                    if (imageId.Value >= 0) // Only existing images should be deleted from DB
                    {
                        if (!imageData.ContainsKey("deleteimages"))
                        {
                            imageData["deleteimages"] = new List<Dictionary<string, int>>();
                        }

                        var deleteImages = (List<Dictionary<string, int>>)imageData["deleteimages"];
                        if (!deleteImages.Any(d => d["imageid"] == imageId.Value))
                        {
                            deleteImages.Add(new Dictionary<string, int> { { "imageid", imageId.Value } });
                        }
                        imageData["deleteimages"] = deleteImages;
                    }

                    Console.WriteLine($"Image {imageId} removed successfully.");
                }
            }
            else
            {
                MessageBox.Show("Image not found in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private PictureBox CreatePictureBox(Image image, string filePath, int imageId)
        {
            PictureBox pictureBox = new PictureBox
            {
                Width = 100,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = image,
                Margin = new Padding(5),
                Tag = new ImageTag { Id = imageId, Path = filePath }
            };

            pictureBox.Click += PictureBox_Clicked;
            return pictureBox;
        }
        private void PictureBox_Clicked(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox && pictureBox.Tag is ImageTag tag)
            {
                img_preview.Image = pictureBox.Image;
                img_preview.SizeMode = PictureBoxSizeMode.Zoom;

                // Display the ID of the clicked image
                txt_item_image_id.Text = tag.Id.ToString();
            }
        }
        private bool CheckIfCalpeda()
        {
            if (cmb_item_brand.SelectedValue != null)
            {
                string selectedBrandId = cmb_item_brand.SelectedValue.ToString();

                DataRow[] rows = CacheData.ItemBrand.Select("name = 'CALPEDA'");
                if (rows.Length > 0)
                {
                    string calpedaId = rows[0]["id"].ToString();

                    if (selectedBrandId == calpedaId && string.IsNullOrWhiteSpace(txt_catalogue_year.Text))
                    {
                        Helpers.ShowDialogMessage("error", "Catalogue Year is required for CALPEDA.");

                        txt_catalogue_year.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        private void BtnToggle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_edit.Visible = !isEdit;
            btn_search.Visible = !isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            pnl_header.Enabled = isEdit;
            pnl_item_specs.Enabled = isEdit;
            pnl_item_specs.Enabled = isEdit;
            pnl_additional_specs.Enabled = isEdit;
            btn_upload_image.Enabled = isEdit;
            btn_replace_image.Enabled = isEdit;
            btn_remove_image.Enabled = isEdit;
            txt_trade_type.ReadOnly = isEdit;
            txt_pump_type_compatability.ReadOnly = isEdit;
        }
        private void ToggleItemPages(string tradeStatusText, string tangibility)
        {
            string[] tradeStatuses = tradeStatusText.Split(',')
                                                    .Select(s => s.Trim().ToUpper())
                                                    .ToArray();
            tangibility = tangibility.ToUpper().Trim();

            ShowAllTabs();

            bool isTrade = tradeStatuses.Contains("TRADE");
            bool isNonTrade = tradeStatuses.Contains("NON-TRADE");

            //Condition 1: Trade, Non-Trade, Tangible
            if (isTrade && isNonTrade && tangibility == "TANGIBLE")
            {
                return;
            }

            //Condition 2: Trade, Non-Trade, Non-Tangible
            if (isTrade && isNonTrade && tangibility == "NON-TANGIBLE")
            {
                RemoveTabPage("tab_item_specs");
            }

            // Condition3: Non-Trade & Tangible
            if (isNonTrade && tangibility == "TANGIBLE")
            {
                RemoveTabPage("tab_sales");
            }

            // Condition4: Non-Trade & Non-Tangible
            if (!isTrade && isNonTrade && tangibility == "NON-TANGIBLE")
            {
                RemoveTabPage("tab_item_specs");
                RemoveTabPage("tab_sales");
            }
        }
        private void RemoveTabPage(string tabName)
        {
            TabPage tab = tabcontrol1.TabPages.Cast<TabPage>().FirstOrDefault(t => t.Name == tabName);
            if (tab != null)
            {
                if (!tabOrder.ContainsKey(tabName))
                {
                    tabOrder[tabName] = tabcontrol1.TabPages.IndexOf(tab);
                }

                tabcontrol1.TabPages.Remove(tab);
                hiddenTabs.Add(tab);
            }
        }
        private void ShowAllTabs()
        {
            foreach (TabPage tab in hiddenTabs.ToList())
            {
                if (!tabcontrol1.TabPages.Contains(tab))
                {
                    int index = tabOrder.ContainsKey(tab.Name) ? tabOrder[tab.Name] : tabcontrol1.TabPages.Count;
                    tabcontrol1.TabPages.Insert(index, tab);
                }
            }
            hiddenTabs.Clear();
        }
        private void btn_get_trade_type_Click(object sender, EventArgs e)
        {
            modalSelection = new SetupSelectionModal("Trade Types", ENUM_ENDPOINT.ITEM_TYPE, CacheData.ItemType, currentSelectedTradeTypeIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();

            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();
                Helpers.GetModalData(txt_trade_type, result);
                currentSelectedTradeTypeIds.Clear();
            }
        }
        private void btn_select_pump_type_Click(object sender, EventArgs e)
        {
            modalSelection = new SetupSelectionModal("Pump Types Compatability", ENUM_ENDPOINT.ITEM_PUMP_TYPE, CacheData.PumpType, currentSelectedPumpTypeIds, new List<string>(), 0);
            DialogResult modalResult = modalSelection.ShowDialog();

            if (modalResult == DialogResult.OK)
            {
                var result = modalSelection.GetResult();
                Helpers.GetModalData(txt_pump_type_compatability, result);
                currentSelectedPumpTypeIds.Clear();
            }
        }
        //private void BindMultiSelectField(List<AdditionalSpecsModel> additionalSpecs)
        //{

        //    var matchSelectedPumpType = additionalSpecs.FirstOrDefault(f => f.based_id == int.Parse(items.Rows[this.selectedRecord]["id"].ToString()));

        //    string selectedPumpType = "";
        //    if (matchSelectedPumpType != null)
        //    {
        //        txt_pump_type_compatability.Text = matchSelectedPumpType.pump_type_compatability_names;
        //        selectedPumpType = matchSelectedPumpType.pump_type_compatability_id;

        //    }

        //    currentSelectedPumpTypeIds = selectedPumpType.Split(',')
        //                                       .Where(val => int.TryParse(val, out _))
        //                                       .Select(int.Parse)
        //                                       .ToList();
        //}
        private void RemoveSelectedDataTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Columns.Contains("select"))
                {
                    row["select"] = false;
                }
            }
        }
        private string ConvertImageToBase64(Image image, ImageFormat format, long quality = 50L)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (format == ImageFormat.Jpeg)
                    {
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                        image.Save(ms, jpgEncoder, encoderParams); // Save as compressed JPEG
                    }
                    else
                    {
                        image.Save(ms, format); // Save normally for PNG/BMP
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to Base64: {ex.Message}");
                return null;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        class ImageTag
        {
            public int Id { get; set; }
            public string Path { get; set; }
        }
    }
}
