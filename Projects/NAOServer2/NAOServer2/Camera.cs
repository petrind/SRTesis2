/** 
 * This software was developed by Austin Hughes 
 * Last Modified: 2013‐06‐11 
 */ 
 
using System; 
using System.Collections; 
using System.Collections.Generic; 
using System.Windows; 
using Aldebaran.Proxies;
using System.Threading;
 
namespace NAO_Camera_WPF 
{ 
    /// <summary> 
    /// Data structure to hold image format information 
    /// </summary> 
    public struct NaoCamImageFormat 
    { 
        public string name; 
        public int id; 
        public int width; 
        public int height; 
    } 
    
 
    public class Camera 
    { 
        public VideoDeviceProxy naoCameraUpper = null;
        
        
        string subsNameUpper = "NAO Camera Upper";
        string subsNameLower= "NAO Camera Lower";
        public List<NaoCamImageFormat> NaoCamImageFormats = new 
List<NaoCamImageFormat>();
        public bool cameraReady;
 
        // class constructor 
        public Camera() 
        { 
            // set up image formats 
            NaoCamImageFormat format120 = new NaoCamImageFormat(); 
            NaoCamImageFormat format240 = new NaoCamImageFormat(); 
            NaoCamImageFormat format480 = new NaoCamImageFormat(); 
            NaoCamImageFormat format960 = new NaoCamImageFormat(); 
 
            format120.name = "160 * 120"; 
            format120.id = 0; 
            format120.width = 160; 
            format120.height = 120; 
 
            format240.name = "320 * 240"; 
            format240.id = 1; 
            format240.width = 320; 
            format240.height = 240; 
 
            format480.name = "640 * 480"; 
            format480.id = 2; 
            format480.width = 640; 
            format480.height = 480; 
 
            format960.name = "1280 * 960"; 
            format960.id = 3; 
            format960.width = 1280; 
            format960.height = 960; 
 
            // add them to the formats list 
            NaoCamImageFormats.Add(format120); 
            NaoCamImageFormats.Add(format240); 
            NaoCamImageFormats.Add(format480); 
            NaoCamImageFormats.Add(format960); 
        } 
 
        /// <summary> 
        /// Connects to the camera on the NAO robot 
        /// </summary> 
        /// <param name="ip"> the ip address of the robot </param> 
        /// <param name="format"> the video format desired </param> 
        /// <param name="ColorSpace"> the video color space </param> 
        /// <param name="FPS"> the FPS of the video </param> 
        public void connect(string ip, NaoCamImageFormat format, int ColorSpace, int FPS) 
        { 
            try 
            { 
                if (naoCameraUpper != null ) 
                { 
                    Disconnect(); 
                } 
 
                naoCameraUpper = new VideoDeviceProxy(ip, 9559);
               
 
                // Attempt to unsubscribe incase program was not shut down properly 
                try 
                {
                    naoCameraUpper.unsubscribe(subsNameUpper);
                    
                } 
                catch (Exception) 
                { 
                }
                
                // subscribe to NAO Camera for easier access to camera memory 
                naoCameraUpper.subscribe(subsNameUpper, format.id, ColorSpace, FPS);
                
                SetCamera(1);
            } 
            catch (Exception e) 
            { 
                // display error message and write exceptions to a file 
                MessageBox.Show("Exception occurred in naocam connect, error log in C:\\NAOserver\\exception.txt"); 
                naoCameraUpper = null;
                
                System.IO.File.WriteAllText(@"C:\\NAOserver\\exception.txt",e.ToString()); 
            } 
        } 
 
        /// <summary> 
        /// Disconnects from the NAO camera 
        /// </summary> 
        public void Disconnect() 
        { 
            try 
            { 
                if (naoCameraUpper != null ) 
                { 
                    // unsubscribe so the NAO knows we do not need data from the camera anymore 
                    naoCameraUpper.unsubscribe(subsNameUpper);
                    
                } 
            } 
            catch 
            {  } 
 
            naoCameraUpper = null;
            
        }
        /// <summary>
        /// Change to Bottom Cam
        /// </summary>
        public void SetCamera(int option)
        {
            cameraReady = false;
            if(option==0)
                naoCameraUpper.setCameraParameter(subsNameUpper, 18, 0);
            else if(option==1)
                naoCameraUpper.setCameraParameter(subsNameUpper, 18, 1);
            //Thread.Sleep(1000);
            cameraReady = true;
        }
        

        /// <summary> 
        /// Gets an image from the camera 
        /// </summary> 
        /// <returns> single frame from the camera </returns> 
        public byte[] getImageUpper() 
        { 
            byte[] image = new byte[0]; 
 
            try 
            { 
                if (naoCameraUpper != null) 
                {
                    Object imageObject = naoCameraUpper.getImageRemote(subsNameUpper); 
                    image = (byte[])((ArrayList)imageObject)[6]; 
                } 
            } 
            catch (Exception) 
            { } 
            return image; 
        }
        /// <summary> 
        /// Gets an image from the camera 
        /// </summary> 
        /// <returns> single frame from the camera </returns> 
        public byte[] getImageLower()
        {
            byte[] image = new byte[0];

            try
            {
                
            }
            catch (Exception)
            { }
            return image;
        }
    } 
} 
 