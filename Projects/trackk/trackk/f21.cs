using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;

using RedCorona.Net;


namespace trackk
{
    public partial class f21 : Form
    {
        System.Drawing.Image img;
        Bitmap imageGlobal;
        bool loadedImage2 = true, loadedImage3 = true;
        MessageClientObjRecognition mc = new MessageClientObjRecognition();
        bool useNao = false, useWebcam = false;
        string d = "";
        private FilterInfoCollection videoDevices;
        EuclideanColorFiltering filter = new EuclideanColorFiltering();
        Color color = Color.Black;
        GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
        BlobCounter blobCounter = new BlobCounter();
        int range = 120;
        public f21()
        {
            InitializeComponent();
           
            blobCounter.MinWidth = 10;
            blobCounter.MinHeight = 10;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            try
            {
                // enumerate video devices
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                // add all devices to combo
                foreach (FilterInfo device in videoDevices)
                {
                    camerasCombo.Items.Add(device.Name);
                }

                camerasCombo.SelectedIndex = 0;
            }
            catch (ApplicationException)
            {
                camerasCombo.Items.Add("No local capture devices");
                videoDevices = null;
            }

            Bitmap b = new Bitmap(320, 240);
           // Rectangle a = (Rectangle)r;
            Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
            Graphics g2 = Graphics.FromImage(b);
            pen1 = new Pen(Color.FromArgb(255, 0, 0), 3);
            g2.Clear(Color.White);
            g2.DrawLine(pen1, b.Width / 2, 0, b.Width / 2, b.Width);
            g2.DrawLine(pen1, b.Width, b.Height / 2, 0, b.Height / 2); 
            pictureBox1.Image = (System.Drawing.Image)b;
        }        

        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
        public static Bitmap ConvertTo24bppRgb(System.Drawing.Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        
        }
        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            if (useNao && !useWebcam)
            {
                Console.WriteLine(image.PixelFormat);
                //override the image from
                while (!mc.updated)
                { }
                img = System.Drawing.Image.FromStream(new MemoryStream(mc.getByte()),true);
                imageGlobal = ConvertTo24bppRgb(img);
                image = (Bitmap)imageGlobal.Clone();
                Console.WriteLine(image.PixelFormat);
                loadedImage2 = false;
                loadedImage3 = false;
            }
        }
        /// <summary>
        /// Image with lot of rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void videoSourcePlayer2_NewFrame(object sender, ref Bitmap image)
        {
            Console.WriteLine(image.PixelFormat);
            if (useNao && !useWebcam)
            {
                while (loadedImage2)
                { }
                image = (Bitmap)imageGlobal.Clone();
                loadedImage2 = true;
                
            }
                Bitmap objectsImage = null;
                Bitmap mImage = null;
                mImage = (Bitmap)image.Clone();
                filter.CenterColor = Color.FromArgb(color.ToArgb());
                filter.Radius = (short)range;

                objectsImage = (Bitmap)image.Clone();
                filter.ApplyInPlace(objectsImage);

                BitmapData objectsData = objectsImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, image.PixelFormat);
                UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
                objectsImage.UnlockBits(objectsData);


                blobCounter.ProcessImage(grayImage);
                Rectangle[] rects = blobCounter.GetObjectRectangles();

