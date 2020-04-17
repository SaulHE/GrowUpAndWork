using System.Xml.Serialization;
using ModLib;
using ModLib.Attributes;
using TaleWorlds.Library;

namespace GrowUpAndWork
{
    public class SettingClass: SettingsBase
    {
        public const string InstanceID = "GrowUpAndWorkModSettingID";

        public static SettingClass Instance
        {
            get
            {
                return (SettingClass) SettingsDatabase.GetSettings(InstanceID);
            }
            
        }

        [XmlElement]
        public override string ID { get; set; } = InstanceID;
        public override string ModuleFolderName { get; } = "GrowUpAndWork";
        public override string ModName { get; } = "GrowUpAndWork";

        [XmlElement]
        [SettingProperty("Enable Crash Error Reporting", "When enabled, shows a message box showing the cause of a crash.")]
        [SettingPropertyGroup("Debugging")]
        public bool IsDebugMode { get; set; } = true;

        
        [XmlElement]
        public string LogFileName { get; set; } = BasePath.Name + "Modules/GrowUpAndWork" + "/" + "log.txt";
        
        
        [XmlElement]
        [SettingProperty("Becoming Hero Age", 6, 18, "The age at which a child becomes a playable hero. Recommend setting to a value greater than 14, otherwise sometimes NPCs will spawn as children")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int BecomeHeroAge { get; set; } = 14;

        
        [XmlElement]
        [SettingProperty("Max Age", 50, 120, "Set the max age a person could possibly get. Everyone will die when reaching this age")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int MaxAge { get; set; } = 80;

        [XmlElement] 
        [SettingProperty("Children Aging Cycle in Days", 2, 84, "How many days your children need to grow one year older. In Native 1 year is 84 days")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int ChildrenGrowthCycle { get; set; } = 25;

        [XmlElement]
        [SettingProperty("Enable NPC Children Faster Growth", "If enabled, NPC's children will grow at the same rate as your children")]
        [SettingPropertyGroup("Aging System Tweak")]
        public bool NPCChildrenGrowthBoostEnabled { get; set; } = false;


        [XmlElement]
        [SettingProperty("Disable Maternal Mortality", "If enabled, your wife will not die of dystocia. In native your wife might die when giving birth to a child")]
        [SettingPropertyGroup("Aging System Tweak")]
        public bool DisableMaternalMortality { get; set; } = false;


        [XmlElement]
        [SettingProperty("Growth Boost Stop Age", 14, 30,"The age after which your children will stop growing any faster. After this age, you child return to normal growth(1 year older per year)")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int GrowthStopAge { get; set; } = 18;
    }
}