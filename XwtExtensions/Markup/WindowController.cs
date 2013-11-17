using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XwtExtensions
{
    public static class WindowController
    {
        struct WidgetContext
        {
            public WindowWrapper Context;
            public string Name;
            public Xwt.XwtComponent Self;
        }

        public static Dictionary<string, Xwt.RadioButtonGroup> RadioGroups = new Dictionary<string, Xwt.RadioButtonGroup>();
        public static Dictionary<string, Xwt.RadioButtonMenuItemGroup> MenuRadioGroups = new Dictionary<string, Xwt.RadioButtonMenuItemGroup>(); 

        static List<WidgetContext> AllWidgets = new List<WidgetContext>();

        public static Xwt.XwtComponent GetWidget(object Context, string Name)
        {
            return AllWidgets.FirstOrDefault(Y => Y.Name == Name && Y.Context == AllWidgets.FirstOrDefault(X => X.Self == (Context as Xwt.XwtComponent)).Context).Self;
        }

        public static void RegisterWidget(string Name, WindowWrapper Context, Xwt.XwtComponent W)
        {
            AllWidgets.Add(new WidgetContext { Name = Name, Context = Context, Self = W });
        }
        public static bool TryAttachEvent(object Target, string TargetEvent, object ResponsibleObject, string HandlerMethod)
        {
            if (HandlerMethod != "")
            {
                try
                {
                    var eventInfo = Target.GetType().GetEvent(TargetEvent);
                    var methodInfo = ResponsibleObject.GetType().GetMethod(HandlerMethod);
                    Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, ResponsibleObject, methodInfo);
                    eventInfo.AddEventHandler(Target, handler);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Не удается определить событие " + TargetEvent + " методом " + HandlerMethod+": "+e.Message);
                }
            }
            return false;
        }
    }
}
