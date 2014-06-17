using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace Xwt.Ext.UI
{

    public static class Drawing
    {
        public static void DrawAdaptiveString(this System.Drawing.Graphics g, string Text, System.Drawing.Font Font, System.Drawing.Brush Brush, System.Drawing.RectangleF Where, System.Drawing.StringFormat Format)
        {
            System.Drawing.Font curFont = (System.Drawing.Font)Font.Clone();
            System.Drawing.SizeF curSize;
            do
            {
                curSize = g.MeasureString(Text, curFont);
                if (curSize.Width > Where.Width || curSize.Height > Where.Height)
                    curFont = new System.Drawing.Font(curFont.FontFamily, curFont.Size - 1);
            } while ((curSize.Width > Where.Width || curSize.Height > Where.Height));
            g.DrawString(Text, curFont, Brush, Where, Format);

        }
    }
    public class GradientButton
    {
        public Point Position;
        public Size Size;
        public Color Color;
        public Font Font = Xwt.Drawing.Font.SystemFont;
        public Font DescriptionFont = Xwt.Drawing.Font.SystemFont;
        public System.Drawing.Image Img = null;
        public string Text = "";
        public string Description = "";
        public int RawLayoutX;
        public int RawLayoutY;
        public bool CurrentMode = false;
        public string Clicked = "";
        public event EventHandler ButtonPressed;
        public bool DrawDescription = false;
        public bool DrawImg = false;

        public void Draw(System.Drawing.Graphics g, System.Drawing.PointF Where, double ScaleFactor = 1)
        {
            Size Size = this.Size;
            Size.Width *= ScaleFactor;
            Size.Height *= ScaleFactor;
            Where.X *= (float)ScaleFactor;
            Where.Y *= (float)ScaleFactor;
            System.Drawing.Pen penBlack = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
            //Рисуем прямоугольник
            System.Drawing.Brush brushGradient = new System.Drawing.Drawing2D.LinearGradientBrush(
                new System.Drawing.PointF(Where.X, Where.Y),
                new System.Drawing.PointF(Where.X + (float)Size.Width - 1, Where.Y + (float)Size.Height - 1),
                Xwt.Ext.CanvasSystemDrawing.DrawingExtensions.ToWindowsColor(this.Color),
                Xwt.Ext.CanvasSystemDrawing.DrawingExtensions.ToWindowsColor(this.Color.BlendWith(Colors.White, 0.8))
            );
            g.FillRectangle(brushGradient, Where.X, Where.Y, (float)Size.Width - 1, (float)Size.Height - 1);
            float Factor = (this.DrawDescription) ? 3 : 2;
            if (!DrawImg || Img == null)
            {
                g.DrawString(this.Text,
                    new System.Drawing.Font(Font.Family, (float)Font.Size),
                    new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                    new System.Drawing.RectangleF(Where.X,
                        Where.Y + ((float)Size.Height - 1) / Factor - (float)Font.Size / 2,
                        (float)Size.Width - 1,
                        (float)Size.Height - 1),
                    new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Center }
                    );
                if (DrawDescription)
                    g.DrawString(this.Description,
                        new System.Drawing.Font(DescriptionFont.Family, (float)DescriptionFont.Size),
                        new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                        new System.Drawing.RectangleF(Where.X,
                            Where.Y + 2 * ((float)Size.Height - 1) / 3 - (float)Font.Size / 2,
                            (float)Size.Width - 1,
                            (float)Size.Height - 1),
                        new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Center }
                        );
            }
            else
            {
                g.DrawImage(Img, new System.Drawing.Rectangle((int)(Where.X + 0 * Size.Width / 8f) + 1, (int)(Where.Y + 0 * Size.Height / 10f) + 1, (int)(8 * Size.Width / 8f) - 1, (int)(8 * Size.Height / 8f) - 1));
                g.DrawAdaptiveString(this.Text,
                                     new System.Drawing.Font(Font.Family, (float)Font.Size),
                                     new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                                     new System.Drawing.RectangleF(Where.X,
                                                                   Where.Y + ((float)Size.Height - 1) * 0.865f,
                                                                   (float)Size.Width - 1,
                                                                   (float)Size.Height * 0.13f),
                                     new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Center }
                                     );
            }
            g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Gray), Where.X, Where.Y, (float)Size.Width - 1, (float)Size.Height - 1);
            
        }

        public void RaiseButtonPressed()
        {
            if (this.ButtonPressed != null)
                this.ButtonPressed(this, null);
        }
    }

    [YAXLib.YAXSerializableType(FieldsToSerialize = YAXLib.YAXSerializationFields.AllFields)]
    [YAXLib.YAXSerializeAs("Button")]
    public class GradientButtonNode
    {
        [YAXLib.YAXAttributeForClass]
        public string Color = "";
        [YAXLib.YAXAttributeForClass]
        public string Font = "";
        [YAXLib.YAXAttributeForClass]
        public string Text = "";
        [YAXLib.YAXAttributeForClass]
        public string Description = "";
        [YAXLib.YAXAttributeForClass]
        public string DescriptionFont = "";
        [YAXLib.YAXAttributeForClass]
        public string Clicked = "";
        [YAXLib.YAXAttributeForClass]
        public string Image = "";

        public GradientButton Makeup(IXwtWrapper Parent)
        {
            GradientButton T = new GradientButton()
            {
                Text = this.Text,
                Description = this.Description
            };
            if (this.Font != "")
                T.Font = Xwt.Drawing.Font.FromName(this.Font);
            if (this.DescriptionFont != "")
                T.DescriptionFont = Xwt.Drawing.Font.FromName(this.DescriptionFont);
            if (this.Color != "")
                T.Color = Xwt.Drawing.Color.FromName(this.Color);
            if (this.Image != "")
                T.Img = System.Drawing.Image.FromStream(Assembly.GetEntryAssembly().GetManifestResourceStream(this.Image));
            WindowController.TryAttachEvent(T, "ButtonPressed", Parent, this.Clicked);
            return T;


        }
    }

}
