namespace Proyecto_LFYA
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.textExpression = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.text = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.resultadoExpresion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generar Arbol";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textExpression
            // 
            this.textExpression.Location = new System.Drawing.Point(254, 90);
            this.textExpression.Name = "textExpression";
            this.textExpression.Size = new System.Drawing.Size(191, 26);
            this.textExpression.TabIndex = 1;
            this.textExpression.Text = "a+@(a|h)\\.com#";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Recorrido:";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(347, 287);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(0, 20);
            this.labelResult.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Expresion Regular:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(246, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Texto:";
            // 
            // text
            // 
            this.text.Location = new System.Drawing.Point(254, 167);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(191, 26);
            this.text.TabIndex = 5;
            this.text.Text = "a";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(250, 334);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mensaje:";
            // 
            // resultadoExpresion
            // 
            this.resultadoExpresion.AutoSize = true;
            this.resultadoExpresion.Location = new System.Drawing.Point(330, 334);
            this.resultadoExpresion.Name = "resultadoExpresion";
            this.resultadoExpresion.Size = new System.Drawing.Size(0, 20);
            this.resultadoExpresion.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.resultadoExpresion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.text);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textExpression);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textExpression;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label resultadoExpresion;
    }
}

