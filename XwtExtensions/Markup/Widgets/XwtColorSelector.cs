using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("ColorSelector")]
    public class XwtColorSelectorNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public bool SupportsAlpha = false;
        [YAXAttributeForClass]
        public string Value = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Changed = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.ColorSelector Target = new Xwt.ColorSelector()
            {
                SupportsAlpha = this.SupportsAlpha
            };
            if (this.Value != "")
                Target.Color = Xwt.Drawing.Color.FromName(this.Value);
            //Binding
            if (Source != "")
            {
                Target.Color = (Xwt.Drawing.Color)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.Color = (Xwt.Drawing.Color)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
                Target.ColorChanged += (o, e) =>
                {
                    Parent.GetType().GetProperty(this.Source).SetValue(Parent, Target.Color);
                };
            }

            WindowController.TryAttachEvent(Target, "ColorChanged", Parent, Changed);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
