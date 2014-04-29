using System.Windows.Forms;
using Microsoft.Win32;

namespace MultiFaceRec
{
    partial class FrameShooterDistance
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            //base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonWebcam = new System.Windows.Forms.Button();
            this.ImageBoxUpper = new Emgu.CV.UI.ImageBox();
            this.ImageBoxUpperResult = new Emgu.CV.UI.ImageBox();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.connectLumenButton = new System.Windows.Forms.Button();
            this.RecognizeColorObjectButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpperResult)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonWebcam
            // 
            this.ButtonWebcam.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonWebcam.Location = new System.Drawing.Point(657, 384);
            this.ButtonWebcam.Name = "ButtonWebcam";
            this.ButtonWebcam.Size = new System.Drawing.Size(181, 31);
            this.ButtonWebcam.TabIndex = 18;
            this.ButtonWebcam.Text = "Use image and recognize";
            this.ButtonWebcam.UseVisualStyleBackColor = true;
            this.ButtonWebcam.Click += new System.EventHandler(this.ConnectWebcamButton_Click);
            // 
            // ImageBoxUpper
            // 
            this.ImageBoxUpper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageBoxUpper.Location = new System.Drawing.Point(12, 12);
            this.ImageBoxUpper.Name = "ImageBoxUpper";
            this.ImageBoxUpper.Size = new System.Drawing.Size(320, 240);
            this.ImageBoxUpper.TabIndex = 4;
            this.ImageBoxUpper.TabStop = false;
            // 
            // ImageBoxUpperResult
            // 
            this.ImageBoxUpperResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageBoxUpperResult.Location = new System.Drawing.Point(331, 12);
            this.ImageBoxUpperResult.Name = "ImageBoxUpperResult";
            this.ImageBoxUpperResult.Size = new System.Drawing.Size(320, 240);
            this.ImageBoxUpperResult.TabIndex = 5;
            this.ImageBoxUpperResult.TabStop = false;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(657, 358);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(182, 20);
            this.FileNameTextBox.TabIndex = 19;
            this.FileNameTextBox.TextChanged += new System.EventHandler(this.FileNameTextBox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(657, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.browse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(657, 221);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 23;
            this.button2.Text = "Board Recog";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.frameShoot_Click);
            // 
            // connectLumenButton
            // 
            this.connectLumenButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectLumenButton.Location = new System.Drawing.Point(657, 292);
            this.connectLumenButton.Name = "connectLumenButton";
            this.connectLumenButton.Size = new System.Drawing.Size(76, 31);
            this.connectLumenButton.TabIndex = 32;
            this.connectLumenButton.Text = "Connect to Lumen Server";
            this.connectLumenButton.UseVisualStyleBackColor = true;
            this.connectLumenButton.Click += new System.EventHandler(this.connectLumenButton_Click);
            // 
            // RecognizeColorObjectButton
            // 
            this.RecognizeColorObjectButton.Location = new System.Drawing.Point(657, 263);
            this.RecognizeColorObjectButton.Name = "RecognizeColorObjectButton";
            this.RecognizeColorObjectButton.Size = new System.Drawing.Size(75, 23);
            this.RecognizeColorObjectButton.TabIndex = 33;
            this.RecognizeColorObjectButton.Text = "Peon Recog";
            this.RecognizeColorObjectButton.UseVisualStyleBackColor = true;
            this.RecognizeColorObjectButton.Click += new System.EventHandler(this.PeonRecog_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(657, 177);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 38);
            this.button3.TabIndex = 34;
            this.button3.Text = "Setting Mask";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // FrameShooterDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 516);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.RecognizeColorObjectButton);
            this.Controls.Add(this.connectLumenButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.ImageBoxUpperResult);
            this.Controls.Add(this.ButtonWebcam);
            this.Controls.Add(this.ImageBoxUpper);
            this.Name = "FrameShooterDistance";
            this.Text = "Petrus\'s Distance detector";
            this.Load += new System.EventHandler(this.FrameShooter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpperResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox ImageBoxUpper;
        private System.Windows.Forms.Button ButtonWebcam;
        private Emgu.CV.UI.ImageBox ImageBoxUpperResult;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Button button1;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private Button connectLumenButton;
        private Button RecognizeColorObjectButton;
        private Button button3;

                          
    }
}

