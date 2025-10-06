 using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Item;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing.PurchasingList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing.PurchaseList
{
    public partial class PRPurchaseItemCard : UserControl
    {
        public DataTable records;
        public DataTable guidingprice;
        public PRPurchasingListModel purchasereq;
        public string ItemCode => purchasereq.item_code;
        public string ItemDescription => purchasereq.item_description;
        public string ItemBrand => purchasereq.item_brand;
        DataTable payment = new DataTable();

        public PRPurchaseItemCard(PRPurchasingListModel item)
        {
            InitializeComponent();
            this.purchasereq = item;
            
        }
        // LOAD
        private async void PRPurchaseItemCard_Load(object sender, EventArgs e)
        {
            Helpers.SetInputsReadOnlyState(new[] { pnl_head }, true);
            txt_order_qty.ReadOnly = false;

            LoadItemData();
            FetchPaymentTerms();

            // Attach event before binding happens
            dgv_canvass.DataBindingComplete += dgv_canvass_DataBindingComplete;
            
            await FetchCanvassSheet();
            FetchGuidingPrice();
        }
        private void LoadItemData()
        {
            txt_item_code.Text = "I#" + purchasereq.item_code;
            txt_item_description.Text = purchasereq.item_description;
            txt_req_qty.Text = purchasereq.total_qty.ToString();
            txt_order_qty.Text = "0";
            txt_brand.Text = purchasereq.item_brand;
            txt_req_qty_uom.Text = purchasereq.unit_of_measure;
            txt_order_qty_uom.Text = purchasereq.unit_of_measure;
            txt_id.Text = purchasereq.item_id.ToString();
            // etc.
        }
        private async Task FetchCanvassSheet(Action onComplete = null)
        {
            records = await PurchasingCanvassSheetServices.GetAsDataTable();

            var paymentTermsid = payment.AsEnumerable()
                .Select(i => i.Field<int>("id"))
                .ToList();

            var defaultValue = paymentTermsid.FirstOrDefault();

            foreach (DataRow row in records.AsEnumerable()
                .Where(r => !paymentTermsid.Contains(r.Field<int>("payment_terms"))))
            {
                row["payment_terms"] = defaultValue;
            }


            DataView dv = new DataView(records);
            dv.RowFilter = $"item_id = '{purchasereq.item_id}'";
            bindingSourceCanvassSheet.DataSource = dv.ToTable();
            DisplayTrend();

            foreach (DataGridViewRow row in dgv_canvass.Rows)
            {
                if (row.Cells["current_list_price"].Value != null &&
                    row.Cells["previous_list_price"].Value != null)
                {
                    var current = Convert.ToDecimal(row.Cells["current_list_price"].Value);
                    var previous = Convert.ToDecimal(row.Cells["previous_list_price"].Value);

                    row.Cells["price_trend"].ToolTipText = $"Previous Price: ₱{previous:N2}";
                }

                // Contact tooltip logic
                var contactValue = row.Cells["contact_no"].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(contactValue))
                {
                    var contacts = contactValue.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (contacts.Length > 1)
                    {
                        row.Cells["contact_no"].Value = contacts[0].Trim();
                        var remaining = string.Join("\n", contacts.Skip(1).Select(c => c.Trim()));
                        row.Cells["contact_no"].ToolTipText = $"Other: {remaining}";

                    }
                    else
                    {
                        row.Cells["contact_no"].ToolTipText = "No other contact";
                    }
                }
            }

            HideColumn(dgv_canvass, "bpi_item_id", "item_id");

            onComplete?.Invoke();
        }
        private async void FetchGuidingPrice()
        {
            guidingprice = await GuidingPriceServices.GetAsDataTable();
            DataView guidingPriceView = new DataView(guidingprice);

            if (guidingPriceView.Count != 0)
            {
                guidingPriceView.RowFilter = $"item_id = '{purchasereq.item_id}'";

                if (guidingPriceView.Count > 0)
                {
                    var lastPrice = guidingPriceView[0]["last_price"];
                    purchasereq.last_purchase_price = lastPrice.ToString();
                }

                bindingSourceGuidingPrice.DataSource = guidingPriceView.ToTable();

                foreach (DataGridViewRow row in dgv_guiding_price.Rows)
                {
                    var pairs = new (string PriceCol, string SupplierCol)[]
                    {
                        ("last_price", "last_supplier_name"),
                        ("second_last_price", "second_last_supplier_name"),
                        ("third_last_price", "third_last_supplier_name"),
                        ("lowest_1yr", "lowest_1yr_supplier_name"),
                        ("lowest_3yr", "lowest_3yr_supplier_name"),
                        ("lowest_alltime", "lowest_alltime_supplier_name")
                    };

                    foreach (var (priceCol, supplierCol) in pairs)
                    {
                        var priceCell = row.Cells[priceCol];
                        var supplierName = row.Cells[supplierCol]?.Value?.ToString();

                        if (priceCell != null && !string.IsNullOrWhiteSpace(supplierName))
                        {
                            priceCell.ToolTipText = $"Supplier: {supplierName}";
                        }
                    }
                }
            }
        }
        private async void FetchPaymentTerms()
        {
            var serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            payment = await serviceSetup.GetAsDatatable();

            payment_terms.DataSource = payment;
            payment_terms.ValueMember = "id";
            payment_terms.DisplayMember = "name";
        }
        // ACTIONS
        private void ComputeDgv(DataGridViewRow row)
        {

            try
            {
                decimal currentListPrice = TryParseDecimal(row.Cells["current_list_price"].Value) ?? 0;
                string discountText = row.Cells["discount"].Value?.ToString() ?? "";
                decimal netPrice = currentListPrice;

                if (!string.IsNullOrEmpty(discountText) && discountText != "0")
                {
                    if (discountText.Contains("/"))
                    {
                        string[] discounts = discountText.Split('/');
                        decimal cumulativeMultiplier = 1;

                        foreach (string discount in discounts)
                        {
                            if (decimal.TryParse(discount, out decimal dVal))
                            {
                                cumulativeMultiplier *= (1 - (dVal / 100));
                            }
                        }
                        netPrice = currentListPrice * cumulativeMultiplier;
                    }
                    else
                    {
                        if (decimal.TryParse(discountText, out decimal singleDiscount))
                        {
                            netPrice = currentListPrice * (1 - (singleDiscount / 100));
                        }
                        else
                        {
                            discountText = "0";
                        }
                    }
                }
                else
                {
                    discountText = "0";
                }

                // Round and update net_price in column 8
                row.Cells["net_price"].Value = netPrice;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error computing discount/net price: " + ex.Message);
            }
        }
        public void DisplayTrend()
        {
            dgv_canvass.CellFormatting += (s, e) =>
            {
                if (dgv_canvass.Columns[e.ColumnIndex].Name == "price_trend" && e.Value != null)
                {
                    string trend = e.Value.ToString();
                    if (trend == "▲")
                    {
                        e.CellStyle.ForeColor = Color.Firebrick;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    }
                    else if (trend == "▼")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    }
                }
            };
        }
        private async void SaveEditedCell(int rowIndex, int colIndex)
        {
            try
            {
                var row = dgv_canvass.Rows[rowIndex];
                var cellValue = row.Cells[colIndex].Value;

                int? id = TryParseInt(row.Cells["id"].Value);
                if (id == null || id == 0)
                {
                    MessageBox.Show("Select supplier first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    row.Cells[colIndex].Value = null;
                    row.Cells[colIndex].ToolTipText = "Please select supplier first before editing.";

                    return;
                }

                var columnMap = new Dictionary<int, string>
                {
                    { 0, "id" },
                    { 1, "supplier_id" },
                    { 2, "supplier_name" },
                    { 3, "contact_no" },
                    { 4, "order_size" },
                    { 5, "supplier_stock" },
                    { 6, "previous_list_price" },
                    { 7, "current_list_price" },
                    { 8, "new_list_price" },
                    { 9, "discount" },
                    { 10, "net_price" },
                    { 11, "price_trend" },
                    { 12, "price_validity" },
                    { 13, "payment_terms" },
                    { 14, "lead_time" },
                };

                if (!columnMap.ContainsKey(colIndex))
                    return;

                var data = new Dictionary<string, dynamic>();

                string fieldName = columnMap[colIndex];
                dynamic parsedValue = ParseValueByColumn(fieldName, cellValue);

                // data to be saved
                data["id"] = id;
                data[fieldName] = parsedValue;

                // Recalculate net_price for new price list and discount
                if (fieldName == "new_list_price" || fieldName == "discount")
                {
                    if (fieldName == "new_list_price")
                    {
                        data["previous_list_price"] = TryParseDecimal(row.Cells["current_list_price"].Value);
                        row.Cells["current_list_price"].Value = parsedValue;
                        row.Cells["new_list_price"].Value = 0;
                        data["current_list_price"] = parsedValue;
                    }

                    data["item_id"] = TryParseInt(txt_id.Text);
                    data["supplier_id"] = row.Cells["supplier_id"].Value;

                    ComputeDgv(row);

                    data["net_price"] = row.Cells["net_price"].Value;
                    data["price_validity"] = 1;
                    //set validity and date changed upon saving
                    data["start_date"] = dtp_date.Value.ToString("MM-dd-yyyy");
                    // Remove consumed new price
                    data.Remove("new_list_price");

                    MessageBox.Show("Current List Price updated!");
                }

                if (fieldName == "price_validity")
                {
                    var priceValidityText = row.Cells["price_validity"].Value?.ToString();
                    data["start_date"] = dtp_date.Value.ToString("MM/dd/yyyy");

                    if (!string.IsNullOrEmpty(priceValidityText))
                    {
                        if (DateTime.TryParse(dtp_date.Text, out DateTime baseDate))
                        {
                            if (DateTime.TryParseExact(priceValidityText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                            {
                                int days = (parsedDate - baseDate).Days;
                                data["price_validity"] = days;
                            }

                            else if (int.TryParse(priceValidityText, out int days))
                            {
                                data["price_validity"] = days;
                            }
                            else
                            {
                                MessageBox.Show("Expected input [(MM/dd/yyyy) or (no. of days)]");
                            }
                        }
                    }
                }

                // call update service
                var response = await PurchasingCanvassSheetServices.Update(data);

                if (!response.Success)
                {
                    Helpers.ShowDialogMessage("error", "Failed to update item");
                }
                // Ensure CurrentCell is not null to avoid NullReferenceException
                if (dgv_canvass.CurrentCell == null)
                {
                    dgv_canvass.CurrentCell = dgv_canvass.Rows[rowIndex].Cells[colIndex];
                }

                int selectedRowIndex = dgv_canvass.CurrentCell.RowIndex;
                int selectedColumnIndex = dgv_canvass.CurrentCell.ColumnIndex;

                await FetchCanvassSheet(() =>
                {
                    if (selectedRowIndex >= 0 &&
                        selectedRowIndex < dgv_canvass.Rows.Count &&
                        selectedColumnIndex >= 0 &&
                        selectedColumnIndex < dgv_canvass.Columns.Count)
                    {
                        dgv_canvass.CurrentCell = dgv_canvass.Rows[selectedRowIndex].Cells[selectedColumnIndex];
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
        private async void SaveSupplierRow(int rowIndex)
        {
            try
            {
                ApiResponseModel response = new ApiResponseModel();

                if (rowIndex < 0 || rowIndex >= dgv_canvass.Rows.Count)
                    return;

                var row = dgv_canvass.Rows[rowIndex];
                var cells = row.Cells;
                var data = new Dictionary<string, dynamic>();

                //save primary data to generate id
                string[] fieldNames = { "id", "item_id", "supplier_id", "supplier_name", "current_list_price", "contact_no", "payment_terms"};
                object[] values = {
                    row.Cells["id"].Value,
                    txt_id.Text,
                    row.Cells["supplier_id"] .Value,
                    row.Cells["supplier_name"].Value,
                    0,
                    row.Cells["contact_no"].Value,
                    row.Cells["payment_terms"].Value
                };

                for (int i = 0; i < fieldNames.Length; i++)
                {
                    var parsed = ParseValueByColumn(fieldNames[i], values[i]);
                    if (parsed != null || fieldNames[i] == "id") // allow id to be null for new insert
                        data[fieldNames[i]] = parsed;
                }

                data["start_date"] = dtp_date.Value.ToString("MM-dd-yyyy");
                data["order_size"] = 0;
                data["supplier_stock"] = 0;
                data["new_list_price"] = 0;

                // Check if it's a new record
                bool isNewRecord = !data.ContainsKey("id") || data["id"] == null || (data["id"] is int && data["id"] == 0);

                if (isNewRecord)
                {
                    data.Remove("id");
                }

                // Save to API
                response = isNewRecord
                    ? await PurchasingCanvassSheetServices.Insert(data)
                    : await PurchasingCanvassSheetServices.Update(data);

                if (response.Success)
                {
                    await FetchCanvassSheet(); // Refresh data after saving
                }
                else
                {
                    string message = isNewRecord
                        ? "Failed to save item.\n" + response.message
                        : "Failed to update item.\n" + response.message;

                    Helpers.ShowDialogMessage("error", message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving row {rowIndex}: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void UpdateCanvassButtonText(bool isExpanded, bool hasWarning)
        {
            string arrow = isExpanded ? "▲" : "▼";
            btn_canvass.Text = (hasWarning ? "! " : "") + $" CANVASS {arrow}";

            btn_canvass.ForeColor = hasWarning ? Color.Firebrick : SystemColors.ControlText;

        }
        // EVENTS
        private void btn_add_supplier_Click(object sender, EventArgs e)
        {
            BusnessPartnerInfoModal modal = new BusnessPartnerInfoModal();
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ShowDialog();
        }
        private void btn_canvass_Click(object sender, EventArgs e)
        {
            bool isExpanded = pnl_canvass.Height != 0;
            this.Height = isExpanded ? 130 : 354;

            bool hasWarning = btn_canvass.Text.StartsWith("!");

            UpdateCanvassButtonText(!isExpanded, hasWarning);
        }
        private void btn_view_details_Click(object sender, EventArgs e)
        {
            string[] orderNos = purchasereq.purchase_requisition_nos.Split(',');
            string[] orderDetailIds = purchasereq.purchase_requisition_detail_ids.Split(',');
            string[] qtys = purchasereq.qtys.Split(',');
            string[] commitmentDates = purchasereq.commitment_dates.Split(',');


            DataTable dt = new DataTable();
            dt.Columns.Add("order_no", typeof(string));
            dt.Columns.Add("order_detail_id", typeof(string));
            dt.Columns.Add("qty", typeof(string));
            dt.Columns.Add("commitment_date", typeof(string));

            int maxLength = new[] { orderNos.Length, orderDetailIds.Length, qtys.Length, commitmentDates.Length}.Max();

            for (int i = 0; i < maxLength; i++)
            {
                string orderNo = i < orderNos.Length ? "SO#" + orderNos[i] : "N/A";
                string orderDetailId = i < orderDetailIds.Length ? orderDetailIds[i] : "N/A";
                int qty = (i < qtys.Length && int.TryParse(qtys[i], out int parsedQty)) ? parsedQty : 0;
                string commitmentDate = i < commitmentDates.Length ? commitmentDates[i] : "N/A";

                dt.Rows.Add(orderNo, orderDetailId, qty, commitmentDate);

                this.purchasereq.purchase_requisition_detail_ids = orderDetailId;
                this.purchasereq.qty = qty.ToString();
            }

            ViewDetailsModal modal = new ViewDetailsModal(dt);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "order_no", "DOC NO" },
                { "order_detail_id", "ORDER DETAIL ID" },
                { "qty", "QTY" },
                { "commitment_date", "COMMITMENT DATE" },
            };
            modal.SetData(dt, headers);
            modal.ShowDialog();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_order_qty.Text) || txt_order_qty.Text == "0")
            {
                MessageBox.Show("Order Quantity is required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txt_order_qty.Focus();

                chk_pr.CheckedChanged -= checkBox1_CheckedChanged;
                chk_pr.Checked = false;
                chk_pr.CheckedChanged += checkBox1_CheckedChanged;

                return;
            }
            if (dgv_canvass.Rows.Count == 0 || dgv_canvass.Rows.Count == 1 && dgv_canvass.Rows[0].IsNewRow)
            {
                MessageBox.Show("Select Supplier.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                dgv_canvass.Focus();

                chk_pr.CheckedChanged -= checkBox1_CheckedChanged;
                chk_pr.Checked = false;
                chk_pr.CheckedChanged += checkBox1_CheckedChanged;

                return;
            }
            purchasereq.order_is_checked = chk_pr.Checked;
            purchasereq.order_type = "PR";
        }
        private async void dgv_canvass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgv_canvass.Columns[e.ColumnIndex].Name == "supplier_name" && dgv_canvass.Rows[e.RowIndex].IsNewRow)
                {
                    List<int> existingSupplierIds = new List<int>();
                    foreach (DataGridViewRow row in dgv_canvass.Rows)
                    {
                        if (row.Cells["supplier_id"].Value != null && int.TryParse(row.Cells["supplier_id"].Value.ToString(), out int id))
                        {
                            existingSupplierIds.Add(id);
                        }
                    }

                    SupplierModal modal = new SupplierModal(existingSupplierIds, this.purchasereq.item_id.ToString());
                    DialogResult r = modal.ShowDialog();

                    if (r == DialogResult.OK)
                    {
                        Dictionary<string, dynamic> result = modal.GetResult();

                        this.dgv_canvass.Rows[e.RowIndex].Cells[1].Value = result["supplier_id"];
                        this.dgv_canvass.Rows[e.RowIndex].Cells[2].Value = result["supplier_name"];
                        this.dgv_canvass.Rows[e.RowIndex].Cells[3].Value = result["contact_no"];
                        this.dgv_canvass.Rows[e.RowIndex].Cells["payment_terms"].Value = result["payment_terms_id"];


                        SaveSupplierRow(e.RowIndex);
                    }
                    await FetchCanvassSheet();
                }
            }
        }
        private void dgv_canvass_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    int rowIndex = e.RowIndex;
                    int colIndex = e.ColumnIndex;

                    SaveEditedCell(rowIndex, colIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }

            }
        }
        private void dgv_canvass_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            bool isExpired = false;

            foreach (DataGridViewRow row in dgv_canvass.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["price_validity"].Value != null &&
                    int.TryParse(row.Cells["price_validity"].Value.ToString(), out int daysRemaining) &&
                    daysRemaining <= 0)
                {
                    isExpired = true;

                    row.Cells["current_list_price"].Style.ForeColor = Color.Firebrick;
                    row.Cells["price_validity"].Style.ForeColor = Color.Firebrick;

                    //row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }

            UpdateCanvassButtonText(pnl_canvass.Height != 0, isExpired);
        }
        private void dgv_canvass_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException)
            {
                if (e.Exception is FormatException)
                {
                    var columnName = ((DataGridView)sender).Columns[e.ColumnIndex].HeaderText;
                    MessageBox.Show($"Invalid value in column '{columnName}'. Please enter a valid number.",
                                    "Data Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    e.Cancel = true;         // Optional: cancels the commit of the invalid cell
                    e.ThrowException = false;
                }
            }
        }
        private async Task DeleteSupplierRow(DataGridView dgv, string idColumnName, Func<Task> onRefresh = null)

        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.IsNewRow)
                return;

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            int cellId = int.TryParse(dgv.CurrentRow.Cells[idColumnName]?.Value?.ToString(), out int idVal) ? idVal : 0;

            if (cellId <= 0)
            {
                Helpers.ShowDialogMessage("warning", "Invalid ID. Cannot delete.");
                return;
            }

            var data = new Dictionary<string, dynamic>
            {
                ["id"] = cellId
            };

            bool isSuccess = await PurchasingCanvassSheetServices.Delete(data);

            if (isSuccess)
            {
                Helpers.ShowDialogMessage("success", "Item deleted successfully.");
                if (onRefresh != null)
                    await onRefresh();
            }
            else
            {
                Helpers.ShowDialogMessage("error", "Failed to delete item.");
            }
        }

        private async void dgv_canvass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true; // Prevent default delete behavior
                await DeleteSupplierRow(dgv_canvass, "id", () => FetchCanvassSheet());

            }
        }
        private void txt_order_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txt_order_qty_TextChanged(object sender, EventArgs e)
        {
            this.purchasereq.order_qty = txt_order_qty.Text;
            Console.WriteLine("New order quantity: " + purchasereq.order_qty);
        }
        // HELPERS
        private string ConvertToString(object value)
        {
            return value?.ToString();
        }
        private void HideColumn(DataGridView grid, params string[] columnNames)
        {
            foreach (string colName in columnNames)
            {
                if (grid.Columns.Contains(colName))
                {
                    grid.Columns[colName].Visible = false;
                }
            }
        }
        private dynamic ParseValueByColumn(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "id":
                case "supplier_id":
                case "item_id":
                case "order_size":
                case "supplier_stock":
                case "payment_terms":
                case "price_validity":
                    {
                        var parsedValue = TryParseInt(value);
                        return parsedValue;
                    }
                case "current_list_price":
                case "new_list_price":
                case "net_price":
                    {
                        var parsedValue = TryParseDecimal(value);
                        return parsedValue;
                    }
                case "lead_time":
                case "supplier_name":
                case "contact_no":
                case "discount":
                default:
                    return ConvertToString(value);
            }
        }
        private decimal? TryParseDecimal(object value)
        {
            decimal result;
            if (decimal.TryParse(value?.ToString(), out result))
                return result;
            return null;
        }
        private int? TryParseInt(object value)
        {
            int result;
            if (int.TryParse(value?.ToString(), out result))
                return result;
            return null;
        }

        private void pnl_head_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txt_brand_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txt_order_qty_uom_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
