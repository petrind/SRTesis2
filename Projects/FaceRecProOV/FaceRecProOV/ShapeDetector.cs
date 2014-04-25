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
    
    public class ShapeDetector : DisposableObject
    {
        
        public Image<Gray, Byte> imageGray;
        private Image<Gray, Byte> imageSelector;
        public Image<Bgr, Byte> imagecolor;
        public List<double> areas = new List<double>();
        private MCvBox2D[] minBoxesBlack = new MCvBox2D[4];
        private PointF[] pointBlack = new PointF[4];
        private MemStorage _rectStorage;
        private MemStorage joinContourStorage;
        private Contour<Point> rect;
        private Contour<Point> joinContour;
        double shapeRatio;

        public ShapeDetector()
        {
            _rectStorage = new MemStorage();
            joinContourStorage = new MemStorage();
            joinContour = new Contour<Point>(joinContourStorage);
            rect = new Contour<Point>(_rectStorage);
            rect.PushMulti(new Point[] { 
                //rect
                new Point(0, 0),
                new Point(20, 0),
                new Point(20, 20),
                new Point(0, 20)},
                
               Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
            imageSelector = new Image<Gray,byte>("C:\\monitor_photo_tengah_Repaired_Selected.jpg").Resize(320,240,Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            
        }

        /// <summary>
        /// Compute the red pixel mask for the given image. 
        /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
        /// </summary>
        /// <param name="image">The color image to find red mask from</param>
        /// <returns>The red pixel mask</returns>
        public Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image, int minHue, int maxHue, int minSat, int maxSat, int minValue, int maxValue)
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
        /// <param name="stopSignList"></param>
        /// <param name="boxList"></param>
        /// <param name="contours"></param>
        /// <param name="color">1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white</param>
        private void FindRect(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours, int color)
        {
            int i=0;
            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
            for (; contours != null; contours = contours.HNext)
            {
                //draw box from any contour
                
                //imageGray.Draw(new CircleF(centerBox(contours.BoundingRectangle), 3), new Gray(150), 2);
                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                
                if (contours.Area > 100 && contours.Total>0)
                {
                    
                    //handle normal rectangle
                    MCvBox2D minAreaRect = contours.GetMinAreaRect();

                    shapeRatio = CvInvoke.cvMatchShapes(rect, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);
                    double areaRatio = areaSize(minAreaRect.size) / contours.Area;
                    PointF c = centerBox(contours.BoundingRectangle);
                     if (shapeRatio < 0.1 && areaRatio<1.2)
                    {
                        Rectangle box = contours.BoundingRectangle;
                        //imageGray.Draw(box, new Gray(150), 1);                        

                        //imageGray.Draw(contours, new Gray(50), 2);                        
                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(200));
                        pointBlack[i] = c;
                        minBoxesBlack[i] = minAreaRect;
                        foreach (Point p in contours) joinContour.Push(p);
                        i++;
                        
                    }
                     //handle the rectangle with diferent orientation
                     else if (areaRatio < 1.3 && shapeRatio < 0.5)
                    {
                        Rectangle box = contours.BoundingRectangle;
                        //imageGray.Draw(box, new Gray(150), 1);
                        

                        //imageGray.Draw(contours, new Gray(50), 2);
                        
                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(200));
                        pointBlack[i] = c;
                        minBoxesBlack[i] = minAreaRect;
                        i++;
                        foreach (Point p in contours) joinContour.Push(p);
                        
                    } else
                         imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(100));

                     
                }
                
            }
            
        }

        public void DetectRect(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
        {
            imagecolor = img;
            joinContour.Clear();
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
                FindRect(img, stopSignList, boxList, contours, 5);
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

            
            //imageGray.Draw(joinContour.GetMinAreaRect(),new Gray(180),1);

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
                        tempbox= minBoxesBlack[i];

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
            LineSegment2DF[]lines  = new LineSegment2DF[9];
            

            lines[0] = new LineSegment2DF(pointBlack[0], pointBlack[3]);
            lines[1] = new LineSegment2DF(pointBlack[1], pointBlack[2]);
            lines[2] = translatationLineXNeg(lines[0], lines[1]);
            lines[3] = translatationLineXPos(lines[0], lines[1]);
            
            imageGray.Draw(lines[0], new Gray(100), 2);
            imageGray.Draw(lines[1], new Gray(100), 2);
            imageGray.Draw(lines[2], new Gray(100), 2);
            imageGray.Draw(lines[3], new Gray(100), 2);
            
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
                FindRect(img, stopSignList, boxList, contours,6);
            }
            CvInvoke.cvShowImage("Image White", smoothedWhiteMask);
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
        private double areaSize(SizeF size)
        {
            double area;
            area = size.Width * size.Height;
            return area;
        }
        private LineSegment2DF translatationLineXPos(LineSegment2DF line, LineSegment2DF translationLine)
        {
            
            float x1 = line.P1.X + (float) translationLine.Length / 2 * translationLine.Direction.X;
            float x2 = line.P2.X + (float)translationLine.Length / 2 * translationLine.Direction.X;
            PointF newpoint1 = new PointF(x1, line.P1.Y);
            PointF newpoint2 = new PointF(x2, line.P2.Y);
            LineSegment2DF lineTranslated = new LineSegment2DF(newpoint1, newpoint2);
            return lineTranslated;
        }
        private LineSegment2DF translatationLineXNeg(LineSegment2DF line, LineSegment2DF translationLine)
        {

            float x1 = line.P1.X - (float)translationLine.Length / 2 * translationLine.Direction.X;
            float x2 = line.P2.X - (float)translationLine.Length / 2 * translationLine.Direction.X;
            PointF newpoint1 = new PointF(x1, line.P1.Y);
            PointF newpoint2 = new PointF(x2, line.P2.Y);
            LineSegment2DF lineTranslated = new LineSegment2DF(newpoint1, newpoint2);
            return lineTranslated;
        }
    }
}
