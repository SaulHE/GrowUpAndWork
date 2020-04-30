using System;
using GrowUpAndWorkLib.Debugging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace GrowUpAndWork.GrowthClasses
{
    public class InheritHelper
    {
        public static void FixCappedKids()
        {
            Hero.MainHero.Children.ForEach((kid) =>
            {
                if (!kid.IsChild && kid.Age < 30)
                {
                    bool ShouldFixChildrenFlag = false;
                    int CappedSkillCounter = 0;
                    int SkillTotal = 0;

                    foreach (var skillObject in DefaultSkills.GetAllSkills())
                    {
                        SkillTotal += kid.GetSkillValue(skillObject);
                        if (kid.HeroDeveloper.GetSkillXpProgress(skillObject) < 0)
                        {
                            CappedSkillCounter++;
                        }

                        if (CappedSkillCounter > 5)
                        {
                            ShouldFixChildrenFlag = true;
                        }
                    }

                    if (SkillTotal <= 10 && kid.Level > 5)
                    {
                        ShouldFixChildrenFlag = true;
                    }

                    if (kid.HeroDeveloper.GetTotalSkillPoints() < 80 && kid.Level >= 5)
                    {
                        ShouldFixChildrenFlag = true;
                    }


                    int attrAccumulator = 0;
                    foreach (var VARIABLE in  CharacterAttributes.All)
                    {
                        attrAccumulator += kid.GetAttributeValue(VARIABLE.AttributeEnum); 
                    }

                    attrAccumulator += kid.HeroDeveloper.UnspentAttributePoints;
                    GrowthDebug.LogInfo($"Kids{kid.Name} attribute sum: {attrAccumulator}");
                    
                    if (attrAccumulator < 9 && kid.Level > 8)
                    {
                        ShouldFixChildrenFlag = true;
                    }

                    if (attrAccumulator >= 10 || kid.Level == 0)
                    {
                        ShouldFixChildrenFlag = false;
                    }

                    if (ShouldFixChildrenFlag)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(
                            SettingClass.CurrentLanguage == "zh"
                                ? $"检测到你的孩子{kid.Name}属性异常, 已经修复"
                                : $"Detected Your Child {kid.Name}'s stats are abnormal, already fixed", Colors.Magenta));
                        GrowthDebug.LogInfo($"Detected Your Child{kid.Name}'s stats are abnormal, already fixed", "Fixed");
                        Inherit(kid);
                    }
                }
            });
        }

        public static void Inherit(Hero targetInheriter)
        {
            if (targetInheriter == null)
            {
                return;
            }

            try
            {
                GrowthDebug.LogInfo($"Enter inherit method, handling inherit of {targetInheriter}");

                targetInheriter.ClearSkills();
                targetInheriter.HeroDeveloper.ClearHeroLevel();
                int fatherInheritDivider = 0;
                int motherInheritDivider = 0;

                if (targetInheriter.IsFemale == true)
                {
                    fatherInheritDivider = 10;
                    motherInheritDivider = 5;
                }
                else
                {
                    fatherInheritDivider = 5;
                    motherInheritDivider = 10;
                }

                foreach (SkillObject skillIT in DefaultSkills.GetAllSkills())
                {
                    Hero InheritFather = targetInheriter.Father != null ? targetInheriter.Father : targetInheriter;
                    Hero InheritMother = targetInheriter.Mother != null ? targetInheriter.Mother : targetInheriter;
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,
                        InheritFather.GetSkillValue(skillIT) / fatherInheritDivider +
                        InheritMother.GetSkillValue(skillIT) / motherInheritDivider, false);
                }

                targetInheriter.Level = 0;

                targetInheriter.HeroDeveloper.UnspentFocusPoints = 10;
                targetInheriter.HeroDeveloper.UnspentAttributePoints = 10;
            }
            catch (Exception e)
            {
                GrowthDebug.ShowError($"Error during inheritance", "Error During Inheritance", e);
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