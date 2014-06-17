using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Ext.Markup;
using YAXLib;

namespace Xwt.Ext
{
    public class FrameExt : IXwtWrapper
    {
        public Xwt.Widget Root;

        public static string Prefix
        {
            get
            {
                return YAXSerializer.Prefix;
            }
            set
            {
                YAXSerializer.Prefix = value;
            }
        }

        public FrameExt()
        {
            this.Widgets = new Dictionary<string, XwtComponent>();

            if (Attribute.IsDefined(this.GetType(), typeof(WindowXmlResource)))
            {
                var XmlAttribute = Attribute.GetCustomAttribute(this.GetType(), typeof(WindowXmlResource)) as WindowXmlResource;
                var A = System.Reflection.Assembly.GetAssembly(this.GetType());
                System.IO.Stream I = A.GetManifestResourceStream(XmlAttribute.Ref);
                string Text = (new System.IO.StreamReader(I)).ReadToEnd();
                this.Read(Text);
            }
        }
        protected Xwt.Widget Read(string Text)
        {
            YAXLib.YAXSerializer Y = new YAXSerializer(typeof(FrameRootNode), YAXExceptionHandlingPolicies.DoNotThrow);
            FrameRootNode Target = (FrameRootNode)Y.Deserialize(Text);
            this.Root = Target.Content.Makeup(this);
            return Root;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, XwtComponent> Widgets
        {
            get;
            set;
        }
    }

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("FrameRoot")]
    public class FrameRootNode
    {
        public Xwt.Ext.Markup.Widgets.XwtBoxNode Content;
    }

}
