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
    public class PointDetector : DisposableObject
    {
        /// <summary> image that already smoothed will be output</summary>
        public Image<Gray, Byte> imageGray;
        /// <summary> image for processing</summary>
        Image<Gray, Byte> Gray_Frame; 
        public Image<Bgr, Byte> imagecolor;
        public List<double> areas = new List<double>();
        public List<PointF> centerPoints = new List<PointF>();
        public point3DF[] Point3D;//store 3D point
        PointF[] corners; //corners found from chessboard
        PointF[] boardPoint = new PointF[64];
        PointF[] boardPoint3D = new PointF[64];
        const int width = 8;//9 //width of chessboard no. squares in width - 1
        const int height = 5;//6 // heght of chess board no. squares in heigth - 1
        Size patternSize = new Size(width, height); //size of chess board to be detected
        double dpp = 5.13967; // distance constant for 310/pix * dpp = distance
        float fx = 674.122f, fy = 684.541f, cx = 147.790f, cy = 121.238f;

        public PointDetector()
        {
            
        }

        /// <summary>
        /// Compute the red pixel mask for the given image. 
        /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
        /// </summary>
        /// <param name="image">The color image to find red mask from</param>
        /// <returns>The red pixel mask</returns>
        private static Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image,int minHue,int maxHue, int minSat, int maxSat, int minValue, int maxValue)
        {
            using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
            {
                Image<Gray, Byte>[] channels = hsv.Split();
                try
                {

                    CvInvoke.cvInRangeS(channels[0], new MCvScalar(minHue), new MCvScalar(maxHue), channels[0]);
                    CvInvoke.cvShowImage("channel 0", channels[0]);
                    //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
                    CvInvoke.cvInRangeS(channels[1], new MCvScalar(minSat), new MCvScalar(maxSat), channels[1]);

                    CvInvoke.cvInRangeS(channels[2], new MCvScalar(minValue), new MCvScalar(maxValue), channels[2]);
                                        
                    CvInvoke.cvAnd(channels[0], channels[1], channels[0], IntPtr.Zero);
                    CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
                    //CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);
                    
                }
                finally
                {  
                    CvInvoke.cvShowImage("channel 1", channels[1]);
                    CvInvoke.cvShowImage("channel 2", channels[2]);
                    channels[1].Dispose();
                    channels[2].Dispose();
                    //channels[0].Dispose();
                }
                return channels[0];
            }
        }
        /// <summary>
        /// use for filter
        /// </summary>
        /// <param name="image"></param>
        /// <param name="minHue"></param>
        /// <param name="maxHue"></param>
        /// <param name="not"></param>
        /// <returns></returns>
        private static Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image, int maxHue, int minHue, int minSat, bool not=true)
        {
            if (not)
                using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
                {
                    Image<Gray, Byte>[] channels = hsv.Split();
                    try
                    {

                        //channels[0] is the mask for hue less than 20 or larger than 160
                        //red
                        CvInvoke.cvInRangeS(channels[0], new MCvScalar(maxHue), new MCvScalar(minHue), channels[0]);
                        channels[0]._Not();
                        //
                        //CvInvoke.cvInRangeS(channels[0], new MCvScalar(minHue), new MCvScalar(maxHue), channels[0]);

                        CvInvoke.cvShowImage("channel 0", channels[0]);
                        //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
                        channels[1]._ThresholdBinary(new Gray(minSat), new Gray(255.0));
                        //channels[2]._ThresholdBinary(new Gray(90), new Gray(255.0));

                        CvInvoke.cvAnd(channels[0], channels[1], channels[0], IntPtr.Zero);
                        //CvInvoke.cvAnd(channels[0], channels[2], channels[0], IntPtr.Zero);

                    }
                    finally
                    {

                        CvInvoke.cvShowImage("channel 1", channels[1]);
                        CvInvoke.cvShowImage("channel 2", channels[2]);
                        channels[1].Dispose();
                        channels[2].Dispose();
                        //channels[0].Dispose();
                    }
                    return channels[0];
                }
            else
                return null;
        }



        private void FindPoints3D(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours)
        {
            for (; contours != null; contours = contours.HNext)
            {
                //draw box from any contour
                
                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 20)
                {
                    PointF c = centerBox(contours.BoundingRectangle);

                    imageGray.Draw(new CircleF(c, 3), new Gray(150), 2);
                    
                   
                    centerPoints.Add(c);
                    // detect the chessboard
                    Gray_Frame = img.Convert<Gray, Byte>();//
                    corners = CameraCalibration.FindChessboardCorners(Gray_Frame, patternSize, Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH);
                    if (false) //chess board found
                    {
                        Gray_Frame.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));

                        // tentukan di kotak mana dia berada
                        Boolean contained = false;
                        int container = 0;
                        for (int i = 1; i < corners.Length && !contained; )
                        {
                            try
                            {
                                if (isContain(c, corners[i], corners[i + 1], corners[i + width], corners[i + width + 1]))
                                {
                                    contained = true;
                                    container = i;
                                }

                            }
                            catch { }
                            i++;
                        }
                        //tentukan posisi titik
                        point3DF pointOfObject = new point3DF();
                        if (container != 0)
                        {
                            pointOfObject.z = zOfPoint(corners[container], corners[container + 1], corners[container + width], corners[container + width + 1]);//divided by 2 because the resolution is half of calibrating image
                            pointOfObject.x = (c.X * pointOfObject.z - cx * pointOfObject.z) / fx;
                            pointOfObject.y = (c.Y * pointOfObject.z - cy * pointOfObject.z) / fy;
                            
                            Console.Write("X: " + pointOfObject.x + " Y: " + pointOfObject.y + " Z: " + pointOfObject.z + "\n");
                            //Create the font
                            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.8, 0.8);
                            imageGray.Draw("X: " + pointOfObject.x, ref f, new Point((int)c.X+5, (int)c.Y + 5), new Gray(140));
                            imageGray.Draw("Y: " + pointOfObject.y, ref f, new Point((int)c.X+5, (int)c.Y + 15), new Gray(140));
                            imageGray.Draw("Z: " + pointOfObject.z, ref f, new Point((int)c.X+5, (int)c.Y + 25), new Gray(140));
                        }
                    }
                    
                }
            }
        }
        private void FindPointsImageFeature(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours)
        {
            for (; contours != null; contours = contours.HNext)
            {
                //draw box from any contour

                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 20)
                {
                    PointF c = centerBox(contours.BoundingRectangle);

                    imageGray.Draw(new CircleF(c, 3), new Gray(150), 2);

                    centerPoints.Add(c);
                    // detect the chessboard
                    Gray_Frame = img.Convert<Gray, Byte>();//
                    
                }
            }
        }

        public void DetectPointBoard(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
        {
            
            imagecolor = img;
            areas.Clear();
            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
            Image<Gray, Byte> smoothedRedMask = GetColorPixelMask(smoothImg,110,130,50,255,150,255);
            imageGray = smoothedRedMask;

            //Use Dilate followed by Erode to eliminate small gaps in some countour.
            smoothedRedMask._Dilate(1);
            smoothedRedMask._Erode(1);

            using (Image<Gray, Byte> canny = smoothedRedMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = canny.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindPoints3D(img, stopSignList, boxList, contours);
            }
            
        }
        public void recognizeBoard(Image<Bgr, byte> img)
        {
            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
            Image<Gray, Byte> smoothedBlueMask = GetColorPixelMask(smoothImg, 145, 155, 50);
            smoothedBlueMask._Dilate(1);smoothedBlueMask._Erode(1);
            Image<Gray, Byte> smoothedPurpleMask = GetColorPixelMask(smoothImg, 140, 160, 50);
            smoothedPurpleMask._Dilate(1); smoothedPurpleMask._Erode(1);
            Image<Gray, Byte> smoothedGreenMask = GetColorPixelMask(smoothImg, 140, 160, 50);
            smoothedGreenMask._Dilate(1); smoothedGreenMask._Erode(1);
            Image<Gray, Byte> smoothedPinkMask = GetColorPixelMask(smoothImg, 140, 160, 50);
            smoothedPinkMask._Dilate(1);smoothedPinkMask._Erode(1);

            using (Image<Gray, Byte> cannyBlue = smoothedBlueMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = cannyBlue.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
            }

            using (Image<Gray, Byte> cannyPurple = smoothedPurpleMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = cannyPurple.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
            }

            using (Image<Gray, Byte> cannyGreen = smoothedGreenMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = cannyGreen.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
            }

            using (Image<Gray, Byte> cannyPink = smoothedPinkMask.Canny(new Gray(100), new Gray(50)))//Canny(100,50))
            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = cannyPink.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
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
        private Boolean isContain(PointF p, PointF a,PointF b,PointF c, PointF d)
        {
            float abx, aby, bdx, bdy, dcx, dcy, cay, cax;
            abx = b.X - a.X;
            aby = b.Y - a.Y;
            bdx = d.X - b.X;
            bdy = d.Y - b.Y;
            dcx = c.X - d.X;
            dcy = c.Y - d.Y;
            cax = a.X - c.X;
            cay = a.Y - c.Y;

                if ((p.Y - a.Y) * abx - (p.X - a.X) * aby <= 0) return false;
                if ((p.Y - b.Y) * bdx - (p.X - b.X) * bdy <= 0) return false;
                if ((p.Y - d.Y) * dcx - (p.X - d.X) * dcy <= 0) return false;
                if ((p.Y - c.Y) * cax - (p.X - a.X) * cay <= 0) return false;
                
            return true;
            }

        private float zOfPoint(PointF a, PointF b, PointF c, PointF d)
        {
            double abx, aby, bdx, bdy, dcx, dcy, cay, cax;
            abx = b.X - a.X;
            aby = b.Y - a.Y;
            bdx = d.X - b.X;
            bdy = d.Y - b.Y;
            dcx = c.X - d.X;
            dcy = c.Y - d.Y;
            cax = a.X - c.X;
            cay = a.Y - c.Y;
            Console.Write(abx+" "+aby+" "+bdx+" "+bdy+" "+dcx+" "+dcy+" "+cax+" "+cay+"\n");
            float z = (float) (310*dpp/Math.Sqrt( abx * abx + aby * aby)+
                310 * dpp / Math.Sqrt(bdx * bdx + bdy * bdy) +
                310 * dpp / Math.Sqrt(dcx * dcx + dcy * dcy) +
                310 * dpp / Math.Sqrt(cax * cax + cay * cay)) / 4;
            return z;
        }


    }
}