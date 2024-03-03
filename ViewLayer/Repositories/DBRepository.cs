using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace ViewLayer.Repositories
{
    public class DBRepository
    {
        public string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";
        public void CreateTable(
            int countFields,
            string tableName,
            string fieldOneName,
            string fieldTwoName,
            string fieldThreeName,
            string fieldFourName,
            List<string> paramsFieldOne,
            List<string> paramsFieldTwo,
            List<string> paramsFieldThree,
            List<string> paramsFieldFour,
            string bindingTableName)
        {
            string primaryKeyFieldName = "";
            List<string> primaryKeyFieldParams = new List<string>() { };

            string sqlQuery = $"CREATE TABLE [{tableName}] (";
            string sqlBindingQuery = "";
            string sqlBindingPrimaryKey = "";
            if (countFields >= 2)
            {
                sqlQuery += $"[{fieldOneName}] ";
                foreach (string param in paramsFieldOne)
                {
                    sqlQuery += $"{param} ";
                    if (param == "PRIMARY KEY")
                    {
                        primaryKeyFieldName = fieldOneName;
                        primaryKeyFieldParams = paramsFieldOne;
                    }
                }
                sqlQuery += ", ";
                sqlQuery += $"[{fieldTwoName}] ";
                foreach (string param in paramsFieldTwo)
                {
                    sqlQuery += $"{param} ";
                    if (param == "PRIMARY KEY")
                    {
                        primaryKeyFieldName = fieldTwoName;
                        primaryKeyFieldParams = paramsFieldTwo;
                    }
                }
                if (countFields >= 3)
                {
                    sqlQuery += ", ";
                    sqlQuery += $"[{fieldThreeName}] ";
                    foreach (string param in paramsFieldThree)
                    {
                        sqlQuery += $"{param} ";
                        if (param == "PRIMARY KEY")
                        {
                            primaryKeyFieldName = fieldThreeName;
                            primaryKeyFieldParams = paramsFieldThree;
                        }
                    }
                }
                if (countFields == 4)
                {
                    sqlQuery += ", ";
                    sqlQuery += $"[{fieldFourName}] ";
                    foreach (string param in paramsFieldFour)
                    {
                        sqlQuery += $"{param} ";
                        if (param == "PRIMARY KEY")
                        {
                            primaryKeyFieldName = fieldFourName;
                            primaryKeyFieldParams = paramsFieldFour;
                        }
                    }
                }
                if (bindingTableName != "" && primaryKeyFieldName != "")
                {
                    sqlBindingQuery = $"ALTER TABLE [{bindingTableName}] ADD [{tableName}_{primaryKeyFieldName}] {primaryKeyFieldParams[0]}";
                    sqlBindingPrimaryKey = $"ALTER TABLE [{bindingTableName}] ADD FOREIGN KEY ([{tableName}_{primaryKeyFieldName}]) REFERENCES [{tableName}]({primaryKeyFieldName})";
                }
                sqlQuery += " )";
            }
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute(sqlQuery);
                if (sqlBindingPrimaryKey != "")
                {
                    dbConnection.Execute(sqlBindingQuery);
                    dbConnection.Execute(sqlBindingPrimaryKey);
                }
            }

        }

        public DataView GetAllRecordsByTableName(string tableName)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlQuery = $"SELECT * FROM [{tableName}]";
                DataSet table = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, dbConnection);
                adapter.Fill(table);
                return table.Tables[0].DefaultView;
            }
        }

        public void DeleteRecordFromTableNameById(int id, string tableName)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlQuery = $"DELETE FROM [{tableName}] WHERE [Id] = {id}";

                dbConnection.Execute(sqlQuery);
            }
        }

        public DataTable GetColumsNameByTableName(string tableName)
        {

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlQuery = $"SELECT * FROM [{tableName}] WHERE [Id] = 0";
                DataSet table = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, dbConnection);
                adapter.Fill(table);
                return table.Tables[0];
            }
        }

        public void AddRecord(DataTable record, string tableName)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlLastIdQuery = $"SELECT MAX([Id]) FROM [{tableName}]";
                int lastId = dbConnection.ExecuteScalar<int>(sqlLastIdQuery);
                string sqlQuery = $"INSERT INTO [{tableName}] ( [Id], ";
                for (int i = 1; i < record.Columns.Count - 1; i++)
                {
                    sqlQuery += $"[{record.Columns[i].ColumnName}], ";
                }
                sqlQuery += $"[{record.Columns[record.Columns.Count - 1].ColumnName}] ) VALUES ( {lastId + 1}, ";
                for (int i = 1; i < record.Columns.Count - 1; i++)
                {
                    if (record.Rows[0][record.Columns[i].ColumnName] is int)
                    {
                        sqlQuery += $"{record.Rows[0][record.Columns[i].ColumnName]}, ";
                    }
                    else
                    {
                        sqlQuery += $"'{record.Rows[0][record.Columns[i].ColumnName]}', ";
                    }
                }
                if (record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName] is int)
                {
                    sqlQuery += $"{record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName]} )";
                }
                else
                {
                    sqlQuery += $"'{record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName]}' )";
                }
                dbConnection.Execute(sqlQuery);
            }
        }

        public DataTable GetObjectInfoFromDb(string tableName, int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlQuery = $"SELECT * FROM [{tableName}] WHERE [Id] = {id}";
                DataSet table = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, dbConnection);
                adapter.Fill(table);
                return table.Tables[0];
            }
        }

        public void EditRecord(string tableName, DataTable record)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                string sqlQuery = $"UPDATE [{tableName}] SET ";
                for (int i = 1; i < record.Columns.Count - 1; i++)
                {
                    if (record.Rows[0][record.Columns[i].ColumnName] is int)
                    {
                        sqlQuery += $"[{record.Columns[i].ColumnName}] = {record.Rows[0][record.Columns[i].ColumnName]}, ";
                    }
                    else
                    {
                        sqlQuery += $"[{record.Columns[i].ColumnName}] = '{record.Rows[0][record.Columns[i].ColumnName]}', ";
                    }
                }
                if (record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName] is int)
                {
                    sqlQuery +=
                        $"[{record.Columns[record.Columns.Count - 1].ColumnName}] = {record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName]} " +
                        $"WHERE [Id] = {record.Rows[0][record.Columns[0].ColumnName]}";
                }
                else
                {
                    sqlQuery +=
                        $"[{record.Columns[record.Columns.Count - 1].ColumnName}] = '{record.Rows[0][record.Columns[record.Columns.Count - 1].ColumnName]}' " +
                        $"WHERE [Id] = {record.Rows[0][record.Columns[0].ColumnName]}";
                }

                dbConnection.Execute(sqlQuery);
            }
        }

        public List<string> GetAllTableName()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {

                List<string> tableNames = new List<string>();
                dbConnection.Open();
                DataTable foreignKeys = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, "User" });
                DataTable tbls = dbConnection.GetSchema("Tables", new string[] { null, null, null, "TABLE" });

                foreach (DataRow row in tbls.Rows)
                {
                    string TableName = row["TABLE_NAME"].ToString();
                    tableNames.Add(TableName);

                }
                return tableNames;
            }
        }

        public void DeleteTable(string tableName)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                DataTable foreignKeys = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] { null, null, tableName });

                string sqlDeleteTableQuery = $"DROP TABLE [{tableName}]";
                string sqlDeleteRelationshipQuery = "";
                string sqlDeleteForeignKeysQuery = "";

                if (foreignKeys.Rows.Count > 0)
                {
                    foreach (DataRow row in foreignKeys.Rows)
                    {
                        string relationshipName = row["FK_NAME"].ToString();
                        string tableWithRelationship = row["FK_TABLE_NAME"].ToString();
                        sqlDeleteRelationshipQuery = $"ALTER TABLE [{tableWithRelationship}] DROP CONSTRAINT [{relationshipName}]";
                        dbConnection.Execute(sqlDeleteRelationshipQuery);
                    }
                    foreach (DataRow row in foreignKeys.Rows)
                    {
                        string foreignKeyFieldName = row["FK_COLUMN_NAME"].ToString();
                        string tableWithRelationship = row["FK_TABLE_NAME"].ToString();
                        sqlDeleteForeignKeysQuery = $"ALTER TABLE [{tableWithRelationship}] DROP [{foreignKeyFieldName}]";
                        dbConnection.Execute(sqlDeleteForeignKeysQuery);
                    }
                }
                dbConnection.Execute(sqlDeleteTableQuery);
            }

        }
    }
}
