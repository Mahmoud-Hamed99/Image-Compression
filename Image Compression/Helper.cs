using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ImageProcessor.Imaging.Quantizers.WuQuantizer;
using System.Windows.Threading;

namespace Image_Compression
{
    public class Helper
    {
        public static void ResultFolderDialog(System.Windows.Controls.TextBox Filepath)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
             folderDlg.RootFolder =Environment.SpecialFolder.UserProfile;
            folderDlg.SelectedPath = @"C:\Users\OffBeat\source\repos\Image Compression\Image Compression\bin\Debug";
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                Filepath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                
            }
        }
        
        public static void CompressImage(string SoucePath, string DestPath)
        {
            var FileName = Path.GetFileName(SoucePath);

            DestPath = DestPath + "\\" + FileName;

            var quantizer = new WuQuantizer();

            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                using (var quantized = quantizer.Quantize(bmp1))
                {
                    //Dispatcher.Invoke(() => { });
                    quantized.Save(DestPath, ImageFormat.Png);
                }

            }
        }

       

    }
}
