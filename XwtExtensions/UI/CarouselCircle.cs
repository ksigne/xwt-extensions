using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using Xwt.Motion;

namespace Xwt.Ext.UI
{
    public class CarouselCircle : Xwt.Canvas
    {
        class ButtonLayouter
        {
            public CarouselCircle Parent;

            public int Size
            {
                get
                {
                    return Parent.ButtonSize;
                }
            }

            public int Padding
            {
                get
                {
                    return Parent.Padding;
                }
            }

            public int LeftMargin
            {
                get
                {
                    return Parent.LeftMargin;
                }
            }

            public int TopMargin
            {
                get
                {
                    return Parent.TopMargin;
                }
            }

            public List<GradientButton> Elements
            {
                get
                {
                    return Parent.Buttons;
                }
            }

            public GradientButton PrimaryButton
            {
                get
                {
                    return Elements.FirstOrDefault(X => X.CurrentMode);
                }
            }

            public void Layout()
            {
                int i = 0;
                Elements.ForEach(X =>
                {
                    if (X.CurrentMode)
                        X.Size = new Size(1.5 * Size, 1.5 * Size);
                    else
                        X.Size = new Size(Size, Size);

                    double Angle = ((double)(i++)) / Elements.Count() * 2 * Math.PI;
                    X.Position = 
                        new Point(
                            (Parent.Size.Width - 2 * LeftMargin)*(0.5*Math.Cos(Angle)+0.5) + LeftMargin - X.Size.Width/2,
                            TopMargin + (Parent.Size.Height - 2 * TopMargin)*(0.5*Math.Sin(Angle) + 0.5) - X.Size.Height/2);
                    
                });
            }

            public void MakePrimary(GradientButton B)
            {
                Dictionary<GradientButton, double> BasicWidth = Elements.ToDictionary(k => k, k => k.Size.Width),
                        BasicHeight = Elements.ToDictionary(k => k, k => k.Size.Height),
                        BasicX = Elements.ToDictionary(k => k, k => k.Position.X),
                        BasicY = Elements.ToDictionary(k => k, k => k.Position.Y);
                bool Direction = (B.RawLayoutX > PrimaryButton.RawLayoutX);
                GradientButton P = PrimaryButton;
                    PrimaryButton.CurrentMode = false;
                B.CurrentMode = true;

                Parent.Animate(
                    name: "",
                    length: 300,
                    callback: (X) =>
                    {
                        //Анимация уменьшения предыдущего активного элемента
                        P.Position.X = BasicX[P] + ((double)Size/4 * (X));
                        P.Position.Y = BasicY[P] + ((double)Size/4 * (X));
                        P.Size.Width = BasicWidth[P] + (double)(Size - BasicWidth[P]) * (X);
                        P.Size.Height = BasicHeight[P] + (double)(Size - BasicHeight[P]) * (X);
                        //Анимация увеличения текущего активного элемента
                        B.Position.X = BasicX[B] - ((double)Size / 4 * (X));
                        B.Position.Y = BasicY[B] - ((double)Size / 4 * (X));
                        B.Size.Width = BasicWidth[B] + (double)((1.5 * Size) - BasicWidth[B]) * (X);
                        B.Size.Height = BasicHeight[B] + (double)((1.5 * Size) - BasicHeight[B]) * (X);
                        Parent.QueueDraw();
                    },
                    finished: (X, Y) =>
                    {
                        this.Layout();
                        Parent.QueueDraw();
                    }
                );
            }

            public void Init()
            {
                Elements[new Random().Next(Elements.Count)].CurrentMode = true;
                for (int i = 0; i < Elements.Count(); i++)
                {
                    Elements[i].DrawImg = true;
                    Elements[i].Color = Elements[i].Color.BlendWith(Xwt.Drawing.Colors.Blue, (double)i / Elements.Count);
                }
            }

        }

        public List<GradientButton> Buttons = new List<GradientButton>()
        {
            
        };

        ButtonLayouter Layout;

        public int ButtonSize = 150,
            TopMargin = 130,
            LeftMargin = 130,
            Padding = 20;

        protected override void OnBoundsChanged()
        {
            this.Layout.Layout();
        }

        public void Relayout()
        {
            this.Layout.Layout();
        }

        public CarouselCircle(List<GradientButton> Buttons)
        {
            this.Buttons = Buttons;
            Layout = new ButtonLayouter()
            {
                Parent = this
            };
            Layout.Init();
            Layout.Layout();
            System.Timers.Timer T = new System.Timers.Timer(10000);
            T.Elapsed += (o, e) =>
            {
                Xwt.Application.Invoke(() =>
                {
                    if (ParentWindow != null && ParentWindow.Visible)
                    {
                        Layout.MakePrimary(Layout.Elements[new Random().Next(Layout.Elements.Count())]);
                    }
                });
            };

            //T.Start();
        }

