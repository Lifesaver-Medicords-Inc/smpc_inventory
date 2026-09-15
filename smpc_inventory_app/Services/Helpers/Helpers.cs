using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Data; 
using System.Data.SqlTypes;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using smpc_inventory_app.Services.Helpers;
using System.Globalization;
using System.Reflection;
using smpc_inventory_app.Pages.Business_Partner_Info;

namespace smpc_app.Services.Helpers
{
    internal static class Helpers
    {
        public static void HandleNumericColumns(DataGridView dgv, DataGridViewEditingControlShowingEventArgs e, string[] numericColumnNames, params char[] extraAllowedChars)
        {
            if (dgv.CurrentCell == null)
                return;

            string columnName = dgv.Columns[dgv.CurrentCell.ColumnIndex].Name;

            // Always detach first
            e.Control.KeyPress -= NumericColumn_KeyPress;

            if (numericColumnNames.Contains(columnName))
            {
                // Pass allowed characters via Tag
                if (e.Control is TextBox tb)
                {
                    tb.Tag = extraAllowedChars;
                }

                e.Control.KeyPress += NumericColumn_KeyPress;
            }
        }

        private static void NumericColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb = sender as TextBox;
            var extraAllowedChars = tb?.Tag as char[];

            // Allow control keys
            if (char.IsControl(e.KeyChar))
                return;

            // Allow digits
            if (char.IsDigit(e.KeyChar))
                return;

            // Allow decimal point (only once)
            if (e.KeyChar == '.' && tb != null && !tb.Text.Contains("."))
                return;

            // Allow extra characters
            if (extraAllowedChars != null &&
                extraAllowedChars.Contains(e.KeyChar))
                return;

            // Block everything else
            e.Handled = true;
        }

        public static class DatagridviewMapper
        {
            // Model mapper for DataGridView / DataTable
            public static List<T> BuildModelsFromData<T>(object dataSource) where T : new()
            {
                var models = new List<T>();
                var modelType = typeof(T);
                var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // --- CASE 1: DataGridView ---
                if (dataSource is DataGridView dgv)
                {
                    if (dgv.Rows.Count == 0)
                        return models;

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        // 🔹 Check if row has ANY data in mapped columns
                        bool rowHasData = false;

                        foreach (var prop in properties)
                        {
                            if (!dgv.Columns.Contains(prop.Name))
                                continue;

                            var cellValue = row.Cells[prop.Name].Value;

                            if (cellValue != null &&
                                !string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                rowHasData = true;
                                break;
                            }
                        }

                        // ⛔ Skip completely empty rows
                        if (!rowHasData)
                            continue;

                        var model = new T();

                        foreach (var prop in properties)
                        {
                            if (!dgv.Columns.Contains(prop.Name))
                                continue;

                            var value = row.Cells[prop.Name].Value;
                            SetModelPropertyValue(model, prop, value);
                        }

                        models.Add(model);
                    }

                    return models;
                }

                // --- CASE 2: DataTable ---
                if (dataSource is DataTable dt)
                {
                    if (dt.Rows.Count == 0)
                        return models;

                    foreach (DataRow dr in dt.Rows)
                    {
                        var model = new T();

                        foreach (var prop in properties)
                        {
                            if (!dt.Columns.Contains(prop.Name))
                                continue;

                            var value = dr[prop.Name];
                            SetModelPropertyValue(model, prop, value);
                        }

                        models.Add(model);
                    }

                    return models;
                }

                return models;
            }

            // Helper method for safe conversion and assignment
            private static void SetModelPropertyValue<T>(
                T model,
                PropertyInfo prop,
                object value)
            {
                if (value == null || value == DBNull.Value)
                    return;

