using smpc_app.Services.Helpers;
using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Item;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing
{
    public partial class PurchaseItemCard : UserControl
    {
        public string OrderId { get; private set; }
        public string BasedId { get; private set; }
        public string OrderNo { get; private set; }
        public string Purchaser { get; private set; }
        public string ItemId { get; private set; }
        public string ItemName { get; private set; }
        public string ItemBrand { get; private set; }
        public string ItemCode { get; private set; }
        public string ItemDescription { get; private set; }
        public string UnitPrice { get; private set; }
        public string Qty { get; private set; }
        public string UnitOfMeasures { get; private set; }
        public string TotalQty { get; private set; }
        GeneralSetupServices serviceSetup;

        public PurchaseItemCard(string orderId, string basedId, string orderNo, string purchaser, string itemId, string itemCode, string itemName, string itemBrand, string itemDescription, string unitPrice, string qty, string unitOfMeasures, string totalQty)
        {
            InitializeComponent();

            OrderId = orderId;
            BasedId = basedId;
            OrderNo = orderNo;
            Purchaser = purchaser;
            ItemId = itemId;
            ItemName = itemName;
            ItemBrand = itemBrand;
            ItemCode = itemCode;
            ItemDescription = itemDescription;
            UnitPrice = unitPrice;
            Qty = qty;
            UnitOfMeasures = unitOfMeasures;
            TotalQty = totalQty;

            btn_canvass.Text = "▼ Canvass";
        }
        private void btn_canvass_Click(object sender, EventArgs e)
        {
            if (pnl_canvass.Height == 0)
            {
                //pnl_canvass.Height = 185;
                this.Height = 354;
                btn_canvass.Text = "▲ Canvass";
            }
            else
            {
                //pnl_canvass.Height = 0;
                this.Height = 130;
                btn_canvass.Text = "▼ Canvass";
            }
        }

        private void PurchaseItemCard_Load(object sender, EventArgs e)
        {
            GetPaymentTerms();
            FetchSalesOrder();
            //FetchItemSupplierDetails();
        }
        private void FetchSalesOrder()
        {
            txt_item_description.Text = ItemDescription;
            txt_req_qty.Text = TotalQty;
            txt_brand.Text = ItemBrand;
            txt_req_qty_uom.Text = UnitOfMeasures;
            txt_order_qty_uom.Text = UnitOfMeasures;
            txt_id.Text = ItemId;
        }
        private async void FetchItemSupplierDetails()
        {
            var records = await PurchasingListSupplierServices.GetAsDataTable();
            DataView dv = new DataView(records);
            dv.RowFilter = $"item_id = '{ItemId}'";
            bindingSourceSupplier.DataSource = dv;

            HideColumn(dgv_canvass, "bpi_item_id", "item_id");

        }
        private void btn_view_details_Click(object sender, EventArgs e)
        {
            string[] orderNos = OrderNo.Split(',');
            string[] qtys = Qty.Split(',');
            string[] unitPrices = UnitPrice.Split(',');


            DataTable dt = new DataTable();
            dt.Columns.Add("order_no", typeof(string));
            dt.Columns.Add("qty", typeof(string));
            dt.Columns.Add("unit_price", typeof(string));

            int maxLength = Math.Max(orderNos.Length, Math.Max(qtys.Length, unitPrices.Length));

            for (int i = 0; i < maxLength; i++)
            {
                string orderNo = i < orderNos.Length ? orderNos[i] : "N/A";
                int qty = (i < qtys.Length && int.TryParse(qtys[i], out int parsedQty)) ? parsedQty : 0;
                string unitPrice = i < unitPrices.Length ? unitPrices[i] : "N/A";

                dt.Rows.Add(orderNo, qty, unitPrice);
            }

            ViewDetailsModal modal = new ViewDetailsModal(dt);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "order_no", "DOC NO" },
                { "qty", "QTY" },
                { "unit_price", "UNIT PRICE" }
            };
            modal.SetData(dt, headers);
            modal.ShowDialog();
        }

        private void btn_add_supplier_Click(object sender, EventArgs e)
        {
            BusnessPartnerInfoModal modal = new BusnessPartnerInfoModal();
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ShowDialog();
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
        private async void GetPaymentTerms()
        {
            serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            CacheData.PaymentTerms = await serviceSetup.GetAsDatatable();

            var combobox = (DataGridViewComboBoxColumn)dgv_canvass.Columns["payment_terms"];
            combobox.DataSource = CacheData.PaymentTerms;
            combobox.DisplayMember = "name";
            combobox.ValueMember = "id";

        }

        private void dgv_canvass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                if (dgv_canvass.Columns[e.ColumnIndex].Name == "supplier")
                {


                    SupplierModal modal = new SupplierModal();
                    DialogResult r = modal.ShowDialog();

                    if (r == DialogResult.OK)
                    {

                        Dictionary<string, dynamic> result = modal.GetResult();

                        this.dgv_canvass.Rows[e.RowIndex].Cells[1].Value = result["supplier_id"];
                        this.dgv_canvass.Rows[e.RowIndex].Cells[2].Value = result["supplier"];
                        this.dgv_canvass.Rows[e.RowIndex].Cells[3].Value = result["contact_no"];

                    }
                    else
                    {
                        MessageBox.Show("CANCELLED");
                    }
                }
            }
        }
        private void dgv_canvass_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row was edited
            {
                SaveEditedRow(e.RowIndex);
            }
        }

        private void SaveEditedRow(int rowIndex)
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

            // Get row reference
            DataGridViewRow row = dgv_canvass.Rows[rowIndex];

            // Extract values directly from DataGridViewRow
            if (int.TryParse(row.Cells["id"].Value?.ToString(), out int id)) data["id"] = id;
            if (int.TryParse(row.Cells["supplier_id"].Value?.ToString(), out int supplierId)) data["supplier_id"] = supplierId;
            data["supplier_name"] = row.Cells["supplier_name"].Value?.ToString();
            if (double.TryParse(row.Cells["order_size"].Value?.ToString(), out double orderSize)) data["order_size"] = orderSize;
            if (double.TryParse(row.Cells["supplier_stock"].Value?.ToString(), out double supplierStock)) data["supplier_stock"] = supplierStock;
            if (decimal.TryParse(row.Cells["current_list_price"].Value?.ToString(), out decimal currentListPrice)) data["current_list_price"] = currentListPrice;
            if (decimal.TryParse(row.Cells["new_list_price"].Value?.ToString(), out decimal newListPrice)) data["new_list_price"] = newListPrice;
            if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discount)) data["discount"] = discount;
            if (decimal.TryParse(row.Cells["net_price"].Value?.ToString(), out decimal netPrice)) data["net_price"] = netPrice;
            data["price_validity"] = row.Cells["price_validity"].Value?.ToString();
            if (int.TryParse(row.Cells["payment_terms"].Value?.ToString(), out int paymentTerms)) data["payments_terms"] = paymentTerms;
            data["lead_time"] = row.Cells["lead_time"].Value?.ToString();

            // Print collected data (Can be replaced with a database update call)
            Console.WriteLine("=== Edited Row Data ===");
            foreach (var kvp in data)
            {
                MessageBox.Show($"Key: {kvp.Key}, Value: {kvp.Value}");
            }

            // Optional: Call a function to save this data to the database
        }

    }
}