using System;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;

namespace GrowUpAndWork.Patches
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloperVM),
              "OnCharacterSelection")]
        public class OnCharacterSelectionPatch
        {
            static void Postfix()
            {
                if (SettingClass.Instance.EnableAutoFixBrokenKids)
                {
                    InheritHelper.FixCappedKids();
                }
            }
        }
}