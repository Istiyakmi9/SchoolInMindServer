using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CoreServiceLayer.Implementation
{
    public class CommonService : CurrentUserObject, ICommonService<CommonService>
    {
        private readonly IDb db;

        public void ValidateFilterModal(SearchModal searchModal, string SortBy)
        {
            if (string.IsNullOrEmpty(searchModal.SearchString))
                searchModal.SearchString = " 1=1 ";
            if (string.IsNullOrEmpty(searchModal.SortBy))
            {
                if (string.IsNullOrEmpty(SortBy))
                    searchModal.SortBy = "";
                else
                    searchModal.SortBy = SortBy;
            }
            if (searchModal.PageIndex <= 0)
                searchModal.PageIndex = 1;
            if (searchModal.PageSize <= 0)
                searchModal.PageSize = 10;
        }

        public IList<string> GetAllFileNames(string DirectoryPath)
        {
            List<string> FileNames = null;
            if (Directory.Exists(DirectoryPath))
                FileNames = Directory.GetFiles(DirectoryPath).ToList<string>();
            if (FileNames != null && FileNames.Count > 0)
                FileNames = FileNames.Select(x => x.Substring(x.LastIndexOf(@"\") + 1, (x.Length - x.LastIndexOf(@"\")) - 1)).ToList<string>();
            return FileNames;
        }

        public string GetSubjectByClassSectionService(int Class, string Section)
        {
            string Result = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid"),
                new DbParam(Class, typeof(System.Int32), "_Class"),
                new DbParam(Section, typeof(System.String), "_Section")
            };

            var Ds = db.GetDataset("sp_Subjects_SelByClass", param);
            if (Ds == null || Ds.Tables.Count == 0)
                Result = JsonConvert.SerializeObject(new DataTable());
            else
                Result = JsonConvert.SerializeObject(Ds);
            return Result;
        }

        public IDictionary<string, List<string>> ValidateMobileAndEmail(string MobileNos, string EmailIds)
        {
            IDictionary<string, List<string>> ResultSet = null;
            List<string> ExistedMobileNo = new List<string>();
            List<string> ExistedEmailId = new List<string>();
            DbParam[] param = new DbParam[]
            {
                new DbParam(MobileNos, typeof(System.String), "_MobileNos"),
                new DbParam(EmailIds, typeof(System.String), "_EmailIds"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid")
            };

            var Ds = db.GetDataset("sp_Validate_MobileNos_And_EmailIds", param);
            if (Ds != null && Ds.Tables.Count > 0)
            {
                ExistedMobileNo = new List<string>();
                ExistedEmailId = new List<string>();
                if (Ds.Tables.Count == 1)
                {
                    foreach (DataRow row in Ds.Tables[0].Rows)
                    {
                        if (row["MobileNo"] != DBNull.Value)
                            ExistedMobileNo.Add(row["MobileNo"].ToString());
                    }
                }
                else if (Ds.Tables.Count == 2)
                {
                    foreach (DataRow row in Ds.Tables[0].Rows)
                    {
                        if (row["MobileNo"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(row["MobileNo"].ToString()))
                                ExistedMobileNo.Add(row["MobileNo"].ToString());
                        }
                    }

                    foreach (DataRow row in Ds.Tables[1].Rows)
                    {
                        if (row["Email"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(row["Email"].ToString()))
                                ExistedEmailId.Add(row["Email"].ToString());
                        }
                    }
                }
                ResultSet = new Dictionary<string, List<string>>();
                ResultSet.Add("mobile", ExistedMobileNo);
                ResultSet.Add("email", ExistedEmailId);
            }
            return ResultSet;
        }
    }
}
