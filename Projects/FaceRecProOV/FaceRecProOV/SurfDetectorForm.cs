
////Multiple face detection and recognition in real time
////Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
////Writed by Sergio Andrés Guitérrez Rojas
////"Serg3ant" for the delveloper comunity
//// Sergiogut1805@hotmail.com
////Regards from Bucaramanga-Colombia ;)

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Text;
//using System.Linq;
//using System.IO;
//using System.Diagnostics;

//using Emgu.CV;
//using Emgu.CV.Features2D;
//using Emgu.CV.Structure;
//using Emgu.Util;


//using RedCorona.Net;

//namespace MultiFaceRec
//{
//    public partial class SurfDetectorForm : Form
//    {
//        //declaration from PS for image outside        
//        Image img;
//        string curDir = Directory.GetCurrentDirectory();
//        MessageClientFaceRecognition mc = new MessageClientFaceRecognition();

//        bool useNao = false;
//        bool useWebcam = false;
//        List<string> calledName = new List<string>();
        

//        //signdetector
//        static Image img2 = Image.FromFile(String.Format("{0}/Resources/ImageStop/StopSignNorthAmerican.png", Directory.GetCurrentDirectory()));
//        SignDetector stopDetector = new SignDetector(new Image<Bgr, byte>(new Bitmap(img2)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));
//        List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, Byte>>();
//        List<Rectangle> boxList = new List<Rectangle>();

//        //Declararation of all variables, vectors and haarcascades
//        Image<Bgr, Byte> currentFrame;
//        //Capture grabber;
//        HaarCascade face;
//        //HaarCascade eye;
        
//        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
//        Image<Gray, byte> result, TrainedFace = null;
//        Image<Gray, byte> gray = null;
//        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
//        List<string> labels= new List<string>();
//        List<string> NamePersons = new List<string>();
//        int ContTrain, NumLabels, t;
        
//        string name, names = null;


//        public SurfDetectorForm()
//        {
//            InitializeComponent();
//            //Load haarcascades for face detection
//            face = new HaarCascade("haarcascade_frontalface_default.xml");
//            //eye = new HaarCascade("haarcascade_eye.xml");
//            try
//            {
//                //Load of previus trainned faces and labels for each image
//                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
//                string[] Labels = Labelsinfo.Split('%');
//                NumLabels = Convert.ToInt16(Labels[0]);
//                ContTrain = NumLabels;
//                string LoadFaces;

//                for (int tf = 1; tf < NumLabels+1; tf++)
//                {
//                    LoadFaces = "face" + tf + ".bmp";
//                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
//                    labels.Add(Labels[tf]);
//                }
            
//            }
//            catch(Exception e)
//            {
//                MessageBox.Show(e.ToString());
//                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Trained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//            }

//        }

//        /**
//         * button to start capture image
//         */

//        private void ConnectWebcamButton_Click(object sender, EventArgs e)
//        {

//            useWebcam = true;
//            useNao = false;

//            FrameGrabber();
//            //ButtonWebcam.Enabled = false;
//        }

//        void FrameGrabber()
//        {

            
//            //label4.Text = "";
//            NamePersons.Add("");
//            if (mc.isconnect() && useNao && !useWebcam)
//            {
//                while (!mc.updated)
//                { }
//                img = Image.FromStream(new MemoryStream(mc.getByte()));
//                currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
//            }
//            else if (!useNao && useWebcam)
//            {
//                try
//                {
//                    //Get the current frame form capture device
//                    //currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
//                    img = Image.FromFile(String.Format(FileNameTextBox.Text));
//                    currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
//                }
//            }
//            else
//            {
//                img = Image.FromFile(String.Format("{0}/Resources/loading.jpg", curDir));
//                currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
//            }
//            try
//            {
//                stopDetector.DetectStopSign(currentFrame, stopSignList, boxList,contourSignFound);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
//            }

//            TrainedFace = stopDetector.imageGray;

//            //Show the faces procesed and recognized
//            foreach (Rectangle r in boxList)
//            {
//                currentFrame.Draw(r, new Bgr(Color.Red), 2);
//            }

//            imageBoxFrameGrabber.Image = currentFrame;

//            imageBox1.Image = TrainedFace;

//        }

//        private void button3_Click(object sender, EventArgs e)
//        {
//            Process.Start("Donate.html");
//        }
             
//         /// <summary>
//        /// http://www.codeproject.com/Articles/12286/Simple-Client-server-Interactions-using-C
//        /// </summary>
//        class MessageClientFaceRecognition
//        {
//            bool mcConnect = false;
//            public delegate void ImageRetrievedHandler(object sender, EventArgs e);
//            public event ImageRetrievedHandler ImageRetrieved;
//            void OnImageRetrieved(EventArgs e)
//            {
//                if (ImageRetrieved != null)
//                    ImageRetrieved(this, e);
//            }

//            byte[] ba;
//            public bool updated = false;
//            ClientInfo client;

//            public void Start()
//            {
//                Socket sock = Sockets.CreateTCPSocket("localhost", 2345);
//                client = new ClientInfo(sock, false); // Don't start receiving yet
//                client.MessageType = MessageType.CodeAndLength;
//                client.OnReadMessage += new ConnectionReadMessage(ReadMessage);
//                client.BeginReceive();
//                mcConnect = true;
//            }


//            void ReadMessage(ClientInfo ci, uint code, byte[] buf, int len)
//            {
//                if (code == ClientInfo.ImageCode)
//                {
//                    Console.WriteLine("Message length: " + len + ", code " + code.ToString("X8") + ", content:");
//                    ba = new byte[len];
//                    Array.Copy(buf, ba, len);
//                    //Console.WriteLine("  " + ByteBuilder.FormatParameter(new Parameter(ba, ParameterType.Byte)));
//                    updated = true;
//                    OnImageRetrieved(EventArgs.Empty);
//                }
//            }
//            public void sendCommand(string command)
//            {
//                client.SendMessage(ClientInfo.CommandCode, Encoding.UTF8.GetBytes(command));
//            }
//            public void sendVoice(string command)
//            {
//                client.SendMessage(ClientInfo.VoiceCode, Encoding.UTF8.GetBytes(command));
//            }
//            public bool isconnect()
//            {
//                return mcConnect;
//            }
//            public byte[] getByte()
//            {
//                updated = false;
//                return ba;
//            }

//        }

//        private void SurfDetector_Load(object sender, EventArgs e)
//        {

//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
            
//            OpenFileDialog openFileDialog1 = new OpenFileDialog();

//            openFileDialog1.InitialDirectory = curDir + "Resources/ImageStop";
//            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
//            openFileDialog1.FilterIndex = 2;
//            openFileDialog1.RestoreDirectory = true;

//            if (openFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                try
//                {
//                    FileNameTextBox.Text= openFileDialog1.FileName;
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
//                }
//            }
//        }


    
//    }

    
//}