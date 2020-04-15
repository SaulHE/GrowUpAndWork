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
                InformationManager.DisplayMessage(new InformationMessage(e.ToString()));
                throw;
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (!(game.GameType is Campaign))
                return;
            CampaignGameStarter gameInitializer = (CampaignGameStarter) gameStarterObject;

            // initiallize the SaveCampaignBehaviour for the cycle count every 40 days.
            GrowUpAndWorkCampaignBehaviour gInstance = GrowUpAndWorkCampaignBehaviour.Instance;
            gameInitializer.AddBehavior(gInstance);
            gameStarterObject.AddModel(new NormalAgeModel());

            _log.WriteLog("Campaign Game Started");
            Campaign.Current.DailyTickEvent.AddHandler(ChildrenGrowthBoostEventHandlers.DailyChildrenGrowthTickHandler);
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

                    //grow up 1 year up each cycle
                    if (incResult >= 25)
                    {
                        growUpAndWorkCampaignBehaviour.initCycleCount();
                        Hero.MainHero.Children.ForEach((Hero child) =>
                        {
                            if (child.Age < 18)
                            {
                                child.BirthDay = HeroHelper.GetRandomBirthDayForAge((int)child.Age + 1);
                                _log.WriteLog($"Now your child: {child.Name} is {child.Age} years old");
                                InformationManager.AddQuickInformation(
                                    new TextObject($"Now your child: {child.Name} is {(int) child.Age} years old"), 0,
                                    null, "event:/ui/notification/quest_update");

                                if ((int) child.Age == 13 && child.IsChild == false)
                                {

                                    _log.WriteLog($"Before inheritance");
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
                                    Hero.MainHero.BirthDay = HeroHelper.GetRandomBirthDayForAge((int)Hero.MainHero.Age + 1);
                                    child.Mother.BirthDay =
                                        HeroHelper.GetRandomBirthDayForAge((int) child.Mother.Age + 1);
                                    InformationManager.AddQuickInformation(new TextObject("You are 1 year older due to the growth of your children") , 0, null, "event:/ui/notification/quest_update");
                                    _log.WriteLog("You and your wife are 1 year older due to the growth of your children");


                                    foreach (Hero sibling in child.Siblings )
                                    {
                                        if (sibling.Age > child.Age)
                                        {
                                            sibling.BirthDay =
                                                HeroHelper.GetRandomBirthDayForAge((int) sibling.Age + 1);
                                        }
                                        
                                    }

                                    
                                    InformationManager.AddQuickInformation(new TextObject("His older siblings are 1 year older due to the growth of their sibling"), 0, null, "event:/ui/notification/quest_update");
                                    _log.WriteLog("His older siblings are 1 year older due to the growth of their sibling");
                                    child.Level = 0;
                                    child.HeroDeveloper.UnspentFocusPoints += 15;
                                    child.HeroDeveloper.UnspentAttributePoints += 20;
                                    foreach (SkillObject skillIterator in DefaultSkills.GetAllSkills())
                                    {
                                        int thisSkillXp = child.HeroDeveloper.GetSkillXpProgress(skillIterator);
                                        if (thisSkillXp < 0)
                                        {
                                            child.AddSkillXp(skillIterator, Math.Abs(thisSkillXp));
                                        }
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
                throw new NotImplementedException();
            }
        }
    }
}