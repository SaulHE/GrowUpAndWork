using System.Collections.Generic;
using System.Linq;
 using GrowUpAndWorkLib.GUI.ViewModels;

 namespace GrowUpAndWorkLib
{
    public static class ICollectionExtensions
    {
        public static SettingPropertyGroup GetGroup(this ICollection<SettingPropertyGroup> groupsList, string groupName)
        {
            return groupsList.Where((x) => x.GroupName == groupName).FirstOrDefault();
        }
    }
}
