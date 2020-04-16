﻿using System;
using System.Threading;
using System.Windows.Forms;
using GrowUpAndWork.LightLogger;
using TaleWorlds.Library;

namespace ModLib.Debugging
{
    public static class ModDebug
    {
        public static void ShowError(string message, string title="", Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "";
            MessageBox.Show($"{message}\n\n{exception?.ToStringFull()}", title);
            
            String logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";
            Log logger = new Log(logFileName);
            logger.WriteLog($"message: {message}, title:{title}, {exception.ToString()}");
        }

        public static void ShowMessage(string message, string title = "", bool nonModal = false)
        {
            if (nonModal)
            {
                new Thread(() => MessageBox.Show(message, title)).Start();
            }
            MessageBox.Show(message, title);
            
            String logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";
            Log logger = new Log(logFileName);
            logger.WriteLog($"{title}, message is: {message}");
        }

        public static void LogError(string error, Exception ex = null)
        {

        }
    }
}
