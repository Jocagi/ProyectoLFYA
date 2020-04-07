namespace Proyecto_LFYA
{
    partial class TreeDetails
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
            this.treePictureBox = new System.Windows.Forms.PictureBox();
            this.download = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.treePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // treePictureBox
            // 
            this.treePictureBox.BackgroundImage = global::Proyecto_LFYA.Properties.Resources.background_blue;
            this.treePictureBox.Image = global::Proyecto_LFYA.Properties.Resources.Gaticornio;
            this.treePictureBox.Location = new System.Drawing.Point(12, 12);
            this.treePictureBox.Name = "treePictureBox";
            this.treePictureBox.Size = new System.Drawing.Size(724, 600);
            this.treePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.treePictureBox.TabIndex = 0;
            this.treePictureBox.TabStop = false;
            this.treePictureBox.WaitOnLoad = true;
            // 
            // download
            // 
            this.download.Location = new System.Drawing.Point(12, 618);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(110, 43);
            this.download.TabIndex = 1;
            this.download.Text = "Descargar";
            this.download.UseVisualStyleBackColor = true;
            this.download.Click += new System.EventHandler(this.download_Click);
            // 
            // TreeDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1337, 665);
            this.Controls.Add(this.download);
            this.Controls.Add(this.treePictureBox);
            this.MaximizeBox = false;
            this.Name = "TreeDetails";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tree Details";
            ((System.ComponentModel.ISupportInitialize)(this.treePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox treePictureBox;
        private System.Windows.Forms.Button download;
    }
}