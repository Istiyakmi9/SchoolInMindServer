using BottomhalfCore.FactoryContext;
using BottomhalfCore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BottomhalfCore.Services.Code
{
    public class TableAutoMapper : IAutoMapper<TableAutoMapper>
    {
        #region UserDefine mapping

        private readonly BeanContext context;
        public TableAutoMapper()
        {
            context = BeanContext.GetInstance();
        }

        public List<T> MapTo<T>(DataTable table)
        {
            Type ExpectedType = typeof(T);
            Object NewObject = null;
            string ColumnName = null;
            PropertyInfo property = null;
            List<T> DynamicListObject = new List<T>();
            NewObject = context.GetBean<T>();

            if (NewObject != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    NewObject = null;
                    NewObject = Activator.CreateInstance(ExpectedType);
                    foreach (DataColumn column in table.Columns)
                    {
                        ColumnName = (column.ColumnName[0]).ToString().ToUpper() + column.ColumnName.Substring(1, column.ColumnName.Length - 1);
                        property = ExpectedType.GetProperty(ColumnName, BindingFlags.Instance | BindingFlags.Public);

                        if (property != null)
                        {
                            if (dr[column] == DBNull.Value)
                                property.SetValue(NewObject, null);
                            else
                                property.SetValue(NewObject, dr[column]);
                        }
                    }

                    DynamicListObject.Add((T)NewObject);
                }
            }

            return DynamicListObject;
        }

        public dynamic AutoMapToObject<T>(DataTable table)
        {
            Type ExpectedType = typeof(T);
            Object NewObject = null;
            string ColumnName = null;
            PropertyInfo property = null;
            NewObject = context.GetBean<T>();
            if (NewObject != null && table.Rows.Count > 0)
            {
                if (table.Rows.Count == 1)
                {
                    DataRow dr = table.Rows[0];
                    foreach (DataColumn column in table.Columns)
                    {
                        ColumnName = column.ColumnName;
                        property = ExpectedType.GetProperty(ColumnName, BindingFlags.Instance | BindingFlags.Public);

                        if (property != null)
                        {
                            if (dr[column] == DBNull.Value)
                                property.SetValue(NewObject, null);
                            else
                                property.SetValue(NewObject, dr[column]);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            return NewObject;
        }

        public dynamic AutoMapToObjectList<T>(DataTable table)
        {
            Type ExpectedType = typeof(T);
            Object NewObject = null;
            string ColumnName = null;
            PropertyInfo property = null;
            IList<T> DynamicListObject = new List<T>();
            NewObject = context.GetBean<T>();
            if (NewObject != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    NewObject = null;
                    NewObject = Activator.CreateInstance(ExpectedType);
                    foreach (DataColumn column in table.Columns)
                    {
                        ColumnName = (column.ColumnName[0]).ToString().ToUpper() + column.ColumnName.Substring(1, column.ColumnName.Length - 1);
                        property = ExpectedType.GetProperty(ColumnName, BindingFlags.Instance | BindingFlags.Public);

                        if (property != null)
                        {
                            if (dr[column] == DBNull.Value)
                                property.SetValue(NewObject, null);
                            else
                                property.SetValue(NewObject, dr[column]);
                        }
                    }

                    DynamicListObject.Add((T)NewObject);
                }
            }

            return DynamicListObject;
        }

        #endregion
    }
}