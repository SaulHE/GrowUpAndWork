#define LAN_ZH
using System.Configuration;
using System.Xml.Serialization;
using GrowUpAndWorkLib;
using ModLib;
using ModLib.Attributes;
using TaleWorlds.Library;
using SettingsBase = ModLib.SettingsBase;

namespace GrowUpAndWork
{
    public class SettingClass : SettingsBase
    {
        public const string InstanceID = "GrowUpAndWorkModSettingID";

        public static SettingClass Instance
        {
            get { return (SettingClass) SettingsDatabase.GetSettings(InstanceID); }
        }

        [XmlElement] public override string ID { get; set; } = InstanceID;
        public override string ModuleFolderName { get; } = "GrowUpAndWork";
        public override string ModName { get; } = "GrowUpAndWork";

        public bool IsDebugMode { get; set; } = true;

        public static string CurrentLanguage = "en";

        public static string LogFileName { get; set; } = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";

        // Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("孩子成为战斗英雄的年龄", 6, 18, "设置卡拉迪亚大陆的孩子在几岁成为一个可以为家族而战的英雄，建议设置的值大于14，不然有些NPC将会以小孩的形式出现")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("Children Becoming Hero Age", 6, 18, "The age at which kids of Calradia become a playable hero. Recommend setting to a value greater than 14. Otherwise sometimes NPCs will spawn as small kids")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public int BecomeHeroAge { get; set; } = 14;
        
        
        [XmlElement]
#if LAN_ZH
        [SettingProperty("所有人物的最大年龄", 50, 120, "注意，卡拉迪亚大陆所有的NPC以及玩家的角色将在达到您设置的最大年龄后死亡")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("Max Age", 50, 120, "Set the maximum age a character in Calradia could possibly live. Everyone in Calradia will die after reaching this age")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public int MaxAge { get; set; } = 80;
        
        // Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("设置主角的孩子多少天长大一岁", 2, 84, "每过您设置的天数，您的孩子都会成长一岁。原版中，一年有84天")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("How many days your child grows at", 2, 84, "How many days your children need to grow one year older. In Native 1 year is 84 days")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public int ChildrenGrowthCycle { get; set; } = 25;

        [XmlElement]
#if LAN_ZH
        [SettingProperty("启用NPC的孩子更快地生长", "启用后，所有NPC的孩子也会和您的孩子一样长得飞快")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("Enable NPC Children Faster Growth", "If enabled, NPC's children will grow at the same rate as your children")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public bool NPCChildrenGrowthBoostEnabled { get; set; } = false;


        [XmlElement]
#if LAN_ZH
        [SettingProperty("设置停止飞速增长的年龄", 14, 35, "当达到设置的年龄后，你的孩子将会返回正常的生长速率(一年涨一岁)")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("Growth Boost Stop Age", 14, 30,"The age after which your children will stop growing any faster. After this age, your children return to normal growth rate(1 year older per year)")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public int GrowthStopAge { get; set; } = 18;


        //Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("设置孕期", 10, 80,
            "一个女性角色怀孕多少天后可以生下孩子, 原版的设置值是36天。最大可以设置到80天")]
        [SettingPropertyGroup("年龄成长系统")]
#else
        [SettingProperty("Duration of Pregnancy(Days)", 10, 80,
            "The number of days from a woman's conception to the birth of child in all Calradia . Native value is 36")]
        [SettingPropertyGroup("Aging System Tweak")]
#endif
        public int PregnancyDurationInDays { get; set; } = 36;

        // Tested

        [XmlElement]
#if LAN_ZH
        [SettingProperty("开启无难产模式", "如果开启，卡拉迪亚的所有女性角色都不会死于难产。原版中女性角色有可能死于难产")]
        [SettingPropertyGroup("怀孕系统设置")]
#else
        [SettingProperty("Disable Maternal Mortality(All Character)", "If enabled, all female characters in Calradia will never die of dystocia. In native female characters might die when giving birth to a child")]
        [SettingPropertyGroup("Pregnancy Setting")]
#endif
        public bool DisableMaternalMortality { get; set; } = false;

        //Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("死胎(小产)概率", 0.0f, 1.0f, "设置卡拉迪亚大陆所有孩子在出生过程中死胎(小产)的概率。设置为0将不会发生死胎，设置为1.0您的孩子会全部死于小产. 原版值为0.01")]
        [SettingPropertyGroup("怀孕系统设置")]
#else
        [SettingProperty("Still Birth Probability", 0.0f, 1.0f,"The probability of all kids in Calradia dying of stillbirth. Native value is 0.01. Set to 0 to disable, set to 1.0 to kill every child at birth")]
        [SettingPropertyGroup("Pregnancy Setting")]
#endif
        public float StillBirthProbability { get; set; } = 0.01f;


        //Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("最大怀孕年龄", 30, 60, "设置游戏中女性角色在多少岁后不再有能力怀孕, 原版中45岁之后女性角色将绝经失去怀孕能力")]
        [SettingPropertyGroup("怀孕系统设置")]
#else
        [SettingProperty("Max Pregnant Age", 30, 60,"The maximum age for all women to get pregnant in Calradia. Native Value is 45")]
        [SettingPropertyGroup("Pregnancy Setting")]
#endif
        public int MaxPregnantAge { get; set; } = 45;

        [XmlElement]
#if LAN_ZH
        [SettingProperty("最小怀孕年龄", 12, 24,
            "设置女性角色至少要达到多少岁才会怀孕，原版中女性要达到18岁才可以怀孕")]
        [SettingPropertyGroup("怀孕系统设置")]
#else
        [SettingProperty("Min Pregnant Age", 12, 22,
            "The minimum age for all female characters to get pregnant in Calradia. Native Value is 18")]
        [SettingPropertyGroup("Pregnancy Setting")]
#endif
        public int MinPregnantAge { get; set; } = 18;
        
        //Tested
        [XmlElement]
#if LAN_ZH
        [SettingProperty("主角家族女性的怀孕几率", 0.0f, 1.0f,
            "设置主角家族中女性的怀孕概率, 设置为0将不会怀孕, 设置为1.0您家族的女性只要有机会就会怀孕. 原版的值为0.95")]
        [SettingPropertyGroup("怀孕系统设置")]
#else
        [SettingProperty("Main Hero's Clan Pregnancy Probability", 0.0f, 1.0f,
            "The pregnancy probability of heros in the main character's clan. Setting to 0 means your clan members will no longer have any more children. Native value is close to 1.0")]
        [SettingPropertyGroup("Pregnancy Setting")]
#endif
        public float DailyPregnancyChanceOfTheMC { get; set; } = 0.95f;
    }
}