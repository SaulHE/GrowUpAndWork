using System;
using GrowUpAndWorkLib.Debugging;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(HeroCreator), "DeliverOffSpring")]
    public class DeliverOffSpringPatch
    {
        /**
         * @detail:  change the seed of the children when delivered. To change children's appearance, make them look different.
         */
        static void Postfix(Hero mother, Hero father, bool isOffspringFemale, Hero __result)
        {
            if (__result == null)
            {
                GrowthDebug.LogError("The offspring is null", "Offspring", null);
                return;
            }

            if (__result.Mother == Hero.MainHero || __result.Father == Hero.MainHero ||
                __result.Clan == Hero.MainHero.Clan)
            {
                BodyProperties bodyPropertiesMinFather = father.CharacterObject.GetBodyPropertiesMin(false);
                BodyProperties bodyPropertiesMinMother = mother.CharacterObject.GetBodyPropertiesMin(false);

                int seed = __result.StringId.GetDeterministicHashCode() * 6791 + 1 * 197; 
                
                Random rd = new Random();
                int addition = rd.Next(-5, 5);
                seed += addition;
                
                seed = (seed >= 0) ? seed : (-seed) % 2000;

                string hairTags = isOffspringFemale ? mother.CharacterObject.HairTags : father.CharacterObject.HairTags;
                string tattooTags = isOffspringFemale
                    ? mother.CharacterObject.TattooTags
                    : father.CharacterObject.TattooTags;
                
                __result.CharacterObject.StaticBodyPropertiesMin =
                    BodyProperties.GetRandomBodyProperties(isOffspringFemale, bodyPropertiesMinMother,
                            bodyPropertiesMinFather, 1, seed, hairTags, father.CharacterObject.BeardTags, tattooTags)
                        .StaticProperties;
            }
        }
    }
}