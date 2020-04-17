using System.Xml.Serialization;
using GrowUpAndWork.FileDatabase;
using TaleWorlds.Library;

namespace GrowUpAndWork
{
    public class Settings: SettingsBase
    {
        public const bool IsDebugMode = true;
        public const int BecomeHeroAge = 14;
        public static string logFileName = BasePath.Name + "\\Modules\\GrowUpAndWork" + "\\" + "log.txt";
        public const string InstanceID = "GrowUpAndWorkModSettingID";

        [XmlElement]
        public override string ID { get; set; } = InstanceID;
        public override string ModuleFolderName { get; } = "GrowUpAndWork";
        public override string ModName { get; } = "GrowUpAndWork";
    }
}