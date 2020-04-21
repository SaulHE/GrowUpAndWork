﻿using System;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using GrowUpAndWorkLib.Debugging;
using ModLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;


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
                FileDatabase.Initialise(ModuleFolderName);

                SettingsDatabase.RegisterSettings(
                    ((SettingsBase) (FileDatabase.Get<SettingClass>(SettingClass.InstanceID) ?? new SettingClass())));

                Harmony harmony = new Harmony("mod.growupandwork.kleinersilver");
                harmony.PatchAll();

                GrowthDebug.LogInfo("Mod Loaded");
                GrowthDebug.LogInfo($"Current Language: {BannerlordConfig.Language}");
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