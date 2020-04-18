using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.SaveSystem;
using GrowUpAndWork.GrowthClasses;
using GrowUpAndWorkLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace GrowUpAndWork.Behaviour
{
    public class GrowUpAndWorkCampaignBehaviour : CampaignBehaviorBase
    {
        private GrowthData _growthData;

        public static GrowUpAndWorkCampaignBehaviour MyInstance
        {
            get { return Campaign.Current.GetCampaignBehavior<GrowUpAndWorkCampaignBehaviour>(); }
        }

        public override void RegisterEvents()
        {
            this._growthData = new GrowthData();
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this,
                new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        public void PrintData()
        {
            GrowthDebug.LogInfo($"The cycleCount is {this._growthData.CycleCount}");
            GrowthDebug.LogInfo($"The beenRunBefore boolean is {this._growthData.BeenRunBefore}");
        }

        public int IncreaseCount()
        {
            String logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";
            int res = this._growthData.increaseCycleCount(); 
            return res;

        }

        public int initCycleCount()
        {
            this._growthData.setCycleCount(0);
            return this._growthData.CycleCount;
        }


        private void OnSessionLaunched(CampaignGameStarter cgs)
        {
            String logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";

            this._growthData.BeenRunBefore = true;
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("GrowUpAndWork", ref this._growthData);
        }

        public GrowthData GrowthDataOfThisGame
        {
            get { return this._growthData; }
        }

        public static readonly GrowUpAndWorkCampaignBehaviour Instance = new GrowUpAndWorkCampaignBehaviour();

        public class MySaveDefiner : SaveableTypeDefiner
        {
            public MySaveDefiner() : base(GrowthData.GrowthDataID)
            {
            }

            protected override void DefineClassTypes()
            {
                base.AddClassDefinition(typeof(GrowthData), GrowthData.GrowthDataID);
            }
        }
    }
}