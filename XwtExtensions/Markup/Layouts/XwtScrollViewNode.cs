using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xwt;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("ScrollView")]
    public class XwtScrollViewNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public ScrollPolicy HScrollPolicy = ScrollPolicy.Automatic;
        [YAXAttributeForClass]
        public ScrollPolicy VScrollPolicy = ScrollPolicy.Automatic;
        [YAXAttributeForClass]
        public bool BorderVisible = false;
        [YAXAttributeForClass]
        public string VisibleRectChanged = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.ScrollView Target = new Xwt.ScrollView()
            {
                HorizontalScrollPolicy = this.HScrollPolicy,
                VerticalScrollPolicy = this.VScrollPolicy,
                BorderVisible = this.BorderVisible
            };

            if (Items != null && Items.Count() > 0)
                Target.Content = Items[0].Makeup(Parent);

            WindowController.TryAttachEvent(Target, "VisibleRectChanged", Parent, VisibleRectChanged);
            
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
