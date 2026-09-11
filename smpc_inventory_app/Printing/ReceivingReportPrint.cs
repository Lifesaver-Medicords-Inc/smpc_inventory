using smpc_inventory_app.Model;
using smpc_inventory_app.Printing.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace smpc_inventory_app.Printing
{
    // The Receiving Report on the house template (spec 5.7, 2.10). The columns
    // follow the RR's MAIN and INVENTORY tabs: code, description, order /
    // received / rejected qty, reason for rejection, serial numbers, bin location.
    // PREPARED BY is the creating user; ADDRESS is our (the warehouse's) address.
    public static class ReceivingReportPrint
    {
        public static HouseTemplateReport Build(ReceivingReportModel rr, IEnumerable<ReceivingReportDetailsModel> lines)
        {
            if (rr == null) throw new ArgumentNullException(nameof(rr));

            var report = new HouseTemplateReport { Title = "RECEIVING REPORT" };

            report.LeftBlock.Add(HouseTemplateReport.Pair("SUPPLIER", rr.supplier));
            report.LeftBlock.Add(HouseTemplateReport.Pair("SUPPLIER CODE", rr.supplier_code));
            report.LeftBlock.Add(HouseTemplateReport.Pair("WAREHOUSE", rr.warehouse));
            report.LeftBlock.Add(HouseTemplateReport.Pair("ADDRESS", rr.warehouse_address));

            report.RightBlock.Add(HouseTemplateReport.Pair("DOC NO.", "RR#" + rr.doc_no.ToString("D4")));
            report.RightBlock.Add(HouseTemplateReport.Pair("DATE RECEIVED", FormatDate(rr.date_received)));
            report.RightBlock.Add(HouseTemplateReport.Pair("REFERENCE DOC.", rr.ref_doc));

            report.Columns.Add(new HouseTemplateColumn("CODE"));
            report.Columns.Add(new HouseTemplateColumn("DESCRIPTION"));
            report.Columns.Add(new HouseTemplateColumn("ORDER QTY", 'R'));
            report.Columns.Add(new HouseTemplateColumn("RECEIVED QTY", 'R'));
            report.Columns.Add(new HouseTemplateColumn("REJECTED QTY", 'R'));
            report.Columns.Add(new HouseTemplateColumn("REASON FOR REJECTION"));
            report.Columns.Add(new HouseTemplateColumn("SERIAL NO/S"));
            report.Columns.Add(new HouseTemplateColumn("BIN LOCATION"));

            foreach (var d in lines ?? Enumerable.Empty<ReceivingReportDetailsModel>())
            {
                report.Rows.Add(new[]
                {
                    d.item_code,
                    d.item_desc,
                    Qty(d.ordered_qty, d.ordered_uom),
                    Qty(d.received_qty, d.received_uom),
                    Qty(d.rejected_qty, d.rejected_uom),
                    d.reason_for_rejection,
                    d.serial_number,
                    d.bin_location,
                });
            }

            report.Signatures.Add(HouseTemplateReport.Pair("PREPARED BY", rr.prepared_by));
            return report;
        }

        private static string Qty(int? qty, string uom)
        {
            if (!qty.HasValue) return "";
            return string.IsNullOrWhiteSpace(uom) ? qty.Value.ToString(CultureInfo.InvariantCulture)
                                                  : qty.Value.ToString(CultureInfo.InvariantCulture) + " " + uom.Trim();
        }

        private static string FormatDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                ? date.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture)
                : value ?? "";
        }
    }
}
