﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Text;
//using Emgu.CV;
//using Emgu.CV.Features2D;
//using Emgu.CV.Structure;
//using Emgu.Util;

//namespace MultiFaceRec
//{
//    public struct point3DF 
//    {
//        public float x,y,z;
//    }
//    public struct colorPointF
//    {
//        public colorPointF(PointF p, int c)
//        {
//            point = p;
//            color = c;
//        }
//        public PointF point;
//        /// <summary>
//        /// 1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white
//        /// </summary>
//        public int color;
//    }
//    public class PointDetector : DisposableObject
//    {
//        /// <summary> image that already smoothed will be output</summary>
//        public Image<Gray, Byte> imageGray;
//        /// <summary> image for processing</summary>
//        Image<Gray, Byte> Gray_Frame;
//        private Image<Gray, Byte> imageSelector;
//        public int imgWidth = 320, imgHeight = 240;
//        public Image<Bgr, Byte> imagecolor;
//        public List<double> areas = new List<double>();
//        public List<PointF> centerPoints = new List<PointF>();
//        public point3DF[] Point3D;//store 3D point
//        PointF[] corners; //corners found from chessboard
//        PointF[] boardPoint = new PointF[64];
//        PointF[] boardPoint3D = new PointF[64];
//        public List<colorPointF> colorPoints = new List<colorPointF>();
//        const int width = 8;//9 //width of chessboard no. squares in width - 1
//        const int height = 5;//6 // heght of chess board no. squares in heigth - 1
//        Size patternSize = new Size(width, height); //size of chess board to be detected
//        double dppTop = 5.13967/1.86; // distance constant for 310/pix * dpp = distance
//        float fxTop = 674.122f, fyTop = 684.541f, cxTop = 147.790f, cyTop = 121.238f;
//        double dppBottom = 0; // distance constant for 310/pix * dpp = distance
//        float fxBottom = 495.76f, fyBottom = 495.10f, cxBottom = 174.650f, cyBottom = 115.501f;
//        public float z = 0f;
//        private MemStorage _rectStorage;
//        private Contour<Point> rect;
//        double ratio;
//        private MCvBox2D[] minBoxesBlack = new MCvBox2D[4];
//        private PointF[] pointBlack = new PointF[4];        
//        private MemStorage joinContourStorage;        
//        private Contour<Point> joinContour;
//        double shapeRatio;

//        public PointDetector()
//        {
//            joinContourStorage = new MemStorage();
//            joinContour = new Contour<Point>(joinContourStorage);
//            imageSelector = new Image<Gray, byte>("C:\\monitor_photo_tengah_Repaired_Selected.jpg").Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
//            _rectStorage = new MemStorage();
//            rect = new Contour<Point>(_rectStorage);
//            rect.PushMulti(new Point[] { 
//                //rect
//                new Point(0, 0),
//                new Point(20, 0),
//                new Point(20, 20),
//                new Point(0, 20)},
//               Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
//        }

//        /// <summary>
//        /// Compute the red pixel mask for the given image. 
//        /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
//        /// </summary>
//        /// <param name="image">The color image to find red mask from</param>
//        /// <returns>The red pixel mask</returns>
//        public Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image,int minHue,int maxHue, int minSat, int maxSat, int minValue, int maxValue)
//        {
//            using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
//            {
//                Image<Gray, Byte>[] channels = hsv.Split();
//                try
//                {

//                    CvInvoke.cvInRangeS(channels[0], new MCvScalar(minHue), new MCvScalar(maxHue), channels[0]);
//                    //CvInvoke.cvShowImage("channel 0", channels[0]);
//                    //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
//                    CvInvoke.cvInRangeS(channels[1], new MCvScalar(minSat), new MCvScalar(maxSat), channels[1]);

//                    CvInvoke.cvInRangeS(channels[2], new MCvScalar(minValue), new MCvScalar(maxValue), channels[2]);
                                        
