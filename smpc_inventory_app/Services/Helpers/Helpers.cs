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

        public static class Loading
        {
            private static UserControl overlayPanel;

            public static void ShowLoading(Control parentControl, string message = "Loading, please wait...")
            {
                if (overlayPanel != null) return; // already showing

                overlayPanel = new UserControl
                {
                    BackColor = Color.FromArgb(180, Color.Gray), // semi-transparent overlay
                    Dock = DockStyle.Fill
                };

                Label lblMessage = new Label
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Text = message,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                overlayPanel.Controls.Add(lblMessage);

                // Add overlay inside the DataGridView's parent (so it sits on top of the grid)
                parentControl.Controls.Add(overlayPanel);
                overlayPanel.BringToFront();
            }

            /// <summary>
            /// Hide the loading overlay from the DataGridView
            /// </summary>
            public static void HideLoading(Control parentControl)
            {
                if (overlayPanel != null)
                {
                    parentControl.Controls.Remove(overlayPanel);
                    overlayPanel.Dispose();
                    overlayPanel = null;
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

                        if (status.HasValue)
                        {
                            col.ReadOnly = status.Value;
                        }
                        else
                        {
                            col.ReadOnly = !col.ReadOnly;
                        }
                        Console.WriteLine(" " + dgv.Name + "." + col.Name +
                            ".ReadOnly = " + col.ReadOnly);
                    }
                }
            }
            pnl.Enabled = true;
            Console.WriteLine("]\n");
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

                    if (comboBox.Tag == "DYNAMIC")
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

                        if (comboBox.Tag == "DYNAMIC")
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

                                if (comboBox.Tag == "DYNAMIC")
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

                                if (comboBox.Tag == "DYNAMIC")
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
