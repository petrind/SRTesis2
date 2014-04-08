namespace MultiFaceRec
{
    partial class FormCameraCalibNao
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
            this.Main_Picturebox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.useWebCamButton = new System.Windows.Forms.Button();
            this.useNaoButton = new System.Windows.Forms.Button();
            this.Start_BTN = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Sub_PicturBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Main_Picturebox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sub_PicturBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Main_Picturebox
            // 
            this.Main_Picturebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Main_Picturebox.Location = new System.Drawing.Point(3, 3);
            this.Main_Picturebox.Name = "Main_Picturebox";
            this.Main_Picturebox.Size = new System.Drawing.Size(641, 440);
            this.Main_Picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Main_Picturebox.TabIndex = 0;
            this.Main_Picturebox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.Main_Picturebox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 446);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.useWebCamButton);
            this.panel1.Controls.Add(this.useNaoButton);
            this.panel1.Controls.Add(this.Start_BTN);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Sub_PicturBox);
            this.panel1.Location = new System.Drawing.Point(650, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 440);
            this.panel1.TabIndex = 1;
            // 
            // useWebCam
            // 
            this.useWebCamButton.Location = new System.Drawing.Point(96, 144);
            this.useWebCamButton.Name = "useWebCam";
            this.useWebCamButton.Size = new System.Drawing.Size(89, 23);
            this.useWebCamButton.TabIndex = 7;
            this.useWebCamButton.Text = "Use Webcam";
            this.useWebCamButton.UseVisualStyleBackColor = true;
            this.useWebCamButton.Click += new System.EventHandler(this.useWebCam_Click);
            // 
            // useNaoButton
            // 
            this.useNaoButton.Location = new System.Drawing.Point(96, 115);
            this.useNaoButton.Name = "useNaoButton";
            this.useNaoButton.Size = new System.Drawing.Size(89, 23);
            this.useNaoButton.TabIndex = 6;
            this.useNaoButton.Text = "use Nao";
            this.useNaoButton.UseVisualStyleBackColor = true;
            this.useNaoButton.Click += new System.EventHandler(this.useNaoButton_Click);
            // 
            // Start_BTN
            // 
            this.Start_BTN.Location = new System.Drawing.Point(110, 74);
            this.Start_BTN.Name = "Start_BTN";
            this.Start_BTN.Size = new System.Drawing.Size(75, 23);
            this.Start_BTN.TabIndex = 5;
            this.Start_BTN.Text = "Go...";
            this.Start_BTN.UseVisualStyleBackColor = true;
            this.Start_BTN.Click += new System.EventHandler(this.Start_BTN_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(96, 34);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(89, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Frame Buffer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Set Up";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original";
            // 
            // Sub_PicturBox
            // 
            this.Sub_PicturBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Sub_PicturBox.Location = new System.Drawing.Point(3, 249);
            this.Sub_PicturBox.Name = "Sub_PicturBox";
            this.Sub_PicturBox.Size = new System.Drawing.Size(188, 188);
            this.Sub_PicturBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Sub_PicturBox.TabIndex = 0;
            this.Sub_PicturBox.TabStop = false;
            // 
            // FormCameraCalibNao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 470);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormCameraCalibNao";
            this.Text = "Camera Caibration";
            ((System.ComponentModel.ISupportInitialize)(this.Main_Picturebox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sub_PicturBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Main_Picturebox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Sub_PicturBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Start_BTN;
        private System.Windows.Forms.Button useWebCamButton;
        private System.Windows.Forms.Button useNaoButton;
    }
}

