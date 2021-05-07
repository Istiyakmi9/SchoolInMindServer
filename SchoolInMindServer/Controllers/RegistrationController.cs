using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Net;

namespace SchoolInMindServer.Controllers
{
    [Route("api/Registration")]
    public class RegistrationController : BaseController
    {
        private readonly ICommonService<CommonService> commonService;
        private readonly IRegistrationService<RegistrationService> registrationService;
        private readonly IEventService<EventService> eventService;
        private readonly HttpContext httpContext;
        public RegistrationController(RegistrationService registrationService,
            IHttpContextAccessor httpContext,
            CommonService commonService,
            EventService eventService)
        {
            this.httpContext = httpContext.HttpContext;
            this.commonService = commonService;
            this.eventService = eventService;
            this.registrationService = registrationService;
        }

        [HttpPost]
        [Route("Faculty")]
        public IResponse<ApiResponse> FacultyRegistration()
        {
            StringValues RegistrationData = default(string);
            StringValues FileData = default(string);
            httpContext.Request.Form.TryGetValue("facultObject", out RegistrationData);
            httpContext.Request.Form.TryGetValue("fileDetail", out FileData);
            if (RegistrationData.Count > 0)
            {
                RegistrationFormData facultydetails = JsonConvert.DeserializeObject<RegistrationFormData>(RegistrationData[0]);
                List<Files> fileDetail = JsonConvert.DeserializeObject<List<Files>>(FileData);
                if (facultydetails != null)
                {
                    IFormFileCollection files = httpContext.Request.Form.Files;
                    string Result = this.registrationService.RegisterStaffFaculty(facultydetails, fileDetail, files);
                    BuildResponse(Result);
                }
            }
            return apiResponse;
        }

        [HttpDelete]
        [Route("DeleteImage")]
        public IResponse<ApiResponse> DeleteImage(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string Result = this.registrationService.DeleteImageService(data);
                BuildResponse("Result", HttpStatusCode.OK);
            }
            return apiResponse;
        }

        [HttpPost]
        [Route("StudentRegistration")]
        public IResponse<ApiResponse> StudentRegistration()
        {
            StringValues SubmitedStringifyData = default(string);
            StringValues FileData = default(string);
            httpContext.Request.Form.TryGetValue("studentObject", out SubmitedStringifyData);
            httpContext.Request.Form.TryGetValue("fileDetail", out FileData);
            if (SubmitedStringifyData.Count > 0)
            {
                StudentRegistrationModel studentRegistrationModel = JsonConvert.DeserializeObject<StudentRegistrationModel>(SubmitedStringifyData[0]);
                List<Files> fileDetail = JsonConvert.DeserializeObject<List<Files>>(FileData);
                if (studentRegistrationModel != null)
                {
                    IFormFileCollection files = httpContext.Request.Form.Files;
                    string Result = this.registrationService.StudentRegistrationService(studentRegistrationModel, fileDetail, files);
                    BuildResponse(Result, HttpStatusCode.OK);
                }
            }
            return apiResponse;
        }


