using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.Towns;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(CharacterDevelopmentCampaignBehaivor), "DailyTickHero")]
    public class DailyTickHeroPatch
    {
        static bool Prefix(Hero hero)
        {
            if ((hero.HeroDeveloper.UnspentFocusPoints > 0 && hero.HeroDeveloper.NumberOfOpenedPerks > 0 ||
                 hero.HeroDeveloper.UnspentAttributePoints > 0) &&
                hero.Clan != Clan.PlayerClan && hero.IsNoble && !hero.IsChild)
            {
                CharacterDevelopmentCampaignBehaivor.DevelopCharacterStats(hero);
            }
            return false;
        }
    }
}