using BottomhalfCore.Annotations;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CoreServiceLayer.Implementation
{
    [Transient]
    public class UserDefineMapping : IMapper<UserDefineMapping>
    {
        #region UserDefine mapping

        public dynamic ConvertToObject(DataTable table, Type ExpectedObjectType, out string OperationMessage)
        {
            Object NewObject = null;
            string ColumnName = null;
            PropertyInfo property = null;
            NewObject = Activator.CreateInstance(ExpectedObjectType);
            if (NewObject != null && table.Rows.Count > 0)
            {
                if (table.Rows.Count == 1)
                {
                    DataRow dr = table.Rows[0];
                    foreach (DataColumn column in table.Columns)
                    {
                        ColumnName = (column.ColumnName[0]).ToString().ToUpper() + column.ColumnName.Substring(1, column.ColumnName.Length - 1);
                        property = ExpectedObjectType.GetProperty(ColumnName, BindingFlags.Instance | BindingFlags.Public);

                        if (property != null)
                        {
                            if (dr[column] == DBNull.Value)
                                property.SetValue(NewObject, null);
                            else if (dr[column].GetType() == typeof(System.UInt64))
                            {
                                System.UInt64 Value = Convert.ToUInt64(dr[column]);
                                if (Value == 1)
                                    property.SetValue(NewObject, true);
                                else
                                    property.SetValue(NewObject, false);
                            }
                            else
                                property.SetValue(NewObject, dr[column]);
                        }
                    }
                }
                else
                {
                    OperationMessage = "Datatable container multiple records. Please call ListObject version of current function";
                    return null;
                }
            }

            OperationMessage = "100";
            return NewObject;
        }

        public dynamic ConvertListToObject(DataTable table, Type ExpectedObjectType, out string OperationMessage)
        {
            Object NewObject = null;
            string ColumnName = null;
            PropertyInfo property = null;
            IList<dynamic> DynamicListObject = new List<dynamic>();

            if (NewObject != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    NewObject = null;
                    NewObject = Activator.CreateInstance(ExpectedObjectType);
                    foreach (DataColumn column in table.Columns)
                    {
                        ColumnName = (column.ColumnName[0]).ToString().ToUpper() + column.ColumnName.Substring(1, column.ColumnName.Length - 1);
                        property = ExpectedObjectType.GetProperty(ColumnName, BindingFlags.Instance | BindingFlags.Public);

                        if (property != null)
                        {
                            if (dr[column] == DBNull.Value)
                                property.SetValue(NewObject, null);
                            else
                                property.SetValue(NewObject, dr[column]);
                        }
                    }

                    DynamicListObject.Add(NewObject);
                }
            }

            OperationMessage = "Object created for class" + ExpectedObjectType.FullName + " Successfully";
            return NewObject;
        }

        #endregion
    }
}
