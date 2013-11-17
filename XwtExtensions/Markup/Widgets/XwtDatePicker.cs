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
    [YAXSerializeAs("DatePicker")]
    public class XwtDatePickerNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public DatePickerStyle Style = DatePickerStyle.Date;
        [YAXAttributeForClass]
        public string Value = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Changed = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.DatePicker Target = new Xwt.DatePicker()
            {
                Style = this.Style
            };
            if (this.Value != "")
            {
                try {
                    DateTime Val = DateTime.Parse(Value);
                    Target.DateTime = Val;
                }
                catch { }
            }
            //Binding
            if (Source != "")
            {
                Target.DateTime = (DateTime)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.DateTime = (DateTime)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
                Target.ValueChanged += (o, e) =>
                {
                    Parent.GetType().GetProperty(this.Source).SetValue(Parent, Target.DateTime);
                };
            }

            WindowController.TryAttachEvent(Target, "ValueChanged", Parent, Changed);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
