using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
namespace VisionProDetectObject.Camera.Tool
{
    internal class ClassToolBlockLoader : WindowsFormsHost

    {
        public CogToolBlockEditV2 toolbloockEdit;
        public static readonly DependencyProperty ToolBlockProperty =
            DependencyProperty.Register("ToolBlock", typeof(CogToolBlock), typeof(ClassToolBlockLoader),
                new PropertyMetadata(null, OnToolBlockChanged));
        public CogToolBlock ToolBlock
        {
            get => (CogToolBlock)GetValue(ToolBlockProperty);
            set => SetValue(ToolBlockProperty, value);
        }

        private static void OnToolBlockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = d as ClassToolBlockLoader;
            host.toolbloockEdit.Subject = e.NewValue as CogToolBlock;
        }

        public ClassToolBlockLoader()
        {
            this.Child=toolbloockEdit=new CogToolBlockEditV2();
        }

    }
}
