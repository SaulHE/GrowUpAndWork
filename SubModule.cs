using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GrowUpAndWork.LightLogger;
using GrowUpAndWork.Behaviour;
using GrowUpAndWork.GrowthClasses;
using HarmonyLib;
using Helpers;
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
using ModLib.GUI.GauntletUI;

namespace GrowUpAndWork
{
    public class SubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "GrowUpAndWork";

        protected override void OnSubModuleLoad()
        {
            /*
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("TestMainMenuOption", new TextObject("Click Me!", null), 9990,
                () =>
                {
                    InformationManager.DisplayMessage(new InformationMessage("Hello World!"));
                }, false));
            */
            try
            {
                FileDatabase.Initialise("GrowUpAndWork");
                SettingClass SettingInstance = FileDatabase.Get<SettingClass>(SettingClass.InstanceID);
                if (SettingInstance == null) SettingInstance = new SettingClass();
                SettingsDatabase.RegisterSettings(SettingInstance);

                SettingsDatabase.SaveSettings(SettingInstance);

                ModDebug.WriteLog("yes");

                
                // add the screen
                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ModOptionsMenu",
                    new TextObject("Mod Options"), 9990,
                    () => { ScreenManager.PushScreen(new ModOptionsGauntletScreen()); }, false));
                
                Harmony harmony = new Harmony("mod.growupandwork.kleinersilver");
                harmony.PatchAll();
                
            }
            catch (Exception e)
            {
                ModDebug.ShowError($"An error occured whilst initializing the GrowUpAndWork",
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
                ModDebug.WriteLog("Campaign Game Started");
            }
            catch (Exception e)
            {
                ModDebug.ShowError($"An error occured whilst game starting initializing the GrowUpAndWork",
                    "Game Starting GrowUpAndWork Error", e);
            }
        }
    }
}