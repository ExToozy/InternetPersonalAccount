using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.Linq;
using System.Windows.Documents;
using ViewLayer.Models;

namespace ViewLayer.Repositories
{
    public class TrafficRepository
    {
        public string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb;";

        public void Create(Traffic traffic, User user)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO [Traffic] ([UserId],[Ip], [Date], [UseTimeDuration]) VALUES ({user.Id}, @Ip, @Date, @UseTimeDuration)";
                dbConnection.Execute(sqlQuery, traffic);
            }
        }

        public void Delete(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM [Traffic] WHERE Id = @id";
                dbConnection.Execute(sqlQuery, new { id });
            }
        }

        public int GetUseTimeDurationSum(string ip, int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.ExecuteScalar<int>("SELECT Sum([UseTimeDuration]) FROM [Traffic] WHERE [UserId] = @id AND [Ip] = @ip", new { id, ip });
            }
        }
        public (string, Dictionary<string, string>) GetFilteredListByDate(int userId,string date)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                var sqlQuery = 
                    $"TRANSFORM Sum([UseTimeDuration]) " +
                    $"SELECT [UserId], [Date], Sum([UseTimeDuration])" +
                    $"FROM [Traffic] " +
                    $"WHERE [UserId]={userId} AND [Date] Like '%{date}%' " +
                    $"GROUP BY [UserId], [Date] " +
                    $"PIVOT [Ip]";
                var query = dbConnection.Query(sqlQuery);
                Dictionary<string, string> ipDict = new Dictionary<string, string>();
                IDictionary<string, object> resultDictionary = new Dictionary<string,object>();
                string totalSum = "0";
                (string, Dictionary<string, string>) data = (totalSum, ipDict);
                foreach (var rawData in query)
                {
                    foreach(var item in (IDictionary<string, object>)rawData)
                    {
                        if (item.Value != null)
                        {
                            if ((!resultDictionary.Keys.Contains(item.Key)) && item.Key != "Expr1004")
                            {
                                resultDictionary.Add(item.Key, item.Value);
                            }
                            else if ((resultDictionary.Keys.Contains(item.Key)) && (item.Key != "Expr1004" && item.Key != "UserId" && item.Key != "Date"))
                            {
                                resultDictionary[item.Key] = (Convert.ToInt32(resultDictionary[item.Key]) + Convert.ToInt32(item.Value)).ToString();
                            }
                            if (item.Key == "Expr1004")
                            {
                                totalSum = (Convert.ToInt32(totalSum) + Convert.ToInt32(item.Value)).ToString();
                            }
                        }
                    }
                }
                if (resultDictionary.Count > 0)
                {
                    foreach (var item in resultDictionary)
                    {
                        if ((item.Key != "Expr1004" && item.Key != "UserId" && item.Key != "Date") && item.Value != null)
                        {
                            ipDict.Add(item.Key.ToString().Replace("_", "."), item.Value.ToString());
                        }
                    }
                    data = (totalSum, ipDict);
                }
                return data;
            }
        }

        public IEnumerable<Traffic> GetAllByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Traffic>("SELECT * FROM [Traffic] WHERE [UserId] = @id", new { id }).ToList();
            }
        }
        public Traffic GetLastByUserId(int id)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Traffic>("SELECT * FROM [Traffic] WHERE [UserId] = @id", new { id }).LastOrDefault();
            }
        }
        public IEnumerable<Traffic> GetAll()
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                return dbConnection.Query<Traffic>("SELECT * FROM [Traffic]");
            }
        }
    }
}
