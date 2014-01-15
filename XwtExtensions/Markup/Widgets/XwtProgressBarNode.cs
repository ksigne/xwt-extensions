using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Backends;
using XwtExtensions.Bindings;
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
                Target.Fraction = (double)PathBind.GetValue(Source, Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Fraction = (double)PathBind.GetValue(Source, Parent));
                };
            }

        

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
