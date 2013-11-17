using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Backends;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("ProgressBar")]
    public class XwtProgressBar : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double Value = 0;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public bool Indeterminate = false;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.ProgressBar Target = new Xwt.ProgressBar()
            {
                Fraction = this.Value,
                Indeterminate = this.Indeterminate
            };

            //Binding
            if (Source != "")
            {
                Target.Fraction = (double)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.Fraction = (double)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
            }

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
