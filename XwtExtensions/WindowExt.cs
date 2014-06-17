using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt.Ext.Markup;
using YAXLib;

namespace Xwt.Ext
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowXmlResource: System.Attribute 
    {
        public string Ref;

        public WindowXmlResource(string Ref)
        {
            this.Ref = Ref;
        }
    }

    public class WindowExt: Xwt.Window, IXwtWrapper
    {
        public static string Prefix
        {
            get {
                return YAXSerializer.Prefix;
            }
            set
            {
                YAXSerializer.Prefix = value;
            }
        }

        protected void Read(string Text)
        {
            YAXLib.YAXSerializer Y = new YAXSerializer(typeof(XwtWindowNode), YAXExceptionHandlingPolicies.DoNotThrow);
            XwtWindowNode Target = (XwtWindowNode)Y.Deserialize(Text);
            Target.Makeup(this, this);
        }

        public WindowExt()
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, XwtComponent> Widgets { get; set; }

    }
}
