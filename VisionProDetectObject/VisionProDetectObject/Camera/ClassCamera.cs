using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.GenTL;
using Cognex.VisionPro;
using System.Windows;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ToolBlock;
using System.Drawing;
using System.Windows.Threading;

namespace VisionProDetectObject.Camera
{
    public sealed class ClassCamera
    {
        private static readonly Lazy<ClassCamera> lazy =
       new Lazy<ClassCamera>(() => new ClassCamera());

        public static ClassCamera Instance { get { return lazy.Value; } }

        private ClassCamera()
        {
            InitializerCamera();
        }
        #region AcqImage
        // Khai báo delegate cho event
        public delegate void Notify();
        public event Notify AcqImageCompleted;

        private CogFrameGrabberGenTLs enumeratedCameras;
        private ICogAcqFifo activeFifo;
        private int currentAcquisitionTicket = -1;
        private String videoFormat;
        private String cameraName;
        public bool CameraReady = false;
        private ICogFrameGrabber castedCamera;
        CogStringCollection availableFormats = null;
        public ICogImage acquiredImage;
        public bool isconnect = false;

        private void InitializerCamera()
        {
            enumeratedCameras = new CogFrameGrabberGenTLs();
            if (enumeratedCameras.Count == 0)
            {
                MessageBox.Show("No GenTL cameras were enumerated. " +
                  "Please ensure that at least one USB3 GenTL is connected to the system and " +
                  "is receiving power. Refer to the 'GenTL Architecture' page in the help documentation " +
                  "for more information on requirements such as cable usage for GenTL camera enumeration if failure to " +
                  "enumerate persists");
                return;
            }
            else if (enumeratedCameras.Count == 1)
            {
                if (enumeratedCameras[0].OwnedGenTLAccess.TLType.Equals("U3V"))
                {
                    castedCamera = enumeratedCameras[0];

                    cameraName = castedCamera.Name;
                    availableFormats = castedCamera.AvailableVideoFormats;
                    if (availableFormats.Count > 0)
                    {
                        videoFormat = availableFormats[3];
                    }
                }
                else
                    cameraName = null;

            }
        }
        private void DisposeActiveFifo()
        {
            isconnect = false;
            CameraReady = false;
            //Dispose of the previous Fifo. These hold references in the module, so it is 
            //a good practice to dispose of them once you are finished using the Fifo.
            if (activeFifo != null)
            {
                activeFifo.Flush();
                activeFifo.Complete -= ActiveFifo_Complete;
                //The underlying object implements IDisposable
                ((IDisposable)activeFifo).Dispose();
                activeFifo = null;
            }
        }
        private int completeCallCount = 0;
        private int garbageCollectionCounter = 0;
        private int pending = 0;
        private int ImageReady = 0;
        private bool busy = false;
        private void ActiveFifo_Complete(object sender, CogCompleteEventArgs e)
        {
            if (completeCallCount < Int32.MaxValue)
            {//Just in case, unlikely you'd ever hit the max value of an int in this circumstance, but it is good to keep such scenarios in mind
                completeCallCount++;
            }

            acquiredImage = null;
            //We are not using these, but they are required by CompleteAcquire
            //See the documentation on CompleteAcquire for further details
            int completedTicket = -1;
            int triggerNumber = -1;

            try
            {
                activeFifo.GetFifoState(out pending, out ImageReady, out busy); //The important value is ready, informing us if any images may be retrieved via CompleteAcquire.
                if (ImageReady > 0) //In theory, this should always be true for our particular application. In practice, it does not hurt to be certain.
                {
                    //currentAcquisitionTicket is -1 in the case of continuous, meaning take the oldest buffered acquistion result
                    acquiredImage = activeFifo.CompleteAcquire(currentAcquisitionTicket, out completedTicket, out triggerNumber);
                }
            }
            catch (Exception err)
            {
                //We'll get things back in the default state before triggers were initiated
                activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual;
                activeFifo.Flush();

                //if (InvokeRequired)
                //{//This is needed because this event handler does not execute on the main GUI thread, but we are performing changes to GUI components
                //    Invoke(new handleCompleteAcquireExceptionDelegate(handleCompleteAcquireException), new object[] { err });
                //}

                return;//Don't bother trying to set the display
            }

            AcqImageCompleted?.Invoke();
            garbageCollectionCounter++;
            CameraReady = true;

            //This is important, without the periodic garbage collection calls, the garbage collector will not think it needs to free the relatively small amount of
            //managed memory in use. However, that managed memory is holding alive a much larger amount of unmanaged memory by maintaining a reference to it. 
            //Thus we must manually remind the garbage collector to do a collection periodically in order to work around this limitation of mixed managed/unmanaged code.
            //You do not need to use 4 specifically, but the value that you choose should be relatively small and 4 is generally a reasonable value to keep unmanaged 
            //memory under control.
            if (garbageCollectionCounter > 4)
            {
                garbageCollectionCounter = 0;
                GC.Collect();
            }
        }

