using System;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using Helpers;
using ModLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.MountAndBlade.Module), "OnApplicationTick")]
    public class OnApplicationTickPatch
    {
        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                ModDebug.ShowError(
                    $"Bannerlord has encounter an error and needs to close. See the error information below.",
                    "Mount and Blade Bannerlord has crashed", __exception);
            }
        }
    }

}