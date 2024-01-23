using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkSearch
{
    public class SettingActiveIndex
    {
        public JobSearch.pathToIndex GetPathToActiveIndex()
        {
            SettingManager settingManager = new SettingManager();
            Setting setting = settingManager.GetSetting(SettingManager.SettingNames.ActiveIndex);

            JobSearch.pathToIndex path = JobSearch.pathToIndex.SearchIndex1;
            if (setting != null)
            {
                Enum.TryParse(setting.SettingValue, out path);
            }

            return path;
        }

        public bool UpdatePathToActiveIndex(JobSearch.pathToIndex path)
        {
            SettingManager settingManager = new SettingManager();
            return settingManager.AddUpdateSetting(SettingManager.SettingNames.ActiveIndex, path.ToString());
        }

        public JobSearch.pathToIndex InvertPath(JobSearch.pathToIndex path)
        {
            if (path == JobSearch.pathToIndex.SearchIndex1)
            {
                path = JobSearch.pathToIndex.SearchIndex2;
            }
            else
            {
                path = JobSearch.pathToIndex.SearchIndex1;
            }

            return path;
        }
    }
}
