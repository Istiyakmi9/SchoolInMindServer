using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class LogInformationToFile
    {
        private readonly string CurrentBinDirectory;
        private FileStream fs = null;
        public LogInformationToFile(string CurrentBinDirectory)
        {
            this.CurrentBinDirectory = CurrentBinDirectory;
        }
        public void WriteToFile(Dictionary<string, TypeRefCollection> ClassTypeCollection)
        {
            try
            {
                string LoggerFilePath = Path.GetFullPath(Path.Combine(this.CurrentBinDirectory)) + "/logger.txt";
                if (!File.Exists(LoggerFilePath))
                    fs = new FileStream(LoggerFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                if (ClassTypeCollection != null)
                {
                    using (StreamWriter writer = new StreamWriter(LoggerFilePath, false))
                    {
                        foreach (KeyValuePair<string, TypeRefCollection> refCollection in ClassTypeCollection)
                            writer.WriteLine(refCollection.Value.ClassName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public void WriteArrayToFile(List<string> FileArray, string FileName)
        {
            try
            {
                string LoggerFilePath = Path.GetFullPath(Path.Combine(this.CurrentBinDirectory)) + "/" + FileName + ".txt";
                if (!File.Exists(LoggerFilePath))
                    fs = new FileStream(LoggerFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                if (FileArray != null)
                {
                    using (StreamWriter writer = new StreamWriter(LoggerFilePath, true))
                    {
                        foreach (var file in FileArray)
                            writer.WriteLine(file);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        //private void GiveFileFullAccess(string FilePath)
        //{
        //    DirectoryInfo directoryInfo = new DirectoryInfo(FilePath);
        //    DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
        //    directorySecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
        //        FileSystemRights.FullControl,
        //        InheritanceFlags.ObjectInherit |
        //        InheritanceFlags.ContainerInherit,
        //        PropagationFlags.NoPropagateInherit,
        //        AccessControlType.Allow));
        //    directoryInfo.SetAccessControl(directorySecurity);
        //}
    }
}
