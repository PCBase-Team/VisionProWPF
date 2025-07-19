using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Implementation;


namespace VisionProDetectObject.Camera.Tool
{
    internal class ClassCogdisplay : WindowsFormsHost
    {
        public CogDisplay CogDisplay;
       // public Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        
        public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ICogImage), typeof(ClassCogdisplay),
            new PropertyMetadata(null, OnImageChanged));
        public ICogImage Image
        {
            get => (ICogImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = d as ClassCogdisplay;
            host.CogDisplay.Image = e.NewValue as ICogImage;
        }
        public ClassCogdisplay()
        {
            this.Child = CogDisplay = new CogDisplay();
        }
    }
}
