using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class ChatService : CurrentUserObject, IChatService
    {
        private readonly IDb db;
        public Object GetChatUniqueService(ChatKeys objChatKeys)
        {
            Object chatkey = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(objChatKeys.FirstUserKey, typeof(System.String), "_userKey"),
                new DbParam(objChatKeys.SecondUserKey, typeof(System.String), "_sndUserKey"),
            };

            chatkey = db.ExecuteSingle("sp_GetChatKey", param, true);
            return chatkey;
        }

        public string GetParentNameService(string FacultyUid)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(FacultyUid, typeof(System.String), "_facultyUid")
            };

            DataSet ds = db.GetDataset("sp_GetChattingParentNames", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }
    }
}
