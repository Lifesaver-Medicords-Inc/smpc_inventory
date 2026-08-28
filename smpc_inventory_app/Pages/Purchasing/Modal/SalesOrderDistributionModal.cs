using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static smpc_inventory_app.Pages.Purchasing.OrderDistributionCard;

namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    public partial class SalesOrderDistributionModal : Form
    {

        public SalesOrderDistributionModal(List<OrderDistributionData> itemsNeedingDistribution)
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
                if (control is OrderDistributionCard card)
                {
                    var results = card.GetDistributionResults();
                    allResults.AddRange(results);

                    if (results.Any(res => res.UnallocatedQty > 0))
                    {
                        hasUnallocated = true;
                    }
                }
            }

            // Hard block, not a dismissable warning: §11.4/§14.13 - "Sigma QTY TO GIVE
            // MUST equal the order qty - no more, no less." Over-allocation is already
            // hard-blocked per-keystroke in OrderDistributionCard's CellValidating; this
            // closes the other half, which previously let a user click OK past an
            // under-allocated total.
            if (hasUnallocated)
            {
                MessageBox.Show(
                    "Every unit must be allocated before continuing - the total QTY TO GIVE must equal the ORDER QTY exactly (§11.4).",
                    "Allocation Incomplete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
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