using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Item;
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
    public partial class CanvassSheetModal : Form
    {
        private DataRow selectedRow;
        DataTable selectedItem;
        int selectedRecord = 0;

        public CanvassSheetModal(DataRow row)
        {
            InitializeComponent();
            this.selectedRow = row;
        }

        private void CanvassSheetModal_Load(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                selectedItem = ConvertDataRowToDataTable(selectedRow);
                Bind(true);
            } 
            else
            {
                txt_doc_no.Text = GenerateDocNo();
            }
        }
        private DataTable ConvertDataRowToDataTable(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            DataTable table = row.Table.Clone();
            table.ImportRow(row);

            return table;

        }
        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                // Bind UI panels
                Panel[] pnlItem = { pnl_parent };

                Helpers.BindControls(pnlItem, selectedItem, this.selectedRecord);
            }
        }
        private string GenerateDocNo()
        {
            return "DOC-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}
