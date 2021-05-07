using CommonModal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IRegistrationService<T> : IServiceKeyIdentifier
    {
        int VerifyMobibleExist(string Mobile, string Email);
        string SubmitStudentForm(StudentRegistrationModel objRegistrationFormData, List<Files> fileDetail, IFormFileCollection files, string ProcedureName);
        string RegisterStaffFaculty(RegistrationFormData objRegistrationFormData, List<Files> fileDetail, IFormFileCollection FileCollection);
        string DriverRegistrationService(StaffRegistrationForm registrationFormData, List<Files> fileDetail, IFormFileCollection files);
        string GetParentInfoService(string Mobile);
        DataTable ListClassesService();
        DataTable GetStudentListService(string RegistrationNo, bool CompleteReg);
        string StudentDetailByUidService(string StudentUid);
        string OtherStaffMemberByUidService(string StaffUid);
        string StaffMemberByUidService(string StaffUid);
        string SignUpService(SignUpModel signUpModel);
        string QuickStudentRegistrationService(QuickRegistrationModal quickRegistrationModal, List<Files> fileDetail, IFormFileCollection files);
        string StudentRegistrationService(StudentRegistrationModel registrationFormData, List<Files> fileDetail, IFormFileCollection files);
        string DeleteImageService(string ImageUid);
        string UpdateParentDetailService(ParentDetails ParentDetails);
    }
}
