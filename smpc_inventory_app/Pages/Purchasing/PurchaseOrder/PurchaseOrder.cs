using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Setup.Model.Item;
using smpc_inventory_app.Pages.Setup;
using Inventory_SMPC.Pages;

namespace smpc_inventory_app.Pages.Purchasing
{
    public partial class PurchaseOrder : UserControl
    {
        PurchaseOrdersWithDetails records;
        int selectedRecord = 0;
        bool isCreatingNewPO = false;
        GeneralSetupServices serviceSetup;
        DataTable purchaseorder;
        DataTable updatedpurchaseorder;
        DataTable purchaseorderdetails;
        DataTable activePO;
        string position = CacheData.CurrentUser.position_id;


        string ReferenceOrderNos;

        public class PurchaseOrderItem
        {
            public int SupplierId { get; set; }
            public int ItemId { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string ItemDescription { get; set; }
            public string UnitOfMeasure { get; set; }
            public string OrderNo { get; set; }
            public string OrderDetailId { get; set; }
            public string Qty { get; set; }
            public string AllocatedQty { get; set; }
            public int OrderQty { get; set; }
            public int PurchaseReq { get; set; }
            public decimal UnitPrice { get; set; }
            public string Discount { get; set; }
            public int PaymentTermsId { get; set; }
            public string Status { get; set; }
            public string OrderType { get; set; }
        }
        public PurchaseOrder()
        {
            InitializeComponent();
        }
        private void PurchaseOrder_Load(object sender, EventArgs e)
        {
            FetchExistingPurchaseOrders();

            if (cmb_status.Text == "APPROVED" && (position != "Chief Operation Officer" || position == "Chief Business Development Officer"))
            {
                btn_edit.Enabled = false;
            }


        }
        private async void FetchExistingPurchaseOrders()
        {
            var response = await RequestToApi<ApiResponseModel<PurchaseOrdersWithDetails>>.Get(ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);
            records = response.Data;

            purchaseorder = JsonHelper.ToDataTable(records.purchaseorder);
            purchaseorderdetails = JsonHelper.ToDataTable(records.purchaseorderdetails);

            var filteredRows = purchaseorder.AsEnumerable().Where(row => !row.IsNull("status") && row.Field<string>("status") != "CANCELLED");

            activePO = filteredRows.Any()
                ? filteredRows.CopyToDataTable()
                : purchaseorder.Clone(); // empty table with same structure


            if (activePO != null && activePO.Rows.Count > 0 && !isCreatingNewPO)
            {
                selectedRecord = 0;
                Bind(true);

                BtnToggle(false);
            }
            else if (!isCreatingNewPO)
            {
                toolStrip1.Visible = false;
                MessageBox.Show("No records found");
            }
            else
            {
                BtnToggle(true);
                IsNewPO(true);
            }
           
        }

        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                FetchPaymentTermsSetup();
                FetchShipType();
                Panel[] pnlPurchaseOrder  = { pnl_header, pnl_footer};
                Helpers.BindControls(pnlPurchaseOrder, activePO, this.selectedRecord);
                Helpers.SetInputsReadOnlyState(pnlPurchaseOrder, true);

                foreach (var pnl in pnlPurchaseOrder)
                {
                    foreach (Control control in pnl.Controls)
                    {
                        if (control is TextBox textBox && textBox.Name.Contains("txt_doc_no"))
                        {
                            if (!textBox.Text.StartsWith("PO#"))
                            {
                                textBox.Text = "PO#" + textBox.Text;
                            }
                        }
                    }
                }

