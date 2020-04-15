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
            targetInheriter.ClearSkills();
            if (targetInheriter.IsFemale)
            {
                foreach (SkillObject skillIT in DefaultSkills.GetAllSkills())
                {
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,  targetInheriter.Mother.GetSkillValue(skillIT) / 3);
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,  targetInheriter.Father.GetSkillValue(skillIT) / 5);
                }
                
            }
            if (targetInheriter.IsFemale == false)
            {
                foreach (SkillObject skillIT in DefaultSkills.GetAllSkills())
                {
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,  targetInheriter.Mother.GetSkillValue(skillIT) / 5);
                    targetInheriter.HeroDeveloper.ChangeSkillLevel(skillIT,   targetInheriter.Father.GetSkillValue(skillIT) / 3);
                }
                
            }
            
        }
    }
    public class NormalAgeModel : DefaultAgeModel
    {
        public override int BecomeInfantAge
        {
            get { return 1; }
        }

        public override int BecomeChildAge
        {
            get { return 6; }
        }

        public override int BecomeTeenagerAge
        {
            get { return 10; }
        }

        public override int HeroComesOfAge {
            get { return 13; }
        }

        public override int BecomeOldAge
        {
            get { return 50; }
        }

        public override int MaxAge
        {
            get { return 75; }
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