using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Ext.Bindings;
using YAXLib;

namespace Xwt.Ext.Markup.ComplexWidgets
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
        public string DataSource = "";
        [YAXAttributeForClass]
        public string Source = "";
        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public List<XwtColumnDefinitionNode> ColumnDefinition;
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtSimpleBindingNode> Items;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.ComboBox Target = new Xwt.ComboBox();
            if (this.DataSource != "")
            {
                Target.ItemsSource = (IListDataSource)Parent.GetType().GetField(this.DataSource).GetValue(Parent);
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
                        if (N.Value != "")
                        {
                            object value = Parent.GetType().GetField(N.Value).GetValue(Parent);
                            Target.Items.Add(value, N.Text);
                        }
                        else
                        {
                            Target.Items.Add((object)N.Text, N.Text);
                        }
                    }
                }            
            }
            Target.SelectedIndex = this.Selection;
            if (this.Source != "")
            {
                Target.SelectedItem = PathBind.GetValue(Source, Parent, Target.Items[0]);
                Parent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == this.Source.Split('.')[0])
                        Xwt.Application.Invoke(() => Target.SelectedItem = PathBind.GetValue(Source, Parent, Target.Items[0]));
                };
                Target.SelectionChanged += (o, e) =>
                {
                    PathBind.SetValue(Source, Parent, Target.SelectedItem);
                };
            }
            WindowController.TryAttachEvent(Target, "SelectionChanged", Parent, Changed);

            InitWidget(Target, Parent);
            return Target;
        }
    }
}
