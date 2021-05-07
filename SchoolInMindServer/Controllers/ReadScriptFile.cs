using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInMindServer.Controllers
{
    //public class ReadScriptFile
    //{

    //}
    //public class BuildTable
    //{
    //    public BuildTable()
    //    {
    //    }

    //    public void Build(object sender, string e)
    //    {
    //        if (!string.IsNullOrEmpty(e))
    //        {
    //            if (e.IndexOf("dbo") != -1)
    //            {
    //                var PartedTableName = e.Split("[dbo].");
    //                var TableName = ReplaceAllSpecialChar(PartedTableName[PartedTableName.Length - 1]);
    //            }
    //        }
    //    }

    //    public string ReplaceAllSpecialChar(string Value)
    //    {
    //        string builder = Value;
    //        if (Value.IndexOf("(") != -1)
    //            builder = Value.Replace("(", "");
    //        if (Value.IndexOf(")") != -1)
    //            builder = Value.Replace(")", "");
    //        return builder;
    //    }
    //}
    //class Program
    //{
    //    private BuildTable buildTable;
    //    public Program()
    //    {
    //        buildTable = new BuildTable();
    //        OnTablebuildInvoked += buildTable.Build;
    //    }
    //    public event EventHandler<string> OnTablebuildInvoked;
    //    static void Main1(string[] args)
    //    {
    //        var program = new Program();
    //        Dictionary<string, string> TableDetail = new Dictionary<string, string>();
    //        StringBuilder procedureBuilder = new StringBuilder();
    //        StringBuilder functionBuilder = new StringBuilder();
    //        StringBuilder tableBuilder = new StringBuilder();
    //        Boolean IsFun = false;
    //        Boolean IsProc = false;
    //        Boolean IsTab = false;
    //        string FilePath = @"C:\Users\istayaquemd\Desktop\TERMPoint.sql";
    //        if (File.Exists(FilePath))
    //        {
    //            using (Stream stream = File.OpenRead(FilePath))
    //            {
    //                string Data = "";
    //                string SmallData = "";
    //                using (StreamReader reader = new StreamReader(stream))
    //                {
    //                    while (!reader.EndOfStream)
    //                    {
    //                        Data = reader.ReadLine();
    //                        SmallData = Data.ToLower();
    //                        if (SmallData.IndexOf("create") != -1 || IsTab || IsProc || IsFun)
    //                        {
    //                            if (SmallData.IndexOf("function") != -1 || IsFun)
    //                            {
    //                                functionBuilder.AppendLine(Data);
    //                                if (SmallData.Trim() == "end")
    //                                {
    //                                    IsFun = false;
    //                                    functionBuilder.AppendLine();
    //                                    functionBuilder.AppendLine();
    //                                    functionBuilder.AppendLine(@"/*--------------------------------------------------------------- End --------------------------------------*/");
    //                                }
    //                                else
    //                                    IsFun = true;
    //                            }
    //                            else if (SmallData.IndexOf(" table ") != -1 || IsTab)
    //                            {
    //                                program.OnTablebuildInvoked?.Invoke(program, SmallData.Trim());
    //                                if (SmallData.Trim() == "go")
    //                                {
    //                                    IsTab = false;
    //                                    tableBuilder.AppendLine();
    //                                    tableBuilder.AppendLine();
    //                                    tableBuilder.AppendLine(@"/*--------------------------------------------------------------- End --------------------------------------*/");
    //                                }
    //                                else
    //                                {
    //                                    tableBuilder.AppendLine(Data);
    //                                    IsTab = true;
    //                                }
    //                            }
    //                            else if (SmallData.IndexOf(" procedure ") != -1 || IsProc)
    //                            {
    //                                procedureBuilder.AppendLine(Data);
    //                                if (SmallData.Trim() == "end")
    //                                {
    //                                    IsProc = false;
    //                                    procedureBuilder.AppendLine();
    //                                    procedureBuilder.AppendLine();
    //                                    procedureBuilder.AppendLine(@"/*--------------------------------------------------------------- End --------------------------------------*/");
    //                                }
    //                                else
    //                                    IsProc = true;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        TableDetail.Add("Function", functionBuilder.ToString());
    //        TableDetail.Add("Table", tableBuilder.ToString());
    //        TableDetail.Add("Procedure", procedureBuilder.ToString());
    //    }
    //}
}
