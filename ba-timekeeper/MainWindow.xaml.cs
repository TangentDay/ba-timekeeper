using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ba_timekeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        OcrEngine ocrEngine;

        public MainWindow()
        {
            InitializeComponent();
            ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
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


        private async void Capture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IntPtr handle = (IntPtr)Target.SelectedValue;
                var bmp = CaptureWindow(handle);

                var sofBmp = await ConvertSoftwareBmp(bmp);

                var ocrResult = await RecognizeText(sofBmp);

                Result.Text = ocrResult.Text;
            }
            catch (Exception ex)
            {
                MsgBox.ShowErr(ex.ToString());
            }
        }

        private async Task<OcrResult> RecognizeText(SoftwareBitmap sofBmp)
        {
            var ocrResult = await ocrEngine.RecognizeAsync(sofBmp);
            return ocrResult;
        }

        private static async Task<SoftwareBitmap> ConvertSoftwareBmp(Bitmap bmp)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".bmp";
            var filePath = System.IO.Path.Combine(App.TmpDir, fileName);
            bmp.Save(filePath, ImageFormat.Bmp);
            StorageFolder appFolder = await StorageFolder.GetFolderFromPathAsync(App.TmpDir);
            var bmpFile = await appFolder.GetFileAsync(fileName);
            SoftwareBitmap sofBmp;

            using (IRandomAccessStream stream = await bmpFile.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                sofBmp = await decoder.GetSoftwareBitmapAsync();
            }
            return sofBmp;
        }

        private static void CaptureScreen(Rect rect, string filePath)
        {
            using Bitmap bmp = new((int)rect.Width, (int)rect.Height);
            using Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bmp.Size);
            bmp.Save(filePath, ImageFormat.Bmp);
        }

        private static Bitmap CaptureWindow(IntPtr handle)
        {
            bool ok = GetWindowRect(handle, out RECT rect);
            if (!ok)
            {
                throw new Exception("fail to get window rect");
            }
            Bitmap bmp = new(rect.right - rect.left, rect.bottom - rect.top);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr dc = g.GetHdc();
            PrintWindow(handle, dc, 0);
            g.ReleaseHdc(dc);
            g.Dispose();
            return bmp;
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
