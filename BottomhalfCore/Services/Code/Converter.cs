using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace BottomhalfCore.Services.Code
{
    public static class Converter
    {
        public static List<T> ToList<T>(this DataTable table)
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties)
        {
            T item = Activator.CreateInstance<T>();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                    {
                        if (property.PropertyType == typeof(bool))
                        {
                            if (row[property.Name].ToString() == "1")
                                property.SetValue(item, true, null);
                            else
                                property.SetValue(item, false, null);
                        }
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
            }
            return item;
        }
    }
}
