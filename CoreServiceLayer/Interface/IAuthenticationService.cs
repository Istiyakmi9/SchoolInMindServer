using CommonModal.Models;
using System.Data;

namespace ServiceLayer.Interface
{
    public interface IAuthenticationService<T>
    {
        DataSet GetUserObject(AuthUser objAuthUser);
        (DataSet, string) GetLoginUserObject(AuthUser objAuthUser);
        bool ValidateMobileNo(string Mobile, string TenentId, bool IsStudent);
        void LogoutUserService();
    }
}
