﻿using BottomhalfCore.DatabaseLayer.Common.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BottomhalfCore.DatabaseLayer.MsSql.Code
{
    public class Db : IDb
    {
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private SqlDataAdapter da = null;
        private DataSet ds = null;
        private SqlCommandBuilder builder = null;

        public Db(string ConnectionString)
        {
            StabilizeConnection(ConnectionString);
        }

        public void StabilizeConnection(string ConnectionString)
        {
            if (cmd != null)
                cmd.Parameters.Clear();
            if (ConnectionString != null)
            {
                con = new SqlConnection();
                cmd = new SqlCommand();
                con.ConnectionString = ConnectionString;
            }
        }

        public DataSet FetchResult(string ProcedureName)
        {
            ds = null;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;
            da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /*===========================================  GetDataSet =============================================================*/

        #region GetDataSet
        public DataSet GetDataset(string ProcedureName, DbParam[] param)
        {
            try
            {
                ds = null;
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd = AddCommandParameter(cmd, param);
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return ds;
        }

        public DataSet GetDataset(string ProcedureName)
        {
            try
            {
                ds = null;
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
            }
            finally
            {
                con.Close();
            }

            return ds;
        }

        public DataSet GetDataset(string ProcedureName, DbParam[] param, bool OutParam, ref string PrcessingStatus)
        {
            try
            {
                ds = null;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                {
                    cmd.Parameters.Add("@ProcessingResult", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                }

                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                if (OutParam)
                    PrcessingStatus = cmd.Parameters["@ProcessingResult"].Value.ToString();
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
                    cmd.Parameters.Add("@ProcessingResult", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                }

                con.Open();
                OutPut = cmd.ExecuteScalar();
                if (OutParam)
                    OutPut = cmd.Parameters["@ProcessingResult"].Value;
            }
            catch (SqlException ex)
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
                cmd = AddCommandParameter(cmd, param);
                if (OutParam)
                {
                    cmd.Parameters.Add("@ProcessingResult", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                }

                con.Open();
                var result = cmd.ExecuteNonQuery();
                if (OutParam)
                    state = cmd.Parameters["@ProcessingResult"].Value.ToString();
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

        public void UserDefineTypeBulkInsert(DataSet dataset, string ProcedureName, bool OutParam)
        {
            int state = 0;
            string fileId = DateTime.Now.Ticks.ToString();
            try
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    if (dr["MedicineName"].ToString().Trim().Length == 0)
                    {
                        break;
                    }
                    cmd.Parameters.Clear();
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("_Tanent", "NAN"),
                        new SqlParameter("_Medicine_Id", DateTime.Now.Ticks.ToString()),
                        new SqlParameter("_Description", dr["Description"]),
                        new SqlParameter("_FileUploadKey", fileId),
                        new SqlParameter("_MedicineName", dr["MedicineName"]),
                        new SqlParameter("_MRP", dr["MRP"]),
                        new SqlParameter("_Company", dr["Company"]),
                        new SqlParameter("_Batch", dr["Batch"]),
                        new SqlParameter("_WholesalePrice", dr["WholesalePrice"]),
                        new SqlParameter("_Discount", dr["Discount"]),
                        new SqlParameter("_ExpiryDate", dr["ExpiryDate"]),
                        new SqlParameter("_MedicineType", dr["MedicineType"]),
                        new SqlParameter("_IsPrescribed", dr["IsPrescribed"]),
                        new SqlParameter("_Active", dr["Active"]),
                        new SqlParameter("_Quantity", dr["Quantity"]),
                        new SqlParameter("_Alternet_Medicine", dr["Alternet_Medicine"]),
                        new SqlParameter("_Type", dr["Type"]),
                        new SqlParameter("_Tablet_Power", dr["Tablet_Power"]),
                        new SqlParameter("_SaleOption", dr["SaleOption"]),
                        new SqlParameter("_Img_Src", dr["Img_Src"]),
                        new SqlParameter("_ItemType", 1000),
                        new SqlParameter("_AddLinks", dr["AddLinks"]),
                    };

                    //cmd = AddParameters(parameters);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProcedureName;
                    cmd.Connection = con;
                    if (OutParam)
                    {
                        cmd.Parameters.Add("@ProcessingResult", SqlDbType.Int).Direction = ParameterDirection.Output;
                    }

                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();
                    state = cmd.ExecuteNonQuery();
                    if (OutParam)
                        state = Convert.ToInt32(cmd.Parameters["@ProcessingResult"].Value);
                }
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

        public List<T> GetDataSet<T>(string ProcedureName, DbParam[] param, ref string PrcessingStatus, bool OutParam = false)
        {
            return null;
        }

        public class DbTypeDetail
        {
            public SqlDbType DbType { set; get; }
            public int Size { set; get; }
        }

        private DbTypeDetail GetType(Type ColumnType)
        {
            if (ColumnType == typeof(System.String))
                return new DbTypeDetail { DbType = SqlDbType.VarChar, Size = 250 };
            else if (ColumnType == typeof(System.Int64))
                return new DbTypeDetail { DbType = SqlDbType.BigInt, Size = 8 };
            else if (ColumnType == typeof(System.Int32))
                return new DbTypeDetail { DbType = SqlDbType.Int, Size = 4 };
            else if (ColumnType == typeof(System.Char))
                return new DbTypeDetail { DbType = SqlDbType.VarChar, Size = 1 };
            else if (ColumnType == typeof(System.DateTime))
                return new DbTypeDetail { DbType = SqlDbType.DateTime, Size = 10 };
            else if (ColumnType == typeof(System.Double))
                return new DbTypeDetail { DbType = SqlDbType.BigInt, Size = 8 };
            else if (ColumnType == typeof(System.Boolean))
                return new DbTypeDetail { DbType = SqlDbType.Bit, Size = 1 };
            else
                return new DbTypeDetail { DbType = SqlDbType.VarChar, Size = 250 };
        }

        public string InsertUpdateBatchRecord(string ProcedureName, DataTable table, Boolean OutParam = false)
        {
            try
            {
                string state = "";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.UpdatedRowSource = UpdateRowSource.None;

                foreach (DataColumn column in table.Columns)
                {
                    var DbType = this.GetType(column.DataType);
                    cmd.Parameters.Add("_" + column.ColumnName, DbType.DbType, DbType.Size, column.ColumnName);
                }
                da = new SqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 2;
                con.Open();
                int Count = da.Update(table);
                if (OutParam)
                    state = cmd.Parameters["_ProcessingResult"].Value.ToString();
                //status = da.Update()
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

        public DataSet CommonadBuilderBulkInsertUpdate(string SelectQuery, string TableName)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (int, int) InsertUpdateJsonBatch(string ProcedureName, DataTable table)
        {
            return (0, 0);
        }

        #endregion

        #region Common

        public string GenerateSchema()
        {
            //Server server = new Server("XXX");
            //Database database = new Database();
            //database = server.Databases["YYY"];
            //Table table = database.Tables["ZZZ", @"PPP"];
            //StringCollection result = table.Script();
            //var script = "";
            //foreach (var line in result)
            //{
            //    script += line;
            //}

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

        public SqlCommand AddCommandParameter(SqlCommand cmd, DbParam[] param)
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
                            if (p.Type == typeof(System.Guid))
                            {
                                Guid guid = Guid.Empty;
                                if (!string.IsNullOrEmpty(p.Value.ToString()))
                                    guid = Guid.Parse(p.Value.ToString());
                                cmd.Parameters.Add(p.ParamName, SqlDbType.UniqueIdentifier).Value = guid;
                            }
                            else if (p.Type == typeof(System.String))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.VarChar, p.Size).Value = Convert.ToString(p.Value);
                            else if (p.Type == typeof(System.Int16))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Int).Value = Convert.ToInt16(p.Value);
                            else if (p.Type == typeof(System.Int32))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Int).Value = Convert.ToInt32(p.Value);
                            else if (p.Type == typeof(System.Double))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Float).Value = Convert.ToDouble(p.Value);
                            else if (p.Type == typeof(System.Int64))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.BigInt).Value = Convert.ToInt64(p.Value);
                            else if (p.Type == typeof(System.Char))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Char, 1).Value = Convert.ToChar(p.Value);
                            else if (p.Type == typeof(System.Decimal))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.BigInt).Value = Convert.ToDecimal(p.Value);
                            else if (p.Type == typeof(System.DBNull))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.BigInt).Value = Convert.DBNull;
                            else if (p.Type == typeof(System.Boolean))
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Bit).Value = Convert.ToBoolean(p.Value);
                            else if (p.Type == typeof(System.DateTime))
                            {
                                if (Convert.ToDateTime(p.Value).Year == 1)
                                    cmd.Parameters.Add(p.ParamName, SqlDbType.DateTime).Value = Convert.ToDateTime("1/1/1976");
                                else
                                    cmd.Parameters.Add(p.ParamName, SqlDbType.DateTime).Value = Convert.ToDateTime(p.Value);
                            }
                            else if (p.Type == null)
                                cmd.Parameters.Add(p.ParamName, SqlDbType.Structured).Value = p.Value;
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

        #endregion
    }

}
