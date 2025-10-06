using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages
{
    public partial class SetupSelectionModal : Form
    {
        private string Title { get; }
        private string EndPoint { get;}
        private List<int> CurrentValues { get; }
        private List<string> CurrentGridValues { get; }


        private DataView result { get; set; }
        private DataTable Dt { get; set; }
        public SetupSelectionModal(string title, string api, DataTable dt, List<int> currentValues, List<string> currentGridValues, int recordIndex )
        {
            InitializeComponent();
       
            lbl_title.Text = title;
           
            this.EndPoint = api;


            this.CurrentValues = currentValues;
            this.CurrentGridValues = (currentGridValues != null && recordIndex >= 0 && recordIndex < currentGridValues.Count && !string.IsNullOrEmpty(currentGridValues[recordIndex]))
                   ? new List<string>(currentGridValues[recordIndex].Split(','))
                   : new List<string>();
            this.Dt = dt;
            if (dt != null)
            {
                if (dt.Columns["select"] != null)    // Check if select column already exist
                    return;
                dt.Columns.Add("select");            // Add select column if not 
            }

        }

        private void SelectionModal_Load(object sender, EventArgs e)

        {

            if (this.CurrentValues.Count != 0)
            {
                if (!this.Dt.Columns.Contains("select"))
                {
                    this.Dt.Columns.Add("select", typeof(bool));
                }
                foreach (DataRow row in this.Dt.Rows)
                {
                    int industryId = row.Field<int>("id"); // Replace with actual column name
                    row["select"] = this.CurrentValues.Contains(industryId);
                }
                dg_general.DataSource = this.Dt;

            }

            else if (this.CurrentGridValues.Count != 0) {

                this.Dt.AsEnumerable().ToList().ForEach(row => row["select"] = false);   // 

                if (!this.Dt.Columns.Contains("select"))
                {
                    this.Dt.Columns.Add("select", typeof(bool));
                }
                foreach (DataRow row in this.Dt.Rows)
                {
                    string code = row.Field<string>("code"); // Replace with actual column name
                    foreach (string value in this.CurrentGridValues)
                    {

                        if (value == code)
                        {
                            row["select"] = value.Contains(code);
                            break;
                        }
                      

                    }
                }
            }
          
                dg_general.DataSource = this.Dt;

           




        }

        private DataView GetEntityData()
        {

            DataView dataView = new DataView(dg_general.DataSource as DataTable);
            dataView.RowFilter = $"select = true";

            return dataView;
         
        }
        public DataView GetResult()
        {
           return this.result ;
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.result = GetEntityData();
        
            this.Close();
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
