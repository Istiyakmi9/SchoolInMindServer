using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Services.Code;
using CommonModal.Models;
using CommonModal.ORMModels;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreServiceLayer.Implementation
{
    public class AdminMasterService : CurrentUserObject, IAdminMasterService<AdminMasterService>
    {
        private string Result = null;
        public readonly IDb db;
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        public AdminMasterService(IDb db, ValidateModalService validateModalService, CurrentSession currentSession)
        {
            this.db = db;
            userDetail = currentSession.CurrentUserDetail;
            this.validateModalService = validateModalService;
        }

        public DataSet GetClassesService(SearchModal searchModal)
        {
            this.validateModalService.ValidateSeachModal(searchModal);
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize")
            };
            DataSet ds = db.GetDataset("sp_ListClassDetail_BySectionGroup", param);
            if (ds != null && ds.Tables.Count > 0)
                return ds;
            return null;
        }

        public DataSet DeleteClassAndSectionService(int Class, string Section)
        {
            DataSet result = null;
            if (Class > 0 && !string.IsNullOrEmpty(Section))
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(Class, typeof(System.String), "_Class"),
                    new DbParam(Section.ToUpper(), typeof(System.String), "_Section"),
                };
                db.ExecuteNonQuery("sp_ClassDetail_del", param, true);
                result = GetClassesService(new SearchModal { SearchString = "1=1", PageIndex = 1, PageSize = 15, SortBy = null });
            }

            return result;
        }

        public string ExamDescriptionService()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentId")
            };
            DataSet ds = db.GetDataset("sp_ExamDescription_SelAll", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                Result = JsonConvert.SerializeObject(ds);
            return Result;
        }

        public string GetExamDetail(int Year)
        {
            if (Year == 0)
                Year = userDetail.AccedemicStartYear;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId"),
                new DbParam(Year, typeof(System.Int32), "_year")
            };
            DataSet ds = db.GetDataset("sp_ExamDetails_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0)
                Result = JsonConvert.SerializeObject(ds);
            else
                Result = JsonConvert.SerializeObject(new DataTable());
            return Result;
        }

        public string ExamDataInsertion(Examdetails ObjExamdetails)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(null, typeof(System.String), "_ExamDetailId"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_SchooltenentId"),
                new DbParam(ObjExamdetails.ExamDescriptionId, typeof(System.String), "_ExamDescriptionId"),
                new DbParam(ObjExamdetails.Class, typeof(System.String), "_Class"),
                new DbParam(ObjExamdetails.SubjectId, typeof(System.String), "_SubjectId"),
                new DbParam(ObjExamdetails.ExamDate, typeof(System.DateTime), "_ExamDate"),
                new DbParam(ObjExamdetails.StartTime, typeof(System.String), "_Starttime"),
                new DbParam(ObjExamdetails.Duration, typeof(System.Int64), "_Duration"),
                new DbParam(userDetail.AccedemicStartYear, typeof(System.Int32), "_AcademicYearFrom"),
                new DbParam(userDetail.AccedemicStartYear + 1, typeof(System.Int32), "_AcademicYearTo"),
                new DbParam(userDetail.UserId, typeof(System.String), "_Adminid")
            };
            Result = db.ExecuteNonQuery("sp_examdetails_InsUpd", param, true);
            if (Result == null || Result == "")
                Result = "";
            return Result;
        }

        public string InsertResultDescriptionService(ExamDescription examDescription)
        {
            ServiceResult serviceResult = validateModalService.ValidateModalFieldsService<ExamDescription>(examDescription);
            if (serviceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                new DbParam(examDescription.ExamDescriptionUid, typeof(System.String), "_examiddescriptionid"),
                new DbParam(examDescription.ExamName, typeof(System.String), "_examName"),
                new DbParam(examDescription.Description, typeof(System.String), "_description"),
                new DbParam(examDescription.ExpectedDate, typeof(System.DateTime), "_expectedDate"),
                new DbParam(examDescription.ActualDate, typeof(System.DateTime), "_actualDate"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentid"),
                new DbParam(userDetail.UserId, typeof(System.String), "_adminId")
                };
                Result = db.ExecuteNonQuery("sp_examiddescription_InsUpd", param, true);
            }
            return Result;
        }

        public string MoveMenuItemService(MenuAndRolesModal menuAndRolesModal)
        {
            ServiceResult serviceResult = validateModalService.ValidateModalFieldsService<MenuAndRolesModal>(menuAndRolesModal);
            if (serviceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(menuAndRolesModal.MenuName, typeof(System.String), "_menuName"),
                    new DbParam(menuAndRolesModal.OldCatagory, typeof(System.String), "_oldSubCategory"),
                    new DbParam(menuAndRolesModal.Category, typeof(System.String), "_subCategory")
                };
                Result = db.ExecuteNonQuery("sp_ReOrderMenu", param, true);
            }
            return Result;
        }

        public string ViewAddSubjectService(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            if (string.IsNullOrEmpty(SearchStr))
                SearchStr = "1=1";
            if (string.IsNullOrEmpty(SortBy))
                SortBy = "";
            if (string.IsNullOrEmpty(PageIndex))
                PageIndex = "1";
            if (string.IsNullOrEmpty(PageSize))
                PageSize = "15";
            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchStr, typeof(System.String), "_searchString"),
                new DbParam(SortBy, typeof(System.String), "_sortBy"),
                new DbParam(PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(PageSize, typeof(System.String), "_pageSize")
            };
            DataSet ds = db.GetDataset("sp_Subject_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                Result = JsonConvert.SerializeObject(ds);
            return Result;
        }

        public string InsertNewClassInfoService(Classdetail objClassdetail)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objClassdetail.ClassDetailUid, typeof(System.String), "_ClassDetailId"),
                new DbParam(objClassdetail.Class, typeof(System.String), "_Class"),
                new DbParam(objClassdetail.TotalSeats, typeof(System.String), "_TotalSeats"),
                new DbParam(objClassdetail.Section.ToUpper(), typeof(System.String), "_Section"),
                new DbParam(objClassdetail.GirlSeats, typeof(System.String), "_GirlSeats"),
                new DbParam(objClassdetail.BoySeats, typeof(System.String), "_BoySeats"),
                new DbParam(objClassdetail.RoomUid, typeof(System.Int64), "_RoomUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentId"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
            };
            Result = db.ExecuteNonQuery("sp_ClassDetail_InsUpd", param, true);
            return Result;
        }

        public string GetVehicleTypeService()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };

            string ProcessingStatus = string.Empty;
            ResultSet = db.GetDataset("sp_GetVehicleType", param, true, ref ProcessingStatus);
            if (ResultSet != null && ResultSet.Tables.Count == 0)
                return null;
            return JsonConvert.SerializeObject(ResultSet);
        }

        public string AddEditSubjectService(Subject subject)
        {
            ServiceResult serviceResult = this.validateModalService.ValidateModalFieldsService<Subject>(subject);
            if (serviceResult.IsValidModal)
            {

                DbParam[] param = new DbParam[]
                {
                new DbParam(subject.SubjectId, typeof(System.String), "_Subjectid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_Schooltenentid"),
                new DbParam(subject.SubjectName, typeof(System.String), "_SubjectName"),
                new DbParam(subject.SubjectCode, typeof(System.Int32), "_SubjectCode"),
                new DbParam(subject.SubjectCredit, typeof(System.Int64), "_SubjectCredit"),
                new DbParam(subject.IsActive, typeof(System.Boolean), "_IsActive"),
                new DbParam(subject.ForClass, typeof(System.String), "_ForClass"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
                };
                Result = db.ExecuteNonQuery("sp_subjects_InsUpd", param, true);
            }
            return Result;
        }

        public string AttendenceByClassDetailService(Classdetail classdetail)
        {
            if (!string.IsNullOrEmpty(classdetail.ClassDetailUid) && classdetail.CreatedOn != null)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(classdetail.ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
                    new DbParam(classdetail.CreatedOn.Month, typeof(System.Int32), "_Month"),
                    new DbParam(classdetail.CreatedOn.Year, typeof(System.Int32), "_Year")
                };
                ResultSet = db.GetDataset("sp_Attendance_DetailByClass", param);
            }
            return JsonConvert.SerializeObject(ResultSet);
        }

        public string AddUpdateAttendenceService(List<AttendanceClassWise> attendanceClassWise)
        {
            Parallel.ForEach(attendanceClassWise, item =>
            {
                item.TanentUid = userDetail.schooltenentId;
            });
            ResultSet = beanContext.ConvertToDataSet<AttendanceClassWise>(attendanceClassWise);
            if (ResultSet != null && ResultSet.Tables.Count > 0)
                Result = db.InsertUpdateBatchRecord("sp_AttendanceSheet_InsUpd", ResultSet.Tables[0]);
            return JsonConvert.SerializeObject(ResultSet);
        }

        public string AddUpdateSingleClassAttendenceService(List<AttendanceSingleData> attendanceSingleData)
        {
            AttendanceSingleData attendance = attendanceSingleData.FirstOrDefault();
            if (attendance != null)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(attendance.ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
                    new DbParam(attendance.Date.Month, typeof(System.Int32), "_Month"),
                    new DbParam(attendance.Date.Year, typeof(System.Int32), "_Year")
                };
                ResultSet = db.GetDataset("sp_Attendance_SelForUpdateMultipleRows", param);
                if (ResultSet != null && ResultSet.Tables.Count > 0 && ResultSet.Tables[0].Rows.Count > 0)
                {
                    string StudentUid = "";
                    string AttendanceDate = "";
                    foreach (DataRow row in ResultSet.Tables[0].Rows)
                    {
                        if (row["StudentUid"] != DBNull.Value)
                        {
                            StudentUid = row["StudentUid"].ToString();
                            if (!string.IsNullOrEmpty(StudentUid))
                            {
                                var Record = attendanceSingleData.Where(x => x.StudentUid == StudentUid).FirstOrDefault();
                                if (Record != null)
                                {
                                    AttendanceDate = "Day" + Record.Date.Day;
                                    if (row[AttendanceDate] != DBNull.Value)
                                        row[AttendanceDate] = Record.IsPresent;
                                }
                            }
                        }
                    }

                    List<AttendanceClassWise> attendanceClassWises = Converter.ToList<AttendanceClassWise>(ResultSet.Tables[0]);
                    if (attendanceClassWises.Count > 0)
                    {
                        ResultSet = beanContext.ConvertToDataSet<AttendanceClassWise>(attendanceClassWises);
                        if (ResultSet != null && ResultSet.Tables.Count > 0)
                            Result = db.InsertUpdateBatchRecord("sp_AttendanceSheet_InsUpd", ResultSet.Tables[0]);
                        return JsonConvert.SerializeObject("Success");
                    }
                }
            }
            return null;
        }

        private void EnableParentMenu()
        {

        }

        public string AddUpdateRoleService(MenuAndRoles menuAndRoles)
        {
            ServiceResult serviceResult = validateModalService.ValidateModalFieldsService<MenuAndRoles>(menuAndRoles);
            if (serviceResult.IsValidModal && menuAndRoles.MenuAndRolesModal != null && menuAndRoles.MenuAndRolesModal.Count > 0)
            {
                List<MenuAndRolesModal> ParentMenu = null;
                List<MenuAndRolesModal> ActiveMenu = menuAndRoles.MenuAndRolesModal.Where(x => x.IsActive == true).ToList<MenuAndRolesModal>();
                foreach (MenuAndRolesModal menu in ActiveMenu)
                {
                    ParentMenu = menuAndRoles.MenuAndRolesModal.Where(x => x.MenuName == menu.Childs).ToList<MenuAndRolesModal>();
                    foreach (var item in ParentMenu)
                    {
                        item.IsActive = true;
                        menuAndRoles.MenuAndRolesModal.Where(x => x.MenuName == item.Childs).ToList<MenuAndRolesModal>().ForEach(Item =>
                        {
                            Item.IsActive = true;
                        });
                    }
                }

                if (menuAndRoles.MenuAndRolesModal != null && menuAndRoles.MenuAndRolesModal.Count > 0)
                {
                    DbParam[] param = new DbParam[]
                    {
                        new DbParam(menuAndRoles.AccessLevelUid, typeof(System.String), "_AccessLevelId"),
                        new DbParam(menuAndRoles.AccessCode, typeof(System.Int32), "_AccessCode"),
                        new DbParam(menuAndRoles.RoleName, typeof(System.String), "_Roles"),
                        new DbParam(menuAndRoles.RoleDescription, typeof(System.String), "_AccessCodeDefination"),
                        new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentId")
                    };
                    Result = db.ExecuteNonQuery("sp_AccessLevel_InsUpd", param, true);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        IList<RoleModal> roleModal = (from n in menuAndRoles.MenuAndRolesModal
                                                      select new RoleModal
                                                      {
                                                          AccessUid = Convert.ToInt32(Result),
                                                          AccessCode = n.AccessCode,
                                                          PermissionLevel = (n.IsActive ? n.PermissionLevel : 0)
                                                      }).ToList<RoleModal>();

                        if (roleModal.Count > 0)
                        {
                            ResultSet = this.beanContext.ConvertToDataSet<RoleModal>(roleModal);
                            Result = db.InsertUpdateBatchRecord("sp_MenuAndRoles_InsUpdBulk", ResultSet.Tables[0]);
                            return JsonConvert.SerializeObject("Success");
                        }
                    }
                }
            }
            return null;
        }

        public string GetRoleService()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
            };
            ResultSet = db.GetDataset("sp_AccessLevel_Sel", param);
            DataSet MenuSet = db.GetDataset("sp_RolesAndMenu_GetAll");
            return JsonConvert.SerializeObject(new { MenuByRoles = ResultSet, FullMenu = MenuSet });
        }

        public string RolesByAccessLevelService(string AccessLevelUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(AccessLevelUid, typeof(System.String), "_AccessLevelUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid")
            };
            ResultSet = db.GetDataset("sp_MenuDetail_ByAccessCode", param);
            return JsonConvert.SerializeObject(ResultSet);
        }

        public string GetAllMenuService()
        {
            ResultSet = db.GetDataset("sp_RolesAndMenu_GetAll");
            return JsonConvert.SerializeObject(ResultSet);
        }
    }
}
