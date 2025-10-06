using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    public partial class SetupItemModal : Form
    {

        private string Title { get; }
        private string EndPoint { get; }
        private List<int> CurrentValues { get; }
        private List<string> CurrentGridValues { get; }

        private int result { get; set; }
        //private DataView result { get; set; }
        private DataTable Dt { get; set; }


        public SetupItemModal(string title, string api, DataTable dt, List<int> currentValues, List<string> currentGridValues, int recordIndex = 0)
        {
            InitializeComponent();


            lbl_title.Text = title;
            this.Text = title;


            this.EndPoint = api;


            this.CurrentValues = currentValues;
            this.CurrentGridValues = (currentGridValues != null && recordIndex >= 0 && recordIndex < currentGridValues.Count && !string.IsNullOrEmpty(currentGridValues[recordIndex]))
                   ? new List<string>(currentGridValues[recordIndex].Split(','))
                   : new List<string>();
            this.Dt = dt;
        }

        private void SetupItemModal_Load(object sender, EventArgs e)
        {
            dg_item_bom.DataSource = this.Dt;
        




            foreach (DataGridViewColumn column in dg_item_bom.Columns)
            {
                if (column.Name != "item_code" && column.Name != "general_name")
                {
                    column.Visible = false;
                }
            }


        }

        public int GetResult()
        {
            return result;
        }
        private void dg_item_bom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.result = e.RowIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }
    }
}
