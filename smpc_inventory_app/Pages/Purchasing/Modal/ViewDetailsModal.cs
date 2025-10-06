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
    public partial class ViewDetailsModal : Form
    {
        private DataTable _detailsData;
        public ViewDetailsModal(DataTable detailsData)
        {
            InitializeComponent();
            _detailsData = detailsData;
        }

        public void SetData(DataTable detailsData, Dictionary<string, string> columnHeaders = null)
        {
            _detailsData = detailsData;
            dgv_details.DataSource = _detailsData;

            if (columnHeaders != null)
            {
                foreach (var column in columnHeaders)
                {
                    if (dgv_details.Columns.Contains(column.Key))
                    {
                        dgv_details.Columns[column.Key].HeaderText = column.Value;
                    }
                }
            }
            // hide ids
            if (dgv_details.Columns.Contains("order_detail_id"))
            {
                dgv_details.Columns["order_detail_id"].Visible = false;
            }
            

            dgv_details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ViewDetailsModal_Load(object sender, EventArgs e)
        {

        }
    }
}
