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
        private ExpressionTree tree;
        private FollowTable follows;
        private TransitionTable transitions;

        public TreeDetails()
        {
            InitializeComponent();
        }

        public TreeDetails(ExpressionTree tree)
        {
            InitializeComponent();
            this.tree = tree;

            expressionTextBox.Text = tree.expression;
        }

        public TreeDetails(ExpressionTree tree, FollowTable follows, TransitionTable transitions)
        {
            InitializeComponent();
            this.tree = tree;
            this.follows = follows;
            this.transitions = transitions;

            expressionTextBox.Text = tree.expression;
        }

        public void PaintTree(ExpressionTree _tree)
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

        private void loadTree()
        {
            PaintTree(tree);
        }

        private void loadFunctions()
        {
            List<string[]> functions = tree.getValuesOfNodes();
            
            TreeData.Columns.Add("SIMBOLO", "SIMBOLO");
            TreeData.Columns.Add("FIRST", "FIRST");
            TreeData.Columns.Add("LAST", "LAST");
            TreeData.Columns.Add("NULLABLE", "NULLABLE");

            int length = functions.Count;
            for (int i = 0; i < length; i++)
            {
                TreeData.Rows.Add();

                TreeData.Rows[i].Cells[0].Value = functions[i][0];
                TreeData.Rows[i].Cells[1].Value = functions[i][1];
                TreeData.Rows[i].Cells[2].Value = functions[i][2];
                TreeData.Rows[i].Cells[3].Value = functions[i][3];
            }
        }

        private void loadFollows()
        {
            followData.Columns.Add("SIMBOLO", "SIMBOLO");
            followData.Columns.Add("FOLLOW", "FOLLOW");

            for (int rowIndex = 1; rowIndex < follows.nodes.Count; rowIndex++)
            {
                var item = follows.nodes[rowIndex];
                followData.Rows.Add();
                followData.Rows[rowIndex - 1].Cells[0].Value = item.character;
                followData.Rows[rowIndex - 1].Cells[1].Value = string.Join(",", item.follows);
            }
        }

        private void loadTransitions()
        {
            int indexCount = 1;
            //Key=symbol, value=index
            Dictionary<string, int> index = new Dictionary<string, int>();

            //Columns
            transitionData.Columns.Add("ESTADO", "ESTADO");
            foreach (var item in transitions.symbolsList)
            {
                index.Add(item, indexCount);
                transitionData.Columns.Add(item, item);
                indexCount++;
            }
            //Rows (States)
            for (int i = 0; i < transitions.states.Count; i++)
            {
                transitionData.Rows.Add("---");
                var item = string.Join(",", transitions.states[i]);

                transitionData.Rows[i].Cells[0].Value = item;
            }
            ////Rows (Transitions)
            //foreach (var item in transitions.symbolsList)
            //{
            //    index.Add(item, indexCount);
            //    transitionData.Columns.Add(item, item);
            //    indexCount++;
            //}
        }

        private void TreeDetails_Load(object sender, EventArgs e)
        {

            loadFunctions();
            loadFollows();
            loadTransitions();

            Cursor.Current = Cursors.WaitCursor;

            System.Threading.ThreadStart
                FStart = loadTree;
            System.Threading.Thread MyThread =
                new System.Threading.Thread(FStart);
            MyThread.Start();

            Cursor.Current = Cursors.Default;

        }
    }
}
