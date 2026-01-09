using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Printing.Core
{
    public interface IPrintableDocument
    {
        string Title { get; }

        void BeginPrint();

        bool RenderPage(
            Graphics graphics,
            Rectangle marginBounds,
            int pageNumbers
            );

        void EndPrint();
    }
}
