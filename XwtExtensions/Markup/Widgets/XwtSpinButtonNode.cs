using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XwtExtensions.Bindings;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize=YAXSerializationFields.AllFields)]
    [YAXSerializeAs("SpinButton")]
    public class XwtSpinButtonNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double ClimbRate;
        [YAXAttributeForClass]
        public int Digits;
        [YAXAttributeForClass]
        public double Value;
        [YAXAttributeForClass]
        public bool Wrap = false;
        [YAXAttributeForClass]
        public double Min = 0;
        [YAXAttributeForClass]
        public double Max = 1;
        [YAXAttributeForClass]
        public double Step = 0.1;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string ValueChanged = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.SpinButton Target = new Xwt.SpinButton()
            {
                ClimbRate = this.ClimbRate,
                Digits = this.Digits,
                Wrap = this.Wrap,
                MinimumValue = this.Min,
                MaximumValue = this.Max,
                IncrementValue = this.Step,
                Value = this.Value
            };

            if (Source != "")
            {
                Target.Value = (double)PathBind.GetValue(Source, Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Value = (double)PathBind.GetValue(Source, Parent));
                };
                Target.ValueChanged += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.Value);
                };
            }

            WindowController.TryAttachEvent(Target, "ValueChanged", Parent, ValueChanged);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
