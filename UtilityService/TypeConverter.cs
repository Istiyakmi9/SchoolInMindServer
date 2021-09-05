using System;

namespace UtilityService
{
    public class TypeConverter
    {
        public static dynamic ConvertTo(dynamic Value, Type DataType)
        {
            switch (DataType.FullName)
            {
                case "System.Int16":
                    return Convert.ToInt16(Value);
                case "System.Int32":
                    return Convert.ToInt16(Value);
                case "System.Int64":
                    return Convert.ToInt64(Value);
                case "System.Single":
                    return Convert.ToSingle(Value);
                case "System.Decimal":
                    return Convert.ToDecimal(Value);
                case "System.Boolean":
                    return Convert.ToBoolean(Value);
                case "System.Char":
                    return Convert.ToChar(Value);
                case "System.Double":
                    return Convert.ToDouble(Value);
                case "System.DateTime":
                    return Convert.ToDateTime(Value);
                case "System.Byte":
                    return Convert.ToByte(Value);
                default:
                    return Convert.ToString(Value);
            }
        }
    }
}
