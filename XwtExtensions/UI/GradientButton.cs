using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace XwtExtensions.UI
{
    public class GradientButton
    {
        public Point Position;
        public Size Size;
        public Color Color;
        public Font Font = Xwt.Drawing.Font.SystemFont;
        public Font DescriptionFont = Xwt.Drawing.Font.SystemFont;
        public string Text = "";
        public string Description = "";
        public int RawLayoutX;
        public int RawLayoutY;
        public bool CurrentMode = false;
        public string Clicked = "";
        public event EventHandler ButtonPressed;

        public void Draw(System.Drawing.Graphics g, System.Drawing.PointF Where)
        {
            
            System.Drawing.Pen penBlack = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
            //Рисуем прямоугольник
            System.Drawing.Brush brushGradient = new System.Drawing.Drawing2D.LinearGradientBrush(
                new System.Drawing.PointF(Where.X,Where.Y),
                new System.Drawing.PointF(Where.X+(float)Size.Width-1, Where.Y+(float)Size.Height-1),
                XwtExtensions.CanvasSystemDrawing.DrawingExtensions.ToWindowsColor(this.Color),
                XwtExtensions.CanvasSystemDrawing.DrawingExtensions.ToWindowsColor(this.Color.BlendWith(Colors.White, 0.8))
            );
            g.FillRectangle(brushGradient, Where.X, Where.Y, (float)Size.Width-1, (float)Size.Height-1);
            g.DrawRectangle(penBlack, Where.X, Where.Y, (float)Size.Width-1, (float)Size.Height-1);
            float Factor = (this.Description != "") ? 3 : 2;
            g.DrawString(this.Text,
                new System.Drawing.Font(Font.Family, (float)Font.Size),
                new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.RectangleF(Where.X,
                    Where.Y + ((float)Size.Height - 1)/Factor - (float)Font.Size/2,
                    (float)Size.Width-1,
                    (float)Size.Height-1),
                new System.Drawing.StringFormat() {Alignment = System.Drawing.StringAlignment.Center}
                );
            g.DrawString(this.Description,
                new System.Drawing.Font(DescriptionFont.Family, (float)DescriptionFont.Size),
                new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.RectangleF(Where.X,
                    Where.Y + 2*((float)Size.Height - 1) / 3 - (float)Font.Size / 2,
                    (float)Size.Width - 1,
                    (float)Size.Height - 1),
                new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Center }
                );
            
            
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

        public GradientButton Makeup(WindowWrapper Parent)
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
            WindowController.TryAttachEvent(T, "ButtonPressed", Parent, this.Clicked);
            return T;
        }
    }

}
