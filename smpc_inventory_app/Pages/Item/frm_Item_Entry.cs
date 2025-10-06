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
using System.Threading;
using ApiResponseModel = smpc_inventory_app.Services.Setup.ApiResponseModel;
using System.Net.Http;
using smpc_invemtory_app.Pages.Shared;

namespace smpc_inventory_app.Pages.Item
{
    public partial class frm_Item_Entry : UserControl
    {

        public delegate void getBpiAddedItem(Dictionary<string,dynamic> value);

        public event getBpiAddedItem OnItem;

        private CancellationTokenSource _imageLoadCts;
        GeneralSetupServices serviceSetup;
        SetupModal modalSetup;
        TradeTypeSelectionModal tradetypemodal = new TradeTypeSelectionModal();
        DataTable items; //parent
        DataTable itemspecs; //children
        DataTable additionalspecs;
        DataTable itempurchasing;
        DataTable itemsales;
        DataTable itemproduction;
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
        private List<int> currentSelectedPumpTypeIds = new List<int>();
        private List<int> currentSelectedTradeTypeIds = new List<int>();
        private Dictionary<int, string> imageFilePaths = new Dictionary<int, string>();
        private Dictionary<string, object> imageData = new Dictionary<string, object>();
        private List<Dictionary<string, object>> newbase64Images = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> replaceBase64Images = new List<Dictionary<string, object>>();
        private List<int> temporaryImageIds = new List<int>();
        private Dictionary<int, Bitmap> removedImages = new Dictionary<int, Bitmap>();

        private int tempImageIdCounter = -1;

