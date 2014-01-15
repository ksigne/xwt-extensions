using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using Xwt.Motion;
using XwtExtensions.CanvasSystemDrawing;

namespace XwtExtensions.UI
{
   
    public class CarouselTable : Xwt.Canvas
    {
        class ButtonLayouter
        {
            public CarouselTable Parent;

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

        public CarouselTable(List<GradientButton> Buttons)
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
            T.Start();

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
            //G.DrawImage(B.Draw(), B.Position);
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
            System.Drawing.Bitmap B = new System.Drawing.Bitmap((int)dirtyRect.Width, (int)dirtyRect.Height);
            System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(B);

            G.FillRectangle(
                new System.Drawing.SolidBrush(System.Drawing.Color.White),
                new System.Drawing.Rectangle(0, 0, (int)dirtyRect.Width - 1, (int)dirtyRect.Height - 1)
            );
            List<GradientButton> S = this.Buttons.OrderBy(X => X.CurrentMode).ToList();
            S.ForEach(X =>X.Draw(G, new System.Drawing.PointF((float)X.Position.X, (float)X.Position.Y)));
            ctx.DrawImage(B, new Point(0, 0));
        }
    }

    [YAXLib.YAXSerializableType(FieldsToSerialize = YAXLib.YAXSerializationFields.AllFields)]
    [YAXLib.YAXSerializeAs("CarouselTable")]
    public class CarouselTableNode: XwtExtensions.Markup.XwtWidgetNode
    {
        [YAXLib.YAXCollection(YAXLib.YAXCollectionSerializationTypes.Recursive)]
        public List<GradientButtonNode> Nodes;
        public override Widget Makeup(WindowWrapper Parent)
        {
            List<GradientButton> L = new List<GradientButton>();
            foreach (GradientButtonNode B in Nodes)
            {
                GradientButton T = B.Makeup(Parent);
                L.Add(T);
            }

            XwtExtensions.UI.CarouselTable Target = new XwtExtensions.UI.CarouselTable(L);
           
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
