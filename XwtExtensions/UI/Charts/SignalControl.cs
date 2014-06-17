using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xwt.Ext.Bindings;
using Xwt.Ext.CanvasSystemDrawing;
using YAXLib;

namespace Xwt.Ext.UI.Charts
{
    public struct SignalControlTheme
    {
        public string FontFace { get; set; }
        public int FontSize { get; set; }
        public int XGrid { get; set; }
        public int YGrid { get; set; }
        public float GridThickness { get; set; }
        public float GraphThickness { get; set; }
        public double Sharping { get; set; }
        public Xwt.Drawing.Color BackgroundColor { get; set; }
        public Xwt.Drawing.Color SelectionColor { get; set; }
        public Xwt.Drawing.Color SelectionLineColor { get; set; }
        public Xwt.Drawing.Color GridColor { get; set; }
        public Xwt.Drawing.Color GraphColor { get; set; }

        public static SignalControlTheme Default = new SignalControlTheme() {
          FontFace = "Arial",
          FontSize = 8,
          XGrid = 10,
          YGrid = 10,
          GridThickness = 1,
          GraphThickness = 2,
          Sharping = 1,
          BackgroundColor = Xwt.Drawing.Colors.White,
          SelectionColor = Xwt.Drawing.Colors.Azure,
          SelectionLineColor = Xwt.Drawing.Colors.Yellow,
          GridColor = Xwt.Drawing.Colors.LightGray,
          GraphColor = Xwt.Drawing.Colors.Black
        };
    }

