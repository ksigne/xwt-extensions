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
                Target.Password = (string)Parent.GetType().GetProperty(this.Source).GetValue(Parent);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source)
                        Xwt.Application.Invoke(() => Target.Password = (string)Parent.GetType().GetProperty(e.PropertyName).GetValue(Parent));
                };
                Target.Changed += (o, e) =>
                {
                    Parent.GetType().GetProperty(this.Source).SetValue(Parent, Target.Password);
                };
            }

            WindowController.TryAttachEvent(Target, "Changed", Parent, Changed);
            WindowController.TryAttachEvent(Target, "Activated", Parent, Activated);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
