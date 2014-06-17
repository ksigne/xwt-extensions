using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xwt;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Notebook")]
    public class XwtNotebookNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public int CurrentTab = 0;
        [YAXAttributeForClass]
        public NotebookTabOrientation Orientation = NotebookTabOrientation.Top;
        [YAXAttributeForClass]
        public string Changed = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtNotebookTabNode> Tabs;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.Notebook Target = new Xwt.Notebook()
            {
                TabOrientation = this.Orientation
            };

            InitWidget(Target, Parent);

            if (this.Tabs != null)
            foreach (XwtNotebookTabNode Node in this.Tabs)
            {
                Xwt.Widget TWidget;
                if (Node.Items != null && Node.Items.Count() > 0)
                    TWidget = Node.Items[0].Makeup(Parent); 
                else TWidget = new Xwt.VBox();
                Target.Add(TWidget, Node.Text);
            }

            Target.CurrentTabIndex = this.CurrentTab;

            return Target;
        }
    }
}