    public class SignalControl : Xwt.Canvas
    {
        public List<double> Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                this.XAxis = GenerateXAxis(_Data, Freq);
                ResetBounds();
            }
        }
        public List<string> XAxis { get; set; }

        public double Max { get; set; }
        public double Min { get; set; }

        public double Freq {
            get
            {
                return _Freq;
            }
            set
            {
                _Freq = value;
                this.XAxis = GenerateXAxis(_Data, _Freq);
            }
        }
        double _Freq = 1;

        public SignalControlTheme CurrentTheme { get; set; }

        List<double> _Data = new List<double> { 0, 1 };
        bool Selecting = false;
        bool Resize = false;

        public int SelectionStart;
        public int SelectionEnd;

        int _MarginLeft = 40;
        int _MarginRight = 0;
        int _MarginBottom = 0;
        int _MarginTop = 0;

        double StartY;
        double EndY;

        int _SelectionEnd;

        public event Action SelectionChanged;

        public void RaiseSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged();
        }

        public List<double> Selection
        {
            get
            {
                if (SelectionStart != SelectionEnd)
                    return Data.Skip(SelectionStart).Take(SelectionEnd - SelectionStart).ToList();
                else return Data.ToList();
            }
        }

        void ResetBounds()
        {
            if (_Data.Count > 0)
            {
                this.Max = _Data.Max() + 0.1;
                this.Min = _Data.Min() - 0.1;
            }
            else
            {
                this.Max = 1;
                this.Min = 0;
            }
        }

        protected override void OnDraw(Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
        {
            int width = (int)this.Size.Width, height = (int)this.Size.Height;
            ctx.DrawImage(Build(width, height), new Xwt.Point(0, 0), this.ParentWindow.Screen.ScaleFactor);
        }

        protected override void OnButtonPressed(Xwt.ButtonEventArgs args)
        {
            if (args.Button == Xwt.PointerButton.Left)
            {
                int width = (int)this.Size.Width, height = (int)this.Size.Height;
                if (args.X > _MarginLeft)
                {
                    Selecting = true;
                    SelectionStart = XToData((int)args.X, width);
                    SelectionEnd = SelectionStart;
                }
                else
                {
                    Resize = true;
                    StartY = args.Y;
                }
            }

            base.OnButtonPressed(args);
        }

        protected override void OnButtonReleased(Xwt.ButtonEventArgs args)
        {
            int width = (int)this.Size.Width, height = (int)this.Size.Height;
            if (args.Button == Xwt.PointerButton.Left)
            {
                if (Selecting)
                {
                    Selecting = false;
                    SelectionEnd = XToData((int)args.X, width);
                    if (SelectionEnd < SelectionStart)
                    {
                        int T = SelectionEnd;
                        SelectionEnd = SelectionStart;
                        SelectionStart = T;
                    }
                }
                if (Resize)
                {
                    Resize = false;
                }
                RaiseSelectionChanged();
                this.QueueDraw();
            }
        }

        protected override void OnMouseMoved(Xwt.MouseMovedEventArgs args)
        {
            int width = (int)this.Size.Width, height = (int)this.Size.Height;
            if (Selecting)
            {
                _SelectionEnd = XToData((int)args.X, width);
            }
            if (Resize)
            {
                EndY = args.Y;
                double S = YToData(EndY, height) - YToData(StartY, height);
                double Max = this.Data.Max() + S;
                double Min = this.Data.Min() - S;
                if (Max > Min)
                {
                    this.Max = Max;
                    this.Min = Min;
                }

            }
            this.QueueDraw();
        }

        public SignalControl()
        {
            //field initialization
            this.CurrentTheme = SignalControlTheme.Default;
            //here goes events
            ResetBounds();
        }

        List<string> GenerateXAxis(List<double> From, double Freq)
        {
            return Enumerable.Range(0, From.Count).Select(X =>
            {
                if (X / Freq < 60)
                    return ((double)X / Freq).ToString("0.00");
                else return (new TimeSpan(0, 0, (int)(X / Freq))).ToString();
            }).ToList();
        }

        int XToData(int X, int width)
        {
            double S = ((double)(X - _MarginLeft) / ((double)(width - _MarginLeft - _MarginRight) / Data.Count));
            if (S < 0) S = 0;
            if (S > Data.Count) S = Data.Count;
            if (S - Math.Floor(S) > 0.5) return (int)Math.Floor(S) + 1; else return (int)Math.Floor(S);
        }

        float DataToY(double val, int height)
        {
            return (float)((1 - (val - Min) / (Max - Min)) * (height - _MarginTop - _MarginBottom)) + 1;
        }

        float YToData(double y, int height)
        {
            return (float)((Min - Max) * y - 1 * Min + (height - 1) * Max) / (height - 1 - 1);
        }

        float DataToX(int D, int width)
        {
            if (Data.Count > 0)
                return _MarginLeft+(int)((double)D * (width - _MarginLeft - _MarginRight) / Data.Count);
            else return 1 + (width - _MarginLeft - _MarginRight) / 2;
        }

        void BuildGrid(Graphics G, int width, int height)
        {
            int S = Math.Min(CurrentTheme.XGrid + 1, Data.Count + 1);
            if (Data.Count > CurrentTheme.XGrid && CurrentTheme.XGrid > 0)
                Enumerable.Range(0, S).Select(X => X * Data.Count / S).ToList().ForEach(
                    X =>
                    {
                        G.DrawLine(new Pen(CurrentTheme.GridColor.ToWindowsColor(), CurrentTheme.GridThickness),
                            new PointF(DataToX(X, width), 0),
                            new PointF(DataToX(X, width), height));
                        string ToWrite = X.ToString();
                        if (X != Data.Count)
                        {
                            if ((XAxis != null) && (XAxis.Count == Data.Count))
                                ToWrite = XAxis[X];
                        }
                        else ToWrite = "";
                        G.DrawString(ToWrite, new Font(CurrentTheme.FontFace, CurrentTheme.FontSize), new SolidBrush(CurrentTheme.GridColor.ToWindowsColor()), new PointF(DataToX(X, width), height - 20));
                    });
            if (CurrentTheme.YGrid > 0)
                Enumerable.Range(0, CurrentTheme.YGrid + 1).Select(X => Min + (Max - Min) / CurrentTheme.YGrid * X).ToList().ForEach(
                    X =>
                    {
                        G.DrawLine(new Pen(CurrentTheme.GridColor.ToWindowsColor(), CurrentTheme.GridThickness),
                        new PointF(0, DataToY(X, height)),
                        new PointF(width, DataToY(X, height)));
                        G.DrawString(X.ToString("0.000"), new Font(CurrentTheme.FontFace, CurrentTheme.FontSize), new SolidBrush(CurrentTheme.GridColor.ToWindowsColor()), new PointF(0, DataToY(X, height)));
                    });
        }

        public Bitmap Build(int width, int height)
        {
            this.XAxis = GenerateXAxis(_Data, Freq);
            if ((width < 10) || (height < 10))
            {
                return new Bitmap(1, 1);
            }
            Bitmap Target = new Bitmap(width, height);
            Graphics G = Graphics.FromImage(Target);
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            //G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float Step = (float)(width - _MarginLeft - _MarginRight) / Data.Count;

            G.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            G.FillRectangle(new SolidBrush(CurrentTheme.BackgroundColor.ToWindowsColor()), new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(width, height)));
            if (!Selecting)
                G.FillRectangle(new SolidBrush(CurrentTheme.SelectionColor.ToWindowsColor()), new RectangleF(new PointF(DataToX(SelectionStart, width), 0), new SizeF(DataToX(SelectionEnd, width) - DataToX(SelectionStart, width), height)));

            BuildGrid(G, width, height);
            if (Data.Count > 0)
            {
                int x = 0; float y = DataToY(Data[0], height);
                List<double> Graph = Data;
                if (Data.Count / CurrentTheme.Sharping > width - _MarginLeft - _MarginRight)
                {
                    int Factor = (int)(Data.Count / ((width - _MarginRight - _MarginLeft) * CurrentTheme.Sharping));
                    int n = 0;
                    Graph = Data.Where(X => (n++) % Factor == 0).ToList();
                    Step = (float)(width - _MarginLeft - _MarginRight) / Graph.Count;
                }
                Graph.ForEach((X) =>
                    G.DrawLine(new Pen(CurrentTheme.GraphColor.ToWindowsColor(), CurrentTheme.GraphThickness), new PointF(_MarginLeft + Step * x, y), new PointF(_MarginLeft + Step * (++x), y = DataToY(X, height)))
                );
            }

            if (Selecting)
            {
                G.DrawLine(new Pen(CurrentTheme.SelectionLineColor.ToWindowsColor(), 3), new PointF(DataToX(SelectionStart, width), 0), new PointF(DataToX(SelectionStart, width), height));
                G.DrawLine(new Pen(CurrentTheme.SelectionLineColor.ToWindowsColor(), 3), new PointF(DataToX(_SelectionEnd, width), 0), new PointF(DataToX(_SelectionEnd, width), height));
            }

            return Target;
        }
    }

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Signal")]
    public class SignalWidgetNode: Xwt.Ext.Markup.XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string DataSource = "";
        [YAXAttributeForClass]
        public string ThemeSource = "";
        [YAXAttributeForClass]
        public string FreqSource = "";

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            SignalControl Target = new SignalControl();
            if (DataSource != "")
            {
                Target.Data = (List<double>)PathBind.GetValue(DataSource, Parent, new List<double> () {0});
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.DataSource.Split('.')[0])
                        Xwt.Application.Invoke(() =>
                        {
                            Target.Data = (List<double>)PathBind.GetValue(DataSource, Parent, new List<double> () {0});
                            Target.QueueDraw();
                        });
                };
            }

            if (ThemeSource != "")
            {
                Target.CurrentTheme = (SignalControlTheme)PathBind.GetValue(ThemeSource, Parent, SignalControlTheme.Default);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.ThemeSource.Split('.')[0])
                        Xwt.Application.Invoke(() =>
                        {
                            Target.CurrentTheme = (SignalControlTheme)PathBind.GetValue(ThemeSource, Parent, SignalControlTheme.Default);
                            Target.QueueDraw();
                        });
                };
            }

            if (FreqSource != "")
            {
                Target.Freq = (double)PathBind.GetValue(FreqSource, Parent, 0);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.FreqSource.Split('.')[0])
                        Xwt.Application.Invoke(() =>
                        {
                            Target.Freq = (double)PathBind.GetValue(FreqSource, Parent, 0);
                            Target.QueueDraw();
                        });
                };
            }

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
