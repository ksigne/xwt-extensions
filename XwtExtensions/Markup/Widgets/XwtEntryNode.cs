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
    [YAXSerializeAs("Entry")]
    public class XwtEntryNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public Alignment Align = Alignment.Start;
        [YAXAttributeForClass]
        public string PlaceholderText = "";
        [YAXAttributeForClass]
        public bool ReadOnly = false;
        [YAXAttributeForClass]
        public bool ShowFrame = true;
        [YAXAttributeForClass]
        public bool Multiline = true;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Changed = "";
        [YAXAttributeForClass]
        public string Activated = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.TextEntry Target = new Xwt.TextEntry()
            {
                TextAlignment = this.Align,
                PlaceholderText = this.PlaceholderText,
                ShowFrame = this.ShowFrame,
                MultiLine = this.Multiline,
            };
            if (this.Text != "")
                Target.Text = this.Text;
            if (this.ReadOnly == true)
                Target.ReadOnly = true;
            //Binding
            if (Source != "")
            {
                Target.Text = (string)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.Text = (string)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
                Target.Changed += (o, e) =>
                {
                    Parent.GetType().GetProperty(this.Source).SetValue(Parent, Target.Text);
                };
            }
            WindowController.TryAttachEvent(Target, "Changed", Parent, Changed);
            WindowController.TryAttachEvent(Target, "Activated", Parent, Activated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
