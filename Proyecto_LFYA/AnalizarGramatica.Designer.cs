namespace Proyecto_LFYA
{
    partial class AnalizarGramatica
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fileButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.grammarTextBox = new System.Windows.Forms.RichTextBox();
            this.detailsButton = new System.Windows.Forms.Button();
            this.gneratorButtom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seleccione el archivo:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 467);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Resultado:";
            // 
            // fileButton
            // 
            this.fileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fileButton.Location = new System.Drawing.Point(417, 44);
            this.fileButton.Name = "fileButton";
            this.fileButton.Size = new System.Drawing.Size(100, 40);
            this.fileButton.TabIndex = 3;
            this.fileButton.Text = "File";
            this.fileButton.UseVisualStyleBackColor = true;
            this.fileButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(37, 51);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(361, 26);
            this.pathTextBox.TabIndex = 5;
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(133, 464);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(384, 26);
            this.resultTextBox.TabIndex = 6;
            // 
            // grammarTextBox
            // 
            this.grammarTextBox.Location = new System.Drawing.Point(37, 98);
            this.grammarTextBox.Name = "grammarTextBox";
            this.grammarTextBox.ReadOnly = true;
            this.grammarTextBox.Size = new System.Drawing.Size(480, 335);
            this.grammarTextBox.TabIndex = 7;
            this.grammarTextBox.Text = "";
            // 
            // detailsButton
            // 
            this.detailsButton.Location = new System.Drawing.Point(417, 531);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(100, 34);
            this.detailsButton.TabIndex = 8;
            this.detailsButton.Text = "Detalles";
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Visible = false;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // gneratorButtom
            // 
            this.gneratorButtom.Location = new System.Drawing.Point(192, 524);
            this.gneratorButtom.Name = "gneratorButtom";
            this.gneratorButtom.Size = new System.Drawing.Size(174, 41);
            this.gneratorButtom.TabIndex = 9;
            this.gneratorButtom.Text = "Generar Scanner";
            this.gneratorButtom.UseVisualStyleBackColor = true;
            this.gneratorButtom.Click += new System.EventHandler(this.gneratorButtom_Click);
            // 
            // AnalizarGramatica
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 609);
            this.Controls.Add(this.gneratorButtom);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.grammarTextBox);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.fileButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "AnalizarGramatica";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Analizar Gramatica";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AnalizarGramatica_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AnalizarGramatica_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button fileButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.RichTextBox grammarTextBox;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Button gneratorButtom;
    }
}