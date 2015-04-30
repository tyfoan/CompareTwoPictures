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
using System.IO;
using Microsoft.Win32;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;

namespace CompareTwoPictures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Bitmap _img1;
        private Bitmap _img2;



        public MainWindow()
        {
            InitializeComponent();
        }



        private Bitmap OpenFileDialog(Image ctrl)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".jpg|*.jpg|.png|*.png|.bmp|*.bmp";
            if (ofd.ShowDialog() ?? false)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(ofd.FileName);
                img.EndInit();
                ctrl.Source = img;
                return Bitmap.FromFile(ofd.FileName) as Bitmap;
            }
            return null;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            _img1 = OpenFileDialog(img1);
        }

        private void btnOpenFile2_Click(object sender, RoutedEventArgs e)
        {
            _img2 = OpenFileDialog(img2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImageComporator a = new ImageComporator(_img1, _img2);
            a.Compare();
            var gr = Graphics.FromImage(_img2);
            gr.DrawRectangles(new System.Drawing.Pen(System.Drawing.Color.Red, 5), a.Areas.ToArray());
            gr.Flush();
            

            using (MemoryStream ms = new MemoryStream())
            {
                _img2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Position = 0;
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.StreamSource = ms;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();

                img3.Source = source;
            }
        }



    }
}
