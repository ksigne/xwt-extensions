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
        public WindowWrapper(string Filename)
        {
            YAXLib.YAXSerializer Y = new YAXSerializer(typeof(XwtWindowNode), YAXExceptionHandlingPolicies.DoNotThrow);
            XwtWindowNode Target = (XwtWindowNode)Y.DeserializeFromFile(Filename);
            this.Window = Target.Makeup(this);
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
