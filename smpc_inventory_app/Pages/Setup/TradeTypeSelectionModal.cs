using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class TradeTypeSelectionModal : Form
    {
        public DataView SelectedTradeTypes { get; private set; }

        public TradeTypeSelectionModal()
        {
            InitializeComponent();
        }

        private void TradeTypeSelectionModal_Load(object sender, EventArgs e)
        {
            DataTable tradeTypesTable = new DataTable();
            tradeTypesTable.Columns.Add("NAME", typeof(string));
            tradeTypesTable.Columns.Add("select", typeof(bool));
            List<string> tradeTypes = new List<string> { "TRADE", "NON-TRADE" };
            foreach (var tradeType in tradeTypes)
            {
                tradeTypesTable.Rows.Add(tradeType, false);
            }

            dg_trade_type.DataSource = tradeTypesTable;
        }

        private DataView GetTradeData()
        {
            DataView dataView = new DataView(dg_trade_type.DataSource as DataTable);
            dataView.RowFilter = $"select = true";

            return dataView;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SelectedTradeTypes = GetTradeData();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
