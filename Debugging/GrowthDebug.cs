#define DEBUG_MODE_G
using System;
using System.Threading;
using System.Windows.Forms;
using GrowUpAndWork;
using HarmonyLib;

namespace GrowUpAndWorkLib.Debugging
{
    public class GrowthDebug
    {
        // private static Logger log = new LoggerConfiguration().WriteTo.File(SettingClass.LogFileName,
        //         rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 90000000)
        //     .CreateLogger();
        public GrowthDebug()
        {
            HarmonyLib.FileLog.logPath = SettingClass.LogFileName;
        }

        public static void ShowMessageInBox(string message)
        {
            MessageBox.Show($"{message}", "This is a log Info");
        }

        public static void ShowError(string message, string title = "", Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "";

            MessageBox.Show($"{message}\n\n{exception}", title);
            LogError(message, title, exception);
        }

        public static void LogInfo(string message, string title = "")
        {
#if DEBUG_MODE_G
            FileLog.Log("--------------");
            FileLog.Log($"title: {title}, Pure Log");
            FileLog.Log($"{message}");
#endif
        }

        public static void LogError(string message, string title = "", Exception exception = null)
        {
            FileLog.Log($"============={title}===============================>");
            FileLog.Log($"!!!This is An Error, Happens in {DateTime.Now.ToString()}");
            FileLog.Log($"{message} The detailed information is {exception.ToString()}");
            FileLog.Log("<===================================================");
        }

        public static void ShowMessage(string message, string title = "", bool nonModal = false)
        {
            if (nonModal)
            {
                new Thread(() => MessageBox.Show(message, title)).Start();
            }

            MessageBox.Show(message, title);

            String logFileName = SettingClass.LogFileName;
            LogInfo(message, title);
        }
    }
}