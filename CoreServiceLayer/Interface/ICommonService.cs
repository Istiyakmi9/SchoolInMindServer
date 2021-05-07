using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ICommonService<T>
    {
        IDictionary<string, List<string>> ValidateMobileAndEmail(string MobileNos, string EmailIds);
        IList<string> GetAllFileNames(string DirectoryPath);
        string GetSubjectByClassSectionService(int Class, string Section);
        void ValidateFilterModal(SearchModal searchModal, string SortBy);
    }
}
