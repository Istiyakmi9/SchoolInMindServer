using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.Exceptions;
using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BottomhalfCore.FactoryContext
{
    public class FileCollector : IFileCollector
    {
        private readonly IContainer container;
        public FileCollector()
        {
            container = Container.GetInstance();
        }

        //public void ReadProjectFile()
        //{
        //    string ProjectName = string.Empty;
        //    DiscoverPath();
        //    string RootPath = container.GetProjectPath();
        //    XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
        //    string CurrentProjectName = Directory.GetFiles(RootPath, "*.csproj").FirstOrDefault(); // AppDomain.CurrentDomain.FriendlyName;
        //    if (!string.IsNullOrEmpty(CurrentProjectName))
        //    {
        //        AddProjectName(CurrentProjectName);
        //        container.AddProjectName(CurrentProjectName.Substring(CurrentProjectName.LastIndexOf(@"\") + 1, (CurrentProjectName.IndexOf(".csproj") - CurrentProjectName.LastIndexOf(@"\")) - 1));
        //        XDocument projDefinition = XDocument.Load(Path.Combine(CurrentProjectName));
        //        var ProjectRefItems = projDefinition.Descendants(msbuild + "ProjectReference");
        //        if (ProjectRefItems != null && ProjectRefItems.Count() > 0)
        //        {
        //            foreach (var projectRef in ProjectRefItems)
        //            {
        //                ProjectName = string.Empty;
        //                ProjectName = projectRef.Attribute("Include").Value;
        //                if (!string.IsNullOrEmpty(ProjectName))
        //                    AddProjectName(ProjectName);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        BeanException ObjBeanException = new BeanException();
        //        ObjBeanException.LocationTrack(this.GetType().FullName + "ResolverClassType()");
        //        ObjBeanException.SetMessage("Not able to find current project name");
        //        throw ObjBeanException;
        //    }
        //}

        //private void AddProjectName(string ProjectName)
        //{
        //    if (!string.IsNullOrEmpty(ProjectName))
        //        container.AddProjectName(ProjectName.Substring(ProjectName.LastIndexOf(@"\") + 1, (ProjectName.IndexOf(".csproj") - ProjectName.LastIndexOf(@"\")) - 1));
        //}

        //public void ReadCoreProjectFile()
        //{
        //    string ProjectName = string.Empty;
        //    string RootPath = container.GetProjectPath();
        //    XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
        //    string CurrentProjectName = AppDomain.CurrentDomain.FriendlyName;
        //    if (!string.IsNullOrEmpty(CurrentProjectName))
        //    {
        //        container.AddProjectName(CurrentProjectName);
        //        XDocument projDefinition = XDocument.Load(Path.Combine(RootPath, CurrentProjectName + ".csproj"));
        //        var ItemGrouElements = projDefinition.Root.Elements("ItemGroup");
        //        if (ItemGrouElements != null && ItemGrouElements.Count() > 0)
        //        {
        //            foreach (var elem in ItemGrouElements)
        //            {
        //                var ProjectRefItems = elem.Elements("ProjectReference");
        //                if (ProjectRefItems != null && ProjectRefItems.Count() > 0)
        //                {
        //                    foreach (var projectRef in ProjectRefItems)
        //                    {
        //                        ProjectName = string.Empty;
        //                        ProjectName = projectRef.Attribute("Include").Value;
        //                        if (!string.IsNullOrEmpty(ProjectName))
        //                            AddProjectName(ProjectName);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        BeanException ObjBeanException = new BeanException();
        //        ObjBeanException.LocationTrack(this.GetType().FullName + "ResolverClassType()");
        //        ObjBeanException.SetMessage("Not able to find current project name");
        //        throw ObjBeanException;
        //    }
        //}
        public List<string> GetClasses(DirectoryInfo directory)
        {
            List<string> Files = null;
            FileInfo[] files = null;
            DirectoryInfo[] directories = directory.GetDirectories();
            Files = new List<string>();
            if (directories.Count() > 0)
            {
                foreach (DirectoryInfo dir in directories)
                {
                    if (ExcludedFolder.Where(x => x.Name.ToLower() == dir.Name.ToLower()).FirstOrDefault() == null)
                    {
                        if (File.GetAttributes(dir.FullName) == FileAttributes.Directory)
                        {
                            var FolderFiles = GetClasses(dir);
                            Files.AddRange(FolderFiles);
                        }
                    }
                }

                files = directory.GetFiles("*.cs");
                if (files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        if (!file.GetType().IsInterface)
                            Files.Add(file.Name);
                    }
                }
            }
            else
            {
                files = directory.GetFiles("*.cs");
                foreach (var file in files)
                {
                    if (!file.GetType().IsInterface)
                        Files.Add(file.Name);
                }
            }

            return Files;
        }

        #region EXCLUDED FOLDER NAME
        public IList<ExcludeFile> ExcludedFolder = new List<ExcludeFile> {
            new ExcludeFile() { Name = "bin" },
            new ExcludeFile() { Name = "obj" },
            new ExcludeFile() { Name = "properties" },
            new ExcludeFile() { Name = "packages" },
            new ExcludeFile() { Name = "Bottomhalf" },
            new ExcludeFile() { Name = "config" },
            new ExcludeFile() { Name = ".vs" },
            new ExcludeFile() { Name = "Properties" },
            new ExcludeFile() { Name = "packages" },
            new ExcludeFile() { Name = "App_Start" },
            new ExcludeFile() { Name = "HelpPage" },
            new ExcludeFile() { Name = "v14" }
        };
        #endregion

        public string GetConfigValue(string Key)
        {
            string Value = "";// ConfigurationManager.AppSettings.Get(Key);
            return Value;
        }

        public void DiscoverPath()
        {
            string ProjectDirctory = string.Empty;
            try
            {
                string CurrentBinDirectory = null;
                if (AppDomain.CurrentDomain.BaseDirectory != null)
                {
                    CurrentBinDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    if (CurrentBinDirectory.IndexOf("bin") != -1)
                    {
                        var DirectoryPart = Regex.Split(CurrentBinDirectory, "bin");
                        if (DirectoryPart.Count() == 2)
                        {
                            ProjectDirctory = DirectoryPart[0];
                            CurrentBinDirectory = Path.Combine(ProjectDirctory, "bin");
                        }
                        else
                        {
                            BeanException ObjBeanException = new BeanException();
                            ObjBeanException.LocationTrack(this.GetType().FullName + "DiscoverPath()");
                            ObjBeanException.SetMessage("Not able to find current bin directory");
                            throw ObjBeanException;
                        }
                    }
                    else if (Directory.Exists(Path.Combine(CurrentBinDirectory, "bin")))
                    {
                        ProjectDirctory = CurrentBinDirectory;
                        CurrentBinDirectory = Path.Combine(CurrentBinDirectory, "bin");
                    }
                }
                else
                {
                    CurrentBinDirectory = this.GetType().Assembly.Location;
                    ProjectDirctory = CurrentBinDirectory.Replace("\\Bottomhalf.dll", "");
                    ProjectDirctory = CurrentBinDirectory;
                }
                container.SetProjectPath(ProjectDirctory, CurrentBinDirectory);
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "DiscoverPath()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "DiscoverPath()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        public ConfigDetail LoadConfigurationDetail()
        {
            try
            {
                ConfigDetail ObjConfigDetail = new ConfigDetail();
                string Key = null;
                string CSName = "";
                //int CSCounter = ConfigurationManager.ConnectionStrings.Count;
                //if (CSCounter > 0)
                //{
                //    ObjConfigDetail.ConnectionStringCollection = new ConcurrentDictionary<string, string>();
                //    while (CSCounter != 0)
                //    {
                //        CSName = ConfigurationManager.ConnectionStrings[CSCounter - 1].ConnectionString;
                //        Key = ConfigurationManager.ConnectionStrings[CSCounter - 1].Name;
                //        if (!string.IsNullOrEmpty(CSName) && Key != "LocalSqlServer")
                //            ObjConfigDetail.ConnectionStringCollection.Add(Key, CSName);
                //        CSCounter--;
                //    }
                //}

                //string[] KeyArrays = ConfigurationManager.AppSettings.AllKeys;
                //if (KeyArrays.Count() > 0)
                //{
                //    ObjConfigDetail.AppSettingCollection = new ConcurrentDictionary<string, string>();
                //    foreach (string key in KeyArrays)
                //        ObjConfigDetail.AppSettingCollection.Add(key, ConfigurationManager.AppSettings[key]);
                //}
                return ObjConfigDetail;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "LoadConfigurationDetail()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "LoadConfigurationDetail()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}
