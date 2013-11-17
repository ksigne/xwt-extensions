using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize=YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Button")]
    public class XwtButton: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public Xwt.ButtonStyle Style;
        [YAXAttributeForClass]
        public Xwt.ButtonType Type;
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public bool UseMnemonic = false;
        [YAXAttributeForClass]
        public string Clicked = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Button Target = new Xwt.Button(this.Text)
            {
                Style = this.Style,
                Type = this.Type,
                UseMnemonic = this.UseMnemonic
            };
            WindowController.TryAttachEvent(Target, "Clicked", Parent, Clicked);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
