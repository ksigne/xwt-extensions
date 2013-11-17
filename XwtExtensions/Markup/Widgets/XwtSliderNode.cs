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
    [YAXSerializeAs("Slider")]
    public class XwtSliderNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public Xwt.Backends.Orientation Orientation = Orientation.Horizontal;
        [YAXAttributeForClass]
        public double Min = 0;
        [YAXAttributeForClass]
        public double Max = 1;
        [YAXAttributeForClass]
        public double Step = 0.1;
        [YAXAttributeForClass]
        public bool ShowLabels = false;
        [YAXAttributeForClass]
        public double Value = 0;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string ValueChanged = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Slider Target;
            if (Orientation == Orientation.Horizontal)
                Target = new Xwt.HSlider();
            else Target = new Xwt.VSlider(); 
            Target.MinimumValue = Min;
            Target.MaximumValue = Max;
            Target.Value = Value;

            //Target.Step = Step;
            //Target.ShowLabels = ShowLabels;

            if (Source != "")
            {
                Target.Value = (double)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.Value = (double)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
                Target.ValueChanged += (o, e) =>
                {
                    Parent.GetType().GetProperty(this.Source).SetValue(Parent, Target.Value);
                };
            }

            WindowController.TryAttachEvent(Target, "ValueChanged", Parent, ValueChanged);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
