using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace FaceDetection
{
    public class Engine
    {
        private const string ORIGINAL_WINDOW_NAME = "original";
        private const string OUTPUT_WINDOW_NAME = "output";
        private const int ESC_KEY = 27;
        private VideoCapture capture;
        private Fps fps;
        private FaceDetection faceDetection;
        private MCvScalar redColor = new MCvScalar(0, 0, 255);
        private MCvScalar whiteColor = new MCvScalar(255, 255, 255);
        private MCvScalar fontColor = new MCvScalar(0, 0, 0);
        private Mat rememberFrame;
        
        public Engine(int captureIndex = 0)
        {
            try
            {
                capture = new VideoCapture(captureIndex);
                capture.ImageGrabbed += ProcessFrame;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            fps = new Fps(capture.GetCaptureProperty(CapProp.Fps));
        }
        
        public void Run(FaceDetection faceDetection)
        {
            CvInvoke.NamedWindow(ORIGINAL_WINDOW_NAME);
            CvInvoke.NamedWindow(OUTPUT_WINDOW_NAME);
            this.faceDetection = faceDetection;

            capture.Start();
            fps.Start();

            while (CvInvoke.WaitKey() == ESC_KEY)
            {
                capture.ImageGrabbed -= ProcessFrame;
                capture.Stop();
                fps.Stop();
                CvInvoke.DestroyAllWindows();
                break;
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            // Update frame counter.
            fps.UpdateFrameCount();

            // Create the inputFrame.
            Mat inputFrame = new Mat();

            // Get image frame from camera to inputFrame.
            capture.Retrieve(inputFrame);

            // Show the original frame.
            CvInvoke.Imshow(ORIGINAL_WINDOW_NAME, inputFrame);

            // Increase the speed to detection. We have set the detect frame one by one skip.
            if (fps.FrameCounter % 2 != 0)
            {
                // We have use face detection object to detect inputFrame and return face rectangle.
                Rectangle[] faces = faceDetection.Detect(inputFrame);

                foreach (Rectangle face in faces)
                {
                    // Draw the red color rectangle into inputFrame.
                    CvInvoke.Rectangle(inputFrame, face, redColor);
                }

                // keep frame to detect.
                rememberFrame = inputFrame;
            }
            else
            {
                // if frame have no detect. Use the remember last frame.
                inputFrame = rememberFrame;
            }
            
            // Draw title bar.
            CvInvoke.Rectangle(inputFrame, new Rectangle(0, 0, inputFrame.Width, 20), whiteColor, -1);
            // Draw text.
            CvInvoke.PutText(inputFrame, "Fps:" + fps.AverageFrameRate.ToString("N2"), new Point(5, 15), FontFace.HersheyPlain, 1.0, fontColor);
            
            // Show the output frame.
            CvInvoke.Imshow(OUTPUT_WINDOW_NAME, inputFrame);
        }
    }
}
