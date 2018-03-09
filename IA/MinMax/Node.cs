using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

////////////////////////////////////////////////////////////////////////////////
// class Node is used to implement the minimax algorithm.
// it will enable us to store the tree explored by alpha beta algorithm
// could be optimized, but was developped for debug purposes
////////////////////////////////////////////////////////////////////////////////

namespace IA
{
    class NodeData
    {
        public NodeData()
        {
            HeuristicScore = 0;
            MinMaxScore = 0;
        }

        // stores current position
        public float HeuristicScore { get; set; }
        public float MinMaxScore { get; set; }      // stores min max score

        public List<Move> Moves { get; set; }
    }

    class Node
    {
        public Node(NodeData NodeData)
        {
            Data = NodeData;
            this.Children = new List<Node>();
        }

        public NodeData Data { get; private set; }
        public List<Node> Children { get; private set; }

        // Method checking if Node is Leaf
        public bool IsLeaf()
        {
            return (Children.Count() == 0);
        }
    }
}
