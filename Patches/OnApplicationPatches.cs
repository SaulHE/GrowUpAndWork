using System;
using GrowUpAndWorkLib.Debugging;
using HarmonyLib;

namespace GrowUpAndWork.Patches
{
    
       
       [HarmonyPatch(typeof(TaleWorlds.Engine.TestContext), "OnApplicationTick")]
       public class OnApplicationTickPatchOfTestContext
       {
           static void Finalizer(Exception __exception)
           {
               if (__exception != null)
               {
                   GrowthDebug.ShowError($"Mount and Blade Bannerlord has encountered an error and needs to close. See the error information below.",
                         "Mount and Blade Bannerlord has crashed", __exception);
               }
           }
       } 
}