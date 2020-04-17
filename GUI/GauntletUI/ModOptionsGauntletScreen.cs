using System;
using ModLib.Debugging;
using ModLib.GUI.ViewModels;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace ModLib.GUI.GauntletUI
{
    internal class ModOptionsGauntletScreen : ScreenBase
    {
        private GauntletLayer gauntletLayer;
        private GauntletMovie movie;
        private ModSettingsScreenVM vm;

        protected override void OnInitialize()
        {
            try
            {
                base.OnInitialize();
                SpriteData spriteData = UIResourceManager.SpriteData;
                TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
                ResourceDepot uiresourceDepot = UIResourceManager.UIResourceDepot;
                spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, uiresourceDepot);
                gauntletLayer = new GauntletLayer(1);
                gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                gauntletLayer.Input.RegisterHotKeyCategory(
                    HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
                gauntletLayer.IsFocusLayer = true;
                ScreenManager.TrySetFocus(gauntletLayer);
                AddLayer(gauntletLayer);
                vm = new ModSettingsScreenVM();
                ModDebug.LogError($"vm:  {vm == null}");
                ModDebug.LogError($"gauntletLayer is {gauntletLayer == null}");
                movie = gauntletLayer.LoadMovie("ModOptionsScreen", vm);
            }
            catch (Exception e)
            {
                ModDebug.ShowError("Rendering Error during initialized", "Rendering error occured", e);
            }
        }

        protected override void OnFrameTick(float dt)
        {
            try
            {
                base.OnFrameTick(dt);
                // || gauntletLayer.Input.IsGameKeyReleased(34)
                if (gauntletLayer.Input.IsHotKeyReleased("Exit"))
                {
                    vm.ExecuteCancel();
                }
            }
            catch (Exception e)
            {
                ModDebug.ShowError("On FrameTick Error", "Rendering Error Happens", e);
            }
        }

        protected override void OnFinalize()
        {
            try
            {
                base.OnFinalize();
                RemoveLayer(gauntletLayer);
                gauntletLayer.ReleaseMovie(movie);
                gauntletLayer = null;
                movie = null;
                vm.ExecuteSelect(null);
                vm.AssignParent(true);
                vm = null;
            }
            catch (Exception e)
            {
                ModDebug.ShowError("On Finalize Error", "Rendering Error Happens", e);
            }
        }
    }
}