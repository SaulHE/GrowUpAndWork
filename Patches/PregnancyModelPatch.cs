using System.Text;
using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;

namespace GrowUpAndWork.Patches
{

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPregnancyModel),
        "PregnancyDurationInDays", MethodType.Getter)]
    public class PregnancyDurationInDaysPatch
    {
        static void Postfix(ref float __result)
        {
            __result = SettingClass.Instance.PregnancyDurationInDays;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPregnancyModel),
        "MaternalMortalityProbabilityInLabor", MethodType.Getter)]
    public class MaternalMortalityProbabilityInLaborPatch
    {
        static void Postfix(ref float __result)
        {
            if (SettingClass.Instance.DisableMaternalMortality)
            {
                __result = 0.0f;
            }
            else
            {
                __result = 0.015f;
            }
        }
    }


    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPregnancyModel),
        "StillbirthProbability", MethodType.Getter)]
    public class StillbirthProbabilityPatch
    {
        static void Postfix(ref float __result)
        {
            __result = SettingClass.Instance.StillBirthProbability;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPregnancyModel),
        "IsHeroAgeSuitableForPregnancy")]
    public class IsHeroAgeSuitableForPregnancyPatch
    {
        static bool Prefix(ref bool __result, Hero hero)
        {
            __result = (hero.Age >= (double) SettingClass.Instance.MinPregnantAge) &&
                       (hero.Age <= (double) SettingClass.Instance.MaxPregnantAge);
            return false;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPregnancyModel),
        "GetDailyChanceOfPregnancyForHero")]
    public class GetDailyChanceOfPregnancyForHero
    {
        static bool Prefix(ref float __result, Hero hero)
        {
            if (hero == Hero.MainHero || hero.Spouse == Hero.MainHero || hero.Clan.Leader == Hero.MainHero)
            {
                float num = 0.0f;
                if (hero.Spouse != null && hero.Age >= (double) SettingClass.Instance.MinPregnantAge &&
                    hero.Age <= (double) SettingClass.Instance.MaxPregnantAge &&
                    SettingClass.Instance.DailyPregnancyChanceOfTheMC - 0.0f >= 0.09f)
                {
                    ExplainedNumber bonuses =
                        new ExplainedNumber(1f, new StringBuilder("The chance of hero being pregnant"));
                    PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.PerfectHealth,
                        hero.Clan.Leader.CharacterObject, ref bonuses);
                    num = (float) bonuses.ResultNumber * 0.001f + SettingClass.Instance.DailyPregnancyChanceOfTheMC;
                    if (num >= 1.0f)
                    {
                        num = 1.0f;
                    }
                }
                __result = num;
                return false;
            }

            return true;
        }
    }
}