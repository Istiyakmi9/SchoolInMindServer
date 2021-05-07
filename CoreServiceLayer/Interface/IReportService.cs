using CommonModal.Models;
using System.Data;

namespace ServiceLayer.Interface
{
    public interface IReportService<T> : IServiceKeyIdentifier
    {
        string StudentReportService(SearchModal searchModal);
        string GuardianReportService(SearchModal searchModal);
        string StaffReportService(SearchModal searchModal);
        string FacultyReportService(SearchModal searchModal);
        DataSet ParentDetailByMobileService(string StudentUid);
    }
}
