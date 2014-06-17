using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Frame")]
    public class XwtFrameBoxNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double BorderWidth = 1;
        [YAXAttributeForClass]
        public string Text = null;
        [YAXAttributeForClass]
        public double PaddingLeft = 0;
        [YAXAttributeForClass]
        public double PaddingRight = 0;
        [YAXAttributeForClass]
        public double PaddingTop = 0;
        [YAXAttributeForClass]
        public double PaddingBottom = 0;
        [YAXAttributeForClass]
        public string BorderColor = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.Frame Target;
            Target = new Xwt.Frame()
            {
                PaddingLeft = this.PaddingLeft,
                PaddingRight = this.PaddingRight,
                PaddingTop = this.PaddingTop,
                PaddingBottom = this.PaddingBottom,
                BorderWidth = this.BorderWidth,
                Label = this.Text
            };

            if (this.BorderColor != "")
                Target.BorderColor = Xwt.Drawing.Color.FromName(this.BorderColor);

            if (Items != null && Items.Count() > 0)
                Target.Content = Items[0].Makeup(Parent);
            
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
