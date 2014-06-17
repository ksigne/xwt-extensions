using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt.Ext.Markup.Widgets;
using YAXLib;

namespace Xwt.Ext.Markup
{
    public static class WindowFactory
    {
        public static void Test()
        {
            XwtWindowNode Wnd = new XwtWindowNode() 
            {Title = "Wnd1"};
            Wnd.Content = new XwtBoxNode()
            {
            };
            //YAXLib.YAXSerializer Y = new YAXSerializer(typeof(XwtWindowNode), YAXExceptionHandlingPolicies.ThrowWarningsAndErrors);
            //string S = Y.Serialize(Wnd);
            //Console.Write(S);
        }

    }
}