                FetchDataGridViewChild();

            }
        }
        private void FetchDataGridViewChild()
        {
            DataView dataViewPurchaseOrderDetails = new DataView(purchaseorderdetails);
            if (dataViewPurchaseOrderDetails.Count != 0)
            {
                dataViewPurchaseOrderDetails.RowFilter = "based_id = '" + activePO.Rows[this.selectedRecord]["id"].ToString() + "'";
                bindingSourcePurchaseOrder.DataSource = dataViewPurchaseOrderDetails;
            }
        }
        public async void LoadSelectedOrders(List<PurchaseOrderItem> selectedOrders)
        {
            isCreatingNewPO = true;

            FetchPaymentTermsSetup();
            FetchShipType();
            
            var suppliers = await PurchasingListSupplierServices.GetAsDataTable();

            // Generate ReferenceOrderNos by joining all OrderNo
            ReferenceOrderNos = string.Join(Environment.NewLine, selectedOrders
                .Select(o => o.OrderNo?.Trim())
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Distinct());

            string type = selectedOrders.FirstOrDefault()?.OrderType == "SO" ? "SO#" : "PR#";

            txt_ref_doc_no.Text = string.Join(Environment.NewLine,
                selectedOrders
                    .Where(o => !string.IsNullOrWhiteSpace(o.OrderNo))
                    .SelectMany(o =>
                        o.OrderNo
                            .TrimEnd(',')
                            .Split(',')
                            .Where(n => !string.IsNullOrWhiteSpace(n))
                            .Select(n => $"{type}{n.Trim()}")
                    )
                    .Distinct()
            );



            dgv_item.DataSource = null;
            dgv_item.Rows.Clear();

            foreach (var order in selectedOrders)
            {
                int supplierId = order.SupplierId;
                int itemId = order.ItemId;
                string itemCode = order.ItemCode;
                string itemName = order.ItemName;
                string itemDescription = order.ItemDescription;
                string unitOfMeasure = order.UnitOfMeasure;
                int reqQty = order.PurchaseReq;
                int qty = order.OrderQty;
                decimal unitPrice = order.UnitPrice;
                string discount = order.Discount;
                int paymentTermsId = order.PaymentTermsId;
                string status = order.Status;
                string orderType = order.OrderType;
                string orderDetailIds = order.OrderDetailId;
                string qtys = order.Qty;
                string allocqtys = order.AllocatedQty;

                var filteredRows = suppliers.AsEnumerable()
                    .Where(r => r.Field<int>("supplier_id") == supplierId);

                if (filteredRows.Any())
                {
                    var filteredTable = filteredRows.CopyToDataTable();
                    Panel[] purchaseOrder = { pnl_header };
                    Helpers.BindControls(purchaseOrder, filteredTable, 0);
                    cmb_status.Text = status;
                    txt_order_type.Text = orderType;
                    cmb_payment_terms.SelectedValue = paymentTermsId;
                }

                int rowIndex = dgv_item.Rows.Add();
                var dgvRow = dgv_item.Rows[rowIndex];

                dgvRow.Cells["item_id"].Value = itemId;
                dgvRow.Cells["item_code"].Value = itemCode;
                dgvRow.Cells["item_name"].Value = itemName;
                dgvRow.Cells["item_description"].Value = itemName;
                dgvRow.Cells["unit_of_measure"].Value = unitOfMeasure;
                dgvRow.Cells["req_qty"].Value = reqQty;
                dgvRow.Cells["order_qty"].Value = qty;
                dgvRow.Cells["unit_price"].Value = unitPrice;
                dgvRow.Cells["discount"].Value = discount;
                var (discountedPrice, discountedDiscountedPrice) = ComputeDiscountAndTotal(unitPrice, discount, qty);
                dgvRow.Cells["discounted_price"].Value = discountedPrice;
                dgvRow.Cells["total_price"].Value = discountedDiscountedPrice;
                dgvRow.Cells["order_detail_ids"].Value = orderDetailIds;
                dgvRow.Cells["qtys"].Value = qtys;
                dgvRow.Cells["allocated_qtys"].Value = allocqtys;
            }

            ComputeVatSummary();
        }

        private async void FetchPaymentTermsSetup()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();