                try
                {
                    object convertedValue = Convert.ChangeType(
                        value,
                        Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                    );

                    prop.SetValue(model, convertedValue);
                }
                catch
                {
                    // Intentionally ignored
                }
            }
        }

        public static T BuildModelFromPanels<T>(Panel[] panels) where T : new()
        {
            var model = new T();
            var modelType = typeof(T);

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Control control = null;

                foreach (var panel in panels)
                {
                    control = panel.Controls
                        .Cast<Control>()
                        .FirstOrDefault(c =>
                            c.Name.Equals("txt_" + prop.Name, StringComparison.OrdinalIgnoreCase) ||
                            c.Name.Equals("dtp_" + prop.Name, StringComparison.OrdinalIgnoreCase) ||
                            c.Name.Equals("cmb_" + prop.Name, StringComparison.OrdinalIgnoreCase));

                    if (control != null)
                        break;
                }

                if (control == null)
                    continue;

                object value = null;

                if (control is TextBox textBox)
                {
                    string tag = textBox.Tag?.ToString() ?? "";
                    bool isMoney = tag.IndexOf("MONEY", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool isDocument = tag.IndexOf("DOCUMENT", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (isMoney)
                    {
                        // MONEY: try exact stored value first
                        if (!string.IsNullOrWhiteSpace(textBox.AccessibleDescription) &&
                            decimal.TryParse(textBox.AccessibleDescription, out decimal exactVal))
                        {
                            value = exactVal;
                        }
                        else
                        {
                            // fallback parse formatted currency
                            if (decimal.TryParse(
                                textBox.Text,
                                NumberStyles.Currency,
                                CultureInfo.GetCultureInfo("en-PH"),
                                out decimal parsedDecimal))
                            {
                                value = parsedDecimal;
                            }
                            else
                            {
                                value = 0m;
                            }
                        }
                    }
                    else if (isDocument)
                    {
                        // DOCUMENT: get numeric value from AccessibleDescription
                        if (!string.IsNullOrWhiteSpace(textBox.AccessibleDescription) &&
                            int.TryParse(textBox.AccessibleDescription, out int docVal))
                        {
                            value = docVal;
                        }
                        else
                        {
                            // fallback: remove prefix and parse numeric part
                            string numericPart = new string(textBox.Text.Where(char.IsDigit).ToArray());
                            if (int.TryParse(numericPart, out int fallbackVal))
                                value = fallbackVal;
                            else
                                value = 0;
                        }
                    }
                    else
                    {
                        value = textBox.Text;
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    if (comboBox.Tag?.ToString() == "DYNAMIC")
                        value = comboBox.SelectedValue;
                    else
                        value = comboBox.Text;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    value = dateTimePicker.Value.ToString("MM/dd/yyyy");
                }

                if (value != null && prop.CanWrite)
                {
                    try
                    {
                        Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        object convertedValue = Convert.ChangeType(value, targetType);
                        prop.SetValue(model, convertedValue);
                    }
                    catch
                    {
                        // Optional: log error
                    }
                }
            }

            return model;
        }

        public static async Task<bool> ValidateDataGridViewCells(DataGridView dgv, string[] columnsToCheck, bool showError = true)
        {
            bool hasError = false;
            List<DataGridViewCell> invalidCells = new List<DataGridViewCell>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (string colName in columnsToCheck)
                {
                    if (!dgv.Columns.Contains(colName))
                        continue;

                    var cell = row.Cells[colName];
                    string value = cell?.Value?.ToString()?.Trim();

                    bool isEmpty = string.IsNullOrEmpty(value);
                    bool isZero = false;

                    if (decimal.TryParse(value, out decimal numericValue))
                        isZero = numericValue == 0;

                    if (isEmpty || isZero)
                    {
                        hasError = true;
                        invalidCells.Add(cell);
                        cell.Style.BackColor = Color.Red;
                    }
                }
            }

            if (hasError)
            {
                if (showError)
                    ShowDialogMessage("error", "Please ensure all required fields are filled.");

                // Wait 3 seconds before resetting color
                await Task.Delay(3000);

                foreach (var cell in invalidCells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            return hasError;
        }

        public static void SetButtonVisibility(ToolStrip toolStrip, Control parentControl, IEnumerable<string> visibleButtons, IEnumerable<string> hiddenButtons)
        {
            if (toolStrip == null && parentControl == null) return;

            var allControls = new List<Control>();

            if (parentControl != null)
                allControls.AddRange(GetAllControls(parentControl));

            // ToolStrip buttons
            var toolStripButtons = toolStrip?.Items
                .OfType<ToolStripButton>()
                .ToDictionary(b => b.Name, b => b);

            // Show buttons
            foreach (var buttonName in visibleButtons ?? Enumerable.Empty<string>())
            {
                if (toolStripButtons != null && toolStripButtons.TryGetValue(buttonName, out var tsBtn))
                    tsBtn.Visible = true;

                var ctrl = allControls.FirstOrDefault(c => c.Name == buttonName);
                if (ctrl != null)
                    ctrl.Visible = true;
            }

            // Hide buttons
            foreach (var buttonName in hiddenButtons ?? Enumerable.Empty<string>())
            {
                if (toolStripButtons != null && toolStripButtons.TryGetValue(buttonName, out var tsBtn))
                    tsBtn.Visible = false;

                var ctrl = allControls.FirstOrDefault(c => c.Name == buttonName);
                if (ctrl != null)
                    ctrl.Visible = false;
            }
        }

        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;

                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }

        public static class DataGridViewDocumentFormatter
        {
            private static readonly Dictionary<DataGridView, Tuple<string, string, int>> _docConfigs
                = new Dictionary<DataGridView, Tuple<string, string, int>>();

            public static void DataGridViewDocumentFormat(DataGridView dgv, string columnName, string prefix, int digits = 8)
            {
                if (dgv == null) return;

                _docConfigs[dgv] = new Tuple<string, string, int>(columnName, prefix, digits);

                dgv.DataBindingComplete -= Dgv_DataBindingComplete;
                dgv.DataBindingComplete += Dgv_DataBindingComplete;

                dgv.CellFormatting -= Dgv_CellFormatting;
                dgv.CellFormatting += Dgv_CellFormatting;
            }

            private static void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
            {
                var dgv = sender as DataGridView;
                if (dgv == null || !_docConfigs.ContainsKey(dgv)) return;

                var tag = _docConfigs[dgv];

                if (!dgv.Columns.Contains(tag.Item1)) return;

                dgv.Columns[tag.Item1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            private static void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            {
                var dgv = sender as DataGridView;
                if (dgv == null || !_docConfigs.ContainsKey(dgv)) return;

                var tag = _docConfigs[dgv];

                string columnName = tag.Item1;
                string prefix = tag.Item2;
                int digits = tag.Item3;

                if (dgv.Columns[e.ColumnIndex].Name != columnName) return;
                if (e.Value == null) return;

                if (int.TryParse(e.Value.ToString(), out int number))
                {
                    e.Value = prefix + number.ToString($"D{digits}");
                    e.FormattingApplied = true;
                }
            }
        }

        public static void SetChildControlsEnabled(Control[] parents, bool enable, string[] excludeNames)
        {
            foreach (Control parent in parents)
            {
                foreach (Control control in parent.Controls)
                {
                    // Skip excluded controls
                    if (excludeNames != null && excludeNames.Contains(control.Name))
                        continue;

                    // Affect controls of these types
                    if (control is TextBox || control is ComboBox || control is CheckBox || control is DateTimePicker)
                        control.Enabled = enable;

                    // Recurse into child containers
                    if (control.HasChildren)
                        SetChildControlsEnabled(new Control[] { control }, enable, excludeNames);
                }
            }
        }

        public static void SetChildControlsEnabled2(Control[] parents, bool readOnly, string[] excludeNames)
        {
            foreach (Control parent in parents)
            {
                foreach (Control control in parent.Controls)
                {
                    // Skip excluded controls
                    if (excludeNames != null && excludeNames.Contains(control.Name))
                        continue;

                    if (control is TextBox textBox)
                    {
                        textBox.ReadOnly = readOnly;
                        textBox.BackColor = readOnly ? Color.FromArgb(235, 235, 235) : Color.White;
                    }
                    else if (control is ComboBox comboBox)
                    {
                        comboBox.Enabled = !readOnly;
                        comboBox.DropDownStyle = readOnly ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
                        comboBox.BackColor = readOnly ? Color.FromArgb(235, 235, 235) : Color.White;
                    }
                    else if (control is DateTimePicker datePicker)
                        datePicker.Enabled = !readOnly; // No true ReadOnly, fallback behavior

                    else if (control is CheckBox checkBox)
                        checkBox.Enabled = !readOnly; // Prevent user from changing value

                    // Recurse into child containers
                    if (control.HasChildren)
                        SetChildControlsEnabled2(new Control[] { control }, readOnly, excludeNames);
                }
            }
        }

        public static void SetChildControlsEnabledInclude(Control[] parents, bool readOnly, string[] includeNames)
        {
            foreach (Control parent in parents)
            {
                foreach (Control control in parent.Controls)
                {
                    bool shouldAffect = includeNames == null || includeNames.Contains(control.Name);

                    if (shouldAffect)
                    {
                        if (control is TextBox textBox)
                        {
                            textBox.ReadOnly = readOnly;
                            textBox.BackColor = readOnly ? Color.FromArgb(235, 235, 235) : Color.White;
                        }

                        else if (control is ComboBox comboBox)
                        {
                            comboBox.Enabled = !readOnly;
                            comboBox.DropDownStyle = readOnly ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
                            comboBox.BackColor = readOnly ? Color.FromArgb(235, 235, 235) : Color.White;
                        }

                        else if (control is DateTimePicker datePicker)
                            datePicker.Enabled = !readOnly;

                        else if (control is CheckBox checkBox)
                            checkBox.AutoCheck = !readOnly;
                    }

                    // Recurse into child containers
                    if (control.HasChildren)
                        SetChildControlsEnabledInclude(new Control[] { control }, readOnly, includeNames);
                }
            }
        }

        public static void ApplySearchingFilter(DataGridView dataGridView, string searchText, params string[] columnsToSearch)
        {
            if (dataGridView.DataSource == null)
                return;

            DataTable dt = null;

            // Check if DataSource is BindingSource -> unwrap to DataTable
            if (dataGridView.DataSource is BindingSource bs)
            {
                dt = bs.DataSource as DataTable;
                if (dt == null)
                    return;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    bs.RemoveFilter();
                    return;
                }

                string safeText = searchText.Replace("'", "''");

                var filters = columnsToSearch
                    .Where(col => dt.Columns.Contains(col))
                    .Select(col => $"CONVERT([{col}], System.String) LIKE '%{safeText}%'");

                string finalFilter = string.Join(" OR ", filters);

                bs.Filter = finalFilter;
            }
            // Check if DataSource is DataTable directly
            else if (dataGridView.DataSource is DataTable directDt)
            {
                dt = directDt;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                    return;
                }

                string safeText = searchText.Replace("'", "''");

                var filters = columnsToSearch
                    .Where(col => dt.Columns.Contains(col))
                    .Select(col => $"CONVERT([{col}], System.String) LIKE '%{safeText}%'");

                string finalFilter = string.Join(" OR ", filters);

                dt.DefaultView.RowFilter = finalFilter;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            if (items == null || items.Count == 0) return null;

            var dataTable = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties()
                .Where(p => p.CanRead &&
                            !p.PropertyType.IsClass ||
                            p.PropertyType == typeof(string))
                .ToArray();

            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                if (item == null) continue;

                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    try
                    {
                        values[i] = props[i].GetValue(item, null) ?? DBNull.Value;
                    }
                    catch
                    {
                        values[i] = DBNull.Value;
                    }
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static void FreezeVisibleColumns(DataGridView dgv, int count)
        {
            if (dgv == null || dgv.Columns.Count == 0)
                return;

            dgv.SuspendLayout();

            // Reset frozen state
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.Frozen = false;
                col.DividerWidth = 0;
            }

            int frozen = 0;
            DataGridViewColumn lastFrozen = null;

            // Freeze first visible columns only
            foreach (DataGridViewColumn col in dgv.Columns
                         .Cast<DataGridViewColumn>()
                         .OrderBy(c => c.DisplayIndex))
            {
                if (!col.Visible)
                    continue;

                col.Frozen = true;
                lastFrozen = col;

                frozen++;

                if (frozen >= count)
                    break;
            }

            // Add visual divider
            if (lastFrozen != null)
                lastFrozen.DividerWidth = 3;

            dgv.ResumeLayout();

            // VERY IMPORTANT
            dgv.Invalidate();          // redraw headers
            dgv.Refresh();
        }

        public static void RestrictColumnsToNumbers(DataGridView dgv, params string[] columnNames)
        {
            dgv.EditingControlShowing += (s, e) =>
            {
                if (e.Control is TextBox tb)
                {
                    // Always remove previous handler to avoid duplicates
                    tb.KeyPress -= NumericOnly_KeyPress;

                    var colName = dgv.Columns[dgv.CurrentCell.ColumnIndex].Name;

                    if (Array.Exists(columnNames, name => name == colName))
                    {
                        tb.KeyPress += NumericOnly_KeyPress;
                    }
                }
            };
        }

        private static void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, delete, arrows)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block non-numeric
            }
        }

        public static TextBox CreateSearchBox(string placeholderText, EventHandler onTextChanged)
        {
            TextBox txtSearch = new TextBox
            {
                Name = "txt_search",
                Dock = DockStyle.Top,
                ForeColor = Color.Gray,
                Text = placeholderText
            };

            // Event handlers
            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == placeholderText)
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    txtSearch.Text = placeholderText;
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            if (onTextChanged != null)
                txtSearch.TextChanged += onTextChanged;

            return txtSearch;
        }

        public static void EnableGroupHeaders(DataGridView dgv, Dictionary<string, string[]> columnGroups)
        {
            if (dgv == null || columnGroups == null || columnGroups.Count == 0)
                return;

            // Double buffer to reduce flickering
            var doubleBufferedProperty = dgv.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (doubleBufferedProperty != null)
            {
                doubleBufferedProperty.SetValue(dgv, true);
            }

            // Redraw on scroll/resize
            dgv.Scroll += (s, e) => dgv.Invalidate();
            dgv.ColumnWidthChanged += (s, e) => dgv.Invalidate();

            // Paint group headers
            dgv.Paint += (s, e) => DrawGroupHeaders(dgv, e, columnGroups);

            // Override column header painting
            dgv.CellPainting += (s, e) => DrawGroupedHeaderCells(dgv, e);
        }

        private static void DrawGroupHeaders(DataGridView dgv, PaintEventArgs e, Dictionary<string, string[]> groups)
        {
            // Determine frozen column boundary
            int frozenBoundary = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Frozen)
                {
                    Rectangle rect = dgv.GetCellDisplayRectangle(col.Index, -1, true);
                    frozenBoundary = Math.Max(frozenBoundary, rect.Right);
                }
            }

            foreach (var group in groups)
            {
                string groupName = group.Key;
                string[] cols = group.Value;

                if (!cols.All(c => dgv.Columns.Contains(c)))
                    continue;

                DataGridViewColumn firstCol = dgv.Columns[cols.First()];
                DataGridViewColumn lastCol = dgv.Columns[cols.Last()];

                Rectangle r1 = dgv.GetCellDisplayRectangle(firstCol.Index, -1, true);
                Rectangle r2 = dgv.GetCellDisplayRectangle(lastCol.Index, -1, true);

                if (r1.Width <= 0 || r2.Width <= 0)
                    continue;

                // If the first column is hidden behind frozen columns, don't draw the group text
                bool firstColumnHiddenByFrozen = r1.X < frozenBoundary;

                Rectangle headerRect = new Rectangle(
                    Math.Max(r1.X, frozenBoundary),
                    r1.Y,
                    r2.Right - Math.Max(r1.X, frozenBoundary),
                    r1.Height / 2
                );

                if (headerRect.Width <= 0)
                    continue;

                using (Brush b = new SolidBrush(SystemColors.Control))
                    e.Graphics.FillRectangle(b, headerRect);

                e.Graphics.DrawRectangle(Pens.Gray, headerRect);

                if (!firstColumnHiddenByFrozen)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        groupName,
                        dgv.ColumnHeadersDefaultCellStyle.Font,
                        headerRect,
                        Color.Black,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                }
            }
        }

        private static void DrawGroupedHeaderCells(DataGridView dgv, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                Rectangle rect = dgv.GetCellDisplayRectangle(e.ColumnIndex, -1, true);

                // Determine frozen boundary
                int frozenBoundary = 0;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Frozen)
                    {
                        Rectangle r = dgv.GetCellDisplayRectangle(col.Index, -1, true);
                        frozenBoundary = Math.Max(frozenBoundary, r.Right);
                    }
                }

                // If column header is completely behind frozen columns, skip drawing
                if (rect.Right <= frozenBoundary)
                    return;

                e.PaintBackground(e.CellBounds, true);

                Rectangle textRect = e.CellBounds;

                // Prevent drawing behind frozen area
                if (textRect.X < frozenBoundary)
                {
                    int diff = frozenBoundary - textRect.X;
                    textRect.X += diff;
                    textRect.Width -= diff;
                }

                // Bottom half for column text
                textRect.Y += textRect.Height / 2;
                textRect.Height /= 2;

                TextRenderer.DrawText(
                    e.Graphics,
                    e.FormattedValue?.ToString() ?? "",
                    e.CellStyle.Font,
                    textRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis
                );

                e.Handled = true;
            }
        }

        public static void ApplySearchFilter(DataGridView dataGridView, string searchText, params string[] columnsToSearch)
        {
            if (dataGridView.DataSource == null)
                return;

            DataTable dt = null;

            // Check if DataSource is BindingSource -> unwrap to DataTable
            if (dataGridView.DataSource is BindingSource bs)
            {
                dt = bs.DataSource as DataTable;
                if (dt == null)
                    return;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    bs.RemoveFilter();
                    return;
                }

                string safeText = searchText.Replace("'", "''");

                var filters = columnsToSearch
                    .Where(col => dt.Columns.Contains(col))
                    .Select(col => $"CONVERT([{col}], System.String) LIKE '%{safeText}%'");

                string finalFilter = string.Join(" OR ", filters);

                bs.Filter = finalFilter;
            }
            // Check if DataSource is DataTable directly
            else if (dataGridView.DataSource is DataTable directDt)
            {
                dt = directDt;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                    return;
                }

                string safeText = searchText.Replace("'", "''");

                var filters = columnsToSearch
                    .Where(col => dt.Columns.Contains(col))
                    .Select(col => $"CONVERT([{col}], System.String) LIKE '%{safeText}%'");

                string finalFilter = string.Join(" OR ", filters);

                dt.DefaultView.RowFilter = finalFilter;
            }
        }

        public static class Placeholder
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

            private const int EM_SETCUEBANNER = 0x1501;

            public static void SetPlaceholder(TextBox textBox, string placeholder)
            {
                if (textBox == null) throw new ArgumentNullException(nameof(textBox));

                // If handle already exists, set immediately
                if (textBox.IsHandleCreated)
                {
                    SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, placeholder);
                }
                else
                {
                    // If not, wait for handle creation
                    textBox.HandleCreated += (s, e) =>
                    {
                        SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, placeholder);
                    };
                }
            }
        }

        // The one standard loading screen (spec 2.1): "Loading... Please wait", it
        // covers the WHOLE PAGE rather than one section, and it clears only once
        // every part of the page has finished loading.
        //
        // Rewritten 2026-09-12. The previous version broke all three of those rules
        // and the bugs were not cosmetic:
        //
        //   - It overlaid whatever control the caller passed - in practice a single
        //     DataGridView - so the grid was covered while the toolbar, the header
        //     fields and the action buttons stayed live. A user could press Save on
        //     a form whose data had not arrived yet.
        //
        //   - One static overlay for the entire app, first-wins: a second load
        //     starting while one was up returned silently, and then the FIRST
        //     HideLoading tore the overlay down while the other load was still
        //     running. A page with two or three fetches cleared early, every time.
        //
        //   - HideLoading disposed and nulled the static field while removing the
        //     panel from whichever control it was handed. Hand it a different
        //     control than Show got - easy, since callers pass grids - and the
        //     overlay is orphaned on screen with nothing left pointing at it.
        //
        // Now: the host is resolved UP to the page that contains it, input to the
        // page is held back for the duration, and shows are REF-COUNTED per
        // page, so the screen lifts on the last completion rather than the first.
        //
        // Callers need no change - every existing call site passes some control on
        // the page, which is exactly what this resolves from.
        public static class Loading
        {
            // Spec 2.1's exact wording. Callers may pass their own message, but the
            // default is the standard one and should stay that way.
            public const string StandardMessage = "Loading... Please wait";

            private class PageOverlay
            {
                public Panel Panel;
                public int Depth;
            }

            // Clicks, keys and wheel turns aimed at anything on a covered page are swallowed
            // here, before the control they were meant for sees them. This used to be done by
            // disabling the page's controls and switching them all back on when the screen
            // cleared - which restored the state from when the screen went UP, so a save that
            // settles the form read-only (spec 2.1) while the screen is up would have had its
            // locked panels switched straight back on. Holding the input back instead leaves
            // every control's Enabled state to the page's own code. The cover itself still takes
            // the mouse: there is nothing on it to press, and a wheel turn over it scrolls the page.
            private class InputBlocker : IMessageFilter
            {
                private const int FirstKeyMessage = 0x0100;   // WM_KEYDOWN
                private const int LastKeyMessage = 0x0109;    // WM_UNICHAR
                private const int FirstMouseMessage = 0x0201; // WM_LBUTTONDOWN - movement passes
                private const int LastMouseMessage = 0x020E;  // WM_MOUSEHWHEEL

                public bool PreFilterMessage(ref Message m)
                {
                    if (Overlays.Count == 0) return false;

                    bool input = (m.Msg >= FirstKeyMessage && m.Msg <= LastKeyMessage)
                        || (m.Msg >= FirstMouseMessage && m.Msg <= LastMouseMessage);
                    if (!input) return false;

                    Control target = Control.FromChildHandle(m.HWnd);
                    for (Control c = target; c != null; c = c.Parent)
                    {
                        PageOverlay record;
                        if (Overlays.TryGetValue(c, out record))
                            return target != record.Panel;
                    }

                    return false;
                }
            }

            private static bool inputBlockerInstalled;

            // Keyed per page, so two pages loading at once do not cancel each other.
            private static readonly Dictionary<Control, PageOverlay> Overlays =
                new Dictionary<Control, PageOverlay>();

            // Resolves the PAGE a control belongs to - the UserControl sitting in the
            // tab - and deliberately stops short of the Form.
            //
            // The first version of this accepted `c is UserControl || c is Form` while
            // walking all the way up, overwriting as it went, so it always finished on
            // the Form: the loading screen covered the entire window, sidebar, red box
            // and tab strip included, instead of the tab's content.
            //
            // Outermost UserControl rather than innermost, because pages are built from
            // nested UserControls (ItemSetUC inside a quotation, BpiBranchUC inside a
            // partner) and covering only the inner one would leave most of the page
            // live. The Form is used only when there is no UserControl above the
            // control at all, which is the modal-dialog case - there the dialog IS the
            // page.
            private static Control ResolvePage(Control control)
            {
                if (control == null) return null;

                Control page = null;
                for (Control c = control; c != null; c = c.Parent)
                {
                    if (c is UserControl) page = c;
                }

                // A dialog, or a control not parented yet - a load kicked off from a
                // constructor, where walking up finds nothing.
                return page ?? control.FindForm() ?? control;
            }

            public static void ShowLoading(Control host, string message = StandardMessage)
            {
                Control page = ResolvePage(host);
                if (page == null || page.IsDisposed) return;

                PageOverlay existing;
                if (Overlays.TryGetValue(page, out existing))
                {
                    // Another fetch on the same page. Count it and leave the screen up.
                    existing.Depth++;
                    return;
                }

                // Deliberately NOT Dock = Fill. A Fill-docked child is added at the end
                // of the Controls collection, which makes it dock FIRST and lays every
                // other docked sibling out in what is left - nothing. The siblings then
                // reflow again when it is removed, and any page whose layout is not
                // perfectly reversible comes back subtly rearranged.
                //
                // Explicit bounds keep the overlay out of dock layout entirely.
                // ClientRectangle rather than DisplayRectangle on purpose - on a
                // scrollable page DisplayRectangle can be larger than the visible area,
                // and a child that big extends the scroll region, which is its own
                // phantom-space bug.
                //
                // No anchors either. Sales Quotation starts loading from its Load event,
                // before its tab has given it its final size, and it scrolls (AutoScroll):
                // an anchored overlay stayed at the size it started with - a grey block over
                // one corner, the message out of sight (user-reported 2026-09-15). The overlay
                // is instead put back over the visible area whenever the page lays out,
                // resizes or scrolls, for as long as it is up (keepOverPage, below).
                var overlay = new Panel
                {
                    // Darker than a plain grey wash so the white box stands out against it.
                    BackColor = Color.FromArgb(150, 40, 40, 40),
                    Name = "pnl_page_loading",
                    Bounds = page.ClientRectangle,
                };

                // Everything is painted by the cover itself, in one pass into an off-screen buffer,
                // and redrawn in full whenever it resizes. The box used to be a child panel of its
                // own and the cover was drawn straight to the screen in two passes - the page behind
                // it, then the grey - with Windows filling the box in as a separate, later step. At
                // the start of every load that showed as a flicker: grey with a hole in it, then the
                // box (user-reported 2026-09-15). Cover, box, ring and message now reach the screen
                // in the same frame.
                typeof(Control).GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.Invoke(overlay, new object[] { ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true });

                // A plain box in the middle of the covered page (the page stays covered, spec 2.1):
                // a turning ring above the standard message, moved by a UI-thread timer so it keeps
                // turning while the page awaits the API and stops only if the UI thread itself is
                // busy. Square corners like the rest of the app; the message in the default C# font,
                // bold as a modal's header is (1.4); neutral greys, since red is the attention colour
                // and there is no blue convention. The spec names only the message and the full-page
                // cover, so the ring and the box are a provisional standard, chosen 2026-09-15 - the
                // sliding bar, rounded corners and Segoe UI first tried with them were taken out the
                // same day at the user's request.
                var messageFont = new Font(Control.DefaultFont, FontStyle.Bold);
                var boxSize = new Size(Math.Max(240, TextRenderer.MeasureText(message, messageFont).Width + 64), 108);
                var ink = Color.FromArgb(70, 70, 70);
                var faint = Color.FromArgb(225, 225, 225);
                int angle = 0;

                // Worked out at paint time, so the box stays in the middle as the cover follows the page.
                Func<Rectangle> boxBounds = () => new Rectangle(
                    Math.Max(0, (overlay.ClientSize.Width - boxSize.Width) / 2),
                    Math.Max(0, (overlay.ClientSize.Height - boxSize.Height) / 2),
                    boxSize.Width, boxSize.Height);
                Func<Rectangle> ringBounds = () =>
                {
                    var b = boxBounds();
                    return new Rectangle(b.X + b.Width / 2 - 20, b.Y + 18, 40, 40);
                };

                overlay.Paint += (s, e) =>
                {
                    var g = e.Graphics;

                    var b = boxBounds();
                    using (var fill = new SolidBrush(Color.White))
                        g.FillRectangle(fill, b);

                    // Anti-aliased for the ring only; the box edges are straight.
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var ring = ringBounds();
                    using (var track = new Pen(faint, 5))
                    using (var arc = new Pen(ink, 5))
                    {
                        arc.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        arc.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawEllipse(track, ring);
                        g.DrawArc(arc, ring, angle, 100);
                    }

                    TextRenderer.DrawText(g, message, messageFont, new Rectangle(b.X, b.Y + 68, b.Width, 24), ink,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };

                var spin = new System.Windows.Forms.Timer { Interval = 40 };
                spin.Tick += (s, e) =>
                {
                    angle = (angle + 12) % 360;
                    // Only the ring changes from one frame to the next.
                    var ring = ringBounds();
                    ring.Inflate(6, 6);
                    overlay.Invalidate(ring);
                };
                overlay.Disposed += (s, e) =>
                {
                    spin.Dispose();
                    messageFont.Dispose();
                };
                spin.Start();

                var record = new PageOverlay { Panel = overlay, Depth = 1 };

                // Suspended around the add so the overlay never triggers a layout pass
                // of its own over the page's real controls.
                page.SuspendLayout();
                page.Controls.Add(overlay);
                overlay.BringToFront();
                page.ResumeLayout(false);

                // Painted now rather than whenever the message loop next gets to it. The caller
                // usually carries straight on with work of its own - binding, the first request -
                // and until the cover has painted the page shows through where it should be.
                overlay.Update();

                // Input to the page is held back until the screen clears (InputBlocker). A
                // Cancel pressed during a long save would otherwise throw the save away.
                if (!inputBlockerInstalled)
                {
                    Application.AddMessageFilter(new InputBlocker());
                    inputBlockerInstalled = true;
                }

                // Bounds are written only when they differ, and the overlay is brought forward
                // only when something has been put above it, so the Layout this answers never
                // sets itself off again. Unhooked when HideLoading disposes the overlay.
                EventHandler keepOverPage = (s, e) =>
                {
                    if (overlay.IsDisposed || page.IsDisposed || overlay.Parent != page) return;
                    if (overlay.Bounds != page.ClientRectangle) overlay.Bounds = page.ClientRectangle;
                    if (page.Controls.GetChildIndex(overlay) != 0) overlay.BringToFront();
                };
                LayoutEventHandler keepOnLayout = (s, e) => keepOverPage(s, e);
                ScrollEventHandler keepOnScroll = (s, e) => keepOverPage(s, e);
                var scrollable = page as ScrollableControl;

                page.Layout += keepOnLayout;
                page.ClientSizeChanged += keepOverPage;
                if (scrollable != null) scrollable.Scroll += keepOnScroll;
                // The mouse wheel scrolls a page without raising Scroll, and every kind of scroll
                // carries the overlay along with the content - so it also snaps back when moved.
                overlay.LocationChanged += keepOverPage;
                overlay.Disposed += (s, e) =>
                {
                    page.Layout -= keepOnLayout;
                    page.ClientSizeChanged -= keepOverPage;
                    if (scrollable != null) scrollable.Scroll -= keepOnScroll;
                };

                Overlays[page] = record;
            }

            public static void HideLoading(Control host)
            {
                Control page = ResolvePage(host);
                if (page == null) return;

                PageOverlay record;
                if (!Overlays.TryGetValue(page, out record)) return;

                // Only the last outstanding load clears the screen (spec 2.1).
                record.Depth--;
                if (record.Depth > 0) return;

                Overlays.Remove(page);

                if (!page.IsDisposed)
                {
                    page.SuspendLayout();
                    page.Controls.Remove(record.Panel);
                    page.ResumeLayout(true);
                }

                record.Panel.Dispose();
            }

            // Clears a page's loading screen no matter how many shows are outstanding.
            // For a failure path that has to bail out of several fetches at once - a
            // dropped connection - where matching every Show with a Hide is not
            // practical. Never call it to "make sure" the screen is gone: that is the
            // early-clear bug this class was rewritten to remove.
            public static void ForceHide(Control host)
            {
                Control page = ResolvePage(host);
                if (page == null) return;

                PageOverlay record;
                if (!Overlays.TryGetValue(page, out record)) return;

                record.Depth = 1;
                HideLoading(page);
            }
        }

        // Fits the page in the active tab to the space the window gives it (spec 1.3: responsive,
        // never by squeezing the centre pane). A page is never made smaller than its natural size -
        // its Designer size, or whatever size its own code later gives it - but it is stretched to
        // fill the tab whenever the window has more room than that. Nearly every page is built from
        // docked bands (header Top, body Fill, footer Bottom), so the bands, grids, inner tab
        // controls and right-aligned toolbar buttons follow the new size; fields placed at fixed
        // positions inside a band stay where they are. A page bigger than the window keeps its size:
        // the tab control grows past the window and the panel it sits in scrolls.
        //
        // Before this only the tab control was sized. The page kept its Designer size and sat in
        // the top-left corner of its tab with dead space to the right and below on a large monitor
        // (user-reported 2026-09-15). Proportional Control.Scale() was tried in August 2026 and
        // garbled the pages' AutoSize labels, so pages are stretched, never scaled. Copy-identical
        // in the four apps that open pages in tabs (sales, inventory, engineering, accounting).
        public static class PageFit
        {
            private static readonly Dictionary<Control, Size> NaturalSizes = new Dictionary<Control, Size>();

            // Set while Fit resizes a page, so the page's SizeChanged can tell that apart from the
            // page resizing itself.
            private static bool fitting;

            // Starts watching a page. Safe to call more than once for the same page.
            public static void Track(Control page, Action refit)
            {
                if (page == null || page.IsDisposed || NaturalSizes.ContainsKey(page)) return;

                NaturalSizes[page] = page.Size;

                // Only Fit sizes a tracked page. Anchored to the right or bottom edge it would also
                // stretch on its own, that stretched size would be taken for its natural size, and it
                // could never shrink again; AutoSize would fight the sizes Fit gives it.
                page.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                page.AutoSize = false;

                // A page may resize itself once it is open - Quotation changes its height when it
                // switches between Quick Quote and Project, Orders sets its own width - and the size it
                // chooses becomes the one it is never made smaller than.
                page.SizeChanged += (s, e) =>
                {
                    if (fitting || page.IsDisposed) return;
                    NaturalSizes[page] = page.Size;
                    refit?.Invoke();
                };
                page.Disposed += (s, e) => NaturalSizes.Remove(page);
            }

            // The page showing in the selected tab. Pages replace themselves inside their own tab by
            // adding the next page and hiding or disposing themselves (Opportunities -> Quotation,
            // Quotation -> Sales Order and back), so the one on screen is the last one added.
            public static Control ActivePage(TabControl tabs)
            {
                // SelectedTab can throw while the Designer's SelectedIndex points at a tab that does
                // not exist yet - every fresh launch, before anything is opened - so check first.
                if (tabs == null || tabs.TabPages.Count == 0) return null;
                TabPage selected = tabs.SelectedTab;
                if (selected == null) return null;

                for (int i = selected.Controls.Count - 1; i >= 0; i--)
                {
                    if (!selected.Controls[i].IsDisposed) return selected.Controls[i];
                }
                return null;
            }

            // Sizes the tab control, and the page in its selected tab, to the scrolling panel (host)
            // they sit in. refit is what the host calls to run this again.
            public static void Fit(ScrollableControl host, TabControl tabs, Action refit)
            {
                if (host == null || tabs == null) return;

                Size available = host.ClientSize;
                Control page = ActivePage(tabs);

                // The tab control goes at the top-left of the host's content, not of its visible
                // area. Bounds are client coordinates, which move as the host scrolls: placed at
                // (0, 0) while the host was scrolled down, the tab control landed that far below the
                // start of the content, and when the page then shrank (Sales Quotation, Project back
                // to Quick Quote) the scroll range collapsed and left that distance as empty space
                // above the tab (user-reported 2026-09-15). AutoScrollPosition is where the content's
                // origin currently is.
                Point origin = host.AutoScrollPosition;

                // Nothing open, or a page that docks itself and so looks after its own size.
                if (page == null || page.Dock != DockStyle.None)
                {
                    tabs.Bounds = new Rectangle(origin.X, origin.Y, available.Width, available.Height);
                    return;
                }

                Track(page, refit);
                Size natural;
                if (!NaturalSizes.TryGetValue(page, out natural)) natural = page.Size;

                Padding padding = page.Parent is TabPage tabPage ? tabPage.Padding : Padding.Empty;

                // The tab strip and border around a tab's display area, plus the tab page's padding.
                int chromeWidth = tabs.Width - tabs.DisplayRectangle.Width + padding.Horizontal;
                int chromeHeight = tabs.Height - tabs.DisplayRectangle.Height + padding.Vertical;

                tabs.Bounds = new Rectangle(origin.X, origin.Y,
                    Math.Max(available.Width, natural.Width + chromeWidth),
                    Math.Max(available.Height, natural.Height + chromeHeight));

                Rectangle display = tabs.DisplayRectangle;
                fitting = true;
                try
                {
                    page.Size = new Size(
                        Math.Max(natural.Width, display.Width - padding.Horizontal),
                        Math.Max(natural.Height, display.Height - padding.Vertical));
                }
                finally
                {
                    fitting = false;
                }
            }
        }

        public static void SetReadOnlyControl(DataGridView dg, bool? boolean = null)
        {
            Console.WriteLine($"{dg.Name}: [");
            foreach (DataGridViewRow row in dg.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!cell.Visible) continue; // skip hidden cells

                    if (cell.OwningColumn is DataGridViewTextBoxColumn)
                    {
                        if (boolean.HasValue)
                            cell.ReadOnly = boolean.Value;
                        else
                            cell.ReadOnly = !cell.ReadOnly;

                        Console.WriteLine($"  Row {row.Index}, " +
                            $"Col {cell.ColumnIndex} (TextBox) ReadOnly = {cell.ReadOnly}");
                    }
                    else if (cell.OwningColumn is DataGridViewComboBoxColumn)
                    {
                        if (boolean.HasValue)
                            cell.ReadOnly = boolean.Value;
                        else
                            cell.ReadOnly = !cell.ReadOnly;

                        Console.WriteLine($"  Row {row.Index}, " +
                            $"Col {cell.ColumnIndex} (ComboBox) ReadOnly = {cell.ReadOnly}");
                    }
                    // need to add other types (checkbox, button, etc.)
                }
            }
            Console.WriteLine("]\n");
        }
        public static void SetPanelToReadOnly(Panel pnl, bool? status = null)
        {
            Console.WriteLine(pnl.Name + ": [");
            foreach (Control control in pnl.Controls)
            {
                if (!control.Visible
                    && (!(control is DataGridView) && control.Parent is TabControl) //skip if control is 
                    || control is Label)
                    continue; //hidden, label or datagrid, since controlTab makes other dg unvisible when tab aint active
                if (control.Tag != null &&
                    (control.Tag.Equals("no_edit") || control.Tag.Equals("manual")))
                    continue; //skip if control is hidden

                if (control is TextBox txt)
                {
                    if (status.HasValue)
                        txt.ReadOnly = status.Value; // if there thrown boolean
                    else
                        txt.ReadOnly = !txt.ReadOnly; // toggle if no action provided  
                    if (txt.ReadOnly) txt.TabStop = false;
                    Console.WriteLine(" " + control.Name +
                        ".ReadOnly = " + txt.ReadOnly.ToString());
                }
                else if (control is ComboBox cmb)
                {
                    if (status.HasValue)
                        cmb.Enabled = !status.Value;
                    else
                        cmb.Enabled = !cmb.Enabled;
                    Console.WriteLine(" " + control.Name +
                        ".Enabled = " + cmb.Enabled.ToString());
                }
                else if (control is CheckBox chk)
                {
                    if (status.HasValue)
                        chk.Enabled = !status.Value;
                    else
                        chk.Enabled = !chk.Enabled;
                    Console.WriteLine(" " + control.Name +
                        ".Enabled = " + chk.Enabled.ToString());
                }
                else if (control is Button btn)
                {
                    if (status.HasValue)
                        btn.Enabled = !status.Value;
                    else
                        btn.Enabled = !btn.Enabled;
                    Console.WriteLine(" " + control.Name +
                        ".Enabled = " + btn.Enabled.ToString());
                }
                else if (control is DataGridView dgv) //datagridview is datagdrid
                {   //not status since it is reversed no deletion : yes deletion
                    dgv.AllowUserToAddRows = status.HasValue ? !status.Value : false; //outOfRange ex warning here if the dgv isnt visible/loaded
                    dgv.AllowUserToDeleteRows = status.HasValue ? !status.Value : false;
                    dgv.Enabled = true;
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (!col.Visible)
                            continue; //skip hidden columns

                        try
                        {
                            // A DataGridViewColumn bound (via DataPropertyName) to a DataColumn
                            // that is itself ReadOnly (e.g. an auto-increment "id" column - which
                            // a DataTable can end up marking ReadOnly on its own once the table is
                            // emptied and its schema gets re-inferred/rebuilt from scratch) can
                            // NEVER be set ReadOnly = false; WinForms throws
                            // InvalidOperationException if you try. Once that underlying column is
                            // read-only there's nothing this "make it editable" toggle can do about
                            // it, so just force True and move on instead of blowing up the whole
                            // panel toggle (which otherwise breaks Edit mode for every other field
                            // and grid on the form too, not just this column).
                            if (IsBoundToReadOnlyDataColumn(dgv, col))
                            {
                                col.ReadOnly = true;
                            }
                            else if (status.HasValue)
                            {
                                col.ReadOnly = status.Value;
                            }
                            else
                            {
                                col.ReadOnly = !col.ReadOnly;
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            // Belt-and-suspenders: if some other, not-yet-seen case still hits the
                            // same "must have ReadOnly set to True" restriction, don't let one
                            // column crash this shared helper for every screen that calls it -
                            // leave it ReadOnly and keep going.
                            col.ReadOnly = true;
                            Console.WriteLine(" " + dgv.Name + "." + col.Name +
                                ".ReadOnly could not be changed (" + ex.Message + "), forced to True");
                        }
                        Console.WriteLine(" " + dgv.Name + "." + col.Name +
                            ".ReadOnly = " + col.ReadOnly);
                    }
                }
            }
            pnl.Enabled = true;
            Console.WriteLine("]\n");
        }

        // Resolves the DataGridViewColumn back to the DataColumn it's bound to (walking
        // through a BindingSource if that's how the grid is wired, which is how
        // frm_warehouse_name_setup's dg_areas is bound - dgv.DataSource is a BindingSource
        // wrapping a DataView over the "areas" DataTable) and reports whether that
        // DataColumn itself is ReadOnly. Returns false (not a problem) for anything not
        // DataTable/DataView-backed, e.g. grids bound to a List<T>/BindingList<T>, where
        // this restriction doesn't apply.
        private static bool IsBoundToReadOnlyDataColumn(DataGridView dgv, DataGridViewColumn col)
        {
            if (string.IsNullOrEmpty(col.DataPropertyName)) return false;

            object source = dgv.DataSource;
            if (source is BindingSource bs) source = bs.DataSource;

            DataTable table = source as DataTable;
            if (table == null && source is DataView dv) table = dv.Table;

            if (table == null || !table.Columns.Contains(col.DataPropertyName)) return false;

            return table.Columns[col.DataPropertyName].ReadOnly;
        }

        public static DataTable ConvertDataGridViewToDataTable(DataGridView dgv, string childName = "")
        {
            DataTable dataTable = new DataTable();

            //Add columns to DataTable
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Name.Contains("id_")) //for multiple children 
                { //e.g dg_id_child name - will remove child name and set it as id
                    dataTable.Columns.Add(column.Name = "id");
                }
                else if (column.Name.Contains("dg_"))
                {            // e.g dg_child_name_parent_id => dg_parent_id
                    string tmpColName = column.Name;
                    if (column.Name.Contains(childName) && !string.IsNullOrEmpty(childName))
                    {
                        dataTable.Columns.Add(tmpColName.Replace("dg_" + childName + "_", ""));
                    }//just to let children have their own name
                    else dataTable.Columns.Add(tmpColName.Replace("dg_", "")); //for orphans
                }
                else //for old
                {
                    dataTable.Columns.Add(column.Name);
                }
            }

            // Add rows to DataTable
            foreach (DataGridViewRow row in dgv.Rows)
            {
                // Skip the new row placeholder if it's present
                if (!row.IsNewRow)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        dataRow[i] = row.Cells[i].Value ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        } 
        public static dynamic ConvertDataTableToDictionary(DataTable dt, string parentKey = "")
        {
            var rowsList = new List<Dictionary<string, dynamic>>();

            foreach (DataRow row in dt.Rows)
            {
                var rowDict = new Dictionary<string, dynamic>();

                foreach (DataColumn col in dt.Columns)
                {
                    var cellValue = row[col]?.ToString() ?? "";

                    if (int.TryParse(cellValue, out int intValue) &&
                        (col.ColumnName.Contains("_id")
                          || col.ColumnName.Equals("id"))) //if referencing id and/or id itself, parse it
                    {
                        rowDict[col.ColumnName] = intValue;
                    }
                    else
                    {
                        if ((col.ColumnName.Equals("id")
                               || col.ColumnName.Contains("_id")) //backend dapat pag set neto
                            && string.IsNullOrEmpty(row[col]?.ToString()))
                        {  //skip empty id
                            continue;
                        }
                        rowDict[col.ColumnName] = cellValue;
                    }
                }

                rowsList.Add(rowDict);
            }
            //turns to "headkey":[{list of kvp}]
            if (!string.IsNullOrEmpty(parentKey))
            {
                return new Dictionary<string, List<Dictionary<string, dynamic>>>
                {
                    [parentKey] = rowsList
                };
            }
            else
            {
                return rowsList;
            }
        }

        public static void ResetControls(Panel[] pnls)
        {
            foreach (Panel pnl in pnls)
            {
                foreach (Control control in pnl.Controls)
                {
                    // Check if the control is a TextBox
                    if (control is TextBox textBox)
                    {
                        // Reset the TextBox's text
                        textBox.Text = "";
                    }
                    else if (control is ComboBox combobox)
                    {
                        combobox.SelectedIndex = -1;
                    }
                    // Reset DateTimePicker to current date
                    else if (control is DateTimePicker datePicker)
                    {
                        datePicker.Value = DateTime.Now;   // or DateTime.Today
                    }
                }
            }
        }

        public static void ResetControls(Panel pnl)
        {
            foreach (Control control in pnl.Controls)
            {
                // Reset TextBox
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
                // Reset DateTimePicker to current date
                else if (control is DateTimePicker datePicker)
                {
                    datePicker.Value = DateTime.Now;   // or DateTime.Today
                }
                else if (control is ComboBox combobox)
                {
                    combobox.SelectedIndex = -1;
                }
                else if (control is RichTextBox richTextBox)
                {
                    richTextBox.Text = string.Empty;
                }
            }
        }
        //public static void ResetControls(Panel[] pnls)
        //{
        //    foreach (Panel pnl in pnls)
        //    {
        //        foreach (Control control in pnl.Controls)
        //        {
        //            // Reset TextBox
        //            if (control is TextBox textBox)
        //            {
        //                textBox.Text = "";
        //            }
        //            // Reset ComboBox
        //            else if (control is ComboBox combobox)
        //            {
        //                combobox.SelectedIndex = -1;
        //            }
        //            // Reset DateTimePicker
        //            else if (control is DateTimePicker datePicker)
        //            {
        //                datePicker.Value = DateTime.Now;   // or DateTime.Today
        //            }
        //        }
        //    }
        //}

        // Control.Tag can hold multiple comma-separated markers (e.g. "DYNAMIC, REQUIRED").
        // Exact-equality checks like `Tag == "DYNAMIC"` silently fail for those compound tags,
        // which was causing dropdowns like Item Name / Item Brand / Item Class / Unit of Measure
        // to be sent to the API under the wrong key (e.g. "item_name" with display text) instead
        // of "item_name_id" with the selected ID — so the backend never received the real value.
        internal static bool TagHasMarker(object tag, string marker)
        {
            string tagStr = tag as string;
            if (string.IsNullOrWhiteSpace(tagStr)) return false;

            return tagStr.Split(',')
                          .Select(t => t.Trim())
                          .Any(t => string.Equals(t, marker, StringComparison.OrdinalIgnoreCase));
        }

        public static Dictionary<string, dynamic> GetControlsValues(Panel pnl)
        {
            Dictionary<string,dynamic> values = new Dictionary<string, dynamic>();
            foreach (Control control in pnl.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {

                    string key = textBox.Name.Replace("txt_", "");
                    string val = "";

                    if (textBox.Tag == "MONEY")
                    {

                        val = String.Format("{0}", textBox.Text.ToString().Replace(",", ""));
                    }

                    if (textBox.Tag != null   && textBox.Tag is List<int> ids && ids.Count > 0)
                    {
                        // Assuming Tag contains a list of IDs (if applicable)
                      
                        
                            values.Add(key + "_id", ids);  // Add the list of IDs under the key + "_id"
                       

                    }


                    else
                    {
                        val = String.Format("{0}", textBox.Text.ToString());

                        if (key == "id" && val != "")
                        {

                            values.Add(key, int.Parse(val));
                        }
                        else
                        {
                            values.Add(key, val);
                        }
                    }
                }

                // Check if the control is a Combobox
                if (control is ComboBox comboBox)
                {
                    string key = comboBox.Name.Replace("cmb_", "");
                    string val = "";

                    //if (string.IsNullOrEmpty(comboBox.Text.ToString()))
                    //{
                    //    val = "";
                    //}
                    //else
                    //{
                    //    val = comboBox.Text.ToString();
                    //}

                    if (TagHasMarker(comboBox.Tag, "DYNAMIC"))
                    {
                        key = key + "_id";
                        values.Add(key, comboBox.SelectedValue);
                    }
                
                    else
                    {
                        val = comboBox.Text.ToString();

                        values.Add(key, val);
                    }

                }

                // Check if the control is a Checkbox
                if (control is CheckBox checkbox)
                {
                    string key = checkbox.Name.Replace("chk_", "");
                    //int val = checkbox.Checked ? 1 : 0;
                    bool val = checkbox.Checked ? true : false;
                    values.Add(key, val);
                }

                // Check if the control is a DATETIME PICKER
                if (control is DateTimePicker dateTimePicker)
                {
                    string key = dateTimePicker.Name.Replace("dtp_", "");
                    string val = String.Format("'{0:yyyy-MM-dd}'", dateTimePicker.Value);
                    values.Add(key, val);
                }

                // Check if the control is a NUMERIC
                if (control is NumericUpDown numericUpDown)
                {
                    string key = numericUpDown.Name.Replace("txt_", "");
                    string val = String.Format("'{0}'", numericUpDown.Value);
                    values.Add(key, val);
                }

                // Check if the control is a Rich Textbox
                if (control is RichTextBox richTextBox)
                {
                    string key = richTextBox.Name.Replace("rtxt_", "");
                    dynamic val = richTextBox.Text.ToString();

                    values[key] = val;

                }
            }

            return values;
        }

        internal static void BindControls(Panel[] pnlItemSales, List<object> priceList, int selectedRecord)
        {
            throw new NotImplementedException();
        }
        public static Dictionary<string, dynamic> GetControlsValues(Panel[] pnl1)
        {
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();

            foreach (Panel pnl in pnl1)
            {
                foreach (Control control in pnl.Controls)
                {
                    // Check if the control is a TextBox
                    if (control is TextBox textBox)
                    {
                        string key = textBox.Name.Replace("txt_", "");
                        dynamic val = null;

                        if (textBox.Tag != null && textBox.Tag.ToString() == "MONEY")
                        {
                            if (decimal.TryParse(textBox.AccessibleDescription, out decimal exactVal))
                            {
                                val = exactVal;
                            }
                            else
                            {
                                // fallback to parsing cleaned text
                                string isParsed = GetCleanedPriceValue(textBox.Text);
                                if (decimal.TryParse(isParsed, out decimal tempVal))
                                {
                                    val = tempVal;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid money format. Please enter a valid number.");
                                    val = 0;
                                }
                            }
                        }

                        else if (textBox.Tag != null && textBox.Tag is List<int> ids && ids.Count > 0)
                        {
                            // Assuming Tag contains a list of IDs (if applicable)


                            values.Add(key + "_id", ids);  // Add the list of IDs under the key + "_id"


                        }
                        else
                        {
                            val = textBox.Text.ToString();
                        }
                        values[key] = val;
                    }

                    if (control is ComboBox comboBox)
                    {
                        string key = comboBox.Name.Replace("cmb_", "");
                        string val = "";

                        if (TagHasMarker(comboBox.Tag, "DYNAMIC"))
                        {
                            key = key + "_id";
                            values.Add(key, comboBox.SelectedValue);
                        }
                        else
                        {
                            val = comboBox.Text.ToString();
                            values.Add(key, val);
                        }
                    }



                    if (control is CheckBox checkbox)
                    {
                        string key = checkbox.Name.Replace("chk_", "");
                        string val = String.Format("{0}", checkbox.Checked ? 1 : 0);
                        values.Add(key, val);
                    }


                    if (control is DateTimePicker dateTimePicker)
                    {
                        string key = dateTimePicker.Name.Replace("dtp_", "");
                        string val = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dateTimePicker.Value);
                        values.Add(key, val);
                    }


                    if (control is NumericUpDown numericUpDown)
                    {
                        string key = numericUpDown.Name.Replace("txt_", "");
                        string val = String.Format("'{0}'", numericUpDown.Value);
                        values.Add(key, val);
                    }
                }
            }
            return values;
        }

        public static void BindControls2(Panel[] pnl_list, DataTable dt, int selectedIndex = 0)
        {
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();

            foreach (var col_name in dt.Columns)
            {
                foreach (var pnl in pnl_list)
                {
                    foreach (Control control in pnl.Controls)
                    {
                        if (control.Name.Contains(col_name.ToString()))
                        {
                            string column_name = col_name.ToString();
                            Console.WriteLine(column_name);

                            // Check if the control is a TextBox
                            if (control is TextBox textBox && textBox.Name.Replace("txt_", "") == column_name)
                            {
                                string key = textBox.Name.Replace("txt_", "");
                                object rawValue = dt.Rows[selectedIndex][column_name];

                                // MONEY FORMAT
                                if (textBox.Tag?.ToString().Contains("MONEY") == true)
                                {
                                    if (decimal.TryParse(rawValue.ToString(), out decimal moneyVal))
                                    {
                                        textBox.Text = moneyVal.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-PH"));
                                        textBox.AccessibleDescription = moneyVal.ToString(); // Store precise value
                                    }
                                    else
                                    {
                                        textBox.Text = "₱0.00";
                                        textBox.AccessibleDescription = "0";
                                    }
                                }

                                // DOCUMENT FORMAT
                                else if (textBox.Tag?.ToString().StartsWith("DOCUMENT") == true)
                                {
                                    string tag = textBox.Tag.ToString();   // e.g. "DOCUMENTAV REQUIRED"

                                    // Remove "DOCUMENT" and split by space, take first part
                                    string prefix = tag.Substring("DOCUMENT".Length).Split(' ')[0]; // "AV"

                                    if (int.TryParse(rawValue?.ToString(), out int docNumber))
                                    {
                                        textBox.Text = prefix + docNumber.ToString("D8");
                                        textBox.AccessibleDescription = docNumber.ToString(); // Store real value
                                    }
                                    else
                                    {
                                        textBox.Text = prefix + "00000000";
                                        textBox.AccessibleDescription = "0"; // fallback real value
                                    }
                                }

                                // MULTI TAG
                                else if (textBox.Tag is List<int> ids && ids.Count > 0)
                                {
                                    textBox.Text = string.Join(", ", ids);
                                }

                                // DEFAULT
                                else
                                {
                                    if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                    {
                                        Console.WriteLine("IndexOutOfRangeException selectedIndex");
                                        return;
                                    }

                                    textBox.Text = rawValue?.ToString() ?? "";
                                }
                            }

                            // Check if the control is a Combobox
                            if (control is ComboBox comboBox)
                            {
                                Console.WriteLine($"This is a combobox: {comboBox.Name}");
                                string key = comboBox.Name.Replace("cmb_", "") + "_id";
                                comboBox.BackColor = Color.FromArgb(235, 235, 235);

                                if (TagHasMarker(comboBox.Tag, "DYNAMIC"))
                                {
                                    Console.WriteLine("DYNAMICS:", comboBox.Name);
                                    string rawVal = dt.Rows[selectedIndex][key].ToString();

                                    if (comboBox.DataSource != null)
                                    {
                                        // Items are loaded, bind normally
                                        comboBox.SelectedValue = rawVal;
                                    }
                                    else
                                    {
                                        // Items not loaded yet (view mode) — store for deferred binding
                                        comboBox.AccessibleDescription = rawVal;
                                        comboBox.Text = dt.Rows[selectedIndex][column_name].ToString();
                                    }
                                }
                                else if (comboBox.Tag == "MULTIVALUE")
                                {
                                    string rawValue = dt.Rows[selectedIndex][column_name].ToString();
                                    var multiValues = rawValue.Split(',')
                                        .Select(v => v.Trim())
                                        .Where(v => !string.IsNullOrEmpty(v))
                                        .ToList();

                                    comboBox.Text = multiValues.FirstOrDefault() ?? string.Empty;

                                    foreach (var val in multiValues)
                                        comboBox.Items.Add(val);

                                    if (multiValues.Count > 0)
                                        comboBox.SelectedIndex = 0;
                                }
                                else
                                {
                                    // View mode — no items loaded, just display the text value
                                    string displayValue = dt.Rows[selectedIndex][column_name].ToString();

                                    if (comboBox.Items.Count > 0)
                                    {
                                        // Try to select matching item first
                                        int matchIndex = comboBox.FindStringExact(displayValue);
                                        if (matchIndex >= 0)
                                            comboBox.SelectedIndex = matchIndex;
                                        else
                                            comboBox.Text = displayValue;
                                    }
                                    else
                                    {
                                        // No items — force text display and store raw value
                                        comboBox.DropDownStyle = ComboBoxStyle.DropDown; // must be DropDown to allow free text
                                        comboBox.Text = displayValue;
                                        comboBox.AccessibleDescription = displayValue; // stash for later if needed
                                        comboBox.BackColor = Color.FromArgb(235, 235, 235);
                                    }
                                }
                            }

                            // Check if the control is a Checkbox
                            if (control is CheckBox checkbox)
                            {
                                //to hand outofbound rows
                                if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                {
                                    Console.WriteLine("IndexOutOfRangeException  ");
                                    return;
                                }
                                string key = checkbox.Name.Replace("chk_", "");
                                checkbox.Checked = (string)dt.Rows[selectedIndex][column_name].ToString() == "1" ||
                                (string)dt.Rows[selectedIndex][column_name].ToString().ToLower() == "true"
                                ? true : false;
                            }
                            // Check if the control is a DATETIME PICKER
                            if (control is DateTimePicker dateTimePicker)
                            {
                                if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                    return;

                                object rawValue = dt.Rows[selectedIndex][column_name];
                                string rawStr = rawValue?.ToString() ?? "";

                                if (rawValue != DBNull.Value && !string.IsNullOrWhiteSpace(rawStr))
                                {
                                    // Try exact format first, then fallback to general parse
                                    if (!DateTime.TryParseExact(rawStr, "MM/dd/yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out DateTime parsedDate))
                                    {
                                        DateTime.TryParse(rawStr, out parsedDate); // fallback
                                    }

                                    if (parsedDate != default)
                                    {
                                        dateTimePicker.Format = DateTimePickerFormat.Custom;
                                        dateTimePicker.CustomFormat = "MM/dd/yyyy";
                                        dateTimePicker.Value = parsedDate;             // ← Set Value AFTER setting Format
                                    }
                                }
                                else
                                {
                                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                                    dateTimePicker.CustomFormat = " ";
                                }
                            }
                            // Check if the control is a NUMERIC
                            if (control is NumericUpDown numericUpDown)
                            {
                                string key = numericUpDown.Name.Replace("txt_", "");
                                numericUpDown.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<string, dynamic> GetControlsValues(Panel pnl1, Panel pnl2)
        { 
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
            foreach (Control control in pnl1.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    string key = textBox.Name.Replace("txt_", "");
                    string val = "";

                    if (textBox.Tag == "MONEY")
                    {

                        val = String.Format("{0}", textBox.Text.ToString().Replace(",",""));
                    }
                    else
                    {
                        val = String.Format("'{0}'", textBox.Text.ToString());
                    }
                    values.Add(key, val);
                }

                // Check if the control is a Combobox
                if (control is ComboBox comboBox)
                {
                    string key = comboBox.Name.Replace("cmb_", "");
                    string val = "";
                    if (string.IsNullOrEmpty(comboBox.Text))
                    {
                        val = "";
                    }
                    else
                    {
                        val = String.Format("'{0}'", comboBox.Text.ToString());
                    }
                    values.Add(key, val);
                }

                // Check if the control is a Checkbox
                if (control is CheckBox checkbox)
                {
                    string key = checkbox.Name.Replace("chk_", "");
                    string val = String.Format("{0}", checkbox.Checked ? 1 : 0);
                    values.Add(key, val);
                }

                // Check if the control is a DATETIME PICKER
                if (control is DateTimePicker dateTimePicker)
                {
                    string key = dateTimePicker.Name.Replace("dtp_", "");

                    string val = String.Format("'{0:yyyy-MM-dd}'", dateTimePicker.Value);

                    //string val = String.Format("'{0}'", dateTimePicker.Value);
                    values.Add(key, val);
                }

                // Check if the control is a NUMERIC
                if (control is NumericUpDown numericUpDown)
                {
                    string key = numericUpDown.Name.Replace("txt_", "");
                    string val = String.Format("'{0}'", numericUpDown.Value);
                    values.Add(key, val);
                }
            }
            foreach (Control control in pnl2.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    string key = textBox.Name.Replace("txt_", "");
                    string val = "";

                    if (textBox.Tag == "MONEY")
                    {

                        val = String.Format("'{0}'", textBox.Text.ToString().Replace(",", ""));
                    }
                    else
                    {
                        val = String.Format("'{0}'", textBox.Text.ToString().Replace(",", ""));
                    }
                    values.Add(key, val);
                }

                // Check if the control is a Combobox
                if (control is ComboBox comboBox)
                {
                    string key = comboBox.Name.Replace("cmb_", "");
                    string val = "";
                    if (string.IsNullOrEmpty(comboBox.Text))
                    {
                        val = "";
                    }
                    else
                    {
                        val = String.Format("'{0}'", comboBox.Text.ToString());
                    }
                    values.Add(key, val);
                }

                // Check if the control is a Checkbox
                if (control is CheckBox checkbox)
                {
                    string key = checkbox.Name.Replace("chk_", "");
                    string val = String.Format("{0}", checkbox.Checked ? 1 : 0);
                    values.Add(key, val);
                }

                // Check if the control is a DATETIME PICKER
                if (control is DateTimePicker dateTimePicker)
                {
                    string key = dateTimePicker.Name.Replace("dtp_", "");
                    string val = String.Format("'{0}'", dateTimePicker.Value);
                    values.Add(key, val);
                }

                // Check if the control is a NUMERIC
                if (control is NumericUpDown numericUpDown)
                {
                    string key = numericUpDown.Name.Replace("num_", "");
                    string val = String.Format("{0}", numericUpDown.Value);
                    values.Add(key, val);
                }

            }

            return values;
        }

        internal static void ResetControls(DataGridView dg_bom)
        {
            throw new NotImplementedException();
        }

        public static bool ValidateControlsValues(Panel pnl)
        {
            bool isError = false;

            foreach (Control control in pnl.Controls)
            {
                // Handle TextBox
                if (control is TextBox textBox)
                {
                    if (string.Equals(textBox.Tag as string, "REQUIRED", StringComparison.OrdinalIgnoreCase)
                        && string.IsNullOrEmpty(textBox.Text))
                    {
                        FlashRed(control);
                        isError = true;

                        // Log the control name
                        Console.WriteLine($"Validation error: TextBox '{textBox.Name}' is required.");
                    }
                    else
                    {
                        control.BackColor = Color.White;
                    }
                }

                // Handle ComboBox
                else if (control is ComboBox comboBox)
                {
                    if (string.Equals(comboBox.Tag as string, "REQUIRED", StringComparison.OrdinalIgnoreCase)
                        && string.IsNullOrWhiteSpace(comboBox.Text)) 
                    {
                        FlashRed(comboBox);
                        isError = true;

                        // Log the control name
                        Console.WriteLine($"Validation error: ComboBox '{comboBox.Name}' is required.");
                    }
                    else
                    {
                        comboBox.BackColor = Color.White;
                    }
                }
            }

            return isError;
        }

        public static bool ValidateControlsValues2(Panel pnl)
        {
            bool isError = false;

            foreach (Control control in pnl.Controls)
            {
                string tag = control.Tag as string;
                if (string.IsNullOrEmpty(tag))
                    continue;

                bool isRequired = tag.IndexOf("REQUIRED", StringComparison.OrdinalIgnoreCase) >= 0;
                bool isMoney = tag.IndexOf("MONEY", StringComparison.OrdinalIgnoreCase) >= 0;

                if (control is TextBox textBox)
                {
                    string value = textBox.Text.Trim();

                    // REQUIRED validation
                    if (isRequired && string.IsNullOrEmpty(value))
                    {
                        FlashRed(textBox);
                        isError = true;
                        continue;
                    }

                    // MONEY validation
                    if (isMoney && !string.IsNullOrEmpty(value))
                    {
                        if (!decimal.TryParse(
                                value,
                                NumberStyles.Currency,
                                CultureInfo.GetCultureInfo("en-PH"),
                                out decimal moneyValue)
                            || moneyValue < 0)
                        {
                            FlashRed(textBox);
                            isError = true;
                            continue;
                        }
                    }

                    textBox.BackColor = Color.FromArgb(235, 235, 235);
                }
                else if (control is ComboBox comboBox)
                {
                    if (isRequired && comboBox.SelectedIndex < 0)
                    {
                        FlashRed(comboBox);
                        isError = true;
                    }
                    else
                    {
                        comboBox.BackColor = Color.FromArgb(235, 235, 235);
                    }
                }
                else if (control is DateTimePicker dtp)
                {
                    if (isRequired)
                    {
                        if (dtp.Value == dtp.MinDate || dtp.Value == default(DateTime))
                        {
                            FlashRed(dtp);
                            isError = true;
                        }
                        else
                        {
                            dtp.CalendarMonthBackground = Color.FromArgb(235, 235, 235);
                            dtp.BackColor = Color.FromArgb(235, 235, 235);
                        }
                    }
                }
            }

            return isError;
        }

        public static bool ValidateControlsValues2(Panel[] panels, string[] excludeNames = null)
        {
            bool isError = false;

            foreach (var pnl in panels)
            {
                foreach (Control control in pnl.Controls)
                {
                    // Skip excluded controls
                    if (excludeNames != null && excludeNames.Contains(control.Name))
                        continue;

                    string tag = control.Tag as string;
                    if (string.IsNullOrEmpty(tag))
                        continue;

                    bool isRequired = tag.IndexOf("REQUIRED", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool isMoney = tag.IndexOf("MONEY", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (control is TextBox textBox)
                    {
                        string value = textBox.Text.Trim();

                        if (isRequired && string.IsNullOrEmpty(value))
                        {
                            FlashRed(textBox);
                            isError = true;
                            continue;
                        }

                        if (isMoney && !string.IsNullOrEmpty(value))
                        {
                            decimal moneyValue;

                            if (!string.IsNullOrWhiteSpace(textBox.AccessibleDescription) &&
                                decimal.TryParse(textBox.AccessibleDescription, out moneyValue))
                            {
                                // valid
                            }
                            else if (!decimal.TryParse(
                                        value,
                                        NumberStyles.Currency,
                                        CultureInfo.GetCultureInfo("en-PH"),
                                        out moneyValue))
                            {
                                FlashRed(textBox);
                                isError = true;
                                continue;
                            }

                            if (moneyValue < 0)
                            {
                                FlashRed(textBox);
                                isError = true;
                                continue;
                            }
                        }

                        textBox.BackColor = Color.FromArgb(235, 235, 235);
                    }
                    else if (control is ComboBox comboBox)
                    {
                        if (isRequired && comboBox.SelectedIndex < 0)
                        {
                            FlashRed(comboBox);
                            isError = true;
                        }
                        else
                        {
                            comboBox.BackColor = Color.FromArgb(235, 235, 235);
                        }
                    }
                    else if (control is DateTimePicker dtp)
                    {
                        if (isRequired)
                        {
                            if (dtp.Value == dtp.MinDate || dtp.Value == default(DateTime))
                            {
                                FlashRed(dtp);
                                isError = true;
                            }
                            else
                            {
                                dtp.CalendarMonthBackground = Color.FromArgb(235, 235, 235);
                                dtp.BackColor = Color.FromArgb(235, 235, 235);
                            }
                        }
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

        public static void BindControls(Panel[] pnl_list, DataTable dt, int selectedIndex = 0)
        {
            // Guard at the top — covers all controls below
            if (dt == null || dt.Rows.Count == 0 || selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
            {
                Console.WriteLine("BindControls: No rows to bind.");
                return;
            }

            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();

            foreach (var col_name in dt.Columns)
            {
                foreach (var pnl in pnl_list)
                {
                    foreach (Control control in pnl.Controls)
                    {
                        if (control.Name.Contains(col_name.ToString()))
                        {
                            string column_name = col_name.ToString();
                            Console.WriteLine(column_name);

                            // Check if the control is a TextBox 
                            if (control is TextBox textBox && textBox.Name.Replace("txt_", "") == column_name)
                            {
                                string key = textBox.Name.Replace("txt_", "");
                                object rawValue = dt.Rows[selectedIndex][column_name];

                                if (textBox.Tag?.ToString() == "MONEY")
                                {
                                    if (decimal.TryParse(rawValue.ToString(), out decimal moneyVal))
                                    {
                                        textBox.Text = moneyVal.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-PH"));
                                        textBox.AccessibleDescription = moneyVal.ToString(); // Store full precise value
                                    }
                                    else
                                    {
                                        textBox.Text = "₱0.00";
                                        textBox.AccessibleDescription = "0";
                                    }
                                }
                                else if (textBox.Tag is List<int> ids && ids.Count > 0)
                                {
                                    // If you're still handling MULTI-tagged list items here (you may want to adjust this based on how you store MULTI values)
                                    textBox.Text = string.Join(", ", ids);
                                }
                                else
                                {
                                    //to hand outofbound rows
                                    if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                    {
                                        Console.WriteLine("IndexOutOfRangeException selectedIndex");
                                        return;
                                    }
                                    textBox.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
                                    //textBox.Text = rawValue?.ToString() ?? "";

                                }
                            }

                            // Check if the control is a Combobox
                            if (control is ComboBox comboBox)
                            {
                                Console.WriteLine($"This is a  combobox: {comboBox.Name} ");
                                string key = comboBox.Name.Replace("cmb_", "") + "_id";

                                if (TagHasMarker(comboBox.Tag, "DYNAMIC"))
                                {
                                    Console.WriteLine("DYNAMICS:", comboBox.Name);
                                    comboBox.SelectedValue = (string)dt.Rows[selectedIndex][key].ToString();
                                }
                                // Check multiple values
                                else if (comboBox.Tag == "MULTIVALUE")
                                {
                                    string rawValue = dt.Rows[selectedIndex][column_name].ToString();
                                    var multiValues = rawValue.Split(',')
                                                         .Select(v => v.Trim())
                                                         .Where(v => !string.IsNullOrEmpty(v))
                                                         .ToList();

                                    // Set the first value as the display text (optional behavior)
                                    comboBox.Text = multiValues.FirstOrDefault() ?? string.Empty;

                                    // Populate the ComboBox with all values
                                    //comboBox.Items.Clear();
                                    foreach (var val in multiValues)
                                    {
                                        comboBox.Items.Add(val);
                                    }

                                    // Optionally set the first item as selected (you could change this logic)
                                    if (multiValues.Count > 0)
                                    {
                                        comboBox.SelectedIndex = 0;  // Select the first item (if needed)
                                    }
                                }
                                else
                                {
                                    string keys = comboBox.Name.Replace("cmb_", "");
                                    comboBox.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
                                }

                            }
                            // Check if the control is a Checkbox
                            if (control is CheckBox checkbox)
                            {
                                //to hand outofbound rows
                                if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                {
                                    Console.WriteLine("IndexOutOfRangeException  ");
                                    return;
                                }
                                string key = checkbox.Name.Replace("chk_", "");
                                checkbox.Checked = (string)dt.Rows[selectedIndex][column_name].ToString() == "1" ||
                                    (string)dt.Rows[selectedIndex][column_name].ToString().ToLower() == "true"
                                    ? true : false;
                            }

                            // Check if the control is a DATETIME PICKER
                            if (control is DateTimePicker dateTimePicker)
                            {
                                if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                    return;

                                object rawValue = dt.Rows[selectedIndex][column_name];

                                if (rawValue != DBNull.Value &&
                                    DateTime.TryParse(rawValue.ToString(), out DateTime parsedDate))
                                {
                                    dateTimePicker.Value = parsedDate;
                                }
                                else
                                {
                                    dateTimePicker.Value = DateTime.Now; // or MinDate if you prefer
                                }
                            }
                            // Check if the control is a NUMERIC
                            if (control is NumericUpDown numericUpDown)
                            {
                                string key = numericUpDown.Name.Replace("txt_", "");
                                numericUpDown.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
                            }
                            // Check if the control is a RichTextBox
                            if (control is RichTextBox richTextBox && richTextBox.Name.Replace("rtxt_", "") == column_name)
                            {
                                object rawValue = dt.Rows[selectedIndex][column_name];
                                richTextBox.Text = rawValue != DBNull.Value ? rawValue.ToString() : string.Empty;
                            }
                        }
                    }
                }
            }
        }

        public static string GetLocalIPAddress()
        {
            string localIP = string.Empty;

            // Get the host name
            string hostName = Dns.GetHostName();

            // Get the list of IP addresses associated with the host
            foreach (var ip in Dns.GetHostAddresses(hostName))
            {
                // Check if it's an IPv4 address
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break; // Exit the loop after Getting the first IPv4 address
                }
            }

            return localIP;
        }
        public static string GetSerialNumber()
        {
            try
            {
                string serialNumber = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

                foreach (ManagementObject mo in searcher.Get())
                {
                    serialNumber = mo["SerialNumber"].ToString();
                    break; // Assuming only one motherboard
                }
                return serialNumber;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error: " + ex.Message);
                return "";
            }
        }
        public static void ShowDialogMessage(string status,string message="")
        {
            switch (status)
            {
                case "success":
                    MessageBox.Show(message, "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "error":
                    MessageBox.Show(message, "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case "warning":
                    MessageBox.Show(message, "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    // Handle unexpected status values
                    MessageBox.Show("Unknown status: " + status, "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        public static void CopyFileTo(string filePath,string destinationPath)
        {
            try
            {
                File.Copy(filePath, destinationPath, true);
            }
            catch (Exception)
            { 
                throw;
            }
        }
        
        public static string MoneyFormat(double money)
        {
            return String.Format("{0:N2}", money);
        }

        // format to peso
        public static string FormatAsCurrency(TextBox textbox, decimal value)
        {
            // Format and assign
            textbox.Text = value.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-PH"));
            textbox.Tag = "MONEY";
            textbox.AccessibleDescription = value.ToString();

            return textbox.Text;
        }


        // trims the peso sign
        public static string GetCleanedPriceValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "0";
            // Remove currency symbols and thousands separators
            var cleaned = input.Replace("₱", "")
                               .Replace("$", "")
                               .Replace(",", "")
                               .Trim();

            return cleaned;
        }

        // Converts the data types to string so it can be easily editable
        public static DataTable ConvertDataTableToStringTable(DataTable originalTable)
        {
            DataTable stringTable = new DataTable();


            foreach (DataColumn col in originalTable.Columns)
            {
                stringTable.Columns.Add(col.ColumnName, typeof(string));
            }

            // Copy rows as strings
            foreach (DataRow row in originalTable.Rows)
            {
                var newRow = stringTable.NewRow();
                foreach (DataColumn col in originalTable.Columns)
                {
                    newRow[col.ColumnName] = row[col]?.ToString();
                }
                stringTable.Rows.Add(newRow);
            }

            return stringTable;
        }
        public static void GetModalData(TextBox textBox, DataView dataView)
        {
            int recordIndex = 0;
            textBox.Text = "";
            List<int> ids = new List<int>();

            foreach (DataRowView rowView in dataView)
            {
                // Always collect IDs regardless of Tag state
                if (int.TryParse(rowView["id"]?.ToString(), out int id))
                {
                    ids.Add(id);
                }

                textBox.Text += recordIndex == 0
                    ? rowView["name"].ToString()
                    : ", " + rowView["name"].ToString();

                recordIndex++;
            }

            textBox.Tag = ids; // Always store as List<int>
        }
        public static DataTable FilterDataTable(DataTable dataTable, string searchTerm, params string[] columnsToSearch)
        {
            if (dataTable == null || columnsToSearch == null || columnsToSearch.Length == 0)
            {
                return dataTable;
            }

            searchTerm = searchTerm?.ToLower() ?? string.Empty;

            var filteredRows = dataTable.AsEnumerable().Where(row =>
                columnsToSearch.Any(column =>
                    row[column]?.ToString().ToLower().Contains(searchTerm) == true));

            return filteredRows.Any() ? filteredRows.CopyToDataTable() : dataTable.Clone();
        }
        public static void SetInputsReadOnlyState(Panel[] panels, bool isReadOnly)
        {
            void SetStateRecursive(Control container)
            {
                foreach (Control ctrl in container.Controls)
                {
                    switch (ctrl)
                    {
                        case TextBox textBox:
                            textBox.ReadOnly = isReadOnly;
                            break;
                        case ComboBox comboBox:
                            comboBox.Enabled = !isReadOnly;
                            break;
                        case DateTimePicker dateTimePicker:
                            dateTimePicker.Enabled = !isReadOnly;
                            break;
                        //case CheckBox checkBox:
                        //    checkBox.Enabled = !isReadOnly;
                            break;
                        case NumericUpDown numericUpDown:
                            numericUpDown.Enabled = !isReadOnly;
                            break;
                    }

                    if (ctrl.HasChildren)
                    {
                        SetStateRecursive(ctrl);
                    }
                }
            }

            foreach (Panel pnl in panels)
            {
                SetStateRecursive(pnl);
            }
        }
        public static void SetControlsEditable(List<Control> controls)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case TextBox textBox:
                        textBox.ReadOnly = false;
                        break;
                    case ComboBox comboBox:
                        comboBox.Enabled = true;
                        break;
                    case CheckBox checkBox:
                        checkBox.Enabled = true;
                        break;
                    case DateTimePicker dateTimePicker:
                        dateTimePicker.Enabled = true;
                        break;
                    case NumericUpDown numericUpDown:
                        numericUpDown.Enabled = true;
                        break;
                        // Add other control types as needed
                }
            }
        }

        public static void GetBPIModalData(TextBox textBox, DataView dataView, int columnIndex)
        {
            if (dataView != null && dataView.Count > 0)
            {
                textBox.Text = dataView[0][columnIndex].ToString();
            }
        }
        public static void SetRowNumber(DataGridView grid, DataGridViewRowPostPaintEventArgs e, int columnIndex = 0)
        {
            if (grid != null && e.RowIndex >= 0 && columnIndex >= 0 && columnIndex < grid.ColumnCount)
            {
                grid.Rows[e.RowIndex].Cells[columnIndex].Value = (e.RowIndex + 1).ToString();
            }
        }
        public static void ClearDataGridView(DataGridView grid)
        {
            if (grid != null && grid.Rows.Count > 0)
            {
                grid.Rows.Clear();
            }
        }
        internal static DataTable SafeTable<T>(List<T> list)
        {
            var typeName = typeof(T).Name;

            if (list == null)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[SafeTable] {typeName} list is NULL"
                );

                return JsonHelper.ToDataTable(new List<T>());
            }

            if (list.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[SafeTable] {typeName} list is EMPTY"
                );
            }

            return JsonHelper.ToDataTable(list);
        }
        public static void ConvertColumnToInt(DataTable dt, string columnName)
        {
            if (!dt.Columns.Contains(columnName)) return;

            DataColumn oldCol = dt.Columns[columnName];
            int ordinal = oldCol.Ordinal;

            DataColumn newCol = new DataColumn(columnName + "_temp", typeof(int));
            dt.Columns.Add(newCol);

            foreach (DataRow row in dt.Rows)
            {
                row[newCol] = int.TryParse(row[oldCol]?.ToString(), out int val) ? val : 0;
            }

            dt.Columns.Remove(oldCol);
            newCol.ColumnName = columnName;
            newCol.SetOrdinal(ordinal);
        }
        public static void AddCmbDefaultVal(DataTable dt)
        {
            if (dt == null) return;

            DataRow newRow = dt.NewRow();
            newRow["id"] = DBNull.Value;
            newRow["name"] = "-- SELECT --";

            dt.Rows.InsertAt(newRow, 0);
        }
        public static void BindCmbValues(ComboBox cmb, DataView dv)
        {
            cmb.DataSource = dv;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            cmb.SelectedIndex = 0;
        }
        public static void BindCmbValues(ComboBox cmb, DataTable dt)
        {
            cmb.DataSource = dt;
            cmb.ValueMember = "id";
            cmb.DisplayMember = "name";
            cmb.SelectedIndex = 0;
        }
        public static void SetTabControlReadOnly(TabControl tabControl, bool isReadOnly)
        {
            foreach (TabPage tab in tabControl.TabPages)
                foreach (Control control in tab.Controls)
                    SetControlsReadOnly(control, isReadOnly);
        }

        private static void SetControlsReadOnly(Control parent, bool isReadOnly)
        {
            foreach (Control ctrl in parent.Controls)
            {
                switch (ctrl)
                {
                    case BpiBranchUC uc:
                        uc.SetReadOnly(isReadOnly);
                        continue; // UC handles its own internals, skip recursion
                    case TextBox tb:
                        tb.ReadOnly = isReadOnly;
                        break;
                    case ComboBox cmb:
                        cmb.Enabled = !isReadOnly;
                        break;
                    case CheckBox chk:
                        chk.Enabled = !isReadOnly;
                        break;
                    case DataGridView dgv:
                        dgv.ReadOnly = isReadOnly;
                        dgv.AllowUserToAddRows = !isReadOnly;
                        dgv.AllowUserToDeleteRows = !isReadOnly;
                        break;
                    case Button btn:
                        btn.Enabled = !isReadOnly;
                        break;
                }

                if (ctrl.HasChildren)
                    SetControlsReadOnly(ctrl, isReadOnly);
            }
        }
        public static void DiagnoseReadOnly(Control parent, int depth = 0)
        {
            Console.WriteLine("Diagnosing Readonly");
            string indent = new string('-', depth * 2);
            foreach (Control ctrl in parent.Controls)
            {
                switch (ctrl)
                {
                    case TextBox tb:
                        Console.WriteLine($"{indent}TextBox [{tb.Name}] ReadOnly={tb.ReadOnly}");
                        break;
                    case ComboBox cmb:
                        Console.WriteLine($"{indent}ComboBox [{cmb.Name}] Enabled={cmb.Enabled}");
                        break;
                    case DataGridView dgv:
                        Console.WriteLine($"{indent}DataGridView [{dgv.Name}] ReadOnly={dgv.ReadOnly}");
                        break;
                    case Button btn:
                        Console.WriteLine($"{indent}Button [{btn.Name}] Enabled={btn.Enabled}");
                        break;
                    case CheckBox chk:
                        Console.WriteLine($"{indent}CheckBox [{chk.Name}] Enabled={chk.Enabled}");
                        break;
                }

                if (ctrl.HasChildren)
                    DiagnoseReadOnly(ctrl, depth + 1);
            }
        }

    }
} 
