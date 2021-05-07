using CommonModal.ORMModels;
using System.Collections.Generic;
using System.Data;

namespace ServiceLayer.Interface
{
    public interface IExamDataService<T>
    {
        DataSet ExamDetailService(string Class, string ExamDescriptionUid);
        string ExamDetailUpdateService(List<Examdetails> examdetails);
    }
}
