using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Microsoft.Win32;
using Unity.Injection;
using VisionProDetectObject.Camera;
using VisionProDetectObject.Modbus;
using VisionProDetectObject.Views;
using VisionProDetectObject.ViewModels;

namespace VisionProDetectObject.Views
{
    /// <summary>
    /// Interaction logic for modbusView.xaml
    /// </summary>
    public partial class ModbusView : System.Windows.Controls.UserControl
    {
       
        //private ModbusPolling ModbusPolling;
        //private bool prevTrigger;
        //private bool trigger;
        //private ushort[] Data;
        //private bool readyOn;

        public ModbusView()
        {
            InitializeComponent();
           // this.DataContext = new MobusViewModel();
           //// ResgisterUpdated += res => txtData.Text = res[0].ToString();
           // ClassCamera.Instance.AcqImageCompleted += AcqImageComplete;
           //// ClassCamera.Instance.cogToolBlock.Ran += CogToolBlock_Ran;
           
           // ModbusPolling = new ModbusPolling();
           // ModbusPolling.DataRefresh += ModbusResfeshData;


        }

        //private void ModbusResfeshData(object sender, ModbusDataEventArgs e)
        //{
        //    Data = e.Data;
        //    trigger = Data[5] == 1 ? true : false;//signal ready
        //    signalProcess();
        //}

        //private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        //{
        //    ModbusTCPClient.Instance.Disconnect();
        //    ModbusPolling.Stop();
        //}


        //private void AcqImageComplete()
        //{
        //    ClassCamera.Instance.cogToolBlock.Inputs["InputImage"].Value = ClassCamera.Instance.acquiredImage as CogImage8Grey;
        //    ClassCamera.Instance.cogToolBlock.Run();
        //}
        //private async void CogToolBlock_Ran(object sender, EventArgs e)
        //{
        //    //write signal ResultReady to PLC
        //    //await client.Master.WriteSingleRegisterAsync(1, 1, 1);

        //}


        //private void signalProcess()
        //{
        //    // Edge detection: OFF -> ON
            
        //    if (trigger && !prevTrigger)
        //    {
        //        if (ClassCamera.Instance.isconnect)
        //            ClassCamera.Instance.triggerImage();
        //    }
        //    // Nếu trigger OFF thì tắt ready
        //    else if (!trigger && readyOn)
        //    {
        //        readyOn = false;

        //    }
        //    prevTrigger = trigger;
        //}

        //private void btnConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    connnectionAsync();
        //}

        //private async Task connnectionAsync()
        //{
        //    await ModbusTCPClient.Instance.ConnectAsync(txtIP.Text, 502);
        //    if (ModbusTCPClient.Instance.IsConnected)
        //        ModbusPolling.Start();
        //}
    }
}
