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
using smpc_inventory_app.Services.Setup.Inventory;
using smpc_inventory_app.Services.Setup.Model.Warehouse;
using smpc_inventory_app.Properties;
using System.Diagnostics;

namespace smpc_inventory_app.Pages.Item
{
    public partial class frm_Item_Entry : UserControl
    {

        public delegate void getBpiAddedItem(Dictionary<string, dynamic> value);

        public event getBpiAddedItem OnItem;

        private CancellationTokenSource _imageLoadCts;
        private Dictionary<string, List<ComboBox>> _endpointCmbMap;
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
        DataTable iteminventory;
        DataTable itemavailableinv;
        DataTable dt_template;
        SetupSelectionModal modalSelection;
        private WarehouseList _warehouseData;
        DataTable warehouseName;
        DataTable warehouseArea;

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
        private Dictionary<int, Dictionary<string, object>> _pendingNewImages = new Dictionary<int, Dictionary<string, object>>();
        private List<int> temporaryImageIds = new List<int>();
        private static readonly HttpClient _httpClient = new HttpClient();
        private Dictionary<int, Bitmap> removedImages = new Dictionary<int, Bitmap>();
        private List<WarehouseAreaModel> _availableUseTypes;
        private int tempImageIdCounter = -1;
        private List<string> _zone;
        private List<string> _area;
        private List<string> _rack;
        private List<string> _level;
        private List<string> _bins;
        private bool _isCascading;
        private const string BACK_ITEM = "← Back";
        private readonly string itemEntryPath = Settings.Default.ITEMENTRYPATH;
        private TreeNode selectedNode;
        private readonly string[] systemFolders =
        {
            "CERTIFICATIONS",
            "TECHNICAL DATA SHEETS",
            "BROCHURES",
            "POST PRODUCTION REPORT"
        };


        private class CascadingTag
        {
            public string Zone { get; set; }
            public string Area { get; set; }
            public string Rack { get; set; }
            public string Level { get; set; }
            public string Bin { get; set; }
        }

        public frm_Item_Entry()
        {
            InitializeComponent();

            try
            {
                // LoadDirectory creates folders on disk (Directory.CreateDirectory against
                // Settings.ITEMENTRYPATH, e.g. "C:\Users\Public\Documents\SMPC\ITEM ENTRY").
                // This used to run unguarded directly in the constructor - any I/O failure
                // (missing drive, no write permission, path too long, etc.) threw out of the
                // constructor with no message, which made the whole Item Entry module fail to
                // open at all ("not accessible") instead of just losing the file-tree feature.
                LoadDirectory(ITEM_TV, itemEntryPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Item Entry couldn't set up its document folder (" + itemEntryPath + ") and the file browser will be unavailable, but the rest of the module will still work." +
                    Environment.NewLine + Environment.NewLine + "Details: " + ex.Message,
                    "Item Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Create ImageList
            ImageList imageList = new ImageList();
            imageList.Images.Add("folder", Properties.Resources.FolderIcon);
            imageList.Images.Add("pdf", Properties.Resources.pdf);
            imageList.Images.Add("word", Properties.Resources.word);
            imageList.Images.Add("excel", Properties.Resources.excel);
            imageList.Images.Add("image", Properties.Resources.img);
            imageList.Images.Add("file", Properties.Resources.file);

            // Assign to TreeView
            ITEM_TV.ImageList = imageList;
            ITEM_LV.SmallImageList = imageList;

            // Enable drag and drop for ListView
            ITEM_LV.AllowDrop = true;
            ITEM_LV.DragEnter += ITEM_LV_DragEnter;
            ITEM_LV.DragDrop += ITEM_LV_DragDrop;

            InitializeIdControlVisibility();
            InitializeListViewContextMenu();
            InitializeContextMenu();
            SpecsTemplateVisibility();
        }

        private void frm_Item_Entry_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCmbMap();
                BtnToggle(false);
                FetchClassSetup();
                FetchNameSetup();
                FetchBrandSetup();
                FetchUOMSetup();
                FetchItemTradeType();
                FetchMaterialSetup();
                FetchPumpTypeSetup();
                FetchPumpCountSetup();
                FetchWarehouse();
                FetchValuationMethodSetup();
                FetchItemData();

                // subscribe event
                dgv_template.CellValueChanged += dgv_template_CellValueChanged;
                dgv_template.CurrentCellDirtyStateChanged += dgv_template_CurrentCellDirtyStateChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }


        #region "Bind Records"
        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                if (items == null)
                {
                    MessageBox.Show("No records found.");
                    return;
                }

                GetCMBValues();
                lbl_filename.Visible = false;


                // Bind parent panels
                // ---- Items ----
                Panel[] pnlItem = { pnl_header, pnl_sales };

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

                int currentItemId = Convert.ToInt32(items.Rows[selectedRecord]["id"]);

                BindDataToComboBox(currentItemId);
                BindDataToPanel(currentItemId);
                BindDataToFlowLayoutPanel(currentItemId);
                BindDataToDataGridView();

                var item = records.items?.ElementAtOrDefault(this.selectedRecord);
                var additionalSpec = records.additionalspecs?.ElementAtOrDefault(this.selectedRecord);

                txt_trade_type.Text = item?.trade_type_names ?? "";
                txt_pump_type_compatability.Text = additionalSpec?.pump_type_compatability_names ?? "";

                //Check Item Type Before Binding
                ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);

                // Bind data to dgv using data binding source

                string tradeTypes = string.IsNullOrEmpty(records.items[this.selectedRecord].trade_type_id) ? "" : (records.items[this.selectedRecord].trade_type_id);

                //Getting the List of Ids to match in getmodal
                currentSelectedTradeTypeIds = tradeTypes.Split(',')
                                                    .Where(val => int.TryParse(val, out _))
                                                    .Select(int.Parse)
                                                    .ToList();
                txt_trade_type.Tag = currentSelectedTradeTypeIds;


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

                string pumpTypeIds = additionalSpec?.pump_type_compatability_id ?? "";

                //Getting the List of Ids to match in getmodal
                currentSelectedPumpTypeIds = pumpTypeIds.Split(',')
                                                    .Where(val => int.TryParse(val, out _))
                                                    .Select(int.Parse)
                                                    .ToList();
                txt_pump_type_compatability.Tag = currentSelectedPumpTypeIds;
                // Setting default value 
                if (string.IsNullOrEmpty(matchingAdditionalSpec?.connection_type))
                {
                    cmb_connection_type.SelectedIndex = -1;
                    cmb_connection_type.Text = "";
                }
                if (string.IsNullOrEmpty(matchingAdditionalSpec?.calibration))
                {
                    cmb_calibration.SelectedIndex = -1;
                    cmb_calibration.Text = "";
                }


                // Bind Inventory
                //var matchingInventory = records.iteminventory.FirstOrDefault(spec => spec.based_id == currentItemId);

                //if (matchingInventory != null)
                //{
                //    txt_item_inventory_id.Text = matchingInventory.id.ToString();
                //    txt_item_specs_based_id.Text = matchingInventory.based_id.ToString();

