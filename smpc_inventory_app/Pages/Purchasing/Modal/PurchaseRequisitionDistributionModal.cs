using smpc_inventory_app.Pages.Purchasing.PurchaseList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static smpc_inventory_app.Pages.Purchasing.PurchaseList.PurchaseRequisitionDisbtributionCard;

namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    public partial class PurchaseRequisitionDistributionModal : Form
    {
        public PurchaseRequisitionDistributionModal(List<OrderDistributionData2> itemsNeedingDistribution)
        {
            InitializeComponent();

            flowLayoutPanel1.Padding = new Padding(5);

            foreach (var item in itemsNeedingDistribution)
            {
                var card = new OrderDistributionCard();

                card.LoadData(
                    item.ItemId,
                    item.ItemDescription,
                    item.ItemBrand,
                    item.ReqQty,
                    item.OrderQty,
                    item.UnitOfMeasure,
                    item.OrderNos,
                    item.ProjectNames,
                    item.SalesExecutives,
                    item.CommitmentDates,
                    item.OrderDetailIds,
                    item.Qtys
                );
                flowLayoutPanel1.Controls.Add(card);
            }
        }
        public List<DistributionResult> DistributedResults { get; private set; } = new List<DistributionResult>();

        private void btn_done_Click(object sender, EventArgs e)
        {
            List<DistributionResult> allResults = new List<DistributionResult>();
            bool hasUnallocated = false;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is PurchaseRequisitionDisbtributionCard card)
                {
                    var results = card.GetDistributionResults();
                    allResults.AddRange(results);

                    if (results.Any(res => res.UnallocatedQty > 0))
                    {
                        hasUnallocated = true;
                    }
                }
            }

            if (hasUnallocated)
            {
                var confirm = MessageBox.Show(
                    "There are remaining unallocated items. Do you want to proceed?",
                    "Unallocated Quantity Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Cancel)
                    return;
            }

            DistributedResults = allResults;
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
