using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cognex.VisionPro;
using VisionProDetectObject.Camera;
using static System.Net.Mime.MediaTypeNames;

namespace VisionProDetectObject.Views
{
    /// <summary>
    /// Interaction logic for ShowResultImageView.xaml
    /// </summary>
    public partial class ShowResultImageView : Window
    {
        public ShowResultImageView()
        {
            InitializeComponent();
        }
        public ShowResultImageView(Bitmap bitmap,string test)
        {
            InitializeComponent();
            this.Name = test;
            if (bitmap != null)
            {
                IntPtr hBitmap = bitmap.GetHbitmap();
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                Display.Source = bitmapSource;
                //Display.CogDisplay.Image = cogImage;
                //Display.CogDisplay.Fit();
            }
        }
        //public ShowResultImageView(Bitmap image) { 
        //    Display.DataContext= image;
        //}
    }
}
