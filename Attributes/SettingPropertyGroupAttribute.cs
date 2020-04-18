﻿using System;
 using GrowUpAndWorkLib.GUI.ViewModels;

 namespace GrowUpAndWorkLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyGroupAttribute : Attribute
    {
        public string GroupName { get; private set; }
        public bool IsMainToggle { get; set; }

        public SettingPropertyGroupAttribute(string groupName, bool isMainToggle = false)
        {
            GroupName = groupName;
            IsMainToggle = isMainToggle;
        }

        public static SettingPropertyGroupAttribute Default => new SettingPropertyGroupAttribute(SettingPropertyGroup.DefaultGroupName, false);
    }
}
