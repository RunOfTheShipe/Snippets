using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snippets.Graph
{
    public class DirectedGraph
    {
        public List<GraphNode> Nodes { get; set; }

        public DirectedGraph()
        {
            Nodes = new List<GraphNode>();
        }

        void AddNode()
        {
            var intNode = new GraphNode(typeof(IntParams));
            ((IntParams)intNode.Params).IntNumber = 5;
            //intNode.Params.IntNumber = 5;
            Nodes.Add(intNode); // Won't compile; doesn't like that T=IntParams in intNode and T=NodeParams in this.Nodes.

            var doubleNode = new GraphNode(typeof(DoubleParams));
            ((DoubleParams)doubleNode.Params).DoubleNumber = 0.50;
            //doubleNode.Params.DoubleNumber = 0.05;
            Nodes.Add(doubleNode);
        }
    }

    public class GraphNode //<T> where T : NodeParams
    {
        //public T Params { get; set; } // Of type NodeParams

        public NodeParams Params { get; set; }

        public GraphNode(Type nodeParamsType)
        {
            Params = Activator.CreateInstance(nodeParamsType);
        }
    }

    #region Node Params

    abstract public class NodeParams
    {
        public string Name { get; set; }
    }

    public class IntParams : NodeParams
    {
        public int IntNumber { get; set; }
    }

    public class DoubleParams : NodeParams
    {
        public double DoubleNumber { get; set; }
    }

    #endregion
}
