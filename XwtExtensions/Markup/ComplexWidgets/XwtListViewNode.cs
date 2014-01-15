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
    [YAXSerializeAs("ListView")]
    public class XwtListViewNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Changed = "";
        [YAXAttributeForClass]
        public string Clicked = "";
        [YAXAttributeForClass]
        public int Selection = 0;
        [YAXAttributeForClass]
        public bool BorderVisible = true;
        [YAXAttributeForClass]
        public bool HeadersVisible = true;
        [YAXAttributeForClass]
        public SelectionMode SelectionMode = SelectionMode.Single;
        [YAXAttributeForClass]
        public ScrollPolicy VScrollPolicy = ScrollPolicy.Automatic;
        [YAXAttributeForClass]
        public ScrollPolicy HScrollPolicy = ScrollPolicy.Automatic;
        [YAXAttributeForClass]
        public string Source = "";
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtSimpleBindingNode> Items;
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtColumnDefinitionNode> ColumnDefinition;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.ListView Target = new Xwt.ListView()
            {
                BorderVisible = this.BorderVisible,
                HeadersVisible = this.HeadersVisible,
                SelectionMode = this.SelectionMode,
                VerticalScrollPolicy = this.VScrollPolicy,
                HorizontalScrollPolicy = this.HScrollPolicy
            };
            if (this.Source != "")
            {
                IListDataSource Source = (IListDataSource)Parent.GetType().GetField(this.Source).GetValue(Parent);
                Target.DataSource = Source;
                Type BackgroundType = (Source).GetType().GetGenericArguments()[0];
                if (ColumnDefinition != null && ColumnDefinition.Count() > 0)
                {
                    foreach (XwtColumnDefinitionNode N in ColumnDefinition)
                    {
                        IDataField X = DataField.GenerateDataField(N.Source, BackgroundType);
                        ListViewColumn C = new ListViewColumn(N.Title, CellViewFactory.Make(X, N.Editable))
                        {
                            SortDataField = X,
                            SortIndicatorVisible = N.Sortable,
                            CanResize = N.Resizable
                        };
                        Target.Columns.Add(C);
                    }
                }                              
            }
            if (Target.DataSource != null && Target.DataSource.RowCount > 0)
                Target.SelectRow(this.Selection);
            WindowController.TryAttachEvent(Target, "SelectionChanged", Parent, Changed);
            WindowController.TryAttachEvent(Target, "RowActivated", Parent, Clicked);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