//                    CvInvoke.cvAnd(channels[0], channels[1], channels[0], IntPtr.Zero);
//                    CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
//                    //CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
                    
//                }
//                finally
//                {  
//                    //CvInvoke.cvShowImage("channel 1", channels[1]);
//                    //CvInvoke.cvShowImage("channel 2", channels[2]);
//                    channels[1].Dispose();
//                    channels[2].Dispose();
//                    //channels[0].Dispose();
//                }
//                return channels[0];
//            }
//        }
        

//        private void FindPoints3D(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours)
//        {
//            for (; contours != null; contours = contours.HNext)
//            {
//                //draw box from any contour
                
//                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
//                if (contours.Area > 20)
//                {
//                    PointF c = centerBox(contours.BoundingRectangle);

//                    imageGray.Draw(new CircleF(c, 3), new Gray(150), 2);
                    
                   
//                    centerPoints.Add(c);
                    
//                    // detect the chessboard
//                    Gray_Frame = img.Convert<Gray, Byte>();//
//                    corners = CameraCalibration.FindChessboardCorners(Gray_Frame, patternSize, Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH);
//                    if (false) //chess board found
//                    {
//                        Gray_Frame.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));

//                        // tentukan di kotak mana dia berada
//                        Boolean contained = false;
//                        int container = 0;
//                        for (int i = 1; i < corners.Length && !contained; )
//                        {
//                            try
//                            {
//                                if (isContain(c, corners[i], corners[i + 1], corners[i + width], corners[i + width + 1]))
//                                {
//                                    contained = true;
//                                    container = i;
//                                }

//                            }
//                            catch { }
//                            i++;
//                        }
//                        //tentukan posisi titik
//                        point3DF pointOfObject = new point3DF();
//                        if (container != 0)
//                        {
//                            pointOfObject.z = zOfPointChess(corners[container], corners[container + 1], corners[container + width], corners[container + width + 1]);//divided by 2 because the resolution is half of calibrating image
//                            pointOfObject.x = (c.X * pointOfObject.z - cxTop * pointOfObject.z) / fxTop;
//                            pointOfObject.y = (c.Y * pointOfObject.z - cyTop * pointOfObject.z) / fyTop;
                            
//                            Console.Write("X: " + pointOfObject.x + " Y: " + pointOfObject.y + " Z: " + pointOfObject.z + "\n");
//                            //Create the font
//                            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
//                            imageGray.Draw("X: " + pointOfObject.x, ref f, new Point((int)c.X+5, (int)c.Y + 5), new Gray(140));
//                            imageGray.Draw("Y: " + pointOfObject.y, ref f, new Point((int)c.X+5, (int)c.Y + 15), new Gray(140));
//                            imageGray.Draw("Z: " + pointOfObject.z, ref f, new Point((int)c.X+5, (int)c.Y + 25), new Gray(140));
//                        }
//                    }
                    
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="img"></param>
//        /// <param name="stopSignList"></param>
//        /// <param name="boxList"></param>
//        /// <param name="contours"></param>
//        /// <param name="color">1=dark blue, 2 = pink, 3 = light green, 4 = purple, 5 = black, 6 = white</param>
//        private void FindRect(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours, int color)
//        {
//            int i = 0;
//            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
//            for (; contours != null; contours = contours.HNext)
//            {
//                //draw box from any contour

//                //imageGray.Draw(new CircleF(centerBox(contours.BoundingRectangle), 3), new Gray(150), 2);
//                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);

//                if (contours.Area > 100 && contours.Total > 0)
//                {

//                    //handle normal rectangle
//                    MCvBox2D minAreaRect = contours.GetMinAreaRect();

//                    shapeRatio = CvInvoke.cvMatchShapes(rect, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);
//                    double areaRatio = areaSize(minAreaRect.size) / contours.Area;
//                    PointF c = centerBox(contours.BoundingRectangle);
//                    if (shapeRatio < 0.1 && areaRatio < 1.2)
//                    {
//                        Rectangle box = contours.BoundingRectangle;
//                        //imageGray.Draw(box, new Gray(150), 1);                        

