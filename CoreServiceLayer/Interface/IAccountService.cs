using CommonModal.Models;
using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IAccountService<T>
    {
        string StaffSalaryService(string SearchString, string SortBy, string PageIndex, string PageSize);
        string StaffSalaryGenericService(string SearchString, string SortBy, string PageIndex, string PageSize, string ProcedureName);
        string SchoolFeesDetailService(string PayeeUid, string ClassDetailUid, int PaymentYear);
        string AddEditFeesService(CommonFeesModal ObjCommonFeesModal);
        string TransactionIdExists(string PayeeUid, string MonthString, int PayeeCode);
        string Generatetxnid();
        StudentParentDetail GetStudentDetailByUid(string StudentUid);
        FeesPaymentDetail FeesPaymentDataService(FeesPaymentDetail ObjSoldItems, StudentParentDetail ObjStudentParentDetail);
        string InsertPaymentInformation(PayUResponse ObjPayUResponse, string MonthNumber, int Year, string PayeeUid, int PayeeCode, int FeeCode, int FineCode);
        string Generatehash512(string text);
        Task<string> GetOTP(string userNumber, string Amount, string TransactionId, string Message);
    }
}
