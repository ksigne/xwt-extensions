using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XwtExtensions.Commands
{
    public abstract class Command: INotifyPropertyChanged
    {
        public abstract void Run(object Context);

        public string Title { get; set; }
        public Xwt.Drawing.Image Icon { get; set; }
        public bool Enabled { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class MenuItemFactory
    {
        public static Xwt.MenuItem FromCommand(Command X)
        {
            if (X != null)
            {
                Xwt.MenuItem Target = new Xwt.MenuItem()
                {
                    Label = X.Title,
                    Image = X.Icon,
                    Sensitive = X.Enabled
                };
                Target.Clicked += (o, e) =>
                {
                    if (X.Enabled)
                        X.Run(null);
                };
                X.PropertyChanged += (o, e) =>
                {
                    Target.Label = X.Title;
                    Target.Image = X.Icon;
                    Target.Sensitive = X.Enabled;
                };
                return Target;
            }
            else throw new NullReferenceException();
        }
    }
}
