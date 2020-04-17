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
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.AgingCampaignBehavior), "DailyTick")]
    public class AgingCampaignBehaviorDailyTickPatch
    {
        static void Postfix()
        {
            // ModDebug.ShowError($"One day has passed", "Harmony PostFix text", null);
            if (Hero.MainHero.Children.Count != 0)
            {
                int cycleCeiling = SettingClass.Instance.ChildrenGrowthCycle;
                int growthStopAge = SettingClass.Instance.GrowthStopAge;

                if ((int) (Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow % cycleCeiling) == 0)
                {
                    Hero.MainHero.Children.ForEach((Hero child) =>
                    {
                        ModDebug.WriteLog($"Handling Your child {child.Name}");
                        ModDebug.WriteLog($"Your child is {child.Age} years old before");
                        ModDebug.WriteLog($"Your child's father is {child.Father.Name}");
                        ModDebug.WriteLog($"Your child's mother is {child.Mother.Name}");

                        if (child.Age < growthStopAge)
                        {
                            child.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) child.Age + 1);
                            ModDebug.WriteLog($"Now your child: {child.Name} is {child.Age} years old");
                            InformationManager.AddQuickInformation(
                                new TextObject($"Now your child: {child.Name} is {(int) child.Age} years old"), 0,
                                null, "event:/ui/notification/quest_update");
                            if ((int) child.Age == Campaign.Current.Models.AgeModel.HeroComesOfAge)
                            {
                                ModDebug.WriteLog($"Before inheritance");
                                ModDebug.WriteLog($"{child.Name}'s skill is level, now is {child.Level}");

                                InheritHelper.Inherit(child);

                                ModDebug.WriteLog($"after inheritance");

                                ModDebug.WriteLog(
                                    $"Your child {child.Name} has now become a hero and is ready to fight for his clan!");
                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        $"Your child {child.Name} has become a hero and is ready to fight for his clan!"),
                                    0, null, "event:/ui/notification/quest_finished");
                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        $"Your child inherits from its parents and become capable in many fields"),
                                    0, null, "event:/ui/notification/quest_finished");

                                if (child.Mother != null)
                                {
                                    child.Mother.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) child.Mother.Age + 1);
                                }

                                if (child.Father != null)
                                {
                                    child.Father.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) child.Father.Age + 1);
                                }

                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        "You and your spouse are 1 year older due to the growth of your children"), 0,
                                    null, "event:/ui/notification/quest_update");
                                ModDebug.WriteLog(
                                    "You and your spouse are 1 year older due to the growth of your children");


                                foreach (Hero sibling in child.Siblings)
                                {
                                    if (sibling.Age > child.Age)
                                    {
                                        sibling.BirthDay =
                                            HeroHelper.GetRandomBirthDayForAge((int) sibling.Age + 1);
                                    }
                                }


                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        $"{child.Name}'s older siblings are 1 year older due to the growth of {child.Name}"),
                                    0, null, "event:/ui/notification/quest_update");
                                ModDebug.WriteLog(
                                    $"${child.Name}'s older siblings are 1 year older due to the growth of {child.Name}");
                                /*
                                ModDebug.WriteLog("starting add the negative skillXpProgress back");
                                foreach (SkillObject skillIterator in DefaultSkills.GetAllSkills())
                                {
                                    int thisSkillXp = child.HeroDeveloper.GetSkillXpProgress(skillIterator);
                                    ModDebug.WriteLog(
                                        $"Your child {child.Name}'s {skillIterator.Name} has {thisSkillXp} xp",
                                        "Children skills Xp before updating");
                                    if (thisSkillXp < 0)
                                    {
                                        ModDebug.WriteLog("enter the adding part", "Debugger");
                                        child.HeroDeveloper.AddSkillXp(skillIterator, thisSkillXp * -1 + 1, false,
                                            false);
                                    }
                
                                    ModDebug.WriteLog(
                                        $"After Adding, {child.Name}'s {skillIterator.Name} has {thisSkillXp} xp",
                                        "Children skills Xp updating current showing");
                                }
                            */
                            }

                            if (child.Age > 18)
                            {
                                if (child.Mother != null)
                                {
                                    child.Mother.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) child.Mother.Age + 1);
                                }

                                if (child.Father != null)
                                {
                                    child.Father.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) child.Father.Age + 1);
                                }

                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        "You and your spouse are 1 year older due to the growth of your children"), 0,
                                    null, "event:/ui/notification/quest_update");
                                ModDebug.WriteLog(
                                    "You and your spouse are 1 year older due to the growth of your children");


                                foreach (Hero sibling in child.Siblings)
                                {
                                    if (sibling.Age > child.Age)
                                    {
                                        sibling.BirthDay =
                                            HeroHelper.GetRandomBirthDayForAge((int) sibling.Age + 1);
                                    }
                                }


                                InformationManager.AddQuickInformation(
                                    new TextObject(
                                        $"{child.Name}'s older siblings are 1 year older due to the growth of {child.Name}"),
                                    0, null, "event:/ui/notification/quest_update");
                                ModDebug.WriteLog(
                                    $"${child.Name}'s older siblings are 1 year older due to the growth of {child.Name}");
                            }
                        }
                    });
                }
            }
        }

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