using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class SettingManager
    {
        public enum SettingNames { ActiveIndex };

        public Setting GetSetting(SettingNames settingName)
        {
            SettingDataAccess sda = new SettingDataAccess();
            return sda.GetSetting(settingName.ToString());
        }

        public bool AddUpdateSetting(SettingNames settingName, string value)
        {
            SettingDataAccess sda = new SettingDataAccess();
            Setting setting = sda.GetSetting(settingName.ToString());
            if (setting == null)
            {
                setting = new Setting();
                setting.SettingName = settingName.ToString();
                setting.SettingValue = value;
                return sda.InsertSetting(setting) > 0;
            }
            else
            {
                setting.SettingValue = value;
                return sda.UpdateSetting(setting);
            }
        }
    }
}
