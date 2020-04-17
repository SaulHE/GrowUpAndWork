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
        [SettingProperty("Set The Logfile Folder Directory", "Don't mess this property if not needed")]
        [SettingPropertyGroup("Debugging")]
        public string LogFileName { get; set; } = BasePath.Name + "Modules/GrowUpAndWork" + "/" + "log.txt";
        
        
        [XmlElement]
        [SettingProperty("Set the age when a child becomes a playable hero", 6, 18, "Recommend Setting Greater than 14. NPC will spawn into hero at this age as well")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int BecomeHeroAge { get; set; } = 14;

        [XmlElement] 
        [SettingProperty("Children Growth Days Cycle", 2, 84, "How many days your children need to grow one year older")]
        [SettingPropertyGroup("Aging System Tweak")]
        public int ChildrenGrowthCycle { get; set; } = 25;

        [XmlElement]
        [SettingProperty("Enable NPC children faster growth", "If enabled, NPC's children will grow rapidly as well")]
        [SettingPropertyGroup("Aging System Tweak")]
        public bool NPCChildrenGrowthBoostEnabled { get; set; } = false;
    }
}