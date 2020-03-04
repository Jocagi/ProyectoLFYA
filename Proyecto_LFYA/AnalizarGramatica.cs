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

namespace Proyecto_LFYA
{
    public partial class AnalizarGramatica : Form
    {
        public AnalizarGramatica()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); 

            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                labelFilePath.Text = file;

                try
                {
                    string text = File.ReadAllText(file);
                    labelResult.Text = Utilities.AnalizarGramatica.analizarAchivoGramatica(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
