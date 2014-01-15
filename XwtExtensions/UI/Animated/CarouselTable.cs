using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using Xwt.Motion;

namespace ConsoleApplication2
{
    public class DrawKinetic : Xwt.Canvas
    {
        public class GradientButton
        {
            public Point Position;
            public Size Size;
            public Color Color;
            public Font Font;
            public string Text;
            public int RawLayoutX;
            public int RawLayoutY;
            public bool CurrentMode = false;
            public Action ButtonPressed;
        }

        class ButtonLayouter
        {
            public DrawKinetic Parent;

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
                Elements.ForEach(X =>
                {
                    X.Position = new Point(LeftMargin + X.RawLayoutX * (Size + Padding), TopMargin + X.RawLayoutY * (Size + Padding));
                    if (X.CurrentMode)
                        X.Size = new Size(2 * Size + Padding, 2 * Size + Padding);
                    else
                        X.Size = new Size(Size, Size);
                });
            }

            public Point FirstEmpty()
            {
                int i = 0;
                int Px = PrimaryButton.RawLayoutX, Py = PrimaryButton.RawLayoutY;
                while (true)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        GradientButton B = Elements.FirstOrDefault(X => X.RawLayoutX == i && X.RawLayoutY == j);
                        if (B == null)
                        {
                            if (!BadZone(i, j))
                                return new Point(i, j);
                        }
                    }
                    i++;
                }
            }

            public bool BadZone(int x, int y)
            {
                int i = 0;
                int Px = PrimaryButton.RawLayoutX, Py = PrimaryButton.RawLayoutY;
                if (!((x - Px) >= 0 && (x - Px) < 2 && (y - Py) >= 0 && (y - Py) < 2))
                    return false;
                else return true;
            }

            public void MakePrimary(GradientButton B)
            {
                Dictionary<GradientButton, double> BasicWidth = Elements.ToDictionary(k => k, k => k.Size.Width),
                        BasicHeight = Elements.ToDictionary(k => k, k => k.Size.Height),
                        BasicX = Elements.ToDictionary(k => k, k => k.Position.X),
                        BasicY = Elements.ToDictionary(k => k, k => k.Position.Y);
                bool Direction = (B.RawLayoutX > PrimaryButton.RawLayoutX);
                GradientButton P = PrimaryButton;
                int B_RawLayoutY = B.RawLayoutY, P_RawLayoutY = P.RawLayoutY, P_RawLayoutX = P.RawLayoutX;

                PrimaryButton.CurrentMode = false;
                B.CurrentMode = true;
                B.RawLayoutY = 0;
                if (Direction) B.RawLayoutX--;
                {
                    Elements.ForEach(Z =>
                    {
                        if (Z.CurrentMode != true && BadZone(Z.RawLayoutX, Z.RawLayoutY))
                        {
                            Point NewPlace = FirstEmpty();
                            Z.RawLayoutX = (int)NewPlace.X;
                            Z.RawLayoutY = (int)NewPlace.Y;
                        }
                    });
                }

                Parent.Animate(
                    name: "",
                    callback: (X) =>
                    {
                        //Анимация уменьшения предыдущего активного элемента
                        if (P.RawLayoutX - P_RawLayoutX > 0)
                            P.Position.X = BasicX[P] - (double)((Size) - BasicWidth[P]) * (X);
                        if (P.RawLayoutY - P_RawLayoutY > 0)
                            P.Position.Y = BasicY[P] - (double)((Size) - BasicHeight[P]) * (X);
                        P.Size.Width = BasicWidth[P] + (double)(Size - BasicWidth[P]) * (X);
                        P.Size.Height = BasicHeight[P] + (double)(Size - BasicHeight[P]) * (X);
                        //Анимация увеличения текущего активного элемента
                        if (Direction)
                            B.Position.X = BasicX[B] - (double)((2 * Size + Padding) - BasicWidth[B]) * (X);
                        if (B_RawLayoutY == 1)
                            B.Position.Y = BasicY[B] - (double)((2 * Size + Padding) - BasicHeight[B]) * (X);
                        B.Size.Width = BasicWidth[B] + (double)((2 * Size + Padding) - BasicWidth[B]) * (X);
                        B.Size.Height = BasicHeight[B] + (double)((2 * Size + Padding) - BasicHeight[B]) * (X);
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
                int c = 0;
                GradientButton big = Elements[2*(new Random().Next(Elements.Count()/2))];
                Elements.ForEach(X =>
                {
                    if (X == big)
                    {
                        X.CurrentMode = true;
                        X.RawLayoutX = c / 2;
                        X.RawLayoutY = 0;
                        c += 4;
                    }
                    else
                    {
                        X.CurrentMode = false;
                        X.RawLayoutX = c / 2;
                        X.RawLayoutY = (c++) % 2;
                    }
                });
            }

        }

        public List<GradientButton> Buttons = new List<GradientButton>()
        {
            
        };

        ButtonLayouter Layout;

        public int ButtonSize = 150,
            TopMargin = 30,
            LeftMargin = 30,
            Padding = 20;

        public DrawKinetic(List<GradientButton> Buttons)
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
                    Layout.MakePrimary(Layout.Elements[new Random().Next(Layout.Elements.Count())]);
                });
            };
            //T.Start();

            int ColumnCount = (int)Math.Ceiling((double)Buttons.Count()/2);
            this.WidthRequest = 2 * LeftMargin + (ColumnCount+1) * ButtonSize + (ColumnCount) * Padding;
            this.HeightRequest = 2 * LeftMargin + 2 * ButtonSize + Padding;
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
            DrawingPath P = new DrawingPath();
            P.Rectangle(new Xwt.Rectangle(B.Position, B.Size));
            LinearGradient gr;
            G.AppendPath(P);
            Pattern pat = gr = new LinearGradient(B.Position.X, B.Position.Y, B.Position.X + B.Size.Width, B.Position.Y + B.Size.Height);
            gr.AddColorStop(0, B.Color.BlendWith(Colors.White, 0.8));
            gr.AddColorStop(0.5, B.Color);
            gr.AddColorStop(1, B.Color.BlendWith(Colors.White, 0.8));
            G.Pattern = pat;
            G.Fill();
            G.AppendPath(P);
            G.SetColor(Xwt.Drawing.Colors.Black);
            G.SetLineWidth(1);
            G.Stroke();
            TextLayout L = new TextLayout()
            {
                Font = B.Font,
                Text = B.Text
            };
            Size TextSize = new Size(0.6 * L.Font.Size * L.Text.Count(), L.Font.Size);
            G.DrawTextLayout(L, new Xwt.Point(B.Position.X + B.Size.Width / 2 - TextSize.Width / 2, B.Position.Y + B.Size.Height / 2 - TextSize.Height / 2));
        }

        protected override void OnButtonPressed(ButtonEventArgs args)
        {
            GradientButton B = Buttons.FirstOrDefault(X => CheckIfIn(args.Position, X));
            try
            {
                B.ButtonPressed();
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

            ctx.Rectangle(dirtyRect);
            ctx.SetColor(Colors.White);
            ctx.Fill();
            this.Buttons.ForEach(X => DrawGradientButton(ctx, X));
        }
    }
}