//                        //imageGray.Draw(contours, new Gray(50), 2);                        
//                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(200));
//                        pointBlack[i] = c;
//                        minBoxesBlack[i] = minAreaRect;
//                        foreach (Point p in contours) joinContour.Push(p);
//                        i++;

//                    }
//                    //handle the rectangle with diferent orientation
//                    else if (areaRatio < 1.3 && shapeRatio < 0.5)
//                    {
//                        Rectangle box = contours.BoundingRectangle;
//                        //imageGray.Draw(box, new Gray(150), 1);


//                        //imageGray.Draw(contours, new Gray(50), 2);

//                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(200));
//                        pointBlack[i] = c;
//                        minBoxesBlack[i] = minAreaRect;
//                        i++;
//                        foreach (Point p in contours) joinContour.Push(p);

//                    }
//                    else
//                        imageGray.Draw("" + i, ref f, new Point((int)c.X, (int)c.Y), new Gray(100));


//                }

//            }

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="img"></param>
//        /// <param name="contours"></param>
//        /// <param name="color">1=dark blue, 2 = pink, 3 = light green, 4 = purple</param>
//        private void FindColorPointsImageFeature(Image<Bgr, byte> img, Contour<Point> contours, int color)
//        {
//            for (; contours != null; contours = contours.HNext)
//            {
//                //draw box from any contour

//                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
//                if (contours.Area > 20)
//                {
//                    PointF c = centerBox(contours.BoundingRectangle);

//                    imageGray.Draw(new CircleF(c, 3), new Gray(150), 2);

//                    centerPoints.Add(c);

//                    PointF p = new PointF();
//                    p.X = (c.X * 50 - cxTop * 50) / fxTop;
//                    p.Y = (c.Y * 50- cyTop * 50) / fyTop;
//                    colorPoints.Add(new colorPointF(p, color));

//                    // detect the chessboard
//                    Gray_Frame = img.Convert<Gray, Byte>();//
//                }
//            }
//        }

//        public void DetectPointBoard(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
//        {
            
//            imagecolor = img;
//            areas.Clear();
//            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
//            Image<Gray, Byte> smoothedRedMask = GetColorPixelMask(smoothImg,110,130,50,255,150,255);
//            imageGray = smoothedRedMask;

//            //Use Dilate followed by Erode to eliminate small gaps in some countour.
//            smoothedRedMask._Dilate(1);
//            smoothedRedMask._Erode(1);

//            using (Image<Gray, Byte> canny = smoothedRedMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = canny.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                FindPoints3D(img, stopSignList, boxList, contours);
//            }
            
//        }

//        public void recognizeBoard(Image<Bgr, byte> img)
//        {
//            z = 0;
//            colorPoints.Clear();
//            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
//            Image<Gray, Byte> smoothedBlueMask = GetColorPixelMask(smoothImg, 110, 130, 120, 252, 50, 255);
//            smoothedBlueMask._Dilate(1);smoothedBlueMask._Erode(1);
//            imageGray = smoothedBlueMask;
//            //Image<Gray, Byte> smoothedPurpleMask = GetColorPixelMask(smoothImg, 130, 144, 0,255,50,255);
//            //smoothedPurpleMask._Dilate(1); smoothedPurpleMask._Erode(1);
//            //Image<Gray, Byte> smoothedGreenMask = GetColorPixelMask(smoothImg, 70, 86, 26,256,50,255);
//            //smoothedGreenMask._Dilate(1); smoothedGreenMask._Erode(1);
//            Image<Gray, Byte> smoothedPinkMask = GetColorPixelMask(img, 162, 170, 0, 255, 50, 255);
//            smoothedPinkMask._Dilate(1); smoothedPinkMask._Erode(1);
//            Image<Gray, Byte> imageCombine = new Image<Gray, byte>(imgWidth, imgHeight);

