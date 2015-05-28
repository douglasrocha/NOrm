using NOrm.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NOrm.Action
{
    public static class DatabaseService
    {
        private static DbCommand CreateDatabaseCommand(DbConnection conn, string query)
                                                       
        {
            var connectionTypeName = conn.GetType().Name;
            DbCommand cmd;

            switch (connectionTypeName)
            {
                case "MySqlConnection":
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(query);
                    break;

                case "OracleConnection":
                    cmd = new Oracle.DataAccess.Client.OracleCommand(query);
                    break;

                case "SqlConnection":
                    cmd = new System.Data.SqlClient.SqlCommand(query);
                    break;

                case "FbConnection":
                    cmd = new FirebirdSql.Data.FirebirdClient.FbCommand(query);
                    break;

                default:
                    return null;
            }

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandText = query;

            return cmd;
        }

        public static int ExecuteNonQuery(DbConnection conn, string query, DbTransaction transaction = null)
        {
            int lines = -1;
            bool closeTransaction = false;

            try
            {
                if (conn == null)
                {
                    throw new ConnectionException("A conexão solicitada é nula");
                }

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                if (transaction == null)
                {
                    closeTransaction = true;
                    transaction = conn.BeginTransaction();
                }

                var cmd = CreateDatabaseCommand(conn, query);
                cmd.Transaction = transaction;

                lines = cmd.ExecuteNonQuery();

                if (closeTransaction)
                {
                    transaction.Commit();
                    transaction = null;
                }

                return lines;
            }
            catch (Exception ex)
            {
                if (closeTransaction)
                {
                    transaction.Rollback();
                }

                return -1;
            }
        }

        public static DataTable ExecuteQuery(DbConnection conn, string query)
        {
            try
            {
                if (conn == null)
                {
                    throw new ConnectionException("A conexão solicitada é nula");
                }

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var cmd = CreateDatabaseCommand(conn, query);

                return cmd.ExecuteReader().GetSchemaTable();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
