using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace Xwt.Ext.Bindings
{
    public static class CellViewFactory
    {
        public static Xwt.CellView Make(Xwt.IDataField F, bool Editable)
        {
            if (F.FieldType == typeof(Xwt.Drawing.Image))
                return new Xwt.ImageCellView(F as Xwt.IDataField<Xwt.Drawing.Image>);
            if (F.FieldType == typeof(bool))
                return new Xwt.CheckBoxCellView(F as Xwt.IDataField<bool>) {
                    Editable = Editable
                };
            return new Xwt.TextCellView(F) { Editable = Editable };
        }
    }
}
