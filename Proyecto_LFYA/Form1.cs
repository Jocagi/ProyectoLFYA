using Proyecto_LFYA.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_LFYA.Utilities.DFA_Procedures;

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
            //ExpressionTree tree = new ExpressionTree(textExpression.Text);
            //PaintTree(tree);

            Dictionary<string, string[]> sets = new Dictionary<string, string[]>();
            sets.Add("LETRA", new []{ "97,100", "107" });
            ExpressionTree tree2 = new ExpressionTree(textExpression.Text, sets);
            PaintTree(tree2);

            //labelResult.Text = tree.root.Inorder();
            try
            {
                RegEx reg = new RegEx(textExpression.Text);
                string message = reg.ValidateString(text.Text);
                resultadoExpresion.Text = message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            var codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const double quality = 1;
            var d = new SaveFileDialog { Filter = @"png files|*.png" };
            try
            {
                if (d.ShowDialog() != DialogResult.OK)
                    return;
                var bmp = TreePicture.Image;
                var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                    (long)(quality * 75));
                
                var pngCodec = GetEncoderInfo("image/png");
                if (pngCodec == null)
                    return;
                var encoderParams = new EncoderParameters(1) {Param = {[0] = qualityParam}};
                bmp.Save(d.FileName, pngCodec, encoderParams);

                MessageBox.Show(@"Archivo guardado exitosamente.");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        void PaintTree(ExpressionTree _tree)
        {
            if (_tree == null) return;
            TreePicture.Image = _tree.Draw();
        }
    }
}
