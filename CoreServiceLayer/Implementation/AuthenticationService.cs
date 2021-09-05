using AuthenticationToken;
using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Services.Code;
using BottomhalfCore.Services.Interface;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Interface;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreServiceLayer.Implementation
{
    public class AuthenticationService : CurrentUserObject, IAuthenticationService<AuthenticationService>
    {
        private IAutoMapper<TableAutoMapper> autoMapper;
        private readonly IDb db;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager _JwtTokenManager;

        public AuthenticationService(IDb db,
            TableAutoMapper autoMapper,
            IConfiguration configuration,
            IJwtTokenManager JwtTokenManager)
        {
            this.db = db;
            _JwtTokenManager = JwtTokenManager;
            this.configuration = configuration;
            this.autoMapper = autoMapper;
        }

        public DataSet GetUserObject(AuthUser objAuthUser)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objAuthUser.MobileNo, typeof(System.String), "_mobile"),
                new DbParam(objAuthUser.Password, typeof(System.String), "_password"),
                new DbParam(objAuthUser.SchoolTenentId, typeof(System.String), "_schooltenentId"),
                new DbParam(objAuthUser.IsFaculty, typeof(System.Boolean), "_isFaculty")
            };
            DataSet ds = db.GetDataset("sp_MobileUserMaster_Detail", param);
            if (ds != null && ds.Tables.Count == 3)
            {
                ds.Tables[0].TableName = "LoginUser";
                ds.Tables[1].TableName = "Student";
                ds.Tables[2].TableName = "Notification";
            }
            else
            {
                ds = null;
            }
            return ds;
        }

        public (DataSet, string) GetLoginUserObject(AuthUser authUser)
        {
            string Token = null;
            string ProcessingData = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(authUser.UserId, typeof(System.String), "_mobile"),
                new DbParam(authUser.Password, typeof(System.String), "_password")
            };
            DataSet ds = db.GetDataset("sp_GetLoginUser_Detail", param, true, ref ProcessingData);
            if (ds != null && ds.Tables.Count > 0)
            {
                this.autoMapper = new TableAutoMapper();
                UserDetail userDetail = this.autoMapper.AutoMapToObject<UserDetail>(ds.Tables[0]);
                if (userDetail != null)
                {
                    UserToken userToken = new UserToken
                    {
                        Email = userDetail.Email,
                        UserId = userDetail.UserId,
                        Mobile = userDetail.MobileNo,
                        TenentId = userDetail.schooltenentId,
                        Role = Policies.Admin
                    };
                    Token = _JwtTokenManager.GenerateToken<UserToken>(userToken.UserId, userDetail.Email, userToken);
                }
            }

            if (ds.Tables.Count == 8)
            {
                ds.Tables[0].TableName = "CurrentUser";
                ds.Tables[1].TableName = "Classes";
                ds.Tables[2].TableName = "TotalCount";
                ds.Tables[3].TableName = "Menu";
                ds.Tables[4].TableName = "Subject";
                ds.Tables[5].TableName = "Roles";
                ds.Tables[6].TableName = "ColumnMapping";
                ds.Tables[7].TableName = "StateNCity";
            }
            return (ds, Token);
        }

        public bool ValidateMobileNo(string Mobile, string TenentId, bool IsStudent)
        {
            bool State = false;
            DbParam[] param = new DbParam[]
            {
                new DbParam(Mobile, typeof(System.String), "_Mobileno"),
                new DbParam(TenentId, typeof(System.String), "_TenentId"),
                new DbParam(IsStudent, typeof(System.Boolean), "_Student")
            };

            var OutCome = (string)db.ExecuteSingle("sp_VerifyMobileno", param, true);
            if (OutCome != "0")
                State = true;
            return State;
        }

        public void LogoutUserService()
        {
            //beanContext.Logout();
        }
    }
}