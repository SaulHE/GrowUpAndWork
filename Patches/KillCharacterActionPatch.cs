namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.AgingCampaignBehavior), "DailyTick")]
    public class ApplyByOldAgePatch
    {
        
    }
}