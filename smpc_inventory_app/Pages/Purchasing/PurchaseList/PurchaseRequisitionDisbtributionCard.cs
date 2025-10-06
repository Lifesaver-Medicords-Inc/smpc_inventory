using smpc_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing.PurchaseList
{
    public partial class PurchaseRequisitionDisbtributionCard : UserControl
    {
        public class DistributionResult
        {
            public int ItemId { get; set; }
            public string OrderDetailIds { get; set; }
            public string OrderDetailNos { get; set; }
            public string OrderQtys { get; set; }
            public string QtysToGive { get; set; }
            public int UnallocatedQty { get; set; }
        }
        public PurchaseRequisitionDisbtributionCard()
        {
            InitializeComponent();
        }
        private void OrderDistributionCard_Load(object sender, EventArgs e)
        {
            Helpers.SetInputsReadOnlyState(new[] { pnl_header }, true);
        }
        public void LoadData(
            int itemId,
            string itemDescription,
            string itemBrand,
            int reqQty,
            int orderQty,
            string unitOfMeasure,
            string orderNos,
            string projectNames,
            string salesExecutives,
            string commmitmentDates,
            string orderDetailIds,
            string qtys
            )
        {
            txt_item_id.Text = itemId.ToString();
            txt_item_description.Text = itemDescription;
            txt_brand.Text = itemBrand;
            txt_req_qty.Text = reqQty.ToString();
            txt_order_qty.Text = orderQty.ToString();
            txt_req_qty_uom.Text = unitOfMeasure;
            txt_order_qty_uom.Text = unitOfMeasure;

            string[] orderNoArray = orderNos.Split(',');
            string[] projectNameArray = projectNames.Split(',');
            string[] salesExecutiveArray = salesExecutives.Split(',');
            string[] commmitmentDateArray = commmitmentDates.Split(',');
            string[] orderDetailIdArray = orderDetailIds.Split(',');
            string[] qtyArray = qtys.Split(',');

            int rowCount = Math.Min(Math.Min(orderNoArray.Length, projectNameArray.Length),
                                    Math.Min(salesExecutiveArray.Length, qtyArray.Length));

            dgv_distribute.Rows.Clear();

            for (int i = 0; i < rowCount; i++)
            {
                int rowIndex = dgv_distribute.Rows.Add();
                dgv_distribute.Rows[rowIndex].Cells["order_detail_id"].Value = orderDetailIdArray[i].Trim();
                dgv_distribute.Rows[rowIndex].Cells["doc_no"].Value = orderNoArray[i].Trim();
                dgv_distribute.Rows[rowIndex].Cells["project_name"].Value = projectNameArray[i].Trim();
                dgv_distribute.Rows[rowIndex].Cells["requestor"].Value = salesExecutiveArray[i].Trim();
                dgv_distribute.Rows[rowIndex].Cells["commitment_date"].Value = commmitmentDateArray[i].Trim();
                dgv_distribute.Rows[rowIndex].Cells["qty_req"].Value = qtyArray[i].Trim();
            }
        }
        private void txt_order_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public List<DistributionResult> GetDistributionResults()
        {
            int itemId = int.Parse(txt_item_id.Text);
            int orderQty = int.Parse(txt_order_qty.Text);
            List<string> orderDetailIdsList = new List<string>();
            List<string> orderNoList = new List<string>();
            List<string> orderQtysList = new List<string>();
            List<string> qtysToGiveList = new List<string>();

            int totalQtyToGive = 0;

            foreach (DataGridViewRow row in dgv_distribute.Rows)
            {
                if (row.IsNewRow) continue;

                string orderDetailId = row.Cells["order_detail_id"].Value?.ToString();
                string orderNo = row.Cells["doc_no"].Value?.ToString();
                string reqQtyStr = row.Cells["qty_req"].Value?.ToString();
                string qtyToGiveStr = row.Cells["qty_to_give"].Value?.ToString();

                int qtyToGive = 0;
                int reqQty = 0;

                int.TryParse(reqQtyStr, out reqQty);
                int.TryParse(qtyToGiveStr, out qtyToGive);

                if (qtyToGive > 0)
                {
                    orderDetailIdsList.Add(orderDetailId);
                    orderNoList.Add(orderNo);
                    orderQtysList.Add(reqQty.ToString());
                    qtysToGiveList.Add(qtyToGive.ToString());
                    totalQtyToGive += qtyToGive;
                }
                else
                {
                    MessageBox.Show($"This row has not been allocated: {orderDetailId}");
                }
            }

            var result = new DistributionResult
            {
                ItemId = itemId,
                OrderDetailIds = string.Join(",", orderDetailIdsList),
                OrderDetailNos = string.Join(",", orderNoList),
                OrderQtys = string.Join(",", orderQtysList),
                QtysToGive = string.Join(",", qtysToGiveList),
                UnallocatedQty = orderQty - totalQtyToGive
            };

            return new List<DistributionResult> { result };
        }


        private void dgv_distribute_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int qtyToGiveColumnIndex = 6;
            int qtyReqColumnIndex = 5;

            if (e.ColumnIndex == qtyToGiveColumnIndex)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int newValue))
                {
                    MessageBox.Show("Please enter a valid number.");
                    e.Cancel = true;
                    return;
                }

                // 1. Check if QTY TO GIVE exceeds QTY REQ in the same row
                var qtyReqCell = dgv_distribute.Rows[e.RowIndex].Cells[qtyReqColumnIndex].Value;
                if (qtyReqCell != null && int.TryParse(qtyReqCell.ToString(), out int qtyReq))
                {
                    if (newValue > qtyReq)
                    {
                        MessageBox.Show("QTY TO GIVE cannot exceed QTY REQ for this order.");

                        e.Cancel = true;
                        return;
                        //this.BeginInvoke(new Action(() =>
                        //{
                        //    dgv_distribute.Rows[e.RowIndex].Cells["qty_to_give"].Value = qtyReq;
                        //}));
                    }
                }

                // 2. Calculate the total QTY TO GIVE if new value is accepted
                int total = 0;
                for (int i = 0; i < dgv_distribute.Rows.Count; i++)
                {
                    if (i == e.RowIndex) // use the new value
                    {
                        total += newValue;
                    }
                    else
                    {
                        var cellValue = dgv_distribute.Rows[i].Cells[qtyToGiveColumnIndex].Value;
                        if (cellValue != null && int.TryParse(cellValue.ToString(), out int val))
                        {
                            total += val;
                        }
                    }
                }

                // 3. Compare with ORDER QTY
                if (!int.TryParse(txt_order_qty.Text, out int orderQty))
                {
                    MessageBox.Show("Invalid ORDER QTY.");

                    e.Cancel = true;
                    return;
                }

                if (total > orderQty)
                {
                    MessageBox.Show("Total 'QTY TO GIVE' exceeds the ORDER QTY.");

                    e.Cancel = true;
                    return;
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    dgv_distribute.Rows[e.RowIndex].Cells["qty_to_give"].Value = 0;
                    //}));
                }

            }
        }
    }
}