//            using (Image<Gray, Byte> cannyBlue = smoothedBlueMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = cannyBlue.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                FindColorPointsImageFeature(img, contours, 1);
//            }



//            //using (Image<Gray, Byte> cannyPurple = smoothedPurpleMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            //using (MemStorage stor = new MemStorage())
//            //{
//            //    Contour<Point> contours = cannyPurple.FindContours(
//            //       Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//            //       Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//            //       stor);
//            //}

//            //using (Image<Gray, Byte> cannyGreen = smoothedGreenMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            //using (MemStorage stor = new MemStorage())
//            //{
//            //    Contour<Point> contours = cannyGreen.FindContours(
//            //       Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//            //       Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//            //       stor);
//            //}

//            using (Image<Gray, Byte> cannyPink = smoothedPinkMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = cannyPink.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                FindColorPointsImageFeature(img, contours, 2);
//            }

//            try
//            {
//                z = zCalc(8, colorPoints[0].point.Y - colorPoints[1].point.Y);
//            }
//            catch { }
//        }

//        public void DetectRect(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
//        {
//            imagecolor = img;
//            joinContour.Clear();
//            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
//            Image<Gray, Byte> smoothedBlackMask = GetColorPixelMask(smoothImg, 0, 180, 0, 94, 0, 100);
//            imageGray = smoothedBlackMask;

//            //Use Dilate followed by Erode to eliminate small gaps in some countour.
//            smoothedBlackMask._Dilate(1);
//            smoothedBlackMask._Erode(1);

//            using (Image<Gray, Byte> canny = smoothedBlackMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = canny.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                FindRect(img, stopSignList, boxList, contours, 5);
//            }
//            CvInvoke.cvAnd(imageGray, imageSelector, imageGray, IntPtr.Zero);
//            using (Image<Gray, Byte> cannySelector = imageSelector.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = cannySelector.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                imageGray.Draw(contours, new Gray(255), 1);
//            }


//            //imageGray.Draw(joinContour.GetMinAreaRect(),new Gray(180),1);

//            CvInvoke.cvShowImage("Image Black", imageGray);
//            PointF temp = new PointF();
//            MCvBox2D tempbox = new MCvBox2D();
//            bool swapped = false;
//            //bubble sort for making following sorting
//            //   0
//            // 1   2
//            //   4
//            do
//            {
//                swapped = false;
//                for (int i = 0; i < 3; i++)
//                {
//                    if (pointBlack[i].Y > pointBlack[i + 1].Y)
//                    {
//                        temp = pointBlack[i];
//                        tempbox = minBoxesBlack[i];

//                        pointBlack[i] = pointBlack[i + 1];
//                        minBoxesBlack[i] = minBoxesBlack[i + 1];

//                        pointBlack[i + 1] = temp;
//                        minBoxesBlack[i + 1] = tempbox;
//                        swapped = true;
//                    }
//                }
//            } while (swapped);

//            if (pointBlack[1].X > pointBlack[2].X)
//            {
//                temp = pointBlack[1];
//                tempbox = minBoxesBlack[1];
//                pointBlack[1] = pointBlack[2];
//                minBoxesBlack[1] = minBoxesBlack[2];
//                pointBlack[2] = temp;
//                minBoxesBlack[2] = tempbox;
//            }
//            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
//            //for (int i=0; i < 4; i++)
//            //{
//            //    imageGray.Draw("    " + i, ref f, new Point((int)pointBlack[i].X, (int)pointBlack[i].Y), new Gray(200));
//            //    imageGray.Draw(minBoxesBlack[i], new Gray(100), 2);
//            //}
//            LineSegment2DF[] lines = new LineSegment2DF[9];


//            lines[0] = new LineSegment2DF(pointBlack[0], pointBlack[3]);
//            lines[1] = new LineSegment2DF(pointBlack[1], pointBlack[2]);
            

