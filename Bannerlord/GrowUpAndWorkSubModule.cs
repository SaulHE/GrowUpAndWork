using System;
using Fasterflect;
using GrowUpAndWork.Data;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using GrowUpAndWorkLib.Debugging;
using MBOptionScreen.Settings;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;


namespace GrowUpAndWork
{
    public class SubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "zGrowUpAndWork";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {

                Harmony harmony = new Harmony("mod.growupandwork.kleinersilver");
                harmony.PatchAll();

                // GrowthDebug.LogInfo("Mod Loaded");
                // GrowthDebug.LogInfo($"Current Language: {BannerlordConfig.Language}");
                // CommunityPatchSubModule.Print("abc");
                Module.CurrentModule.GlobalTextManager.LoadGameTexts($"{BasePath.Name}/Modules/{ModuleFolderName}/ModuleData/module_strings.xml");
                
                
            }
            catch (Exception e)
            {
                GrowthDebug.ShowError($"An error occured whilst initializing the GrowUpAndWork",
                    "Error during initialization", e);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (gameStarterObject == null)
            {
                return;
            }

            try
            {
                base.OnGameStart(game, gameStarterObject);
                if (!(game.GameType is Campaign))
                    return;
                CampaignGameStarter gameInitializer = (CampaignGameStarter) gameStarterObject;
                gameInitializer.AddBehavior(GrowUpAndWorkAgingCampaignBehavior.Instance);
            }
            catch (Exception e)
            {
                GrowthDebug.ShowError($"An error occured whilst game starting initializing the GrowUpAndWork",
                    "Game Starting GrowUpAndWork Error", e);
            }
        }
        //The following code fix the wrong encyclopedia link for good.
        //Move the fix to a dependent

        /*
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            
            HeroStringIdManager.SyncMBCharacterStringIdToHeroStringIdManager();
            HeroStringIdManager.LogAllStringIdofManager();
            
            MBObjectManager.Instance.TrySetFieldValue("_lastGeneratedId",HeroStringIdManager.GenerateNonDuplicateStringIdNum());
            GrowthDebug.LogInfo($"the last generatedId is {MBObjectManager.Instance.TryGetFieldValue("_lastGeneratedId")}");
        }
    */
    }
}