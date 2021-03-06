﻿using System;
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
    [YAXSerializeAs("TreeView")]
    public class XwtTreeViewNode : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public string Changed = "";
        [YAXAttributeForClass]
        public string Clicked = "";
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

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.TreeView Target = new Xwt.TreeView()
            {
                HeadersVisible = this.HeadersVisible,
                SelectionMode = this.SelectionMode,
                VerticalScrollPolicy = this.VScrollPolicy,
                HorizontalScrollPolicy = this.HScrollPolicy
            };
            if (this.Source != "")
            {
                TreeStoreWrapper Source = ((Bindings.TreeStoreWrapper)(Parent.GetType().GetField(this.Source).GetValue(Parent)));
                Target.DataSource = Source.TreeStore;
                if (ColumnDefinition != null && ColumnDefinition.Count() > 0)
                {
                    foreach (XwtColumnDefinitionNode N in ColumnDefinition)
                    {
                        if (Source.Fields.ContainsKey(N.Source))
                        {
                            IDataField X = Source.Fields[N.Source];
                            IDataField S = X;
                            if (X.FieldType == typeof(DateTime))
                            {
                                S = Source.Fields[N.Source + "_sort"];
                                X = Source.Fields[N.Source + "_text"];
                            }
                            ListViewColumn C = new ListViewColumn(N.Title, CellViewFactory.Make(X, N.Editable))
                            {
                                SortDataField = S,
                                SortIndicatorVisible = N.Sortable,
                                CanResize = N.Resizable
                            };
                            Target.Columns.Add(C);
                        }
                    }
                }                              
            }    
            WindowController.TryAttachEvent(Target, "SelectionChanged", Parent, Changed);
            WindowController.TryAttachEvent(Target, "RowActivated", Parent, Clicked);
            InitWidget(Target, Parent);
            return Target;
        }
    }
}
