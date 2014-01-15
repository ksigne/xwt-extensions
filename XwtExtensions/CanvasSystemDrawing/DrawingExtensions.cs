using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XwtExtensions.CanvasSystemDrawing
{
    public static class DrawingExtensions
    {
        public static void DrawImage(this Xwt.Drawing.Context ctx, System.Drawing.Bitmap image, Xwt.Point where)
        {
            using (System.IO.MemoryStream M = new System.IO.MemoryStream())
            {
                image.Save(M, System.Drawing.Imaging.ImageFormat.Bmp);
                M.Position = 0;
                ctx.DrawImage(Xwt.Drawing.Image.FromStream(M), where);
            }
        }

        public static System.Drawing.Color ToWindowsColor(this Xwt.Drawing.Color clr) {
            return System.Drawing.Color.FromArgb((int)(clr.Alpha*255), (int)(clr.Red*255), (int)(clr.Green*255), (int)(clr.Blue*255));
        }
    }
}