                //    if (string.IsNullOrEmpty(matchingInventory?.storage_type))
                //    {
                //        cmb_storage_type.SelectedIndex = -1;
                //        cmb_storage_type.Text = "";
                //    }
                //}
                //else
                //{
                //    ClearTextBoxes(txt_item_inventory_id, txt_item_inventory_based_id);
                //}
            }
            else
            {
                this.items.Rows.Clear();
                this.itemspecs.Rows.Clear();
                this.additionalspecs.Rows.Clear();
                this.itemimages.Rows.Clear();
                this.itempurchasing.Rows.Clear();
                this.itemsales.Rows.Clear();
                this.iteminventory.Rows.Clear();
            }
        }
        private void BindDataToPanel(int currentItemId)
        {
            if (currentItemId <= 0)
                return;

            Panel[] pnlItemSpecs = { pnl_item_specs };
            Panel[] pnlAdditionalSpecs = { pnl_additional_specs };
            Panel[] pnlItemImages = { pnl_item_image };
            Panel[] pnlInventoryPanel = { splitContainer1.Panel1 };

            // ---- Item Specs ----
            ItemSpecsModel currentSpec = records.itemspecs?.FirstOrDefault(x => x.based_id == currentItemId);

            SpecsTemplateVisibility();
            if (currentSpec != null)
            {
                DataTable dtCurrentSpecs = Helpers.ToDataTable(new List<ItemSpecsModel> { currentSpec });

                if (dtCurrentSpecs != null)
                {
                    dtCurrentSpecs.Columns["id"].ColumnName = "item_specs_id";
                    dtCurrentSpecs.Columns["based_id"].ColumnName = "item_specs_based_id";
                    Helpers.BindControls(pnlItemSpecs, dtCurrentSpecs);

                    List<ItemSpecstemplate> templateRows = null;
                    if (currentSpec.item_specs_template != null)
                    {
                        templateRows = currentSpec.item_specs_template
                            .Where(x => x != null && x.based_id == currentSpec.id)
                            .ToList();
                    }

                    dgv_template.AutoGenerateColumns = true;
                    dgv_template.DataSource = null;
                    dgv_template.Columns.Clear();
                    dgv_template.DataSource = templateRows?.Count > 0 ? templateRows : null;

                    string[] columnsToHide = { "id", "based_id" };
                    HideColumns(dgv_template, columnsToHide);

                    string phase = templateRows?
                        .FirstOrDefault(x => x.title == "PHASE (1 OR 3)")?.value ?? "";

                    bool showCalibration = currentSpec.template == "PUMP" || currentSpec.template == "WATER METER";
                    cmb_calibration.Visible = showCalibration;
                    lbl_calibration.Visible = showCalibration;

                    if (currentSpec.template == "PUMP")
                    {
                        bool isThreePhase = phase == "3";
                        lbl_fla.Visible = true;
                        lbl_volt.Visible = true;
                        txt_fla_1.Visible = true;
                        txt_volt_1.Visible = true;
                        txt_fla_2.Visible = isThreePhase;
                        txt_volt_2.Visible = isThreePhase;
                        lbl_impeller.Visible = true;
                        cmb_impeller.Visible = true;
                        btn_add_impeller.Visible = true;
                    }
                    else
                    {
                        SpecsTemplateVisibility();
                    }
                }
                else
                {
                    Helpers.ResetControls(pnl_item_specs);
                    dgv_template.DataSource = null;
                }
            }
            else
            {
                // currentSpec is null — reset and fall through to bind the rest
                Helpers.ResetControls(pnl_item_specs);
                dgv_template.DataSource = null;
            }

            // ---- Additional Specs ---- (always runs)
            DataView dvAdditionalSpecs = new DataView(additionalspecs)
            {
                RowFilter = $"based_id = {currentItemId}"
            };

            if (dvAdditionalSpecs.Count > 0)
            {
                DataTable additionalSpecsTable = dvAdditionalSpecs.ToTable();
                additionalSpecsTable.Columns["id"].ColumnName = "txt_additional_specs_id";
                additionalSpecsTable.Columns["based_id"].ColumnName = "txt_additional_specs_based_id";
                Helpers.BindControls(pnlAdditionalSpecs, additionalSpecsTable);
            }
            else
            {
                Helpers.ResetControls(pnl_additional_specs);
            }

            // ---- Item Images ---- (always runs)
            DataView dvImage = new DataView(itemimages)
            {
                RowFilter = $"based_id = {currentItemId}"
            };

            if (dvImage.Count > 0)
            {
                DataTable imageTable = dvImage.ToTable();
                imageTable.Columns["id"].ColumnName = "item_image_id";
                imageTable.Columns["based_id"].ColumnName = "item_image_based_id";
                Helpers.BindControls(pnlItemImages, imageTable);
            }
            else
            {
                Helpers.ResetControls(pnl_item_image);
            }

            // ---- Inventory ---- (always runs)
            DataView dvInventory = new DataView(iteminventory)
            {
                RowFilter = $"based_id = {currentItemId}"
            };

            if (dvInventory.Count > 0)
            {
                DataTable inventoryTable = dvInventory.ToTable();
                inventoryTable.Columns["id"].ColumnName = "item_inventory_id";
                inventoryTable.Columns["based_id"].ColumnName = "item_inventory_based_id";
                Helpers.BindControls(pnlInventoryPanel, inventoryTable);
            }
            else
            {
                Helpers.ResetControls(splitContainer1.Panel1);
            }
        }
        private void BindDataToComboBox(int currentItemId)
        {
            if (currentItemId <= 0)
                return;

            // Item Specs
            SetComboBoxValue(itemspecs, "based_id", currentItemId, cmb_impeller, "impeller_id");

            // Additional Specs
            SetComboBoxValue(additionalspecs, "based_id", currentItemId, cmb_material, "material_id");
            SetComboBoxValue(additionalspecs, "based_id", currentItemId, cmb_pump_count_compatability, "pump_count_compatability_id");
            SetComboBoxValue(additionalspecs, "based_id", currentItemId, cmb_volume_unit_of_measure, "volume_unit_of_measure_id");
            SetComboBoxValue(additionalspecs, "based_id", currentItemId, cmb_weight_unit_of_measure, "weight_unit_of_measure_id");


            // Inventory Tab
            SetComboBoxValue(iteminventory, "based_id", currentItemId, cmb_warehouse, "warehouse_id");
            SetComboBoxValue(iteminventory, "based_id", currentItemId, cmb_valuation_method, "valuation_method_id");
        }
        private void BindDataToDataGridView()
        {
            List<DataGridView> DgvList = new List<DataGridView>()
            {
                dgv_sales,
                dgv_purchasing,
                dgv_canvass_sheet,
                dgv_available_inventory,
                dgv_released_stock,
                dgv_bom,
                dgv_production_request
            };

            DisbleAutoColumnGeneration(DgvList);



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

            // Fetch Item Available Inventory
            DataView dataViewAvailableInv = new DataView(itemavailableinv);
            if (dataViewAvailableInv.Count != 0)
            {
                dataViewAvailableInv.RowFilter = "item_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourceInventory.DataSource = dataViewAvailableInv;
            }

            //Fetch Item Production
            DataView dataViewProduction = new DataView(itemproduction);
            if (dataViewProduction.Count != 0)
            {
                dataViewProduction.RowFilter = "item_id = '" + items.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourceProduction.DataSource = dataViewProduction;
            }
        }
        private void BindDataToFlowLayoutPanel(int currentItemId)
        {
            flowLayoutPanel1.Controls.Clear();
            img_preview.Image = null;

            var filteredImages = records.itemimages
                .Where(image => image.based_id == currentItemId)
                .ToList();

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
                if (string.IsNullOrEmpty(imageRecord.image))
                    continue;

                string imageUrl = BuildImageUrl(imageRecord.image);

                PictureBox placeholder = CreatePictureBox(
                    Properties.Resources.spinner,
                    imageUrl,
                    imageRecord.id,
                    imageRecord.filename
                );

                flowLayoutPanel1.Controls.Add(placeholder);
                _ = LoadImageAsync(placeholder, imageUrl);
            }
        }

        private static string BuildImageUrl(string imagePath)
        {
            string path = imagePath.Trim();

            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return path;

            // Use the environment-resolved base URL from Program
            return $"{Program.ApiBaseUrl}/vfile/{path}";
        }

        private async Task LoadImageAsync(PictureBox pictureBox, string imageUrl)
        {
            try
            {
                byte[] data = await _httpClient.GetByteArrayAsync(imageUrl);

                // MemoryStream must stay open — Image.FromStream needs it alive
                var ms = new MemoryStream(data);
                Image img = Image.FromStream(ms);

                if (pictureBox.IsDisposed)
                {
                    img.Dispose();
                    ms.Dispose();
                    return;
                }

                pictureBox.Invoke((MethodInvoker)(() =>
                {
                    // Dispose old image before replacing
                    pictureBox.Image?.Dispose();
                    pictureBox.Image = img;
                }));
            }
            catch
            {
                if (!pictureBox.IsDisposed)
                    pictureBox.Invoke((MethodInvoker)(() =>
                    {
                        pictureBox.Image = Properties.Resources.no_pictures;
                    }));
            }
        }
        #endregion
        #region "Get Values"
        public static Boolean ValidateControlsValues(Panel pnl)
        {
            Boolean isError = false;
            foreach (Control control in pnl.Controls)
            {
                // Handle TextBox
                if (control is TextBox textBox)
                {
                    string key = textBox.Name.Replace("txt_", "");
                    if (string.Equals(textBox.Tag as string, "REQUIRED", StringComparison.OrdinalIgnoreCase)
                        && string.IsNullOrEmpty(textBox.Text))
                    {
                        FlashRed(control);
                        isError = true;
                    }
                    else
                    {
                        control.BackColor = Color.White;
                    }
                }

                else if (control is ComboBox comboBox)
                {
                    if ((string.Equals(comboBox.Tag as string, "REQUIRED", StringComparison.OrdinalIgnoreCase)
                        && comboBox.SelectedIndex <= 0 && comboBox.Name != "cmb_volume_unit_of_measure" && comboBox.Name != "cmb_weight_unit_of_measure"))
                    {
                        FlashRed(comboBox);
                        isError = true;

                        string field = comboBox.AccessibleName ?? comboBox.Name;
                        MessageBox.Show($"{field} is required.", "Validation Error");
                    }
                    else
                    {
                        comboBox.BackColor = Color.White;
                    }
                }
            }
            return isError;
        }
        private static void FlashRed(Control control)
        {
            Color originalColor = control.BackColor;
            control.BackColor = Color.Red;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 3000; // 3 seconds
            timer.Tick += (s, e) =>
            {
                control.BackColor = originalColor;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
        private Dictionary<string, object> GetItemSpecs()
        {
            dgv_template.EndEdit();
            dgv_template.CommitEdit(DataGridViewDataErrorContexts.Commit);

            var itemspecs = Helpers.GetControlsValues(pnl_item_specs);
            var allSpecsTemplate = Helpers.ConvertDataGridViewToDataTable(dgv_template);

            Helpers.ConvertColumnToInt(allSpecsTemplate, "id");
            Helpers.ConvertColumnToInt(allSpecsTemplate, "based_id");

            // ✅ For new template rows, clear id and based_id
            // The backend will assign the correct based_id after insert
            foreach (DataRow row in allSpecsTemplate.Rows)
            {
                int rowId = 0;
                int.TryParse(row["id"]?.ToString(), out rowId);

                if (rowId == 0) // new row
                {
                    row["based_id"] = 0; // backend handles this
                }
            }

            itemspecs["item_specs_template"] = allSpecsTemplate;

            // ✅ Don't pull based_id/id from the template rows —
            // these are meaningless for new records
            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                itemspecs["based_id"] = 0; // backend will assign
                itemspecs["id"] = 0;
            }
            else
            {
                int basedId = 0;
                int id = 0;
                if (allSpecsTemplate.Rows.Count > 0)
                {
                    int.TryParse(allSpecsTemplate.Rows[0]["based_id"]?.ToString(), out basedId);
                    int.TryParse(allSpecsTemplate.Rows[0]["id"]?.ToString(), out id);
                }
                itemspecs["based_id"] = basedId;
                itemspecs["id"] = id;
            }

            return itemspecs;
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
        private Dictionary<string, object> GetItemInventory()
        {
            var iteminventory = Helpers.GetControlsValues(splitContainer1.Panel1);
            iteminventory["maximum_inventory"] = float.TryParse(iteminventory["maximum_inventory"]?.ToString(), out float max) ? max : 0f;
            iteminventory["minimum_inventory"] = float.TryParse(iteminventory["minimum_inventory"]?.ToString(), out float min) ? min : 0f;

            iteminventory["id"] = uint.TryParse(txt_item_inventory_id.Text, out uint inventoryId) ? inventoryId : 0;
            iteminventory["based_id"] = uint.TryParse(txt_item_inventory_based_id.Text, out uint inventoryBasedId) ? inventoryBasedId : 0;

            return iteminventory;
        }
        #endregion
        #region "Setups"
        private void SetComboBoxValue(DataTable table, string filterColumn, int filterValue, ComboBox combo, string valueColumn)
        {
            // Helper: select "-- SELECT --" if it exists
            void SelectDefault()
            {
                if (combo.Items.Count > 0 &&
                    combo.Items[0] is DataRowView drv &&
                    drv[combo.DisplayMember]?.ToString() == "-- SELECT --")
                {
                    combo.SelectedIndex = 0;
                }
                else
                {
                    combo.SelectedIndex = -1;
                }
            }

            // Find matching row
            var row = table.AsEnumerable()
                           .FirstOrDefault(r => Convert.ToInt32(r[filterColumn]) == filterValue);

            // No row or NULL value → default
            if (row == null || row.IsNull(valueColumn))
            {
                SelectDefault();
                return;
            }

            int value = row.Field<int>(valueColumn);

            // Value is 0 → default
            if (value == 0)
            {
                SelectDefault();
                return;
            }

            // Check if value exists in ComboBox datasource
            bool exists = combo.Items
                .OfType<DataRowView>()
                .Any(drv =>
                    drv[combo.ValueMember] != DBNull.Value &&
                    Convert.ToInt32(drv[combo.ValueMember]) == value);

            if (exists)
            {
                combo.SelectedValue = value;
            }
            else
            {
                SelectDefault();
            }
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



        }
        private static void BindCmbValues(ComboBox cmb, DataTable dt)
        {
            cmb.DataSource = dt;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            cmb.SelectedIndex = 0;
        }
        private async void FetchClassSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_CLASS);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.ItemClass = result;
            Helpers.AddCmbDefaultVal(CacheData.ItemClass);
            Helpers.BindCmbValues(cmb_item_class, CacheData.ItemClass);
        }
        private async void FetchNameSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_NAME);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.ItemName = result;
            Helpers.AddCmbDefaultVal(CacheData.ItemName);
            cmb_item_name.DataSource = CacheData.ItemName;
            cmb_item_name.ValueMember = "id";
            cmb_item_name.DisplayMember = "name";
        }
        private async void FetchBrandSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_BRAND);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.ItemBrand = result;
            Helpers.AddCmbDefaultVal(CacheData.ItemBrand);
            cmb_item_brand.DataSource = CacheData.ItemBrand;
            cmb_item_brand.ValueMember = "id";
            cmb_item_brand.DisplayMember = "name";
        }
        private void BindUOMComboBoxes(DataTable result)
        {
            DataView dvUnit = new DataView(result.Copy());
            DataView dvWeight = new DataView(result.Copy());
            DataView dvVolume = new DataView(result.Copy());

            // Apply your filters here if dvWeight/dvVolume are subsets
            // e.g. dvWeight.RowFilter = "UOMType = 'Weight'";
            //      dvVolume.RowFilter = "UOMType = 'Volume'";

            Helpers.AddCmbDefaultVal(dvUnit.Table);
            Helpers.AddCmbDefaultVal(dvWeight.Table);
            Helpers.AddCmbDefaultVal(dvVolume.Table);

            Helpers.BindCmbValues(cmb_unit_of_measure, dvUnit);
            Helpers.BindCmbValues(cmb_weight_unit_of_measure, dvWeight);
            Helpers.BindCmbValues(cmb_volume_unit_of_measure, dvVolume);
        }

        private async void FetchUOMSetup()
        {
            var serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.UNIT_OF_MEASUREMENT);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;
            CacheData.UnitOfMeasurement = result;
            BindUOMComboBoxes(result);
        }
        private async void FetchItemTradeType()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_TYPE);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.ItemType = result;
        }
        private async void FetchMaterialSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_MATERIAL);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.Material = result;

            DataView dvImpeller = new DataView(result.Copy());
            DataView dvMaterial = new DataView(result.Copy());
            Helpers.AddCmbDefaultVal(dvImpeller.Table);
            Helpers.AddCmbDefaultVal(dvMaterial.Table);

            Helpers.BindCmbValues(cmb_impeller, dvImpeller);
            Helpers.BindCmbValues(cmb_material, dvMaterial);
        }
        private async void FetchPumpTypeSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_PUMP_TYPE);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.PumpType = result;
        }
        private async void FetchPumpCountSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.ITEM_PUMP_COUNT);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.PumpCount = result;
            Helpers.AddCmbDefaultVal(CacheData.PumpCount);
            Helpers.BindCmbValues(cmb_pump_count_compatability, CacheData.PumpCount);
        }
        private async void FetchValuationMethodSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.VALUATIONMETHOD);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            CacheData.ValuationMethod = result;
            Helpers.AddCmbDefaultVal(CacheData.ValuationMethod);
            Helpers.BindCmbValues(cmb_valuation_method, CacheData.ValuationMethod);
        }
        private async void FetchWarehouse()
        {
            _warehouseData = await ReceivingReportService.GetWarehouseDetails();
            if (_warehouseData == null) return;

            warehouseName = JsonHelper.ToDataTable(_warehouseData.warehouse_name);
            warehouseArea = JsonHelper.ToDataTable(_warehouseData.warehouse_area);

            LoadActiveWarehouseName();
        }
        private void LoadActiveWarehouseName()
        {
            if (warehouseName?.Rows.Count > 0)
            {
                cmb_warehouse.DisplayMember = "name";
                cmb_warehouse.ValueMember = "id";
                cmb_warehouse.DataSource = warehouseName;
                cmb_warehouse.SelectedIndex = -1;
            }
            else
            {
                cmb_warehouse.DataSource = null;
                cmb_warehouse.Text = "No warehouse";
            }
        }
        private async Task RefreshCache(string api)
        {
            serviceSetup = new GeneralSetupServices(api);
            var result = await serviceSetup.GetAsDatatable();
            if (result == null) return;

            switch (api)
            {
                case var _ when api == ENUM_ENDPOINT.ITEM_CLASS:
                    CacheData.ItemClass = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_NAME:
                    CacheData.ItemName = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_BRAND:
                    CacheData.ItemBrand = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_TYPE:
                    CacheData.ItemType = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_MATERIAL:
                    CacheData.Material = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_PUMP_TYPE:
                    CacheData.PumpType = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.ITEM_PUMP_COUNT:
                    CacheData.PumpCount = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.VALUATIONMETHOD:
                    CacheData.ValuationMethod = result;
                    break;
                case var _ when api == ENUM_ENDPOINT.UNIT_OF_MEASUREMENT:
                    CacheData.UnitOfMeasurement = result;
                    BindUOMComboBoxes(result);
                    break;
                default:
                    return;
            }

            // Bind the corresponding ComboBox after cache update
            if (_endpointCmbMap.TryGetValue(api, out List<ComboBox> cmbs))
                foreach (var cmb in cmbs)
                    Helpers.BindCmbValues(cmb, result);
        }
        private void InitializeCmbMap()
        {
            _endpointCmbMap = new Dictionary<string, List<ComboBox>>
            {
                { ENUM_ENDPOINT.ITEM_CLASS,         new List<ComboBox> { cmb_item_class } },
                { ENUM_ENDPOINT.ITEM_NAME,          new List<ComboBox> { cmb_item_name } },
                { ENUM_ENDPOINT.ITEM_BRAND,         new List<ComboBox> { cmb_item_brand } },
                { ENUM_ENDPOINT.ITEM_MATERIAL,      new List<ComboBox> { cmb_impeller, cmb_material } }, // shared endpoint
                { ENUM_ENDPOINT.ITEM_PUMP_COUNT,    new List<ComboBox> { cmb_pump_count_compatability } },
                { ENUM_ENDPOINT.VALUATIONMETHOD,    new List<ComboBox> { cmb_valuation_method } },
                { ENUM_ENDPOINT.UNIT_OF_MEASUREMENT, new List<ComboBox> { cmb_unit_of_measure } },
            };
        }
        private void OpenSetupModal(string title, string api, DataTable cacheData)
        {
            if (cacheData == null) return;

            DataTable dt = cacheData.Copy();
            if (dt.Columns["select"] != null)
                dt.Columns.Remove("select");

            modalSetup = new SetupModal(title, api, dt);
            modalSetup.OnDataChanged += async () => await RefreshCache(api);
            modalSetup.ShowDialog();
        }
        private void btn_add_name_Click(object sender, EventArgs e) =>
            OpenSetupModal("General Name", ENUM_ENDPOINT.ITEM_NAME, CacheData.ItemName);
        private void btn_add_class_Click(object sender, EventArgs e) =>
            OpenSetupModal("Class", ENUM_ENDPOINT.ITEM_CLASS, CacheData.ItemClass);
        private void btn_add_brand_Click(object sender, EventArgs e) =>
            OpenSetupModal("Brand", ENUM_ENDPOINT.ITEM_BRAND, CacheData.ItemBrand);
        private void btn_add_impeller_Click(object sender, EventArgs e) =>
            OpenSetupModal("Impeller", ENUM_ENDPOINT.ITEM_MATERIAL, CacheData.Material);
        private void btn_add_material_Click(object sender, EventArgs e) =>
            OpenSetupModal("Material", ENUM_ENDPOINT.ITEM_MATERIAL, CacheData.Material);
        private void btn_add_valuation_method_Click(object sender, EventArgs e) =>
            OpenSetupModal("Valuation Method", ENUM_ENDPOINT.VALUATIONMETHOD, CacheData.ValuationMethod);
        private void btn_pump_type_Click(object sender, EventArgs e) =>
          OpenSetupModal("Pump Type", ENUM_ENDPOINT.ITEM_PUMP_TYPE, CacheData.PumpType);
        private void cmb_pump_count_Click(object sender, EventArgs e) =>
            OpenSetupModal("Pump Count", ENUM_ENDPOINT.ITEM_PUMP_COUNT, CacheData.PumpCount);
        private void AddUOM() =>
            OpenSetupModal("Unit of Measure", ENUM_ENDPOINT.UNIT_OF_MEASUREMENT, CacheData.UnitOfMeasurement);
        private void btn_add_oum_Click(object sender, EventArgs e) => AddUOM();
        private void add_volume_uom_Click(object sender, EventArgs e) => AddUOM();
        private void add_weight_uom_Click(object sender, EventArgs e) => AddUOM();
        private void add_height_uom_Click(object sender, EventArgs e) => AddUOM();
        private void add_length_uom_Click(object sender, EventArgs e) => AddUOM();
        // MULTISELECT --- BUG FIX OPEN SELECTION MODAL UPDATE CACHE ON DATA CHANGED
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
        #endregion
        #region "Warehouse Tab"
        private void txt_minimum_inventory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txt_maximum_inventory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void cmb_default_zone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                cmb_default_zone.Text = "";
                cmb_default_zone.Items.Clear();
                cmb_default_zone.Items.AddRange(_zone.ToArray());

                cmb_default_zone.Tag = new CascadingTag();
                cmb_default_zone.Text = "";

                e.SuppressKeyPress = true;
            }
        }
        private void cmb_default_zone_DropDown(object sender, EventArgs e)
        {
            if (cmb_warehouse.SelectedIndex == -1)
            {
                cmb_default_zone.Items.Clear();
                cmb_default_zone.Text = string.Empty;

                MessageBox.Show("Select Warehouse first.");
                ((ComboBox)sender).DroppedDown = false;
            }
        }
        private void SetLocationItems(ComboBox combo, IEnumerable<string> items)
        {
            combo.Items.Clear();
            combo.Items.Add(BACK_ITEM);
            combo.Items.AddRange(items.ToArray());
            combo.DroppedDown = true;
        }
        private void GoBackOneLevel(ComboBox combo, CascadingTag tag)
        {
            if (!string.IsNullOrEmpty(tag.Bin))
            {
                tag.Bin = "";
                LoadBins(combo, tag);
            }
            else if (!string.IsNullOrEmpty(tag.Level))
            {
                tag.Level = "";
                LoadLevels(combo, tag);
            }
            else if (!string.IsNullOrEmpty(tag.Rack))
            {
                tag.Rack = "";
                LoadRacks(combo, tag);
            }
            else if (!string.IsNullOrEmpty(tag.Area))
            {
                tag.Area = "";
                LoadAreas(combo, tag);
            }
            else if (!string.IsNullOrEmpty(tag.Zone))
            {
                tag.Zone = "";
                LoadZones(combo);
            }
        }
        private void LoadZones(ComboBox combo)
        {
            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            var zones = warehouseArea.AsEnumerable()
                .Where(r => r.Field<int>("warehouse_name_id") == warehouseId)
                .Select(r => r.Field<string>("zone"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();

            SetLocationItems(combo, zones);
        }
        private void LoadAreas(ComboBox combo, CascadingTag tag)
        {
            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            var areas = warehouseArea.AsEnumerable()
                .Where(r =>
                    r.Field<int>("warehouse_name_id") == warehouseId &&
                    r.Field<string>("zone") == tag.Zone)
                .Select(r => r.Field<string>("area"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();

            SetLocationItems(combo, areas);
        }
        private void LoadRacks(ComboBox combo, CascadingTag tag)
        {
            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            var racks = warehouseArea.AsEnumerable()
                .Where(r =>
                    r.Field<int>("warehouse_name_id") == warehouseId &&
                    r.Field<string>("zone") == tag.Zone &&
                    r.Field<string>("area") == tag.Area)
                .Select(r => r.Field<string>("rack"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();

            SetLocationItems(combo, racks);
        }
        private void LoadLevels(ComboBox combo, CascadingTag tag)
        {
            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            var levels = warehouseArea.AsEnumerable()
                .Where(r =>
                    r.Field<int>("warehouse_name_id") == warehouseId &&
                    r.Field<string>("zone") == tag.Zone &&
                    r.Field<string>("area") == tag.Area &&
                    r.Field<string>("rack") == tag.Rack)
                .Select(r => r.Field<string>("level"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();

            SetLocationItems(combo, levels);
        }
        private void LoadBins(ComboBox combo, CascadingTag tag)
        {
            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            var bins = warehouseArea.AsEnumerable()
                .Where(r =>
                    r.Field<int>("warehouse_name_id") == warehouseId &&
                    r.Field<string>("zone") == tag.Zone &&
                    r.Field<string>("area") == tag.Area &&
                    r.Field<string>("rack") == tag.Rack &&
                    r.Field<string>("level") == tag.Level)
                .Select(r => r.Field<string>("bins"))
                .Where(b => int.TryParse(b, out _))
                .Select(int.Parse)
                .Distinct()
                .OrderBy(n => n)
                .Select(n => n.ToString());

            SetLocationItems(combo, bins);
        }
        private void cmb_warehouse_name_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmb_warehouse.SelectedValue == null)
                return;


            int warehouseId = Convert.ToInt32(cmb_warehouse.SelectedValue);

            cmb_default_zone.Items.Clear();
            cmb_default_zone.Text = "";
            cmb_default_zone.Tag = new CascadingTag();

            var zones = warehouseArea.AsEnumerable()
                .Where(r => r.Field<int>("warehouse_name_id") == warehouseId)
                .Select(r => r.Field<string>("zone"))
                .Where(z => !string.IsNullOrWhiteSpace(z))
                .Distinct()
                .ToArray();

            cmb_default_zone.Items.AddRange(zones);
            txt_default_bin_location.Text = "";
        }
        private void UpdatePath(CascadingTag tag)
        {
            txt_default_bin_location.Text =
                $"{tag.Zone}-{tag.Area}-{tag.Rack}-{tag.Level}-{tag.Bin}"
                .Trim('-')
                .Replace("--", "-");
        }
        private void txt_default_bin_location_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                MessageBox.Show("Backspace pressed!");
                // You can also cancel it if needed
                // e.SuppressKeyPress = true;
            }
        }
        private void cmb_default_zone_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (_isCascading)
                return;

            _isCascading = true;

            try
            {
                ComboBox combo = (ComboBox)sender;
                CascadingTag tag = combo.Tag as CascadingTag ?? new CascadingTag();
                string selected = combo.Text;

                if (cmb_warehouse.SelectedValue == null)
                    return;

                // 🔙 BACK handling (MUST be first)
                if (selected == BACK_ITEM)
                {
                    GoBackOneLevel(combo, tag);
                    combo.Tag = tag;
                    UpdatePath(tag);
                    return;
                }

                // Zone → Area
                if (string.IsNullOrEmpty(tag.Zone))
                {
                    tag.Zone = selected;
                    LoadAreas(combo, tag);
                }
                // Area → Rack
                else if (string.IsNullOrEmpty(tag.Area))
                {
                    tag.Area = selected;
                    LoadRacks(combo, tag);
                }
                // Rack → Level
                else if (string.IsNullOrEmpty(tag.Rack))
                {
                    tag.Rack = selected;
                    LoadLevels(combo, tag);
                }
                // Level → Bin
                else if (string.IsNullOrEmpty(tag.Level))
                {
                    tag.Level = selected;
                    LoadBins(combo, tag);
                }
                // Bin selected (final)
                else
                {
                    tag.Bin = selected;
                }

                combo.Tag = tag;
                UpdatePath(tag);
            }
            finally
            {
                _isCascading = false;
            }
        }
        #endregion "Warehouse Tab"
        #region "Toolstrip"
        private void btn_new_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
            ResetPanels(
                pnl_additional_specs,
                pnl_item_specs, pnl_header,
                pnl_item_image,
                pnl_item_sales_price,
                splitContainer1.Panel1
            );

            ResetComboBoxes(
                cmb_template, cmb_item_name, cmb_item_class,
                cmb_item_brand, cmb_unit_of_measure,
                cmb_item_tangibility_type, cmb_impeller,
                cmb_material, cmb_connection_type,
                cmb_pump_count_compatability,
                cmb_volume_unit_of_measure,
                cmb_weight_unit_of_measure, cmb_calibration,
                cmb_warehouse, cmb_storage_type,
                cmb_valuation_method
            );

            ResetCheckboxes(chk_is_stop_selling, chk_special_item);

            if (dgv_template.Columns["title"] != null)
                dgv_template.Columns["title"].ReadOnly = true;

            flowLayoutPanel1.Controls.Clear();
            img_preview.Image = null;
            lbl_filename.Text = string.Empty;

            currentSelectedTradeTypeIds.Clear();
            txt_trade_type.Tag = "MULTI";
            currentSelectedPumpTypeIds.Clear();
            txt_pump_type_compatability.Tag = "MULTI";

            RemoveSelectedDataTable(CacheData.PumpType);
            RemoveSelectedDataTable(CacheData.ItemType);

            // Full image state reset
            _pendingNewImages.Clear();
            replaceBase64Images.Clear();
            imageData.Clear();
            removedImages.Clear();
            imageFilePaths.Clear();
            tempImageIdCounter = -1;

            ItemModelGenerator();

            // Bug #265: dgv_purchasing/dgv_sales are bound to bindingSourcePurchasing/
            // salesBindingSource (set at design time). Setting .DataSource directly to null
            // here disconnects the grid from its binding source entirely, so later calls to
            // BindDataToDataGridView() - which only reassign bindingSourcePurchasing.DataSource,
            // never dgv_purchasing.DataSource itself - can no longer get anything to show:
            // the grid stayed pointed at "null" instead of the binding source, so the
            // Purchasing tab appeared empty even for records with data (Click New -> Close ->
            // Purchasing). Clear through the binding source instead so the grid/source link
            // stays intact for the next bind.
            bindingSourcePurchasing.DataSource = null;
            salesBindingSource.DataSource = null;
        }
        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
            txt_trade_type.Tag = "MULTI";
            txt_pump_type_compatability.Tag = "MULTI";
            img_preview.Image = null;
            lbl_filename.Text = string.Empty; ;
            txt_item_image_id.Text = null;

            if (dgv_template.Columns["title"] != null)
            {
                dgv_template.Columns["title"].ReadOnly = true;
            }
        }
        private async void btn_save_Click(object sender, EventArgs e)
        {
            ApiResponseModel response = new ApiResponseModel();
            btn_save.Enabled = false;

            bool hasError = ValidateControlsValues(pnl_header) | ValidateControlsValues(pnl_additional_specs);

            if (hasError)
            {
                Helpers.ShowDialogMessage("error", "Please fill in all required fields.");
                btn_save.Enabled = true;
                return;
            }

            isProgrammaticChange = true;
            if (!CheckIfCalpeda())
                return;

            if (currentSelectedTradeTypeIds.Count != 0 && txt_id.Text != "")
                txt_trade_type.Tag = currentSelectedTradeTypeIds;

            var data = Helpers.GetControlsValues(pnl_header);
            var itemprice = Helpers.GetControlsValues(pnl_item_sales_price);

            if (data.ContainsKey("item_code") && data["item_code"] is string itemCode)
            {
                data["item_code"] = itemCode.StartsWith("I#")
                    ? itemCode.Substring(2)
                    : itemCode;
            }

            if (itemprice.TryGetValue("price", out var priceValue))
                data["price"] = priceValue;

            data["price"] = float.TryParse(data["price"]?.ToString(), out float price) ? price : 0f;
            data["itemspecs"] = GetItemSpecs();

            if (data["itemspecs"] is Dictionary<string, object> itemSpecs && itemSpecs["item_specs_template"] is DataTable dt)
            {
                foreach (DataColumn col in dt.Columns)
                    Debug.WriteLine($"Column: {col.ColumnName} | DataType: {col.DataType}");
            }

            data["additionalspecs"] = GetAdditionalSpecs();

            // Build imageData cleanly before attaching
            if (_pendingNewImages.Count > 0)
                imageData["newimages"] = _pendingNewImages.Values.ToList();
            else
                imageData.Remove("newimages");

            data["itemimages"] = imageData;
            data["iteminventory"] = GetItemInventory();

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
                btn_save.Enabled = true;
                return;
            }

            response = isNewRecord
                ? await ItemServices.Insert(data)
                : await ItemServices.Update(data);

            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);

            if (response.Success)
            {
                BpiAddItem(response.Data["id"].ToString());

                Helpers.ResetControls(pnl_header);
                FetchItemData();
                selectedRecord = isNewRecord ? items.Rows.Count - 1 : selectedRecord;

                BtnToggle(false);
                currentSelectedTradeTypeIds.Clear();
                txt_trade_type.Tag = 0;
                currentSelectedPumpTypeIds.Clear();
                txt_pump_type_compatability.Tag = 0;

                // Full image state reset
                _pendingNewImages.Clear();
                replaceBase64Images.Clear();
                imageData.Remove("newimages");
                imageData.Remove("replaceimages");
                imageData.Remove("deleteimages");
                removedImages.Clear();
            }
            else
            {
                // Restore removed images on failure
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
        private void btn_close_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
            FetchItemData();

            if (dgv_template.Columns["title"] != null)
                dgv_template.Columns["title"].ReadOnly = true;

            // Full image state reset
            _pendingNewImages.Clear();
            replaceBase64Images.Clear();
            imageData.Remove("newimages");
            imageData.Remove("replaceimages");
            imageData.Remove("deleteimages");
            removedImages.Clear();
        }
        private void ChangeRecord(int step)
        {
            if (items == null || items.Rows.Count == 0) return;

            int newIndex = this.selectedRecord + step;
            if (newIndex < 0 || newIndex >= items.Rows.Count) return;

            RemoveSelectedDataTable(CacheData.PumpType);
            RemoveSelectedDataTable(CacheData.ItemType);

            this.selectedRecord = newIndex;
            Bind(true);

            btn_prev.Enabled = this.selectedRecord > 0;
            btn_next.Enabled = this.selectedRecord < items.Rows.Count - 1;
        }
        private void btn_next_Click(object sender, EventArgs e) => ChangeRecord(1);
        private void btn_prev_Click(object sender, EventArgs e) => ChangeRecord(-1);
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
                    { "item_code", "ITEM CODE" },
                    { "item_name", "ITEM NAME" },
                    { "item_model", "MODEL" },
                    { "item_brand", "BRAND" },
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
        #endregion
        #region "BPI Form"
        public void HideButton()
        {
            btn_add_supplier.Visible = false;
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
            var statusTangible = cmb_item_tangibility_type.Text;
            Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

            item.Add("item_id", itemId);
            item.Add("item_code", itemCode);
            item.Add("status_tangible", statusTangible);
            item.Add("status_trade", tradeType);

            OnItem?.Invoke(item);
        }
        #endregion
        #region "Utils"
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
        private void ResetComboBoxes(params ComboBox[] comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.Name == "cmb_warehouse_name")
                {
                    comboBox.SelectedIndex = -1; // deselect
                }
                else
                {
                    // Only set to 0 if there are items
                    if (comboBox.Items.Count > 0)
                    {
                        comboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox.SelectedIndex = -1; // no items, just deselect
                    }
                }
            }
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
            pnl_inventory.Enabled = isEdit;
            pnl_additional_specs.Enabled = isEdit;
            btn_upload_image.Enabled = isEdit;
            btn_replace_image.Enabled = isEdit;
            btn_remove_image.Enabled = isEdit;
            txt_trade_type.ReadOnly = isEdit;
            txt_pump_type_compatability.ReadOnly = isEdit;
        }
        private void ResetCheckboxes(params CheckBox[] checkboxes)
        {
            foreach (var checkBox in checkboxes)
            {
                checkBox.Checked = false;
            }
        }
        private void ResetPanels(params Panel[] panels)
        {
            foreach (var panel in panels)
            {
                Helpers.ResetControls(panel);
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
        private void DisbleAutoColumnGeneration(List<DataGridView> dgvs)
        {
            foreach (var dgv in dgvs)
            {
                dgv.AutoGenerateColumns = false;
            }
        }
        #endregion
        #region "Specs Tab"
        // Load specs template
        private void cmb_template_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btn_new.Visible)
            {
                string selectedText = cmb_template.Text;
                SpecsTemplateVisibility();
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

                    // Add id and based_id columns if not present
                    if (!dt_template.Columns.Contains("id"))
                    {
                        DataColumn idCol = new DataColumn("id", typeof(int));
                        idCol.DefaultValue = 0;
                        dt_template.Columns.Add(idCol);
                    }

                    if (!dt_template.Columns.Contains("based_id"))
                    {
                        DataColumn basedIdCol = new DataColumn("based_id", typeof(int));
                        basedIdCol.DefaultValue = 0;
                        dt_template.Columns.Add(basedIdCol);
                    }

                    dgv_template.DataSource = dt_template;
                    dgv_template.Columns["title"].ReadOnly = true;
                    dgv_template.Columns["id"].Visible = false;
                    dgv_template.Columns["based_id"].Visible = false;
                    cmb_impeller.SelectedIndex = 0;

                    if (cmb_template.Text == "WATER METER")
                    {
                        lbl_calibration.Visible = true;
                        cmb_calibration.Visible = true;
                    }
                    if (cmb_template.Text == "PUMP")
                    {
                        SetPhaseComboBoxCell();
                        lbl_calibration.Visible = true;
                        cmb_calibration.Visible = true;
                        lbl_impeller.Visible = true;
                        cmb_impeller.Visible = true;
                        btn_add_impeller.Visible = true;
                    }
                }
            }
        }
        private void dgv_template_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cmb_template.Text == "PUMP" &&
                dgv_template.Rows[e.RowIndex].Cells["title"].Value?.ToString() == "PHASE (1 OR 3)")
            {
                string selectedPhase = dgv_template.Rows[e.RowIndex].Cells["value"].Value?.ToString();

                if (selectedPhase == "1")
                {
                    // Show single phase textboxes
                    lbl_fla.Visible = true;
                    lbl_volt.Visible = true;
                    txt_fla_1.Visible = true;
                    txt_volt_1.Visible = true;

                    // Hide phase 2 textboxes
                    txt_fla_2.Visible = false;
                    txt_volt_2.Visible = false;
                }
                else if (selectedPhase == "3")
                {
                    SpecsTemplateVisibility(true);
                }
            }
        }
        private void dgv_template_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_template.IsCurrentCellDirty)
            {
                dgv_template.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void SetPhaseComboBoxCell()
        {
            foreach (DataGridViewRow row in dgv_template.Rows)
            {
                if (row.Cells["title"].Value?.ToString() == "PHASE (1 OR 3)")
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                    comboCell.Items.AddRange("1", "3");
                    comboCell.Value = comboCell.Items[0];
                    row.Cells["value"] = comboCell;
                    break;
                }
            }
        }
        private void SpecsTemplateVisibility(bool isVisible = false)
        {
            lbl_fla.Visible = isVisible;
            lbl_volt.Visible = isVisible;

            lbl_impeller.Visible = isVisible;
            cmb_impeller.Visible = isVisible;
            btn_add_impeller.Visible = isVisible;

            lbl_calibration.Visible = isVisible;
            cmb_calibration.Visible = isVisible;

            txt_fla_1.Visible = isVisible;
            txt_fla_2.Visible = isVisible;
            txt_volt_1.Visible = isVisible;
            txt_volt_2.Visible = isVisible;
        }
        #endregion
        #region "Parent Tab"
        private async void FetchItemData()
        {
            try
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
                    Items = Helpers.SafeTable(records.items),
                    ItemSpecs = Helpers.SafeTable(records.itemspecs),
                    AdditionalSpecs = Helpers.SafeTable(records.additionalspecs),
                    ItemImages = Helpers.SafeTable(records.itemimages),
                    ItemPurchasing = Helpers.SafeTable(records.itempurchasing),
                    ItemSales = Helpers.SafeTable(records.itemsales),
                    ItemInventory = Helpers.SafeTable(records.iteminventory),
                    ItemAvailableInv = Helpers.SafeTable(records.itemavailableinv),
                    ItemProduction = Helpers.SafeTable(records.itemproduction)
                });

                if (tables == null) return;

                items = tables.Items;
                itemspecs = tables.ItemSpecs;
                additionalspecs = tables.AdditionalSpecs;
                itemimages = tables.ItemImages;
                itempurchasing = tables.ItemPurchasing;
                itemsales = tables.ItemSales;
                iteminventory = tables.ItemInventory;
                itemavailableinv = tables.ItemAvailableInv;
                itemproduction = tables.ItemProduction;

                if (records.items.Count > 0)
                {
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
            catch (Exception ex)
            {
                Debug.WriteLine($"[FetchItemData] {ex.Message}");
                MessageBox.Show("Failed to load item data. Please try again.");
            }
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
            if (tabcontrol1.TabPages.Count > 0)
            {
                tabcontrol1.SelectedIndex = 0;
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
        private void cmb_item_tangibility_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);
        }
        private void txt_trade_status_TextChanged(object sender, EventArgs e)
        {
            ToggleItemPages(txt_trade_type.Text, cmb_item_tangibility_type.Text);
        }


        private void btn_add_supplier_Click(object sender, EventArgs e)
        {
            BusnessPartnerInfoModal modal = new BusnessPartnerInfoModal();
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ShowDialog();
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
        #endregion
        #region "Image Tab"
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
                                Image image = new Bitmap(tempImage);
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
                                    _pendingNewImages[tempImageId] = new Dictionary<string, object>
                            {
                                { "image", base64String },
                                { "filename", fileName }
                            };
                                }
                            }
                        }
                    }
                }
            }
        }
        private void btn_replace_image_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_item_image_id.Text) || !int.TryParse(txt_item_image_id.Text, out int id))
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
                            if (imageFilePaths.ContainsKey(id))
                                imageFilePaths[id] = filePath;

                            var existingPictureBox = flowLayoutPanel1.Controls
                                .OfType<PictureBox>()
                                .FirstOrDefault(pb => pb.Tag is ImageTag tag && tag.Id == id);

                            if (existingPictureBox != null)
                            {
                                existingPictureBox.Image?.Dispose();
                                existingPictureBox.Image = (Image)image.Clone();
                                existingPictureBox.Tag = new ImageTag
                                {
                                    Id = id,
                                    Path = filePath,
                                    Filename = fileName
                                };
                            }

                            img_preview.Image?.Dispose();
                            img_preview.Image = (Image)image.Clone();
                            img_preview.SizeMode = PictureBoxSizeMode.Zoom;
                            lbl_filename.Text = fileName;

                            string base64String = ConvertImageToBase64(image, ImageFormat.Jpeg);

                            if (!string.IsNullOrEmpty(base64String))
                            {
                                if (id < 0)
                                {
                                    // Temp image — update pending new images directly by key
                                    if (_pendingNewImages.ContainsKey(id))
                                    {
                                        _pendingNewImages[id]["image"] = base64String;
                                        _pendingNewImages[id]["filename"] = fileName;
                                    }
                                }
                                else
                                {
                                    // Persisted image — update replace list
                                    replaceBase64Images.RemoveAll(d => Convert.ToInt32(d["id"]) == id);
                                    replaceBase64Images.Add(new Dictionary<string, object>
                            {
                                { "id", id },
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
            if (string.IsNullOrEmpty(txt_item_image_id.Text))
            {
                MessageBox.Show("No image selected, select one to proceed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txt_item_image_id.Text);

            PictureBox pictureToRemove = flowLayoutPanel1.Controls
                .OfType<PictureBox>()
                .FirstOrDefault(pb => pb.Tag is ImageTag tag && tag.Id == id);

            if (pictureToRemove == null)
            {
                MessageBox.Show("Image not found in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to remove this image?",
                                                  "Confirm Removal",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                img_preview.Image = pictureToRemove.Image;
                removedImages[id] = new Bitmap(pictureToRemove.Image);

                pictureToRemove.Image?.Dispose();
                pictureToRemove.Image = null;
                flowLayoutPanel1.Controls.Remove(pictureToRemove);
                pictureToRemove.Dispose();

                img_preview.Image = null;
                txt_item_image_id.Clear();
                lbl_filename.Text = "";

                if (imageFilePaths.ContainsKey(id))
                    imageFilePaths.Remove(id);

                if (id < 0)
                {
                    // Temp image — remove from pending dict directly, no delete instruction needed
                    _pendingNewImages.Remove(id);
                }
                else
                {
                    // Persisted image — remove any pending replace, add to delete list
                    replaceBase64Images.RemoveAll(d => Convert.ToInt32(d["id"]) == id);
                    imageData["replaceimages"] = replaceBase64Images;

                    if (!imageData.ContainsKey("deleteimages"))
                        imageData["deleteimages"] = new List<Dictionary<string, int>>();

                    var deleteImages = (List<Dictionary<string, int>>)imageData["deleteimages"];
                    if (!deleteImages.Any(d => d["id"] == id))
                        deleteImages.Add(new Dictionary<string, int> { { "id", id } });
                }
            }
        }
        private PictureBox CreatePictureBox(Image image, string filePath, int id, string fileName)
        {
            PictureBox pictureBox = new PictureBox
            {
                Width = 100,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = image,
                Margin = new Padding(5),
                Tag = new ImageTag { Id = id, Path = filePath, Filename = fileName }

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
        #endregion
        #region "Attachments Tab"
        private void LoadDirectory(TreeView treeView, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Ensure ACTIVE and BENCHED exist with subfolders
            CreateSubDirectories(directoryPath);

            // Clear tree view and add root
            treeView.Nodes.Clear();
            treeView.ImageKey = "folder";
            treeView.SelectedImageKey = "folder";

            TreeNode rootNode = new TreeNode(directoryPath)
            {
                Tag = directoryPath,
                ImageKey = "folder",
                SelectedImageKey = "folder"
            };
            treeView.Nodes.Add(rootNode);

            // Add ACTIVE and BENCHED with subfolders
            LoadManualSubDirectories(directoryPath, rootNode);

            rootNode.ExpandAll();
        }
        private void CreateSubDirectories(string directoryPath)
        {
            string certificationsDir = Path.Combine(directoryPath, "CERTIFICATIONS");
            string technicalDataSheetsDir = Path.Combine(directoryPath, "TECHNICAL DATA SHEETS");
            string brochuresDir = Path.Combine(directoryPath, "BROCHURES");
            string postProductionReportDir = Path.Combine(directoryPath, "POST PRODUCTION REPORT");

            if (!Directory.Exists(certificationsDir)) Directory.CreateDirectory(certificationsDir);
            if (!Directory.Exists(technicalDataSheetsDir)) Directory.CreateDirectory(technicalDataSheetsDir);
            if (!Directory.Exists(brochuresDir)) Directory.CreateDirectory(brochuresDir);
            if (!Directory.Exists(postProductionReportDir)) Directory.CreateDirectory(postProductionReportDir);
        }
        private void LoadManualSubDirectories(string path, TreeNode parentNode)
        {
            string currentItemId = txt_id.Text;
            string itemSuffix = $"_RR{currentItemId}";

            foreach (var category in new[] { "CERTIFICATIONS", "TECHNICAL DATA SHEETS", "BROCHURES", "POST PRODUCTION REPORT" })
            {
                string categoryPath = Path.Combine(path, category);
                TreeNode categoryNode = new TreeNode(category)
                {
                    Tag = categoryPath,
                    ImageKey = "folder",
                    SelectedImageKey = "folder"
                };
                parentNode.Nodes.Add(categoryNode);

                foreach (var subFolder in Directory.GetDirectories(categoryPath))
                {
                    string folderName = Path.GetFileName(subFolder);

                    // Always show system folders
                    if (systemFolders.Contains(folderName))
                    {
                        TreeNode sysNode = new TreeNode(folderName)
                        {
                            Tag = subFolder,
                            ImageKey = "folder",
                            SelectedImageKey = "folder"
                        };
                        categoryNode.Nodes.Add(sysNode);

                        // Use recursive loader
                        LoadSubDirectoriesRecursive(sysNode, subFolder, itemSuffix);

                        continue;
                    }

                    if (!string.IsNullOrEmpty(currentItemId) && !folderName.EndsWith(itemSuffix, StringComparison.OrdinalIgnoreCase))
                        continue;

                    TreeNode subNode = new TreeNode(folderName)
                    {
                        Tag = subFolder,
                        ImageKey = "folder",
                        SelectedImageKey = "folder"
                    };
                    categoryNode.Nodes.Add(subNode);
                }
            }
        }
        private void LoadSubDirectoriesRecursive(TreeNode parentNode, string parentPath, string itemSuffix)
        {
            foreach (var dir in Directory.GetDirectories(parentPath))
            {
                string folderName = Path.GetFileName(dir);

                if (!string.IsNullOrEmpty(itemSuffix) &&
                    !folderName.EndsWith(itemSuffix, StringComparison.OrdinalIgnoreCase) &&
                    !systemFolders.Contains(folderName)) // system folders always show
                {
                    continue;
                }

                TreeNode newNode = new TreeNode(folderName)
                {
                    Tag = dir,
                    ImageKey = "folder",
                    SelectedImageKey = "folder"
                };

                parentNode.Nodes.Add(newNode);

                //Recursive call to load subfolders inside this folder
                LoadSubDirectoriesRecursive(newNode, dir, itemSuffix);
            }
        }
        // Drag and drop event handlers
        private void ITEM_LV_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void ITEM_LV_DragDrop(object sender, DragEventArgs e)
        {
            if (ITEM_TV.SelectedNode == null)
            {
                MessageBox.Show("Please select a folder first to upload files.", "Info",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string targetFolder = ITEM_TV.SelectedNode.Tag?.ToString();

            if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
            {
                UploadFiles(files, targetFolder);
            }
        }
        private void InitializeIdControlVisibility()
        {
            txt_id.Visible = false;
            txt_item_specs_id.Visible = false;
            txt_item_specs_based_id.Visible = false;
            txt_additional_specs_id.Visible = false;
            txt_additional_specs_based_id.Visible = false;
            txt_item_image_id.Visible = false;
            txt_item_image_based_id.Visible = false;
            txt_item_inventory_id.Visible = false;
            txt_item_inventory_based_id.Visible = false;
            lbl_inv_based_id.Visible = false;
            lbl_inv_id.Visible = false;

            // stress testing
            btn_debug_state.Visible = false;
        }
        private void InitializeListViewContextMenu()
        {
            ContextMenuStrip lvContextMenu = new ContextMenuStrip();

            ToolStripMenuItem renameFileItem = new ToolStripMenuItem("Rename File");
            renameFileItem.Click += RenameFileItem_Click;

            ToolStripMenuItem deleteFileItem = new ToolStripMenuItem("Delete File");
            deleteFileItem.Click += DeleteFileItem_Click;

            lvContextMenu.Items.Add(renameFileItem);
            lvContextMenu.Items.Add(deleteFileItem);

            ITEM_LV.ContextMenuStrip = lvContextMenu;
        }
        private void DeleteFileItem_Click(object sender, EventArgs e)
        {
            if (ITEM_LV.SelectedItems.Count == 0 || ITEM_LV.SelectedItems[0].Text == "No files found")
                return;

            string currentFile = Path.Combine(GetCurrentDirectory(), ITEM_LV.SelectedItems[0].Text);

            if (!File.Exists(currentFile)) return;

            var result = MessageBox.Show($"Are you sure you want to delete the file '{Path.GetFileName(currentFile)}'?",
                                         "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    File.Delete(currentFile);

                    // Refresh the ListView
                    LoadFiles(GetCurrentDirectory());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string GetCurrentDirectory()
        {
            if (ITEM_TV.SelectedNode != null)
            {
                return ITEM_TV.SelectedNode.Tag?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }
        // InputDialog class for getting user input
        private class InputDialog : Form
        {
            private TextBox textBox;
            private Button okButton;
            private Button cancelButton;

            public string InputText => textBox.Text;

            public InputDialog(string title, string prompt, string defaultValue = "")
            {
                InitializeComponents(title, prompt, defaultValue);
            }

            private void InitializeComponents(string title, string prompt, string defaultValue)
            {
                this.Text = title;
                this.Size = new Size(300, 150);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                Label promptLabel = new Label
                {
                    Text = prompt,
                    Location = new Point(10, 10),
                    Size = new Size(260, 20)
                };

                textBox = new TextBox
                {
                    Text = defaultValue,
                    Location = new Point(10, 40),
                    Size = new Size(260, 20)
                };

                okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 70),
                    Size = new Size(75, 25)
                };

                cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(180, 70),
                    Size = new Size(75, 25)
                };

                this.Controls.Add(promptLabel);
                this.Controls.Add(textBox);
                this.Controls.Add(okButton);
                this.Controls.Add(cancelButton);

                this.AcceptButton = okButton;
                this.CancelButton = cancelButton;
            }
        }
        private void RenameFileItem_Click(object sender, EventArgs e)
        {
            if (ITEM_LV.SelectedItems.Count == 0 || ITEM_LV.SelectedItems[0].Text == "No files found")
                return;

            string currentFile = Path.Combine(GetCurrentDirectory(), ITEM_LV.SelectedItems[0].Text);

            if (!File.Exists(currentFile)) return;

            string currentFileName = Path.GetFileName(currentFile);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(currentFileName);
            string extension = Path.GetExtension(currentFileName);

            string itemCode = txt_item_code.Text;
            string itemSuffix = $"_{itemCode}"; // Display item code

            if (!nameWithoutExt.EndsWith(itemSuffix, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This file is not associated with the current Item and cannot be renamed.",
                                "Rename Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nameWithoutSuffix = nameWithoutExt.Substring(0, nameWithoutExt.Length - itemSuffix.Length);

            using (var dialog = new InputDialog("Rename File", "Enter new file name:", nameWithoutSuffix))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFileNameWithoutSuffix = dialog.InputText.Trim();
                    string newFileName = $"{newFileNameWithoutSuffix}{itemSuffix}{extension}";
                    string newFilePath = Path.Combine(GetCurrentDirectory(), newFileName);

                    try
                    {
                        File.Move(currentFile, newFilePath);

                        // Refresh the ListView
                        LoadFiles(GetCurrentDirectory());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming file: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void UploadFiles(string[] files, string targetFolder)
        {
            try
            {
                int successCount = 0;
                int errorCount = 0;
                string itemCode = txt_item_code.Text;

                foreach (string file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            string originalFileName = Path.GetFileName(file);
                            string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                            string extension = Path.GetExtension(originalFileName);

                            string newFileName = $"{nameWithoutExt}{itemCode}{extension}";

                            string destinationPath = Path.Combine(targetFolder, newFileName);

                            // If file exists, ask to overwrite or rename
                            if (File.Exists(destinationPath))
                            {
                                var result = MessageBox.Show($"File '{newFileName}' already exists. Overwrite?",
                                                           "File Exists",
                                                           MessageBoxButtons.YesNoCancel,
                                                           MessageBoxIcon.Question);

                                if (result == DialogResult.No)
                                {
                                    // Add timestamp to filename
                                    newFileName = $"{nameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}_{itemCode}{extension}";
                                    destinationPath = Path.Combine(targetFolder, newFileName);
                                }
                                else if (result == DialogResult.Cancel)
                                {
                                    continue;
                                }
                            }

                            File.Copy(file, destinationPath, true);
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"Error uploading {file}: {ex.Message}");
                    }
                }

                // Refresh the file list
                LoadFiles(targetFolder);

                MessageBox.Show($"Files uploaded successfully: {successCount}\nFailed: {errorCount}",
                              "Upload Complete",
                              MessageBoxButtons.OK,
                              successCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadFiles(string path)
        {
            try
            {
                ITEM_LV.Items.Clear();

                // Configure ListView for better appearance
                ITEM_LV.View = View.Details;
                ITEM_LV.FullRowSelect = true;
                ITEM_LV.GridLines = false;
                ITEM_LV.HeaderStyle = ColumnHeaderStyle.Nonclickable;

                // Ensure columns exist and are properly sized
                if (ITEM_LV.Columns.Count == 0)
                {
                    ITEM_LV.Columns.Add("File Name", 250);
                    ITEM_LV.Columns.Add("Size", 80);
                    ITEM_LV.Columns.Add("Modified", 120);
                    ITEM_LV.Columns.Add("Type", 100);
                }

                if (Directory.Exists(path))
                {
                    // Get all files and sort by name
                    var files = Directory.GetFiles(path)
                                        .OrderBy(f => Path.GetFileName(f))
                                        .ToArray();

                    string currentItemCode = txt_item_code.Text;
                    string rrSuffix = $"_{currentItemCode}";

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        string fileName = fi.Name;
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

                        if (!string.IsNullOrEmpty(currentItemCode) &&
                            !nameWithoutExt.EndsWith(rrSuffix, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        ListViewItem item = new ListViewItem(fileName);

                        // Format file size with appropriate units
                        string fileSize = FormatFileSize(fi.Length);

                        // Format date in a more readable format
                        string modifiedDate = fi.LastWriteTime.ToString("MMM dd, yyyy hh:mm tt");

                        // Get file type/extension
                        string fileType = fi.Extension.ToUpper().TrimStart('.');
                        if (string.IsNullOrEmpty(fileType)) fileType = "File";

                        item.SubItems.Add(fileSize);
                        item.SubItems.Add(modifiedDate);
                        item.SubItems.Add(fileType);

                        // Set appropriate icon based on file type
                        SetFileIcon(item, fi.Extension);

                        ITEM_LV.Items.Add(item);
                    }

                    // Show message if no files found
                    if (ITEM_LV.Items.Count == 0)
                    {
                        ListViewItem emptyItem = new ListViewItem("No files found");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        ITEM_LV.Items.Add(emptyItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
        private void SetFileIcon(ListViewItem item, string extension)
        {
            // You can expand this method to set different icons based on file type
            // For now, using a simple approach - you might want to use ImageList with icons

            switch (extension.ToLower())
            {
                case ".pdf":
                    item.ImageKey = "pdf";
                    break;
                case ".doc":
                case ".docx":
                    item.ImageKey = "word";
                    break;
                case ".xls":
                case ".xlsx":
                    item.ImageKey = "excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    item.ImageKey = "image";
                    break;
                default:
                    item.ImageKey = "file";
                    break;
            }
        }
        private void InitializeContextMenu()
        {
            // Create context menu items
            ToolStripMenuItem addFolderItem = new ToolStripMenuItem("Add Folder");
            ToolStripMenuItem renameItem = new ToolStripMenuItem("Rename");
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
            ToolStripSeparator separator = new ToolStripSeparator();

            // Add click events
            addFolderItem.Click += AddFolderItem_Click;
            renameItem.Click += RenameItem_Click;
            deleteItem.Click += DeleteItem_Click;

            // Add items to context menu
            treeViewContextMenu.Items.AddRange(new ToolStripItem[] {
                addFolderItem,
                separator,
                renameItem,
                deleteItem
            });

            // Assign context menu to TreeView
            ITEM_TV.ContextMenuStrip = treeViewContextMenu;
        }
        private void AddFolderItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null) return;

            string parentPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(parentPath)) return;

            using (var dialog = new InputDialog("Add New Folder", "Enter folder name:"))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFolderName = dialog.InputText.Trim();

                    string itemCode = txt_item_code.Text;
                    newFolderName = $"{newFolderName}_{itemCode}";

                    string newFolderPath = Path.Combine(parentPath, newFolderName);

                    try
                    {
                        Directory.CreateDirectory(newFolderPath);

                        TreeNode newNode = new TreeNode(newFolderName)
                        {
                            Tag = newFolderPath,
                            ImageKey = "folder",
                            SelectedImageKey = "folder"
                        };
                        selectedNode.Nodes.Add(newNode);
                        selectedNode.Expand();

                        ITEM_TV.SelectedNode = newNode;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating folder: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void RenameItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || selectedNode.Parent == null) return;

            string currentPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(currentPath)) return;

            string currentFolderName = Path.GetFileName(currentPath);

            string currentItemCode = txt_item_code.Text;
            string itemSuffix = $"_{currentItemCode}";

            // If folder doesn't have suffix, do not allow renaming
            if (!currentFolderName.EndsWith(itemSuffix, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This folder is not associated with the current Item and cannot be renamed.",
                                "Rename Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ask user for new name (excluding suffix)
            string nameWithoutSuffix = currentFolderName.Substring(0, currentFolderName.Length - itemSuffix.Length);

            using (var dialog = new InputDialog("Rename Folder", "Enter new folder name:", nameWithoutSuffix))
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
                {
                    string newFolderNameWithoutSuffix = dialog.InputText.Trim();
                    string newFolderName = $"{newFolderNameWithoutSuffix}{itemSuffix}"; // Changed from {rrPrefix}{newFolderNameWithoutPrefix}

                    string parentDirectory = Path.GetDirectoryName(currentPath);
                    string newFolderPath = Path.Combine(parentDirectory, newFolderName);

                    try
                    {
                        // Rename directory
                        Directory.Move(currentPath, newFolderPath);

                        // Update TreeView
                        selectedNode.Text = newFolderName;
                        selectedNode.Tag = newFolderPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming folder: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (selectedNode == null || selectedNode.Parent == null) return;

            string folderPath = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(folderPath)) return;

            var result = MessageBox.Show($"Are you sure you want to delete the folder '{selectedNode.Text}'?",
                                       "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete directory (recursively)
                    Directory.Delete(folderPath, true);

                    // Remove from TreeView
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ITEM_TV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Check if the node has children (means it's a parent node)
            if (e.Node.Nodes.Count > 0)
            {
                // Parent node → show panel
                pnl_Receiving.Visible = true;
            }
            else
            {
                // Child node → hide panel
                pnl_Receiving.Visible = false;
            }

            string path = GetFullPath(e.Node);
            LoadFiles(path);
        }
        private string GetFullPath(TreeNode node)
        {
            if (node.Parent == null) return node.Text;
            return Path.Combine(GetFullPath(node.Parent), node.Text);
        }
        private bool IsSystemFolder(TreeNode node)
        {
            // Check if this is one of the predefined system folders
            string[] systemFolders = { "DELIVERY RECEIPT", "ITEM PICTURES" };
            return systemFolders.Contains(node.Text);
        }
        private void ITEM_TV_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the node under the mouse pointer
                selectedNode = ITEM_TV.GetNodeAt(e.X, e.Y);
                if (selectedNode != null)
                {
                    ITEM_TV.SelectedNode = selectedNode;

                    // Enable/disable menu items based on node type
                    bool isRoot = selectedNode.Parent == null;
                    bool isCategory = selectedNode.Text == "ACTIVE" || selectedNode.Text == "BENCHED";
                    bool isSystemFolder = IsSystemFolder(selectedNode);

                    treeViewContextMenu.Items[0].Enabled = !isRoot; // Add Folder
                    treeViewContextMenu.Items[2].Enabled = !isRoot && !isCategory && !isSystemFolder; // Rename
                    treeViewContextMenu.Items[3].Enabled = !isRoot && !isCategory && !isSystemFolder; // Delete
                }
            }
        }
        private void ITEM_TV_DoubleClick(object sender, EventArgs e)
        {
            if (ITEM_LV.SelectedItems.Count > 0 && ITEM_LV.SelectedItems[0].Text != "No files found")
            {
                string selectedFile = Path.Combine(GetCurrentDirectory(), ITEM_LV.SelectedItems[0].Text);
                try
                {
                    System.Diagnostics.Process.Start(selectedFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ITEM_LV_MouseEnter(object sender, EventArgs e)
        {
            if (ITEM_TV.SelectedNode != null)
            {
                toolTip1.SetToolTip(ITEM_LV, "Drag and drop files here to upload to the selected folder");
            }
            else
            {
                toolTip1.SetToolTip(ITEM_LV, "Select a folder first to upload files");
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ITEM_TV.SelectedNode == null)
            {
                MessageBox.Show("Please select a folder first to upload files.", "Info",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select files to upload";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string targetFolder = ITEM_TV.SelectedNode.Tag?.ToString();
                    if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
                    {
                        UploadFiles(openFileDialog.FileNames, targetFolder);
                    }
                }
            }
        }

        private void lbl_filename_Click(object sender, EventArgs e)
        {

        }

        private void btn_debug_state_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("=== PENDING NEW IMAGES ===");
            foreach (var kvp in _pendingNewImages)
                sb.AppendLine($"  ID: {kvp.Key} | File: {kvp.Value["filename"]}");

            sb.AppendLine("\n=== REPLACE IMAGES ===");
            if (imageData.ContainsKey("replaceimages"))
                foreach (var d in (List<Dictionary<string, object>>)imageData["replaceimages"])
                    sb.AppendLine($"  ID: {d["id"]} | File: {d["fileName"]}");

            sb.AppendLine("\n=== DELETE IMAGES ===");
            if (imageData.ContainsKey("deleteimages"))
                foreach (var d in (List<Dictionary<string, int>>)imageData["deleteimages"])
                    sb.AppendLine($"  ID: {d["id"]}");

            sb.AppendLine("\n=== FLOW PANEL PICTUREBOXES ===");
            foreach (PictureBox pb in flowLayoutPanel1.Controls.OfType<PictureBox>())
            {
                var tag = pb.Tag as ImageTag;
                sb.AppendLine($"  ID: {tag?.Id} | File: {tag?.Filename}");
            }

            sb.AppendLine("\n=== CONSISTENCY CHECK ===");

            // Every negative ID in flowPanel must exist in _pendingNewImages
            foreach (PictureBox pb in flowLayoutPanel1.Controls.OfType<PictureBox>())
            {
                var tag = pb.Tag as ImageTag;
                if (tag != null && tag.Id < 0 && !_pendingNewImages.ContainsKey(tag.Id))
                    sb.AppendLine($"  [BUG] PictureBox ID {tag.Id} has no pending data!");
            }

            // Every key in _pendingNewImages must have a matching PictureBox
            var pbIds = flowLayoutPanel1.Controls.OfType<PictureBox>()
                .Select(pb => (pb.Tag as ImageTag)?.Id)
                .ToHashSet();

            foreach (var key in _pendingNewImages.Keys)
                if (!pbIds.Contains(key))
                    sb.AppendLine($"  [BUG] Pending ID {key} has no PictureBox!");

            // An ID cannot be in both replaceimages and deleteimages
            if (imageData.ContainsKey("replaceimages") && imageData.ContainsKey("deleteimages"))
            {
                var replaceIds = ((List<Dictionary<string, object>>)imageData["replaceimages"])
                    .Select(d => Convert.ToInt32(d["id"])).ToHashSet();
                var deleteIds = ((List<Dictionary<string, int>>)imageData["deleteimages"])
                    .Select(d => d["id"]).ToHashSet();

                foreach (var id in replaceIds.Intersect(deleteIds))
                    sb.AppendLine($"  [BUG] ID {id} exists in both replace AND delete!");
            }

            MessageBox.Show(sb.ToString(), "Image State Debug");
        }
    }
    #endregion
}