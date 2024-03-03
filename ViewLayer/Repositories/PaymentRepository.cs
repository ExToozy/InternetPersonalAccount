using Dapper;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using ViewLayer.Models;

namespace ViewLayer.Repositories
{
    public class PaymentRepository
    {
        public string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public void Create(Payment payment)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO [Payments] ([UserId], [DateOnly], [Status], [Value]) VALUES ({payment.UserId}, '{payment.DateOnly}', '{payment.Status}', {payment.Value})";
                dbConnection.Execute(sqlQuery);
                sqlQuery = $"SELECT [Id] FROM [Payments] WHERE [UserId] = {payment.UserId}";
                int? paymentId = dbConnection.Query<int>(sqlQuery, payment).LastOrDefault();
                payment.Id = paymentId.Value;
                sqlQuery = $"UPDATE [PersonalAccount] SET [LastSuccessfullPaymentId] = {payment.Id} WHERE [UserId] = {payment.UserId}";
                dbConnection.Execute(sqlQuery);
            }
        }

        public Payment GetByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Payment>("SELECT * FROM [Payments] WHERE [UserId] = @id", new { id }).FirstOrDefault();
            }
        }

        public IEnumerable<Payment> GetAllByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {

                return dbConnection.Query<Payment>($"SELECT * FROM [Payments] WHERE UserId = @id", new { id });
            }
        }

        public IEnumerable<Payment> FilteredGetAllByUserId(int id, string date)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                if (date == null)
                {
                    return dbConnection.Query<Payment>($"SELECT * FROM [Payments] WHERE UserId = @id", new { id });
                }
                return dbConnection.Query<Payment>($"SELECT * FROM [Payments] WHERE UserId = @id AND DateOnly like '%{date}%'", new { id });
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Payment>("SELECT * FROM [Payments]");
            }
        }
    }
}
