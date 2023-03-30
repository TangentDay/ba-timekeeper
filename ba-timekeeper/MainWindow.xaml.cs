using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

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
            var filePath = System.IO.Path.Combine(App.TmpDir, DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png");

            CaptureScreen(targetRect, filePath);
        }

        private static void CaptureScreen(Rect rect, string filePath)
        {
            using var bm = new Bitmap((int)rect.Width, (int)rect.Height);
            using var g = Graphics.FromImage(bm);
            g.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bm.Size);
            bm.Save(filePath, ImageFormat.Png);
        }

        private void RefreshTarget_Click(object sender, RoutedEventArgs e)
        {
            RefreshTargetList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTargetList();
        }

        private void RefreshTargetList()
        {
            Refresh.IsEnabled = false;
            Target.Items.Clear();
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.MainWindowTitle.Length > 0)
                {
                    Target.Items.Add($"{p.ProcessName}: {p.MainWindowTitle}");
                }
            }
            Refresh.IsEnabled = true;
        }
    }
}
