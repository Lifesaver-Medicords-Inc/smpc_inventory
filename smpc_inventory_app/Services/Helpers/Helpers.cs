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

namespace smpc_app.Services.Helpers
{
    internal static class Helpers
    {
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
            var dataTable = new DataTable(typeof(T).Name);

            // Get all properties of T
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static void FreezeVisibleColumns(DataGridView dgv, int count)
        {
            if (dgv == null || dgv.Columns.Count == 0)
                return;

            // Reset all frozen states and dividers
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.Frozen = false;
                col.DividerWidth = 0;
            }

            int frozen = 0;
            DataGridViewColumn lastFrozen = null;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                {
                    col.Frozen = true;
                    lastFrozen = col;
                    frozen++;
                    if (frozen >= count)
                        break;
                }
            }

            //Add a visual gap between frozen and unfrozen columns
            if (lastFrozen != null)
            {
                lastFrozen.DividerWidth = 2; // try 2–5 pixels for clarity
            }
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
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dgv, new object[] { true });

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

                if (r1.IsEmpty || r2.IsEmpty) continue;

                Rectangle headerRect = new Rectangle(r1.X, r1.Y, r2.Right - r1.X, r1.Height / 2);

                using (Brush b = new SolidBrush(SystemColors.Control))
                    e.Graphics.FillRectangle(b, headerRect);

                e.Graphics.DrawRectangle(Pens.Gray, headerRect);

                TextRenderer.DrawText(e.Graphics, groupName,
                    dgv.ColumnHeadersDefaultCellStyle.Font,
                    headerRect, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private static void DrawGroupedHeaderCells(DataGridView dgv, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);

                Rectangle fullRect = e.CellBounds;

                // Bottom half for column text
                Rectangle textRect = fullRect;
                textRect.Y += textRect.Height / 2;
                textRect.Height /= 2;

                TextRenderer.DrawText(e.Graphics,
                    e.FormattedValue?.ToString() ?? "",
                    e.CellStyle.Font, textRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

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

        public static void ResetControls(Panel pnl)
        {
            foreach (Control control in pnl.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    // Reset the TextBox's text
                    textBox.Text = "";
                }
            }
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

                // Handle ComboBox
                //comboboxes should be dropdown list and flatsyle flat
                else if (control is ComboBox comboBox)
                {
                    if (string.Equals(comboBox.Tag as string, "REQUIRED", StringComparison.OrdinalIgnoreCase)
                        && comboBox.SelectedIndex < 0)   // ✅ correct check for DropDownList
                    {
                        FlashRed(comboBox);
                        isError = true;
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

        public static void BindControls(Panel[] pnl_list, DataTable dt, int selectedIndex = 0)
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
                                Console.WriteLine(comboBox.Name);
                                string key = comboBox.Name.Replace("cmb_", "") + "_id";

                                if (comboBox.Tag == "DYNAMIC")
                                {

                                    //Console.WriteLine(comboBox.Name);
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
                                    //Console.WriteLine(comboBox.Name);
                                    comboBox.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
                                }

                            }
                            // Check if the control is a Checkbox
                            if (control is CheckBox checkbox)
                            {

                                //to hand outofbound rows
                                if (selectedIndex < 0 || selectedIndex >= dt.Rows.Count)
                                {
                                    Console.WriteLine("IndexOutOfRangeException selectedIndex");
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
                                string key = dateTimePicker.Name.Replace("dtp_", "");
                                string val = String.Format("'{0}'", dateTimePicker.Value);
                                object valueFromDataTable = dt.Rows[selectedIndex][column_name];

                                if (valueFromDataTable != DBNull.Value && valueFromDataTable is DateTime dateTimeValue)
                                {
                                    dateTimePicker.Value = dateTimeValue;
                                }
                                else
                                {
                                    dateTimePicker.Value = DateTime.Now;
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
                
                if (textBox.Tag == "MULTI" || textBox.Tag is List<int>) {
                    int id = Convert.ToInt32(rowView["id"]);  
                    ids.Add(id);
                }
                textBox.Text += recordIndex == 0 ? rowView["name"].ToString() : ", " + rowView["name"].ToString();  
                recordIndex++;

            }
            textBox.Tag = ids;
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
    }
} 