        public void triggerImage()
        {
            try
            {
                activeFifo.Flush();//Just in case. Should not be any outstanding acquisitions
                activeFifo.OwnedTriggerParams.TriggerEnabled = true;
                currentAcquisitionTicket = activeFifo.StartAcquire();
                CameraReady = false;
            }
            catch (Exception err)
            {
                MessageBox.Show("Exception of type " + err.GetType() + " was encountered like attempting to start a single acquire with the message: " +
                  err.Message);
                CameraReady = true;
            }
        }
        public void CreateAcqFifo()
        {
            try
            {
                activeFifo =
                castedCamera.CreateAcqFifo(videoFormat, /* Video Format is what determines what will be set for what the Genicam spec calls the Pixel Format */
                CogAcqFifoPixelFormatConstants.Format8Grey /* Ignored param in all cases, included for backwards compatibility reasons */,
                 0 /* For a USB3 camera, there are no ports, this is relevant for actual framegrabbers, which could be supported by GenTL but not this sample */,
                 true /* This one is not required to be true, we're just using that for this sample. Refer to documentation for details */);
                activeFifo.OwnedExposureParams.Exposure = 2;
            }
            catch
            {

            }
            try
            {
                activeFifo.Complete += ActiveFifo_Complete;
                isconnect = true;
                CameraReady = true;
            }
            catch (Exception err)
            {

                MessageBox.Show("Exception of type " + err.GetType() + " was encountered while attempting to assign event handlers for the AcqFifo (video format: " +
                  videoFormat + ", camera name: " + cameraName + ") with the message: " + err.Message);

                //Failed to properly configure the fifo, don't enable any buttons and clear out the fifo
                DisposeActiveFifo();
                return;
            }
        }
        public void disconnectAcq()
        {
            DisposeActiveFifo();
            CogFrameGrabbers allFramegrabbers = new CogFrameGrabbers();
            foreach (ICogFrameGrabber allFramegrabber in allFramegrabbers)
            {
                try
                {
                    allFramegrabber.Disconnect(false);
                }
                catch
                {

                }

            }

        }
        #endregion
        #region Load Tool Block
        private CogImageFileTool mIFTool;
        public CogToolBlock cogToolBlock;
        private long numPass = 0;
        private long numFail = 0;
        public void InitializerLoadToolBlock()
        {

        }
        public void ToolBlockDispose()
        {

        }
        public CogToolBlock loadtoolBlock()
        {
            cogToolBlock = CogSerializer.LoadObjectFromFile(@"C:\Users\Dell\Desktop\TB_DetectObject.vpp") as CogToolBlock;
            //    cogToolBlock.Ran += new EventHandler(ToolBlockRunComplete);
            return cogToolBlock;
        }
        public void savetoolBlock(CogToolBlock block)
        {
            CogSerializer.SaveObjectToFile(block, @"C:\Users\Dell\Desktop\TB_DetectObject.vpp");
        }

        private async void CogToolBlock_Ran(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {
                //

                // Xử lý hình ảnh trên background thread
                Bitmap bitmap = ((CogImage8Grey)ClassCamera.Instance.cogToolBlock.Outputs["OImage"].Value).ToBitmap();
            });

            // Hiển thị form trên UI thread
            //Dispatcher.Invoke(() =>
            //{
            //   // ShowResultImageView showResultImageView = new ShowResultImageView(bitmap, "test");
            //   // showResultImageView.Show();
            //});

        }
        public void toolBlockRunOnce()
        {

        }
        #endregion
    }
}
