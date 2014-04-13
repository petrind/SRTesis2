namespace MultiFaceRec
{
    partial class FrmPrincipal
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
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddFace = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonConnectNao = new System.Windows.Forms.Button();
            this.ButtonLeft = new System.Windows.Forms.Button();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.ButtonRight = new System.Windows.Forms.Button();
            this.ButtonUp = new System.Windows.Forms.Button();
            this.ButtonDown = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.sayTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ButtonSay = new System.Windows.Forms.Button();
            this.ButtonWebcam = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.buttonAddFace.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonAddFace.Location = new System.Drawing.Point(87, 201);
            this.buttonAddFace.Name = "button2";
            this.buttonAddFace.Size = new System.Drawing.Size(87, 31);
            this.buttonAddFace.TabIndex = 3;
            this.buttonAddFace.Text = "2. Add face";
            this.buttonAddFace.UseVisualStyleBackColor = true;
            this.buttonAddFace.Click += new System.EventHandler(this.TrainFaceButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(67, 170);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(107, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.imageBox1);
            this.groupBox1.Controls.Add(this.buttonAddFace);
            this.groupBox1.Location = new System.Drawing.Point(342, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 242);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Training: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name: ";
            // 
            // imageBox1
            // 
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.Location = new System.Drawing.Point(11, 18);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(163, 134);
            this.imageBox1.TabIndex = 5;
            this.imageBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonWebcam);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.ButtonConnectNao);
            this.groupBox2.Location = new System.Drawing.Point(532, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 242);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(9, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Persons present in the scene:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(9, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 19);
            this.label4.TabIndex = 16;
            this.label4.Text = "Nobody";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(163, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Number of faces detected: ";
            // 
            // button1
            // 
            this.ButtonConnectNao.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonConnectNao.Location = new System.Drawing.Point(13, 201);
            this.ButtonConnectNao.Name = "button1";
            this.ButtonConnectNao.Size = new System.Drawing.Size(181, 31);
            this.ButtonConnectNao.TabIndex = 2;
            this.ButtonConnectNao.Text = "1. Connect and recognize";
            this.ButtonConnectNao.UseVisualStyleBackColor = true;
            this.ButtonConnectNao.Click += new System.EventHandler(this.ConnectNaoButton_Click);
            // 
            // LeftButton
            // 
            this.ButtonLeft.Location = new System.Drawing.Point(13, 314);
            this.ButtonLeft.Name = "LeftButton";
            this.ButtonLeft.Size = new System.Drawing.Size(75, 23);
            this.ButtonLeft.TabIndex = 18;
            this.ButtonLeft.Text = "Left";
            this.ButtonLeft.UseVisualStyleBackColor = true;
            this.ButtonLeft.Enabled = false;
            this.ButtonLeft.Click += new System.EventHandler(this.HeadLeftButtonClick);
            // 
            // imageBoxFrameGrabber
            // 
            this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxFrameGrabber.Location = new System.Drawing.Point(12, 12);
            this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
            this.imageBoxFrameGrabber.Size = new System.Drawing.Size(320, 240);
            this.imageBoxFrameGrabber.TabIndex = 4;
            this.imageBoxFrameGrabber.TabStop = false;
            // 
            // RightButton
            // 
            this.ButtonRight.Location = new System.Drawing.Point(94, 314);
            this.ButtonRight.Name = "RightButton";
            this.ButtonRight.Size = new System.Drawing.Size(75, 23);
            this.ButtonRight.TabIndex = 19;
            this.ButtonRight.Text = "Right";
            this.ButtonRight.UseVisualStyleBackColor = true;
            this.ButtonRight.Enabled = false;
            this.ButtonRight.Click += new System.EventHandler(this.HeadRightButton_Click);
            // 
            // UpButton
            // 
            this.ButtonUp.Location = new System.Drawing.Point(53, 285);
            this.ButtonUp.Name = "UpButton";
            this.ButtonUp.Size = new System.Drawing.Size(75, 23);
            this.ButtonUp.TabIndex = 20;
            this.ButtonUp.Text = "Up";
            this.ButtonUp.Enabled = false;
            this.ButtonUp.UseVisualStyleBackColor = true;
            this.ButtonUp.Click += new System.EventHandler(this.HeadUpButton_Click);
            // 
            // DownButton
            // 
            this.ButtonDown.Location = new System.Drawing.Point(53, 343);
            this.ButtonDown.Name = "DownButton";
            this.ButtonDown.Size = new System.Drawing.Size(75, 23);
            this.ButtonDown.TabIndex = 21;
            this.ButtonDown.Text = "Down";
            this.ButtonDown.Enabled = false;
            this.ButtonDown.UseVisualStyleBackColor = true;
            this.ButtonDown.Click += new System.EventHandler(this.HeadDownButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Head ";
            // 
            // sayTextBox
            // 
            this.sayTextBox.Location = new System.Drawing.Point(225, 287);
            this.sayTextBox.Multiline = true;
            this.sayTextBox.Name = "sayTextBox";
            this.sayTextBox.Size = new System.Drawing.Size(158, 79);
            this.sayTextBox.Enabled = false;
            this.sayTextBox.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 269);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Say: ";
            // 
            // button3
            // 
            this.ButtonSay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonSay.Location = new System.Drawing.Point(296, 372);
            this.ButtonSay.Name = "button3";
            this.ButtonSay.Size = new System.Drawing.Size(87, 31);
            this.ButtonSay.TabIndex = 9;
            this.ButtonSay.Text = "Send";
            this.ButtonSay.UseVisualStyleBackColor = true;
            this.ButtonSay.Enabled = false;
            this.ButtonSay.Click += new System.EventHandler(this.sayButton_Click);
            // 
            // 
            // 
            this.ButtonWebcam.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonWebcam.Location = new System.Drawing.Point(12, 159);
            this.ButtonWebcam.Name = "button4";
            this.ButtonWebcam.Size = new System.Drawing.Size(181, 31);
            this.ButtonWebcam.TabIndex = 18;
            this.ButtonWebcam.Text = "1. Use Webcam and recognize";
            this.ButtonWebcam.UseVisualStyleBackColor = true;
            this.ButtonWebcam.Click += new System.EventHandler(this.ConnectWebcamButton_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 414);
            this.Controls.Add(this.ButtonSay);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.sayTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ButtonDown);
            this.Controls.Add(this.ButtonUp);
            this.Controls.Add(this.ButtonRight);
            this.Controls.Add(this.ButtonLeft);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.imageBoxFrameGrabber);
            this.Name = "FrmPrincipal";
            this.Text = "face detector and recgonizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddFace;
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonConnectNao;
        private System.Windows.Forms.Button ButtonLeft;
        private System.Windows.Forms.Button ButtonRight;
        private System.Windows.Forms.Button ButtonUp;
        private System.Windows.Forms.Button ButtonDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sayTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button ButtonSay;
        private System.Windows.Forms.Button ButtonWebcam;
    }
}

