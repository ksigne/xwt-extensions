using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XwtExtensions.Markup;
using YAXLib;

namespace XwtExtensions
{
    public class WindowWrapper: INotifyPropertyChanged
    {
        public Dictionary<string, Xwt.XwtComponent> Widgets = new Dictionary<string, Xwt.XwtComponent>();
        public Xwt.Window Window;
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
            this.Window = Target.Makeup(this);
        }

        protected void Read(Assembly A, string Ref)
        {
            System.IO.Stream I = A.GetManifestResourceStream(Ref);
            string Text = (new System.IO.StreamReader(I)).ReadToEnd();
            this.Read(Text);
        }

        public WindowWrapper()
        {

        }

        public WindowWrapper(Assembly A, string Ref)
        {
            Read(A, Ref);
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
