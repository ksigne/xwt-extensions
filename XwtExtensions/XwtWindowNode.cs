using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Xwt.Ext.Markup
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
        public bool Visible = false;
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
        public Xwt.Ext.Markup.Widgets.XwtBoxNode Content;
        [YAXSerializeAs("MainMenu")]
        public XwtMenuNode MainMenu;

        public void Makeup(IXwtWrapper Context, Xwt.Window Target)
        {
            Target.Title = this.Title;
            Target.PaddingLeft = this.PaddingLeft;
            Target.PaddingRight = this.PaddingRight;
            Target.PaddingTop = this.PaddingTop;
            Target.PaddingBottom = this.PaddingBottom;
            Target.Width = this.Width;
            Target.Height = this.Height;
            Target.Decorated = this.Decorated;
            Target.ShowInTaskbar = this.ShowInTaskbar;
            Target.Resizable = this.Resizable;
            Target.Visible = this.Visible;
            Target.FullScreen = this.Fullscreen;
            
            WindowController.TryAttachEvent(Target, "Shown", Context, Shown);
            WindowController.TryAttachEvent(Target, "Hidden", Context, Hidden);
            WindowController.TryAttachEvent(Target, "CloseRequested", Context, CloseRequested);
            WindowController.TryAttachEvent(Target, "BoundsChanged", Context, BoundsChanged);
            if (Content != null)
                Target.Content = Content.Makeup(Context);
            if (MainMenu != null)
                Target.MainMenu = MainMenu.Makeup(Context);
        }
    }
    
}
