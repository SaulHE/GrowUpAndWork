using HarmonyLib;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "BecomeInfantAge", MethodType.Getter)]
    public class BecomeInfantAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 1;
        }
    
    }
    
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "BecomeChildAge", MethodType.Getter)]
    public class BecomeChildAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 3;
        }
        
    }
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "BecomeTeenagerAge", MethodType.Getter)]
    public class BecomeTeenagerAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 10;
        }
    }
    
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "HeroComesOfAge", MethodType.Getter)]
    public class HeroComesOfAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 14;
        }
    }
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "BecomeOldAge", MethodType.Getter)]
    public class BecomeOldAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 50;
        }
    }
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultAgeModel), "MaxAge", MethodType.Getter)]
    public class MaxAgePatch
    {
        static void Postfix(ref int __result)
        {
            __result = 80;
        }
    }
    
}