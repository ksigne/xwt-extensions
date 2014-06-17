using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xwt.Ext.Bindings
{
    static class PathBind
    {
        public static object GetValue(string Field, object Source, object Default)
        {
            string[] Path = Field.Split('.');
            int i = -1;
            try
            {
                while (++i < Path.Count())
                {
                    if (Source != null)
                    {
                        if (Source.GetType().GetProperty(Path[i]) != null)
                            Source = (object)(Source.GetType().GetProperty(Path[i]).GetValue(Source));
                        else
                            if (Source.GetType().GetField(Path[i]) != null)
                                Source = (object)(Source.GetType().GetField(Path[i]).GetValue(Source));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error reading "+Field);
                        return Default;
                    }
                }

                return Source;
            }
            catch
            {
                return Default;
            }
        }

        public static void SetValue(string Field, object Source, object Value)
        {
            const System.Reflection.BindingFlags b = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase;

            string[] Path = Field.Split('.');
            int i = -1;
            while (++i < Path.Count()-1)
            {
                if (Source.GetType().GetProperty(Path[i], b) != null)
                    Source = Source.GetType().GetProperty(Path[i], b).GetValue(Source);
                else
                    if (Source.GetType().GetField(Path[i], b) != null)
                        Source = Source.GetType().GetField(Path[i], b).GetValue(Source);
            }
            if (Source.GetType().GetProperty(Path[i], b) != null)
                Source.GetType().GetProperty(Path[i], b).SetValue(Source, Value);
            else
                if (Source.GetType().GetField(Path[i], b) != null)
                    Source.GetType().GetField(Path[i], b).SetValue(Source, Value);
        }
    }
}
