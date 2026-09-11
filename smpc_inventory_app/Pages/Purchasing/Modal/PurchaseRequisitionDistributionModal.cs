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

            // Hard block, not a dismissable warning: §11.4/§14.13 - "Sigma QTY TO GIVE
            // MUST equal the order qty - no more, no less." Over-allocation is already
            // hard-blocked per-keystroke in the card's own CellValidating; this closes
            // the other half, which previously let a user click OK past an
            // under-allocated total.
            if (hasUnallocated)
            {
                // Show every card again, so the one still short is on screen.
                textBox1.Clear();
                ApplySearch(string.Empty);

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

        // SEARCH narrows the cards to those whose item or orders contain the
        // text. Cards are hidden, never removed: DONE still reads every card, so
        // quantities typed into a card that is filtered out are kept, and the
        // every-unit-allocated check (spec 11.4) still covers it.
        private void btn_search_Click(object sender, EventArgs e)
        {
            ApplySearch(textBox1.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            ApplySearch(textBox1.Text);
        }

        private void ApplySearch(string term)
        {
            term = (term ?? string.Empty).Trim();
            flowLayoutPanel1.SuspendLayout();
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is OrderDistributionCard card)
                    card.Visible = card.Matches(term);
            }
            flowLayoutPanel1.ResumeLayout();
        }
    }
}
