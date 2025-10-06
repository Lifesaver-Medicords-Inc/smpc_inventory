using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Purchasing;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Pages.Purchasing.PurchaseList;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing.PurchasingList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing
{
    public partial class NewPurchasingList : UserControl
    {
        private System.Windows.Forms.Timer debounceTimer;
        private Action pendingFilterAction;
        private List<SOPurchaseItemCard> allSalesOrderCards = new List<SOPurchaseItemCard>();
        private List<PRPurchaseItemCard> allRequisitionCards = new List<PRPurchaseItemCard>();
        private DataTable payment = new DataTable();
        private DataTable salesorders;
        private DataTable activePO;
        private DataTable closedPO;
      
        private string user;
        public NewPurchasingList()
        {
            InitializeComponent();
        }

        // Create delegate
        public delegate void TriggerNewFormDelegate(string title, Control control);

        // Define an event bassed on delegate
        public event TriggerNewFormDelegate TriggerNewForm;

        private async void btn_create_order_po_Click(object sender, EventArgs e)
        {
           
        }

        private async void btn_create_po_Click(object sender, EventArgs e)
        {
            TabPage activeTab = tabControl1.SelectedTab;

            if (activeTab.Text == "SALES ORDER")
            {
                btn_create_po.Enabled = false;
                Cursor = Cursors.WaitCursor;

                try
                {
                    await GetCheckedSO();
                }
                finally
                {
                    btn_create_po.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
            else if (activeTab.Text == "PURCHASE REQUISITION")
            {
                btn_create_po.Enabled = false;
                Cursor = Cursors.WaitCursor;

                try
                {
                    await GetCheckedPR();
                }
                finally
                {
                    btn_create_po.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }

            
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private async Task GetCheckedPR()
        {
            // 1. Fetch canvass data
            var response = await RequestToApi<ApiResponseModel<List<PurchasingCanvassSheetModel>>>.Get(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET);
            var canvassRecords = response?.Data;
            if (canvassRecords == null)
            {
                MessageBox.Show("Failed to fetch canvass records.");
                return;
            }

            // 2. Get selected item cards
            var selectedCards = flowLayoutPanel2.Controls
                .OfType<PRPurchaseItemCard>()
                .Where(card => card.purchasereq.order_is_checked && int.TryParse(card.purchasereq.order_qty, out _))
                .ToList();

            if (selectedCards.Count == 0)
            {
                MessageBox.Show("No orders selected.");
                return;
            }

            // 3. Prepare items
            var selectedItems = selectedCards
                .Select(card => new OrderDistributionData2
                {
                    Card = card.purchasereq,
                    OrderDetailIds = card.purchasereq.purchase_requisition_detail_ids,
                    ItemId = card.purchasereq.item_id,
                    ItemCode = card.purchasereq.item_code,
                    ItemDescription = card.purchasereq.item_description,
                    ItemBrand = card.purchasereq.item_brand,
                    ItemName = card.purchasereq.item_name,
                    OrderQty = int.Parse(card.purchasereq.order_qty),
                    ReqQty = card.purchasereq.total_qty,
                    UnitOfMeasure = card.purchasereq.unit_of_measure,
                    OrderNos = card.purchasereq.purchase_requisition_nos,
                    ProjectNames = card.purchasereq.requestors,
                    SalesExecutives = card.purchasereq.departments,
                    CommitmentDates = card.purchasereq.commitment_dates,
                    LastPurchasePrice = card.purchasereq.last_purchase_price,
                    Qtys = card.purchasereq.qtys,
                    Status = card.purchasereq.status
                })
                .ToList();

            // 4.1 Collect under-allocated items
            var itemsNeedingDistribution = selectedItems
                .Where(x => x.OrderQty < x.ReqQty)
                .ToList();

            // 4.2. Collect all above-allocated items
            var itemsExceededDistribution = selectedItems
                .Where(x => x.OrderQty > x.ReqQty)
                .ToList();

            // 5.1. Show distribution modal if needed
            if (itemsNeedingDistribution.Any())
            {
                using (var distributionModal = new PurchaseRequisitionDistributionModal(itemsNeedingDistribution))
                {
                    var originalState = selectedItems.Select(x => new
                    {
                        x.ItemId,
                        x.Card.purchase_requisition_detail_ids,
                        x.Card.purchase_requisition_nos,
                        //x.Card.OrderQtys,
                        x.Card.qtys
                    }).ToList();

                    if (distributionModal.ShowDialog() != DialogResult.OK)
                    {
                        // Restore previous values
                        foreach (var state in originalState)
                        {
                            var item = selectedItems.FirstOrDefault(x => x.ItemId == state.ItemId);
                            if (item != null)
                            {
                                item.Card.purchase_requisition_detail_ids = state.purchase_requisition_detail_ids;
                                item.Card.purchase_requisition_nos = state.purchase_requisition_nos;
                                //item.Card.OrderQtys = state.OrderQtys;
                                item.Card.qtys = state.qtys;
                            }
                        }

                        MessageBox.Show("Manual distribution was cancelled.");
                        return;
                    }

                    var distributedResults = distributionModal.DistributedResults;

                    foreach (var distResult in distributedResults)
                    {
                        var updatedItem = itemsNeedingDistribution.FirstOrDefault(x => x.ItemId == distResult.ItemId);
                        if (updatedItem != null)
                        {
                            updatedItem.OrderDetailIds = distResult.OrderDetailIds;
                            updatedItem.OrderNos = distResult.OrderDetailNos;
                            updatedItem.OrderQtys = distResult.OrderQtys;
                            updatedItem.Qtys = distResult.QtysToGive;
                        }
                    }

                    foreach (var updated in itemsNeedingDistribution)
                    {
                        var itemToUpdate = selectedItems.FirstOrDefault(x => x.ItemId == updated.ItemId);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.Card.purchase_requisition_detail_ids = updated.OrderDetailIds;
                            itemToUpdate.Card.purchase_requisition_nos = updated.OrderNos;
                            itemToUpdate.Card.qtys = updated.Qtys;
                        }
                    }
                }
            }
            if (itemsExceededDistribution.Any())
            {
                foreach (var updated in itemsExceededDistribution)
                {
                    var itemToUpdate = selectedItems.FirstOrDefault(x => x.ItemId == updated.ItemId);
                    if (itemToUpdate != null)
                    {
                        var updatedOrderDetailIds = (updated.OrderDetailIds ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList();

                        var updatedOrderNos = (updated.OrderNos ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList();

                        var updatedOrderdQtys = (updated.OrderQtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0).ToList();

                        var updatedQtys = (updated.Qtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0).ToList();

                        int excessQty = updated.OrderQty - updated.ReqQty;
                        if (excessQty > 0)
                        {
                            updatedOrderdQtys.Add(0);
                            updatedQtys.Add(excessQty);
                            updatedOrderDetailIds.Add("Unallocated");
                            updatedOrderNos.Add("Unallocated");
                        }

                        // Ensure all lists are same length
                        int maxLength = new[] { updatedOrderdQtys.Count, updatedQtys.Count, updatedOrderDetailIds.Count, updatedOrderNos.Count }.Max();

                        // Pad all shorter lists
                        while (updatedOrderdQtys.Count < maxLength) updatedOrderdQtys.Add(0);
                        while (updatedQtys.Count < maxLength) updatedQtys.Add(0);
                        while (updatedOrderDetailIds.Count < maxLength) updatedOrderDetailIds.Add("Unallocated");
                        while (updatedOrderNos.Count < maxLength) updatedOrderNos.Add("Unallocated");

                        itemToUpdate.Card.purchase_requisition_detail_ids = string.Join(",", updatedOrderDetailIds);
                        itemToUpdate.Card.purchase_requisition_nos = string.Join(",", updatedOrderNos);
                        //itemToUpdate.Card.OrderQtys = string.Join(",", updatedOrderdQtys);
                        itemToUpdate.Card.qtys = string.Join(",", updatedQtys);
                    }
                }
            }

            // 6. Process supplier allocation using updated selectedItems
            var supplierMap = new Dictionary<int, List<PurchaseOrder.PurchaseOrderItem>>();

            foreach (var item in selectedItems)
            {
                int remainingQty = item.OrderQty;
                item.Card.status = item.OrderQty > item.ReqQty ? "FOR APPROVAL" : "APPROVED";

                var suppliers = canvassRecords
                    .Where(x => x.item_id == item.ItemId && x.supplier_stock > 0 && x.price_validity != "0")
                    .OrderBy(x => x.net_price)
                    .ToList();

                var orderDetailIdList = item.Card.purchase_requisition_detail_ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var qtyList = item.Card.qtys.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => int.TryParse(s.Trim(), out var q) ? q : 0).ToList();

                var orderQtyList = (item.Card.qtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var orderNos = item.Card.purchase_requisition_nos.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                Console.WriteLine($"New order quantity: {string.Join(",", qtyList)}");

                for (int s = 0; s < suppliers.Count && remainingQty > 0; s++)
                {
                    var supplier = suppliers[s];
                    int supplierAllocQty = Math.Min(supplier.supplier_stock, remainingQty);
                    if (supplierAllocQty <= 0) continue;

                    int qtyToAllocate = supplierAllocQty;

                    var allocatableOrderDetailIds = new List<string>();
                    var allocatableQtys = new List<int>();
                    var allocatableOrderQtys = new List<string>();
                    var allocatableOrderNos = new List<string>();

                    for (int i = 0; i < orderDetailIdList.Count && qtyToAllocate > 0; i++)
                    {
                        int currentQty = qtyList[i];
                        if (currentQty <= 0) continue;

                        if (qtyToAllocate >= currentQty)
                        {
                            allocatableOrderDetailIds.Add(orderDetailIdList[i]);
                            allocatableQtys.Add(currentQty);
                            allocatableOrderQtys.Add(orderQtyList[i]);
                            allocatableOrderNos.Add(orderNos[i]);
                            qtyToAllocate -= currentQty;
                            qtyList[i] = 0;
                        }
                        else
                        {
                            allocatableOrderDetailIds.Add(orderDetailIdList[i]);
                            allocatableQtys.Add(qtyToAllocate);
                            allocatableOrderQtys.Add(qtyToAllocate.ToString());
                            allocatableOrderNos.Add(orderNos[i]);
                            qtyList[i] -= qtyToAllocate;
                            qtyToAllocate = 0;
                        }
                    }

                    decimal lastPurchasePrice = 0;
                    decimal.TryParse(item.Card.last_purchase_price ?? "0", out lastPurchasePrice);

                    if (supplier.current_list_price > lastPurchasePrice)
                    {
                        var result = MessageBox.Show(
                            $"Purchase price ({supplier.current_list_price:C}) is higher than last purchase price ({lastPurchasePrice:C}) from {supplier.supplier_name}.\n\nDo you want to proceed?",
                            "Price Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (result == DialogResult.No) continue;
                        item.Card.status = "FOR APPROVAL";
                    }

                    int actuallyAllocated = allocatableQtys.Sum();
                    if (actuallyAllocated == 0) continue;

                    remainingQty -= actuallyAllocated;

                    if (!supplierMap.ContainsKey(supplier.supplier_id))
                        supplierMap[supplier.supplier_id] = new List<PurchaseOrder.PurchaseOrderItem>();

                    supplierMap[supplier.supplier_id].Add(new PurchaseOrder.PurchaseOrderItem
                    {
                        SupplierId = supplier.supplier_id,
                        OrderNo = string.Join(",", allocatableOrderNos),
                        OrderDetailId = string.Join(",", allocatableOrderDetailIds),
                        AllocatedQty = string.Join(",", allocatableQtys),
                        ItemId = item.ItemId,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        UnitOfMeasure = item.UnitOfMeasure,
                        PurchaseReq = item.ReqQty,
                        OrderQty = actuallyAllocated,
                        Qty = string.Join(",", allocatableOrderQtys),
                        UnitPrice = supplier.current_list_price,
                        Discount = supplier.discount,
                        PaymentTermsId = supplier.payment_terms,
                        Status = item.Card.status,
                        OrderType = item.Card.order_type
                    });
                }

                if (remainingQty > 0)
                {
                    MessageBox.Show($"Not enough stock for Item ID {item.ItemId} (short by {remainingQty}).");
                }
            }

            // 7. Create Purchase Orders per supplier
            foreach (var kvp in supplierMap)
            {
                int supplierId = kvp.Key;
                var items = kvp.Value;

                var purchaseOrderPage = new PurchaseOrder();
                purchaseOrderPage.LoadSelectedOrders(items);

                var item = items.FirstOrDefault();
                var supplierInfo = canvassRecords.FirstOrDefault(x =>
                    x.supplier_id == supplierId && x.item_id == item.ItemId);

                string supplierName = supplierInfo?.supplier_name ?? $"ID {supplierId}";
                string title = $"PO - {supplierName}";
                Console.WriteLine($"Creating PO tab for Supplier {supplierId} with {items.Count} items");

                TriggerNewForm?.Invoke(title, purchaseOrderPage);
            }
        }

        private async Task GetCheckedSO()
        {
            // 1. Fetch canvass data
            var response = await RequestToApi<ApiResponseModel<List<PurchasingCanvassSheetModel>>>.Get(ENUM_ENDPOINT.PURCHASING_CANVASS_SHEET);
            var canvassRecords = response?.Data;
            if (canvassRecords == null)
            {
                MessageBox.Show("Failed to fetch canvass records.");
                return;
            }

            // 2. Get selected item cards
            var selectedCards = flowLayoutPanel1.Controls
                .OfType<SOPurchaseItemCard>()
                .Where(card => card.orderIsChecked && int.TryParse(card.OrderQty, out _))
                .ToList();

            if (selectedCards.Count == 0)
            {
                MessageBox.Show("No orders selected.");
                return;
            }

            // 3. Prepare items
            var selectedItems = selectedCards
                .Select(card => new OrderDistributionData
                {
                    Card = card,
                    OrderDetailIds = card.OrderDetailIds,
                    ItemId = int.Parse(card.ItemId),
                    ItemCode = card.ItemCode,
                    ItemDescription = card.ItemDescription,
                    ItemBrand = card.ItemBrand,
                    ItemName = card.ItemName,
                    OrderQty = int.Parse(card.OrderQty),
                    ReqQty = int.Parse(card.TotalQty),
                    UnitOfMeasure = card.UnitOfMeasure,
                    OrderNos = card.SalesOrderNos,
                    ProjectNames = card.ProjectNames,
                    SalesExecutives = card.SalesExecutives,
                    CommitmentDates = card.CommitmentDates,
                    UnitPrices = card.UnitPrices,
                    Discounts = card.Discounts,
                    LowestSellingPrice = card.LowestSellingPrice,
                    LastPurchasePrice = card.LastPurchasePrice,
                    OrderQtys = card.OrderQtys,
                    Qtys = card.Qtys,
                    Status = card.Status
                })
                .ToList();

            // 4.1 Collect under-allocated items
            var itemsNeedingDistribution = selectedItems
                .Where(x => x.OrderQty < x.ReqQty)
                .ToList();

            // 4.2. Collect all above-allocated items
            var itemsExceededDistribution = selectedItems
                .Where(x => x.OrderQty > x.ReqQty)
                .ToList();

            // 5.1. Show distribution modal if needed
            if (itemsNeedingDistribution.Any())
            {
                using (var distributionModal = new SalesOrderDistributionModal(itemsNeedingDistribution))
                {
                    var originalState = selectedItems.Select(x => new
                    {
                        x.ItemId,
                        x.Card.OrderDetailIds,
                        x.Card.SalesOrderNos,
                        x.Card.OrderQtys,
                        x.Card.Qtys
                    }).ToList();

                    if (distributionModal.ShowDialog() != DialogResult.OK)
                    {
                        // Restore previous values
                        foreach (var state in originalState)
                        {
                            var item = selectedItems.FirstOrDefault(x => x.ItemId == state.ItemId);
                            if (item != null)
                            {
                                item.Card.OrderDetailIds = state.OrderDetailIds;
                                item.Card.SalesOrderNos = state.SalesOrderNos;
                                item.Card.OrderQtys = state.OrderQtys;
                                item.Card.Qtys = state.Qtys;
                            }
                        }

                        MessageBox.Show("Manual distribution was cancelled.");
                        return;
                    }

                    var distributedResults = distributionModal.DistributedResults;

                    foreach (var distResult in distributedResults)
                    {
                        var updatedItem = itemsNeedingDistribution.FirstOrDefault(x => x.ItemId == distResult.ItemId);
                        if (updatedItem != null)
                        {
                            updatedItem.OrderDetailIds = distResult.OrderDetailIds;
                            updatedItem.OrderNos = distResult.OrderDetailNos;
                            updatedItem.OrderQtys = distResult.OrderQtys;
                            updatedItem.Qtys = distResult.QtysToGive;
                        }
                    }

                    foreach (var updated in itemsNeedingDistribution)
                    {
                        var itemToUpdate = selectedItems.FirstOrDefault(x => x.ItemId == updated.ItemId);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.Card.OrderDetailIds = updated.OrderDetailIds;
                            itemToUpdate.Card.SalesOrderNos = updated.OrderNos;
                            itemToUpdate.Card.OrderQtys = updated.OrderQtys;
                            itemToUpdate.Card.Qtys = updated.Qtys;
                        }
                    }
                }
            }
            if (itemsExceededDistribution.Any())
            {
                foreach (var updated in itemsExceededDistribution)
                {
                    var itemToUpdate = selectedItems.FirstOrDefault(x => x.ItemId == updated.ItemId);
                    if (itemToUpdate != null)
                    {
                        var updatedOrderDetailIds = (updated.OrderDetailIds ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList();

                        var updatedOrderNos = (updated.OrderNos ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList();

                        var updatedOrderdQtys = (updated.OrderQtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0).ToList();

                        var updatedQtys = (updated.Qtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0).ToList();

                        int excessQty = updated.OrderQty - updated.ReqQty;
                        if (excessQty > 0)
                        {
                            updatedOrderdQtys.Add(0);
                            updatedQtys.Add(excessQty);
                            updatedOrderDetailIds.Add("Unallocated");
                            updatedOrderNos.Add("Unallocated");
                        }

                        // Ensure all lists are same length
                        int maxLength = new[] { updatedOrderdQtys.Count, updatedQtys.Count, updatedOrderDetailIds.Count, updatedOrderNos.Count }.Max();

                        // Pad all shorter lists
                        while (updatedOrderdQtys.Count < maxLength) updatedOrderdQtys.Add(0);
                        while (updatedQtys.Count < maxLength) updatedQtys.Add(0);
                        while (updatedOrderDetailIds.Count < maxLength) updatedOrderDetailIds.Add("Unallocated");
                        while (updatedOrderNos.Count < maxLength) updatedOrderNos.Add("Unallocated");

                        itemToUpdate.Card.OrderDetailIds = string.Join(",", updatedOrderDetailIds);
                        itemToUpdate.Card.SalesOrderNos = string.Join(",", updatedOrderNos);
                        itemToUpdate.Card.OrderQtys = string.Join(",", updatedOrderdQtys);
                        itemToUpdate.Card.Qtys = string.Join(",", updatedQtys);
                    }
                }
            }

            // 6. Process supplier allocation using updated selectedItems
            var supplierMap = new Dictionary<int, List<PurchaseOrder.PurchaseOrderItem>>();

            foreach (var item in selectedItems)
            {
                int remainingQty = item.OrderQty;
                item.Card.Status = item.OrderQty > item.ReqQty ? "FOR APPROVAL" : "APPROVED";

                var suppliers = canvassRecords
                    .Where(x => x.item_id == item.ItemId && x.supplier_stock > 0 && x.price_validity != "0")
                    .OrderBy(x => x.net_price)
                    .ToList();

                // Parse relevant lists from item.Card
                var orderDetailIdList = item.Card.OrderDetailIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var qtyList = item.Card.Qtys.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s.Trim(), out var q) ? q : 0).ToList();
                var orderQtyList = (item.Card.OrderQtys ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var orderNos = item.Card.SalesOrderNos.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                Console.WriteLine($"New order quantity: {string.Join(",", qtyList)}");

                for (int s = 0; s < suppliers.Count && remainingQty > 0; s++)
                {
                    var supplier = suppliers[s];
                    int supplierAllocQty = Math.Min(supplier.supplier_stock, remainingQty);
                    if (supplierAllocQty <= 0) continue;

                    int qtyToAllocate = supplierAllocQty;

                    var allocatableOrderDetailIds = new List<string>();
                    var allocatableQtys = new List<int>();
                    var allocatableOrderQtys = new List<string>();
                    var allocatableOrderNos = new List<string>();

                    for (int i = 0; i < orderDetailIdList.Count && qtyToAllocate > 0; i++)
                    {
                        int currentQty = qtyList[i];
                        if (currentQty <= 0) continue;

                        if (qtyToAllocate >= currentQty)
                        {
                            allocatableOrderDetailIds.Add(orderDetailIdList[i]);
                            allocatableQtys.Add(currentQty);
                            allocatableOrderQtys.Add(orderQtyList[i]);
                            allocatableOrderNos.Add(orderNos[i]);
                            qtyToAllocate -= currentQty;
                            qtyList[i] = 0;
                        }
                        else
                        {
                            allocatableOrderDetailIds.Add(orderDetailIdList[i]);
                            allocatableQtys.Add(qtyToAllocate);
                            allocatableOrderQtys.Add(qtyToAllocate.ToString());
                            allocatableOrderNos.Add(orderNos[i]);
                            qtyList[i] -= qtyToAllocate;
                            qtyToAllocate = 0;
                        }
                    }

                    int actuallyAllocated = allocatableQtys.Sum();
                    if (actuallyAllocated == 0) continue;

                    remainingQty -= actuallyAllocated;

                    decimal lowestSellingPrice;
                    decimal.TryParse(item.Card.LowestSellingPrice, out lowestSellingPrice);

                    decimal lastPurchasePrice;
                    decimal.TryParse(item.Card.LastPurchasePrice, out lastPurchasePrice);

                    if (supplier.current_list_price > lowestSellingPrice || supplier.current_list_price > lastPurchasePrice)
                    {
                        string message = $"Purchase price ({supplier.current_list_price:C}) ";

                        if (supplier.current_list_price > lowestSellingPrice && supplier.current_list_price > lastPurchasePrice)
                        {
                            message += $"both selling price ({lowestSellingPrice:C}) and last purchase price ({lastPurchasePrice:C})";
                        }
                        else if (supplier.current_list_price > lowestSellingPrice)
                        {
                            message += $"selling price ({lowestSellingPrice:C})";
                        }
                        else // higher than last purchase price only
                        {
                            message += $"last purchase price ({lastPurchasePrice:C})";
                        }

                        message += $" from {supplier.supplier_name}.\n\nDo you want to proceed?";

                        var result = MessageBox.Show(
                            message,
                            "Price Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (result == DialogResult.No) continue;

                        item.Card.Status = "FOR APPROVAL";
                    }

                    if (!supplierMap.ContainsKey(supplier.supplier_id))
                        supplierMap[supplier.supplier_id] = new List<PurchaseOrder.PurchaseOrderItem>();

                    supplierMap[supplier.supplier_id].Add(new PurchaseOrder.PurchaseOrderItem
                    {
                        SupplierId = supplier.supplier_id,
                        OrderNo = string.Join(",", allocatableOrderNos),
                        OrderDetailId = string.Join(",", allocatableOrderDetailIds),
                        AllocatedQty = string.Join(",", allocatableQtys),
                        ItemId = item.ItemId,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        UnitOfMeasure = item.UnitOfMeasure,
                        PurchaseReq = item.ReqQty,
                        OrderQty = actuallyAllocated,
                        Qty = string.Join(",", allocatableOrderQtys),
                        UnitPrice = supplier.current_list_price,
                        Discount = supplier.discount,
                        PaymentTermsId = supplier.payment_terms,
                        Status = item.Card.Status,
                        OrderType = item.Card.OrderType
                    });
                }

                if (remainingQty > 0)
                {
                    MessageBox.Show($"Not enough stock for Item ID {item.ItemId} (short by {remainingQty}).");
                }
            }

            // 7. Create Purchase Orders per supplier
            foreach (var kvp in supplierMap)
            {
                int supplierId = kvp.Key;
                var items = kvp.Value;

                var purchaseOrderPage = new PurchaseOrder();
                purchaseOrderPage.LoadSelectedOrders(items);

                var item = items.FirstOrDefault();
                var supplierInfo = canvassRecords.FirstOrDefault(x =>
                    x.supplier_id == supplierId && x.item_id == item.ItemId);

                string supplierName = supplierInfo?.supplier_name ?? $"ID {supplierId}";
                string title = $"PO - {supplierName}";
                Console.WriteLine($"Creating PO tab for Supplier {supplierId} with {items.Count} items");

                TriggerNewForm?.Invoke(title, purchaseOrderPage);
            }
        }

        private async void GetPurchaseRequisitionList()
        {
            var requisitions = await PRPurchasingListServices.GetList();

            var filteredList = requisitions.Where(r => r.purchaser == user).ToList();

            flowLayoutPanel2.Invoke(new Action(() =>
            {
                flowLayoutPanel2.Controls.Clear();
                flowLayoutPanel2.Padding = new Padding(5);
                allRequisitionCards.Clear(); // clear existing cache

                foreach (var item in filteredList)
                {
                    var card = new PRPurchaseItemCard(item);
                    card.Size = new Size(1237, 130);
                    allRequisitionCards.Add(card); // cache the card
                    flowLayoutPanel2.Controls.Add(card);
                }
            }));
        }

        private void GetSalesOrderList()
        {
            DataView dv = new DataView(salesorders);
            dv.RowFilter = $"Purchaser = '{user}'";

            flowLayoutPanel1.Invoke(new Action(() =>
            {
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.Padding = new Padding(5);

                foreach (DataRowView row in dv)
                {
                    string itemId = row["item_id"].ToString();
                    string purchaser = row["purchaser"].ToString();
                    string orderIds = row["order_ids"].ToString();
                    string orderDetailIds = row["order_detail_ids"].ToString();
                    string salesOrderNos = row["sales_order_nos"].ToString();
                    string projectNames = row["project_names"].ToString();
                    string salesExecutives = row["sales_executives"].ToString();
                    string unitPrices = row["unit_prices"].ToString();
                    string discounts = row["discounts"].ToString();
                    string lowestSellingPrice = "";
                    string quoteSupplier = row["quote_supplier"].ToString();
                    string itemCode = row["item_code"].ToString();
                    string itemDescription = row["item_description"].ToString();
                    string unitOfMeasure = row["unit_of_measure"].ToString();
                    string itemName = row["item_name"].ToString();
                    string itemBrand = row["item_brand"].ToString();
                    string commitmentDates = row["commitment_dates"].ToString();
                    string qtys = row["qtys"].ToString();
                    string totalQty = row["total_qty"].ToString();

                    SOPurchaseItemCard orderCard = new SOPurchaseItemCard(
                      itemId,
                      purchaser,
                      orderIds,
                      orderDetailIds,
                      salesOrderNos,
                      projectNames,
                      salesExecutives,
                      unitPrices,
                      discounts,
                      lowestSellingPrice,
                      quoteSupplier,
                      itemCode,
                      itemDescription,
                      unitOfMeasure,
                      itemName,
                      itemBrand,
                      commitmentDates,
                      qtys,
                      totalQty,
                      this.payment);
                    orderCard.Size = new Size(1237, 130);
                    allSalesOrderCards.Add(orderCard);
                    flowLayoutPanel1.Controls.Add(orderCard);
                }
            }));
        }

        private async void NewPurchasingList_Load(object sender, EventArgs e)
        {
            debounceTimer = new System.Windows.Forms.Timer();
            debounceTimer.Interval = 300; // milliseconds
            debounceTimer.Tick += (s, a) =>
            {
                debounceTimer.Stop();
                pendingFilterAction?.Invoke();
            };


            // Load user assingned sales order
            GetUser();
            FetchPaymentTermsSetup();
            salesorders = await SOPurchasingListServices.GetAsDataTable();
            GetSalesOrderList();
            GetPurchaseRequisitionList();
        }
        private void GetUser()
        {
            user = CacheData.CurrentUser.employee_id;
        }
        private async void FetchPaymentTermsSetup()
        {
            var serviceSetup = new GeneralSetupServices(ENUM_ENDPOINT.PAYMENT_TERMS);
            payment = await serviceSetup.GetAsDatatable();
        }
        
        private async void FetchActivePO()
        {
            activePO = await PurchaseOrderListServices.GetActivePOAsDataTable();

            if (activePO != null && activePO.Rows.Count > 0)
            {
                bindingSource_active_po.DataSource = activePO;
            }
            else
            {
                bindingSource_active_po = null;
            }
           
        }
        private async void FetchClosedPO()
        {
            closedPO = await PurchaseOrderListServices.GetClosedPOAsDataTable();
            if (closedPO != null && closedPO.Rows.Count > 0)
            {
                bindingSource_closed_po.DataSource = closedPO;
            }
            else
            {
                bindingSource_closed_po = null;
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // considerations reload data every tab click ------
            if (tabControl1.SelectedTab.Name == "tab_sales_order")
            {

            }
            else if (tabControl1.SelectedTab.Name == "tab_purchase_requisition")
            {
               
            }
            else if (tabControl1.SelectedTab.Name == "tab_active")
            {
                FetchActivePO();
            }
            else if (tabControl1.SelectedTab.Name == "tab_closed")
            {
                FetchClosedPO();
            }
            else
            {
                return;
            }
        }

        private void txt_search_so_TextChanged(object sender, EventArgs e)
        {
            string search = txt_search_so.Text;
            pendingFilterAction = () => FilterCards(search, flowLayoutPanel1, allSalesOrderCards);
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void txt_search_pr_TextChanged(object sender, EventArgs e)
        {
            string search = txt_search_pr.Text;
            pendingFilterAction = () => FilterCards(search, flowLayoutPanel2, allRequisitionCards);
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void FilterCards(string search, FlowLayoutPanel panel, IEnumerable<dynamic> allCards)
        {
            search = search.Trim().ToLower();

            panel.SuspendLayout();
            panel.Controls.Clear();

            foreach (var card in allCards)
            {
                if (string.IsNullOrEmpty(search) ||
                    (card.ItemCode?.ToLower().Contains(search) ?? false) ||
                    (card.ItemDescription?.ToLower().Contains(search) ?? false) ||
                    (card.ItemBrand?.ToLower().Contains(search) ?? false))
                {
                    panel.Controls.Add(card);
                }
            }

            panel.ResumeLayout();
        }


    }
}

public class OrderDistributionData
{
    public SOPurchaseItemCard Card { get; set; }
    public string CommitmentDates { get; set; }
    public string Discounts { get; set; }
    public string ItemBrand { get; set; }
    public string ItemCode { get; set; }
    public string ItemDescription { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string LastPurchasePrice { get; set; }
    public string LowestSellingPrice { get; set; }
    public string OrderDetailIds { get; set; }
    public string OrderNos { get; set; }
    public int OrderQty { get; set; }
    public string OrderQtys { get; set; }
    public string ProjectNames { get; set; }
    public string Qtys { get; set; }
    public int ReqQty { get; set; }
    public string SalesExecutives { get; set; }
    public string Status { get; set; }
    public string UnitOfMeasure { get; set; }
    public string UnitPrices { get; set; }
}

public class OrderDistributionData2
{
    public PRPurchasingListModel Card { get; set; }
    public string CommitmentDates { get; set; }
    public string Discounts { get; set; }
    public string ItemBrand { get; set; }
    public string ItemCode { get; set; }
    public string ItemDescription { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string LastPurchasePrice { get; set; }
    public string LowestSellingPrice { get; set; }
    public string OrderDetailIds { get; set; }
    public string OrderNos { get; set; }
    public int OrderQty { get; set; }
    public string OrderQtys { get; set; }
    public string ProjectNames { get; set; }
    public string Qtys { get; set; }
    public int ReqQty { get; set; }
    public string SalesExecutives { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string UnitOfMeasure { get; set; }
    public string UnitPrices { get; set; }
}