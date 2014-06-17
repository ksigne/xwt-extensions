using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Ext.Bindings;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize=YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Markdown")]
    public class XwtTextNode: XwtWidgetNode
    {
        [YAXValueForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Navigated = "";
        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.RichTextView Target = new Xwt.RichTextView();
            Target.LoadText(this.Text, Xwt.Formats.TextFormat.Markdown);

            if (Source != "")
            {
                Target.LoadText((string)PathBind.GetValue(Source, Parent, ""), Xwt.Formats.TextFormat.Markdown);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() =>
                            Target.LoadText((string)PathBind.GetValue(Source, Parent, 0), Xwt.Formats.TextFormat.Markdown)
                            );
                };
            }
            

            WindowController.TryAttachEvent(Target, "NavigateToUrl", Parent, Navigated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
