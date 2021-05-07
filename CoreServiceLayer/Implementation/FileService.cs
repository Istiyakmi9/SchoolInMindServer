using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace CoreServiceLayer.Implementation
{
    public class FileService : IFileService<FileService>
    {
        private readonly BeanContext beanContext;
        public FileService()
        {
            this.beanContext = BeanContext.GetInstance();
        }
        public List<Files> SaveFile(string FolderPath, List<Files> fileDetail, IFormFileCollection formFiles, string ProfileUid)
        {
            string Extension = "";
            string NewFileName = string.Empty;
            if (!string.IsNullOrEmpty(FolderPath))
            {
                string ActualFolderPath = Path.Combine(this.beanContext.GetContentRootPath(), FolderPath);
                foreach (var file in formFiles)
                {
                    if (!string.IsNullOrEmpty(file.Name))
                    {
                        if (!Directory.Exists(ActualFolderPath))
                            Directory.CreateDirectory(ActualFolderPath);

                        Extension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1, file.FileName.Length - file.FileName.LastIndexOf('.') - 1);
                        NewFileName = file.Name + "." + Extension;

                        var currentFile = fileDetail.Where(x => x.FileUid == file.Name).FirstOrDefault();

                        if (currentFile != null)
                        {
                            currentFile.FileExtension = Extension;
                            currentFile.ProfileUid = ProfileUid;
                            currentFile.FilePath = FolderPath;

                            string ActualPath = Path.Combine(ActualFolderPath, NewFileName);
                            if (File.Exists(ActualPath))
                                File.Delete(ActualPath);
                            using (FileStream fs = System.IO.File.Create(ActualPath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                }
            }
            return fileDetail;
        }
    }
}
