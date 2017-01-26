using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SqlFileExecutor.Constants;


namespace SqlFileExecutor.Helpers
{
    public static class SqlHelper
    {
        public static void SqlInfoExecutor(string connString, string sqlToExecute)
        {
            var errorList = new List<string>();
            using (var connection = new SqlConnection(connString))
            {
                var sqlBatched = BatchFileHelper.GetBatchStatementList(sqlToExecute);
                foreach (var batch in sqlBatched)
                {
                    try
                    {
                        using (var command = new SqlCommand(batch, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(string.Format(ErrorStrings.ErrorText, ex.Message, connection.ConnectionString, batch));
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            if (errorList.Count > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, errorList.ToArray()));
            }
        }

        public static void SqlConnectionTest(string connString)
        {

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
            }
        }
    }
}
