using smpc_inventory_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Item
{
    public partial class ItemEntryComaparisonModal : Form
    {
        public Dictionary<string, object> Data { get; set; }
        DataTable items;
        public ItemEntryComaparisonModal()
        {
            InitializeComponent();
        }

        private void ItemEntryComaparisonModal_Load(object sender, EventArgs e)
        {
            items = ConvertDictionaryToDataTable(Data);
        }
        private DataTable ConvertDictionaryToDataTable(Dictionary<string, object> data)
        {
            // Create a DataTable to hold the data
            DataTable dt = new DataTable();

            // Add columns to the DataTable from the dictionary keys
            foreach (var key in data.Keys)
            {
                dt.Columns.Add(key);
            }

            // Create a new row and add the values from the dictionary
            DataRow row = dt.NewRow();
            foreach (var key in data.Keys)
            {
                row[key] = data[key];
            }

            // Add the row to the DataTable
            dt.Rows.Add(row);

            return dt;
        }
    }
}
