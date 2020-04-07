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
using Proyecto_LFYA.Utilities;

namespace Proyecto_LFYA
{
    public partial class TreeDetails : Form
    {
        public TreeDetails()
        {
            InitializeComponent();
        }

        public TreeDetails(ExpressionTree tree)
        {
            InitializeComponent();
            PaintTree(tree);
        }

        void PaintTree(ExpressionTree _tree)
        {
            if (_tree == null) return;
            treePictureBox.Image = _tree.Draw();
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            var codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }

        private void download_Click(object sender, EventArgs e)
        {
            const double quality = 1;
            var d = new SaveFileDialog { Filter = @"png files|*.png" };
            try
            {
                if (d.ShowDialog() != DialogResult.OK)
                    return;
                var bmp = treePictureBox.Image;
                var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                    (long)(quality * 75));

                var pngCodec = GetEncoderInfo("image/png");
                if (pngCodec == null)
                    return;
                var encoderParams = new EncoderParameters(1) { Param = { [0] = qualityParam } };
                bmp.Save(d.FileName, pngCodec, encoderParams);

                MessageBox.Show(@"Archivo guardado exitosamente.");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}
