# Face Detection

A simple C# face detection from EmguCV library. Using the classifier is "haarcascade_frontalface_alt2.xml"

## Solution

Find all faces that appear in realtime viedo by fast performance technical.
* Reduce the size of frame by gaussian pyramid down to 320x240 and keep resize ratio.
* Detect the face in odd frame and keep face output to next frame.
* Interpolate the faces result by increase size to original frame.

## Requirement

1. Visual studio 2017
2. Nuget Emgu.CV 3.4.3.3016 and dependency ZedGraph 5.1.7

## Using

The core of Engine class can be set the camera usb port in index (0 base index have the default value).

```C#
int cameraUSBPort = 0;
var engine = new Engine(cameraUSBPort);
```

To run the engine use the Run() method with set the FaceDetection class object. The FaceDetection class must have to set
classifier xml file (Default value we use haarcascade_frontalface_alt2.xml).

```C#
engine.Run(new FaceDetection());
```

To exit program use Esc key.

## Output

<img src="https://drive.google.com/uc?export=view&id=1VYLKaTIllxWdMOT95wCtLpXKKMwj8Skt">