//            imageGray.Draw(lines[0], new Gray(100), 2);
//            imageGray.Draw(lines[1], new Gray(100), 2);
//            imageGray.Draw(lines[2], new Gray(100), 2);
//            imageGray.Draw(lines[3], new Gray(100), 2);

//            //areas.Clear();
//            Image<Gray, Byte> smoothedWhiteMask = GetColorPixelMask(smoothImg, 0, 180, 0, 94, 92, 255);
//            imageGray = smoothedWhiteMask;

//            //Use Dilate followed by Erode to eliminate small gaps in some countour.
//            smoothedWhiteMask._Dilate(1);
//            smoothedWhiteMask._Erode(1);
//            CvInvoke.cvAnd(smoothedWhiteMask, imageSelector, smoothedWhiteMask, IntPtr.Zero);

//            using (Image<Gray, Byte> canny = smoothedWhiteMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
//            using (MemStorage stor = new MemStorage())
//            {
//                Contour<Point> contours = canny.FindContours(
//                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
//                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
//                   stor);
//                FindRect(img, stopSignList, boxList, contours, 6);
//            }
//            CvInvoke.cvShowImage("Image White", smoothedWhiteMask);
//        }

//        protected override void DisposeObject()
//        {
           
//        }
//        private PointF centerBox(Rectangle rec)
//        {
//            PointF center = new PointF();
//            center.X = (rec.Right + rec.Left) / 2;
//            center.Y = (rec.Top + rec.Bottom) / 2;
//            return center;
//        }
//        private Boolean isContain(PointF p, PointF a,PointF b,PointF c, PointF d)
//        {
//            float abx, aby, bdx, bdy, dcx, dcy, cay, cax;
//            abx = b.X - a.X;
//            aby = b.Y - a.Y;
//            bdx = d.X - b.X;
//            bdy = d.Y - b.Y;
//            dcx = c.X - d.X;
//            dcy = c.Y - d.Y;
//            cax = a.X - c.X;
//            cay = a.Y - c.Y;

//                if ((p.Y - a.Y) * abx - (p.X - a.X) * aby <= 0) return false;
//                if ((p.Y - b.Y) * bdx - (p.X - b.X) * bdy <= 0) return false;
//                if ((p.Y - d.Y) * dcx - (p.X - d.X) * dcy <= 0) return false;
//                if ((p.Y - c.Y) * cax - (p.X - a.X) * cay <= 0) return false;
                
//            return true;
//            }

//        private float zOfPointChess(PointF a, PointF b, PointF c, PointF d)
//        {
//            double abx, aby, bdx, bdy, dcx, dcy, cay, cax;
//            abx = b.X - a.X;
//            aby = b.Y - a.Y;
//            bdx = d.X - b.X;
//            bdy = d.Y - b.Y;
//            dcx = c.X - d.X;
//            dcy = c.Y - d.Y;
//            cax = a.X - c.X;
//            cay = a.Y - c.Y;
//            Console.Write(abx+" "+aby+" "+bdx+" "+bdy+" "+dcx+" "+dcy+" "+cax+" "+cay+"\n");
//            float z = (float) (310*dppTop/Math.Sqrt( abx * abx + aby * aby)+
//                310 * dppTop / Math.Sqrt(bdx * bdx + bdy * bdy) +
//                310 * dppTop / Math.Sqrt(dcx * dcx + dcy * dcy) +
//                310 * dppTop / Math.Sqrt(cax * cax + cay * cay)) / 4;
//            return z;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="real">in cm</param>
//        /// <param name="img">in pixel</param>
//        /// <returns></returns>
//        private float zCalc(float real, float img)
//        {
//            float z=0;
//            z = (float)real * (float)dppTop/img;
//            return z;
//        }

//        private double areaSize(SizeF size)
//        {
//            double area;
//            area = size.Width * size.Height;
//            return area;
//        }
//    }
//}