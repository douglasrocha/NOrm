using NOrm.Attributes;
using NOrm.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NOrm.Action
{
    public static class EntityMapperService
    {
        private static void Bind<T>(DataRow row, ref T obj, bool isFriendlyName)
        {
            // Atira erro caso entidade não possua atributo correspondente
            if (!EntityService.IsEntity(obj))
            {
                throw new NotImplementedException();
            }

            var objProperties = obj.GetType().GetProperties();

            // Itera por todas as propriedades do objeto passado por referência
            foreach (var prop in objProperties)
            {
                var currentAttribute = Attribute.GetCustomAttribute(prop, typeof(ORMColumn));

                // Caso propriedade possua atributo de tipo coluna, altera valor
                // do mesmo
                if (currentAttribute != null)
                {
                    var columnName = (currentAttribute as ORMColumn).Name;
                    var friendlyName = (currentAttribute as ORMColumn).FriendlyName;
                    var bindName = isFriendlyName ? friendlyName : columnName;

                    var propType = prop.PropertyType;

                    if (row.Table.Columns.Contains(bindName))
                    {
                        var rowValue = row[bindName].ToString();
                        var convertedValue = ConversionService.ConvertValue(rowValue, Type.GetTypeCode(propType));
                        prop.SetValue(obj, convertedValue, null);
                    }
                }
            }
        }

        public static void BindByColumnName<T>(DataRow row, ref T obj)
        {
            Bind<T>(row, ref obj, false);
        }

        public static void BindByFriendlyName<T>(DataRow row, ref T obj)
        {
            Bind<T>(row, ref obj, true); 
        }

        public static void BindByColumnName<T>(DataTable table, ref T obj)
        {
            foreach (DataRow row in table.Rows)
            {
                Bind<T>(row, ref obj, false);
            }
        }

        public static void BindByFriendlyName<T>(DataTable table, ref T obj)
        {
            foreach (DataRow row in table.Rows)
            {
                Bind<T>(row, ref obj, true);
            }
        }
    }
}
