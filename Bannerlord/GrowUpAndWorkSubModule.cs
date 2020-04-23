#define DEBUG
using System;
using CommunityPatch;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using GrowUpAndWorkLib.Debugging;
using MBOptionScreen.Settings;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;


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
                CommunityPatchSubModule.Print("abc");
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
                GrowthDebug.LogInfo("Campaign Game Started");
            }
            catch (Exception e)
            {
                GrowthDebug.ShowError($"An error occured whilst game starting initializing the GrowUpAndWork",
                    "Game Starting GrowUpAndWork Error", e);
            }
        }
    }
}