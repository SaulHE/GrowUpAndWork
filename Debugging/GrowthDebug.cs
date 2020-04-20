// #define DEBUG_MODE_G
using System;
using System.Threading;
using System.Windows.Forms;
using GrowUpAndWork;
using ModLib;
using Serilog;
using Serilog.Core;

namespace GrowUpAndWorkLib.Debugging
{
    public class GrowthDebug
    {
        private static Logger log = new LoggerConfiguration().WriteTo.File(SettingClass.LogFileName,
                rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 90000000)
            .CreateLogger();

        public static void ShowMessageInBox(string message)
        {
            MessageBox.Show($"{message}", "This is a log Info");
        }

        public static void ShowError(string message, string title = "", Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "";

            MessageBox.Show($"{message}\n\n{exception?.ToStringFull()}", title);

            LogError(message, title, exception);
        }

        public static void LogInfo(string message, string title = "")
        {
#if DEBUG_MODE_G
            log.Information("------------");
            log.Information($"title: {title}, Pure Log");
            log.Information($"{message}");
#endif
        }

        public static void LogError(string message, string title = "", Exception exception = null)
        {
            log.Information("============================================>");
            log.Information($"!!!This is An Error, Happens in {DateTime.Now.ToString()} : {title},");
            log.Error($"{message} The detailed information is {exception.ToStringFull()}", exception);
            log.Information("<==============================================");
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