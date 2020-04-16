using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GrowUpAndWork.LightLogger;
using GrowUpAndWork.Behaviour;
using GrowUpAndWork.GrowthClasses;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace GrowUpAndWork
{
    public class SubModule : MBSubModuleBase
    {
        private String logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";
        private Log _log;

        protected override void OnSubModuleLoad()
        {
            // Module.CurrentModule.AddInitialStateOption(new InitialStateOption("TestMainMenuOption", new TextObject("Click Me!", null), 9990,
            //     () =>
            //     {
            //         InformationManager.DisplayMessage(new InformationMessage("Hello World!"));
            //     }, false));
            this._log = new Log(logFileName);
            _log.WriteLog("1");
            try
            {
                _log.WriteLog("SubModuleLoaded");
            }
            catch (Exception e)
            {
                _log.WriteLog($"ERROR!!!==========>:{e.ToString()}");
                throw e;
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            try
            {
                if (!(game.GameType is Campaign))
                    return;
                CampaignGameStarter gameInitializer = (CampaignGameStarter) gameStarterObject;

                // initiallize the SaveCampaignBehaviour for the cycle count every 40 days.
                GrowUpAndWorkCampaignBehaviour gInstance = GrowUpAndWorkCampaignBehaviour.Instance;
                gameInitializer.AddBehavior(gInstance);
                gameStarterObject.AddModel(new NormalAgeModel());

                _log.WriteLog("Campaign Game Started");
                Campaign.Current.DailyTickEvent.AddHandler(ChildrenGrowthBoostEventHandlers
                    .DailyChildrenGrowthTickHandler);
            }
            catch (Exception e)
            {
                _log.WriteLog($"{e.ToString()}");
                throw e;
            }
        }
    }

    public static class ChildrenGrowthBoostEventHandlers
    {
        private static Log _log;
        private static String _logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";

        public static void DailyChildrenGrowthTickHandler(MBCampaignEvent campaignevent, object[] delegateparams)
        {
            _log = new Log(_logFileName);

            try
            {
                /*
                InformationManager.DisplayMessage(new InformationMessage("Enter Daily Children Growth Tick Handler",
                    Colors.Red));
                */
                _log.WriteLog("Enter Daily Children Growth TickHandler");
                if (Hero.MainHero.Children.IsEmpty())
                {
                    _log.WriteLog("Your hero doesn't have children so no cycle counter");
                    GrowUpAndWorkCampaignBehaviour.MyInstance.initCycleCount();
                    return;
                }
                else
                {
                    _log.WriteLog("Yes your children is going to grow quickly");
                    // Hero.MainHero.Children.ForEach((Hero child) => { childrenNames += child.Name + ","; });

                    // grow children under 13 every 40 days
                    GrowUpAndWorkCampaignBehaviour growUpAndWorkCampaignBehaviour =
                        GrowUpAndWorkCampaignBehaviour.MyInstance;
                    int incResult = growUpAndWorkCampaignBehaviour.IncreaseCount();
                    growUpAndWorkCampaignBehaviour.PrintData();
                    int cycleCeiling = 0;

                    if (Settings.IsDebugMode)
                    {
                        cycleCeiling = 2;
                    }
                    else
                    {
                        cycleCeiling = 25;
                    }

                    //grow up 1 year up each cycle
                    if (incResult >= cycleCeiling)
                    {
                        growUpAndWorkCampaignBehaviour.initCycleCount();
                        Hero.MainHero.Children.ForEach((Hero child) =>
                        {
                            _log.WriteLog($"Handling Your child {child.Name}");
                            _log.WriteLog($"Your child is {child.Age} years old before");
                            _log.WriteLog($"Your child's father is {child.Father.Name}");
                            _log.WriteLog($"Your child's mother is {child.Mother.Name}");
                            if (child.Age < 18)
                            {
                                child.BirthDay = HeroHelper.GetRandomBirthDayForAge((int) child.Age + 1);
                                _log.WriteLog($"Now your child: {child.Name} is {child.Age} years old");
                                InformationManager.AddQuickInformation(
                                    new TextObject($"Now your child: {child.Name} is {(int) child.Age} years old"), 0,
                                    null, "event:/ui/notification/quest_update");

                                if ((int) child.Age == Settings.BecomeHeroAge && child.IsChild == false)
                                {
                                    _log.WriteLog($"Before inheritance");
                                    child.ClearSkills();
                                    child.HeroDeveloper.ClearHeroLevel();
                                    _log.WriteLog($"{child.Name}'s skill is level, now is {child.Level}");
                                    InheritHelper.Inherit(child);
                                    _log.WriteLog($"after inheritance");

                                    _log.WriteLog(
                                        $"Your child {child.Name} has now become a hero and is ready to fight for his clan!");
                                    InformationManager.AddQuickInformation(
                                        new TextObject(
                                            $"Your child {child.Name} has become a hero and is ready to fight for his clan!"),
                                        0, null, "event:/ui/notification/quest_finished");
                                    InformationManager.AddQuickInformation(
                                        new TextObject(
                                            $"Your child inherits from his parents and become capable in many fields"),
                                        0, null, "event:/ui/notification/quest_finished");
                                    Hero.MainHero.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) Hero.MainHero.Age + 1);
                                    if (child.Mother != null)
                                    {
                                        child.Mother.BirthDay =
                                            HeroHelper.GetRandomBirthDayForAge((int) child.Mother.Age + 1);
                                    }

                                    InformationManager.AddQuickInformation(
                                        new TextObject("You are 1 year older due to the growth of your children"), 0,
                                        null, "event:/ui/notification/quest_update");
                                    _log.WriteLog("You and your wife are 1 year older due to the growth of your children");


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
                                            $"{child.Name} older siblings are 1 year older due to the growth of their sibling"),
                                        0, null, "event:/ui/notification/quest_update");
                                    _log.WriteLog(
                                        $"${child.Name}'s older siblings are 1 year older due to the growth of their sibling");
                                    child.HeroDeveloper.UnspentFocusPoints += 15;
                                    child.HeroDeveloper.UnspentAttributePoints += 20;
                                    _log.WriteLog("starting add the negative skillXpProgress back");
                                    foreach (SkillObject skillIterator in DefaultSkills.GetAllSkills())
                                    {
                                        int thisSkillXp = child.HeroDeveloper.GetSkillXpProgress(skillIterator);
                                        _log.WriteLog(
                                            $"Your child {child.Name}'s {skillIterator.Name} has {thisSkillXp} xp");
                                        if (thisSkillXp < 0)
                                        {
                                            _log.WriteLog("enter the adding part");
                                            child.HeroDeveloper.AddSkillXp(skillIterator, thisSkillXp * -1 + 1, false,
                                                false);
                                        }

                                        _log.WriteLog(
                                            $"After Adding, {child.Name}'s {skillIterator.Name} has {thisSkillXp} xp");
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                // InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Colors.Red));
                _log.WriteLog("Error++++" + e.ToString());
                throw e;
            }
        }
    }
}