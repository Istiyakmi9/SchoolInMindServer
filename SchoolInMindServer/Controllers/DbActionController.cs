using BottomhalfCore.FactoryContext;
using BottomhalfCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class DbActionController : ControllerBase
    {
        // GET: DbAction
        private readonly BeanContext context;
        public DbActionController()
        {
            this.context = BeanContext.GetInstance();
        }
        public ActionResult TableAction()
        {
            //ViewBag.Title = "Home Page";
            return null;
        }

        [HttpGet]
        public string GetDatabaseSchema(string Database, string Username, string Password, bool? IntegratedSecurity, string Provider)
        {
            string OutCome = null;
            string ServerPath = null; // Server.MapPath("~");
            ServerPath = Path.Combine(ServerPath, "cachedData.txt");
            DataSet ds = null;
            if (!System.IO.File.Exists(ServerPath))
            {
                ds = null; //  Get database table full schema
                if (ds != null && ds.Tables.Count == 2 && ds.Tables[0].Rows.Count > 0)
                {
                    List<ConstraintTableName> Data = (from n in ds.Tables[0].AsEnumerable()
                                                      group n by n.Field<string>("TableName") into g
                                                      let rows = g
                                                      select new ConstraintTableName
                                                      {
                                                          TableName = rows.Key,
                                                          ObjConstraintValues = (from m in rows
                                                                                 where m.Field<string>("TableName") == rows.Key
                                                                                 select new ConstraintValues
                                                                                 {
                                                                                     ColumnName = m.Field<string>("ColumnName"),
                                                                                     ConstraintName = m.Field<string>("ConstraintName"),
                                                                                     ReferencedColumnName = m.Field<string>("ReferencedColumnName"),
                                                                                     ReferencedTableName = m.Field<string>("ReferencedTableName"),
                                                                                 }).ToList()
                                                      }).OrderBy(x => x.TableName).ToList<ConstraintTableName>();

                    List<TableSchemaDetail> Tables = (from n in ds.Tables[1].AsEnumerable()
                                                      group n by n.Field<string>("TableName") into g
                                                      let rows = g
                                                      select new TableSchemaDetail
                                                      {
                                                          TableName = g.Key,
                                                          ObjTableDetail = (from k in g
                                                                            where k.Field<string>("TableName") == g.Key
                                                                            select new TableDetail
                                                                            {
                                                                                ColumnName = k.Field<string>("ColumnName"),
                                                                                IsNullable = k.Field<string>("IsNullable"),
                                                                                Datatype = k.Field<string>("Datatype"),
                                                                                Length = k.Field<string>("Length")
                                                                            }).ToList()

                                                      }).OrderBy(x => x.TableName).ToList<TableSchemaDetail>();

                    OutCome = JsonConvert.SerializeObject(new { Schema = Data, TableSchema = Tables });
                    if (!System.IO.File.Exists(ServerPath))
                        System.IO.File.Create(ServerPath).Dispose();
                    using (TextWriter write = new StreamWriter(ServerPath))
                    {
                        write.WriteLine(OutCome); write.Close();
                    }
                }
            }
            else
            {
                using (TextReader reader = new StreamReader(ServerPath))
                {
                    OutCome = reader.ReadToEnd(); reader.Close();
                }
            }

            return OutCome;
        }

        public class TableSchemaDetail
        {
            public string TableName { set; get; }
            public List<TableDetail> ObjTableDetail { set; get; }
        }

        public class TableDetail
        {
            public string ColumnName { set; get; }
            public string IsNullable { set; get; }
            public string Datatype { set; get; }
            public string Length { set; get; }
        }

        public class ConstraintTableName
        {
            public string TableName { set; get; }
            public List<ConstraintValues> ObjConstraintValues { set; get; }
        }

        public class ConstraintValues
        {
            public string ConstraintName { set; get; }
            public string ColumnName { set; get; }
            public string ReferencedTableName { set; get; }
            public string ReferencedColumnName { set; get; }
        }

        [HttpGet]
        public string GetTableSchemaView()
        {
            //var Result = PartialView("tableschema");
            //return Result;
            return null;
        }
    }
}