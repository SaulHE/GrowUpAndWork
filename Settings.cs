using System.Xml.Serialization;
using TaleWorlds.Library;
using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;

namespace GrowUpAndWork
{
    public class SettingClass : AttributeSettings<SettingClass>
    {
        //Please change the ID when you update the game.
        public const string InstanceID = "kleinersilver.GrowUpAndWork_v1_1_2";

        [XmlElement] public override string Id { get; set; } = InstanceID;

        public override string ModuleFolderName { get; } = "zGrowUpAndWork";

        public override string ModName { get; } = "Grow Up And Work";

        public static string CurrentLanguage = "en";

        public static string LogFileName { get; set; } = BasePath.Name + "\\Modules\\zGrowUpAndWork" + "\\" + "log.txt";

        // Tested
        [SettingProperty(displayName: "{=sNMtk43F}Child Becoming Hero Age", minValue: 6, maxValue: 18,
            hintText: "{=ExJ1wdfd}The age at which kids of Calradia become a playable hero. Recommend setting to a value greater than 14. Otherwise sometimes NPCs will spawn as small kids")]
        [SettingPropertyGroup("{=Ff59cH4f}Aging System Tweak")]
        public int BecomeHeroAge { get; set; } = 14;

        // Could be working , but when mc died, if you are waiting in a city or castle, it could crash
        // The up crash is fixed by me
        [SettingProperty(displayName: "{=GHqMmr55}Max Age", minValue: 50, maxValue: 120,
            hintText:
            "{=Z4e9stqr}Set the maximum age a character in Calradia could possibly live. Everyone in Calradia will die after reaching this age")]
        [SettingPropertyGroup("{=Ff59cH4f}Aging System Tweak")]
        public int MaxAge { get; set; } = 80;

        // Tested
        [SettingProperty(displayName: "{=ISDCqOXS}How many days your child grows at", minValue: 2, maxValue: 84,
            hintText: "{=1stHpPnB}How many days your children need to grow one year older. In Native 1 year is 84 days")]
        [SettingPropertyGroup("{=Ff59cH4f}Aging System Tweak")]
        public int ChildrenGrowthCycle { get; set; } = 25;

        [SettingProperty(displayName: "{=rzeuOFBk}Enable NPC Children Faster Growth",
            hintText: "{=y5fvV0vl}If enabled, NPC's children will grow at the same rate as your children")]
        [SettingPropertyGroup("{=Ff59cH4f}Aging System Tweak")]
        public bool NPCChildrenGrowthBoostEnabled { get; set; } = false;


        [SettingProperty(displayName: "{=QEHZzYvW}Growth Boost Stop Age", minValue: 14, maxValue: 40,
            hintText:
            "{=M4cE1DxI}The age after which your children will stop growing any faster. After this age, your children return to normal growth rate(1 year older per year)")]
        [SettingPropertyGroup("{=Ff59cH4f}Aging System Tweak")]
        public int GrowthStopAge { get; set; } = 18;


        //Tested
        [SettingProperty(displayName: "{=FHdF0H2k}Duration of Pregnancy(Days)", minValue: 10, maxValue: 80,
            hintText:
            "{=c5V1OXKn}The number of days from a woman's conception to the birth of child in all Calradia . Native value is 36")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public int PregnancyDurationInDays { get; set; } = 36;

        // Tested

        [SettingProperty(displayName: "{=n2sy7FxF}Disable Maternal Mortality(All Character)",
            hintText: "{=IMb7ESZb}If enabled, all female characters in Calradia will never die of dystocia. In native female characters might die when giving birth to a child")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public bool DisableMaternalMortality { get; set; } = false;

        //Tested
        [SettingProperty(displayName: "{=cYZ8Vsfu}Still Birth Probability", minValue: 0.0f, maxValue: 1.0f, requireRestart: true,
            hintText:
            "{=5vUseYr9}The probability of all kids in Calradia dying of stillbirth. Native value is 0.01. Set to 0 to disable, set to 1.0 to kill every child at birth")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public float StillBirthProbability { get; set; } = 0.01f;


        //Tested
        [SettingProperty(displayName: "{=fOCA7c4O}Max Pregnant Age", minValue: 30, maxValue: 60,
            hintText: "{=SGTv7OWL}The maximum age for all female characters to get pregnant in Calradia. Native Value is 45")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public int MaxPregnantAge { get; set; } = 45;

        [SettingProperty(displayName: "{=IA36Xmzq}Min Pregnant Age", minValue: 12, maxValue: 22,
            hintText: "{=219pBKJa}The minimum age for all female characters to get pregnant in Calradia. Native Value is 18")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public int MinPregnantAge { get; set; } = 18;

        //Tested
        [SettingProperty(displayName: "{=HTI4MLDx}Main Hero's Clan Pregnancy Probability", minValue: 0.0f, maxValue: 1.0f,
            hintText: "{=XTumVh4r}The pregnancy probability of heros in the main character's clan. Setting to 0 means your clan members will no longer have any more children. Native value is close to 1.0")]
        [SettingPropertyGroup("{=t7P6z2aQ}Pregnancy Setting")]
        public float DailyPregnancyChanceOfTheMC { get; set; } = 0.95f;
    }
}