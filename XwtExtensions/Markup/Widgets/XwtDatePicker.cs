using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using XwtExtensions.Bindings;
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
                Target.DateTime = (DateTime)PathBind.GetValue(Source, Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.DateTime = (DateTime)PathBind.GetValue(Source, Parent));
                };
                Target.ValueChanged += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.DateTime);
                };
            }

            WindowController.TryAttachEvent(Target, "ValueChanged", Parent, Changed);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
