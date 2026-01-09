using Microsoft.Reporting.WinForms;
using smpc_inventory_app.Data;
using smpc_inventory_app.Printing.Core;
using smpc_inventory_app.Properties;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace smpc_inventory_app.Printing
{
    public class PurchaseOrderReportProvider : IReportProvider
    {
        private readonly int PoId;
        PurchaseOrdersWithDetails records;
        DataTable purchaseorder;
        DataTable updatedpurchaseorder;
        DataTable purchaseorderdetails;
        DataTable activePO;
        public PurchaseOrderReportProvider(int poId)
        {
            PoId = poId;
        }

        private DataTable header;
        private DataTable details;


        public string ReportPath =>
        Path.Combine(Settings.Default.REPORTPATH, "PurchaseOrderReport.rdlc");

        public async Task InitializeAsync()
        {
            var response = await RequestToApi<ApiResponseModel<PurchaseOrdersWithDetails>>
                .Get(ENUM_ENDPOINT.PURCHASING_PURCHASE_ORDER);

            records = response.Data;

            purchaseorder = JsonHelper.ToDataTable(records.purchaseorder);
            purchaseorderdetails = JsonHelper.ToDataTable(records.purchaseorderdetails);

            var headerRows = purchaseorder.Select($"id = {PoId}");
            if (headerRows.Length > 0)
                header = headerRows.CopyToDataTable();
            else
                header = purchaseorder.Clone();

            var detailsRows = purchaseorderdetails.Select($"based_id = {PoId}");
            if (detailsRows.Length > 0)
                details = detailsRows.CopyToDataTable();
            else
                details = purchaseorderdetails.Clone();
        }

        public IEnumerable<ReportDataSource> GetDataSources()
        {
            yield return new ReportDataSource("DataSet1", header);
            yield return new ReportDataSource("DataSet2", details);
        }

        public IEnumerable<ReportParameter> GetParameters()
        {
            if (header == null || header.Rows.Count == 0)
                yield break;

            var row = header.Rows[0];

            string GetValue(string columnName) =>
                row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                    ? row[columnName].ToString()
                    : "";

            string GetDateValue(string columnName)
            {
                if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                    return "";

                if (row[columnName] is DateTime dt)
                    return dt.ToString("MM/dd/yyyy");

                return DateTime.TryParse(row[columnName].ToString(), out var parsed)
                    ? parsed.ToString("MM/dd/yyyy")
                    : "";
            }

            yield return new ReportParameter("SupplierName", GetValue("supplier_name"));
            yield return new ReportParameter("Address", GetValue("address"));
            yield return new ReportParameter("Fax", GetValue("fax_no"));
            yield return new ReportParameter("Tin", GetValue("tin_no"));
            yield return new ReportParameter("TelNo", GetValue("main_tel_no"));
            yield return new ReportParameter("DeliverTo", GetValue("deliver_to"));
            yield return new ReportParameter("TaxCode", GetValue("tax_code"));
            yield return new ReportParameter("PaymentTerms", GetValue("payment_terms_id"));
            yield return new ReportParameter("ShipVia", GetValue("deliver_via"));
            yield return new ReportParameter("Attention", GetValue("supplier_name"));
            yield return new ReportParameter("DocNo", "PO#" + GetValue("doc_no"));
            yield return new ReportParameter("Date", GetDateValue("date"));
            yield return new ReportParameter("RefDocNo", GetValue("ref_doc_no"));
            yield return new ReportParameter("NetOfVat", GetValue("net_of_vat"));
            yield return new ReportParameter("Vat", GetValue("vat"));
            yield return new ReportParameter("TotalAmountDue", GetValue("total_amount_due"));
        }

    }
}
