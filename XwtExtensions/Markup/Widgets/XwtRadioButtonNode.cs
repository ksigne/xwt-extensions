using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize=YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Radio")]
    public class XwtRadioNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public string Group = "";
        [YAXAttributeForClass]
        public string Clicked = "";
        [YAXAttributeForClass]
        public string Activated = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Content;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.RadioButton Target = new Xwt.RadioButton(this.Text);
            if (this.Group != "")
            {
                if (!WindowController.RadioGroups.ContainsKey(this.Group))
                    WindowController.RadioGroups.Add(this.Group, new Xwt.RadioButtonGroup());
                Target.Group = WindowController.RadioGroups[this.Group];
            }
            if (Content != null && Content.Count() > 0)
                Target.Content = Content[0].Makeup(Parent);
            WindowController.TryAttachEvent(Target, "Clicked", Parent, Clicked);
            WindowController.TryAttachEvent(Target, "Activated", Parent, Activated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
