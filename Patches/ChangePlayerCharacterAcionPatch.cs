using System;
using GrowUpAndWorkLib.Debugging;
using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.Actions.ChangePlayerCharacterAction), "Apply")]
    public class ChangePlayerCharacterActionPatch
    {
        static void Postfix()
        {
            if (MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.LastVisitedSettlement == null)
            {
                GrowthDebug.LogInfo("ChangePlayerCharacterAction, two args are null");
                MobileParty.MainParty.CurrentSettlement =
                    SettlementHelper.FindNearestSettlement((Func<Settlement, bool>) (sett =>
                        sett.IsCastle || sett.IsTown));
            }
        }
    }
}