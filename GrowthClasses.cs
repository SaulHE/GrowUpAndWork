using System;
using System.Runtime.CompilerServices;
using GrowUpAndWork.LightLogger;
using ModLib;
using ModLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace GrowUpAndWork.GrowthClasses
{
    public class InheritHelper
    {
        public static void Inherit(Hero targetInheriter)
        {
            try
            {
                ModDebug.WriteLog("Enter inherit method");

                targetInheriter.ClearSkills();
                targetInheriter.HeroDeveloper.ClearHeroLevel();
                int fatherInheritDivider = 0;
                int motherInheritDivider = 0;

                if (targetInheriter.IsFemale == true)
                {
                    fatherInheritDivider = 5;
                    motherInheritDivider = 3;
                }
                else
                {
                    fatherInheritDivider = 3;
                    motherInheritDivider = 5;
                }

                foreach (SkillObject skillIT in DefaultSkills.GetAllSkills())
                {
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,
                        targetInheriter.Father.GetSkillValue(skillIT) / fatherInheritDivider +
                        targetInheriter.Mother.GetSkillValue(skillIT) / motherInheritDivider, false);
                }

                targetInheriter.Level = 0;
                
                targetInheriter.HeroDeveloper.UnspentFocusPoints += 15;
                targetInheriter.HeroDeveloper.UnspentAttributePoints += 20;
                
            }
            catch (Exception e)
            {
                ModDebug.ShowError($"Error during inheritance", "Error During Inheritance", e);
            }
        }
    }


    public class GrowthData
    {
        public static int GrowthDataID = 688835189;
        [SaveableField(1)] public int CycleCount;

        [SaveableField(2)] public bool BeenRunBefore;

        public GrowthData(int CycleCount = 0, bool BeenRunBefore = false)
        {
            this.CycleCount = CycleCount;
            this.BeenRunBefore = false;
        }

        public int setCycleCount(int setValue)
        {
            this.CycleCount = setValue;
            return this.CycleCount;
        }

        public int increaseCycleCount()
        {
            this.SetBeenRunBefore();
            this.CycleCount++;

            return this.CycleCount;
        }

        public bool beenRunBeforeOrNot()
        {
            return this.BeenRunBefore;
        }

        public bool SetBeenRunBefore()
        {
            this.BeenRunBefore = true;
            return this.BeenRunBefore;
        }

        public bool SetNotBeenRunBefore()
        {
            this.BeenRunBefore = false;
            return this.BeenRunBefore;
        }

        public int GetCycleCount()
        {
            return this.CycleCount;
        }
    }
}