using NOrm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Action
{
    public static class ConversionService
    {
        public static dynamic ConvertValue(string strValue, TypeCode toTypeCode)
        {
            switch (toTypeCode)
            {
                case TypeCode.Boolean:
                    return Convert.ToBoolean(strValue);

                case TypeCode.Byte:
                    return Convert.ToByte(strValue);

                case TypeCode.Char:
                    return Convert.ToChar(strValue);

                case TypeCode.DateTime:
                    return Convert.ToDateTime(strValue);

                case TypeCode.Decimal:
                    return Convert.ToDecimal(strValue);

                case TypeCode.Double:
                    return Convert.ToDouble(strValue);

                case TypeCode.Int16:
                    return Convert.ToInt16(strValue);

                case TypeCode.Int32:
                    return Convert.ToInt32(strValue);

                case TypeCode.Int64:
                    return Convert.ToInt64(strValue);

                case TypeCode.UInt16:
                    return Convert.ToUInt16(strValue);

                case TypeCode.UInt32:
                    return Convert.ToUInt32(strValue);

                case TypeCode.UInt64:
                    return Convert.ToUInt64(strValue);

                case TypeCode.String:
                    return strValue;

                default:
                    return null;
            }
        }

    }
}
