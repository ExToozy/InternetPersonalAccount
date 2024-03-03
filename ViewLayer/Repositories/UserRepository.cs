using Dapper;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using ViewLayer.Models;


namespace ViewLayer.Repositories
{
    public class UserRepository
    {
        public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public void Create(User user)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO [User] ([Fullname], [PhoneNumber], [Password]) VALUES (@Fullname, @PhoneNumber, @Password)";
                dbConnection.Execute(sqlQuery, user);
                sqlQuery = "SELECT [Id] FROM [User] WHERE [PhoneNumber] = @PhoneNumber";
                int? userId = dbConnection.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;
                sqlQuery = $"INSERT INTO [PersonalAccount] ([UserId], [LastSuccessfullPaymentId], [TariffsId]) VALUES ({user.Id}, null, null)";
                dbConnection.Execute(sqlQuery);
            }
        }
        public string GetEmailAdress(int userId)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "SELECT Email FROM [User] WHERE [Id] = @userId";
                return dbConnection.Query<string>(sqlQuery, new { userId }).FirstOrDefault();
            }
        }

        public string ChangeMailingStatus(int userId, int status)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "UPDATE [User] SET [MailingIsOn] = @status WHERE [Id] = @userId";
                return dbConnection.Query<string>(sqlQuery, new { status, userId }).FirstOrDefault();
            }
        }

        public void ConnectOrChangeEmailAdress(int userId,string email)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "UPDATE [User] SET [Email] = @email WHERE [Id] = @userId";
                dbConnection.Execute(sqlQuery, new { email, userId });
            }
        }
        public void RefillBalance(int userId, int refillValue)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "UPDATE [User] SET [Balance] = ([Balance] + @refillValue) WHERE [Id] = @userId";
                dbConnection.Execute(sqlQuery, new { refillValue, userId });
            }
        }

        public User CheckExist(string phoneNumber)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "SELECT * FROM [User] WHERE [PhoneNumber] = @phoneNumber";
                return dbConnection.Query<User>(sqlQuery, new { phoneNumber }).FirstOrDefault();
            }
        }

        public User Get(string phoneNumber, string password)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<User>("SELECT * FROM [User] WHERE [PhoneNumber] = @phoneNumber AND [Password] = @password", new { phoneNumber, password }).FirstOrDefault();
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<User>("SELECT * FROM User");
            }
        }
    }
}
