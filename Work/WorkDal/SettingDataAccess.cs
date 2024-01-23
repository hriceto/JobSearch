using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class SettingDataAccess: DataAccess
    {
        private const string cacheKey = "Setting_";

        public Setting GetSetting(string settingName)
        {
            return GetSetting(settingName, false);
        }

        public Setting GetSetting(string settingName, bool getFromDb)
        {
            if (HttpContext.Current.Cache[cacheKey + settingName] != null && !getFromDb)
            {
                return (Setting)HttpContext.Current.Cache[cacheKey + settingName];
            }
            else
            {
                lock (cacheKey + settingName)
                {
                    if (HttpContext.Current.Cache[cacheKey + settingName] != null && !getFromDb)
                    {
                        return (Setting)HttpContext.Current.Cache[cacheKey + settingName];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            var settingQuery = from s in context.Settings
                                               where s.SettingName == settingName
                                               select s;
                            var setting = settingQuery.FirstOrDefault();
                            if (setting != null)
                            {
                                HttpContext.Current.Cache.Insert(cacheKey + settingName, setting, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            }
                            return setting;
                        }
                    }
                }
            }
        }

        public int InsertSetting(Setting setting)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                context.Settings.AddObject(setting);
                if (context.SaveChanges() >= 1)
                {
                    result = setting.SettingId;
                    //remove from cache
                    ResetSettingCache(setting.SettingName);
                }
            }
            return result;
        }

        public bool UpdateSetting(Setting setting)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                context.Settings.Attach(setting);
                context.ObjectStateManager.ChangeObjectState(setting, System.Data.EntityState.Modified);
                result = (context.SaveChanges() == 1);
                //remove from cache
                ResetSettingCache(setting.SettingName);
            }

            return result;
        }

        private void ResetSettingCache(string settingName)
        {
            if (HttpContext.Current.Cache[cacheKey + settingName] != null)
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey + settingName] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey + settingName);
                    }
                }
            }
        }
    }
}