        public frm_Item_Entry()
        {
            InitializeComponent();

        }
        public void HideButton()
        {
            btn_add_supplier.Visible = false;
        }
        private void frm_Item_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                BtnToggle(false);
                FetchClassSetup(); 
                FetchNameSetup();
                FetchBrandSetup();
                FetchUOMSetup();
                FetchItemTradeType();
                FetchMaterialSetup();
                FetchPumpTypeSetup();
                FetchPumpCountSetup();
                FetchItemData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }

        }
        private async Task LoadSetupData()
        {
            //await FetchClassSetup();
            //await FetchNameSetup();
            //await FetchBrandSetup();
            //await FetchUOMSetup();
            //await FetchItemTradeType();
            //await FetchMaterialSetup();
            //await FetchPumpTypeSetup();
            //await FetchPumpCountSetup();
        }
        private async void FetchItemData()
        {
            var response = await RequestToApi<ApiResponseModel<Items>>.Get(ENUM_ENDPOINT.ITEM);

            if (response?.Data == null || response.Data.items == null)
            {
                MessageBox.Show("No records found.");
                return;
            }

            records = response.Data;

            // heavy work off the UI thread
            var tables = await Task.Run(() => new
            {
                Items = JsonHelper.ToDataTable(records.items),
                ItemSpecs = JsonHelper.ToDataTable(records.itemspecs),
                AdditionalSpecs = JsonHelper.ToDataTable(records.additionalspecs),
                ItemImages = JsonHelper.ToDataTable(records.itemimages),
                ItemPurchasing = JsonHelper.ToDataTable(records.itempurchasing),
                ItemSales = JsonHelper.ToDataTable(records.itemsales),
                ItemProduction = JsonHelper.ToDataTable(records.itemproduction)
            });

            // back on UI thread
            items = tables.Items;
            itemspecs = tables.ItemSpecs;
            additionalspecs = tables.AdditionalSpecs;
            itemimages = tables.ItemImages;
            itempurchasing = tables.ItemPurchasing;
            itemsales = tables.ItemSales;
            itemproduction = tables.ItemProduction;

            if (records.items.Count != 0)
            {
                // ensure UI updates run on UI thread
                if (this.InvokeRequired)
                    this.BeginInvoke(new Action(() => Bind(true)));
                else
                    Bind(true);
            }
            else
            {
                MessageBox.Show("No records found.");
            }
        }


        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                GetCMBValues();
                lbl_filename.Visible = false;
                // Bind UI panels
                Panel[] pnlItem = { pnl_header };
                Panel[] pnlAdditionalSpecs = { pnl_additional_specs };
                Panel[] pnlItemSales = { pnl_item_sales_price };
                Panel[] pnlItemImages = { pnl_item_image };

                Helpers.BindControls(pnlItem, items, this.selectedRecord);
                foreach (var pnl in pnlItem)
                {
                    foreach (Control control in pnl.Controls)
                    {
                        if (control is TextBox textBox && textBox.Name.Contains("txt_item_code"))
                        {
                            if (!textBox.Text.StartsWith("I#"))
                            {
                                textBox.Text = "I#" + textBox.Text;
                            }
                        }
                    }
                }
                Helpers.BindControls(pnlItemSales, items, this.selectedRecord);
                Helpers.BindControls(pnlAdditionalSpecs, additionalspecs, this.selectedRecord);
                Helpers.BindControls(pnlItemImages, itemimages, this.selectedRecord);

                int currentItemId = Convert.ToInt32(items.Rows[selectedRecord]["id"]);

                txt_trade_type.Text = string.IsNullOrEmpty(records.items[this.selectedRecord].trade_type_names) ? "" : (records.items[this.selectedRecord].trade_type_names);
                txt_pump_type_compatability.Text = string.IsNullOrEmpty(records.additionalspecs[this.selectedRecord].pump_type_compatability_names) ? "" : (records.additionalspecs[this.selectedRecord].pump_type_compatability_names);

                //Check Item Type Before Binding
                ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);

                FetchItemDataGridViewChild();

                string tradeTypes = string.IsNullOrEmpty(records.items[this.selectedRecord].trade_type_id) ? "" : (records.items[this.selectedRecord].trade_type_id);

                //Getting the List of Ids to match in my getmodal
                currentSelectedTradeTypeIds = tradeTypes.Split(',')
                                                    .Where(val => int.TryParse(val, out _))
                                                    .Select(int.Parse)
                                                    .ToList();
                txt_trade_type.Tag = currentSelectedTradeTypeIds;

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

                var matchingAdditionalSpec = records.additionalspecs.FirstOrDefault(spec => spec.based_id == currentItemId);

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
                        imageUrl = imageUrl.StartsWith("http")
                            ? imageUrl
                            : "http://localhost:3000/api/vfile/" + imageUrl;

                        // placeholder first
                        PictureBox placeholder = CreatePictureBox(Properties.Resources.spinner, imageUrl, imageRecord.id, imageRecord.filename);
                        flowLayoutPanel1.Controls.Add(placeholder);

                        // async download
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                using (HttpClient http = new HttpClient())
                                {
                                    byte[] data = await http.GetByteArrayAsync(imageUrl);
                                    using (MemoryStream ms = new MemoryStream(data))
                                    {
                                        Image img = Image.FromStream(ms);
                                        placeholder.Invoke((MethodInvoker)(() =>
                                        {
                                            placeholder.Image = img;
                                        }));
                                    }
                                }
                            }
                            catch
                            {
                                object v = placeholder.Invoke((MethodInvoker)(() =>
                                {
                                    placeholder.Image = Properties.Resources.no_pictures;
                                }));
                            }
                        });

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
        private bool IsTextResponse(byte[] data)
        {
            string content = Encoding.UTF8.GetString(data).ToLower();
            return content.Contains("file not found") || content.Contains("<html>");
        }
        private async void btn_save_Click(object sender, EventArgs e)
        {
            ApiResponseModel response = new ApiResponseModel();
            btn_save.Enabled = false;

            isProgrammaticChange = true;
            if (!CheckIfCalpeda())
            {
                return;
            }  

            if (currentSelectedTradeTypeIds.Count != 0 && txt_id.Text != "")
            {
                txt_trade_type.Tag = currentSelectedTradeTypeIds;
            }
            // Get Data
            var data = Helpers.GetControlsValues(pnl_header);
            var itemprice = Helpers.GetControlsValues(pnl_item_sales_price);

            if (data.ContainsKey("item_code") && data["item_code"] is string itemCode)
            {
                data["item_code"] = itemCode.StartsWith("I#")
                    ? itemCode.Substring(2)
                    : itemCode;
            }

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


            var testData = txt_trade_type.Text;


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

            //isProgrammaticChange = false;
            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);

            
            if (response.Success)
            {
                var itemResponse = response.Data["id"];

                // Invoke function of BPI 

                //    OnItem.Invoke("STRINGGG");

                BpiAddItem(response.Data["id"].ToString());

                Helpers.ResetControls(pnl_header);
                FetchItemData();
                selectedRecord = isNewRecord ? items.Rows.Count - 1 : selectedRecord;

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
            else
            {
                // Restore removed images in case of failure
                foreach (var removedImage in removedImages)
                {
                    PictureBox restoredPictureBox = new PictureBox
                    {
                        Image = removedImage.Value,
                        Width = 100,
                        Height = 100,
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Margin = new Padding(5),
                    };

                    restoredPictureBox.Tag = new ImageTag { Id = removedImage.Key };

                    flowLayoutPanel1.Controls.Add(restoredPictureBox);
                    restoredPictureBox.Click += PictureBox_Clicked;
                }

                removedImages.Clear();
            }
            btn_save.Enabled = true;

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

            additionalspecs["id"] = uint.TryParse(txt_additional_specs_id.Text, out uint additionalId) ? additionalId : 0;
            additionalspecs["based_id"] = uint.TryParse(txt_additional_specs_based_id.Text, out uint additionalBasedId) ? additionalBasedId : 0;

            return additionalspecs;
        }

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
            ItemModelGenerator();

            dg_purchasing.DataSource = null;
            dgv_sales.DataSource = null;

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
        private async void btn_close_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
            FetchItemData();

            if (dgv_template.Columns["title"] != null)
            {
                dgv_template.Columns["title"].ReadOnly = true;
            }

            // Clear input for fields not being saved
            newbase64Images.Clear();
            replaceBase64Images.Clear();
            imageData.Remove("newimages");
            imageData.Remove("replaceimages");

        }
        private void GetCMBValues()
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
        private async void FetchClassSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_CLASS);
            CacheData.ItemClass = await serviceSetup.GetAsDatatable();

            cmb_item_class.DataSource = CacheData.ItemClass;
            cmb_item_class.ValueMember = "id";
            cmb_item_class.DisplayMember = "name";
            cmb_item_class.SelectedValue = -1;
        }
        private async void FetchNameSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_NAME);
            CacheData.ItemName = await serviceSetup.GetAsDatatable();

            cmb_item_name.DataSource = CacheData.ItemName;
            cmb_item_name.ValueMember = "id";
            cmb_item_name.DisplayMember = "name";
            cmb_item_name.SelectedValue = -1;
        }
        private async void FetchBrandSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.BRAND);
            CacheData.ItemBrand = await serviceSetup.GetAsDatatable();

            DataRow newRow = CacheData.ItemBrand.NewRow();
            newRow["id"] = DBNull.Value;
            newRow["name"] = "-- Select --";

            CacheData.ItemBrand.Rows.InsertAt(newRow, 0);
            cmb_item_brand.DataSource = CacheData.ItemBrand;
            cmb_item_brand.ValueMember = "id";
            cmb_item_brand.DisplayMember = "name";
        }
        private async void FetchUOMSetup()
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
            cmb_unit_of_measure.SelectedValue = -1;

            cmb_weight_unit_of_measure.DataSource = dvWeight;
            cmb_weight_unit_of_measure.ValueMember = "id";
            cmb_weight_unit_of_measure.DisplayMember = "name";
            cmb_weight_unit_of_measure.SelectedValue = -1;

            cmb_volume_unit_of_measure.DataSource = dvVolume;
            cmb_volume_unit_of_measure.ValueMember = "id";
            cmb_volume_unit_of_measure.DisplayMember = "name";
            cmb_volume_unit_of_measure.SelectedValue = -1;
        }
        private async void FetchItemTradeType()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_TYPE);
            CacheData.ItemType = await serviceSetup.GetAsDatatable();
        }
        private async void FetchMaterialSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_MATERIAL);
            CacheData.Material = await serviceSetup.GetAsDatatable();

            cmb_material.DataSource = CacheData.Material;
            cmb_material.ValueMember = "id";
            cmb_material.DisplayMember = "name";
        }
        private async void FetchPumpTypeSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_PUMP_TYPE);
            CacheData.PumpType = await serviceSetup.GetAsDatatable();
        }
        private async void FetchPumpCountSetup()
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
        public string BpiItem(string value)
        {
            string item_recieve = value.Substring(0, 1).ToUpper();

            return item_recieve;
        }
        public void BpiAddItem(string itemId)
        {

            var itemName = cmb_item_name.Text;
            var tradeType = txt_trade_type.Text;
            var itemCode = txt_item_code.Text;
            var shortDesc = txt_short_desc.Text;
            var statusTangible = cmb_item_tangibility_type.Text;
            Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

            item.Add("item_id", itemId);
            item.Add("item_code", itemCode);
            item.Add("short_desc", shortDesc);
            item.Add("status_tangible", statusTangible);
            item.Add("status_trade", tradeType);


            OnItem?.Invoke(item);


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
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
                {
                    { "id", "ID" },
                    { "item_name", "ITEM NAME" },
                    { "short_desc", "SHORT DESCRIPTION" },
                };

            using (SearchModal searchModal = new SearchModal("Search Items", items, columnMappings))
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
        private void FetchItemDataGridViewChild()
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
            DataView dataViewProduction = new DataView(itemproduction);
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
        private void btn_upload_image_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        using (var stream = new MemoryStream(File.ReadAllBytes(filePath)))
                        {
                            stream.Position = 0;
                            using (Image tempImage = Image.FromStream(stream))
                            {
                                Image image = new Bitmap(tempImage); // <--- clone here
                                int tempImageId = tempImageIdCounter--;
                                imageFilePaths[tempImageId] = filePath;
                                string fileName = Path.GetFileName(filePath);
                                txt_item_image_id.Text = tempImageId.ToString();
                                lbl_filename.Text = fileName;
                                flowLayoutPanel1.Controls.Add(CreatePictureBox(image, filePath, tempImageId, fileName));

                                img_preview.Image = image;
                                img_preview.SizeMode = PictureBoxSizeMode.Zoom;

                                string base64String = ConvertImageToBase64(image, ImageFormat.Jpeg);
                                if (!string.IsNullOrEmpty(base64String))
                                {
                                    newbase64Images.Add(new Dictionary<string, object> {
                                        { "image", base64String},
                                        { "filename", fileName}
                                    });
                                }
                                temporaryImageIds.Add(tempImageId);
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
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    using (var stream = new MemoryStream(File.ReadAllBytes(filePath)))
                    {
                        string fileName = Path.GetFileName(filePath);
                        stream.Position = 0;
                        using (Image image = Image.FromFile(filePath))
                        {
                            if (imageFilePaths.ContainsKey(imageId))
                            {
                                imageFilePaths[imageId] = filePath;
                            }

                            var existingPictureBox = flowLayoutPanel1.Controls
                                .OfType<PictureBox>()
                                .FirstOrDefault(pb => pb.Tag is ImageTag tag && tag.Id == imageId);

                            if (existingPictureBox != null)
                            {
                                existingPictureBox.Image?.Dispose();
                                existingPictureBox.Image = (Image)image.Clone();
                            }

                            img_preview.Image?.Dispose();
                            img_preview.Image = (Image)image.Clone();
                            img_preview.SizeMode = PictureBoxSizeMode.Zoom;

                            string base64String = ConvertImageToBase64(image, ImageFormat.Jpeg);
                            
                            if (!string.IsNullOrEmpty(base64String))
                            {
                                if (imageId < 0)
                                {
                                    int index = temporaryImageIds.IndexOf(imageId);
                                    if (index >= 0 && index < newbase64Images.Count)
                                    {
                                        newbase64Images[index] = new Dictionary<string, object>
                                        {
                                            { "image", base64String },
                                            { "filename", fileName},
                                        };

                                        imageData["newimages"] = newbase64Images;
                                    }
                                }
                                else
                                {
                                    replaceBase64Images.RemoveAll(d => Convert.ToInt32(d["id"]) == imageId);
                                    replaceBase64Images.Add(new Dictionary<string, object>
                                    {
                                        { "id", imageId },
                                        { "image", base64String },
                                        { "fileName", fileName }
                                    });
                                    imageData["replaceimages"] = replaceBase64Images;
                                }
                            }
                        }

                    }
                }
            }
        }

        private void btn_remove_image_Click(object sender, EventArgs e)
        {
            Image clonedImage = null;
            if (img_preview.Image == null || string.IsNullOrEmpty(txt_item_image_id.Text))
            {
                MessageBox.Show("No image selected or image ID is missing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PictureBox pictureToRemove = null;
            int imageId = int.Parse(txt_item_image_id.Text);

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Image == img_preview.Image)
                {
                    pictureToRemove = pictureBox;
                    break;
                }
            }

            if (pictureToRemove != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to remove this image?",
                                                      "Confirm Removal",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {

                    // Save the image data before removing
                    removedImages[imageId] = new Bitmap(pictureToRemove.Image);


                    clonedImage = (Image)img_preview.Image.Clone();
                    img_preview.Image = new Bitmap(clonedImage);

                    pictureToRemove.Image?.Dispose();
                    pictureToRemove.Image = null;


                    flowLayoutPanel1.Controls.Remove(pictureToRemove);
                    pictureToRemove.Dispose();

                    img_preview.Image = null;
                    txt_item_image_id.Clear();

                    if (imageFilePaths.ContainsKey(imageId))
                    {
                        imageFilePaths.Remove(imageId);
                    }

                    if (imageId < 0)
                    {
                        int index = temporaryImageIds.IndexOf(imageId);
                        if (index >= 0)
                        {
                            temporaryImageIds.RemoveAt(index);
                            newbase64Images.RemoveAt(index);
                        }
                    }
                    else
                    {
                        replaceBase64Images.RemoveAll(d => (int)d["imageid"] == imageId);
                        imageData["replaceimages"] = replaceBase64Images;

                        if (!imageData.ContainsKey("deleteimages"))
                        {
                            imageData["deleteimages"] = new List<Dictionary<string, int>>();
                        }

                        var deleteImages = (List<Dictionary<string, int>>)imageData["deleteimages"];
                        if (!deleteImages.Any(d => d["imageid"] == imageId))
                        {
                            deleteImages.Add(new Dictionary<string, int> { { "imageid", imageId } });
                        }
                        imageData["deleteimages"] = deleteImages;
                    }
                }
            }
            else
            {
                MessageBox.Show("Image not found in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private PictureBox CreatePictureBox(Image image, string filePath, int imageId, string fileName)
        {
            PictureBox pictureBox = new PictureBox
            {
                Width = 100,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = image,
                Margin = new Padding(5),
                Tag = new ImageTag { Id = imageId, Path = filePath, Filename = fileName }

            };
            
            pictureBox.Click += PictureBox_Clicked;
            return pictureBox;
        }
        private void PictureBox_Clicked(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox && pictureBox.Tag is ImageTag tag)
            {
                lbl_filename.Visible = true;

                img_preview.Image = pictureBox.Image;
                img_preview.SizeMode = PictureBoxSizeMode.Zoom;
                lbl_filename.Text = tag.Filename;
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

            //  Remove tab_sales if NO TRADE exists
            if (!isTrade && isNonTrade)
            {
                RemoveTabPage("tab_sales");
            }

            // Remove tab_item_specs if non-tangible
            if (tangibility == "NON-TANGIBLE")
            {
                RemoveTabPage("tab_item_specs");
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
        private string ConvertImageToBase64(Image image, ImageFormat format, int maxSizeInBytes = 2 * 1024 * 1024)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    byte[] originalBytes = ms.ToArray();

                    if (originalBytes.Length <= maxSizeInBytes)
                    {
                        return Convert.ToBase64String(originalBytes);
                    }
                }

                using (var clonedImage = (Image)image.Clone())
                {
                    long currentQuality = 90L;
                    byte[] imageBytes = null;

                    do
                    {
                        using (var ms = new MemoryStream())
                        {
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, currentQuality);

                            clonedImage.Save(ms, jpgEncoder, encoderParams);
                            imageBytes = ms.ToArray();

                            if (imageBytes.Length <= maxSizeInBytes || currentQuality <= 10L)
                                break;

                            currentQuality -= 10L;
                        }
                    }
                    while (true);

                    return Convert.ToBase64String(imageBytes);
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
            public string Filename { get; set; }
        }
        private void ItemModelGenerator()
        {
            string item_code;

            if (items.Rows.Count > 0)
            {
                int latestIndex = items.Rows.Count - 1;
                DataRow latestRow = items.Rows[latestIndex];
                // Check if "document_no" is not null or DBNull
                if (latestRow["item_code"] != DBNull.Value && !string.IsNullOrEmpty(latestRow["item_code"].ToString()))
                {
                    if (int.TryParse(latestRow["item_code"].ToString(), out int itemNum))
                    {
                        item_code = (itemNum + 1).ToString().PadLeft(4, '0');
                    }
                    else
                    {
                        item_code = "0001";
                    }
                }
                else
                {
                    item_code = "0001";
                }
            }
            else
            {
                item_code = "0001";
            }
            txt_item_code.Text = "I#" + item_code;
        }
    }
}