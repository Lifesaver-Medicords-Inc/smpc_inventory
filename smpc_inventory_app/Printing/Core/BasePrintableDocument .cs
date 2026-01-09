using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Printing.Core
{
    public abstract class  BasePrintableDocument : IPrintableDocument
    {
        protected Font HeaderFont { get; } = new Font("Arial", 14, FontStyle.Bold);
        protected Font SubHeaderFont { get; } = new Font("Arial", 10, FontStyle.Regular);
        protected Font BodyFont { get; } = new Font("Arial", 9);
        protected Font FooterFont { get; } = new Font("Arial", 9, FontStyle.Italic);

        public abstract string Title { get; }

        public virtual void BeginPrint() { }

        public abstract bool RenderPage(Graphics graphics, Rectangle marginBounds, int pageNumber);

        public virtual void EndPrint() { }

        #region Helper Methods

        protected void DrawLine(Graphics g, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
        }

        protected void DrawCenteredString(Graphics g, string text, Font font, Brush brush, Rectangle rect)
        {
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(text, font, brush, rect, sf);
        }
        #endregion
    }
}
