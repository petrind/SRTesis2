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
    public class SignDetector : DisposableObject
    {
        private Features2DTracker<float>  _tracker2;
        private SURFDetector _detector2;
        private MemStorage _octagonStorage2;
        private Contour<Point> _octagon2;
        
        public Image<Gray, Byte> imageGray;
        public Image<Bgr, Byte> imagecolor;
        public List<double> areas = new List<double>();

        public SignDetector(Image<Bgr, Byte> stopSignModel)
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
                //hexagon
                new Point(1, 0),
                new Point(2, 0),
                new Point(3, 1),
                new Point(2, 2),
                new Point(1, 2),
                new Point(0, 1)},
                //octagon
            //new Point(1, 0),
            //new Point(2, 0),
            //new Point(3, 1),
            //new Point(3, 2),
            //new Point(2, 3),
            //new Point(1, 3),
            //new Point(0, 2),
            //new Point(0, 1)},
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
                    //CvInvoke.cvInRangeS(channels[0], new MCvScalar(110), new MCvScalar(130), channels[0]);

                    CvInvoke.cvShowImage("channel 0", channels[0]);
                    //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
                    channels[1]._ThresholdBinary(new Gray(50), new Gray(255.0));
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


                imageGray.Draw(new CircleF(centerBox(contours.BoundingRectangle), 3), new Gray(150), 2);
                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 20)
                {
                    double ratio = CvInvoke.cvMatchShapes(_octagon2, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);

                    if (ratio > 0.1) //not a good match of contour shape
                    {
                        Contour<Point> child = contours.VNext;
                        if (child != null)
                            FindStopSign(img, stopSignList, boxList, child);
                        continue;
                    }
                    
                    Rectangle box = contours.BoundingRectangle;
                    
                    
                    Image<Gray, Byte> candidate;
                    using (Image<Bgr, Byte> tmp = img.Copy(box))
                        candidate = tmp.Convert<Gray, byte>();

                    //set the value of pixels not in the contour region to zero
                    using (Image<Gray, Byte> mask = new Image<Gray, byte>(box.Size))
                    {
                        mask.Draw(contours, new Gray(255), new Gray(255), 0, -1, new Point(-box.X, -box.Y));

                        double mean = CvInvoke.cvAvg(candidate, mask).v0;
                        candidate._ThresholdBinary(new Gray(mean), new Gray(255.0));
                        candidate._Not();
                        mask._Not();
                        candidate.SetValue(0, mask);
                    }

                    ImageFeature<float>[] features = _detector2.DetectFeatures(candidate, null);

                    Features2DTracker<float>.MatchedImageFeature[] matchedFeatures = _tracker2.MatchFeature(features, 2);

                    int goodMatchCount = 0;
                    foreach (Features2DTracker<float>.MatchedImageFeature ms in matchedFeatures)
                        if (ms.SimilarFeatures[0].Distance < 0.5) goodMatchCount++;

                    if (goodMatchCount >= 10)
                    {
                        //imageGray.Draw(contours, new Gray(150), 2);
                        imagecolor.Draw(contours, new Bgr(255,0,0), 2);
                        areas.Add(contours.Area);
                        boxList.Add(box);                                                

                        imageGray.Draw(contours.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE), new Gray(150), 2);
                        stopSignList.Add(candidate);
                    }
                }
            }
        }

        public void DetectStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<Contour<Point>> contourSignFound)
        {
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

    }
}



