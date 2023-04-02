using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;

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

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);


        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            var filePath = System.IO.Path.Combine(App.TmpDir, DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".bmp");
            IntPtr handle = (IntPtr)Target.SelectedValue;
            CaptureWindow(handle, filePath);

        }

        private static void CaptureScreen(Rect rect, string filePath)
        {
            using Bitmap bm = new((int)rect.Width, (int)rect.Height);
            using Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bm.Size);
            bm.Save(filePath, ImageFormat.Bmp);
        }

        private static void CaptureWindow(IntPtr handle, string filePath)
        {
            bool ok = GetWindowRect(handle, out RECT rect);
            if (!ok)
            {
                return;
            }

            Bitmap bm = new(rect.right - rect.left, rect.bottom - rect.top);
            Graphics g = Graphics.FromImage(bm);
            IntPtr dc = g.GetHdc();
            PrintWindow(handle, dc, 0);
            g.ReleaseHdc(dc);
            g.Dispose();
            bm.Save(filePath, ImageFormat.Bmp);
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
                    Target.Items.Add(p);
                }
            }
            Refresh.IsEnabled = true;
        }
    }
}
