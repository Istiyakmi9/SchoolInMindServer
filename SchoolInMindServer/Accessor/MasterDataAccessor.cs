using BottomhalfCore.CacheManagement;
using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.DatabaseLayer.MySql.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Accessor
{
    public class MasterDataAccessor : IAccessor<MasterDataAccessor>
    {
        private readonly IDb db;
        public MasterDataAccessor(Db db)
        {
            this.db = db;
        }
        public string KeyName { get; set; }

        public Object LoadData()
        {
            DataSet Result = null;
            //Result = db.GetDataset("sp_GetIEducationMasterData");
            return Result;
        }
    }
}
