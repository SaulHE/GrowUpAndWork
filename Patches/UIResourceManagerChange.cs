using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GrowUpAndWorkLib.Debugging;
using HarmonyLib;
using TaleWorlds.Core;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.Localization.MBTextManager),
        "ChangeLanguage")]
    public class ChangeLanguagePatch
    {
        static bool Prefix(string language)
        {
            if (language == "简体中文" || language == "繁體中文")
            {
                SettingClass.CurrentLanguage = "zh";
            }
            else
            {
                SettingClass.CurrentLanguage = "en";
            }

            return true;
        }

        static void Postfix()
        {
            try
            {
                GrowthDebug.LogInfo(GameTexts.FindText("GrowUpAndWork_charactermaxage", null).ToString());
            }
            catch (Exception e)
            {
                GrowthDebug.LogError("Testing string localization", "testing", e);
                
            }
        }
    }
}