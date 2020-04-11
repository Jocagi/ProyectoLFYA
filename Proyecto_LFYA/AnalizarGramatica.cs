using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_LFYA.Utilities;

namespace Proyecto_LFYA
{
    public partial class AnalizarGramatica : Form
    {
        ExpressionTree tree = new ExpressionTree();

        public AnalizarGramatica()
        {
            InitializeComponent();
        }

        public AnalizarGramatica(string filePath)
        {
            InitializeComponent();
            AnalizarArchivo(filePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); 

            if (result == DialogResult.OK)
            {
                AnalizarArchivo(openFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show(@"Error al leer el archivo.");
            }
        }

        private void AnalizarGramatica_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void AnalizarGramatica_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                AnalizarGramatica f = new AnalizarGramatica(file);
                f.Show();
            }
            Hide();
        }

        private void AnalizarArchivo(string file)
        {
            detailsButton.Visible = false;
            pathTextBox.Text = file;

            try
            {
                int linea = 0;
                string text = File.ReadAllText(file);
                resultTextBox.Text = Utilities.AnalizarGramatica.analizarAchivoGramatica(text, ref linea);
                grammarTextBox.Text = text;
                
                if (resultTextBox.Text.Contains("Correcto"))
                {
                    resultTextBox.BackColor = Color.LightGray;
                    resultTextBox.ForeColor = Color.Green;

                    detailsButton.Visible = true;
                }
                else
                {
                    resultTextBox.BackColor = Color.LightGray;
                    resultTextBox.ForeColor = Color.Crimson;

                    //Ubicacion del error
                    int lineCounter = 0;

                    foreach (string line in grammarTextBox.Lines)
                    {
                        //add conditional statement if not selecting all the lines
                        if (linea - 1 == lineCounter)
                        {
                            grammarTextBox.Select(grammarTextBox.GetFirstCharIndexFromLine(lineCounter), line.Length);
                            grammarTextBox.SelectionColor = Color.Red;
                        }
                        lineCounter++;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void detailsButton_Click(object sender, EventArgs e)
        {
            try
            {
                tree = Utilities.AnalizarGramatica.obtenerArbolDeGramatica(grammarTextBox.Text);
                FollowTable follows = new FollowTable(tree);
                TransitionTable transitions = new TransitionTable(follows);
                
                TreeDetails window = new TreeDetails(tree, follows, transitions);
                window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
