using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ba_timekeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            var targetRect = new Rect(100, 200, 300, 400);
            var filePath = App.TmpFilePath;

            CaptureScreen(targetRect, filePath);
        }

        private void CaptureScreen(Rect rect, string filePath)
        {
            using (var bm = new Bitmap((int)rect.Width, (int)rect.Height))
            using (var g = Graphics.FromImage(bm))
            {
                g.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bm.Size);
                bm.Save(filePath, ImageFormat.Png);
            }
        }
    }
}
