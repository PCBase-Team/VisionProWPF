using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using Cognex.VisionPro;
using Camera;
using MobusTCP;
using VisionProDetectObject.Camera;
using VisionProDetectObject.Helper;
using Cognex.VisionPro.ToolBlock;

namespace VisionProDetectObject.ViewModels
{
    internal class ViewImageViewModel : BindableBase
    {
        private double _exposure;

        public double Exposure
        {
            get { return _exposure; }
            set { SetProperty(ref _exposure, value); }
        }
        private double _brightness;

        public double Brightness
        {
            get { return _brightness; }
            set { SetProperty(ref _brightness, value); }
        }

        private double _contrast;

        public double Contrast
        {
            get { return _contrast; }
            set { SetProperty(ref _contrast, value); }
        }


        private string _objectX;
        public string ObjectX
        {
            get { return _objectX; }
            set { SetProperty(ref _objectX, value); }
        }
        private string _objectY;
        public string ObjectY
        {
            get { return _objectY; }
            set { SetProperty(ref _objectY, value); }
        }

        private string _sharp;
        public string Sharp
        {
            get { return _sharp; }
            set { SetProperty(ref _sharp, value); }
        }

        private string _counterRectang;
        public string CounterRectang
        {
            get { return _counterRectang; }
            set { SetProperty(ref _counterRectang, value); }
        }

        private string _counterSquare;
        public string CounterSquare
        {
            get { return _counterSquare; }
            set { SetProperty(ref _counterSquare, value); }
        }

        private ICogImage _ImageSource;
        public ICogImage ImageSource
        {
            get { return _ImageSource; }
            set { SetProperty(ref _ImageSource, value); }
        }

        public ICommand conectCameraCommand { get; private set; }
        public ICommand disconnectCameraCommand { get; private set; }
        public ICommand captureImageCommand { get; private set; }
        private ICamera _Camera;
        private ToolBlockService _ToolBlockService;
        private ModbusClientService _ModbusClient;
        private ushort[] CoordinateObject;
        private SaveImage SaveImage;
        private bool prevTrigger;
        private ushort[] Data;

        public ViewImageViewModel(ICamera camera, ToolBlockService toolBlock, ModbusClientService modbusClient)
        {
            _Camera = camera;
            _ToolBlockService = toolBlock;
            _ModbusClient = modbusClient;
            SaveImage = new SaveImage();
             _ToolBlockService.loadtoolBlock(@"C:\Users\Dell\Desktop\TB_DetectObject.vpp");
             _ToolBlockService.cogToolBlock.Ran += CogToolBlock_RunComplete;
            _Camera.AcqImageCompleted += Camera_AcqImageCompleted;
            _ModbusClient.DataRefresh += ModbusDataRefresh;
            conectCameraCommand = new DelegateCommand(async () => { await connectCameraExcute(); });
            disconnectCameraCommand = new DelegateCommand(async () => { await DisconnectCameraExecute(); });
            captureImageCommand = new DelegateCommand(CaptureImageExecute);
        }

        private void ModbusDataRefresh(object sender, ModbusDataEventArgs e)
        {
            Data = e.Data;
            bool trigger = Data[5] == 1 ? true : false;//signal ready
            SignalProcess(trigger);
        }

        private void CogToolBlock_RunComplete(object sender, EventArgs e)
        {
            Cognex.VisionPro.PMAlign.CogPMAlignResults cogPMAlignResults = (Cognex.VisionPro.PMAlign.CogPMAlignResults)_ToolBlockService.cogToolBlock.Outputs["ToolOuput"].Value;
            if (cogPMAlignResults.Count > 0)
            {
                CogTransform2DLinear cogTransform2d = cogPMAlignResults[0].GetPose();
                var rotation = cogTransform2d.Rotation;
                ObjectX = Math.Round(cogTransform2d.TranslationX, 3).ToString();
                ObjectY = Math.Round(cogTransform2d.TranslationY, 3).ToString();
                CoordinateObject[0] = Convert.ToUInt16(Math.Round(cogTransform2d.TranslationX));
                CoordinateObject[1] = Convert.ToUInt16(Math.Round(cogTransform2d.TranslationY));
                Sharp = cogPMAlignResults[0].ModelName;
                if (Sharp == "Square")
                {
                    CounterSquare = CounterSquare + 1;
                    CoordinateObject[2] = 1;
                }
                else if (Sharp == "Rect")
                {
                    CounterRectang = CounterRectang + 1;
                    CoordinateObject[2] = 0;
                }
            }
            if (_ModbusClient.IsConnected)
            {
                
                _ModbusClient.Master.WriteMultipleRegistersAsync(1, 1, CoordinateObject);
            }
            SaveImage.Save(_ToolBlockService.cogToolBlock.Outputs["OImage"].Value);
        }

        private void Camera_AcqImageCompleted()
        {
            ImageSource = _Camera.acquiredImage;
            _ToolBlockService.cogToolBlock.Inputs["InputImage"].Value = _Camera.acquiredImage as CogImage8Grey;
            _ToolBlockService.cogToolBlock.Run();
        }
        private void SignalProcess(bool trigger)
        {
            // Edge detection: OFF -> ON

            if (trigger && !prevTrigger)
            {
                if (_Camera.Isconnect)
                    _Camera.AcquireImage();
            }
            // Nếu trigger OFF thì tắt ready
            //else if (!trigger && readyOn)
            //{
            //    readyOn = false;

            //}
            prevTrigger = trigger;
        }
        private void CaptureImageExecute()
        {
            if (_Camera.acquiredImage != null)
                _Camera.AcquireImage();
        }

        private async Task DisconnectCameraExecute()
        {
            if (_Camera != null)
            {
                _Camera.disconnectAcq();
            }
        }

        private async Task connectCameraExcute()
        {
            if (_Camera == null)
            {
                _Camera.CreateAcqFifo(_Camera.Camera, _Camera.Formart);
            }
        }
    }
}
