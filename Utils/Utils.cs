using System;
using System.Linq;
using GrowUpAndWorkLib.Debugging;

namespace GrowUpAndWork
{
    public class Utils
    {
        public static int MBStringIdToInt(String stringId)
        {
            string digits = new string(stringId.Trim().SkipWhile(c => !("0123456789").Contains(c)).ToArray());
            bool resBool = Int32.TryParse(digits, out int tempResult);
            if (resBool)
            {
                // GrowthDebug.LogInfo($"The string id int number is {tempResult}");
                return tempResult;
            }
            // GrowthDebug.LogInfo($"Pass Error, the error input is {stringId}");
            return -1;
        }

        public static string MBStringIdExtractCharString(String stringId)
        {
            string charString = new string(stringId.Trim().SkipWhile(c => ("0123456789").Contains(c)).ToArray());
            return charString;
        }
    }
}