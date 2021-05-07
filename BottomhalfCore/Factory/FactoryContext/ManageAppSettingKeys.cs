using BottomhalfCore.IFactoryContext;
using System.Collections.Generic;

namespace BottomhalfCore.FactoryContext
{
    public class ManageAppSettingKeys : IManageAppSettingKeys<ManageAppSettingKeys>
    {
        //private Configuration config;
        public void AddKey(IDictionary<string, string> KeyValuePair)
        {
            //config = WebConfigurationManager.OpenWebConfiguration("~");
            //AppSettingsSection settingsSection = (AppSettingsSection)config.GetSection("appSettings");
            //foreach (var ConfigObject in KeyValuePair)
            //{
            //    settingsSection.Settings.Remove(ConfigObject.Key);
            //    settingsSection.Settings.Add(ConfigObject.Key, ConfigObject.Value);
            //    config.Save();
            //}
        }
    }
}
