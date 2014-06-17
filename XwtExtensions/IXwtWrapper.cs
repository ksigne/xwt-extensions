using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xwt.Ext
{
    public interface IXwtWrapper: INotifyPropertyChanged
    {
        Dictionary<string, Xwt.XwtComponent> Widgets { get; set; }
       
    }
}
