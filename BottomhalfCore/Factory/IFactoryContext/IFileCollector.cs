using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface IFileCollector
    {
        //void ReadCoreProjectFile();
        //void ReadProjectFile();
        List<string> GetClasses(DirectoryInfo directory);
        void DiscoverPath();
        ConfigDetail LoadConfigurationDetail();
        string GetConfigValue(string Key);
    }

    public class ExcludeFile
    {
        public string Name { set; get; }
    }
}
