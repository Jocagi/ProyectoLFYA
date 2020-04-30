using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp;
using Proyecto_LFYA.Utilities;
using Proyecto_LFYA.Utilities.DFA_Procedures;

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


                    tree = Utilities.AnalizarGramatica.obtenerArbolDeGramatica(grammarTextBox.Text);
                    
                }
                else
                {
                    resultTextBox.BackColor = Color.LightGray;
                    resultTextBox.ForeColor = Color.Crimson;

                    //Ubicacion del error
                    int lineCounter = 0;

                    foreach (string line in grammarTextBox.Lines)
                    {
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

                resultTextBox.BackColor = Color.LightGray;
                resultTextBox.ForeColor = Color.Crimson;

                resultTextBox.Text = "Error en TOKENS";
                detailsButton.Visible = false;
                
                MessageBox.Show(ex.Message);

                //Show in red all lines in tokens
                int lineCounter = 0;

                foreach (string line in grammarTextBox.Lines)
                {
                    if (line.Contains("TOKEN"))
                    {
                        grammarTextBox.Select(grammarTextBox.GetFirstCharIndexFromLine(lineCounter), line.Length);
                        grammarTextBox.SelectionColor = Color.Red;
                    }
                    lineCounter++;
                }
            }
        }

        private void detailsButton_Click(object sender, EventArgs e)
        {
            try
            {
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

        private void gneratorButtom_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string sourceCode = Resources.Resource1.Program;
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            
            string Output = "Scanner.exe";
            
            //Make sure to generate an EXE
            CompilerParameters parameters = new CompilerParameters
                { GenerateExecutable = true, OutputAssembly = Output};

            //Add References
            parameters.ReferencedAssemblies.AddRange(
                Assembly.GetExecutingAssembly().GetReferencedAssemblies().
                    Select(a => a.Name + ".dll").ToArray());

            //Path to save the EXE
            parameters.CompilerOptions = $" /out:{path}\\" + Output;

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode);
            
            if (results.Errors.Count > 0)
            {
                resultTextBox.ForeColor = Color.Red;
                foreach (CompilerError CompErr in results.Errors)
                {
                    resultTextBox.Text = resultTextBox.Text +
                                         @"Line number " + CompErr.Line +
                                         @", Error Number: " + CompErr.ErrorNumber +
                                         ", '" + CompErr.ErrorText + ";" +
                                         Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                //Successful Compile
                resultTextBox.ForeColor = Color.Magenta;
                resultTextBox.Text = @"Success!";
                //If we clicked run then launch our EXE
                Process.Start(Output);
            }
        }

    }
}
