using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    public partial class ItemDetailsModal : Form
    {
        private DataRow selectedRow;
        DataTable selectedItem;
        public ItemDetailsModal(DataRow row)
        {
            InitializeComponent();
            this.selectedRow = row;
        }

        private DataTable ConvertDataRowToDataTable(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            DataTable table = row.Table.Clone();
            table.ImportRow(row);

            return table;

        }
        private void ItemDetailsModal_Load(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                selectedItem = ConvertDataRowToDataTable(selectedRow);

                if (selectedItem.Columns.Contains("sales_order_nos") &&
                    selectedItem.Columns.Contains("unit_prices") &&
                    selectedItem.Columns.Contains("qtys") &&
                    selectedItem.Columns.Contains("unit_of_measures") &&
                    selectedItem.Rows.Count > 0)
                {
                    string salesOrderNos = selectedItem.Rows[0]["sales_order_nos"].ToString();
                    string unitPrices = selectedItem.Rows[0]["unit_prices"].ToString();
                    string qtys = selectedItem.Rows[0]["qtys"].ToString();
                    string unitOfMeasures = selectedItem.Rows[0]["unit_of_measures"].ToString();

                    string[] docs = salesOrderNos.Split(',');
                    string[] unitprices = unitPrices.Split(',');
                    string[] quantities = qtys.Split(',');
                    string[] units = unitOfMeasures.Split(',');

                    int maxLength = new int[] {docs.Length, unitprices.Length, quantities.Length, units.Length }.Min();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("sales_order_no", typeof(string));
                    dt.Columns.Add("unit_price", typeof(string));
                    dt.Columns.Add("qty", typeof(string));
                    dt.Columns.Add("unit_of_measure", typeof(string));

                    for (int i = 0; i < maxLength; i++)
                    {

                        dt.Rows.Add(docs[i].Trim(), unitprices[i].Trim(), quantities[i].Trim() , units[i].Trim());
                    }
                    
                    dataGridView1.DataSource = dt;
                }
            }
        }



    }
}