        static bool CheckIfIn(Point MousePosition, GradientButton B)
        {
            if ((MousePosition.X > B.Position.X) &&
                (MousePosition.Y > B.Position.Y) &&
                (MousePosition.X < B.Position.X + B.Size.Width) &&
                (MousePosition.Y < B.Position.Y + B.Size.Height))
                return true;
            return false;
        }


        void DrawGradientButton(Xwt.Drawing.Context G, GradientButton B)
        {
            /*DrawingPath P = new DrawingPath();
            P.Rectangle(new Xwt.Rectangle(B.Position, B.Size));
            LinearGradient gr;
            G.AppendPath(P);
            Pattern pat = gr = new LinearGradient(B.Position.X, B.Position.Y, B.Position.X + B.Size.Width, B.Position.Y + B.Size.Height);
            gr.AddColorStop(0, B.Color.BlendWith(Colors.White, 0.8));
            gr.AddColorStop(0.5, B.Color);
            gr.AddColorStop(1, B.Color.BlendWith(Colors.White, 0.8));
            G.Pattern = pat;
            G.Fill();
            G.SetColor(Xwt.Drawing.Colors.Black);
            G.SetLineWidth(1);
            
            /*G.AppendPath(P);
            G.Stroke();*/ /*
            TextLayout L = new TextLayout()
            {
                Font = B.Font,
                Text = B.Text
            };
            Size TextSize = new Size(0.6 * L.Font.Size * L.Text.Count(), L.Font.Size);
            G.DrawTextLayout(L, new Xwt.Point(B.Position.X + B.Size.Width / 2 - TextSize.Width / 2, B.Position.Y + B.Size.Height / 2 - TextSize.Height / 2));*/


        }

        protected override void OnButtonPressed(ButtonEventArgs args)
        {
            GradientButton B = Buttons.FirstOrDefault(X => CheckIfIn(args.Position, X));
            try
            {
                B.RaiseButtonPressed();
            }
            catch (Exception e)
            {
                Xwt.MessageDialog.ShowError(String.Format("Не удается выполнить {0}: {1}", B.Text, e.Message));
            }
        }

        protected override void OnMouseMoved(MouseMovedEventArgs args)
        {
            GradientButton B = Buttons.FirstOrDefault(X => CheckIfIn(args.Position, X));
            if (B != null && B != Layout.PrimaryButton && !this.AnimationIsRunning(""))
                Layout.MakePrimary(B);
        }
        protected override void OnDraw(Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
        {

            /*ctx.Rectangle(dirtyRect);
            ctx.SetColor(Colors.White);
            ctx.Fill();*/
            System.Drawing.Bitmap B = new System.Drawing.Bitmap(
                (int)(dirtyRect.Width*ParentWindow.Screen.ScaleFactor),
                (int)(dirtyRect.Height*ParentWindow.Screen.ScaleFactor)
                );
            System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(B);
            G.FillRectangle(
                new System.Drawing.SolidBrush(System.Drawing.Color.White),
                new System.Drawing.Rectangle(0, 0, (int)B.Width, (int)B.Height)
            );
            List<GradientButton> S = this.Buttons.OrderBy(X => X.CurrentMode).ToList();
            S.ForEach(X => X.DrawImg = true);
            S.ForEach(X => X.Draw(G, new System.Drawing.PointF((float)X.Position.X, (float)X.Position.Y), this.ParentWindow.Screen.ScaleFactor));
            Xwt.Ext.CanvasSystemDrawing.DrawingExtensions.DrawImage(ctx, B, new Point(0, 0), this.ParentWindow.Screen.ScaleFactor);
        }
    }

    [YAXLib.YAXSerializableType(FieldsToSerialize = YAXLib.YAXSerializationFields.AllFields)]
    [YAXLib.YAXSerializeAs("CarouselCircle")]
    public class CarouselCircleNode : Xwt.Ext.Markup.XwtWidgetNode
    {
        [YAXLib.YAXCollection(YAXLib.YAXCollectionSerializationTypes.Recursive)]
        public List<GradientButtonNode> Nodes;
        public override Widget Makeup(IXwtWrapper Parent)
        {
            List<GradientButton> L = new List<GradientButton>();
            foreach (GradientButtonNode B in Nodes)
            {
                GradientButton T = B.Makeup(Parent);
                L.Add(T);
            }

            Xwt.Ext.UI.CarouselCircle Target = new Xwt.Ext.UI.CarouselCircle(L);

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
