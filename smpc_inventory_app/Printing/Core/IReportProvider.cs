using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Printing.Core
{
    public interface IReportProvider
    {
        string ReportPath { get; }

        IEnumerable<ReportDataSource> GetDataSources();

        IEnumerable<ReportParameter> GetParameters();
    }
}