        [HttpPost]
        public IResponse<ApiResponse> AddUpdateDeleteDocument()
        {
            //RegistrationFormData objRegistrationFormData = null;
            //string UploadStatus = null;
            //HttpFileCollectionBase files = Request.Files;
            //foreach (var key in Request.Form.AllKeys)
            //{
            //    if (key == "formObject")
            //    {
            //        objRegistrationFormData = context.Parse<RegistrationFormData>(Request.Form.GetValues("formObject").FirstOrDefault());
            //        break;
            //    }
            //}

            //string fname = null;
            //string NewName = null;
            //string FolderName = objRegistrationFormData.ProofOfDocumentationPath;
            //string DocumentPath = Path.Combine(Server.MapPath("~/Uploads/"), FolderName);
            //for (int i = 0; i < files.Count; i++)
            //{
            //    var file = files[i];

            //    // Checking for Internet Explorer  
            //    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //    {
            //        string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //        fname = testfiles[testfiles.Length - 1];
            //    }
            //    else
            //    {
            //        fname = file.FileName;
            //    }

            //    if (Directory.Exists(DocumentPath))
            //    {
            //        if (string.IsNullOrEmpty(objRegistrationFormData.ExistingDocumentFileName))
            //            NewName = fname;
            //        else
            //            NewName = objRegistrationFormData.ExistingDocumentFileName;
            //        fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //        if (System.IO.File.Exists(fname))
            //            System.IO.File.Delete(fname);
            //        NewName = DateTime.Now.Ticks.ToString() + "." + fname.Split('.')[1];
            //        fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //        file.SaveAs(fname);
            //        UploadStatus = "100";
            //    }
            //}

            //fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, objRegistrationFormData.ExistingDocumentFileName);
            //if (System.IO.File.Exists(fname))
            //{
            //    System.IO.File.Delete(fname);
            //    UploadStatus = "101";
            //}

            return null;// Json(new { response = UploadStatus, NewList = ObjCommonService.GetAllFileNames(DocumentPath) });
        }

        [HttpPost]
        public IResponse<ApiResponse> RegisterFaculty()
        {
            //RegistrationFormData objRegistrationFormData = null;
            //HttpFileCollectionBase files = Request.Files;
            //string fname = null;

            //foreach (var key in Request.Form.AllKeys)
            //{
            //    if (key == "formObject")
            //    {
            //        objRegistrationFormData = context.Parse<RegistrationFormData>(Request.Form.GetValues("formObject").FirstOrDefault());
            //        objRegistrationFormData.DesignationId = 1;
            //        break;
            //    }
            //}

            //HttpPostedFileBase file = null;
            //string ProfileImageFileName = string.Empty;
            //string NewName = string.Empty;
            //if (!string.IsNullOrEmpty(objRegistrationFormData.ProfileImageName))
            //    ProfileImageFileName = "profile_image" + "." + objRegistrationFormData.ProfileImageName.Split('.')[1];

            //if (string.IsNullOrEmpty(objRegistrationFormData.StaffMemberUid))
            //    objRegistrationFormData.StaffMemberUid = Guid.NewGuid().ToString();
            //string FolderName = "Doc_" + objRegistrationFormData.StaffMemberUid;
            //objRegistrationFormData.ProofOfDocumentationPath = FolderName;
            //string returnedData = objRegistrationService.RegisterStaffFaculty(objRegistrationFormData, ProfileImageFileName);
            //if (returnedData != null)
            //{
            //    string DocumentPath = Path.Combine(Server.MapPath("~/Uploads/"), FolderName);
            //    for (int i = 0; i < files.Count; i++)
            //    {
            //        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
            //        //string filename = Path.GetFileName(Request.Files[i].FileName);  

            //        file = files[i];

            //        // Checking for Internet Explorer  
            //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //        {
            //            string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //            fname = testfiles[testfiles.Length - 1];
            //        }
            //        else
            //        {
            //            fname = file.FileName;
            //        }

            //        if (!Directory.Exists(DocumentPath))
            //            Directory.CreateDirectory(DocumentPath);
            //        // Get the complete folder path and store the file inside it.  
            //        if (objRegistrationFormData.ProfileImageName != file.FileName)
            //        {
            //            NewName = Guid.NewGuid() + "." + fname.Split('.')[1];
            //            fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //            file.SaveAs(fname);
            //        }
            //        else
            //        {
            //            NewName = ProfileImageFileName;
            //            fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //            file.SaveAs(fname);
            //        }
            //    }
            //}
            return null;// Json(returnedData);
        }

