using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro;
using Cognex.VisionPro.GenTL;
using VisionProDetectObject.Camera;
using System.Drawing;
using VisionProDetectObject.ViewModels;

namespace VisionProDetectObject.Views
{
    /// <summary>
    /// Interaction logic for ViewImage.xaml
    /// </summary>
    public partial class ViewImageView : UserControl
    {
       
        public ViewImageView()
        {
            InitializeComponent();
          //  this.DataContext = new ViewImageViewModel();
            //ClassCamera.Instance.AcqImageCompleted += cogdisplayLoadImage;
            //ClassCamera.Instance.loadtoolBlock();
            //ClassCamera.Instance.cogToolBlock.Ran += CogToolBlock_Ran;
           // usercontrlLoad();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void CogToolBlock_Ran(object sender, EventArgs e)
        //{
        //    Cognex.VisionPro.PMAlign.CogPMAlignResults cogPMAlignResults = (Cognex.VisionPro.PMAlign.CogPMAlignResults)ClassCamera.Instance.cogToolBlock.Outputs["ToolOuput"].Value;
        //    if (cogPMAlignResults.Count > 0)
        //    {
        //        CogTransform2DLinear cogTransform2d = cogPMAlignResults[0].GetPose();
        //        var rotation = cogTransform2d.Rotation;
        //        txtObjectX.Text = Math.Round(cogTransform2d.TranslationX, 3).ToString();
        //        txtObjectY.Text = Math.Round(cogTransform2d.TranslationY, 3).ToString();
        //        txtSharp.Text = cogPMAlignResults[0].ModelName;
        //    }
        //    //send coordinate to PLC
        //    //Save Image
        //    Bitmap bitmap = ((CogImage8Grey)ClassCamera.Instance.cogToolBlock.Outputs["OImage"].Value).ToBitmap();
        //    DateTime dt = System.DateTime.Now;
        //    bitmap.Save(@"C:\Users\Dell\Documents\testImage\" + "image" + dt.Minute.ToString() + dt.Second.ToString() + ".bmp");
        //}

        //private void btnConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    ClassCamera.Instance.CreateAcqFifo();
        //    if (ClassCamera.Instance.isconnect) { btnConnect.IsEnabled = false; }
        //    else
        //    {
        //        btnConnect.IsEnabled = true;
        //    }
        //}
        //private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        //{
        //    ClassCamera.Instance.disconnectAcq();
        //    if (ClassCamera.Instance.isconnect) { btnConnect.IsEnabled = false; }
        //    else { btnConnect.IsEnabled = true; }
        //}



        //private void cogdisplayLoadImage()
        //{
        //    disCamera.CogDisplay.Image = ClassCamera.Instance.acquiredImage;
        //    disCamera.CogDisplay.Fit();
        //}

        //private void btnTrigger_Click(object sender, RoutedEventArgs e)
        //{
        //    ClassCamera.Instance.triggerImage();
        //}
    }
}
