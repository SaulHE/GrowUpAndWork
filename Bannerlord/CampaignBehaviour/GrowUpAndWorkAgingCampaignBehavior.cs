using System;
using System.Linq;
using GrowUpAndWorkLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace GrowUpAndWork
{
    public class GrowUpAndWorkAgingCampaignBehavior : CampaignBehaviorBase
    {
        private static GrowUpAndWorkAgingCampaignBehavior _instance;
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
            GrowthDebug.LogInfo("Run RegisterEvents", "Tracing RegisterEvents");
        }

        public override void SyncData(IDataStore dataStore)
        {
            GrowthDebug.LogInfo("Entering SyncData", "Tracking SyncData");
        }

        public static GrowUpAndWorkAgingCampaignBehavior Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GrowUpAndWorkAgingCampaignBehavior();
                }
                return _instance;
            }
            private set => _instance = value;
        }

        private void DailyTick()
        {
            try
            {
                // if setting enabled, grow npc children
                if (SettingClass.Instance.NPCChildrenGrowthBoostEnabled)
                {
                    int numberOfNobleChildrenGrow = 0;
                    int numberOfNotableChildrenGrow = 0;

                    // Grow NPC children
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
                        GrowthDebug.LogInfo(
                            $"Today, {numberOfNobleChildrenGrow} noble children grow faster in Calradia");
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

                // HeroStringIdManager.LogAllStringIdofManager();
                // Grow Main Hero's children
                if (Hero.MainHero.Children.Count != 0)
                {
                    int cycleCeiling = SettingClass.Instance.ChildrenGrowthCycle;

                    if ((int) (Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow % cycleCeiling) == 0)
                    {
                        Hero.MainHero.Children.ForEach((Hero child) =>
                        {
                            /*
                             * Change character's id after creation will bring a lot of problems, the encycpedia's link won't link to the original ones.
                             */

                            // if (Utils.MBStringIdToInt(child.StringId) <= npcTempMaxStringId)
                            // {
                            //     AgingSystemHelper.SetHeroStringId(child, AgingSystemHelper.GetMaxStringIdNext(HeroRangeEnum.MainHeroClanOnly));
                            // }

                            if (child.Age < SettingClass.Instance.GrowthStopAge)
                            {
                                AgingSystemHelper.GrowTargetHero(child);
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                GrowthDebug.LogError("Encountering Error in Daily Tick","Error running in DailyTick",e);
            }
        }
    }
}