        [HttpPost]
        public IResponse<ApiResponse> DriverRegister()
        {
            //StaffRegistrationForm objRegistrationFormData = null;
            //HttpFileCollectionBase files = Request.Files;
            //string fname = null;

            //foreach (var key in Request.Form.AllKeys)
            //{
            //    if (key == "formObject")
            //    {
            //        objRegistrationFormData = context.Parse<StaffRegistrationForm>(Request.Form.GetValues("formObject").FirstOrDefault());
            //        objRegistrationFormData.DesignationId = 1;
            //        break;
            //    }
            //}

            //HttpPostedFileBase file = null;
            //string ProfileImageFileName = string.Empty;
            //string NewName = string.Empty;
            //if (!string.IsNullOrEmpty(objRegistrationFormData.ProfileImageName))
            //    ProfileImageFileName = "profile_image" + "." + objRegistrationFormData.ProfileImageName.Split('.')[1];

            //if (string.IsNullOrEmpty(objRegistrationFormData.StaffMemberUid))
            //    objRegistrationFormData.StaffMemberUid = Guid.NewGuid().ToString();
            //string FolderName = "Doc_" + objRegistrationFormData.StaffMemberUid;
            //objRegistrationFormData.ProofOfDocumentationPath = FolderName;
            //var returnedData = objRegistrationService.DriverRegistrationService(objRegistrationFormData, ProfileImageFileName);
            //if (returnedData != null)
            //{
            //    string DocumentPath = Path.Combine(Server.MapPath("~/Uploads/"), FolderName);
            //    if (!Directory.Exists(DocumentPath))
            //        Directory.CreateDirectory(DocumentPath);

            //    for (int i = 0; i < files.Count; i++)
            //    {
            //        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
            //        //string filename = Path.GetFileName(Request.Files[i].FileName);  

            //        file = files[i];

            //        // Checking for Internet Explorer  
            //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //        {
            //            string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //            fname = testfiles[testfiles.Length - 1];
            //        }
            //        else
            //        {
            //            fname = file.FileName;
            //        }

            //        // Get the complete folder path and store the file inside it.  
            //        if (objRegistrationFormData.ProfileImageName != file.FileName)
            //        {
            //            NewName = Guid.NewGuid() + "." + fname.Split('.')[1];
            //            fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //            file.SaveAs(fname);
            //        }
            //        else
            //        {
            //            NewName = ProfileImageFileName;
            //            fname = Path.Combine(Server.MapPath("~/Uploads/"), FolderName, NewName);
            //            file.SaveAs(fname);
            //        }
            //    }
            //}
            return null;// Json(returnedData);
        }

