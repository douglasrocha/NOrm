using NOrm.Action;
using NOrm.Enums;
using NOrm.Exceptions;
using NOrm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace NOrm.Controller
{
    public class ORMContext
    {
        private DbConnection conn;

        public ORMContext(DbConnection connection)
        {
            this.conn = connection;
        }

        public bool GetById<T>(ref T entity) where T : BaseEntity
        {
            var entityType = entity.GetType();
            
            var query = QueryParserService.GetSelectByIdQuery(entity);
            var dataTableEntity = DatabaseService.ExecuteQuery(conn, query);

            if (dataTableEntity.Rows.Count > 0)
            {
                EntityMapperService.BindByColumnName(dataTableEntity.Rows[0], ref entity);
                return true;
            }

            return false;
        }

        public List<T> GetAll<T>() where T : BaseEntity
        {
            List<T> entitiesOutput = new List<T>();

            var query = QueryParserService.GetSelectAllQuery<T>();
            var dataTableEntity = DatabaseService.ExecuteQuery(conn, query);

            foreach (DataRow row in dataTableEntity.Rows)
            {
                var obj = ReflectionService.CreateObjectWithEmptyConstructor<T>();
                EntityMapperService.BindByColumnName(row, ref obj);
                entitiesOutput.Add(obj);
            }

            return entitiesOutput;
        }

        public bool Save(BaseEntity entity)
        {
            string query = string.Empty;

            // Obtém query de acordo com o record state registrado na entidade
            switch (entity.recordState)
            {
                case RecordState.Insert:
                    query = QueryParserService.GetInsertQuery(entity);
                    break;
                
                case RecordState.Update:
                    query = QueryParserService.GetUpdateByIdQuery(entity);
                    break;

                case RecordState.Delete:
                    query = QueryParserService.GetDeleteByIdQuery(entity);
                    break;

                default:
                    return false;
            }
            
            // Inicializa transação para controle de query em apenas uma linha
            var transaction = conn.BeginTransaction();
            var lines = DatabaseService.ExecuteNonQuery(conn, query, transaction);

            if (lines == 1)
            {
                transaction.Commit();
                return true;
            }

            transaction.Rollback();
            return false;
        }
    }
}
