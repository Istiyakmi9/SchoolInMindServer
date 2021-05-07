using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ORMModels;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class EventService : CurrentUserObject, IEventService<EventService>
    {
        private string OutParam = string.Empty;
        private IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public EventService(ValidateModalService validateModalService, CurrentSession currentSession, IDb db)
        {
            this.validateModalService = validateModalService;
            this.db = db;
            userDetail = currentSession.CurrentUserDetail;
        }

        public string GetSchoolEvents(string SearchStr)
        {
            string JsonResult = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchStr, typeof(System.String), "_SearchStr"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId")
            };
            DataSet ds = db.GetDataset("sp_schoolevents_Sel", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                JsonResult = JsonConvert.SerializeObject(ds);
            else
                JsonResult = JsonConvert.SerializeObject(new List<string>() { });
            return JsonResult;
        }

        public string AllSubjectService(SearchModal searchModal)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid")
            };

            DataSet ds = db.GetDataset("sp_Subject_SelFilter", param);
            OutParam = JsonConvert.SerializeObject(ds);
            return OutParam;
        }

        public string SaveAssignedTimetableService(AssignTeacher ObjAssignTeacher)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjAssignTeacher.TimetableUid, typeof(System.String), "_TimetableUid"),
                new DbParam(ObjAssignTeacher.ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                new DbParam(null, typeof(System.String), "_PeriodUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
                new DbParam(ObjAssignTeacher.SubstitutedFacultyUid, typeof(System.String), "_SubstitutedFacultiUid"),
                new DbParam(ObjAssignTeacher.FacultyUid, typeof(System.String), "_FacultyUid"),
                new DbParam(ObjAssignTeacher.SubjectUid, typeof(System.String), "_SubjectUid"),
                new DbParam(ObjAssignTeacher.Period, typeof(System.Int32), "_Period"),
                new DbParam(ObjAssignTeacher.DayName, typeof(System.String), "_WeekDayName"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
            };
            OutParam = db.ExecuteNonQuery("sp_Timetable_Modify", param, true);
            return OutParam;
        }

        public string SaveSchoolCalendarChangesService(SchoolCalendar ObjSchoolCalendar)
        {
            ServiceResult ObjServiceResult = validateModalService.ValidateModalFieldsService(typeof(SchoolCalendar), ObjSchoolCalendar);
            if (ObjServiceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                new DbParam(ObjSchoolCalendar.SchoolCalendarId, typeof(System.String), "_SchoolEventUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid"),
                new DbParam(ObjSchoolCalendar.Title, typeof(System.String), "_Title"),
                new DbParam(ObjSchoolCalendar.Description, typeof(System.String), "_Description"),
                new DbParam(ObjSchoolCalendar.StartDate, typeof(System.DateTime), "_StartDate"),
                new DbParam(ObjSchoolCalendar.EndDate, typeof(System.DateTime), "_EndDate"),
                new DbParam(ObjSchoolCalendar.StartTime, typeof(System.String), "_StartTime"),
                new DbParam(ObjSchoolCalendar.EndTime, typeof(System.String), "_EndTime"),
                new DbParam(ObjSchoolCalendar.IsFullDayEvent, typeof(System.Boolean), "_IsFullDayEvent"),
                new DbParam(ObjSchoolCalendar.BindUrl, typeof(System.String), "_BindUrl"),
                new DbParam(userDetail.UserId, typeof(System.String), "_adminid")
                };
                OutParam = db.ExecuteNonQuery("sp_schoolevents_InsUpd", param, true);
            }
            return OutParam;
        }

        public string FacultyWidthSubjectService(string SubjectUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(SubjectUid, typeof(System.String), "_SubjectUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid")
            };
            DataSet ds = db.GetDataset("sp_Faculty_BySubject", param);
            OutParam = JsonConvert.SerializeObject(ds);
            return OutParam;
        }

        public string ApplicationTimeSettingService(TimeSettingModal timeSettingModal)
        {
            bool IsFormUpdate = timeSettingModal.IsUpdate;
            List<RuleBook> ruleBooks = timeSettingModal.RuleBookDetail;
            if (timeSettingModal.TimingDetails.Count > 0 && ruleBooks.Count > 0)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(timeSettingModal.SchoolOtherDetailUid, typeof(System.String), "_SchoolOtherDetailUid"),
                    new DbParam(ruleBooks[0].RulebookUid, typeof(System.String), "_RulebookUid"),
                    new DbParam(ruleBooks[0].RuleName, typeof(System.String), "_RuleName"),
                    new DbParam(timeSettingModal.TotalPeriods, typeof(System.String), "_TotalNoOfPeriods"),
                    new DbParam(timeSettingModal.SchoolStartTime, typeof(System.String), "_SchoolStartTime"),
                    new DbParam(timeSettingModal.LunchAfterPeriod, typeof(System.Int32), "_LunchAfterPeriod"),
                    new DbParam(timeSettingModal.LunchTime, typeof(System.String), "_LunchBreakTime"),
                    new DbParam(timeSettingModal.LunchDuration, typeof(System.String), "_LunchBreakDuration"),
                    new DbParam(timeSettingModal.TimingDescription, typeof(System.String), "_RuleDescription"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
                    new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
                };
                DataSet RuleBookDs = db.GetDataset("sp_TimetableSetting_InsUpd", param);
                if (RuleBookDs != null && RuleBookDs.Tables.Count > 0)
                {
                    string TimetableRuleUid = "";
                    string LunchRuleUid = "";
                    int RuleCode = 0;
                    foreach (DataRow row in RuleBookDs.Tables[0].Rows)
                    {
                        RuleCode = Convert.ToInt32(row["RuleCode"]);
                        if (RuleCode == 1)
                        {
                            TimetableRuleUid = row["RulebookUid"].ToString();
                        }
                        else if (RuleCode == 2)
                        {
                            LunchRuleUid = row["RulebookUid"].ToString();
                        }
                    }

                    if (!IsFormUpdate)
                    {
                        int i = 1;
                        timeSettingModal.TimingDetails.ForEach(item =>
                        {
                            if (item.TimingFor.ToLower() == "lunch")
                                item.RulebookUid = LunchRuleUid;
                            else
                                item.RulebookUid = TimetableRuleUid;
                            item.TimingDetailUid = item.TimingDetailUid + i++;
                            item.AdminId = userDetail.UserId;
                        });
                    }
                    //if (beanContext == null) beanContext = BeanContext.GetInstance();
                    //DataSet ds = beanContext.ConvertToDataSet<TimingModal>(timeSettingModal.TimingDetails);
                    //if (ds.Tables[0].Rows.Count > 0)
                    //    db.InsertUpdateBatchRecord("sp_TimingDetail_InsUpd", ds.Tables[0], true);

                    foreach (var Item in timeSettingModal.TimingDetails)
                    {
                        param = new DbParam[]
                        {
                            new DbParam(Item.TimingDetailUid, typeof(System.String), "_TimingDetailUid"),
                            new DbParam(Item.RulebookUid, typeof(System.String), "_RulebookUid"),
                            new DbParam(Item.TimingFor, typeof(System.String), "_TimingFor"),
                            new DbParam(Item.DurationInHrs, typeof(System.Int32), "_DurationInHrs"),
                            new DbParam(Item.DurationInMin, typeof(System.Int32), "_DurationInMin"),
                            new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
                        };
                        OutParam = db.ExecuteNonQuery("sp_TimingDetail_InsUpd", param, true);
                    }
                }
            }

            return OutParam;
        }

        public DataSet GetApplicationSettingService()
        {
            ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid")
            };
            DataSet ds = db.GetDataset("sp_TimetableSetting_Sel", param);
            if (ds != null && ds.Tables.Count == 3 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].TableName = "SchoolOtherDetail";
                ds.Tables[1].TableName = "Rulebook";
                ds.Tables[2].TableName = "TimingDetail";
                ResultSet = ds;
            }
            return ResultSet;
        }

        public string TimetableByFilterService(string ClassDetailUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid")
            };
            DataSet ds = db.GetDataset("sp_Timetable_ByFilter", param);
            if (ds != null && ds.Tables.Count == 4)
            {
                ds.Tables[0].TableName = "TimingDetail";
                ds.Tables[1].TableName = "TimetableInfo";
                ds.Tables[2].TableName = "SchoolTimetableDetail";
                ds.Tables[3].TableName = "FacultyWithSubjectDetail";
                OutParam = JsonConvert.SerializeObject(ds);
            }
            return OutParam;
        }

        public string GetFacultyWithSubjectService()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid")
            };
            DataSet ds = db.GetDataset("sp_FacultyWithSubject_Sel", param);
            if (ds != null && ds.Tables.Count == 1)
            {
                ds.Tables[0].TableName = "FacultyWithSubjectDetail";
                OutParam = JsonConvert.SerializeObject(ds);
            }
            return OutParam;
        }

        public string AllocateSubjectService(TimetableAllocationModal timetableAllocationModal)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(timetableAllocationModal.TimetableUid, typeof(System.String), "_TimetableUid"),
                new DbParam(timetableAllocationModal.ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentUid"),
                new DbParam(timetableAllocationModal.RulebookUid, typeof(System.String), "_RulebookUid"),
                new DbParam(timetableAllocationModal.SubstitutedFacultiUid, typeof(System.String), "_SubstitutedFacultiUid"),
                new DbParam(timetableAllocationModal.FacultyUid, typeof(System.String), "_FacultyUid"),
                new DbParam(timetableAllocationModal.SubjectUid, typeof(System.String), "_SubjectUid"),
                new DbParam(timetableAllocationModal.Period, typeof(System.Int32), "_Period"),
                new DbParam(timetableAllocationModal.WeekDayNum, typeof(System.Int32), "_WeekDayNum"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
            };
            OutParam = db.ExecuteNonQuery("sp_Timetable_InsUpt", param, true);
            return OutParam;
        }
    }
}
