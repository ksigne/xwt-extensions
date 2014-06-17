using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Backends;
using Xwt.Ext.Bindings;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
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

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
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
                Target.Value = (double)PathBind.GetValue(Source, Parent, 0);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Value = (double)PathBind.GetValue(Source, Parent, 0));
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
