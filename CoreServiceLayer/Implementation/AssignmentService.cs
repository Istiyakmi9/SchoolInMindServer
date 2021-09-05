using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CoreServiceLayer.Interface;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CoreServiceLayer.Implementation
{
    public class AssignmentService : CurrentUserObject, IAssignmentService
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public AssignmentService(IDb db, ValidateModalService validateModalService, CurrentSession currentSession)
        {
            this.db = db;
            this.validateModalService = validateModalService;
            userDetail = currentSession.CurrentUserDetail;
        }
        public string GetAssignments(SearchModal searchModal)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_tenentId")
            };

            DataSet ds = db.GetDataset("sp_Assignment_SelByFilter", param);
            return JsonConvert.SerializeObject(ds);
        }
    }
}
