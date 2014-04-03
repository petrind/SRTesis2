//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Text;
//using Emgu.CV;
//using Emgu.CV.Features2D;
//using Emgu.CV.Structure;
//using Emgu.Util;
//using Emgu.CV.GPU;


////#include <stdio.h>
////#include <iostream>
////#include "opencv2/core/core.hpp"
////#include "opencv2/features2d/features2d.hpp"
////#include "opencv2/highgui/highgui.hpp"
////#include "opencv2/calib3d/calib3d.hpp"
////#include "opencv2/nonfree/nonfree.hpp"

//namespace MultiFaceRec
//{
//    /// <summary>
//    /// Translation from C++ http://docs.opencv.org/doc/tutorials/features2d/feature_homography/feature_homography.html#feature-homography
//    /// </summary>
//    class Features2DHomography
//    {
//        //Mat img_object = imread( argv[1], CV_LOAD_IMAGE_GRAYSCALE );
//        //Mat img_scene = imread( argv[2], CV_LOAD_IMAGE_GRAYSCALE );
//        public Image<Gray, Byte> imageObject;
//        public Image<Gray, Byte> imageScene;
//        SURFDetector detector;
//        MKeyPoint[] keypoints_object, keypoints_Scene;
//        IDescriptorExtractor extractor ;
//        ImageFeature[] descriptorObject, descriptorScene;
        
//        public Features2DHomography()
//        {

//        }
//        void KeypointDetector(){
//            //-- Step 1: Detect the keypoints using SURF Detector
//              //int minHessian = 400;

//              //SurfFeatureDetector detector( minHessian );

//              //std::vector<KeyPoint> keypoints_object, keypoints_scene;

//              //detector.detect( img_object, keypoints_object );
//              //detector.detect( img_scene, keypoints_scene );
//            int minHessian = 400;
//            detector =new SURFDetector(500, false);
//            keypoints_object=detector.DetectKeyPoints(imageObject);
//            keypoints_Scene=detector.DetectKeyPoints(imageScene);
//        }
//        void CalculateDescriptor(){
//              //-- Step 2: Calculate descriptors (feature vectors)
//              //SurfDescriptorExtractor extractor;

//              //Mat descriptors_object, descriptors_scene;

//              //extractor.compute( img_object, keypoints_object, descriptors_object );
//              //extractor.compute( img_scene, keypoints_scene, descriptors_scene );
//            descriptorObject= detector.ComputeDescriptors(imageObject,keypoints_object);
//            descriptorScene= detector.ComputeDescriptors(imageScene,keypoints_Scene);

//        }
//        void descriptorMatcher(){
//            //-- Step 3: Matching descriptor vectors using FLANN matcher
//            GpuBruteForceMatcher<float> matcher;

//          FlannBasedMatcher matcher;
//          std::vector< DMatch > matches;
//          matcher.match( descriptors_object, descriptors_scene, matches );

          
//        }
//        void distanceCalculator(){
//            double max_dist = 0; double min_dist = 100;
//            //-- Quick calculation of max and min distances between keypoints
//            foreach (ImageFeature IF in descriptorObject)
//            {
//                double dist = matches[i].distance;
//                if (dist < min_dist) min_dist = dist;
//                if (dist > max_dist) max_dist = dist;
//            }

//            //for (int i = 0; i < descriptorObject.rows; i++)
//            //  { double dist = matches[i].distance;
//            //    if( dist < min_dist ) min_dist = dist;
//            //    if( dist > max_dist ) max_dist = dist;
//            //  }

//              //printf("-- Max dist : %f \n", max_dist );
//              //printf("-- Min dist : %f \n", min_dist );
//        }
//        void drawMatch(){
//            //-- Draw only "good" matches (i.e. whose distance is less than 3*min_dist )

//              std::vector< DMatch > good_matches;
//                foreach (ImageFeature IF in descriptorObject){
//                    if( matches[i].distance < 3*min_dist )
//                    { good_matches.push_back( matches[i]); }
//                }
//              //for( int i = 0; i < descriptors_object.rows; i++ )
//              //{ if( matches[i].distance < 3*min_dist )
//              //   { good_matches.push_back( matches[i]); }
//              //}

//              Mat img_matches;
//              drawMatches( img_object, keypoints_object, img_scene, keypoints_scene,
//                           good_matches, img_matches, Scalar::all(-1), Scalar::all(-1),
//                           vector<char>(), DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS );
//        }
//        void localizeObject(){
//            //-- Localize the object
//            List<PointF> objpoint,scenepoint;
//              //std::vector<Point2f> obj;
//              //std::vector<Point2f> scene;

//              for( int i = 0; i < good_matches.size(); i++ )
//              {
//                //-- Get the keypoints from the good matches
//                  objpoint.push_back(keypoints_object[good_matches[i].queryIdx].pt);
//                  scenepoint.push_back(keypoints_Scene[good_matches[i].trainIdx].pt);
//              }

//              Mat H = findHomography( obj, scene, CV_RANSAC );
//        }
//        void getCorner(){
//            //-- Get the corners from the image_1 ( the object to be "detected" )
//              std::vector<Point2f> obj_corners(4);
//              obj_corners[0] = cvPoint(0,0); obj_corners[1] = cvPoint( img_object.cols, 0 );
//              obj_corners[2] = cvPoint( img_object.cols, img_object.rows ); obj_corners[3] = cvPoint( 0, img_object.rows );
//              std::vector<Point2f> scene_corners(4);

//              perspectiveTransform( obj_corners, scene_corners, H);
//        }
//        void drawLine(){
//            //-- Draw lines between the corners (the mapped object in the scene - image_2 )
//              line( img_matches, scene_corners[0] + Point2f( img_object.cols, 0), scene_corners[1] + Point2f( img_object.cols, 0), Scalar(0, 255, 0), 4 );
//              line( img_matches, scene_corners[1] + Point2f( img_object.cols, 0), scene_corners[2] + Point2f( img_object.cols, 0), Scalar( 0, 255, 0), 4 );
//              line( img_matches, scene_corners[2] + Point2f( img_object.cols, 0), scene_corners[3] + Point2f( img_object.cols, 0), Scalar( 0, 255, 0), 4 );
//              line( img_matches, scene_corners[3] + Point2f( img_object.cols, 0), scene_corners[0] + Point2f( img_object.cols, 0), Scalar( 0, 255, 0), 4 );

//              //-- Show detected matches
//              imshow( "Good Matches & Object detection", img_matches );
//        }

//    }
//}

