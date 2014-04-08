
//Multiple face detection and recognition in real time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Sergio Andrés Guitérrez Rojas
//"Serg3ant" for the delveloper comunity
// Sergiogut1805@hotmail.com
//Regards from Bucaramanga-Colombia ;)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading; 
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;
using System.IO;

using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;


using RedCorona.Net;

namespace MultiFaceRec
{
    public partial class FrameShooterDistance : Form
    {
        //declaration from PS for image outside        
        Image imgU;
        Image imgL;
        string curDir;
        string dirSave;
        MessageClient mc;
        Capture grabber;

        bool useNao = false;
        bool useWebcam = false;
        List<string> calledName = new List<string>();
        

        //signdetector
        static Image img2;
        DistanceDetector DisDetector;
        List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, Byte>>();
        List<Rectangle> boxList = new List<Rectangle>();
        List<Contour<Point>> contourSignFound=new List<Contour<Point>>();

        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrameU, currentFrameL;
        //Capture grabber;
        HaarCascade face;
        //HaarCascade eye;
        
        MCvFont font;        
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        
        
        string name, names = null;


        public FrameShooterDistance()
        {
            try
            {
                mc = new MessageClient();
                curDir = Directory.GetCurrentDirectory();
                img2 = Image.FromFile(String.Format("{0}/Resources/ImageStop/hexagon.png", Directory.GetCurrentDirectory()));
                DisDetector = new DistanceDetector(new Image<Bgr, byte>(new Bitmap(img2)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));                                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            InitializeComponent();

        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        /// <summary>
        /// button to start capture image
        /// </summary>
        private void ConnectWebcamButton_Click(object sender, EventArgs e)
        {

            
            //Initialize the capture device
            if (FileNameTextBox.Text == "")
            {
                grabber = new Capture();
                grabber.QueryFrame();
                useWebcam = true;
                useNao = false;
            }
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
        }

        private void connectLumenButton_Click(object sender, EventArgs e)
        {
            mc.Start();
            useNao = true;
            useWebcam = false;
            
            //Initialize the FrameGraber event            
            Application.Idle += new EventHandler(FrameGrabber);
            //ButtonConnectNao.Enabled = false;
        }

        void FrameGrabber(object sender, EventArgs e)
        {

            if (mc.isconnect() && useNao && !useWebcam)
            {
                try
                {
                    while (!mc.updatedUpper)
                    { }
                    //BitmapSource imageBitmap = BitmapSource.Create(320, 240, 96, 96, PixelFormats.Bgr24, BitmapPalettes.WebPalette, mc.getByte(), 320* 3); 
                    //currentFrame = new Image<Bgr, byte>(BitmapFromSource(imageBitmap)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    imgU = Image.FromStream(new MemoryStream(mc.getByte(0)));
                    currentFrameU = new Image<Bgr, byte>(new Bitmap(imgU)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    while (!mc.updatedLower)
                    { }
                    imgL = Image.FromStream(new MemoryStream(mc.getByte(1)));
                    currentFrameL = new Image<Bgr, byte>(new Bitmap(imgU)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                }
                catch (Exception ex)
                {

                }
            }
            else if (!useNao && useWebcam)
            {
                
                try
                {
                    //Get the current frame form capture device
                    currentFrameU = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //img = Image.FromFile(String.Format(FileNameTextBox.Text));
                    //currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Error FrameGrabber. Original error: " + ex.Message);
                }
            }
            else
            {
                imgU = Image.FromFile(String.Format(FileNameTextBox.Text));
                currentFrameU = new Image<Bgr, byte>(new Bitmap(imgU)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //img = Image.FromFile(String.Format("{0}/Resources/loading.jpg", curDir));
                //currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
            
            ImageBoxUpper.Image = currentFrameU;
            ImageBoxLower.Image = currentFrameL;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }
             
         private void FrameShooter_Load(object sender, EventArgs e)
        {

        }

        private void browse_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = curDir + "Resources/ImageStop";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileNameTextBox.Text= openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Bowse_click. Original error: " + ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {}

        private void label2_Click(object sender, EventArgs e)
        {}

        private void frameShoot_Click(object sender, EventArgs e)
        {
            try
            {
                boxList.Clear();
                stopSignList.Clear();
                //contourSignFound.Clear();                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Error FrameShoot_Click. Original error: " + ex.Message);
            }
            DisDetector.DetectStopSign(currentFrameU, stopSignList, boxList, contourSignFound);
            ImageBoxUpperResult.Image = DisDetector.imageGray;
            if (useNao)
            {
                try
                {
                    DisDetector.DetectStopSign(currentFrameL, stopSignList, boxList, contourSignFound);
                    ImageBoxLowerResult.Image = DisDetector.imageGray;
                }
                catch (Exception ex) {
                    Console.Write("Error: " + ex);
                }
            }
            try
            {
                area.Text = DisDetector.areas[0].ToString();
            }
            catch
            {
                area.Text = "No Sign Detected";
            }
            curDir = Directory.GetCurrentDirectory();
            dirSave = curDir + "/Resources/dirSave";
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }
            int fileCount = Directory.GetFiles(dirSave).Length;
            try
            {
                DisDetector.imageGray.Save(dirSave + "/" + fileCount.ToString() + " - area-" + area.Text + ".bmp");
            }
            catch { }

            //before your loop
            var csv = new StringBuilder();
            string filePath = dirSave + "/data.csv";
            var newLine="";

            if (!File.Exists(filePath))
            {
                newLine = string.Format("{0},{1},{2},{3},{4},{5}","tipe", "jarak", "sudutH","sudutV","area", Environment.NewLine);
                csv.Append(newLine);
                File.WriteAllText(filePath, csv.ToString());
                csv.Clear();
            }

            //in your loop
            string tipe = tipeTextBox.Text;
            string jarak = JarakTextBox.Text;
            string sudutH = SudutHTextBox.Text;
            string sudutV = SudutVTextBox.Text;
            string areaValue = area.Text;
            newLine = string.Format("{0},{1},{2},{3},{4},{5}",tipe, jarak, sudutH, sudutV, areaValue, Environment.NewLine);
            //csv.Append(newLine);
            foreach (PointF p in DisDetector.centerPoints)
            {
                Console.Write("X:"+p.X + " Y:" + p.Y);
            }
            //after your loop
            File.AppendAllText(filePath, newLine);
        }


        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }



            
    }

    
}