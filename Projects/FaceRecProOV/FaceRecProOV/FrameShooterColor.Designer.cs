using System.Windows.Forms;
using Microsoft.Win32;

namespace MultiFaceRec
{
    partial class FrameShooterColor
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
            this.label1 = new System.Windows.Forms.Label();
            this.area = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.JarakLabel = new System.Windows.Forms.Label();
            this.SudutHorizontalLabel = new System.Windows.Forms.Label();
            this.SudutVertikalLabel = new System.Windows.Forms.Label();
            this.JarakTextBox = new System.Windows.Forms.TextBox();
            this.SudutHTextBox = new System.Windows.Forms.TextBox();
            this.SudutVTextBox = new System.Windows.Forms.TextBox();
            this.tipeLabel = new System.Windows.Forms.Label();
            this.tipeTextBox = new System.Windows.Forms.TextBox();
            this.connectLumenButton = new System.Windows.Forms.Button();
            this.HueMinTB = new System.Windows.Forms.TrackBar();
            this.HueMaxTB = new System.Windows.Forms.TrackBar();
            this.SMinTB = new System.Windows.Forms.TrackBar();
            this.SMaxTB = new System.Windows.Forms.TrackBar();
            this.VMinTB = new System.Windows.Forms.TrackBar();
            this.VMaxTB = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.HueMinLabel = new System.Windows.Forms.Label();
            this.HueMaxLabel = new System.Windows.Forms.Label();
            this.SMinLabel = new System.Windows.Forms.Label();
            this.SMaxLabel = new System.Windows.Forms.Label();
            this.VMaxLabel = new System.Windows.Forms.Label();
            this.VMinLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpperResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HueMinTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HueMaxTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SMinTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SMaxTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VMinTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VMaxTB)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonWebcam
            // 
            this.ButtonWebcam.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonWebcam.Location = new System.Drawing.Point(657, 354);
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
            this.ImageBoxUpper.Location = new System.Drawing.Point(8, 12);
            this.ImageBoxUpper.Name = "ImageBoxUpper";
            this.ImageBoxUpper.Size = new System.Drawing.Size(320, 240);
            this.ImageBoxUpper.TabIndex = 4;
            this.ImageBoxUpper.TabStop = false;
            // 
            // ImageBoxUpperResult
            // 
            this.ImageBoxUpperResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageBoxUpperResult.Location = new System.Drawing.Point(332, 13);
            this.ImageBoxUpperResult.Name = "ImageBoxUpperResult";
            this.ImageBoxUpperResult.Size = new System.Drawing.Size(320, 239);
            this.ImageBoxUpperResult.TabIndex = 5;
            this.ImageBoxUpperResult.TabStop = false;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(657, 328);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(182, 20);
            this.FileNameTextBox.TabIndex = 19;
            this.FileNameTextBox.TextChanged += new System.EventHandler(this.FileNameTextBox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(657, 299);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(657, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Area";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // area
            // 
            this.area.AutoSize = true;
            this.area.Location = new System.Drawing.Point(659, 30);
            this.area.Name = "area";
            this.area.Size = new System.Drawing.Size(59, 13);
            this.area.TabIndex = 22;
            this.area.Text = "Area Value";
            this.area.Click += new System.EventHandler(this.label2_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(657, 229);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "Shoot Frame";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.frameShoot_Click);
            // 
            // JarakLabel
            // 
            this.JarakLabel.AutoSize = true;
            this.JarakLabel.Location = new System.Drawing.Point(660, 90);
            this.JarakLabel.Name = "JarakLabel";
            this.JarakLabel.Size = new System.Drawing.Size(33, 13);
            this.JarakLabel.TabIndex = 24;
            this.JarakLabel.Text = "Jarak";
            // 
            // SudutHorizontalLabel
            // 
            this.SudutHorizontalLabel.AutoSize = true;
            this.SudutHorizontalLabel.Location = new System.Drawing.Point(660, 130);
            this.SudutHorizontalLabel.Name = "SudutHorizontalLabel";
            this.SudutHorizontalLabel.Size = new System.Drawing.Size(85, 13);
            this.SudutHorizontalLabel.TabIndex = 25;
            this.SudutHorizontalLabel.Text = "Sudut Horizontal";
            // 
            // SudutVertikalLabel
            // 
            this.SudutVertikalLabel.AutoSize = true;
            this.SudutVertikalLabel.Location = new System.Drawing.Point(660, 179);
            this.SudutVertikalLabel.Name = "SudutVertikalLabel";
            this.SudutVertikalLabel.Size = new System.Drawing.Size(73, 13);
            this.SudutVertikalLabel.TabIndex = 26;
            this.SudutVertikalLabel.Text = "Sudut Vertikal";
            // 
            // JarakTextBox
            // 
            this.JarakTextBox.Location = new System.Drawing.Point(663, 107);
            this.JarakTextBox.Name = "JarakTextBox";
            this.JarakTextBox.Size = new System.Drawing.Size(78, 20);
            this.JarakTextBox.TabIndex = 27;
            // 
            // SudutHTextBox
            // 
            this.SudutHTextBox.Location = new System.Drawing.Point(663, 145);
            this.SudutHTextBox.Name = "SudutHTextBox";
            this.SudutHTextBox.Size = new System.Drawing.Size(78, 20);
            this.SudutHTextBox.TabIndex = 28;
            // 
            // SudutVTextBox
            // 
            this.SudutVTextBox.Location = new System.Drawing.Point(663, 195);
            this.SudutVTextBox.Name = "SudutVTextBox";
            this.SudutVTextBox.Size = new System.Drawing.Size(78, 20);
            this.SudutVTextBox.TabIndex = 29;
            // 
            // tipeLabel
            // 
            this.tipeLabel.AutoSize = true;
            this.tipeLabel.Location = new System.Drawing.Point(657, 51);
            this.tipeLabel.Name = "tipeLabel";
            this.tipeLabel.Size = new System.Drawing.Size(28, 13);
            this.tipeLabel.TabIndex = 30;
            this.tipeLabel.Text = "Tipe";
            // 
            // tipeTextBox
            // 
            this.tipeTextBox.Location = new System.Drawing.Point(663, 67);
            this.tipeTextBox.Name = "tipeTextBox";
            this.tipeTextBox.Size = new System.Drawing.Size(78, 20);
            this.tipeTextBox.TabIndex = 31;
            // 
            // connectLumenButton
            // 
            this.connectLumenButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectLumenButton.Location = new System.Drawing.Point(657, 262);
            this.connectLumenButton.Name = "connectLumenButton";
            this.connectLumenButton.Size = new System.Drawing.Size(76, 31);
            this.connectLumenButton.TabIndex = 32;
            this.connectLumenButton.Text = "Connect to Lumen Server";
            this.connectLumenButton.UseVisualStyleBackColor = true;
            this.connectLumenButton.Click += new System.EventHandler(this.connectLumenButton_Click);
            // 
            // HueMinTB
            // 
            this.HueMinTB.Location = new System.Drawing.Point(16, 280);
            this.HueMinTB.Maximum = 180;
            this.HueMinTB.Name = "HueMinTB";
            this.HueMinTB.Size = new System.Drawing.Size(319, 45);
            this.HueMinTB.TabIndex = 34;
            this.HueMinTB.ValueChanged += new System.EventHandler(this.HMinChanged);
            // 
            // HueMaxTB
            // 
            this.HueMaxTB.Location = new System.Drawing.Point(16, 319);
            this.HueMaxTB.Maximum = 180;
            this.HueMaxTB.Name = "HueMaxTB";
            this.HueMaxTB.Size = new System.Drawing.Size(319, 45);
            this.HueMaxTB.TabIndex = 35;
            this.HueMaxTB.ValueChanged += new System.EventHandler(this.HMaxChanged);
            // 
            // SMinTB
            // 
            this.SMinTB.Location = new System.Drawing.Point(16, 388);
            this.SMinTB.Maximum = 255;
            this.SMinTB.Name = "SMinTB";
            this.SMinTB.Size = new System.Drawing.Size(319, 45);
            this.SMinTB.TabIndex = 36;
            this.SMinTB.ValueChanged += new System.EventHandler(this.SMinChanged);
            // 
            // SMaxTB
            // 
            this.SMaxTB.Location = new System.Drawing.Point(16, 426);
            this.SMaxTB.Maximum = 255;
            this.SMaxTB.Name = "SMaxTB";
            this.SMaxTB.Size = new System.Drawing.Size(319, 45);
            this.SMaxTB.TabIndex = 37;
            this.SMaxTB.ValueChanged += new System.EventHandler(this.SMaxChanged);
            // 
            // VMinTB
            // 
            this.VMinTB.Location = new System.Drawing.Point(359, 388);
            this.VMinTB.Maximum = 255;
            this.VMinTB.Name = "VMinTB";
            this.VMinTB.Size = new System.Drawing.Size(319, 45);
            this.VMinTB.TabIndex = 38;
            this.VMinTB.ValueChanged += new System.EventHandler(this.VMinChanged);
            // 
            // VMaxTB
            // 
            this.VMaxTB.Location = new System.Drawing.Point(359, 426);
            this.VMaxTB.Maximum = 255;
            this.VMaxTB.Name = "VMaxTB";
            this.VMaxTB.Size = new System.Drawing.Size(319, 45);
            this.VMaxTB.TabIndex = 39;
            this.VMaxTB.ValueChanged += new System.EventHandler(this.VMaxChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Hue";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 372);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Saturation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(365, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "Value";
            // 
            // HueMinLabel
            // 
            this.HueMinLabel.AutoSize = true;
            this.HueMinLabel.Location = new System.Drawing.Point(340, 290);
            this.HueMinLabel.Name = "HueMinLabel";
            this.HueMinLabel.Size = new System.Drawing.Size(44, 13);
            this.HueMinLabel.TabIndex = 43;
            this.HueMinLabel.Text = "HueMin";
            // 
            // HueMaxLabel
            // 
            this.HueMaxLabel.AutoSize = true;
            this.HueMaxLabel.Location = new System.Drawing.Point(340, 326);
            this.HueMaxLabel.Name = "HueMaxLabel";
            this.HueMaxLabel.Size = new System.Drawing.Size(47, 13);
            this.HueMaxLabel.TabIndex = 44;
            this.HueMaxLabel.Text = "HueMax";
            // 
            // SMinLabel
            // 
            this.SMinLabel.AutoSize = true;
            this.SMinLabel.Location = new System.Drawing.Point(101, 372);
            this.SMinLabel.Name = "SMinLabel";
            this.SMinLabel.Size = new System.Drawing.Size(31, 13);
            this.SMinLabel.TabIndex = 45;
            this.SMinLabel.Text = "SMin";
            // 
            // SMaxLabel
            // 
            this.SMaxLabel.AutoSize = true;
            this.SMaxLabel.Location = new System.Drawing.Point(230, 372);
            this.SMaxLabel.Name = "SMaxLabel";
            this.SMaxLabel.Size = new System.Drawing.Size(34, 13);
            this.SMaxLabel.TabIndex = 46;
            this.SMaxLabel.Text = "SMax";
            // 
            // VMaxLabel
            // 
            this.VMaxLabel.AutoSize = true;
            this.VMaxLabel.Location = new System.Drawing.Point(425, 372);
            this.VMaxLabel.Name = "VMaxLabel";
            this.VMaxLabel.Size = new System.Drawing.Size(24, 13);
            this.VMaxLabel.TabIndex = 48;
            this.VMaxLabel.Text = "Min";
            // 
            // VMinLabel
            // 
            this.VMinLabel.AutoSize = true;
            this.VMinLabel.Location = new System.Drawing.Point(567, 372);
            this.VMinLabel.Name = "VMinLabel";
            this.VMinLabel.Size = new System.Drawing.Size(27, 13);
            this.VMinLabel.TabIndex = 47;
            this.VMinLabel.Text = "Max";
            // 
            // FrameShooterColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 471);
            this.Controls.Add(this.VMaxLabel);
            this.Controls.Add(this.VMinLabel);
            this.Controls.Add(this.SMaxLabel);
            this.Controls.Add(this.SMinLabel);
            this.Controls.Add(this.HueMaxLabel);
            this.Controls.Add(this.HueMinLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.VMaxTB);
            this.Controls.Add(this.VMinTB);
            this.Controls.Add(this.SMaxTB);
            this.Controls.Add(this.SMinTB);
            this.Controls.Add(this.HueMaxTB);
            this.Controls.Add(this.HueMinTB);
            this.Controls.Add(this.connectLumenButton);
            this.Controls.Add(this.tipeTextBox);
            this.Controls.Add(this.tipeLabel);
            this.Controls.Add(this.SudutVTextBox);
            this.Controls.Add(this.SudutHTextBox);
            this.Controls.Add(this.JarakTextBox);
            this.Controls.Add(this.SudutVertikalLabel);
            this.Controls.Add(this.SudutHorizontalLabel);
            this.Controls.Add(this.JarakLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.area);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.ImageBoxUpperResult);
            this.Controls.Add(this.ButtonWebcam);
            this.Controls.Add(this.ImageBoxUpper);
            this.Name = "FrameShooterColor";
            this.Text = "Petrus\'s Distance detector";
            this.Load += new System.EventHandler(this.FrameShooter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxUpperResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HueMinTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HueMaxTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SMinTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SMaxTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VMinTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VMaxTB)).EndInit();
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
        private Label label1;
        private Label area;
        private Button button2;
        private Label JarakLabel;
        private Label SudutHorizontalLabel;
        private Label SudutVertikalLabel;
        private TextBox JarakTextBox;
        private TextBox SudutHTextBox;
        private TextBox SudutVTextBox;
        private Label tipeLabel;
        private TextBox tipeTextBox;
        private Button connectLumenButton;
        private TrackBar HueMinTB;
        private TrackBar HueMaxTB;
        private TrackBar SMinTB;
        private TrackBar SMaxTB;
        private TrackBar VMinTB;
        private TrackBar VMaxTB;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label HueMinLabel;
        private Label HueMaxLabel;
        private Label SMinLabel;
        private Label SMaxLabel;
        private Label VMaxLabel;
        private Label VMinLabel;

                          
    }
}