        [HttpGet]
        public IResponse<ApiResponse> GetParentInfo(string MobileNumber)
        {
            var result = registrationService.GetParentInfoService(MobileNumber);
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> SubmitStudentForm()
        {
            //StudentRegistrationModel objRegistrationFormData = null;
            //HttpFileCollectionBase files = Request.Files;
            //IDictionary<string, List<string>> ObjRecords = null;
            //string Mobiles = null;
            //string Emails = null;
            //string fname = null;

            //foreach (var key in Request.Form.AllKeys)
            //{
            //    if (key == "studentFormObject")
            //    {
            //        objRegistrationFormData = context.Parse<StudentRegistrationModel>(Request.Form.GetValues("studentFormObject").FirstOrDefault());
            //        objRegistrationFormData.IsQuickRegistration = false;
            //        if (!string.IsNullOrEmpty(objRegistrationFormData.MobileNumbers))
            //            Mobiles = objRegistrationFormData.MobileNumbers;
            //        else
            //            Mobiles = "";
            //        if (!string.IsNullOrEmpty(objRegistrationFormData.EmailIds))
            //            Emails = objRegistrationFormData.EmailIds;
            //        else
            //            Emails = "";

            //        if (Mobiles != "" || Emails != "")
            //            ObjRecords = ObjCommonService.ValidateMobileAndEmail(Mobiles, Emails);
            //        if (ObjRecords != null && ObjRecords.Count > 0)
            //        {
            //            Boolean InValidFlag = false;
            //            foreach (KeyValuePair<string, List<string>> ValidObjects in ObjRecords)
            //            {
            //                if (ValidObjects.Key != null && ValidObjects.Value != null)
            //                {
            //                    if (ValidObjects.Value.Count > 0)
            //                        InValidFlag = true;
            //                }
            //            }

            //            if (InValidFlag)
            //            {
            //                fname = context.Stringify(ObjRecords);
            //                return Json(new { EmailMobileExists = fname });
            //            }
            //        }
            //        break;
            //    }
            //}

            //string NewName = string.Empty;
            //if (!string.IsNullOrEmpty(objRegistrationFormData.ProfileImageName))
            //    objRegistrationFormData.ImageUrl = "profile_image." + objRegistrationFormData.ProfileImageName.Split('.')[1];
            //else if (files != null && files.Count > 0)
            //    objRegistrationFormData.ImageUrl = "profile_image." + files[0].FileName.Split('.')[1];
            //NewName = objRegistrationFormData.ImageUrl;
            //if (string.IsNullOrEmpty(objRegistrationFormData.StudentUid))
            //    objRegistrationFormData.StudentUid = Guid.NewGuid().ToString();
            //string FolderName = "Doc_" + objRegistrationFormData.StudentUid;
            //var returnedData = objRegistrationService.StudentRegistrationService(objRegistrationFormData, NewName);
            //if (objRegistrationFormData != null && returnedData != null)
            //{
            //    HttpPostedFileBase file = null;
            //    string DocumentPath = Path.Combine(Server.MapPath("~/Uploads/"), FolderName);
            //    if (!Directory.Exists(DocumentPath))
            //        Directory.CreateDirectory(DocumentPath);
            //    for (int i = 0; i < files.Count; i++)
            //    {
            //        file = files[i];
            //        // Checking for Internet Explorer  
            //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //        {
            //            string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //            fname = testfiles[testfiles.Length - 1];
            //        }
            //        else
            //        {
            //            fname = file.FileName;
            //        }

            //        // Get the complete folder path and store the file inside it.  
            //        fname = Path.Combine(Server.MapPath("~/Uploads/"), DocumentPath, NewName);
            //    }

            //    if (files.Count > 0)
            //        file.SaveAs(fname);
            //    return Json(returnedData);
            //}

            return null;// Json(fname);
        }

        [HttpGet]
        public IResponse<ApiResponse> VerifyMobibleExist(string Mobile, string EmailId)
        {
            IList<string> resultArray = null;
            int result = 0;
            result = registrationService.VerifyMobibleExist(Mobile, EmailId);
            if (result > 0)
            {
                resultArray = new List<string>();
                if (result == 1)
                    resultArray.Add("Mobile");
                if (result == 2)
                    resultArray.Add("Email");
                if (result == 3)
                    resultArray.Add("Email and Mobile");
            }
            return null;// Json(context.Stringify(resultArray), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public IResponse<ApiResponse> ListClasses()
        {
            var result = registrationService.ListClassesService();
            return null;// Json(context.Stringify(result), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public IResponse<ApiResponse> GetStudentList(string RegistrationNo, bool CompleteReg)
        {
            var result = registrationService.GetStudentListService(RegistrationNo, CompleteReg);
            return null;// Json(context.Stringify(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public IResponse<ApiResponse> UpdateParentDetailService(ParentDetails ParentDetails)
        {
            var result = registrationService.UpdateParentDetailService(ParentDetails);
            return null;// Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("GetStudentByUid")]
        public IResponse<ApiResponse> GetStudentByUid(string Uid)
        {
            string Result = this.registrationService.StudentDetailByUidService(Uid);
            BuildResponse(Result, HttpStatusCode.OK);
            return apiResponse;
        }

        [HttpGet]
        [Route("GetStaffMemberByUid")]
        public IResponse<ApiResponse> GetStaffMemberByUid(string Uid)
        {
            string Result = this.registrationService.StaffMemberByUidService(Uid);
            BuildResponse(Result, HttpStatusCode.OK);
            return apiResponse;
        }

        [HttpPost]
        [Route("NewSignUp")]
        public IResponse<ApiResponse> SignUp([FromBody] SignUpModel signUpModel)
        {
            string Result = this.registrationService.SignUpService(signUpModel);
            BuildResponse(Result, HttpStatusCode.OK);
            return apiResponse;
        }
    }
}