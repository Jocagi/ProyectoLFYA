using Proyecto_LFYA.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_LFYA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExpressionTree tree = new ExpressionTree(textExpression.Text);
            labelResult.Text = tree.root.Inorder();

            RegEx reg = new RegEx(textExpression.Text);
            string message = "";
            reg.ValidateString(text.Text, ref message);
            resultadoExpresion.Text = message;
        }
    }
}
