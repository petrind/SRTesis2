
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
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;


using RedCorona.Net;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //declaration from PS for image outside        
        Image img;
        string curDir = Directory.GetCurrentDirectory();
        MessageClient mc = new MessageClient();
        bool imgUpdate = false;        
        bool called = false;
        int calledNumber = 0;
        bool useNao = false;
        bool useWebcam = false;
        int faceIteration = 3;
        List<string> calledName = new List<string>();
        
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        //HaarCascade eye;
        
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        
        string name, names = null;


        public FrmPrincipal()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels+1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Trained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        /**
         * button to start capture image
         */
        private void ConnectNaoButton_Click(object sender, EventArgs e)
        {
            mc.Start();
            useNao = true;
            useWebcam = false;

            ButtonUp.Enabled = true;
            ButtonDown.Enabled = true;
            ButtonLeft.Enabled = true;
            ButtonRight.Enabled = true;
            ButtonSay.Enabled = true;
            sayTextBox.Enabled = true;

            //Initialize the FrameGraber event            
            Application.Idle += new EventHandler(FrameGrabber);
            ButtonConnectNao.Enabled = false;
        }
        private void ConnectWebcamButton_Click(object sender, EventArgs e)
        {

            useWebcam = true;
            useNao = false;
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event            

            ButtonUp.Enabled = false;
            ButtonDown.Enabled = false;
            ButtonLeft.Enabled = false;
            ButtonRight.Enabled = false;
            ButtonSay.Enabled = false;
            sayTextBox.Enabled = false;

            Application.Idle += new EventHandler(FrameGrabber);
            ButtonWebcam.Enabled = false;
        }

        //button to add face
        private void TrainFaceButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;
                if (useNao && !useWebcam)
                {
                    //below is changed code to get image from currentframe.
                    gray = currentFrame.Convert<Gray, byte>().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                }
                else if (!useNao && useWebcam)
                {
                    //Get a gray frame from capture device
                    gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                }
                                
                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    //haar, 1.4, 4,
                    //HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    //new Size(nextFrame.Width / 8, nextFrame.Height / 8)
                    //)[0];
                face,
                1.2,//1.2,
                10,//10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {

            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");
            if (mc.isconnect()&& useNao && !useWebcam)
            {
                while (!mc.updatedUpper)
                { }
                img = Image.FromStream(new MemoryStream(mc.getByte(0)));
                currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
            else if(!useNao && useWebcam)
            {
                //Get the current frame form capture device
                currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
            else
            {
                img = Image.FromFile(String.Format("{0}/loading.jpg", curDir));
                currentFrame = new Image<Bgr, byte>(new Bitmap(img)).Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }             
            
            //End addition by PFS
                    //Convert it to Grayscale
                    gray = currentFrame.Convert<Gray, Byte>();

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                      face,
                      1.2,
                      10,
                      Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                      new Size(20, 20));
                    try
                    {
                        //Action for each element detected
                        foreach (MCvAvgComp f in facesDetected[0])
                        {
                            t = t + 1;
                            result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                            //draw the face detected in the 0th (gray) channel with blue color
                            currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);

                            if (trainingImages.ToArray().Length != 0)
                            {
                                //TermCriteria for face recognition with numbers of trained images like maxIteration
                                MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.00005);

                                //Eigen face recognizer
                                EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                                   trainingImages.ToArray(),
                                   labels.ToArray(),
                                   3000,
                                   ref termCrit);
                                string[] faceName = new string[faceIteration];
                                //Addition by PS
                                for (int i = 0; i < faceIteration; i++)
                                {                                    
                                    termCrit.epsilon = i * 0.00005;
                                    faceName[i]=recognizer.Recognize(result);
                                    Console.WriteLine(faceName[i]);
                                }
                                
                                name = faceName.GroupBy(v => v)
                                        .OrderByDescending(g => g.Count())
                                        .First()
                                        .Key;
                                //End of Addition by PS

                                //    name = recognizer.Recognize(result);
                                if (name != ""  && useNao && !calledName.Any(x=> x==name) )
                                {
                                    
                                    mc.sendVoice(name);                                    
                                    calledName.Add(name);
                                    calledNumber++;
                                }
                                else if (useNao )
                                {
                                    called = true;
                                    mc.sendVoice("");
                                }

                                //Draw the label for each face detected and recognized
                                currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                            }

                            NamePersons[t - 1] = name;
                            NamePersons.Add("");


                            //Set the number of faces detected on the scene
                            label3.Text = facesDetected[0].Length.ToString();

                            /*
                            //Set the region of interest on the faces
                        
                            gray.ROI = f.rect;
                            MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                               eye,
                               1.1,
                               10,
                               Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                               new Size(20, 20));
                            gray.ROI = Rectangle.Empty;

                            foreach (MCvAvgComp ey in eyesDetected[0])
                            {
                                Rectangle eyeRect = ey.rect;
                                eyeRect.Offset(f.rect.X, f.rect.Y);
                                currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                            }
                             */

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                        t = 0;

                        //Names concatenation of persons recognized
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                    }
                    //Show the faces procesed and recognized
                    imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
                    //Clear the list(vector) of names
                    NamePersons.Clear();

                }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }
        private void HeadLeftButtonClick(object sender, EventArgs e)
        {
            mc.sendCommand(ClientInfo.headLeft);
        }
        private void HeadRightButton_Click(object sender, EventArgs e)
        {
            mc.sendCommand(ClientInfo.headRight);
        }
        private void HeadDownButton_Click(object sender, EventArgs e)
        {
            mc.sendCommand(ClientInfo.headDown);
        }
        private void HeadUpButton_Click(object sender, EventArgs e)
        {
            mc.sendCommand(ClientInfo.headUp);
        }
        private void sayButton_Click(object sender, EventArgs e)
        {
            mc.sendVoice(sayTextBox.Text);
        }
        
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }       
         /// <summary>
        /// http://www.codeproject.com/Articles/12286/Simple-Client-server-Interactions-using-C
        /// </summary>
        class MessageClientFaceRecognition
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
                if (code == ClientInfo.ImageCodeUpper)
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

    
}