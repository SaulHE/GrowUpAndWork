using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GrowUpAndWork.Behaviour;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using Helpers;
using GrowUpAndWorkLib;
using GrowUpAndWorkLib.Debugging;
using ModLib;
using ModLib.Debugging;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;
using Module = TaleWorlds.MountAndBlade.Module;

namespace GrowUpAndWork
{
    public class SubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "GrowUpAndWork";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                FileDatabase.Initialise(ModuleFolderName);
                
                SettingClass SettingInstance = FileDatabase.Get<SettingClass>(SettingClass.InstanceID);
                
                if (SettingInstance == null) SettingInstance = new SettingClass();
                
                SettingsDatabase.RegisterSettings(SettingInstance);
                SettingsDatabase.SaveSettings(SettingInstance);
                
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