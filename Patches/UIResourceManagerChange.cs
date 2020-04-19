using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GrowUpAndWorkLib.Debugging;
using HarmonyLib;
using ModLib;
using ModLib.Attributes;
using ModLib.GUI.ViewModels;

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
    }
}