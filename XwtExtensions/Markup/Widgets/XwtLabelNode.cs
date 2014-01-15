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
    [YAXSerializeAs("Label")]
    public class XwtLabelNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public string Markup = "";
        [YAXAttributeForClass]
        public string Color = "";
        [YAXAttributeForClass]
        public Alignment Align = Alignment.Start;
        [YAXAttributeForClass]
        public EllipsizeMode Ellipsize = EllipsizeMode.None;
        [YAXAttributeForClass]
        public WrapMode Wrap = WrapMode.None;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string LinkClicked = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Label Target = new Xwt.Label()
            {
                TextAlignment = this.Align,
                Text = this.Text,
                Ellipsize = this.Ellipsize,
                Wrap = this.Wrap
            };
            if (this.Markup != "")
                Target.Markup = this.Markup;
            if (this.Color != "")
                Target.TextColor = Xwt.Drawing.Color.FromName(this.Color);
            //Binding
            if (Source != "")
            {
                Target.Text = (string)PathBind.GetValue(Source, Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Text = (string)PathBind.GetValue(Source, Parent));
                };
            }
            WindowController.TryAttachEvent(Target, "LinkClicked", Parent, LinkClicked);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
