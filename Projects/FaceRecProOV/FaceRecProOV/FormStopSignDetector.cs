
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
    public partial class FormStopSignDetector : Form
    {
        //declaration from PS for image outside        
        Image img;
        string curDir;
        MessageClient mc;

        bool useNao = false;
        bool useWebcam = false;
        List<string> calledName = new List<string>();
        

        //signdetector
        static Image img2;
        SignDetector stopDetector;
        List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, Byte>>();
        List<Rectangle> boxList = new List<Rectangle>();
        List<Contour<Point>> contourSignFound=new List<Contour<Point>>();

        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        //Capture grabber;
        HaarCascade face;
        //HaarCascade eye;
        
        MCvFont font;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        
        string name, names = null;


        public FormStopSignDetector()
        {
            try
            {
                mc = new MessageClient();
                curDir = Directory.GetCurrentDirectory();
                img2 = Image.FromFile(String.Format("{0}/Resources/ImageStop/StopSignNorthAmerican.png", Directory.GetCurrentDirectory()));
                stopDetector = new SignDetector(new Image<Bgr, byte>(new Bitmap(img2)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));                                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            InitializeComponent();
            //Load haarcascades for face detection
            
            //eye = new HaarCascade("haarcascade_eye.xml");            

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

            useWebcam = true;
            useNao = false;

            FrameGrabber();
            //ButtonWebcam.Enabled = false;
        }

        void FrameGrabber()
        {

            
            //label4.Text = "";
            
            if (mc.isconnect() && useNao && !useWebcam)
            {
                try
                {
                    while (!mc.updatedUpper)
                    { }
                    img = Image.FromStream(new MemoryStream(mc.getByte(0)));
                    currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
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
                    //currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    img = Image.FromFile(String.Format(FileNameTextBox.Text));
                    currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            else
            {
                img = Image.FromFile(String.Format("{0}/Resources/loading.jpg", curDir));
                currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
            try
            {
                boxList.Clear();
                stopSignList.Clear();
                contourSignFound.Clear();
                //contourSignFound = new List<Contour<Point>>();
                stopDetector.DetectStopSign(currentFrame, stopSignList, boxList, contourSignFound);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }

            TrainedFace = stopDetector.imageGray;

            //Show the faces procesed and recognized
            foreach (Rectangle r in boxList)
            {
                currentFrame.Draw(r, new Bgr(Color.Red), 2);
            }

            
            try
            {
                //var icolor = currentFrame;
                //Image<Bgr, Byte> inewcolor = icolor;
                Image<Gray, Byte> ibw = TrainedFace;//icolor.Convert<Gray, Byte>();
                Gray tresh = new Gray(200);
                Gray treshLinking = new Gray(100);
                Image<Gray, Byte> ibw1 = ibw.Canny(tresh, treshLinking);
                ibw1._Dilate(3);
                ibw1._Erode(2);
                Contour<Point> contur = ibw1.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE);

                Bgr[] colors_list = new Bgr[20];
                colors_list[0] = new Bgr(138, 43, 226);
                colors_list[1] = new Bgr(165, 42, 42);
                colors_list[2] = new Bgr(95, 158, 160);
                colors_list[3] = new Bgr(127, 255, 0);
                colors_list[4] = new Bgr(210, 105, 30);
                colors_list[5] = new Bgr(220, 20, 60);
                colors_list[6] = new Bgr(0, 255, 255);
                colors_list[7] = new Bgr(0, 0, 139);
                colors_list[8] = new Bgr(0, 100, 0);
                colors_list[9] = new Bgr(139, 0, 139);
                colors_list[10] = new Bgr(255, 140, 0);
                colors_list[11] = new Bgr(178, 34, 34);
                colors_list[12] = new Bgr(255, 20, 147);
                colors_list[13] = new Bgr(255, 215, 0);
                colors_list[14] = new Bgr(0, 128, 0);
                colors_list[15] = new Bgr(173, 255, 47);
                colors_list[16] = new Bgr(240, 128, 128);
                colors_list[17] = new Bgr(144, 238, 144);
                colors_list[18] = new Bgr(106, 90, 205);
                colors_list[19] = new Bgr(255, 0, 0);

                int[] cls = new int[20];
                //foreach (Contour<Point> c in contourSignFound)
                //{
                //    currentFrame.Draw(c, colors_list[5], 2);
                //    TrainedFace.Draw(c, new Gray(150), 2);
                //    label2.Text = contur.Area.ToString();
                //}
                while (contur != null)
                {
                    int number;
                    do
                    {
                        number = RandomNumber(0, 19);
                    } while (cls.Contains(number));
                    currentFrame.Draw(contur, colors_list[number], 2);
                    TrainedFace.Draw(contur, new Gray(150), 2);
                    label2.Text = contur.Area.ToString();
                    contur = contur.HNext;
                    Thread.Sleep(50);
                }
                imageBoxFrameGrabber.Image = currentFrame;

                imageBox1.Image = TrainedFace;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error: {0}", ex));
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }
             
         

        private void StopSignDetector_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


    
    }

    
}