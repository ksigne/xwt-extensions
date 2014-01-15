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
    [YAXSerializeAs("Password")]
    public class XwtPasswordNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string PlaceholderText = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Changed = "";
        [YAXAttributeForClass]
        public string Activated = "";

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.PasswordEntry Target = new Xwt.PasswordEntry()
            {
                PlaceholderText = this.PlaceholderText
            };

            //Binding
            if (Source != "")
            {
                Target.Password = (string)PathBind.GetValue(Source, Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Password = (string)PathBind.GetValue(Source, Parent));
                };
                Target.Changed += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.Password);
                };
            }

            WindowController.TryAttachEvent(Target, "Changed", Parent, Changed);
            WindowController.TryAttachEvent(Target, "Activated", Parent, Activated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
