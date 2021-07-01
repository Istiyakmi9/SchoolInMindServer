using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Services.Code;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BottomhalfCore.DatabaseLayer.MySql.Code
{
    public class Db : IDb
    {
        private MySqlConnection con = null;
        private MySqlCommand cmd = null;
        private MySqlDataAdapter da = null;
        private MySqlCommandBuilder builder = null;
        private DataSet ds = null;

        public Db(string ConnectionString)
        {
            SetupConnection(ConnectionString);
        }

        public void SetupConnection(string ConnectionString)
        {
            if (cmd != null)
                cmd.Parameters.Clear();
            if (ConnectionString != null)
            {
                con = new MySqlConnection();
                cmd = new MySqlCommand();
                con.ConnectionString = ConnectionString;
            }
        }



        #region NEW DYNAMIC DATABASE INTERACTION CODE

        public T Get<T>(string ProcedureName)
        {
            (T Item, string result) = Execute<T>(ProcedureName, null, false);
            return Item;
        }

        public T Get<T>(string ProcedureName, DbParam[] parameters, bool IsOutputParamter)
        {
            (T Item, string result) = Execute<T>(ProcedureName, parameters, IsOutputParamter);
            return Item;
        }

        private (T, string) Execute<T>(string ProcedureName, DbParam[] param, bool OutParam)
        {
            try
            {
                string value = default(string);
                T Result = default(T);
                using (con = new MySqlConnection())
                {
                    using (cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = ProcedureName;
                        if (param != null)
                            cmd = AddCommandParameter(cmd, param);
                        if (OutParam)
                            cmd.Parameters.Add("_ProcessingResult", MySqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                        da = new MySqlDataAdapter();
                        da.SelectCommand = cmd;
                        ds = new DataSet();
                        da.Fill(ds);
                        if (OutParam)
                            value = cmd.Parameters["_ProcessingResult"].Value.ToString();
                        if (ds.Tables.Count > 0)
                        {
                            if (typeof(T) != typeof(DataSet))
                            {
                                var ResultList = Converter.ToList<T>(ds.Tables[0]);
                                Result = ResultList.FirstOrDefault();
                            }
                        }
                    }
                }

                return (Result, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open || con.State == ConnectionState.Broken)
                    con.Close();
            }
        }

        #endregion


        public DataSet FetchResult(string ProcedureName)
        {
            ds = null;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;
            da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /*===========================================  GetDataSet =============================================================*/

        #region GetDataSet
        public DataSet GetDataset(string ProcedureName, DbParam[] param)
        {
            string ProcessingStatus = null;
            return ExecuteGetDataSet(ProcedureName, param, false, ref ProcessingStatus);
        }

        public DataSet GetDataset(string ProcedureName)
        {
            string ProcessingStatus = null;
            return ExecuteGetDataSet(ProcedureName, null, false, ref ProcessingStatus);
        }

        public DataSet GetDataset(string ProcedureName, DbParam[] param, bool OutParam, ref string PrcessingStatus)
        {
            return ExecuteGetDataSet(ProcedureName, param, OutParam, ref PrcessingStatus);
        }

        public List<T> GetDataSet<T>(string ProcedureName, DbParam[] param, ref string PrcessingStatus, bool OutParam = false)
        {
            try
            {
                List<T> outputList = null;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                if (param != null)
                    cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                    cmd.Parameters.Add("_ProcessingResult", MySqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                var da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                if (OutParam)
                    PrcessingStatus = cmd.Parameters["_ProcessingResult"].Value.ToString();
                if (ds.Tables.Count > 0)
                    outputList = Converter.ToList<T>(ds.Tables[0]);

                return outputList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet ExecuteGetDataSet(string ProcedureName, DbParam[] param, bool OutParam, ref string PrcessingStatus)
        {
            try
            {
                ds = null;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                if (param != null)
                    cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                    cmd.Parameters.Add("_ProcessingResult", MySqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                if (OutParam)
                    PrcessingStatus = cmd.Parameters["_ProcessingResult"].Value.ToString();
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object ExecuteSingle(string ProcedureName, DbParam[] param, bool OutParam)
        {
            Object OutPut = null;
            try
            {
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                {
                    cmd.Parameters.Add("_ProcessingResult", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                }

                con.Open();
                OutPut = cmd.ExecuteScalar();
                if (OutParam)
                    OutPut = cmd.Parameters["_ProcessingResult"].Value;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open || con.State == ConnectionState.Broken)
                    con.Close();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open || con.State == ConnectionState.Broken)
                    con.Close();
            }
            return OutPut;
        }

        public string ExecuteNonQuery(string ProcedureName, DbParam[] param, bool OutParam)
        {
            string state = "";
            string fileId = DateTime.Now.Ticks.ToString();
            try
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.Parameters.Clear();
                cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                {
                    cmd.Parameters.Add("_ProcessingResult", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                }

                con.Open();
                var result = cmd.ExecuteNonQuery();
                if (OutParam)
                    state = cmd.Parameters["_ProcessingResult"].Value.ToString();
                else
                    state = result.ToString();
                return state;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open || con.State == ConnectionState.Broken)
                    con.Close();
            }
        }

        #endregion

        /*===========================================  Bulk Insert Update =====================================================*/

        #region Bulk Insert Update

        public class DbTypeDetail
        {
            public MySqlDbType DbType { set; get; }
            public int Size { set; get; }
        }

        private DbTypeDetail GetType(Type ColumnType)
        {
            if (ColumnType == typeof(System.String))
                return new DbTypeDetail { DbType = MySqlDbType.VarChar, Size = 250 };
            else if (ColumnType == typeof(System.Int64))
                return new DbTypeDetail { DbType = MySqlDbType.Int64, Size = 8 };
            else if (ColumnType == typeof(System.Int32))
                return new DbTypeDetail { DbType = MySqlDbType.Int32, Size = 4 };
            else if (ColumnType == typeof(System.Char))
                return new DbTypeDetail { DbType = MySqlDbType.VarChar, Size = 1 };
            else if (ColumnType == typeof(System.DateTime))
                return new DbTypeDetail { DbType = MySqlDbType.DateTime, Size = 10 };
            else if (ColumnType == typeof(System.Double))
                return new DbTypeDetail { DbType = MySqlDbType.Int64, Size = 8 };
            else if (ColumnType == typeof(System.Boolean))
                return new DbTypeDetail { DbType = MySqlDbType.Bit, Size = 1 };
            else
                return new DbTypeDetail { DbType = MySqlDbType.VarChar, Size = 250 };
        }

        #endregion

        #region Common

        public string GenerateSchema()
        {
            return "";
        }

        public SqlDbType GetParameterType(DataColumn column, out int Length)
        {
            SqlDbType type = SqlDbType.Text;
            Length = 0;
            if (column.ColumnName.GetType() == typeof(System.String))
            {
                type = SqlDbType.VarChar;
                Length = 0;
            }
            else if (column.ColumnName.GetType() == typeof(System.Int32))
            {
                type = SqlDbType.Int;
                Length = 4;
            }
            else if (column.ColumnName.GetType() == typeof(System.Double))
            {
                type = SqlDbType.Float;
                Length = 8;
            }
            else if (column.ColumnName.GetType() == typeof(System.Boolean))
            {
                type = SqlDbType.Bit;
                Length = 1;
            }
            else if (column.ColumnName.GetType() == typeof(System.DateTime))
            {
                type = SqlDbType.DateTime;
                Length = 10;
            }
            else if (column.ColumnName.GetType() == typeof(System.Int64))
            {
                type = SqlDbType.Int;
                Length = 8;
            }
            else if (column.ColumnName.GetType() == typeof(System.Char))
            {
                type = SqlDbType.VarChar;
                Length = 1;
            }
            else if (column.ColumnName.GetType() == typeof(System.Decimal))
            {
                type = SqlDbType.Decimal;
                Length = 8;
            }

            return type;
        }

        public MySqlCommand AddCommandParameter(MySqlCommand cmd, DbParam[] param)
        {
            cmd.Parameters.Clear();
            if (param != null)
            {
                foreach (DbParam p in param)
                {
                    if (p.IsTypeDefined)
                    {
                        if (p.Value != null)
                        {
                            //if (p.Type == typeof(System.Guid))
                            //{
                            //    Guid guid = Guid.Empty;
                            //    if (!string.IsNullOrEmpty(p.Value.ToString()))
                            //        guid = Guid.Parse(p.Value.ToString());
                            //    cmd.Parameters.Add(p.ParamName, MySqlDbType.UniqueIdentifier).Value = guid;
                            //}
                            //else 
                            if (p.Type == typeof(System.String))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.VarChar, p.Size).Value = Convert.ToString(p.Value);
                            else if (p.Type == typeof(System.Int16))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Int16).Value = Convert.ToInt16(p.Value);
                            else if (p.Type == typeof(System.Int32))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Int32).Value = Convert.ToInt32(p.Value);
                            else if (p.Type == typeof(System.Double))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Float).Value = Convert.ToDouble(p.Value);
                            else if (p.Type == typeof(System.Int64))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Int64).Value = Convert.ToInt64(p.Value);
                            else if (p.Type == typeof(System.Char))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.VarChar, 1).Value = Convert.ToChar(p.Value);
                            else if (p.Type == typeof(System.Decimal))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Decimal).Value = Convert.ToDecimal(p.Value);
                            else if (p.Type == typeof(System.DBNull))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Decimal).Value = Convert.DBNull;
                            else if (p.Type == typeof(System.Boolean))
                                cmd.Parameters.Add(p.ParamName, MySqlDbType.Bit).Value = Convert.ToBoolean(p.Value);
                            else if (p.Type == typeof(System.DateTime))
                            {
                                if (Convert.ToDateTime(p.Value).Year == 1)
                                    cmd.Parameters.Add(p.ParamName, MySqlDbType.DateTime).Value = Convert.ToDateTime("1/1/1976");
                                else
                                    cmd.Parameters.Add(p.ParamName, MySqlDbType.DateTime).Value = Convert.ToDateTime(p.Value);
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(p.ParamName, DBNull.Value);
                        }
                    }
                    else
                    {
                        cmd.Parameters.Add(p.ParamName, p.Value);
                    }
                }
            }
            return cmd;
        }

        public void UserDefineTypeBulkInsert(DataSet dataset, string ProcedureName, bool OutParam)
        {
            throw new NotImplementedException();
        }

        public DataSet CommonadBuilderBulkInsertUpdate(string SelectQuery, string TableName)
        {
            try
            {
                da = new MySqlDataAdapter();
                da.SelectCommand = new MySqlCommand(SelectQuery, con);
                builder = new MySqlCommandBuilder(da);
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, TableName);
                da.Update(ds, TableName);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Broken || con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        #endregion

        #region Generic New Code

        public (int, int) InsertUpdateJsonBatch(string ProcedureName, DataTable table)
        {
            try
            {
                int insertState = 0;
                int updateState = 0;
                if (table.Columns.Count == 1)
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProcedureName;
                    cmd.UpdatedRowSource = UpdateRowSource.None;
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("_JsonData", MySqlDbType.JSON);
                    cmd.Parameters.Add("_InsertStatus", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("_UpdateStatus", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    da = new MySqlDataAdapter();
                    da.InsertCommand = cmd;
                    da.UpdateBatchSize = 100;
                    con.Open();
                    int Count = da.Update(table);
                    insertState = Convert.ToInt32(cmd.Parameters["_InsertStatus"].Value);
                    updateState = Convert.ToInt32(cmd.Parameters["_UpdateStatus"].Value);
                }
                return (insertState, updateState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Broken || con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public string InsertUpdateBatchRecord(string ProcedureName, DataTable table, Boolean OutParam = false)
        {
            try
            {
                string state = null;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.UpdatedRowSource = UpdateRowSource.None;
                cmd.Parameters.Clear();

                foreach (DataColumn column in table.Columns)
                {
                    var DbType = this.GetType(column.DataType);
                    cmd.Parameters.Add("_" + column.ColumnName, DbType.DbType, DbType.Size, column.ColumnName);
                }

                if (OutParam)
                    cmd.Parameters.Add("_ProcessingResult", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                con.Open();
                int Count = da.Update(table);
                if (OutParam)
                    state = cmd.Parameters["_ProcessingResult"].Value.ToString();
                else if (Count > 0)
                    state = "Successfull";
                return state;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Broken || con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        #endregion
    }

}
