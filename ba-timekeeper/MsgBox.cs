using System.Windows;

namespace ba_timekeeper
{
    internal class MsgBox
    {
        public static void ShowErr(string msg)
        {
            MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
