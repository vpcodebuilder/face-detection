using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace FaceDetection
{
    public class FaceDetection
    {
        private const string FRONTAL_FACE_CASCADE = "haarcascade_frontalface_alt2.xml";
        private CascadeClassifier haarCascade;
        
        public Mat OutputFrame
        {
            get;
            private set;
        }

        public FaceDetection() : this(Environment.CurrentDirectory + "\\" + FRONTAL_FACE_CASCADE) { }

        public FaceDetection(string cascadeXmlPathFileName)
        {
            try
            {
                haarCascade = new CascadeClassifier(cascadeXmlPathFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Rectangle[] Detect(Mat inputFrame, bool isFastDetectMode = true)
        {
            int ratioScale = 1;

            // Create output frame.
            OutputFrame = new Mat();

            // Convert BGR from inputFrame to grayscale outputFrame.
            CvInvoke.CvtColor(inputFrame, OutputFrame, ColorConversion.Bgr2Gray);

            if (isFastDetectMode)
            {
                // This is the basic technical to fast detection. We have reduce the size of image
                // to small size for good result in 320x240 pixels and
                // use the gaussian pyramid down solution to reduce the image.
                while (OutputFrame.Width > 320)
                {
                    CvInvoke.PyrDown(OutputFrame, OutputFrame);
                    ratioScale *= 2;
                }
            }
            
            // Default to face detection.
            Rectangle[] faces = haarCascade.DetectMultiScale(OutputFrame);

            // Have face to return and fast detection.
            if (faces.Length > 0 && isFastDetectMode)
            {
                // We have resize of rectangle to inform original image.
                for (int i = 0; i < faces.Length; i++)
                {
                    faces[i].X *= ratioScale;
                    faces[i].Y *= ratioScale;
                    faces[i].Width *= ratioScale;
                    faces[i].Height *= ratioScale;
                }
            }

            return faces;
        }
    }
}
