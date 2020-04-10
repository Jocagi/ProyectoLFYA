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
            this.download = new System.Windows.Forms.Button();
            this.expressionTextBox = new System.Windows.Forms.RichTextBox();
            this.TreeData = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.followData = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.transitionData = new System.Windows.Forms.DataGridView();
            this.treePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.TreeData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.followData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transitionData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treePictureBox)).BeginInit();
            this.SuspendLayout();
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
            // expressionTextBox
            // 
            this.expressionTextBox.Font = new System.Drawing.Font("Sitka Text", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expressionTextBox.Location = new System.Drawing.Point(13, 13);
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.ReadOnly = true;
            this.expressionTextBox.Size = new System.Drawing.Size(723, 68);
            this.expressionTextBox.TabIndex = 2;
            this.expressionTextBox.Text = "";
            // 
            // TreeData
            // 
            this.TreeData.AllowUserToAddRows = false;
            this.TreeData.AllowUserToDeleteRows = false;
            this.TreeData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TreeData.Location = new System.Drawing.Point(757, 36);
            this.TreeData.Name = "TreeData";
            this.TreeData.ReadOnly = true;
            this.TreeData.RowTemplate.Height = 28;
            this.TreeData.RowTemplate.ReadOnly = true;
            this.TreeData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TreeData.Size = new System.Drawing.Size(687, 150);
            this.TreeData.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(757, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Functions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(757, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Follows";
            // 
            // followData
            // 
            this.followData.AllowUserToAddRows = false;
            this.followData.AllowUserToDeleteRows = false;
            this.followData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.followData.Location = new System.Drawing.Point(757, 228);
            this.followData.Name = "followData";
            this.followData.ReadOnly = true;
            this.followData.RowTemplate.Height = 28;
            this.followData.Size = new System.Drawing.Size(687, 179);
            this.followData.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(761, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Transitions";
            // 
            // transitionData
            // 
            this.transitionData.AllowUserToAddRows = false;
            this.transitionData.AllowUserToDeleteRows = false;
            this.transitionData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.transitionData.Location = new System.Drawing.Point(761, 444);
            this.transitionData.Name = "transitionData";
            this.transitionData.ReadOnly = true;
            this.transitionData.RowTemplate.Height = 28;
            this.transitionData.Size = new System.Drawing.Size(683, 217);
            this.transitionData.TabIndex = 7;
            // 
            // treePictureBox
            // 
            this.treePictureBox.BackgroundImage = global::Proyecto_LFYA.Properties.Resources.background_blue;
            this.treePictureBox.Image = global::Proyecto_LFYA.Properties.Resources.loading_cat;
            this.treePictureBox.Location = new System.Drawing.Point(12, 87);
            this.treePictureBox.Name = "treePictureBox";
            this.treePictureBox.Size = new System.Drawing.Size(724, 525);
            this.treePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.treePictureBox.TabIndex = 0;
            this.treePictureBox.TabStop = false;
            this.treePictureBox.WaitOnLoad = true;
            // 
            // TreeDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1924, 697);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.transitionData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.followData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TreeData);
            this.Controls.Add(this.expressionTextBox);
            this.Controls.Add(this.download);
            this.Controls.Add(this.treePictureBox);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1735, 753);
            this.Name = "TreeDetails";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tree Details";
            this.Load += new System.EventHandler(this.TreeDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TreeData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.followData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transitionData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox treePictureBox;
        private System.Windows.Forms.Button download;
        private System.Windows.Forms.RichTextBox expressionTextBox;
        private System.Windows.Forms.DataGridView TreeData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView followData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView transitionData;
    }
}