using System;
using System.Linq;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using Helpers;
using GrowUpAndWorkLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace GrowUpAndWork.Patches
{
    public class AgingSystemHelper
    {
        public static int KillOverAgedHero()
        {
            int counter = 0;
            foreach (var hero in Hero.All.ToList<Hero>())
            {
                if (hero != null && !hero.IsDead hero )
                {
                    if (hero.Age >= SettingClass.Instance.MaxAge)
                    {
                        KillCharacterAction.ApplyByOldAge(hero, true);
                        KillCharacterAction.ApplyInternal(victim, (Hero) null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge, true);
                        counter += 1;
                    }
                }
            }

            return counter;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.AgingCampaignBehavior), "DailyTick")]
    public class AgingCampaignBehaviorDailyTickPatch
    {
        static bool Prefix()
        {
            int howMany = AgingSystemHelper.KillOverAgedHero();
            if (howMany > 0)
            {
                InformationManager.AddQuickInformation(
                    SettingClass.CurrentLanguage == "zh"
                        ? new TextObject($"很不幸, 今天有{howMany}位卡拉迪亚的战士因为衰老和疾病永远离开了我们。")
                        : new TextObject($"Unfortunately, {howMany} Hero(s) in Calradia died because of old age today"),
                    0, null, "event:/ui/notification/death");
            }

            return true;
        }

        static void Postfix()
        {
            if (Hero.MainHero.Children.Count != 0)
            {
                int cycleCeiling = SettingClass.Instance.ChildrenGrowthCycle;
                int growthStopAge = SettingClass.Instance.GrowthStopAge;

                if ((int) (Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow % cycleCeiling) == 0)
                {
                    Hero.MainHero.Children.ForEach((Hero child) =>
                    {
                        GrowthDebug.LogInfo($"Handling Your child {child.Name}");
                        GrowthDebug.LogInfo($"Your child is {child.Age} years old before");
                        GrowthDebug.LogInfo($"Your child's father is {child.Father.Name}");
                        GrowthDebug.LogInfo($"Your child's mother is {child.Mother.Name}");

                        if (child.Age < growthStopAge)
                        {
                            child.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) child.Age + 1);
                            GrowthDebug.LogInfo($"Now your child: {child.Name} is {child.Age} years old");
                            InformationManager.AddQuickInformation(
                                SettingClass.CurrentLanguage == "zh"
                                    ? new TextObject($"你的孩子:{child.Name} 现在已经{(int) child.Age}岁了")
                                    : new TextObject($"Now your child: {child.Name} is {(int) child.Age} years old"), 0,
                                null, "event:/ui/notification/quest_update");

                            if ((int) child.Age == Campaign.Current.Models.AgeModel.HeroComesOfAge)
                            {
                                GrowthDebug.LogInfo($"Before inheritance");
                                GrowthDebug.LogInfo($"{child.Name}'s skill is level, now is {child.Level}");

                                InheritHelper.Inherit(child);

                                GrowthDebug.LogInfo($"after inheritance");

                                GrowthDebug.LogInfo(
                                    $"Your child {child.Name} has now become a hero and is ready to fight for his clan!");
                                InformationManager.AddQuickInformation(
                                    SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject($"你的孩子{child.Name}已经成年, 成为了一个可以为家族而战的英雄")
                                        : new TextObject(
                                            $"Your child {child.Name} has become a hero and is ready to fight for his clan!"),
                                    0, null, "event:/ui/notification/quest_finished");

                                InformationManager.AddQuickInformation(
                                    SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject($"你的孩子{child.Name}从父母那里继承了部分能力, 在许多方面都突出常人")
                                        : new TextObject(
                                            $"Your child{child.Name} inherits from its parents and become capable in many fields"),
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
                                    SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject($"因为你的孩子的成长，你和你的配偶都老了一岁")
                                        : new TextObject(
                                            "You and your spouse are 1 year older due to the growth of your children"),
                                    0, null, "event:/ui/notification/quest_update");

                                GrowthDebug.LogInfo(
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
                                    SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject($"因为{child.Name}的成长，他的哥哥姐姐都长大了一岁")
                                        : new TextObject(
                                            $"{child.Name}'s older siblings are 1 year older due to the growth of {child.Name}"),
                                    0, null, "event:/ui/notification/quest_update");

                                GrowthDebug.LogInfo(
                                    $"${child.Name}'s older siblings are 1 year older due to the growth of {child.Name}");
                                /*
                                GrowthDebug.WriteLog("starting add the negative skillXpProgress back");
                                foreach (SkillObject skillIterator in DefaultSkills.GetAllSkills())
                                {
                                    int thisSkillXp = child.HeroDeveloper.GetSkillXpProgress(skillIterator);
                                    GrowthDebug.WriteLog(
                                        $"Your child {child.Name}'s {skillIterator.Name} has {thisSkillXp} xp",
                                        "Children skills Xp before updating");
                                    if (thisSkillXp < 0)
                                    {
                                        GrowthDebug.WriteLog("enter the adding part", "Debugger");
                                        child.HeroDeveloper.AddSkillXp(skillIterator, thisSkillXp * -1 + 1, false,
                                            false);
                                    }
                
                                    GrowthDebug.WriteLog(
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

                                InformationManager.AddQuickInformation(SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject("因为你的孩子的成长，你和你的配偶都老了一岁")
                                        : new TextObject(
                                            "You and your spouse are 1 year older due to the growth of your children"),
                                    0, null, "event:/ui/notification/quest_update");

                                GrowthDebug.LogInfo(
                                    "You and your spouse are 1 year older due to the growth of your children");


                                foreach (Hero sibling in child.Siblings)
                                {
                                    if (sibling.Age > child.Age)
                                    {
                                        sibling.BirthDay =
                                            HeroHelper.GetRandomBirthDayForAge((int) sibling.Age + 1);
                                    }
                                }


                                InformationManager.AddQuickInformation(SettingClass.CurrentLanguage == "zh"
                                        ? new TextObject($"因为{child.Name}的成长，他的哥哥姐姐都长大了一岁")
                                        : new TextObject(
                                            $"{child.Name}'s older siblings are 1 year older due to the rapid growth of {child.Name}"),
                                    0, null, "event:/ui/notification/quest_update");
                                GrowthDebug.LogInfo(
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
                GrowthDebug.ShowError(
                    $"Bannerlord has encounter an error and needs to close. See the error information below.",
                    "Mount and Blade Bannerlord has crashed", __exception);
            }
        }
    }
}