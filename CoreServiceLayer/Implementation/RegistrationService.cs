using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.Enums;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CoreServiceLayer.Implementation
{
    public class RegistrationService : CurrentUserObject, IRegistrationService<RegistrationService>
    {
        private readonly CurrentSession currentSession;
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IFileService<FileService> fileService;
        private readonly IDb db;
        private readonly BeanContext beanContext;

        public RegistrationService(IDb db, UserDetail userDetail, ValidateModalService validateModalService, FileService fileService, CurrentSession currentSession)
        {
            this.db = db;
            this.beanContext = BeanContext.GetInstance();
            this.userDetail = userDetail;
            this.currentSession = currentSession;
            this.fileService = fileService;
            this.validateModalService = validateModalService;
            this.userDetail = currentSession.CurrentUserDetail;
        }

        public string RegisterStaffFaculty(RegistrationFormData registrationFormData, List<Files> fileDetail, IFormFileCollection FileCollection)
        {
            string StaffUid = "";
            string ImagePath = this.beanContext.GetContentRootPath();
            string output = string.Empty;
            DesignationType designationType = DesignationType.Faculty;
            if (string.IsNullOrEmpty(registrationFormData.Email))
                registrationFormData.Email = null;
            if (!string.IsNullOrEmpty(registrationFormData.StaffMemberUid))
                StaffUid = registrationFormData.StaffMemberUid;
            ServiceResult serviceResult = validateModalService.ValidateModalFieldsService(typeof(RegistrationFormData), registrationFormData);
            if (registrationFormData.ApplicationFor == DesignationType.Faculty.ToString().ToLower())
                designationType = DesignationType.Faculty;
            else if (registrationFormData.ApplicationFor == DesignationType.Driver.ToString().ToLower())
                designationType = DesignationType.Driver;
            else
                designationType = DesignationType.Other;
            if (serviceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(StaffUid, typeof(System.String), "_StaffMemberUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentid"),
                    new DbParam(registrationFormData.FirstName, typeof(System.String), "_FirstName"),
                    new DbParam(registrationFormData.LastName, typeof(System.String), "_LastName"),
                    new DbParam(registrationFormData.Gender, typeof(System.Boolean), "_Gender"),
                    new DbParam(registrationFormData.Dob, typeof(System.DateTime), "_Dob"),
                    new DbParam(registrationFormData.MobileNumber, typeof(System.String), "_MobileNumber"),
                    new DbParam(registrationFormData.AlternetNumber, typeof(System.String), "_AlternetNumber"),
                    new DbParam(registrationFormData.Email, typeof(System.String), "_Email"),
                    new DbParam(registrationFormData.Address, typeof(System.String), "_Address"),
                    new DbParam(registrationFormData.State, typeof(System.String), "_State"),
                    new DbParam(registrationFormData.City, typeof(System.String), "_City"),
                    new DbParam(registrationFormData.Pincode, typeof(System.Int64), "_Pincode"),
                    new DbParam(registrationFormData.Subjects, typeof(System.String), "_Subjects"),
                    new DbParam(registrationFormData.Type, typeof(System.String), "_Type"),
                    new DbParam(registrationFormData.SchoolUniversityName, typeof(System.String), "_SchoolUniversityName"),
                    new DbParam(designationType, typeof(System.Int32), "_DesignationId"),
                    new DbParam(registrationFormData.ProofOfDocumentationPath, typeof(System.String), "_ProofOfDocumentationPath"),
                    new DbParam(registrationFormData.DegreeName, typeof(System.String), "_DegreeName"),
                    new DbParam(registrationFormData.Grade, typeof(System.String), "_Grade"),
                    new DbParam(registrationFormData.Position, typeof(System.String), "_Position"),
                    new DbParam(registrationFormData.MarksObtain, typeof(System.Double), "_MarksObtain"),
                    new DbParam(registrationFormData.Title, typeof(System.String), "_Title"),
                    new DbParam(registrationFormData.ExprienceInYear, typeof(System.String), "_ExprienceInYear"),
                    new DbParam(registrationFormData.ExperienceInMonth, typeof(System.String), "_ExperienceInMonth"),
                    new DbParam(registrationFormData.ImageUrl, typeof(System.String), "_ImageUrl"),
                    new DbParam(userDetail.UserId, typeof(System.String), "_AdminId"),
                    new DbParam(registrationFormData.AccessLevelUid, typeof(System.String), "_AccessLevelUid"),
                    new DbParam(registrationFormData.ClassDetailUid, typeof(System.String), "_ClassDetailId")
                };
                output = db.ExecuteNonQuery("sp_RegisterStaffAndFacultyDetails_InsUpd", param, true);
                if (!string.IsNullOrEmpty(output) && output != "fail")
                {
                    if (FileCollection.Count > 0)
                    {
                        string FolderPath = Path.Combine(this.currentSession.FileUploadFolderName,
                            ApplicationConstant.Faculty,
                            output);
                        List<Files> files = this.fileService.SaveFile(FolderPath, fileDetail, FileCollection, output);
                        if (files != null && files.Count > 0)
                        {
                            DataSet fileDs = this.beanContext.ConvertToDataSet<Files>(files);
                            if (fileDs != null && fileDs.Tables.Count > 0 && fileDs.Tables[0].Rows.Count > 0)
                            {
                                DataTable table = fileDs.Tables[0];
                                table.TableName = "Files";
                                db.InsertUpdateBatchRecord("sp_Files_InsUpd", table);
                                output = "Record inserted successfully.";
                            }
                        }
                        else
                        {
                            ///==========  Log information ====================
                            output = "Record updated successfully.";
                        }
                    }
                    else
                    {
                        output = "Record inserted/updated successfully.";
                    }
                }
            }
            else
            {
                output = JsonConvert.SerializeObject(serviceResult.ErrorResultedList);
            }

            return output;
        }

        public string UpdateParentDetailService(ParentDetails ParentDetails)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ParentDetails.ParentDetailId, typeof(System.String), "_ParentDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid"),
                new DbParam(ParentDetails.FatherFirstName, typeof(System.String), "_FatherFirstName"),
                new DbParam(ParentDetails.FatherLastName, typeof(System.String), "_FatherLastName"),
                new DbParam(ParentDetails.FatherMobileno, typeof(System.String), "_FatherMobileno"),
                new DbParam(ParentDetails.Fatheremailid, typeof(System.String), "_Fatheremailid"),
                new DbParam(ParentDetails.Fatheroccupation, typeof(System.String), "_Fatheroccupation"),
                new DbParam(ParentDetails.Fatherqualification, typeof(System.String), "_Fatherqualification"),
                new DbParam(ParentDetails.MotherFirstName, typeof(System.String), "_MotherFirstName"),
                new DbParam(ParentDetails.MotherLastName, typeof(System.String), "_MotherLastName"),
                new DbParam(ParentDetails.MotherMobileno, typeof(System.String), "_MotherMobileno"),
                new DbParam(ParentDetails.Motheremailid, typeof(System.String), "_Motheremailid"),
                new DbParam(ParentDetails.MotherOccupation, typeof(System.String), "_MotherOccupation"),
                new DbParam(ParentDetails.MotherQualification, typeof(System.String), "_MotherQualification"),
                new DbParam(ParentDetails.LocalGuardianFullName, typeof(System.String), "_LocalGuardianFullName"),
                new DbParam(ParentDetails.LocalGuardianMobileno, typeof(System.String), "_LocalGuardianMobileno"),
                new DbParam(ParentDetails.LocalGuardianemailid, typeof(System.String), "_LocalGuardianemailid"),
                new DbParam(userDetail.UserId, typeof(System.String), "_Adminid")
            };

            var output = db.ExecuteNonQuery("sp_Parentdetail_Upd", param, true);
            return output;
        }

        public string DriverRegistrationService(StaffRegistrationForm registrationFormData, List<Files> fileDetail, IFormFileCollection FileCollection)
        {
            string StaffUid = "";
            if (!string.IsNullOrEmpty(registrationFormData.StaffMemberUid))
                StaffUid = registrationFormData.StaffMemberUid;

            DbParam[] param = new DbParam[]
            {
                new DbParam(StaffUid, typeof(System.String), "_StaffMemberUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentid"),
                new DbParam(registrationFormData.FirstName, typeof(System.String), "_FirstName"),
                new DbParam(registrationFormData.LastName, typeof(System.String), "_LastName"),
                new DbParam(registrationFormData.Gender, typeof(System.Boolean), "_Gender"),
                new DbParam(registrationFormData.Dob, typeof(System.DateTime), "_Dob"),
                new DbParam(registrationFormData.MobileNumber, typeof(System.String), "_MobileNumber"),
                new DbParam(registrationFormData.AlternetNumber, typeof(System.String), "_AlternetNumber"),
                new DbParam(registrationFormData.Email, typeof(System.String), "_Email"),
                new DbParam(registrationFormData.Address, typeof(System.String), "_Address"),
                new DbParam(registrationFormData.State, typeof(System.String), "_State"),
                new DbParam(registrationFormData.City, typeof(System.String), "_City"),
                new DbParam(registrationFormData.Pincode, typeof(System.Int64), "_Pincode"),
                new DbParam(registrationFormData.University, typeof(System.String), "_SchoolUniversityName"),
                new DbParam(registrationFormData.ProofOfDocumentationPath, typeof(System.String), "_ProofOfDocumentationPath"),
                new DbParam(registrationFormData.DegreeName, typeof(System.String), "_DegreeName"),
                new DbParam(registrationFormData.Grade, typeof(System.String), "_Grade"),
                new DbParam(null, typeof(System.String), "_Position"),
                new DbParam(registrationFormData.Marks, typeof(System.Double), "_MarksObtain"),
                new DbParam(null, typeof(System.String), "_Title"),
                new DbParam(registrationFormData.ExprienceInYear, typeof(System.String), "_ExprienceInYear"),
                new DbParam(registrationFormData.ExperienceInMonth, typeof(System.String), "_ExperienceInMonth"),
                new DbParam(registrationFormData.ImageUrl, typeof(System.String), "_ImageUrl"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId"),
                new DbParam(registrationFormData.VehicleTypeId, typeof(System.String), "_VehicleTypeId"),
                new DbParam(registrationFormData.VehicleNumber, typeof(System.String), "_VehicleNumber"),
                new DbParam(registrationFormData.VehicleRegistrationNo, typeof(System.String), "_VehicleRegistrationNo"),
                new DbParam(false, typeof(System.Boolean), "_IsAdmin"),
                new DbParam(registrationFormData.VehicleDetailId, typeof(System.String), "_vehicledetailid")
            };

            var output = db.ExecuteNonQuery("sp_DriverAndVehicleRegistration_InsUpd", param, true);
            if (output == "Registration done successfully")
            {
                if (FileCollection.Count > 0 && !string.IsNullOrEmpty(registrationFormData.ImageUrl))
                {
                    string FolderPath = Path.Combine(this.beanContext.GetContentRootPath(),
                        this.currentSession.FileUploadFolderName,
                        "Staff",
                        registrationFormData.MobileNumber);
                    this.fileService.SaveFile(FolderPath, fileDetail, FileCollection, registrationFormData.ImageUrl);
                }
            }
            return output.ToString();
        }

        public string QuickStudentRegistrationService(QuickRegistrationModal quickRegistrationModal, List<Files> fileDetail, IFormFileCollection files)
        {
            string Result = default(string);
            if (quickRegistrationModal.Type == "student")
            {
                StudentRegistrationModel registrationFormData = new StudentRegistrationModel();
                if (!string.IsNullOrEmpty(quickRegistrationModal.ClassDetailUid))
                    registrationFormData.ClassDetailUid = quickRegistrationModal.ClassDetailUid;
                else
                    Result = " Class or section";
                if (!string.IsNullOrEmpty(quickRegistrationModal.Class))
                    registrationFormData.Class = quickRegistrationModal.Class;
                else
                    Result = " Class or section";
                if (!string.IsNullOrEmpty(quickRegistrationModal.Fatherfirstname))
                    registrationFormData.FatherFirstName = quickRegistrationModal.Fatherfirstname;
                else
                    Result = " Father name";
                registrationFormData.FatherLastName = quickRegistrationModal.Fatherlastname;
                if (!string.IsNullOrEmpty(quickRegistrationModal.Motherfirstname))
                    registrationFormData.MotherFirstName = quickRegistrationModal.Motherfirstname;
                else
                    Result = " Mother name";
                registrationFormData.MotherLastName = quickRegistrationModal.Motherlastname;
                if (!string.IsNullOrEmpty(quickRegistrationModal.MobileNumber))
                    registrationFormData.FatherMobileno = quickRegistrationModal.MobileNumber;
                else
                    Result = " Father Mobile no.#";
                if (!string.IsNullOrEmpty(quickRegistrationModal.Class))
                    registrationFormData.FirstName = quickRegistrationModal.Studentfirstname;
                else
                    Result = " Student name";
                registrationFormData.LastName = quickRegistrationModal.Studentlastname;
                if (string.IsNullOrEmpty(Result))
                    Result = SubmitStudentForm(registrationFormData, fileDetail, files, "sp_RegisterStudent");
                else
                    Result = "Invalid" + Result;
            }
            else
            {
                RegistrationFormData registrationFormData = new RegistrationFormData();
                if (!string.IsNullOrEmpty(quickRegistrationModal.FirstName))
                    registrationFormData.FirstName = quickRegistrationModal.FirstName;
                else
                    Result = " First name";
                registrationFormData.LastName = quickRegistrationModal.LastName;
                if (!string.IsNullOrEmpty(quickRegistrationModal.MobileNumber))
                    registrationFormData.MobileNumber = quickRegistrationModal.MobileNumber;
                else
                    Result = " Mobile no.#";
                if (quickRegistrationModal.Type == "faculty")
                    registrationFormData.Designation = "Faculty";
                else
                    registrationFormData.Designation = "Other";
                if (string.IsNullOrEmpty(Result))
                    Result = RegisterStaffFaculty(registrationFormData, fileDetail, files);
                else
                    Result = "Invalid" + Result;
            }
            return Result;
        }

        public string StudentRegistrationService(StudentRegistrationModel registrationFormData, List<Files> fileDetail, IFormFileCollection files)
        {
            return SubmitStudentForm(registrationFormData, fileDetail, files, "sp_RegisterStudent");
        }

        public string SubmitStudentForm(StudentRegistrationModel registrationFormData, List<Files> fileDetail, IFormFileCollection FileCollection, string ProcedureName)
        {
            string output = null;
            ServiceResult ObjServiceResult = validateModalService.ValidateModalFieldsService(typeof(StudentRegistrationModel), registrationFormData);
            if (ObjServiceResult.IsValidModal)
            {
                if (string.IsNullOrEmpty(registrationFormData.ClassDetailUid))
                    registrationFormData.ClassDetailUid = null;
                if (string.IsNullOrEmpty(registrationFormData.EmailId))
                    registrationFormData.EmailId = null;
                if (string.IsNullOrEmpty(registrationFormData.Fatheremailid))
                    registrationFormData.Fatheremailid = null;
                if (string.IsNullOrEmpty(registrationFormData.Mobilenumber))
                    registrationFormData.Mobilenumber = null;
                registrationFormData.AdmissionDatetime = DateTime.Now;
                string ImageName = Guid.NewGuid().ToString();
                DbParam[] param = new DbParam[]
                {
                    new DbParam(registrationFormData.StudentUid, typeof(System.String), "_studentUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentId"),
                    new DbParam(registrationFormData.ParentDetailId, typeof(System.String), "_parentDetailId"),
                    new DbParam(registrationFormData.ClassDetailUid, typeof(System.String), "_classDetailId"),
                    new DbParam(registrationFormData.FirstName, typeof(System.String), "_FirstName"),
                    new DbParam(registrationFormData.LastName, typeof(System.String), "_LastName"),
                    new DbParam(registrationFormData.ImageUrl, typeof(System.String), "_ImageUrl"),
                    new DbParam(registrationFormData.Dob, typeof(System.DateTime), "_Dob"),
                    new DbParam(registrationFormData.Age, typeof(System.Int32), "_age"),
                    new DbParam(registrationFormData.Sex, typeof(System.Boolean), "_sex"),
                    new DbParam(registrationFormData.LastSchoolName, typeof(System.String), "_LastSchoolName"),
                    new DbParam(registrationFormData.LastSchoolAddress, typeof(System.String), "_PSAddress"),
                    new DbParam(registrationFormData.LastSchoolMedium, typeof(System.String), "_PSMedium"),
                    new DbParam(registrationFormData.CurrentSchoolMedium, typeof(System.String), "_CurrentSchoolMedium"),
                    new DbParam(registrationFormData.Rollno, typeof(System.Int32), "_rollno"),
                    new DbParam(registrationFormData.Mobilenumber, typeof(System.String), "_mobilenumber"),
                    new DbParam(registrationFormData.AlternetNumber, typeof(System.String), "_alternetNumber"),
                    new DbParam(registrationFormData.EmailId, typeof(System.String), "_emailId"),
                    new DbParam(registrationFormData.RegistrationNo, typeof(System.String), "_registrationNo"),
                    new DbParam(registrationFormData.MotherTongue, typeof(System.String), "_motherTongue"),
                    new DbParam(registrationFormData.Religion, typeof(System.String), "_religion"),
                    new DbParam(registrationFormData.Catagory, typeof(System.String), "_category"),
                    new DbParam(registrationFormData.CatagoryDocPath, typeof(System.String), "_categoryDocPath"),
                    new DbParam(registrationFormData.SiblingRegistrationNo, typeof(System.String), "_siblingRegistrationNo"),
                    new DbParam(registrationFormData.FatherFirstName, typeof(System.String), "_fatherFirstName"),
                    new DbParam(registrationFormData.FatherLastName, typeof(System.String), "_fatherLastName"),
                    new DbParam(registrationFormData.MotherFirstName, typeof(System.String), "_motherFirstName"),
                    new DbParam(registrationFormData.MotherLastName, typeof(System.String), "_motherLastName"),
                    new DbParam(registrationFormData.LocalGuardianFullName, typeof(System.String), "_localguardianfullname"),
                    new DbParam(registrationFormData.FatherMobileno, typeof(System.String), "_fathermobileno"),
                    new DbParam(registrationFormData.MotherMobileno, typeof(System.String), "_mothermobileno"),
                    new DbParam(registrationFormData.LocalGuardianMobileno, typeof(System.String), "_localguardianmobileno"),
                    new DbParam(registrationFormData.FullAddress, typeof(System.String), "_fulladdress"),
                    new DbParam(registrationFormData.City, typeof(System.String), "_city"),
                    new DbParam(registrationFormData.Pincode, typeof(System.String), "_pincode"),
                    new DbParam(registrationFormData.State, typeof(System.String), "_state"),
                    new DbParam(registrationFormData.Fatheremailid, typeof(System.String), "_fatheremailid"),
                    new DbParam(registrationFormData.Motheremailid, typeof(System.String), "_motheremailid"),
                    new DbParam(registrationFormData.LocalGuardianemailid, typeof(System.String), "_localguardianemailid"),
                    new DbParam(registrationFormData.Fatheroccupation, typeof(System.String), "_fatheroccupation"),
                    new DbParam(registrationFormData.Motheroccupation, typeof(System.String), "_motheroccupation"),
                    new DbParam(registrationFormData.Fatherqualification, typeof(System.String), "_fatherQualification"),
                    new DbParam(registrationFormData.Motherqualification, typeof(System.String), "_motherQualification"),
                    new DbParam(registrationFormData.LastClass, typeof(System.String), "_LastClass"),
                    new DbParam(userDetail.UserId, typeof(System.String), "_CreatedBy"),
                    new DbParam(registrationFormData.ParentRecordExist, typeof(System.Boolean), "_ParentRecordExist"),
                    new DbParam(registrationFormData.ExistingNumber, typeof(System.String), "_ExistingNumber"),
                    new DbParam(registrationFormData.IsQuickRegistration, typeof(System.Boolean), "_IsQuickRegistration")
                };

                output = db.ExecuteNonQuery(ProcedureName, param, true);
                if (!string.IsNullOrEmpty(output) && output.IndexOf("Fail") == -1)
                {
                    if (FileCollection.Count > 0)
                    {
                        string FolderPath = Path.Combine(this.currentSession.FileUploadFolderName,
                            ApplicationConstant.Student,
                            output);
                        List<Files> files = this.fileService.SaveFile(FolderPath, fileDetail, FileCollection, output);
                        if (files != null && files.Count > 0)
                        {
                            DataSet fileDs = this.beanContext.ConvertToDataSet<Files>(files);
                            if (fileDs != null && fileDs.Tables.Count > 0 && fileDs.Tables[0].Rows.Count > 0)
                            {
                                DataTable table = fileDs.Tables[0];
                                table.TableName = "Files";
                                db.InsertUpdateBatchRecord("sp_Files_InsUpd", table);
                                output = "Record inserted successfully.";
                            }
                        }
                        else
                        {
                            ///==========  Log information ====================
                            output = "Record updated successfully.";
                        }
                    }
                }
            }
            else
            {
                output = JsonConvert.SerializeObject(ObjServiceResult.ErrorResultedList);
            }
            return output.ToString();
        }

        public int VerifyMobibleExist(string Mobile, string EmailId)
        {
            int returnData = 0;
            DbParam[] param = new DbParam[]
            {
                new DbParam(Mobile, typeof(System.String), "_mobileNo"),
                new DbParam(EmailId, typeof(System.String), "_emailId"),
                new DbParam(false, typeof(System.Boolean), "_isMobile"),
                new DbParam(false, typeof(System.Boolean), "_isEmail"),
            };

            var result = db.ExecuteSingle("sp_Login_IsMobileOrEmailExist", param, true);
            if (result != DBNull.Value)
            {
                returnData = Convert.ToInt32(result);
            }
            return returnData;
        }

        public string GetParentInfoService(string Mobile)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(Mobile, typeof(System.String), "_mobile"),
            };

            var result = db.GetDataset("sp_GetInfo_ByNumber", param);
            return JsonConvert.SerializeObject(result);
        }

        public DataTable ListClassesService()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam("1=1", typeof(System.String), "_searchString"),
                new DbParam("Class", typeof(System.String), "_sortBy"),
                new DbParam("1", typeof(System.String), "_pageIndex"),
                new DbParam("100", typeof(System.String), "_pageSize")
            };
            var result = db.GetDataset("sp_ListClassDetail", param);
            if (result != null && result.Tables.Count > 0)
                return result.Tables[0];
            return null;
        }

        public DataTable GetStudentListService(string RegistrationNo, bool CompleteReg)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(RegistrationNo, typeof(System.String), "_registrationNo"),
                new DbParam(CompleteReg, typeof(System.Boolean), "_completeReg"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            var result = db.GetDataset("sp_StudentDetail_SelByRegno", param);
            if (result != null && result.Tables.Count > 0)
                return result.Tables[0];
            return null;
        }

        public string StudentDetailByUidService(string StudentUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(StudentUid, typeof(System.String), "_studentUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            var result = db.GetDataset("sp_StudentDetail_SelByStudentUid", param);
            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                return JsonConvert.SerializeObject(result);
            return "";
        }

        public string StaffMemberByUidService(string StaffUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(StaffUid, typeof(System.String), "_staffUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            var result = db.GetDataset("sp_StaffMembers_SelByStaffUid", param);
            if (result != null && result.Tables.Count > 0)
                return JsonConvert.SerializeObject(result);
            return null;
        }

        public string OtherStaffMemberByUidService(string StaffUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(StaffUid, typeof(System.String), "_staffUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            var result = db.GetDataset("sp_OtherStaffMembers_SelByStaffUid", param);
            if (result != null && result.Tables.Count > 0)
                return JsonConvert.SerializeObject(result);
            return null;
        }

        public string DeleteImageService(string ImageUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ImageUid, typeof(System.String), "_imageUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            string result = db.ExecuteNonQuery("sp_DeleteImage_ByUid", param, true);
            return result;
        }

        public string SignUpService(SignUpModel signUpModel)
        {
            if (!string.IsNullOrEmpty(signUpModel.FullName))
            {
                string FirstName = string.Empty;
                string LastName = string.Empty;
                string[] names = signUpModel.FullName.Split(" ");
                if (names.Length >= 2)
                {
                    FirstName = names[0];
                    LastName = names.SkipWhile(x=>x.Trim() == FirstName.Trim()).ToArray<string>().Aggregate((x, y) => x.Trim() + " " + y.Trim());
                }
                else
                {
                    FirstName = names[0];
                }

                DbParam[] param = new DbParam[]
                {
                    new DbParam(null, typeof(System.String), "_tenentId"),
                    new DbParam(null, typeof(System.String), "_accessLevelId"),
                    new DbParam(null, typeof(System.String), "_loginId"),
                    new DbParam("12345", typeof(System.String), "_password"),
                    new DbParam(signUpModel.SchoolName, typeof(System.String), "_schoolfullname"),
                    new DbParam(string.IsNullOrEmpty(signUpModel.LicenseNo) ? "NA" : signUpModel.LicenseNo, typeof(System.String), "_licenseNo"),
                    new DbParam(string.IsNullOrEmpty(signUpModel.AffilatedBy) ? "NA" : signUpModel.AffilatedBy, typeof(System.String), "_AffiliatedBy"),
                    new DbParam(FirstName, typeof(System.String), "_firstName"),
                    new DbParam(LastName, typeof(System.String), "_lastName"),
                    new DbParam(signUpModel.MobileNo, typeof(System.String), "_mobile"),
                    new DbParam(signUpModel.Address, typeof(System.String), "_fullAddress"),
                    new DbParam("IN", typeof(System.String), "_countryCode")
                };

                var result = db.ExecuteNonQuery("sp_signup", param, true);
                return result;
            }
            return null;
        }
    }
}