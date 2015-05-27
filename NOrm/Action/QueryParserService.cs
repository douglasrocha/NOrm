using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NOrm.Attributes;
using NOrm.Exceptions;
using NOrm.Model;

namespace NOrm.Action
{
    public static class QueryParserService
    {
        private static string ParseValueToCompatibleQueryString(object fieldValue)
        {
            var typeField = fieldValue.GetType();

            // Verifica tipo do valor do campo e retorna valor parseado para inclusão
            // em qualquer query
            switch (Type.GetTypeCode(typeField))
            {
                case TypeCode.String:
                    return String.Format("'{0}'", fieldValue);

                case TypeCode.DateTime:
                    return string.Format("'{0}'", ((DateTime)fieldValue).ToString("dd/MM/yyyy"));

                case TypeCode.Boolean:
                    return (bool)fieldValue ? "1" : "0";

                default:
                    return fieldValue.ToString();
            }
        }

        private static String GetEntityName(object obj)
        { 
            // Verifica se objeto em questão é uma entidade válida
            if (!EntityService.IsEntity(obj))
            {
                throw new InvalidEntityException();
            }

            var entityAnnotation = Attribute.GetCustomAttribute(obj.GetType(), typeof(ORMEntity));

            return (entityAnnotation as ORMEntity).Name;
        }

        private static string GetColumnsSeparatedByComma(object obj)
        {
            // Lista com todos os nomes das colunas no banco de dados
            var columnNames = new List<string>();

            if (!EntityService.IsEntity(obj))
            {
                throw new InvalidEntityException();
            }

            // Obtém todas as propriedades do objeto
            var objProperties = obj.GetType().GetProperties();

            foreach (var currentProperty in objProperties)
            {
                // Obtém atributo de tipo coluna da propriedade atual da iteração
                var currentAttribute = Attribute.GetCustomAttribute(currentProperty, typeof(ORMColumn));

                // Caso encontre a propriedade atual, adiciona-o a lista de nomes de colunas
                if (currentAttribute != null)
                {
                    columnNames.Add((currentAttribute as ORMColumn).Name);
                }
            }

            return string.Join(", ", columnNames);
        }

        private static string GetPrimaryKeyConditions(object obj)
        {
            var listPrimaryKeyProperties = new List<PropertyInfo>();
            var listPrimaryKeyConditions = new List<string>();

            if (!EntityService.IsEntity(obj))
            {
                throw new InvalidEntityException();
            }

            var objProperties = obj.GetType().GetProperties();

            // Obtém todas propriedades que são chaves primárias
            foreach (var currentProperty in objProperties)
            {
                var currentAttribute = Attribute.GetCustomAttribute(currentProperty, typeof(ORMPrimaryKey));

                if (currentAttribute != null)
                {
                    listPrimaryKeyProperties.Add(currentProperty);
                }
            }

            // Obtém todas as condições de igualdade das chaves primárias
            foreach (var currentKeyProperty in listPrimaryKeyProperties)
            {
                var currentKeyColumnAttribute = Attribute.GetCustomAttribute(currentKeyProperty, 
                                                                             typeof(ORMColumn));

                // Retorna exceção caso tenha atributo de chave primária
                // mas não tenha atributo de coluna também
                if (currentKeyColumnAttribute == null)
                {
                    throw new InvalidEntityException();
                }

                var columnName = (currentKeyColumnAttribute as ORMColumn).Name;
                var columnValue = currentKeyProperty.GetValue(obj, null);
                var columnCondition = string.Format("{0} = {1}", 
                                                    columnName, 
                                                    ParseValueToCompatibleQueryString(columnValue));

                listPrimaryKeyConditions.Add(columnCondition);
            }

            return string.Join(" AND ", listPrimaryKeyConditions);
        }

        private static string GetValuesSeparatedByComma(object obj, Boolean isInsert)
        {
            var columnValues = new List<string>();

            if (!EntityService.IsEntity(obj))
            {
                throw new InvalidEntityException();
            }

            var objProperties = obj.GetType().GetProperties();

            foreach (var currentProperty in objProperties)
            {
                var currentAttribute = Attribute.GetCustomAttribute(currentProperty, typeof(ORMColumn));
                var currentValue = currentProperty.GetValue(obj, null);
                var currentPrimaryKeyAttribute = Attribute.GetCustomAttribute(currentProperty, 
                                                                              typeof(ORMPrimaryKey));

                if (currentPrimaryKeyAttribute == null)
                {
                    columnValues.Add(ParseValueToCompatibleQueryString(currentValue));
                }
                else
                {
                    ORMPrimaryKey pkAttribute = (ORMPrimaryKey) currentPrimaryKeyAttribute;

                    columnValues.Add
                    (
                        pkAttribute.AutoIncrement && isInsert ? " " 
                                                              : ParseValueToCompatibleQueryString(currentValue)
                    );
                }
            }

            return string.Join(", ", columnValues);
        }

        private static string GetColumnsEqualToValuesSeparatedByComma(Object obj)
        {
            var columnAllKVP = new List<string>();

            if (!EntityService.IsEntity(obj))
            {
                throw new InvalidEntityException();
            }

            var objProperties = obj.GetType().GetProperties();

            foreach (var currentProperty in objProperties)
            {
                var currentAttribute = Attribute.GetCustomAttribute(currentProperty, typeof(ORMColumn));

                if (currentAttribute == null)
                {
                    continue;
                }

                var columnName = (currentAttribute as ORMColumn).Name;
                var columnValue = currentProperty.GetValue(obj, null);
                var columnKVP = string.Format("{0} = {1}", 
                                              columnName, 
                                              ParseValueToCompatibleQueryString(columnValue));

                columnAllKVP.Add(columnKVP);
            }

            return string.Join(", ", columnAllKVP);
        }

        public static String GetSelectByIdQuery(object obj)
        {
            return string.Format("SELECT {0} FROM {1} WHERE {2}",
                                 GetColumnsSeparatedByComma(obj),
                                 GetEntityName(obj),
                                 GetPrimaryKeyConditions(obj));
        }

        public static String GetSelectAllQuery<T>() where T : BaseEntity
        {
            var obj = ReflectionService.CreateObjectWithEmptyConstructor<T>();

            return string.Format("SELECT {0} FROM {1}",
                                 GetColumnsSeparatedByComma(obj),
                                 GetEntityName(obj));
        }

        public static String GetInsertQuery(object obj)
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                                 GetEntityName(obj),
                                 GetColumnsSeparatedByComma(obj),
                                 GetValuesSeparatedByComma(obj, true));
        }

        public static String GetUpdateByIdQuery(object obj)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                                 GetEntityName(obj),
                                 GetColumnsEqualToValuesSeparatedByComma(obj),
                                 GetPrimaryKeyConditions(obj));
        }

        public static String GetDeleteByIdQuery(object obj)
        {
            return string.Format("DELETE FROM {0} WHERE {1}",
                                 GetEntityName(obj),
                                 GetPrimaryKeyConditions(obj));
        }
    }
}
