using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Ext.Bindings;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
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

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
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
            WindowController.TryAttachEvent(Target, "Changed", Parent, Changed);
            WindowController.TryAttachEvent(Target, "Activated", Parent, Activated);
            if (Source != "")
            {
                Target.Text = (string)PathBind.GetValue(Source, Parent, "");
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Text = (string)PathBind.GetValue(Source, Parent, ""));
                };
                Target.Changed += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.Text);
                };
            }

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
