using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Flags;
using BottomhalfCore.Services.Code;
using CommonModal.Models;
using CommonModal.ORMModels;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TenentId")
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TenentUid")
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid"),
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_tenentUid"),
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TenentUid")
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
                    new DbParam(timeSettingModal.PeriodDurationInMinutes, typeof(System.String), "_PeriodDurationInMinutes"),
                    new DbParam(timeSettingModal.SchoolStartTime, typeof(System.String), "_SchoolStartTime"),
                    new DbParam(timeSettingModal.LunchAfterPeriod, typeof(System.Int32), "_LunchAfterPeriod"),
                    new DbParam(timeSettingModal.LunchTime, typeof(System.String), "_LunchBreakTime"),
                    new DbParam(timeSettingModal.LunchDuration, typeof(System.String), "_LunchBreakDuration"),
                    new DbParam(timeSettingModal.TimingDescription, typeof(System.String), "_RuleDescription"),
                    new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid"),
                    new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
                };
                var TimetableRuleUid = db.ExecuteNonQuery("sp_TimetableSetting_InsUpd", param, true);
                if (!string.IsNullOrEmpty(TimetableRuleUid))
                {
                    int i = 1;
                    if (IsFormUpdate)
                    {
                        DbParam[] timeSettingParam = new DbParam[]
                        {
                            new DbParam(TimetableRuleUid, typeof(System.String), "_RulebookUid")
                        };
                        DataSet TimeSettingResultSet = db.GetDataset("sp_Rulebook_GetByUid", param);
                        if (TimeSettingResultSet.Tables.Count > 0)
                        {
                            var TimingDetailModel = Converter.ToList<TimingModal>(TimeSettingResultSet.Tables[0]);
                            timeSettingModal.TimingDetails.ForEach(item =>
                            {
                                item.RulebookUid = TimetableRuleUid;
                                item.TimingDetailUid = TimingDetailModel.Where(x => x.TimingFor == item.TimingFor).First().TimingDetailUid;
                                item.AdminId = userDetail.UserId;
                            });
                        }
                    }
                    else
                    {
                        timeSettingModal.TimingDetails.ForEach(item =>
                        {
                            item.RulebookUid = TimetableRuleUid;
                            item.TimingDetailUid = item.TimingDetailUid + i++;
                            item.AdminId = userDetail.UserId;
                        });
                    }

                    var TimeSettingDataSet = Converter.ToDataSet<TimingModal>(timeSettingModal.TimingDetails);
                    if (TimeSettingDataSet != null)
                        db.InsertUpdateBatchRecord("sp_TimingDetail_InsUpd", TimeSettingDataSet.Tables[0]);
                }
            }

            return OutParam;
        }

        public DataSet GetApplicationSettingService()
        {
            ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid")
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid")
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid")
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
                new DbParam(userDetail.TenentId, typeof(System.String), "_TanentUid"),
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

        public string DeleteSchoolPeriodSettingService(RuleBook ruleBook)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ruleBook.RulebookUid, typeof(System.String), "_rulebookUid")
            };
            OutParam = db.ExecuteNonQuery("sp_SchoolPeriodTiming_DelById", param, false);
            return OutParam;
        }
    }
}
