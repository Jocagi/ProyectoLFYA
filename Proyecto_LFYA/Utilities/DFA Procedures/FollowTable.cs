using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_LFYA.Utilities.DFA_Procedures;

namespace Proyecto_LFYA.Utilities
{
    public class FollowTable:ExpressionCharacters
    {
        //Dictionary with posible next positions
        public List<Follow> nodes = new List<Follow>(); 

        public FollowTable(ExpressionTree tree)
        {
            nodes.Add(new Follow(Epsilon)); //Set apart first position, to use it later as an initial state
            evaluateTree(tree.root); //Get all the follows

            //Initial state
            nodes[0].follows = tree.root.firstPos;
        }

        private void evaluateTree(Node tree)
        {
            getEnumeration(tree);
            getFollowPos(tree);
        }

        private void getEnumeration(Node root)
        {
            if (root.isLeaf())
            {
                nodes.Add(new Follow(root.element));
            }
            else
            {
                getEnumeration(root.left);

                if (root.right != null)
                {
                    getEnumeration(root.right);
                }
            }
        }

        private void getFollowPos(Node node)
        {
            if (node != null)
            {
                getFollowPos(node.left);
                getFollowPos(node.right);

                if (!node.isLeaf())
                {

                    if (node.element == Concatenation && node.left != null && node.right != null)
                    {
                        //Being "i" a position in lastPos(c1) 
                        //Then all positions in firstPos(c2) are in followPos(i)

                        foreach (var item in node.left.lastPos)
                        {
                            nodes[item].follows = nodes[item].follows.Concat(node.right.firstPos).ToList(); 
                        }

                    }
                    else if (node.element == KleenePlus || node.element == KleeneStar)
                    {
                        //Being "n" this node and "i" a position in lastPos(n) 
                        //Then all positions in firstPos(n) are in followPos(i)

                        foreach (var item in node.lastPos)
                        {
                            nodes[item].follows = nodes[item].follows.Concat(node.firstPos).ToList();
                        }
                    }
                }
            }
        }
    }
}
