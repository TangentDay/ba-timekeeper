using System;
using System.IO;
using System.Windows;

namespace ba_timekeeper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string TmpDir
        {
            get
            {
                string? appPath = AppContext.BaseDirectory;
                if (appPath == null)
                {
                    throw new DirectoryNotFoundException("fail to get base directory");
                }

                return Path.Combine(appPath, "tmp");
            }
        }
    }
}