            cmb_payment_terms.DataSource = CacheData.PaymentTerms;
            cmb_payment_terms.ValueMember = "id";
            cmb_payment_terms.DisplayMember = "name";
        }
        private async void FetchShipType()
        {
            CacheData.ShipType = await ShipTypeServices.GetAsDatatable();

            cmb_ship_type.DataSource = CacheData.ShipType;
            cmb_ship_type.ValueMember = "id";
            cmb_ship_type.DisplayMember = "ship_name";
        }
        private string  DocNoGenerator()
        {
            string docNo;

            if (updatedpurchaseorder.Rows.Count > 0)
            {
                int latestIndex = updatedpurchaseorder.Rows.Count - 1;
                DataRow latestRow = updatedpurchaseorder.Rows[latestIndex];

                // Check if "document_no" is not null or DBNull
                if (latestRow["doc_no"] != DBNull.Value && !string.IsNullOrEmpty(latestRow["doc_no"].ToString()))
                {
                    if (int.TryParse(latestRow["doc_no"].ToString(), out int itemNum))
                    {
                        docNo = (itemNum + 1).ToString().PadLeft(4, '0');
                    }
                    else
                    {
                        docNo = "1";
                    }
                }
                else
                {
                    docNo = "1";
                }
            }
            else
            {
                docNo = "1";
            }

            return docNo;
        }
        private (decimal discountedPrice, decimal totalDiscountedPrice) ComputeDiscountAndTotal(decimal unitPrice, string discountText, int quantity)
        {
            try
            {
                decimal discountPrice = unitPrice;
                

                if (!string.IsNullOrEmpty(discountText) && discountText != "0")
                {
                    if (discountText.Contains("/"))
                    {
                        string[] discounts = discountText.Split('/');
                        decimal multiplier = 1;

                        foreach (string discount in discounts)
                        {
                            if (decimal.TryParse(discount, out decimal dVal))
                            {
                                multiplier *= (1 - (dVal / 100));
                            }
                        }

                        discountPrice = unitPrice * multiplier;
                    }
                    else if (decimal.TryParse(discountText, out decimal singleDiscount))
                    {
                        discountPrice = unitPrice * (1 - (singleDiscount / 100));
                    }
                }
                decimal discountedPrice = discountPrice;
                decimal totalDiscountedPrice = discountedPrice * quantity;

                return (discountedPrice, totalDiscountedPrice);
            }
            catch
            {
                return (0, 0);
            }
        }
        private void ComputeVatSummary()
        {
            try
            {
                decimal totalAmount = 0;

                foreach (DataGridViewRow row in dgv_item.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (decimal.TryParse(row.Cells["total_price"].Value?.ToString(), out decimal rowTotal))
                    {
                        totalAmount += rowTotal;
                    }
                }
                // is vatable
                bool isVatable = txt_tax_code.Text == "VAT";
                
                decimal netOfVat = totalAmount;
                decimal vat = isVatable ? totalAmount * 0.12m : 0;
                decimal totalAmountDue = totalAmount + vat;

                Helpers.FormatAsCurrency(txt_total_amount_due, totalAmountDue);
                Helpers.FormatAsCurrency(txt_net_of_vat, netOfVat);
                Helpers.FormatAsCurrency(txt_vat, vat);

            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error calculating VAT summary: " + ex.Message);
            }
        }
        private void dgv_item_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {   
                if (dgv_item.Columns[e.ColumnIndex].Name == "details" && !dgv_item.Rows[e.RowIndex].IsNewRow)
                {
                    var row = dgv_item.Rows[e.RowIndex];

                    string orderNo = this.ReferenceOrderNos ?? "";
                    string orderDetailIds = row.Cells["order_detail_ids"].Value?.ToString() ?? "";
                    string qtys = row.Cells["allocated_qtys"].Value?.ToString() ?? "";

                    string[] referenceOrderNosArr = orderNo.Split(',');
                    string[] orderDetailIdsArr = orderDetailIds.Split(',');
                    string[] qtysArr = qtys.Split(',');

                    DataTable dt = new DataTable();
                    dt.Columns.Add("order_no", typeof(string));
                    dt.Columns.Add("order_detail_id", typeof(string));
                    dt.Columns.Add("qty", typeof(string));

                    int maxLen = Math.Max(referenceOrderNosArr.Length, Math.Max(orderDetailIdsArr.Length, qtysArr.Length));

                    for (int i = 0; i < maxLen; i++)
                    {
                        string salesOrderNo = i < referenceOrderNosArr.Length ? "SO#" + referenceOrderNosArr[i] : "NO REFERENCE";
                        string orderDetailId = i < orderDetailIdsArr.Length ? orderDetailIdsArr[i] : "UNALLOCATED";
                        string qty = i < qtysArr.Length ? qtysArr[i] : "0";

                        dt.Rows.Add(salesOrderNo, orderDetailId, qty);
                    }

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "order_no", "DOC NO" },
                        { "order_detail_id", "ORDER DETAIL ID" },
                        { "qty", "QTY" },
                    };

                    ViewDetailsModal modal = new ViewDetailsModal(dt); // or use parameterless and then SetData()
                    modal.SetData(dt, headers);
                    modal.ShowDialog();
                }
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.activePO.Rows.Count - 1 > this.selectedRecord)
            {
                this.selectedRecord++;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {
                this.selectedRecord--;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void IsNewPO(bool isNew)
        {
            btn_cancel.Visible = isNew;
            btn_close.Visible = !isNew;
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (activePO == null || activePO.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable newpurchaseorder = activePO.Copy();
            newpurchaseorder.Columns.Add("display_doc_no", typeof(string));

            foreach (DataRow row in newpurchaseorder.Rows)
            {
                row["doc_no"] = "PO#" + row["doc_no"].ToString();
            }

            // Change mapping to use the new column
            Dictionary<string, string> columnsToShow = new Dictionary<string, string>
                {
                    { "id", "ID" },
                    { "doc_no", "DOC NO" },
                    { "supplier_name", "SUPPLIER" }
                };


            using (SearchModal searchModal = new SearchModal("Search Items", newpurchaseorder, columnsToShow))
            {
                if (searchModal.ShowDialog() == DialogResult.OK)
                {
                    int selectedIndex = searchModal.SelectedIndex;

                    if (selectedIndex >= 0)
                    {
                        this.selectedRecord = selectedIndex;
                        Bind(true);
                    }
                }
            }

        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
            SetControlsEditable(cmb_ship_type, txt_deliver_to, txt_deliver_via, txt_bill_to, txt_remarks);
            SetControlsEditable(cmb_status);
            LoadStatusOptions(cmb_status.Text);

            if (position == "Chief Operation Officer" || position == "Chief Business Development Officer")
            {
                SetControlsEditable(cmb_status);
                cmb_status.Focus();
                LoadStatusOptions(cmb_status.Text);
            }
            else if (position == "Purchasing Officer")
            {
                SetControlsEditable(cmb_ship_type, txt_deliver_to, txt_deliver_via, txt_bill_to, txt_remarks);
            }
            else
            {
                MessageBox.Show($"{position} not allowed to edit");
            }
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (this.FindForm() is SMPC layout)
                layout.RemoveTabContaining(this);
        }
        private void LoadStatusOptions(string currentStatus)
        {
            cmb_status.Items.Clear();

            switch (currentStatus.ToUpper())
            {
                case "FOR APPROVAL":
                    cmb_status.Items.AddRange(new[] { "APPROVED", "CANCELLED", "FOR APPROVAL" });
                    break;
                case "CANCELLED":
                    cmb_status.Items.AddRange(new[] { "FOR APPROVAL", "CANCELLED" });
                    break;
                default:
                    cmb_status.Items.AddRange(new[] { "APPROVED", "CANCELLED" });
                    break;
            }
        }
        public static void SetControlsEditable(params Control[] controls)
        {
            foreach (var ctrl in controls)
            {
                if (ctrl is TextBox tb) tb.ReadOnly = false;
                else ctrl.Enabled = true;
            }
        }
        public static void SetControlsReadOnly(params Control[] controls)
        {
            foreach (var ctrl in controls)
            {
                if (ctrl is TextBox tb) tb.ReadOnly = true;
                else ctrl.Enabled = false;
            }
        }


        private void BtnToggle(bool isEdit)
        {
            btn_edit.Visible = !isEdit;
            btn_search.Visible = !isEdit;
            btn_prev.Visible = !isEdit;
            btn_next.Visible = !isEdit;
            btn_print.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
            cmb_status.Enabled = false;
            Helpers.SetInputsReadOnlyState(new[] { pnl_header, pnl_footer }, true);

            FetchExistingPurchaseOrders();
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            BtnToggle(false);
            btn_save.Enabled = false;

            try
            {
                SavePurchaseorder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                btn_save.Enabled = true;
            }
        }
        private async void SavePurchaseorder()
        {
            // 1. Fetch latest purchase orders
            var newresponse = await RequestToApi<ApiResponseModel<PurchaseOrdersWithDetails>>.Get(ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);
            records = newresponse.Data;
            updatedpurchaseorder = JsonHelper.ToDataTable(records.purchaseorder);

            // 2. Collect order header values
            var order = Helpers.GetControlsValues(new[] { pnl_header, pnl_footer });

            if (order.ContainsKey("doc_no") && order["doc_no"] is string docNo)
            {
                order["doc_no"] = docNo.StartsWith("PO#")
                    ? docNo.Substring(3)
                    : docNo;
            }

            order["supplier_id"] = int.TryParse(order["supplier_id"]?.ToString(), out int supplier_id) ? supplier_id : 0;

            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                order["doc_no"] = DocNoGenerator();
            }

            // 3. Extract order detail rows
            var orderDetails = Helpers.ConvertDataGridViewToDataTable(dgv_item);
            var orderRow = new List<Dictionary<string, dynamic>>();
            var statusUpdateRow = new List<Dictionary<string, dynamic>>();
            string orderType = txt_order_type.Text.Trim().ToUpper();

            foreach (DataRow row in orderDetails.Rows)
            {
                var detail = new Dictionary<string, dynamic>
                {
                    ["item_id"] = int.TryParse(row["item_id"]?.ToString(), out var itemId) ? itemId : 0,
                    ["item_code"] = row["item_code"]?.ToString(),
                    ["item_name"] = row["item_name"]?.ToString(),
                    ["item_description"] = row["item_description"]?.ToString(),
                    ["unit_of_measure"] = row["unit_of_measure"]?.ToString(),
                    ["req_qty"] = int.TryParse(row["req_qty"]?.ToString(), out var reqQty) ? reqQty : 0,
                    ["order_qty"] = int.TryParse(row["order_qty"]?.ToString(), out var orderQty) ? orderQty : 0,
                    ["unit_price"] = int.TryParse(row["unit_price"]?.ToString(), out var price) ? price : 0,
                    ["discount"] = row["discount"]?.ToString(),
                    ["discounted_price"] = decimal.TryParse(row["discounted_price"]?.ToString(), out var dPrice) ? dPrice : 0,
                    ["total_price"] = decimal.TryParse(row["total_price"]?.ToString(), out var tPrice) ? tPrice : 0,
                    ["order_detail_ids"] = row["order_detail_ids"]?.ToString(),
                    ["qtys"] = row["qtys"]?.ToString(),
                    ["allocated_qtys"] = row["allocated_qtys"]?.ToString()
                };

                orderRow.Add(detail);

                string[] ids = detail["order_detail_ids"].Split(',');
                string[] allocs = detail["allocated_qtys"].Split(',');

                for (int i = 0; i < Math.Min(ids.Length, allocs.Length); i++)
                {
                    if (int.TryParse(ids[i], out int detailId) && int.TryParse(allocs[i], out int allocQty))
                    {
                        statusUpdateRow.Add(new Dictionary<string, dynamic>
                            {
                                { orderType == "SO" ? "order_details_id" : "pr_order_id", detailId },
                                { "allocated_qty", allocQty }
                            });
                    }
                }
            }

            // 4. Validate order type
            if (orderType != "SO" && orderType != "PR")
            {
                Helpers.ShowDialogMessage("error", "Invalid order type selected.");
                return;
            }

            // 5. Prepare payload
            order["purchase_order_details"] = orderRow;
            string detailKey = orderType == "SO" ? "sales_order_details" : "purchase_requisition_details";
            order[detailKey] = statusUpdateRow;

            // 6. Insert or update
            bool isValidId = int.TryParse(txt_id.Text, out int recordId);
            bool isInsert = isNewRecord || !isValidId;
            order["id"] = recordId;

            if (isInsert)
                order.Remove("id");

            var response = isInsert
                ? await PurchaseOrderServices.Insert(order)
                : await PurchaseOrderServices.Update(order);

            if (!response.Success)
            {
                Helpers.ShowDialogMessage("error", $"Failed to {(isInsert ? "save" : "update")} Purchase Order.");
                return;
            }

            // 7. UI feedback
            Helpers.ShowDialogMessage("success", $"Purchase Order {(isInsert ? "saved" : "updated")} successfully.");

            if (isInsert)
            {
                if (this.FindForm() is SMPC layout)
                    layout.RemoveTabContaining(this);
            }
            else
            {
                Helpers.SetInputsReadOnlyState(new[] { pnl_header, pnl_footer }, true);

                FetchExistingPurchaseOrders();
                selectedRecord = isNewRecord ? purchaseorder.Rows.Count - 1 : selectedRecord;
            }
        }

        private void txt_deliver_to_TextChanged(object sender, EventArgs e)
        {

        }

        private void s(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
