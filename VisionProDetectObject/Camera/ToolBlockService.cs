using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;

namespace Camera
{
    public class ToolBlockService
    {
        public CogToolBlock cogToolBlock;
      //  public string ToolBlockPath { get; set; }
        public long numPass  { get; set; }
        public long numFail { get; set; }

        public void InitializerLoadToolBlock()
        {

        }
        public void ToolBlockDispose()
        {

        }
        public CogToolBlock loadtoolBlock(string path)
        {
            //Check file exist
            cogToolBlock = CogSerializer.LoadObjectFromFile(path) as CogToolBlock;
            //    cogToolBlock.Ran += new EventHandler(ToolBlockRunComplete);
            return cogToolBlock;
        }
        public void savetoolBlock(CogToolBlock block, string path)
        {
            CogSerializer.SaveObjectToFile(block, path);
        }

        public async void CogToolBlock_Ran(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {
                //

                // Xử lý hình ảnh trên background thread
                // Bitmap bitmap = ((CogImage8Grey)ClassCamera.Instance.cogToolBlock.Outputs["OImage"].Value).ToBitmap();
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

    }
}
