using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using VisionProDetectObject.Views;
using Camera;
using MobusTCP;
namespace VisionProDetectObject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainView>();
            return w;
        }
        //protected override void ConfigureViewModelLocator()
        //{
        //    base.ConfigureViewModelLocator();
        //}
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICamera, CameraServiceGenTL>();
            containerRegistry.RegisterSingleton<ToolBlockService>();
            containerRegistry.RegisterSingleton<ModbusClientService>();
            // register other needed services here
        }
        protected override void OnInitialized()
        {
            
          //  ToolBlockService toolBlockService = Container.Resolve<ToolBlockService> ();
           // 
            ICamera cameraService = Container.Resolve<ICamera>();
            cameraService.Camera= cameraService.LoadCamera("40315125");
            cameraService.Formart= cameraService.LoadVideoFormat(3);
            base.OnInitialized();

        }
        protected override void OnExit(ExitEventArgs e)
        {
            ICamera cameraService = Container.Resolve<ICamera>();
            if (cameraService.Isconnect)
            {
                cameraService.disconnectAcq();
            }
            base.OnExit(e);
        }
    }
}
