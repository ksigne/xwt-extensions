using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using XwtExtensions.Bindings;
using YAXLib;

namespace XwtExtensions.Markup.ComplexWidgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("ComboBox")]
    public class XwtComboBoxNode : XwtWidgetNode
    {
        

        [YAXAttributeForClass]
        public string Changed = "";
        [YAXAttributeForClass]
        public int Selection = 0;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public List<XwtColumnDefinitionNode> ColumnDefinition;
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtSimpleBindingNode> Items;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.ComboBox Target = new Xwt.ComboBox();
            if (this.Source != "")
            {
                Target.ItemsSource = (IListDataSource)Parent.GetType().GetField(this.Source).GetValue(Parent);
                Type BackgroundType = (Target.ItemsSource).GetType().GetGenericArguments()[0];
                if (ColumnDefinition != null && ColumnDefinition.Count() > 0)
                {
                    foreach (XwtColumnDefinitionNode N in ColumnDefinition)
                    {
                        IDataField X = DataField.GenerateDataField(N.Source, BackgroundType);
                        Target.Views.Add(CellViewFactory.Make(X, false));
                    }
                }
            }
            else
            {
                if (Items != null && Items.Count() > 0)
                {
                    foreach (XwtSimpleBindingNode N in Items)
                    {
                        object value = null;
                        try
                        {
                            Target.ItemsSource = (IListDataSource)Parent.GetType().GetField(N.Value).GetValue(Parent);
                        }
                        catch { }
                        Target.Items.Add(value, N.Text);
                    }
                }            
            }
            Target.SelectedIndex = this.Selection;
            WindowController.TryAttachEvent(Target, "SelectionChanged", Parent, Changed);

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