                if (rects.Length > 0)
                {

                    foreach (Rectangle objectRect in rects)
                    {
                        Graphics g = Graphics.FromImage(mImage);
                        using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }

                        g.Dispose();
                    }
                }

                image = mImage;
            
        }
        /// <summary>
        /// Image with biggest rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void videoSourcePlayer3_NewFrame(object sender, ref Bitmap image)
        {
            if (useNao && !useWebcam)
            {
                while (loadedImage3)
                { }
                image = (Bitmap)imageGlobal.Clone();
                loadedImage3 = true;
            }
                Bitmap objectsImage = null;

                // set center colol and radius
                filter.CenterColor = Color.FromArgb(color.ToArgb());
                filter.Radius = (short)range;
                // apply the filter
                objectsImage = (Bitmap)image.Clone();
                filter.ApplyInPlace(image);

                // lock image for further processing
                BitmapData objectsData = objectsImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, image.PixelFormat);

                // grayscaling
                UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));

                // unlock image
                objectsImage.UnlockBits(objectsData);

                // locate blobs 
                blobCounter.ProcessImage(grayImage);
                Rectangle[] rects = blobCounter.GetObjectRectangles();

                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];

                    // draw rectangle around derected object
                    Graphics g = Graphics.FromImage(image);

                    using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }
                    g.Dispose();
                    int objectX = objectRect.X + objectRect.Width / 2 - image.Width / 2;
                    int objectY = image.Height / 2 - (objectRect.Y + objectRect.Height / 2);
                    ParameterizedThreadStart t = new ParameterizedThreadStart(pFunction);
                    Thread aa = new Thread(t);
                    aa.Start(rects[0]);
                }
                Graphics g1 = Graphics.FromImage(image);
                Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
                g1.DrawLine(pen1, image.Width / 2, 0, image.Width / 2, image.Width);
                g1.DrawLine(pen1, image.Width, image.Height / 2, 0, image.Height / 2);
                g1.Dispose();
            
       }

  


       void pFunction(object r)
       {
           try
           {
          
           Bitmap b = new Bitmap(pictureBox1.Image);
           Rectangle a = (Rectangle)r;
           Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
           Graphics g2 = Graphics.FromImage(b);
           pen1 = new Pen(color, 3);
           // Brush b5 = null;
           SolidBrush b5 = new SolidBrush(color);
           //   g2.Clear(Color.Black);


           Font f = new Font(Font, FontStyle.Bold);

           g2.DrawString("o", f, b5, a.Location);
           g2.Dispose();
           pictureBox1.Image = (System.Drawing.Image)b;
           this.Invoke((MethodInvoker)delegate
               {
                   richTextBox1.Text = a.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
               });
           }
           catch (Exception faa)
           {
               Thread.CurrentThread.Abort();
           }


           Thread.CurrentThread.Abort();
       }
        
        private void startButton_Click(object sender, EventArgs e)
        {
            useNao = false;
            useWebcam = true;
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            // videoDevices = null;
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[camerasCombo.SelectedIndex].MonikerString);
            videoSource.DesiredFrameSize = new Size(320, 240);
            videoSource.DesiredFrameRate = 12;

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
            videoSourcePlayer2.VideoSource = videoSource;
            videoSourcePlayer2.Start();
            videoSourcePlayer3.VideoSource = videoSource;
            videoSourcePlayer3.Start();
            //groupBox1.Enabled = false;
        }

        private void connectNao_Click(object sender, EventArgs e)
        {
            mc.Start();
            useNao = true;
            useWebcam = false;
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            // videoDevices = null;
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[camerasCombo.SelectedIndex].MonikerString);
            videoSource.DesiredFrameSize = new Size(320, 240);
            videoSource.DesiredFrameRate = 12;

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
            videoSourcePlayer2.VideoSource = videoSource;
            videoSourcePlayer2.Start();
            videoSourcePlayer3.VideoSource = videoSource;
            videoSourcePlayer3.Start();
        }

        private void f21_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            groupBox1.Enabled = true;
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
        /// <summary>
        /// button for color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void colorButton_Click(object sender, EventArgs e)
        {
           
            colorDialog1.ShowDialog();
            color = colorDialog1.Color;
        }

        private void rangeUpDown(object sender, EventArgs e)
        {
            range = Convert.ToInt32(numericUpDown1.Value) ;
        }

        private void maxWidthUpDown(object sender, EventArgs e)
        {
            blobCounter.MaxWidth = Convert.ToInt32(numericUpDown2.Value);
        }

        private void minWidthUpDown(object sender, EventArgs e)
        {
            blobCounter.MinWidth  = Convert.ToInt32(numericUpDown3.Value);
        }


    }
           
         /// <summary>
        /// http://www.codeproject.com/Articles/12286/Simple-Client-server-Interactions-using-C
        /// </summary>
        class MessageClientObjRecognition
        {
            bool mcConnect = false;
            public delegate void ImageRetrievedHandler(object sender, EventArgs e);
            public event ImageRetrievedHandler ImageRetrieved;
            void OnImageRetrieved(EventArgs e)
            {
                if (ImageRetrieved != null)
                    ImageRetrieved(this, e);
            }

            byte[] ba;
            public bool updated = false;
            ClientInfo client;

            public void Start()
            {
                Socket sock = Sockets.CreateTCPSocket("localhost", 2345);
                client = new ClientInfo(sock, false); // Don't start receiving yet
                client.MessageType = MessageType.CodeAndLength;
                client.OnReadMessage += new ConnectionReadMessage(ReadMessage);
                client.BeginReceive();
                mcConnect = true;
            }


            void ReadMessage(ClientInfo ci, uint code, byte[] buf, int len)
            {
                if (code == ClientInfo.ImageCode)
                {
                    Console.WriteLine("Message length: " + len + ", code " + code.ToString("X8") + ", content:");
                    ba = new byte[len];
                    Array.Copy(buf, ba, len);
                    //Console.WriteLine("  " + ByteBuilder.FormatParameter(new Parameter(ba, ParameterType.Byte)));
                    updated = true;
                    OnImageRetrieved(EventArgs.Empty);
                }
            }
            public void sendCommand(string command)
            {
                client.SendMessage(ClientInfo.CommandCode, Encoding.UTF8.GetBytes(command));
            }
            public void sendVoice(string command)
            {
                client.SendMessage(ClientInfo.VoiceCode, Encoding.UTF8.GetBytes(command));
            }
            public bool isconnect()
            {
                return mcConnect;
            }
            public byte[] getByte()
            {
                updated = false;
                return ba;
            }

        }
}
