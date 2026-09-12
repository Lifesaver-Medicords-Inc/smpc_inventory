using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace smpc_inventory_app.Printing.Core
{
    // The Class A house template (spec 2.10): company block, centred title,
    // two-column header, ruled line table, remarks, signature footer. One layout,
    // Printing/HouseTemplate.rdlc, is shared by every document printed on it - a
    // document supplies only its content (see ReceivingReportPrint), so the look
    // is set in one place.
    //
    // The layout holds up to 10 columns and 4 signature blocks; a column shows
    // only when it has a header, a signature block only when it has a label.
    //
    // ColumnCount MUST match the number of H1..Hn parameters and C1..Cn dataset
    // fields in HouseTemplate.rdlc. The layout was widened from 8 to 10 for the
    // accounting list prints; leaving this at 8 supplied no H9/H10 and no C9/C10,
    // which made every render on this template throw.
    public class HouseTemplateReport : IReportProvider
    {
        public const int ColumnCount = 10;
        public const int SignatureCount = 4;

        // The company block exactly as the Purchase Order prints it. Inventory has
        // no company setup record to read these from.
        public const string CompanyName = "SUNSHINE MULTI PLUS CORPORATION";
        public const string CompanyAddress = "644 RAJA MATANDA STREET, TONDO, MANILA PHILIPPINES 1012";
        public const string CompanyContact = "Tel #: +63282477015 to 17 Fax #: +63282450287";

        public string Title { get; set; } = "";
        public string Remarks { get; set; } = "";

        // Header pairs, printed one "LABEL: value" per line: the partner side on
        // the left, document number / dates / references on the right.
        public List<KeyValuePair<string, string>> LeftBlock { get; } = new List<KeyValuePair<string, string>>();
        public List<KeyValuePair<string, string>> RightBlock { get; } = new List<KeyValuePair<string, string>>();

        public List<HouseTemplateColumn> Columns { get; } = new List<HouseTemplateColumn>();
        public List<string[]> Rows { get; } = new List<string[]>();

        // Label and the name printed on the line above it, e.g. PREPARED BY.
        public List<KeyValuePair<string, string>> Signatures { get; } = new List<KeyValuePair<string, string>>();

        public string ReportPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Printing", "HouseTemplate.rdlc");

        // Everything is supplied up front; nothing to fetch.
        public Task InitializeAsync() => Task.CompletedTask;

        public IEnumerable<ReportDataSource> GetDataSources()
        {
            var table = new DataTable("Lines");
            for (int i = 1; i <= ColumnCount; i++)
                table.Columns.Add("C" + i, typeof(string));

            foreach (var row in Rows)
            {
                var r = table.NewRow();
                for (int i = 0; i < ColumnCount; i++)
                    r[i] = row != null && i < row.Length ? (row[i] ?? "") : "";
                table.Rows.Add(r);
            }

            yield return new ReportDataSource("Lines", table);
        }

        public IEnumerable<ReportParameter> GetParameters()
        {
            if (Columns.Count > ColumnCount)
                throw new InvalidOperationException("The house template holds at most " + ColumnCount + " columns.");
            if (Signatures.Count > SignatureCount)
                throw new InvalidOperationException("The house template holds at most " + SignatureCount + " signatures.");

            var list = new List<ReportParameter>
            {
                Param("CompanyName", CompanyName),
                Param("CompanyAddress", CompanyAddress),
                Param("CompanyContact", CompanyContact),
                Param("Title", Title),
                Param("LeftBlock", Block(LeftBlock)),
                Param("RightBlock", Block(RightBlock)),
                Param("Remarks", Remarks),
            };

            string align = "";
            for (int i = 0; i < ColumnCount; i++)
            {
                HouseTemplateColumn col = i < Columns.Count ? Columns[i] : null;
                list.Add(Param("H" + (i + 1), col?.Header));
                align += col == null ? 'L' : col.Align;
            }
            list.Add(Param("Align", align));

            for (int i = 0; i < SignatureCount; i++)
            {
                bool has = i < Signatures.Count;
                list.Add(Param("Sig" + (i + 1) + "Label", has ? Signatures[i].Key : ""));
                list.Add(Param("Sig" + (i + 1) + "Name", has ? Signatures[i].Value : ""));
            }

            return list;
        }

        public static KeyValuePair<string, string> Pair(string label, string value)
        {
            return new KeyValuePair<string, string>(label, value ?? "");
        }

        private static ReportParameter Param(string name, string value)
        {
            return new ReportParameter(name, value ?? "");
        }

        private static string Block(IEnumerable<KeyValuePair<string, string>> lines)
        {
            return string.Join(Environment.NewLine, lines.Select(l => l.Key + ": " + (l.Value ?? "")));
        }
    }

    public class HouseTemplateColumn
    {
        public string Header { get; }

        // 'L', 'R' (numbers) or 'C'.
        public char Align { get; }

        public HouseTemplateColumn(string header, char align = 'L')
        {
            Header = header ?? "";
            Align = align;
        }
    }
}
