using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Inventory;
using ClosedXML.Excel;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace smpc_inventory_app.Pages.Inventory.InventoryLogbookModals
{
    public partial class InventoryReport : Form
    {
        private List<InventoryLogbookView> _inventoryList;
        private List<string> _selectedBrands = new List<string>();
        private List<string> _selectedItemCategory = new List<string>();
        private List<string> _selectedGeneralName = new List<string>();
        private List<string> _cartesianCombinations;
        private bool _isUpdating = false;
        public string _selectedYear { get; set; }
        public string _selectedMonth { get; set; }
        public string FileName { get; set; }

        public InventoryReport()
        {
            InitializeComponent();

            // Center the modal relative to its parent form
            this.StartPosition = FormStartPosition.CenterParent;

            CheckDailyInOut();
            UpdateCombineFilterLock();
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            // Get all checked checkboxes inside pnl_main
            var checkedCheckboxes = GetAllCheckboxes(pnl_main)
                .Where(chk => chk.Checked
                    && !chk.Name.StartsWith("chb_graph", StringComparison.OrdinalIgnoreCase)
                    && !chk.Name.StartsWith("chb_combine", StringComparison.OrdinalIgnoreCase))
                .Select(chk => chk.Name
                    .Replace("chb_", "")                  // remove prefix
                    .Replace("_", " ")                    // replace underscores with spaces
                    .ToUpper())                           // make all uppercase
                .ToList();

            // Ensure at least one checkbox is selected
            if (!checkedCheckboxes.Any())
            {
                Helpers.ShowDialogMessage("warning", "Please select at least one column to preview.");
                return;
            }

            try
            {
                // Generate the report, but save to the temp path
                var result = GenerateExcelReport(true);

                // Make the file read-only
                File.SetAttributes(result.FilePath, File.GetAttributes(result.FilePath) | FileAttributes.ReadOnly);

                // Open the preview form
                var previewForm = new ReportPreview
                {
                    FileName = result.FileName.Replace(".xlsx", ""),
                    ReportCode = result.ReportCode,
                    TempFilePath = result.FilePath,
                    ColumnList = checkedCheckboxes,
                    BrandList = result.SelectedBrands,
                    ItemCategoryList = result.SelectedItemCategory,
                    GeneralNameList = result.SelectedGeneralName,
                    ParentFormRef = this
                };

                previewForm.ShowDialog();
            }
            catch (NullReferenceException)
            {

            }
        }

        private async void InventoryReport_Load(object sender, EventArgs e)
        {
            await LoadData();

            // Clear panels first
            pnl_item_category.Controls.Clear();
            pnl_general_name.Controls.Clear();
            pnl_brand.Controls.Clear();

            // After data is loaded and combos are populated
            if (!string.IsNullOrWhiteSpace(_selectedYear) && cmb_year.Items.Contains(_selectedYear))
                cmb_year.SelectedItem = _selectedYear;

            if (!string.IsNullOrWhiteSpace(_selectedMonth) && cmb_month.Items.Contains(_selectedMonth))
                cmb_month.SelectedItem = _selectedMonth;

            // Initially clear all panels
            FilterAndLoadPanels();

            // Add event handlers for month and year selection changes
            cmb_month.SelectedIndexChanged += (s, ev) => FilterAndLoadPanels();
            cmb_year.SelectedIndexChanged += (s, ev) => FilterAndLoadPanels();
        }

        private async Task LoadData()
        {
            // Get inventory data as a list
            _inventoryList = await InventoryLogbookService.GetAsList();

            //Extract unique months and years
            PopulateMonthYearCombos();

            // Initially clear all panels
            FilterAndLoadPanels();

            //Populate panels with checkboxes
            LoadPanels();

            HookParentChildCheckboxLogic();
        }

        private void CheckDailyInOut()
        {
            chb_location.CheckedChanged += (s, e) =>
            {
                if (chb_location.Checked)
                {
                    // When location is checked, daily_in_out should also be checked
                    chb_daily_in_out.Checked = true;
                }
            };
        }

        private void UpdateCombineFilterLock()
        {
            // Check if there is at least one checked checkbox inside panels
            bool anyBrandChecked = GetAllCheckboxes(pnl_brand).Any(cb => cb.Checked);
            bool anyItemCategoryChecked = GetAllCheckboxes(pnl_item_category).Any(cb => cb.Checked);
            bool anyGeneralNameChecked = GetAllCheckboxes(pnl_general_name).Any(cb => cb.Checked);

            // Enable or disable the combine checkboxes
            chb_combine_brand.Enabled = anyBrandChecked;
            chb_combine_item_category.Enabled = anyItemCategoryChecked;
            chb_combine_general_name.Enabled = anyGeneralNameChecked;

            // Optionally, also uncheck it when disabled (to prevent stale state)
            if (!anyBrandChecked)
                chb_combine_brand.Checked = false;

            if (!anyItemCategoryChecked)
                chb_combine_item_category.Checked = false;

            if (!anyGeneralNameChecked)
                chb_combine_general_name.Checked = false;
        }

        private void HookParentChildCheckboxLogic()
        {
            var parentChildMap = new Dictionary<CheckBox, Panel>
            {
                { chb_item_category, pnl_item_category },
                { chb_general_name, pnl_general_name },
                { chb_brand, pnl_brand }
            };

            string FormatPanelName(string panelName)
            {
                if (panelName.StartsWith("pnl_"))
                    panelName = panelName.Substring(4);
                return panelName.Replace("_", " ").ToUpper() + ":"; // add colon
            }

            string FormatChildName(string childName)
            {
                return childName.Replace("_", " ").ToUpper();
            }

            // Show message only for automatic deselects
            void ShowGroupedUncheckedMessage(Dictionary<string, List<string>> uncheckedItemsByPanel, List<string> uncheckedStandalone)
            {
                if (uncheckedItemsByPanel.Count == 0 && uncheckedStandalone.Count == 0) return;

                var messageBuilder = new System.Text.StringBuilder();
                messageBuilder.AppendLine("The following items were unchecked:\n");

                foreach (var item in uncheckedStandalone)
                    messageBuilder.AppendLine(item);

                if (uncheckedStandalone.Count > 0) messageBuilder.AppendLine();

                foreach (var kvp in uncheckedItemsByPanel)
                {
                    messageBuilder.AppendLine(kvp.Key);
                    foreach (var item in kvp.Value)
                        messageBuilder.AppendLine(item);
                    messageBuilder.AppendLine();
                }

                Helpers.ShowDialogMessage("warning", messageBuilder.ToString());
            }

            foreach (var pair in parentChildMap)
            {
                var parent = pair.Key;
                var panel = pair.Value;

                // Parent checkbox logic
                parent.CheckedChanged += (s, e) =>
                {
                    if (_isUpdating) return;

                    _isUpdating = true;

                    var uncheckedItemsByPanel = new Dictionary<string, List<string>>();
                    var uncheckedStandalone = new List<string>();

                    if (parent.Checked)
                    {
                        // Uncheck all children automatically
                        foreach (CheckBox cb in GetAllCheckboxes(panel))
                        {
                            if (cb.Checked)
                            {
                                cb.Checked = false;
                                string panelName = FormatPanelName(panel.Name);
                                if (!uncheckedItemsByPanel.ContainsKey(panelName))
                                    uncheckedItemsByPanel[panelName] = new List<string>();
                                uncheckedItemsByPanel[panelName].Add(FormatChildName(cb.Name));
                            }
                        }
                    }

                    _isUpdating = false;
                    ShowGroupedUncheckedMessage(uncheckedItemsByPanel, uncheckedStandalone);
                };

                // Child checkboxes logic
                foreach (CheckBox cb in GetAllCheckboxes(panel))
                {
                    cb.CheckedChanged += (s, e) =>
                    {
                        if (_isUpdating) return;

                        if (cb.Checked) // only act if automatically checked (parent logic)
                        {
                            _isUpdating = true;

                            var uncheckedItemsByPanel = new Dictionary<string, List<string>>();
                            var uncheckedStandalone = new List<string>();

                            // Uncheck parent if still checked
                            if (parent.Checked)
                            {
                                parent.Checked = false;
                                uncheckedStandalone.Add(parent.Text);
                            }

                            // Uncheck location if checked
                            if (chb_location.Checked)
                            {
                                chb_location.Checked = false;
                                uncheckedStandalone.Add(chb_location.Text);
                            }

                            _isUpdating = false;
                            ShowGroupedUncheckedMessage(uncheckedItemsByPanel, uncheckedStandalone);
                        }
                    };
                }
            }

            // Location checkbox logic
            chb_location.CheckedChanged += (s, e) =>
            {
                if (_isUpdating) return;

                _isUpdating = true;

                var uncheckedItemsByPanel = new Dictionary<string, List<string>>();
                var uncheckedStandalone = new List<string>();

                if (chb_location.Checked)
                {
                    // Uncheck all children in all panels
                    foreach (var panel in parentChildMap.Values)
                    {
                        foreach (CheckBox cb in GetAllCheckboxes(panel))
                        {
                            if (cb.Checked)
                            {
                                cb.Checked = false;
                                string panelName = FormatPanelName(panel.Name);
                                if (!uncheckedItemsByPanel.ContainsKey(panelName))
                                    uncheckedItemsByPanel[panelName] = new List<string>();
                                uncheckedItemsByPanel[panelName].Add(FormatChildName(cb.Name));
                            }
                        }
                    }
                }

                _isUpdating = false;
                ShowGroupedUncheckedMessage(uncheckedItemsByPanel, uncheckedStandalone);
            };
        }

        private void PopulateMonthYearCombos()
        {
            // Parse the date strings into DateTime safely
            var validDates = _inventoryList
                .Select(item =>
                {
                    DateTime parsedDate;
                    return DateTime.TryParseExact(
                        item.date,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out parsedDate
                    ) ? parsedDate : (DateTime?)null;
                })
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .ToList();

            // Extract unique months and years
            var uniqueMonths = validDates
                .Select(d => d.ToString("MMMM")) // Full month name (e.g., "January")
                .Distinct()
                .OrderBy(m => DateTime.ParseExact(m, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month)
                .ToList();

            var uniqueYears = validDates
                .Select(d => d.Year.ToString())
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            // Populate ComboBoxes
            cmb_month.Items.Clear();
            cmb_month.Items.AddRange(uniqueMonths.ToArray());

            cmb_year.Items.Clear();
            cmb_year.Items.AddRange(uniqueYears.ToArray());
        }

        private void LoadPanels()
        {
            // Unique item categories
            var itemCategories = _inventoryList
                .Select(i => i.item_category)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            PopulatePanelWithCheckboxes(pnl_item_category, itemCategories);

            // Unique general name
            var generalName = _inventoryList
                .Select(i => i.general_name)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            PopulatePanelWithCheckboxes(pnl_general_name, generalName);

            // Unique brand
            var brand = _inventoryList
                .Select(i => i.brand)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            PopulatePanelWithCheckboxes(pnl_brand, brand);
        }

        private void PopulatePanelWithCheckboxes(Panel targetPanel, IEnumerable<string> values)
        {
            if (values == null || !values.Any())
                return;

            targetPanel.Controls.Clear(); // clear any previous items

            int yPos = 5; // starting Y position
            int spacing = 5;

            foreach (var name in values)
            {
                Panel itemPanel = new Panel
                {
                    Width = targetPanel.Width - 40,
                    Height = 25,
                    BackColor = Color.White,
                    Tag = name
                };

                CheckBox chk = new CheckBox
                {
                    Location = new Point(5, 3),
                    Width = 20,
                    Height = 20,
                    Tag = name,
                    Name = name
                };

                Label lbl = new Label
                {
                    Text = name,
                    AutoSize = false,
                    Location = new Point(chk.Right + 5, 2),
                    Width = itemPanel.Width - 60,
                    Height = 20,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                //Track checked brands
                if (targetPanel == pnl_brand)
                {
                    chk.CheckedChanged += (s, e) =>
                    {
                        string brandName = chk.Tag.ToString();

                        if (chk.Checked)
                        {
                            if (!_selectedBrands.Contains(brandName))
                                _selectedBrands.Add(brandName);
                        }
                        else
                        {
                            _selectedBrands.Remove(brandName);
                        }

                        UpdateCombineFilterLock();
                    };
                }

                //Track checked general name
                if (targetPanel == pnl_general_name)
                {
                    chk.CheckedChanged += (s, e) =>
                    {
                        string generalName = chk.Tag.ToString();

                        if (chk.Checked)
                        {
                            if (!_selectedGeneralName.Contains(generalName))
                                _selectedGeneralName.Add(generalName);
                        }
                        else
                        {
                            _selectedGeneralName.Remove(generalName);
                        }

                        UpdateCombineFilterLock();
                    };
                }

                //Track checked item category
                if (targetPanel == pnl_item_category)
                {
                    chk.CheckedChanged += (s, e) =>
                    {
                        string itemCategory = chk.Tag.ToString();

                        if (chk.Checked)
                        {
                            if (!_selectedItemCategory.Contains(itemCategory))
                                _selectedItemCategory.Add(itemCategory);
                        }
                        else
                        {
                            _selectedItemCategory.Remove(itemCategory);
                        }

                        UpdateCombineFilterLock();
                    };
                }

                itemPanel.Controls.Add(chk);
                itemPanel.Controls.Add(lbl);

                itemPanel.Location = new Point(5, yPos);
                yPos += itemPanel.Height + spacing;

                targetPanel.Controls.Add(itemPanel);
            }
        }

        private void FilterAndLoadPanels()
        {
            _selectedBrands.Clear();
            _selectedGeneralName.Clear();
            _selectedItemCategory.Clear();

            // Clear panels first
            pnl_item_category.Controls.Clear();
            pnl_general_name.Controls.Clear();
            pnl_brand.Controls.Clear();

            // If month or year is not selected, do nothing further
            if (string.IsNullOrWhiteSpace(cmb_month.Text) || string.IsNullOrWhiteSpace(cmb_year.Text))
                return;

            if (_inventoryList == null || !_inventoryList.Any())
                return;

            string selectedMonth = cmb_month.Text;
            string selectedYear = cmb_year.Text;

            // Convert the month name (e.g., "January") to its numeric value
            int monthNumber = DateTime.ParseExact(selectedMonth, "MMMM",
                System.Globalization.CultureInfo.InvariantCulture).Month;

            // Filter inventory by month and year
            var filteredList = _inventoryList
                .Where(i =>
                {
                    if (DateTime.TryParseExact(
                            i.date,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out DateTime parsedDate))
                    {
                        return parsedDate.Month == monthNumber && parsedDate.Year.ToString() == selectedYear;
                    }
                    return false;
                })
                .ToList();

            // If no results, keep panels empty
            if (!filteredList.Any())
                return;

            // Populate each panel with unique filtered data
            var itemCategories = filteredList
                .Select(i => i.item_category)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            var generalNames = filteredList
                .Select(i => i.general_name)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            var brands = filteredList
                .Select(i => i.brand)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            PopulatePanelWithCheckboxes(pnl_item_category, itemCategories);
            PopulatePanelWithCheckboxes(pnl_general_name, generalNames);
            PopulatePanelWithCheckboxes(pnl_brand, brands);

            // Re-hook logic after reload
            HookParentChildCheckboxLogic();
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || !checkbox.Checked)
                return;

            if (_inventoryList == null || _inventoryList.Count == 0)
            {
                Helpers.ShowDialogMessage("error", "Inventory list is empty or not loaded.");
                return;
            }

            // Get property name by removing "chb_" prefix
            string propertyName = checkbox.Name.Replace("chb_", "");

            // Get the property info from InventoryLogbookView using reflection
            var propertyInfo = typeof(InventoryLogbookView).GetProperty(propertyName);

            if (propertyInfo == null)
            {
                Helpers.ShowDialogMessage("error", $"Property '{propertyName}' not found in InventoryLogbookView.");
                return;
            }

            // Loop through and print the values
            foreach (var item in _inventoryList)
            {
                var value = propertyInfo.GetValue(item);
                Console.WriteLine(value ?? "(null)");
            }
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            GenerateExcelReport();
        }

        private ReportGenerationResult GenerateExcelReport(bool isPreview = false)
        {
            // --- Generate custom Excel filename ---
            string selectedMonth = cmb_month.Text;
            string selectedYear = cmb_year.Text;

            // Convert month to number for filename
            int monthNumber = DateTime.ParseExact(selectedMonth, "MMMM",
                System.Globalization.CultureInfo.InvariantCulture).Month;

            // Generate a random 4-digit code (can be replaced with a counter or timestamp)
            string randomCode = new Random().Next(1000, 9999).ToString();
            string reportCode = $"INVREP#{randomCode}";

            // --- Use provided FileName if available and data exists ---
            string fileName;
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                fileName = FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)
                    ? FileName
                    : $"{FileName}.xlsx";
            }
            else
            {
                fileName = $"InventoryReport_{selectedYear}_{monthNumber:D2}_{reportCode}.xlsx";
            }

            // Save to user's Desktop (avoid overwriting existing files)
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);

            // If a file with the same name exists, append a counter (_1, _2, etc.)
            int counter = 1;
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string fileExt = Path.GetExtension(fileName);

            while (File.Exists(filePath))
            {
                fileName = $"{fileNameWithoutExt}({counter}){fileExt}";
                filePath = Path.Combine(desktopPath, fileName);
                counter++;
            }

            try
            {
                if (_inventoryList == null || !_inventoryList.Any())
                {
                    Helpers.ShowDialogMessage("error", "No inventory data loaded.");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(cmb_month.Text) || string.IsNullOrWhiteSpace(cmb_year.Text))
                {
                    Helpers.ShowDialogMessage("warning", "Please select both a month and a year before creating the report.");
                    return null;
                }

                var filteredList = _inventoryList
                    .Where(i =>
                    {
                        if (DateTime.TryParseExact(
                                i.date,
                                "dd/MM/yyyy",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out DateTime parsedDate))
                        {
                            bool matchesMonthYear = parsedDate.Month == monthNumber && parsedDate.Year.ToString() == selectedYear;

                            bool matchesBrand = !_selectedBrands.Any() || _selectedBrands.Contains(i.brand);
                            bool matchesItemCategory = !_selectedItemCategory.Any() || _selectedItemCategory.Contains(i.item_category);
                            bool matchesGeneralName = !_selectedGeneralName.Any() || _selectedGeneralName.Contains(i.general_name);

                            return matchesMonthYear && matchesBrand && matchesItemCategory && matchesGeneralName;
                        }
                        return false;
                    })
                    .ToList();

                if (!filteredList.Any())
                {
                    string brandFilters = _selectedBrands.Any() ? string.Join(", ", _selectedBrands) : "All Brands";
                    string categoryFilters = _selectedItemCategory.Any() ? string.Join(", ", _selectedItemCategory) : "All Item Categories";
                    string generalNameFilters = _selectedGeneralName.Any() ? string.Join(", ", _selectedGeneralName) : "All General Names";

                    Helpers.ShowDialogMessage("warning", $"No inventory data found for {selectedMonth} {selectedYear}.\n\n" +
                        $"Filters applied:\n" +
                        $"• Brand: {brandFilters}\n" +
                        $"• Item Category: {categoryFilters}\n" +
                        $"• General Name: {generalNameFilters}" +
                        "No Data Found");

                    return null;
                }

                var checkedCheckboxes = GetAllCheckboxes(pnl_main)
                    .Where(chk => chk.Checked)
                    .ToList();

                if (!checkedCheckboxes.Any())
                {
                    Helpers.ShowDialogMessage("warning", "Please select at least one checkbox before creating the Excel file.");
                    return null;
                }

                var propertyOrder = new List<string>
                {
                    "item_code",
                    "item_category",
                    "general_name",
                    "brand",
                    "item_model",
                    "item_description",
                    "calibration",
                    "beg",
                    "end",
                    "remarks"
                };

                var orderedCheckboxes = checkedCheckboxes
                    .Select(chk => chk.Name.Replace("chb_", ""))
                    .Where(name => propertyOrder.Contains(name))
                    .OrderBy(name => propertyOrder.IndexOf(name))
                    .ToList();

                // Ensure these columns are included if filters were applied
                if (_selectedItemCategory.Any() && !orderedCheckboxes.Contains("item_category"))
                    orderedCheckboxes.Insert(propertyOrder.IndexOf("item_category"), "item_category");

                if (_selectedGeneralName.Any() && !orderedCheckboxes.Contains("general_name"))
                    orderedCheckboxes.Insert(propertyOrder.IndexOf("general_name"), "general_name");

                if (_selectedBrands.Any() && !orderedCheckboxes.Contains("brand"))
                    orderedCheckboxes.Insert(propertyOrder.IndexOf("brand"), "brand");

                bool includeTotal = chb_total_in_out.Checked;
                bool includeDaily = chb_daily_in_out.Checked;
                bool includeLocation = chb_location.Checked;
                bool combineCalibration = chb_combine_calibration.Checked;
                bool combineItemCategory = chb_combine_item_category.Checked;
                bool combineGeneralName = chb_combine_general_name.Checked;
                bool combineBrand = chb_combine_brand.Checked;

                var workbook = new XLWorkbook();

                // Determine grouping strategy
                Func<InventoryLogbookView, string> groupSelector = null;
                string groupingType = null;

                // LOCATION always takes priority
                if (includeLocation)
                {
                    groupingType = "location";
                    groupSelector = i => !string.IsNullOrWhiteSpace(i.location)
                        ? i.location.Split('-')[0].Trim()
                        : string.Empty;
                }
                else if (_selectedGeneralName.Any() && !combineGeneralName &&
                         _selectedItemCategory.Any() && !combineItemCategory &&
                         combineBrand)
                {
                    // General Name + Item Category
                    groupingType = "general_name_item_category";
                    var allCombinations = (from g in _selectedGeneralName
                                           from c in _selectedItemCategory
                                           select $"{g} | {c}").ToList();

                    groupSelector = i => $"{i.general_name} | {i.item_category}";

                    // Store the combinations for later use (to ensure empty sheets are created)
                    _cartesianCombinations = allCombinations;
                }
                else if (_selectedGeneralName.Any() && !combineGeneralName &&
                         _selectedBrands.Any() && !combineBrand &&
                         combineItemCategory)
                {
                    // General Name + Brand
                    groupingType = "general_name_brand";
                    var allCombinations = (from g in _selectedGeneralName
                                           from b in _selectedBrands
                                           select $"{g} | {b}").ToList();

                    groupSelector = i => $"{i.general_name} | {i.brand}";
                    _cartesianCombinations = allCombinations;
                }
                else if (_selectedItemCategory.Any() && !combineItemCategory &&
                         _selectedBrands.Any() && !combineBrand &&
                         combineGeneralName)
                {
                    // Item Category + Brand
                    groupingType = "item_category_brand";
                    var allCombinations = (from c in _selectedItemCategory
                                           from b in _selectedBrands
                                           select $"{c} | {b}").ToList();

                    groupSelector = i => $"{i.item_category} | {i.brand}";
                    _cartesianCombinations = allCombinations;
                }
                else if (_selectedGeneralName.Any() && !combineGeneralName)
                {
                    // General Name only
                    groupingType = "general_name";
                    groupSelector = i => i.general_name;
                }
                else if (_selectedItemCategory.Any() && !combineItemCategory)
                {
                    // Item Category only
                    groupingType = "item_category";
                    groupSelector = i => i.item_category;
                }
                else if (_selectedBrands.Any() && !combineBrand)
                {
                    // Brand only
                    groupingType = "brand";
                    groupSelector = i => i.brand;
                }
                else
                {
                    // --- SINGLE SHEET MODE ---
                    groupingType = "single";
                    groupSelector = null;
                }

                // --- Generate sheets ---
                if (groupSelector != null)
                {
                    var groups = filteredList
                        .Where(i => !string.IsNullOrWhiteSpace(groupSelector(i)))
                        .GroupBy(groupSelector)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    // Add missing Cartesian combinations as empty groups
                    if (_cartesianCombinations != null && _cartesianCombinations.Any())
                    {
                        foreach (var combo in _cartesianCombinations)
                        {
                            if (!groups.ContainsKey(combo))
                                groups[combo] = new List<InventoryLogbookView>();
                        }
                    }

                    // --- Build a complete list of sheet names to include ---
                    var allSheetNames = new List<string>();

                    // Add all grouped names that actually have data
                    allSheetNames.AddRange(groups.Keys);

                    // Add missing sheet names based on user selections
                    if (groupingType == "brand" && _selectedBrands.Any())
                        allSheetNames.AddRange(_selectedBrands.Except(allSheetNames));
                    else if (groupingType == "item_category" && _selectedItemCategory.Any())
                        allSheetNames.AddRange(_selectedItemCategory.Except(allSheetNames));
                    else if (groupingType == "general_name" && _selectedGeneralName.Any())
                        allSheetNames.AddRange(_selectedGeneralName.Except(allSheetNames));

                    // Remove duplicates and sort alphabetically
                    allSheetNames = allSheetNames.Distinct().OrderBy(n => n).ToList();

                    foreach (var name in allSheetNames)
                    {
                        string sheetName = string.IsNullOrWhiteSpace(name) ? "Unnamed" : name;
                        if (sheetName.Length > 31)
                            sheetName = sheetName.Substring(0, 31);

                        var worksheet = workbook.Worksheets.Add(sheetName);

                        // Get the actual data for this group (or empty if none)
                        var groupData = groups.ContainsKey(name) ? groups[name] : new List<InventoryLogbookView>();

                        // Write the sheet
                        WriteInventorySheet(
                            worksheet,
                            groupData,
                            orderedCheckboxes,
                            selectedYear,
                            monthNumber,
                            includeTotal,
                            includeDaily,
                            includeLocation: groupingType == "location",
                            combineCalibration,
                            combineItemCategory,
                            combineGeneralName,
                            combineBrand
                        );

                        // If the sheet has no data, add a note
                        if (!groupData.Any())
                        {
                            worksheet.Cell(3, 1).Value = "No data available for this selection.";
                            worksheet.Cell(3, 1).Style.Font.Italic = true;
                            worksheet.Cell(3, 1).Style.Font.FontColor = XLColor.Gray;
                        }

                        AdjustColumnWidthsAndFreeze(worksheet);
                    }
                }
                else
                {
                    // --- SINGLE SHEET MODE ---
                    var worksheet = workbook.Worksheets.Add("Inventory Report");

                    WriteInventorySheet(
                        worksheet,
                        filteredList, // may be empty
                        orderedCheckboxes,
                        selectedYear,
                        monthNumber,
                        includeTotal,
                        includeDaily,
                        includeLocation,
                        combineCalibration,
                        combineItemCategory,
                        combineGeneralName,
                        combineBrand
                    );

                    AdjustColumnWidthsAndFreeze(worksheet);
                }

                workbook.SaveAs(filePath);

                // Update the class fields
                _selectedBrands = _selectedBrands ?? new List<string>();
                _selectedItemCategory = _selectedItemCategory ?? new List<string>();
                _selectedGeneralName = _selectedGeneralName ?? new List<string>();

                // Build result object
                var result = new ReportGenerationResult
                {
                    FileName = fileName,
                    ReportCode = reportCode,
                    FilePath = filePath,
                    SelectedBrands = _selectedBrands,
                    SelectedItemCategory = _selectedItemCategory,
                    SelectedGeneralName = _selectedGeneralName
                };

                // If preview, skip saving message
                if (isPreview)
                    return result;

                Helpers.ShowDialogMessage("success", $"Excel file created successfully for {selectedMonth} {selectedYear} at:\n{filePath}");


                return result;
            }
            catch (ArgumentOutOfRangeException)
            {
                Helpers.ShowDialogMessage("warning", "Please select at least one checkbox before creating the Excel file.");
                return null;
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", "Error: " + ex.Message);
                return null;
            }
        }

        private void AdjustColumnWidthsAndFreeze(IXLWorksheet worksheet)
        {
            // Auto-adjust column widths to fit contents first
            worksheet.ColumnsUsed().AdjustToContents();

            foreach (var col in worksheet.ColumnsUsed())
            {
                string header1 = worksheet.Cell(1, col.ColumnNumber()).GetString().Trim().ToUpper();
                string header2 = worksheet.Cell(2, col.ColumnNumber()).GetString().Trim().ToUpper();

                // Combine both rows in case headers are split between two lines
                string combinedHeader = (header1 + " " + header2).Trim();

                // Compact width for IN/OUT columns
                if (combinedHeader.Contains(" IN") || combinedHeader.EndsWith("IN") ||
                    combinedHeader.Contains(" OUT") || combinedHeader.EndsWith("OUT"))
                {
                    col.Width = 6;

                    // Center the cells under IN/OUT columns (starting from row 3)
                    foreach (var cell in worksheet.Column(col.ColumnNumber()).CellsUsed().Where(c => c.Address.RowNumber > 2))
                    {
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                }
                else
                {
                    col.Width = Math.Max(col.Width + 2, 18); // readable width for text columns
                }
            }

            // Freeze the first two rows
            worksheet.SheetView.FreezeRows(2);
        }

        private void WriteInventorySheet(IXLWorksheet worksheet, List<InventoryLogbookView> data, List<string> orderedCheckboxes, string selectedYear, int monthNumber, bool includeTotal, bool includeDaily, bool includeLocation, bool combineCalibration, bool combineItemCategory, bool combineGeneralName, bool combineBrand)
        {
            // Conditional grouping: respect combineCalibration and includeLocation
            var groupedData =
                includeLocation
                    ? (
                        combineCalibration
                            // --- Combine by item_id + Zone only, merge calibrations ---
                            ? data.GroupBy(i => new
                            {
                                i.item_id,
                                Zone = i.location?.Split('-')[0].Trim() ?? ""
                            })
                            .Select(g => new
                            {
                                ItemId = g.Key.item_id,
                                Zone = g.Key.Zone,
                                Calibration = string.Join(", ",
                                    g.Select(x => x.calibration?.Trim())
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                First = g.First(),
                                QtyIn = g.Sum(x => x.qty_in),
                                QtyOut = g.Sum(x => x.qty_out),
                                Remarks = string.Join(", ",
                                    g.Select(x => x.remarks)
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                Daily = g
                                    .Where(x => DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out _))
                                    .GroupBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture).Day)
                                    .ToDictionary(
                                        dayGroup => dayGroup.Key,
                                        dayGroup => new
                                        {
                                            In = dayGroup.Sum(d => d.qty_in),
                                            Out = dayGroup.Sum(d => d.qty_out)
                                        })
                            })
                            // --- Default: group by item_id + Zone + Calibration ---
                            : data.GroupBy(i => new
                            {
                                i.item_id,
                                Zone = i.location?.Split('-')[0].Trim() ?? "",
                                Calibration = i.calibration?.Trim() ?? ""
                            })
                            .Select(g => new
                            {
                                ItemId = g.Key.item_id,
                                Zone = g.Key.Zone,
                                Calibration = g.Key.Calibration,
                                First = g.First(),
                                QtyIn = g.Sum(x => x.qty_in),
                                QtyOut = g.Sum(x => x.qty_out),
                                Remarks = string.Join(", ",
                                    g.Select(x => x.remarks)
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                Daily = g
                                    .Where(x => DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out _))
                                    .GroupBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture).Day)
                                    .ToDictionary(
                                        dayGroup => dayGroup.Key,
                                        dayGroup => new
                                        {
                                            In = dayGroup.Sum(d => d.qty_in),
                                            Out = dayGroup.Sum(d => d.qty_out)
                                        })
                            })
                    )
                    : (
                        combineCalibration
                            // --- Combine by item_id only, merge calibrations ---
                            ? data.GroupBy(i => new
                            {
                                i.item_id
                            })
                            .Select(g => new
                            {
                                ItemId = g.Key.item_id,
                                Zone = "",
                                Calibration = string.Join(", ",
                                    g.Select(x => x.calibration?.Trim())
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                First = g.First(),
                                QtyIn = g.Sum(x => x.qty_in),
                                QtyOut = g.Sum(x => x.qty_out),
                                Remarks = string.Join(", ",
                                    g.Select(x => x.remarks)
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                Daily = g
                                    .Where(x => DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out _))
                                    .GroupBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture).Day)
                                    .ToDictionary(
                                        dayGroup => dayGroup.Key,
                                        dayGroup => new
                                        {
                                            In = dayGroup.Sum(d => d.qty_in),
                                            Out = dayGroup.Sum(d => d.qty_out)
                                        })
                            })
                            // --- Default: group by item_id + Calibration ---
                            : data.GroupBy(i => new
                            {
                                i.item_id,
                                Calibration = i.calibration?.Trim() ?? ""
                            })
                            .Select(g => new
                            {
                                ItemId = g.Key.item_id,
                                Zone = "",
                                Calibration = g.Key.Calibration,
                                First = g.First(),
                                QtyIn = g.Sum(x => x.qty_in),
                                QtyOut = g.Sum(x => x.qty_out),
                                Remarks = string.Join(", ",
                                    g.Select(x => x.remarks)
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Distinct()),
                                Daily = g
                                    .Where(x => DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out _))
                                    .GroupBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture).Day)
                                    .ToDictionary(
                                        dayGroup => dayGroup.Key,
                                        dayGroup => new
                                        {
                                            In = dayGroup.Sum(d => d.qty_in),
                                            Out = dayGroup.Sum(d => d.qty_out)
                                        })
                            })
                    )
                    .ToList();

            // --- Apply alphabetical ordering if combineItemCategory is true ---
            if (combineItemCategory)
            {
                groupedData = groupedData
                    .OrderBy(g => g.First.item_category, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(g => g.First.item_model, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            else if (combineGeneralName)
            {
                groupedData = groupedData
                    .OrderBy(g => g.First.general_name, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(g => g.First.item_model, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            else if (combineBrand)
            {
                groupedData = groupedData
                    .OrderBy(g => g.First.brand, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(g => g.First.item_model, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            else
            {
                groupedData = groupedData
                    .OrderBy(g => g.First.item_model, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            var finalHeaderOrder = new List<string>(orderedCheckboxes);
            int insertIndex = finalHeaderOrder.IndexOf("remarks");
            if (insertIndex == -1)
                insertIndex = finalHeaderOrder.Count;

            if (includeTotal)
            {
                finalHeaderOrder.Insert(insertIndex, "TOTAL_IN_OUT");
                insertIndex++;
            }

            if (includeDaily)
            {
                finalHeaderOrder.Insert(insertIndex, "DAILY_IN_OUT");
                insertIndex++;
            }

            // Determine active days
            List<int> activeDays = new List<int>();
            if (includeDaily)
            {
                int daysInMonth = DateTime.DaysInMonth(int.Parse(selectedYear), monthNumber);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    bool hasTransactions = data.Any(x =>
                    {
                        if (DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out DateTime d))
                        {
                            return d.Day == day && (x.qty_in != 0 || x.qty_out != 0);
                        }
                        return false;
                    });

                    if (hasTransactions)
                        activeDays.Add(day);
                }
            }

            // --- Write Headers ---
            int colIndex = 1;
            foreach (var propertyName in finalHeaderOrder)
            {
                if (propertyName == "TOTAL_IN_OUT")
                {
                    worksheet.Cell(1, colIndex).Value = "TOTAL";
                    worksheet.Range(1, colIndex, 1, colIndex + 1).Merge();

                    worksheet.Cell(2, colIndex).Value = "IN";
                    worksheet.Cell(2, colIndex + 1).Value = "OUT";
                    worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Font.Bold = true;
                    worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Column(colIndex).Width = 8;
                    worksheet.Column(colIndex + 1).Width = 8;
                    colIndex += 2;
                }
                else if (propertyName == "DAILY_IN_OUT")
                {
                    foreach (int day in activeDays)
                    {
                        worksheet.Cell(1, colIndex).Value = "IN";
                        worksheet.Range(1, colIndex, 2, colIndex).Merge();
                        worksheet.Cell(1, colIndex + 1).Value = "OUT";
                        worksheet.Range(1, colIndex + 1, 2, colIndex + 1).Merge();

                        worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Font.Bold = true;
                        worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Range(1, colIndex, 2, colIndex + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(colIndex).Width = 6;
                        worksheet.Column(colIndex + 1).Width = 6;
                        colIndex += 2;
                    }
                }
                else
                {
                    worksheet.Cell(1, colIndex).Value = propertyName.Replace("_", " ").ToUpper();
                    worksheet.Range(1, colIndex, 2, colIndex).Merge();
                    worksheet.Cell(1, colIndex).Style.Font.Bold = true;
                    worksheet.Cell(1, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(1, colIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Column(colIndex).Width = 20; // or any width you like
                    colIndex++;
                }
            }

            // --- Write Data ---
            int rowIndex = 3;
            foreach (var group in groupedData)
            {
                int dataCol = 1;

                foreach (var propertyName in finalHeaderOrder)
                {
                    if (propertyName == "TOTAL_IN_OUT")
                    {
                        worksheet.Cell(rowIndex, dataCol).Value = group.QtyIn;
                        worksheet.Cell(rowIndex, dataCol + 1).Value = group.QtyOut;
                        dataCol += 2;
                    }
                    else if (propertyName == "DAILY_IN_OUT")
                    {
                        foreach (int day in activeDays)
                        {
                            if (group.Daily.TryGetValue(day, out var dayData))
                            {
                                worksheet.Cell(rowIndex, dataCol).Value = dayData.In;
                                worksheet.Cell(rowIndex, dataCol + 1).Value = dayData.Out;

                                var recordsForThisDay = data
                                    .Where(x =>
                                        x.item_id == group.ItemId &&
                                        x.location?.StartsWith(group.Zone) == true &&
                                        DateTime.TryParseExact(x.date, "dd/MM/yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out DateTime d) &&
                                        d.Day == day)
                                    .ToList();

                                if (recordsForThisDay.Any())
                                {
                                    // Collect unique values for each field
                                    var rrNos = recordsForThisDay
                                        .Select(r => r.rr_no)
                                        .Where(rr => !string.IsNullOrWhiteSpace(rr))
                                        .Distinct()
                                        .ToList();

                                    var poNos = recordsForThisDay
                                        .Select(r => r.po_no)
                                        .Where(po => !string.IsNullOrWhiteSpace(po))
                                        .Distinct()
                                        .ToList();

                                    var suppliers = recordsForThisDay
                                        .Select(r => r.supplier_name)
                                        .Where(s => !string.IsNullOrWhiteSpace(s))
                                        .Distinct()
                                        .ToList();

                                    var dates = recordsForThisDay
                                        .Select(r => r.date)
                                        .Where(d => !string.IsNullOrWhiteSpace(d?.ToString()))
                                        .Select(d =>
                                        {
                                            DateTime parsedDate;
                                            // Use exact format with day first
                                            if (DateTime.TryParseExact(
                                                    d.ToString().Trim(),
                                                    new[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy" },
                                                    System.Globalization.CultureInfo.InvariantCulture,
                                                    System.Globalization.DateTimeStyles.None,
                                                    out parsedDate))
                                            {
                                                return parsedDate.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                // Keep original if parsing fails
                                                return d.ToString();
                                            }
                                        })
                                        .Distinct()
                                        .ToList();

                                    // Build the formatted notes string
                                    string inNote = "";
                                    string outNote = "";

                                    if (rrNos.Any())
                                        inNote += "RR No(s): " + string.Join(", ", rrNos) + "\n";
                                        outNote += "IR No(s): " + string.Join(", ", rrNos) + "\n";

                                    if (poNos.Any())
                                        inNote += "PO No(s): " + string.Join(", ", poNos) + "\n";
                                        outNote += "DR No(s): " + string.Join(", ", poNos) + "\n";

                                    if (suppliers.Any())
                                        inNote += "Supplier(s): " + string.Join(", ", suppliers) + "\n";
                                        outNote += "Customer(s): " + string.Join(", ", suppliers) + "\n";

                                    if (dates.Any())
                                        inNote += "Date(s) Received: " + string.Join(", ", dates) + "\n";
                                        outNote += "Date(s) Released: " + string.Join(", ", dates) + "\n";

                                    // If there’s any info, add it as a comment to both IN and OUT cells
                                    if (!string.IsNullOrWhiteSpace(inNote))
                                    {
                                        var inCell = worksheet.Cell(rowIndex, dataCol);
                                        var outCell = worksheet.Cell(rowIndex, dataCol + 1);

                                        var inComment = inCell.CreateComment();
                                        inComment.AddText(inNote.Trim());
                                        inComment.Style.Alignment.SetAutomaticSize(true);

                                        var outComment = outCell.CreateComment();
                                        outComment.AddText(outNote.Trim());
                                        outComment.Style.Alignment.SetAutomaticSize(true);
                                    }
                                }
                            }
                            else
                            {
                                worksheet.Cell(rowIndex, dataCol).Value = 0;
                                worksheet.Cell(rowIndex, dataCol + 1).Value = 0;
                            }
                            dataCol += 2;
                        }
                    }
                    else
                    {
                        if (propertyName.Equals("remarks", StringComparison.OrdinalIgnoreCase))
                        {
                            worksheet.Cell(rowIndex, dataCol).Value = group.Remarks;
                        }
                        else if (propertyName.Equals("calibration", StringComparison.OrdinalIgnoreCase))
                        {
                            worksheet.Cell(rowIndex, dataCol).Value = group.Calibration;
                        }
                        else
                        {
                            var prop = typeof(InventoryLogbookView).GetProperty(propertyName);
                            if (prop != null)
                            {
                                var value = prop.GetValue(group.First);
                                worksheet.Cell(rowIndex, dataCol).Value = value?.ToString() ?? string.Empty;
                            }
                        }
                        dataCol++;
                    }
                }

                rowIndex++;
            }

            var usedRange = worksheet.RangeUsed();
            if (usedRange != null)
            {
                usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                usedRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                usedRange.Style.Alignment.WrapText = true;
            }
        }

        // Gets all CheckBoxes inside a parent control.
        private IEnumerable<CheckBox> GetAllCheckboxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is CheckBox cb)
                    yield return cb;

                if (ctrl.HasChildren)
                {
                    foreach (var childCb in GetAllCheckboxes(ctrl))
                        yield return childCb;
                }
            }
        }

        public class ReportGenerationResult
        {
            public string FileName { get; set; }
            public string ReportCode { get; set; }
            public string FilePath { get; set; }
            public List<string> SelectedBrands { get; set; }
            public List<string> SelectedItemCategory { get; set; }
            public List<string> SelectedGeneralName { get; set; }
        }

        private void cmb_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            chb_combine_brand.Checked = false;
            chb_combine_general_name.Checked = false;
            chb_combine_item_category.Checked = false;

            chb_combine_brand.Enabled = false;
            chb_combine_general_name.Enabled = false;
            chb_combine_item_category.Enabled = false;
        }

        private void cmb_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            chb_combine_brand.Checked = false;
            chb_combine_general_name.Checked = false;
            chb_combine_item_category.Checked = false;

            chb_combine_brand.Enabled = false;
            chb_combine_general_name.Enabled = false;
            chb_combine_item_category.Enabled = false;
        }
    }
}
