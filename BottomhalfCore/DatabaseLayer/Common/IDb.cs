using BottomhalfCore.DatabaseLayer.Common.Code;
using System;
using System.Collections.Generic;
using System.Data;

namespace BottomhalfCore.DatabaseLayer.Common.Code
{
    public interface IDb
    {
        DataSet FetchResult(string ProcedureName);
        /*===========================================  GetDataSet =============================================================*/
        DataSet GetDataset(string ProcedureName, DbParam[] param);
        DataSet GetDataset(string ProcedureName);
        DataSet GetDataset(string ProcedureName, DbParam[] param, bool OutParam, ref string ProcessingStatus);
        Object ExecuteSingle(string ProcedureName, DbParam[] param, bool OutParam);
        string ExecuteNonQuery(string ProcedureName, DbParam[] param, bool OutParam);
        void UserDefineTypeBulkInsert(DataSet dataset, string ProcedureName, bool OutParam);
        string InsertUpdateBatchRecord(string ProcedureName, DataTable table, Boolean OutParam = false);
        DataSet CommonadBuilderBulkInsertUpdate(string SelectQuery, string TableName);
        List<T> GetDataSet<T>(string ProcedureName, DbParam[] param, ref string PrcessingStatus, bool OutParam = false);
        (int, int) InsertUpdateJsonBatch(string ProcedureName, DataTable table);

        T Get<T>(string ProcedureName);
        T Get<T>(string ProcedureName, DbParam[] parameters, bool IsOutputParamter);
    }
}
