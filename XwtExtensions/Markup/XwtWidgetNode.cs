using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using YAXLib;

namespace XwtExtensions.Markup
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    public abstract class XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Name = "";
        [YAXAttributeForClass]
        public bool Fill = true;
        [YAXAttributeForClass]
        public bool Expand = false;
        [YAXAttributeForClass]
        public int Row = 0;
        [YAXAttributeForClass]
        public int Column = 0;
        [YAXAttributeForClass]
        public double MinWidth = -1;
        [YAXAttributeForClass]
        public double MinHeight = -1;
        [YAXAttributeForClass]
        public double Width = -1;
        [YAXAttributeForClass]
        public double Height = -1;
        [YAXAttributeForClass]
        public string BackgroundColor = "";
        [YAXAttributeForClass]
        public string Font = "";
        [YAXAttributeForClass]
        public double MarginLeft = 0;
        [YAXAttributeForClass]
        public double MarginRight = 0;
        [YAXAttributeForClass]
        public double MarginTop = 0;
        [YAXAttributeForClass]
        public double MarginBottom = 0;
        [YAXAttributeForClass]
        public bool Sensitive = true;
        [YAXAttributeForClass]
        public bool Visible = true;
        [YAXAttributeForClass]
        public Xwt.WidgetPlacement HAlign = Xwt.WidgetPlacement.Fill;
        [YAXAttributeForClass]
        public Xwt.WidgetPlacement VAlign = Xwt.WidgetPlacement.Fill;
        [YAXAttributeForClass]
        public Xwt.CursorType Cursor = Xwt.CursorType.Arrow;
        [YAXAttributeForClass]
        public string TooltipText = null;
        [YAXSerializeAs("ContextMenu")]
        public XwtMenuNode ContextMenu;
        [YAXAttributeForClass]
        public string GotFocus = "";
        [YAXAttributeForClass]
        public string LostFocus = "";
        [YAXAttributeForClass]
        public string MouseEntered = "";
        [YAXAttributeForClass]
        public string MouseExited = "";
        [YAXAttributeForClass]
        public string MouseMoved = "";
        [YAXAttributeForClass]
        public string ButtonPressed = "";
        [YAXAttributeForClass]
        public string ButtonReleased = "";

        protected void InitWidget(Xwt.Widget Widget, WindowWrapper Parent)
        {
            Widget.MinHeight = this.MinHeight;
            Widget.MinWidth = this.MinWidth;
            Widget.WidthRequest = this.Width;
            Widget.HeightRequest = this.Height;
            if (this.BackgroundColor != "")
                Widget.BackgroundColor = Xwt.Drawing.Color.FromName(this.BackgroundColor);
            if (this.Font != "")
                Widget.Font = Xwt.Drawing.Font.FromName(this.Font);
            Widget.TooltipText = this.TooltipText;
            Widget.MarginLeft = this.MarginLeft;
            Widget.MarginRight = this.MarginRight;
            Widget.MarginTop = this.MarginTop;
            Widget.MarginBottom = this.MarginBottom;
            Widget.Sensitive = this.Sensitive;
            Widget.Visible = this.Visible;
            Widget.Cursor = this.Cursor;
            Widget.HorizontalPlacement = this.HAlign;
            Widget.VerticalPlacement = this.VAlign;
            if (this.ContextMenu != null)
            {
                Widget.ButtonPressed += (o, e) =>
                {
                    if (e.Button == Xwt.PointerButton.Right)
                    {
                        Xwt.Menu M = new Xwt.Menu();
                        this.ContextMenu.Subitems.ForEach(X => M.Items.Add(X.Makeup(Parent)));
                        M.Popup();
                    }
                };
            }
            WindowController.TryAttachEvent(Widget, "MouseEntered", Parent, this.MouseEntered);
            WindowController.TryAttachEvent(Widget, "MouseExited", Parent, this.MouseExited);
            WindowController.TryAttachEvent(Widget, "MouseMoved", Parent, this.MouseMoved);
            WindowController.TryAttachEvent(Widget, "GotFocus", Parent, this.GotFocus);
            WindowController.TryAttachEvent(Widget, "LostFocus", Parent, this.LostFocus);
            WindowController.TryAttachEvent(Widget, "ButtonPressed", Parent, this.ButtonPressed);
            WindowController.TryAttachEvent(Widget, "ButtonReleased", Parent, this.ButtonReleased);
            if (this.Name != "")
            {
                Parent.Widgets.Add(this.Name, Widget);
                if (Parent.GetType().GetField(this.Name) != null)
                    Parent.GetType().GetField(this.Name).SetValue(Parent, Widget);
                WindowController.RegisterWidget(this.Name, Parent, Widget);
            }
        }

        public abstract Xwt.Widget Makeup(WindowWrapper Parent);
    }
}
