using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xwt.Ext.UI
{
    public class GDICanvas: Xwt.Canvas
    {
        protected event Action<System.Drawing.Graphics, System.Drawing.Size> DrawRequestEvent;

        protected virtual void OnDrawRequest(System.Drawing.Graphics G, System.Drawing.Size S)
        {
            if (DrawRequestEvent != null)
                DrawRequestEvent(G, S);
        }

        protected override void OnDraw(Xwt.Drawing.Context ctx, Rectangle dirtyRect)
        {
            System.Drawing.Bitmap B = new System.Drawing.Bitmap(
                (int)(dirtyRect.Width * this.ParentWindow.Screen.ScaleFactor),
                (int)(dirtyRect.Height * this.ParentWindow.Screen.ScaleFactor)
            );

            System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(B);

            OnDrawRequest(G, new System.Drawing.Size(B.Width, B.Height));

            CanvasSystemDrawing.DrawingExtensions.DrawImage(ctx, B, new Xwt.Point(0, 0), this.ParentWindow.Screen.ScaleFactor);
        }
    }
}
