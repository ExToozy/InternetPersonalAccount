using Dapper;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using ViewLayer.Models;

namespace ViewLayer.Repositories
{
    public class TariffRepository
    {
        public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";
        public void Create(Tariff tariff)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO [Tariffs] ([Name], [Price], [ConnectionSpeed]) VALUES (@Name, @Price, @ConnectionSpeed)";
                dbConnection.Execute(sqlQuery, tariff);
            }
        }

        public void Delete(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM [Tariffs] WHERE Id = @id";
                dbConnection.Execute(sqlQuery, new { id });
            }
        }

        public Tariff Get(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Tariff>("SELECT * FROM [Tariffs] WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public IEnumerable<Tariff> GetAll()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Tariff>("SELECT * FROM [Tariffs]").ToList();
            }
        }
    }
}
