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

namespace smpc_app.Services.Helpers
{
    public static class Helpers
    {
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

                    //if(textBox.Tag != null && textBox.Tag is string newIds)
                    //{
                    //        values.Add(key , newIds);  // Add the list of IDs under the key + "_id"

                    //}

                    if (textBox.Tag != null && textBox.Tag is List<int> ids && ids.Count > 0)
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
                    //string val = String.Format("'{0}'", checkbox.Checked ? 1 : 0);
                    bool val = checkbox.Checked;
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
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
            foreach (Control control in pnl.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    string key = textBox.Name.Replace("txt_", "");
                    if (textBox.Tag == "REQUIRED" && textBox.Text == "")
                    {
                        control.BackColor = Color.Red;
                        isError = true; 
                    }
                    else
                    {
                        control.BackColor = Color.White;
                    }
                } 
            } 
            return isError;
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


                                if (textBox.Tag =="MULTI" && textBox.Tag is List<int> ids && ids.Count > 0)
                                {
                                    textBox.Text = Helpers.MoneyFormat(double.Parse(dt.Rows[selectedIndex][column_name].ToString()));
                                }
                                else
                                {

                                    textBox.Text = (string)dt.Rows[selectedIndex][column_name].ToString();
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

                                string key = checkbox.Name.Replace("chk_", "");
                                checkbox.Checked = (string)dt.Rows[selectedIndex][column_name].ToString() == "1" ? true : false;
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
        public static DataTable ConvertDataGridViewToDataTable(DataGridView dgv)
        {
            DataTable dataTable = new DataTable();

            // Add columns to DataTable
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                dataTable.Columns.Add(column.Name);
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
                        dataRow[i] = row.Cells[i].Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
        public static string MoneyFormat(double money)
        {
            return String.Format("{0:N2}", money);
        } 
        public static void GetModalData(TextBox textBox, DataView dataView)
        {
            int recordIndex = 0;
            textBox.Text = "";
            List<int> ids = new List<int>();

            foreach (DataRowView rowView in dataView)
            {
                
                if (textBox.Tag == "MULTI"){
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
