using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Ext.Bindings;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("CheckBox")]
    public class XwtCheckBox : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public CheckBoxState State = CheckBoxState.Off;
        [YAXAttributeForClass]
        public bool AllowMixed = false;
        [YAXAttributeForClass]
        public string Toggled = "";
        [YAXAttributeForClass]
        public string Clicked = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Content;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.CheckBox Target = new Xwt.CheckBox()
            {
                Label = Text,
                State = State,
                AllowMixed = AllowMixed,
            };
            //Creating layouts
            if (Content != null && Content.Count() > 0)
                Target.Content = Content[0].Makeup(Parent);
            //Attaching events
            WindowController.TryAttachEvent(Target, "Toggled", Parent, Toggled);
            WindowController.TryAttachEvent(Target, "Clicked", Parent, Clicked);
            //Making binding
            if (Source != "")
            {
                Target.Active = (bool)PathBind.GetValue(Source, Parent, false);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Active = (bool)PathBind.GetValue(Source, Parent, false));
                };
                Target.Toggled += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.Active);
                };
            }
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
