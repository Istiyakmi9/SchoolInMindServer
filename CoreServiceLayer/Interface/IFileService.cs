using CommonModal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ServiceLayer.Interface
{
    public interface IFileService<T>
    {
        List<Files> SaveFile(string FolderPath, List<Files> fileDetail, IFormFileCollection formFiles, string ProfileUid);
    }
}
