﻿using System;
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
    public class DistanceDetector : DisposableObject
    {
        private Features2DTracker<float>  _tracker2;
        private SURFDetector _detector2;
        private MemStorage _octagonStorage2;
        private Contour<Point> _octagon2;
        
        public Image<Gray, Byte> imageGray; //image that already smoothed
        Image<Gray, Byte> Gray_Frame; // image for processing
        public Image<Bgr, Byte> imagecolor;
        public List<double> areas = new List<double>();
        public List<PointF> centerPoints = new List<PointF>();
        public List<point3DF> Point3D = new List<point3DF>();//store 3D point
        PointF[] corners; //corners found from chessboard
        const int width = 8;//9 //width of chessboard no. squares in width - 1
        const int height = 5;//6 // heght of chess board no. squares in heigth - 1
        Size patternSize = new Size(width, height); //size of chess board to be detected
        double dpp=8.84; // distance constant for 310/pix * dpp = distance
        float fx = 1, fy = 1, cx = 1, cy = 1;

        public DistanceDetector(Image<Bgr, Byte> stopSignModel)
        {
            _detector2 = new SURFDetector(500, false);
            using (Image<Gray, Byte> redMask = GetColorPixelMask(stopSignModel))
            {
                try
                {
                    _tracker2 = new Features2DTracker<float>(_detector2.DetectFeatures(redMask, null));
                }
                catch { }
            }
            _octagonStorage2 = new MemStorage();
            _octagon2 = new Contour<Point>(_octagonStorage2);
            _octagon2.PushMulti(new Point[] { 
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 1),
            new Point(3, 2),
            new Point(2, 3),
            new Point(1, 3),
            new Point(0, 2),
            new Point(0, 1)},
               Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
        }

        /// <summary>
        /// Compute the red pixel mask for the given image. 
        /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
        /// </summary>
        /// <param name="image">The color image to find red mask from</param>
        /// <returns>The red pixel mask</returns>
        private static Image<Gray, Byte> GetColorPixelMask(Image<Bgr, byte> image)
        {
            using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
            {
                Image<Gray, Byte>[] channels = hsv.Split();
                

                try
                {
                    
                    //channels[0] is the mask for hue less than 20 or larger than 160
                    //red
                    CvInvoke.cvInRangeS(channels[0], new MCvScalar(15), new MCvScalar(165), channels[0]);
                    channels[0]._Not();
                    //
                    //CvInvoke.cvInRangeS(channels[0], new MCvScalar(45), new MCvScalar(75), channels[0]);

                    CvInvoke.cvShowImage("channel 0", channels[0]);
                    //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
                    channels[1]._ThresholdBinary(new Gray(10), new Gray(255.0));
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
        }

        private void FindStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours)
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
                    if (corners != null) //chess board found
                    {
                        Gray_Frame.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
                    }
                    // tentukan di kotak mana dia berada
                    Boolean contained=false;
                    int container=0;
                    for (int i = 1; i < corners.Length && !contained; )
                    {
                        if (isContain(c, corners[i], corners[i + 1], corners[i + width], corners[i + width + 1]))
                        {
                            contained=true;
                            container = i;
                        }
                        i++;
                    }
                    //tentukan posisi titik
                    point3DF pointOfObject = new point3DF();
                    if (container != 0)
                    {
                        pointOfObject.z = zOfPoint(corners[container], corners[container + 1], corners[container + width], corners[container + width + 1])/2;//divided by 2 because the resolution is half of calibrating image
                        pointOfObject.x = (c.X - cx*pointOfObject.z )/fx;
                        pointOfObject.y = (c.Y - cy * pointOfObject.z) / fx;
                        Point3D.Add(pointOfObject);
                        Console.Write("X: " + pointOfObject.x + " Y: " + pointOfObject.y + " Z: " + pointOfObject.z + "\n");
                        //Create the font
                        MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, 0.4);
                        imageGray.Draw("X: "+pointOfObject.x+ "Y: "+pointOfObject.y+"Z: "+pointOfObject.z, ref f, new Point((int)c.X, (int)c.Y+5), new Gray(140));
                        imageGray.Draw("Y: " + pointOfObject.y, ref f, new Point((int)c.X, (int)c.Y + 15), new Gray(140));
                        imageGray.Draw("Z: " + pointOfObject.z, ref f, new Point((int)c.X, (int)c.Y + 25), new Gray(140));
                    }
                    

                    //double ratio = CvInvoke.cvMatchShapes(_octagon2, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);

                    //if (ratio > 0.1) //not a good match of contour shape
                    //{
                    //    Contour<Point> child = contours.VNext;
                    //    if (child != null)
                    //        FindStopSign(img, stopSignList, boxList, child);
                    //    continue;
                    //}
                    
                    //Rectangle box = contours.BoundingRectangle;
                    
                    
                    //Image<Gray, Byte> candidate;
                    //using (Image<Bgr, Byte> tmp = img.Copy(box))
                    //    candidate = tmp.Convert<Gray, byte>();

                    ////set the value of pixels not in the contour region to zero
                    //using (Image<Gray, Byte> mask = new Image<Gray, byte>(box.Size))
                    //{
                    //    mask.Draw(contours, new Gray(255), new Gray(255), 0, -1, new Point(-box.X, -box.Y));

                    //    double mean = CvInvoke.cvAvg(candidate, mask).v0;
                    //    candidate._ThresholdBinary(new Gray(mean), new Gray(255.0));
                    //    candidate._Not();
                    //    mask._Not();
                    //    candidate.SetValue(0, mask);
                    //}

                    //ImageFeature<float>[] features = _detector2.DetectFeatures(candidate, null);

                    //Features2DTracker<float>.MatchedImageFeature[] matchedFeatures = _tracker2.MatchFeature(features, 2);

                    //int goodMatchCount = 0;
                    //foreach (Features2DTracker<float>.MatchedImageFeature ms in matchedFeatures)
                    //    if (ms.SimilarFeatures[0].Distance < 0.5) goodMatchCount++;

                    //if (goodMatchCount >= 10)
                    //{
                    //    //imageGray.Draw(contours, new Gray(150), 2);
                    //    imagecolor.Draw(contours, new Bgr(255,0,0), 2);
                    //    areas.Add(contours.Area);
                    //    boxList.Add(box);                                                

                    //    imageGray.Draw(contours.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE), new Gray(150), 2);
                    //    stopSignList.Add(candidate);
                    //}
                }
            }
        }

        public void DetectStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
        {
            centerPoints.Clear();
            imagecolor = img;
            areas.Clear();
            Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
            Image<Gray, Byte> smoothedRedMask = GetColorPixelMask(smoothImg);
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
                FindStopSign(img, stopSignList, boxList, contours);
            }
            
        }

        protected override void DisposeObject()
        {
            _tracker2.Dispose();
            _octagonStorage2.Dispose();
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



