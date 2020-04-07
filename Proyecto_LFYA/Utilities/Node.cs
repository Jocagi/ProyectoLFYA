using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

            // get a Graphics from _nodeBg bitmap, 
            var g = Graphics.FromImage(_nodeBg);
            // set the smoothing mode
            g.SmoothingMode = SmoothingMode.HighQuality; 
            // get a rectangle of drawer
            var rcl = new Rectangle(1, 1, _nodeBg.Width - 2, _nodeBg.Height - 2);
            g.FillRectangle(Brushes.Transparent, rcl);
            // get a ellipse of drawer
            g.FillEllipse(new LinearGradientBrush(new Point(0, 0), 
                new Point(1, 1), Color.CornflowerBlue, Color.Black), rcl);
            g.DrawEllipse(new Pen(Color.Black, 2.1f), rcl); 
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

        //Draw tree

        private bool _isChanged = true;
        /// <summary>
        /// true indicates that the current node or it's childs' has been updated or changed, and this value will cause the drawer redraw the current node
        /// </summary>
        public bool IsChanged()
        {
            return _isChanged;
        }

        /// <summary>
        /// the last up to date image of current node and it's childs.
        /// </summary>
        Image _lastImage;

        /// <summary>
        /// the location of the node on the top of the _lastImage.
        /// </summary>
        private int _lastImageLocationOfStarterNode;
        
        /// <summary>
        /// the backgroung image of each nodes, the size of this bitmap affects the quality of output image
        /// </summary>
        private static Bitmap _nodeBg = new Bitmap(120, 100);
        /// <summary>
        /// the free space between nodes on the drawed image, 
        /// </summary>
        private static Size _freeSpace = new Size(_nodeBg.Width / 8, (int)(_nodeBg.Height * 1.3f));
        /// <summary>
        /// a value which is used, (on drawing the Value of the nodes), in order to make sure the drawed image would be the same for any size of _nodeBg.<see cref="_nodeBg"/>
        /// </summary>
        private static readonly float Coef = _nodeBg.Width / 40f;

        private static readonly FontFamily fontFamily = new FontFamily("Arial");

        private static readonly Font font = new Font(
            fontFamily,
            emSize: 15f * Coef,
            style: FontStyle.Bold);


        /// <summary>
        /// paints the node and it's childs
        /// </summary>
        /// <param name="center">the location of the node on the top of the drawed image.</param>
        /// <returns>the image representing the current node and it's childs</returns>
        public Image Draw(out int center)
        {
            center = _lastImageLocationOfStarterNode;
            if (!IsChanged()) // if the current node and it's childs are up to date, just return the last drawed image.
                return _lastImage;

            var lCenter = 0;
            var rCenter = 0;

            Image lNodeImg = null, rNodeImg = null;
            if (left != null)       // draw left node's image
                lNodeImg = left.Draw(out lCenter);
            if (right != null)      // draw right node's image
                rNodeImg = right.Draw(out rCenter);

            // draw current node and it's childs (left node image and right node image)
            var lSize = new Size();
            var rSize = new Size();
            var under = (lNodeImg != null) || (rNodeImg != null);// if true the current node has childs
            if (lNodeImg != null)
                lSize = lNodeImg.Size;
            if (rNodeImg != null)
                rSize = rNodeImg.Size;

            var maxHeight = lSize.Height;
            if (maxHeight < rSize.Height)
                maxHeight = rSize.Height;

            if (lSize.Width <= 0)
                lSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;
            if (rSize.Width <= 0)
                rSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;

            var resSize = new Size
            {
                Width = lSize.Width + rSize.Width + _freeSpace.Width,
                Height = _nodeBg.Size.Height + (under ? maxHeight + _freeSpace.Height : 0)
            };

            var result = new Bitmap(resSize.Width, resSize.Height);
            var g = Graphics.FromImage(result);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), resSize));
            g.DrawImage(_nodeBg, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2, 0);

            var str = element.ToString();
            g.DrawString(str, font, Brushes.Black, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2 + (2 + (str.Length == 1 ? 10 : str.Length == 2 ? 5 : 0)) * Coef, _nodeBg.Height / 2f - 12 * Coef);


            center = lSize.Width + _freeSpace.Width / 2;
            var pen = new Pen(Brushes.Black, 2.0f * Coef)
            {
                EndCap = LineCap.ArrowAnchor,
                StartCap = LineCap.Round
            };


            float x1 = center;
            float y1 = _nodeBg.Height;
            float y2 = _nodeBg.Height + _freeSpace.Height;
            float x2 = lCenter;
            var h = Math.Abs(y2 - y1);
            var w = Math.Abs(x2 - x1);
            if (lNodeImg != null)
            {
                g.DrawImage(lNodeImg, 0, _nodeBg.Size.Height + _freeSpace.Height);
                var points1 = new List<PointF>
                                  {
                                      new PointF(x1, y1),
                                      new PointF(x1 - w/6, y1 + h/3.5f),
                                      new PointF(x2 + w/6, y2 - h/3.5f),
                                      new PointF(x2, y2),
                                  };
                g.DrawCurve(pen, points1.ToArray(), 0.4f);
            }
            if (rNodeImg != null)
            {
                g.DrawImage(rNodeImg, lSize.Width + _freeSpace.Width, _nodeBg.Size.Height + _freeSpace.Height);
                x2 = rCenter + lSize.Width + _freeSpace.Width;
                w = Math.Abs(x2 - x1);
                var points = new List<PointF>
                                 {
                                     new PointF(x1, y1),
                                     new PointF(x1 + w/6, y1 + h/3.5f),
                                     new PointF(x2 - w/6, y2 - h/3.5f),
                                     new PointF(x2, y2)
                                 };
                g.DrawCurve(pen, points.ToArray(), 0.4f);
            }
            _isChanged = false;
            _lastImage = result;
            _lastImageLocationOfStarterNode = center;
            return result;
        }
    }
}
