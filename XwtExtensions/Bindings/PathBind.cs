using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XwtExtensions.Bindings
{
    static class PathBind
    {
        public static object GetValue(string Field, object Source)
        {
            string[] Path = Field.Split('.');
            int i = -1;
            while (++i < Path.Count())
            {
                if (Source.GetType().GetProperty(Path[i])!=null)
                    Source = (object)(Source.GetType().GetProperty(Path[i]).GetValue(Source)); 
                else
                    if (Source.GetType().GetField(Path[i]) != null)
                        Source = (object)(Source.GetType().GetField(Path[i]).GetValue(Source));  
            }
            return Source;
        }

        public static void SetValue(string Field, object Source, object Value)
        {
            string[] Path = Field.Split('.');
            int i = -1;
            while (++i < Path.Count()-1)
            {
                if (Source.GetType().GetProperty(Path[i]) != null)
                    Source = Source.GetType().GetProperty(Path[i]).GetValue(Source);
                else
                    if (Source.GetType().GetField(Path[i]) != null)
                        Source = Source.GetType().GetField(Path[i]).GetValue(Source);
            }
            if (Source.GetType().GetProperty(Path[i]) != null)
                Source.GetType().GetProperty(Path[i]).SetValue(Source, Value);
            else
                if (Source.GetType().GetField(Path[i]) != null)
                    Source.GetType().GetField(Path[i]).SetValue(Source, Value);
        }
    }
}
