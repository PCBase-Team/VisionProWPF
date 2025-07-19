using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Input;
using Cognex.VisionPro.ToolBlock;
using Camera;
using System.Windows.Media.Media3D;
namespace VisionProDetectObject.ViewModels
{
    public class EditProgramViewModel : BindableBase

    {

        private CogToolBlock _cogToolBlock;
        public CogToolBlock CogToolBlock
        {
            get { return _cogToolBlock; }
            set { SetProperty(ref _cogToolBlock, value); }
        }
       
        public ICommand runOnceCommand { get;private set; }
        //public EditProgramViewModel()
        //{
        //    runOnceCommand = new DelegateCommand(runOnceExecute);
        //}
        private ICamera _camera;
        public EditProgramViewModel(ToolBlockService toolBlockService,ICamera camera)//CogToolBlock cogToolBlock)
        {
            //toolBlockService.loadtoolBlock(@"C:\Users\Dell\Desktop\TB_DetectObject.vpp");
            CogToolBlock = toolBlockService.cogToolBlock;
            _camera = camera;
            runOnceCommand = new DelegateCommand(runOnceExecute);
        }

        private void runOnceExecute()
        {
            if (_camera == null)
            {
                _camera.CreateAcqFifo(_camera.Camera, _camera.Formart);
            }
        }
    }
}
