using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using static Camera.CameraServiceGenTL;

namespace Camera
{
    public interface ICamera
    {
        double Exposure { get; set; }
        double Brightness { get; set; }
        double Contrast { get; set; }
        bool CameraReady { get; set; }
        bool Isconnect { get; set; }
        ICogImage acquiredImage { get; set; }
        ICogFrameGrabber Camera {  get; set; }
        string Formart { get; set; }

        event Notify AcqImageCompleted;
        void InitializerCamera();
        ICogFrameGrabber LoadCamera(string serinumber);
        string LoadVideoFormat(int index);
        string LoadVideoFormat(string s);
        void DisposeActiveFifo();
        void AcquireImage();
        void CreateAcqFifo(ICogFrameGrabber acqFifo, string videoFormat);
        void disconnectAcq();

    }
}
