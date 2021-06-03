using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.IO;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using ImageProcessor.Imaging.Quantizers.WuQuantizer;
using System.Drawing.Imaging;

namespace Image_Compression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] files;
        int index = 0;
        bool paused_clicked = false;
        string destPath_ = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        public void CompressImage(string SoucePath, string DestPath)
        {
            var FileName = System.IO.Path.GetFileName(SoucePath);

            DestPath = DestPath + "\\" + FileName;

            var quantizer = new WuQuantizer();

            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                using (var quantized = quantizer.Quantize(bmp1))
                {
                    Dispatcher.Invoke(() =>
                    {

                    });
                    quantized.Save(DestPath, ImageFormat.Png);
                }

            }
        }

        private void browse_btn_Click(object sender, RoutedEventArgs e)
        {
            Helper.ResultFolderDialog(SourcePath);
        }

        private void dest_btn_Click(object sender, RoutedEventArgs e)
        {
            Helper.ResultFolderDialog(destPath);
        }

        private void compress_btn_Click(object sender, RoutedEventArgs e)
        {
            browse_btn.IsEnabled = false;
            dest_btn.IsEnabled = false;
            compress_btn.IsEnabled = false;

            files = Directory.GetFiles(SourcePath.Text);
            pctg.Text = "0/" + files.Length;
            progressBar.Minimum = 0;
            progressBar.Maximum = files.Length;
            destPath_ = destPath.Text;

            //            doWork(paused_clicked);

            new Thread(() =>
            {
                doWork();
            }).Start();

        }

        private void pause_btn_Click(object sender, RoutedEventArgs e)
        {
            if (paused_clicked == false)
            {
                paused_clicked = true;
                pause_btn.Content = "play"; 
            }

            else
            {
                paused_clicked = false;
                pause_btn.Content = "pause";
            }
        }

        private void doWork()
        {
            {
                while (index < files.Length)
                {
                    while (paused_clicked)
                    {
                        Thread.Sleep(1000);
                    }
                    string ext = System.IO.Path.GetExtension(files[index]).ToUpper();
                    if (ext == ".PNG" || ext == ".JPG")
                    {
                        Helper.CompressImage(files[index], destPath_);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        progressBar.Value++;
                        index++;
                        pctg.Text = progressBar.Value + "/" + files.Length;
                    });
                }


                Dispatcher.Invoke(() =>
                        {
                            System.Windows.MessageBox.Show("Finished");
                            destPath.Text = "";
                            SourcePath.Text = "";
                            progressBar.Value = 0;
                            index = 0;
                            pctg.Text = "";

                            browse_btn.IsEnabled = true;
                            dest_btn.IsEnabled = true;
                            compress_btn.IsEnabled = true;
                        });
            }

        }

    }
}
