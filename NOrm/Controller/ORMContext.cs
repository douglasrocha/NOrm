using NOrm.Action;
using NOrm.Enums;
using NOrm.Exceptions;
using NOrm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NOrm.Controller
{
    public static class ORMContext
    {
        public static bool GetById<T>(ref T entity) where T : BaseEntity
        {
            var entityType = entity.GetType();
            var query = QueryParserService.GetSelectByIdQuery(entity);

            // TODO: Executar query no banco
            var dataTableEntity = new DataTable(); // db.retrieveSql(query)

            if (dataTableEntity.Rows.Count > 0)
            {
                EntityMapperService.BindByColumnName(dataTableEntity.Rows[0], ref entity);
                return true;
            }

            return false;
        }

        public static List<T> GetAll<T>() where T : BaseEntity
        {
            List<T> entitiesOutput = new List<T>();
            var query = QueryParserService.GetSelectAllQuery<T>();

            // TODO: Executa query no banco e retorna datatable
            var dataTableEntity = new DataTable(); // db.retrieveSql(query)

            foreach (DataRow row in dataTableEntity.Rows)
            {
                var obj = ReflectionService.CreateObjectWithEmptyConstructor<T>();
                EntityMapperService.BindByColumnName(row, ref obj);
                entitiesOutput.Add(obj);
            }

            return entitiesOutput;
        }

        public static void Save(BaseEntity entity)
        {
            string query = string.Empty;

            switch (entity.recordState)
            {
                case RecordState.insert:
                    query = QueryParserService.GetInsertQuery(entity);
                    break;
                
                case RecordState.update:
                    query = QueryParserService.GetUpdateByIdQuery(entity);
                    break;

                case RecordState.delete:
                    query = QueryParserService.GetDeleteByIdQuery(entity);
                    break;
            }


        }
    }
}
