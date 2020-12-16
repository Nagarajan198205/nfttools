using Microsoft.Data.Sqlite;
using NFTIntegration.Models.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace NFTIntegration.Data
{
    public class DataAdapter
    {
        private string sqliteConnectionString;

        public DataAdapter()
        {
            sqliteConnectionString = $"Data Source={Directory.GetCurrentDirectory()}\\DB\\rAtOOn.db";
        }

        public List<ReportData> GetZapReportList()
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDataList.Add(new ReportData
                    {
                        ReportId = reader.GetInt64("ReportId"),
                        High = reader.GetInt32("High"),
                        Medium = reader.GetInt32("Medium"),
                        Low = reader.GetInt32("Low"),
                        Information = reader.GetInt32("Information"),
                        ReportFileName = reader.GetString("ReportFileName"),
                        RunDate = reader.GetString("RunDate")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public ReportData GetLastRunZapReport()
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports ORDER BY ReportId DESC LIMIT 1", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reportData.ReportId = reader.GetInt64("ReportId");
                    reportData.High = reader.GetInt32("High");
                    reportData.Medium = reader.GetInt32("Medium");
                    reportData.Low = reader.GetInt32("Low");
                    reportData.Information = reader.GetInt32("Information");
                    reportData.ReportFileName = reader.GetString("ReportFileName");
                    reportData.RunDate = reader.GetString("RunDate");
                }

                CloseConnection(sqliteConnection);
            }

            return reportData;
        }

        public ReportData GetZapReportDetails(string reportId)
        {
            var reportDetails = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports WHERE ReportId={reportId}", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDetails.ReportId = reader.GetInt64("ReportId");
                    reportDetails.High = reader.GetInt32("High");
                    reportDetails.Medium = reader.GetInt32("Medium");
                    reportDetails.Low = reader.GetInt32("Low");
                    reportDetails.Information = reader.GetInt32("Information");
                    reportDetails.ReportFileName = reader.GetString("ReportFileName");
                    reportDetails.RunDate = reader.GetString("RunDate");
                }

                CloseConnection(sqliteConnection);

            }

            return reportDetails;
        }

        public void AddReportDetails(ReportData reportData)
        {
            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"INSERT INTO ZapReports(ReportId,High,Medium,Low,Information,ReportFileName,RunDate) " +
                                                $"VALUES({reportData.ReportId},{reportData.High},{reportData.Medium},{reportData.Low}," +
                                                $"{reportData.Information},'{reportData.ReportFileName}','{reportData.RunDate}')", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        public User Login(string userName,string password)
        {
            var userDetails = new User();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT UserId,FirstName,LastName,UserName,Role FROM User WHERE UserName = '{userName}' AND Password='{password}'", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userDetails.UserId = reader.GetInt32("UserId");
                    userDetails.FirstName = reader.GetString("FirstName");
                    userDetails.LastName = reader.GetString("LastName");
                    userDetails.Username = reader.GetString("UserName");
                    userDetails.Role = reader.GetString("Role");
                }

                CloseConnection(sqliteConnection);
            }

            return userDetails;
        }

        public void AddUser(AddUser user)
        {
            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"INSERT INTO User (FirstName,LastName,UserName,Password,CreatedOn,Role) " +
                                                $"VALUES('{user.FirstName}','{user.LastName}','{user.Username}','{user.Password}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{user.Role}')", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        public List<string> GetRoles()
        {
            List<string> userRoles = new List<string>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT UserRole FROM Roles", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userRoles.Add(reader.GetString("UserRole"));
                    
                }

                CloseConnection(sqliteConnection);
            }

            return userRoles;
        }

        private void OpenConnection(SqliteConnection sqliteConnection)
        {
            if (sqliteConnection.State == ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }
        }

        private void CloseConnection(SqliteConnection sqliteConnection)
        {
            if (sqliteConnection.State == ConnectionState.Open)
            {
                sqliteConnection.Close();
            }
        }
    }
}