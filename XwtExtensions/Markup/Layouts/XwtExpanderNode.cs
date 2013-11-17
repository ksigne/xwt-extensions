using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Expander")]
    public class XwtExpanderNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public bool Expanded = true;
        [YAXAttributeForClass]
        public string Activated = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Expander Target = new Xwt.Expander()
            {
                Label = this.Text,
                Expanded = this.Expanded
            };

            if (Items != null && Items.Count() > 0)
                Target.Content = Items[0].Makeup(Parent);
            WindowController.TryAttachEvent(Target, "ExpandChanged", Parent, Activated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
