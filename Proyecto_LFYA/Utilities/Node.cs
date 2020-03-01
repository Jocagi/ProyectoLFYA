using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class Node
    {
        public int number { set; get; }
        public List<int> firstPos { set; get; }
        public List<int> lastPos { set; get; }
        public bool nullable { set; get; }

        public char element { set; get; }
        public Node left { set; get; }
        public Node right { set; get; }
        
        //Functions
        public Node() { }
        public Node(char element)
        {
            this.element = element;

            firstPos = new List<int>();
            lastPos = new List<int>();
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

        public string Inorder()
        {
            return Inorder(this);
        }
        private string Inorder(Node root)
        {
            string result = "";

            if (root.left != null)
            {
                result += Inorder(root.left);
            }

            result += root.element;

            if (root.right != null)
            {
                result += Inorder(root.right);
            }

            return result;
        }

        public bool isLeaf()
        {
         
   return this.left == null && this.right == null;
        }
        
    }
}
