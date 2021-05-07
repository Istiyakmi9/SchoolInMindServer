using BottomhalfCore.CacheManagement.Caching;
using BottomhalfCore.CacheManagement.CachingInterface;
using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CoreServiceLayer.Interface;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CoreServiceLayer.Implementation
{
    public class GradeService : CurrentUserObject, IGradeService
    {
        private readonly IExceptionLogger<ExceptionLogger> exceptionLogger;
        private readonly IDb db;
        private readonly IValidateModalService<ValidateModalService> validateModalService;

        public GradeService(IDb db, ExceptionLogger exceptionLogger, CurrentSession currentSession, ValidateModalService validateModalService)
        {
            this.db = db;
            this.validateModalService = validateModalService;
            this.userDetail = currentSession.CurrentUserDetail;
            this.exceptionLogger = exceptionLogger;
        }

        public string GetGradesService(SearchModal searchModal)
        {
            var Result = db.GetDataset("sp_Grade_filter", null);
            DataSet ds = UiMappedColumn.BuildColumn<GradeDetail>(Result);
            return JsonConvert.SerializeObject(ds);
        }

        public string InsertUpdateGradeService(GradeDetail gradeDetail)
        {
            var StatusModal = validateModalService.ValidateModalFieldsService<GradeDetail>(gradeDetail);
            if (StatusModal.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                new DbParam(gradeDetail.GradeUid, typeof(System.String), "_gradeUid"),
                new DbParam(gradeDetail.Grade, typeof(System.String), "_grade"),
                new DbParam(gradeDetail.Description, typeof(System.String), "_description"),
                new DbParam(gradeDetail.MinMarks, typeof(System.Int32), "_minMarks"),
                new DbParam(gradeDetail.MaxMarks, typeof(System.Int32), "_maxMarks"),
                new DbParam(userDetail.UserId, typeof(System.String), "_adminUid")
                };
                var Result = db.ExecuteNonQuery("sp_Grade_InsUpd", param, true);
                return Result;
            }
            return "Incorrect value submitted";
        }
    }
}
