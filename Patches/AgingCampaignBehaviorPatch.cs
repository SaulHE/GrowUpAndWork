using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GrowUpAndWork;
using GrowUpAndWork.GrowthClasses;
using GrowUpAndWork.Patches;
using HarmonyLib;
using Helpers;
using GrowUpAndWorkLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;
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
                if (hero != null && !hero.IsDead && hero.IsNoble)
                {
                    if (hero.Age >= SettingClass.Instance.MaxAge)
                    {
                        KillCharacterAction.ApplyByOldAge(hero, true);
                        counter += 1;
                    }
                }
            }

            return counter;
        }

        public static int GrowTargetHeroWithoutSideEffect(Hero hero)
        {
            if (hero == null)
            {
                return -1;
            }

            int currentAge = (int) hero.Age;
            if ((int) hero.Age < SettingClass.Instance.GrowthStopAge)
            {
                hero.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) hero.Age + 1);
            }

            currentAge = (int) hero.Age;

            return currentAge;
        }

        public static int GrowTargetHero(Hero hero)
        {
            if (hero == null)
            {
                return -1;
            }

            int currentAge = (int) hero.Age;
            bool notified = false;

            if ((int) hero.Age > SettingClass.Instance.BecomeHeroAge)
            {
                foreach (var heroSibling in hero.Siblings)
                {
                    if (heroSibling.Age >= currentAge)
                    {
                        GrowSibling(heroSibling);

                        GrowthDebug.LogInfo(
                            $"${hero.Name}'s older siblings are 1 year older due to the growth of {hero.Name}");

                        if (heroSibling.Clan == Clan.PlayerClan && !notified)
                        {
                            InformationManager.AddQuickInformation(
                                SettingClass.CurrentLanguage == "zh"
                                    ? new TextObject($"因为{hero.Name}的成长，他的哥哥姐姐都长大了一岁")
                                    : new TextObject(
                                        $"{hero.Name}'s older siblings are 1 year older due to the growth of {hero.Name}"),
                                0, null, "event:/ui/notification/quest_update");
                            notified = true;
                        }
                    }
                }
            }

            hero.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) hero.Age + 1);

            if (hero.Father == Hero.MainHero || hero.Mother == Hero.MainHero)
            {
                InformationManager.AddQuickInformation(
                    SettingClass.CurrentLanguage == "zh"
                        ? new TextObject($"你的孩子:{hero.Name} 现在已经{(int) hero.Age}岁了")
                        : new TextObject($"Now your child: {hero.Name} is {(int) hero.Age} years old"), 0,
                    null, "event:/ui/notification/quest_update");

                
                if (hero.Age >= SettingClass.Instance.BecomeHeroAge && hero.Age <= SettingClass.Instance.GrowthStopAge)
                {
                    // Only Main Heros get the notification
                    InformationManager.AddQuickInformation(
                        SettingClass.CurrentLanguage == "zh"
                            ? new TextObject($"因为你的孩子的成长，你和你的配偶都老了一岁")
                            : new TextObject(
                                "You and your spouse are 1 year older due to the growth of your children"),
                        0, null, "event:/ui/notification/quest_update");
                }
            }

            if ((int) hero.Age >= SettingClass.Instance.BecomeHeroAge && (int)hero.Age <= SettingClass.Instance.GrowthStopAge)
            {
                if (hero.Mother != null)
                {
                    GrowTargetHero(hero.Mother);
                }

                if (hero.Father != null)
                {
                    GrowTargetHero(hero.Father);
                }
            }

            if ((int) hero.Age == SettingClass.Instance.BecomeHeroAge)
            {
                InheritHelper.Inherit(hero);
                if (hero.Clan == Clan.PlayerClan)
                {
                    GrowthDebug.LogInfo(
                        $"Your child {hero.Name} has now become a hero and is ready to fight for his clan!",
                        "Grow Target");

                    InformationManager.AddQuickInformation(
                        SettingClass.CurrentLanguage == "zh"
                            ? new TextObject($"你的孩子{hero.Name}已经成年, 成为了一个可以为家族而战的英雄")
                            : new TextObject(
                                $"Your child {hero.Name} has become a hero and is ready to fight for his clan!"),
                        0, null, "event:/ui/notification/quest_finished");

                    InformationManager.AddQuickInformation(
                        SettingClass.CurrentLanguage == "zh"
                            ? new TextObject($"你的孩子{hero.Name}从父母那里继承了部分能力, 在许多方面都突出常人")
                            : new TextObject(
                                $"Your child{hero.Name} inherits from its parents and become capable in many fields"),
                        0, null, "event:/ui/notification/quest_finished");
                    
                }
            }

            currentAge = (int) hero.Age;
            return currentAge;
        }

        public static int GrowSibling(Hero hero)
        {
            int currentAge = (int) hero.Age;

            hero.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) hero.Age + 1);

            if ((SettingClass.Instance.NPCChildrenGrowthBoostEnabled || hero.Clan == Clan.PlayerClan) &&
                (int) hero.Age == SettingClass.Instance.BecomeHeroAge)
            {
                if (hero.Clan == Clan.PlayerClan)
                {
                    GrowthDebug.LogInfo(
                        $"Your child {hero.Name} has now become a hero and is ready to fight for his clan!",
                        "Grow Sibling");

                    InformationManager.AddQuickInformation(
                        SettingClass.CurrentLanguage == "zh"
                            ? new TextObject($"你的孩子{hero.Name}已经成年, 成为了一个可以为家族而战的英雄")
                            : new TextObject(
                                $"Your child {hero.Name} has become a hero and is ready to fight for his clan!"),
                        0, null, "event:/ui/notification/quest_finished");

                    InformationManager.AddQuickInformation(
                        SettingClass.CurrentLanguage == "zh"
                            ? new TextObject($"你的孩子{hero.Name}从父母那里继承了部分能力, 在许多方面都突出常人")
                            : new TextObject(
                                $"Your child{hero.Name} inherits from its parents and become capable in many fields"),
                        0, null, "event:/ui/notification/quest_finished");
                }

                InheritHelper.Inherit(hero);
            }

            currentAge = (int) hero.Age;
            return currentAge;
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
            // if setting enabled, grow npc children
            if (SettingClass.Instance.NPCChildrenGrowthBoostEnabled)
            {
                int numberOfNobleChildrenGrow = 0;
                int numberOfNotableChildrenGrow = 0;

                if ((int) (Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow %
                           SettingClass.Instance.ChildrenGrowthCycle) == 0)
                {
                    Hero.All.ToList<Hero>().ForEach((heroEle) =>
                    {
                        if (heroEle.IsNoble && heroEle.Clan != Clan.PlayerClan)
                        {
                            if ((int) heroEle.Age < SettingClass.Instance.GrowthStopAge)
                            {
                                AgingSystemHelper.GrowTargetHero(heroEle);
                                numberOfNobleChildrenGrow += 1;
                            }
                        }

                        if (heroEle.IsNotable || heroEle.IsRuralNotable)
                        {
                            if ((int) heroEle.Age < SettingClass.Instance.GrowthStopAge)
                            {
                                GrowthDebug.LogInfo($"Handling Notable {heroEle.Name}", "Handling, notable");
                                AgingSystemHelper.GrowTargetHeroWithoutSideEffect(heroEle);
                                numberOfNotableChildrenGrow += 1;
                            }
                        }
                    });
                }

                if (numberOfNobleChildrenGrow > 0)
                {
                    InformationManager.DisplayMessage(SettingClass.CurrentLanguage == "zh"
                        ? new InformationMessage($"今天卡拉迪亚大陆有{numberOfNobleChildrenGrow}个贵族孩子又长大了一岁", Colors.Cyan)
                        : new InformationMessage(
                            $"Today, {numberOfNobleChildrenGrow} noble children grow up faster in the world of Calradia",
                            Colors.Cyan));
                    GrowthDebug.LogInfo($"Today, {numberOfNobleChildrenGrow} noble children grow faster in Calradia");
                }

                if (numberOfNotableChildrenGrow > 0)
                {
                    InformationManager.DisplayMessage(SettingClass.CurrentLanguage == "zh"
                        ? new InformationMessage($"今天卡拉迪亚大陆有{numberOfNotableChildrenGrow}个要人孩子又长大了一岁", Colors.Cyan)
                        : new InformationMessage(
                            $"Today, {numberOfNotableChildrenGrow} notable children grow up faster in the world of Calradia",
                            Colors.Cyan));
                    GrowthDebug.LogInfo(
                        $"Today, {numberOfNotableChildrenGrow} notable children grow faster in Calradia");
                }
            }

            if (Hero.MainHero.Children.Count != 0)
            {
                int cycleCeiling = SettingClass.Instance.ChildrenGrowthCycle;

                if ((int) (Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow % cycleCeiling) == 0)
                {
                    Hero.MainHero.Children.ForEach((Hero child) =>
                    {
                        GrowthDebug.LogInfo($"Handling Your child {child.Name}");
                        GrowthDebug.LogInfo($"Your child is {child.Age} years old before");
                        GrowthDebug.LogInfo("Entering ForEach");

                        if (child.Age < SettingClass.Instance.GrowthStopAge)
                        {
                            AgingSystemHelper.GrowTargetHero(child);
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