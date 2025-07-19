using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
//using VisionProDetectObject.Camera;

namespace VisionProDetectObject.Helper
{
    internal class SaveImage
    {
        public void Save(object image)
        {
            try
            {
                Bitmap bitmap = ((CogImage8Grey)image).ToBitmap();
                DateTime dt = System.DateTime.Now;
                bitmap.Save(@"C:\Users\Dell\Documents\testImage\" + "image" + dt.Minute.ToString() + dt.Second.ToString() + ".bmp");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
