using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Image")]
    public class XwtImageViewNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Image = "";
        
        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.ImageView Target = new Xwt.ImageView();
            if (Image != "") {
                Xwt.Drawing.Image img = Xwt.Drawing.Image.FromResource(System.Reflection.Assembly.GetEntryAssembly(), Image);
                //Target.Image = img;
                Target.BoundsChanged += (o, e) =>
                {
                    Target.Image = img.WithSize(Target.Size);
                };
            }
            InitWidget(Target, Parent);
            return Target;
            
        }
    }
}
