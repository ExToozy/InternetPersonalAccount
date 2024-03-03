using Dapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using ViewLayer.Models;

namespace ViewLayer.Repositories
{
    public class PersonalAccountRepository
    {
        public string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public void DeleteByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM [PersonalAccount] WHERE [UserId] = @id";
                dbConnection.Execute(sqlQuery, new { id });

            }
        }

        public void ConnectTariffToUser(Tariff tariff, User user)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = $"UPDATE [PersonalAccount] SET TariffsId = {tariff.Id} WHERE [UserId] = {user.Id}";
                dbConnection.Execute(sqlQuery);
                sqlQuery = $"UPDATE [User] SET [Balance] = ([Balance] - {tariff.Price}) WHERE [Id] = {user.Id}";
                dbConnection.Execute(sqlQuery);
                sqlQuery = $"INSERT INTO [Payments] ([UserId], [DateOnly], [Status], [Value]) VALUES ({user.Id}, '{DateTime.Now.ToShortDateString()}', 'Cнятие', {tariff.Price})";
                dbConnection.Execute(sqlQuery);

            }
        }

        public PersonalAccount GetByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<PersonalAccount>("SELECT * FROM [PersonalAccount] WHERE [UserId] = @id", new { id }).FirstOrDefault();
            }
        }

        public IEnumerable<PersonalAccount> GetAll()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<PersonalAccount>("SELECT * FROM [PersonalAccount]").ToList();
            }
        }
    }
}
