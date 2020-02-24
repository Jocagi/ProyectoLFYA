using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class Node
    {
        public char element { set; get; }
        public Node left { set; get; }
        public Node right { set; get; }

        public Node() { }
        public Node(char element)
        {
            this.element = element;
        }
        public Node(char element, Node left, Node right)
        {
            this.element = element;
            this.left = left;
            this.right = right;
        }
        public Node(char element, char left, char right)
        {
            this.element = element;
            this.left = new Node(left);
            this.right = new Node (right);
        }

        public string Inorden()
        {
            return Inorden(this);
        }
        private string Inorden(Node root)
        {
            string result = "";

            if (root.left != null)
            {
                result += Inorden(root.left);
            }

            result += root.element;

            if (root.right != null)
            {
                result += Inorden(root.right);
            }

            return result;
        }

        public bool isLeaf()
        {
         
   return this.left == null && this.right == null;
        }

    }
}
