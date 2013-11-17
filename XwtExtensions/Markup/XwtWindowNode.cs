using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace XwtExtensions.Markup
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Window")]
    public class XwtWindowNode
    {
        [YAXAttributeForClass()]
        public string Title = "Window";
        [YAXAttributeForClass()]
        public Xwt.WindowLocation InitialLocation = Xwt.WindowLocation.CenterScreen;
        [YAXAttributeForClass()]
        public int PaddingLeft = 10;
        [YAXAttributeForClass()]
        public int PaddingRight = 10;
        [YAXAttributeForClass()]
        public int PaddingTop = 10;
        [YAXAttributeForClass()]
        public int PaddingBottom = 10;
        [YAXAttributeForClass()]
        public int Width = 0;
        [YAXAttributeForClass()]
        public int Height = 0;
        [YAXAttributeForClass()]
        public bool Decorated = true;
        [YAXAttributeForClass()]
        public bool ShowInTaskbar = true;
        [YAXAttributeForClass()]
        public bool Resizable = true;
        [YAXAttributeForClass()]
        public bool Visible = true;
        [YAXAttributeForClass()]
        public bool Fullscreen = false;

        [YAXAttributeForClass()]
        public string Shown = "";
        [YAXAttributeForClass()]
        public string Hidden = "";
        [YAXAttributeForClass()]
        public string CloseRequested = "";
        [YAXAttributeForClass()]
        public string BoundsChanged = "";

        [YAXSerializeAs("Content")]
        public XwtExtensions.Markup.Widgets.XwtBoxNode Content;
        [YAXSerializeAs("MainMenu")]
        public XwtMenuNode MainMenu;

        public Xwt.Window Makeup(WindowWrapper Context)
        {
            Xwt.Window Target = new Xwt.Window()
            {
                Title = this.Title,
                PaddingLeft = this.PaddingLeft,
                PaddingRight = this.PaddingRight,
                PaddingTop = this.PaddingTop,
                PaddingBottom = this.PaddingBottom,
                Width = this.Width,
                Height = this.Height,
                Decorated = this.Decorated,
                ShowInTaskbar = this.ShowInTaskbar,
                Resizable = this.Resizable,
                Visible = this.Visible,
                FullScreen = this.Fullscreen
            };
            WindowController.TryAttachEvent(Target, "Shown", Context, Shown);
            WindowController.TryAttachEvent(Target, "Hidden", Context, Hidden);
            WindowController.TryAttachEvent(Target, "CloseRequested", Context, CloseRequested);
            WindowController.TryAttachEvent(Target, "BoundsChanged", Context, BoundsChanged);
            if (Content != null)
                Target.Content = Content.Makeup(Context);
            if (MainMenu != null)
                Target.MainMenu = MainMenu.Makeup(Context);
            return Target;
        }
    }
    
}
