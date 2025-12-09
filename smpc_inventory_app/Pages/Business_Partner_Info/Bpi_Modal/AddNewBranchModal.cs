using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Business_Partner_Info.Bpi_Modal
{
    public partial class AddNewBranchModal : Form
    {
        public string title { get; set; }
        public AddNewBranchModal()
        {
            InitializeComponent();
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            title = txt_title.Text;

            // check branch name if exist


            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public string GetTitle()
        {
            return title;
        }
        public void SetTitle(string value)
        {
            txt_title.Text = value;
        }

    }
}
