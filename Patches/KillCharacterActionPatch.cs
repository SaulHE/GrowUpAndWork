using GrowUpAndWorkLib.Debugging;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace GrowUpAndWork.Patches
{

    [HarmonyPatch(typeof(KillCharacterAction), "ApplyInternal")]
    public class ApplyInternalPatch
    {
        static bool Prefix(Hero victim,
            Hero killer,
            KillCharacterAction.KillCharacterActionDetail actionDetail,
            bool showNotification)
        {
            
            GrowthDebug.LogInfo($"KillCharacterActionDetail? --> {actionDetail.ToString()}");
            return true;
        }
    }
}