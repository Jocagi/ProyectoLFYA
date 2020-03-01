using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class TransitionTable
    {
        //Dictionary with posible next positions
        public Dictionary<int, Dictionary<char, int>> Table { get; set; }
        private Stack<char> characters = new Stack<char>();

        public TransitionTable(ExpressionTree tree)
        {
            evaluateTree(tree.root);
        }

        private void evaluateTree(Node node)
        {
            if (node.isLeaf())
            {
                characters.Push(node.element);
            }
            else
            {
                    
            }
        }
    }
}
