﻿using System;
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
    [YAXSerializeAs("ColorSelector")]
    public class XwtColorSelectorNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public bool SupportsAlpha = false;
        [YAXAttributeForClass]
        public string Value = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public string Changed = "";

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.ColorSelector Target = new Xwt.ColorSelector()
            {
                SupportsAlpha = this.SupportsAlpha
            };
            if (this.Value != "")
                Target.Color = Xwt.Drawing.Color.FromName(this.Value);
            //Binding
            if (Source != "")
            {
                Target.Color = (Xwt.Drawing.Color)PathBind.GetValue(Source, Parent, Xwt.Drawing.Colors.White);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.Color = (Xwt.Drawing.Color)PathBind.GetValue(Source, Parent, Xwt.Drawing.Colors.White));
                };
                Target.ColorChanged += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.Color);
                };
            }

            WindowController.TryAttachEvent(Target, "ColorChanged", Parent, Changed);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
