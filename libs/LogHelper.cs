using System;
using System.IO;
using TaleWorlds.Core;

namespace GrowUpAndWork.LightLogger
{
    public class Log
    {
        private string _logFileName;

        public Log(string logFileName)
        {
            _logFileName = logFileName;
            InitLog();
        }

        private void InitLog()
        {
            if (!File.Exists(_logFileName))
            {
                File.Create(_logFileName);
            }
        }

        public void WriteLog(string msg)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(_logFileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite)))
            {
                sw.WriteLine(DateTime.Now.ToString() + ":" + msg);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }
    }
}