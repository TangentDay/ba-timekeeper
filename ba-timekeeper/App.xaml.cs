using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ba_timekeeper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string TmpFilePath
        {
            get
            {
                string? appPath = AppContext.BaseDirectory;
                if (appPath == null)
                {
                    throw new DirectoryNotFoundException("fail to get base directory");
                }

                return Path.Combine(appPath, "tmp.png");
            }
        }
    }
}
