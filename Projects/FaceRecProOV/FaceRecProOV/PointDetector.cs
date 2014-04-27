using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;

namespace MultiFaceRec
{
    public struct point3DF 
    {
        public float x,y,z;
    }
    public struct colorObject
    {
        public colorObject(float x, float y,float z, int c, Rectangle rect)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            color = c;
            this.rect = rect;            
        }
        public float x, y, z;
        /// <summary>
        /// 1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white
        /// </summary>
        public int color;
        public Rectangle rect;
    }
    public class PointDetector : DisposableObject
    {
        /// <summary> image that already smoothed will be output</summary>
        public Image<Gray, Byte> imageGray;
        /// <summary> image for processing</summary>
        Image<Gray, Byte> Gray_Frame;
        private Image<Gray, Byte> imageSelector;
        public int imgWidth = 320, imgHeight = 240;
        public Image<Bgr, Byte> imageColor;
        public List<double> areas = new List<double>();
        public List<PointF> centerPoints = new List<PointF>();
        public point3DF[] Point3D;//store 3D point
        PointF[] corners; //corners found from chessboard
        PointF[] boardPoint = new PointF[64];
        PointF[] boardPoint3D = new PointF[64];
        public List<colorObject> colorObjects = new List<colorObject>();
        const int width = 8;//9 //width of chessboard no. squares in width - 1
        const int height = 5;//6 // heght of chess board no. squares in heigth - 1
        Size patternSize = new Size(width, height); //size of chess board to be detected
        double dppTop = 5.13967/1.86; // distance constant for 310/pix * dpp = distance
        float fxTop = 674.122f, fyTop = 684.541f, cxTop = 147.790f, cyTop = 121.238f;
        double dppBottom = 0; // distance constant for 310/pix * dpp = distance
        float fxBottom = 495.76f, fyBottom = 495.10f, cxBottom = 174.650f, cyBottom = 115.501f;
        public float z = 0f;
        private MemStorage _rectStorage;
        private Contour<Point> rect;
        double ratio;
        private MCvBox2D[] minBoxesBlack = new MCvBox2D[4];
        private PointF[] pointBlack = new PointF[4];        
        private MemStorage joinContourStorage;        
        private Contour<Point> joinContour;
        double shapeRatio;

        public PointDetector()
        {
            joinContourStorage = new MemStorage();
            joinContour = new Contour<Point>(joinContourStorage);
            imageSelector = new Image<Gray, byte>("C:\\monitor_photo_tengah_Repaired_Selected.jpg").Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            _rectStorage = new MemStorage();
            rect = new Contour<Point>(_rectStorage);
            rect.PushMulti(new Point[] { 
                //rect
                new Point(0, 0),
                new Point(20, 0),
                new Point(20, 20),
                new Point(0, 20)},
               Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
        }

        /// <summary>
        /// Compute the red pixel mask for the given image. 
        /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
        /// </summary>
        /// <param name="image">The color image to find red mask from</param>
        /// <returns>The red pixel mask</returns>
        public Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image,int minHue,int maxHue, int minSat, int maxSat, int minValue, int maxValue)
        {
            using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
            {
                Image<Gray, Byte>[] channels = hsv.Split();
                try
                {

                    CvInvoke.cvInRangeS(channels[0], new MCvScalar(minHue), new MCvScalar(maxHue), channels[0]);
                    //CvInvoke.cvShowImage("channel 0", channels[0]);
                    //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
                    CvInvoke.cvInRangeS(channels[1], new MCvScalar(minSat), new MCvScalar(maxSat), channels[1]);

                    CvInvoke.cvInRangeS(channels[2], new MCvScalar(minValue), new MCvScalar(maxValue), channels[2]);
                                        
                    CvInvoke.cvAnd(channels[0], channels[1], channels[0], IntPtr.Zero);
                    CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
                    //CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
                    
                }
                finally
                {  
                    //CvInvoke.cvShowImage("channel 1", channels[1]);
                    //CvInvoke.cvShowImage("channel 2", channels[2]);
                    channels[1].Dispose();
                    channels[2].Dispose();
                    //channels[0].Dispose();
                }
                return channels[0];
            }
        }
        

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="contours"></param>
        /// <param name="color">1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white</param>
        private void FindRect(Image<Bgr, byte> img, Contour<Point> contours, int color)
        {
            int i = 0;
            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
            for (; contours != null; contours = contours.HNext)
            {
                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);

                if (contours.Area > 100 && contours.Total > 0)
                {
                    MCvBox2D minAreaRect = contours.GetMinAreaRect();

                    shapeRatio = CvInvoke.cvMatchShapes(rect, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);
                    double areaRatio = areaSize(minAreaRect.size) / contours.Area;
                    PointF c = centerBox(contours.BoundingRectangle);
                    if (shapeRatio < 0.1 && areaRatio < 1.2)
                    {
                        Rectangle box = contours.BoundingRectangle;
                        pointBlack[i] = c;
                        minBoxesBlack[i] = minAreaRect;
                        i++;

                    }
                    //handle the rectangle with diferent orientation
                    else if (areaRatio < 1.3 && shapeRatio < 0.5)
                    {
                        Rectangle box = contours.BoundingRectangle;
                        pointBlack[i] = c;
                        minBoxesBlack[i] = minAreaRect;
                        i++;
                    }
                    else
                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(100));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="contours"></param>
        /// <param name="color">1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white</param>
        private void FindColoredObject(Image<Bgr, byte> img, Contour<Point> contours, int color)
        {
            for (; contours != null; contours = contours.HNext)
            {
                //draw box from any contour

                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 20)
                {
                    PointF c = centerBox(contours.BoundingRectangle);

                    imageGray.Draw(new CircleF(c, 3), new Gray(150), 2);
                    imageColor.Draw(new CircleF(c, 3), new Bgr(255, 255, 255), 1);
                    centerPoints.Add(c);

                    PointF pReal = new PointF();
                    float z = zCalc(2.4f, contours.BoundingRectangle.Width, 1);
                    pReal.X = (c.X * z - cxTop * z) / fxTop;
                    pReal.Y = (c.Y * z - cyTop * z) / fyTop;
                    colorObjects.Add(new colorObject(pReal.X,pReal.Y,z, color,contours.BoundingRectangle));

                    // detect the chessboard
                    Gray_Frame = img.Convert<Gray, Byte>();//
                }
            }
        }

        public void recognizeBoard(Image<Bgr, byte> img)
        {
            imageColor = img;
            //joinContour.Clear();
            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
            Image<Gray, Byte> smoothedBlackMask = GetColorPixelMask(smoothImg, 0, 180, 0, 94, 0, 100);
            imageGray = smoothedBlackMask;

            //Use Dilate followed by Erode to eliminate small gaps in some countour.
            smoothedBlackMask._Dilate(1);
            smoothedBlackMask._Erode(1);

            using (Image<Gray, Byte> canny = smoothedBlackMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = canny.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindRect(img, contours, 5);
            }
            CvInvoke.cvAnd(imageGray, imageSelector, imageGray, IntPtr.Zero);
            using (Image<Gray, Byte> cannySelector = imageSelector.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = cannySelector.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                imageGray.Draw(contours, new Gray(255), 1);
            }

            CvInvoke.cvShowImage("Image Black", imageGray);
            PointF temp = new PointF();
            MCvBox2D tempbox = new MCvBox2D();
            bool swapped = false;
            //bubble sort for making following sorting
            //   0
            // 1   2
            //   4
            do
            {
                swapped = false;
                for (int i = 0; i < 3; i++)
                {
                    if (pointBlack[i].Y > pointBlack[i + 1].Y)
                    {
                        temp = pointBlack[i];
                        tempbox = minBoxesBlack[i];

                        pointBlack[i] = pointBlack[i + 1];
                        minBoxesBlack[i] = minBoxesBlack[i + 1];

                        pointBlack[i + 1] = temp;
                        minBoxesBlack[i + 1] = tempbox;
                        swapped = true;
                    }
                }
            } while (swapped);

            if (pointBlack[1].X > pointBlack[2].X)
            {
                temp = pointBlack[1];
                tempbox = minBoxesBlack[1];
                pointBlack[1] = pointBlack[2];
                minBoxesBlack[1] = minBoxesBlack[2];
                pointBlack[2] = temp;
                minBoxesBlack[2] = tempbox;
            }
            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
            //for (int i=0; i < 4; i++)
            //{
            //    imageGray.Draw("    " + i, ref f, new Point((int)pointBlack[i].X, (int)pointBlack[i].Y), new Gray(200));
            //    imageGray.Draw(minBoxesBlack[i], new Gray(100), 2);
            //}
            LineSegment2DF[] lines = new LineSegment2DF[9];


            lines[0] = new LineSegment2DF(pointBlack[0], pointBlack[3]);
            lines[1] = new LineSegment2DF(pointBlack[1], pointBlack[2]);
            
            imageGray.Draw(lines[0], new Gray(100), 2);
            imageGray.Draw(lines[1], new Gray(100), 2);
            //imageGray.Draw(lines[2], new Gray(100), 2);
            //imageGray.Draw(lines[3], new Gray(100), 2);

            //areas.Clear();
            Image<Gray, Byte> smoothedWhiteMask = GetColorPixelMask(smoothImg, 0, 180, 0, 94, 92, 255);
            imageGray = smoothedWhiteMask;

            //Use Dilate followed by Erode to eliminate small gaps in some countour.
            smoothedWhiteMask._Dilate(1);
            smoothedWhiteMask._Erode(1);
            CvInvoke.cvAnd(smoothedWhiteMask, imageSelector, smoothedWhiteMask, IntPtr.Zero);

            using (Image<Gray, Byte> canny = smoothedWhiteMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = canny.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindRect(img, contours, 6);
            }
            CvInvoke.cvShowImage("Image White", smoothedWhiteMask);
        }

        public void RecognizeColorObject(Image<Bgr, byte> img)
        {
            imageColor = img;
            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);

            colorObjects.Clear();
            Image<Gray, Byte> smoothedPurpleMask = GetColorPixelMask(smoothImg, 140, 156, 155, 225, 100, 255);

            //Use Dilate followed by Erode to eliminate small gaps in some countour.
            smoothedPurpleMask._Dilate(1);
            smoothedPurpleMask._Erode(1);
            //CvInvoke.cvAnd(smoothedPurpleMask, imageSelector, smoothedPurpleMask, IntPtr.Zero);
            imageGray = smoothedPurpleMask;

            using (Image<Gray, Byte> canny = smoothedPurpleMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = canny.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindColoredObject(img, contours, 1);
            }
            colorObjects.ToString();

            
            Image<Gray, Byte> smoothedGreenMask = GetColorPixelMask(smoothImg, 44, 71, 80, 190, 100, 255);
            
            //Use Dilate followed by Erode to eliminate small gaps in some countour.
            smoothedGreenMask._Dilate(1);
            smoothedGreenMask._Erode(1);
            //CvInvoke.cvAnd(smoothedBlueMask, imageSelector, smoothedBlueMask, IntPtr.Zero);

            using (Image<Gray, Byte> canny = smoothedGreenMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = canny.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindColoredObject(img, contours, 0);
            }

            
        }

        protected override void DisposeObject()
        {
           
        }
        private PointF centerBox(Rectangle rec)
        {
            PointF center = new PointF();
            center.X = (rec.Right + rec.Left) / 2;
            center.Y = (rec.Top + rec.Bottom) / 2;
            return center;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="real">length of real, in cm</param>
        /// <param name="img">length in pixel</param>
        /// <param name="dDefault"></param>
        /// <returns></returns>
        private float zCalc(float real, float img, float dDefault)
        {
            float z=0;
            z = (float)real * (float)dDefault / img;
            return z;
        }

        private double areaSize(SizeF size)
        {
            double area;
            area = size.Width * size.Height;
            return area;
        }
        public colorObject ImageToNaoCameraFrame(colorObject p)
        {
            colorObject n = new colorObject(p.z, -p.x, p.y, p.color, p.rect);

            return n;
        }
    }
}