using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace Xwt.Ext.Bindings
{
    public class DataField<T>: IDataField<T>
    {
        FieldInfo BackingField;
        int _Index;

        public DataField(string Fieldname, Type BackingType)
        {
            FieldInfo[] Fields = BackingType.GetFields();
            int i = 0;
            for (; i < Fields.Count(); i++)
                if (Fields[i].Name == Fieldname) break;
            this._Index = i;
            this.BackingField = Fields[i];
        }
        public Type FieldType
        {
            get { return BackingField.FieldType; }
        }

        public int Index
        {
            get { return this._Index; }
        }
    }

    public static class DataField
    {
        public static IDataField GenerateDataField(string Fieldname, Type BackingType)
        {
            Type t = BackingType.GetField(Fieldname).FieldType;
            if (t == typeof(bool)) return new DataField<bool>(Fieldname, BackingType);
            if (t == typeof(Xwt.Drawing.Image)) return new DataField<Xwt.Drawing.Image>(Fieldname, BackingType);
            return new DataField<string>(Fieldname, BackingType);
        }
    }
}
