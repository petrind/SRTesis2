namespace My_EMGU_Program
{
    partial class FormSURFFeatureDetection
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
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lineImageBox = new System.Windows.Forms.PictureBox();
            this.circleImageBox = new System.Windows.Forms.PictureBox();
            this.triangleRectangleImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.originalImageBox.Location = new System.Drawing.Point(3, 3);
            this.originalImageBox.Name = "pictureBox1";
            this.originalImageBox.Size = new System.Drawing.Size(254, 205);
            this.originalImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.originalImageBox.TabIndex = 0;
            this.originalImageBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(98, 448);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(372, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.triangleRectangleImageBox);
            this.panel1.Controls.Add(this.circleImageBox);
            this.panel1.Controls.Add(this.lineImageBox);
            this.panel1.Controls.Add(this.originalImageBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(557, 430);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.lineImageBox.Location = new System.Drawing.Point(289, 3);
            this.lineImageBox.Name = "pictureBox2";
            this.lineImageBox.Size = new System.Drawing.Size(254, 205);
            this.lineImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.lineImageBox.TabIndex = 1;
            this.lineImageBox.TabStop = false;
            // 
            // pictureBox3
            // 
            this.circleImageBox.Location = new System.Drawing.Point(3, 214);
            this.circleImageBox.Name = "pictureBox3";
            this.circleImageBox.Size = new System.Drawing.Size(254, 205);
            this.circleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.circleImageBox.TabIndex = 2;
            this.circleImageBox.TabStop = false;
            // 
            // pictureBox4
            // 
            this.triangleRectangleImageBox.Location = new System.Drawing.Point(289, 214);
            this.triangleRectangleImageBox.Name = "pictureBox4";
            this.triangleRectangleImageBox.Size = new System.Drawing.Size(254, 205);
            this.triangleRectangleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.triangleRectangleImageBox.TabIndex = 3;
            this.triangleRectangleImageBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 483);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox triangleRectangleImageBox;
        private System.Windows.Forms.PictureBox circleImageBox;
        private System.Windows.Forms.PictureBox lineImageBox;
    }
}

