using Microsoft.Data.Sqlite;
using NFTIntegration.Models;
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

        public List<ReportData> GetDastReportListForReport()
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    "ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
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
                        RunDate = reader.GetString("RunDate"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public List<ReportData> GetDastReportListForReport(int userId)
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"WHERE rpt.UserId = {userId} " +
                    "ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
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
                        RunDate = reader.GetString("RunDate"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public List<ReportData> GetDastReportListByProject(int projectId)
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information,ReportFileName,RunDate,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"WHERE rpt.ProjectId = {projectId} " +
                    "ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
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
                        RunDate = reader.GetString("RunDate"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public List<ReportData> GetDastReportListByProject(int userId, int projectId)
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information ,ReportFileName,RunDate,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"WHERE rpt.UserId = {userId} " +
                    $"AND rpt.ProjectId = {projectId} " +
                    "ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
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
                        RunDate = reader.GetString("RunDate"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public List<ReportData> GetDastReportList(int userId)
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"WHERE rpt.UserId = {userId} " +
                    $"GROUP BY rpt.ProjectId,pro.ProjectName", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDataList.Add(new ReportData
                    {
                        High = reader.GetInt32("High"),
                        Medium = reader.GetInt32("Medium"),
                        Low = reader.GetInt32("Low"),
                        Information = reader.GetInt32("Information"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public List<ReportData> GetDastReportList()
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information,rpt.ProjectId,pro.ProjectName " +
                    "FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"GROUP BY rpt.ProjectId,pro.ProjectName", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDataList.Add(new ReportData
                    {
                        High = reader.GetInt32("High"),
                        Medium = reader.GetInt32("Medium"),
                        Low = reader.GetInt32("Low"),
                        Information = reader.GetInt32("Information"),
                        ProjectName = reader.GetString("ProjectName"),
                        ProjectId = reader.GetInt32("ProjectId")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public ReportData GetLastRunZapReport(int projectId)
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate " +
                    "FROM ZapReports " +
                    $"WHERE ProjectId = {projectId} " +
                    "ORDER BY ReportId DESC LIMIT 1", sqliteConnection)
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

        public ReportData GetLastRunZapReport()
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information " +
                    "FROM ZapReports ", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reportData.High = reader.GetInt32("High");
                    reportData.Medium = reader.GetInt32("Medium");
                    reportData.Low = reader.GetInt32("Low");
                    reportData.Information = reader.GetInt32("Information");
                }

                CloseConnection(sqliteConnection);
            }

            return reportData;
        }

        public ReportData GetLastRunZapReport(int userId, int projectId)
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate " +
                    $"FROM ZapReports " +
                    $"WHERE ProjectId = {projectId} " +
                    $"AND UserId = {userId} " +
                    $"ORDER BY ReportId DESC LIMIT 1", sqliteConnection)
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

        public ReportData GetLastRunZapReport(string userId)
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT sum(High) as High,sum(Medium) as Medium,sum(Low) as Low,sum(Information) as Information " +
                    $"FROM ZapReports " +
                    $"WHERE UserId = {userId} ", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reportData.High = reader.GetInt32("High");
                    reportData.Medium = reader.GetInt32("Medium");
                    reportData.Low = reader.GetInt32("Low");
                    reportData.Information = reader.GetInt32("Information");
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

        public ReportData GetZapReportDetails(string reportId, int userId)
        {
            var reportDetails = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate " +
                    $"FROM ZapReports " +
                    $"WHERE ReportId={reportId} " +
                    $"AND UserId = { userId } ", sqliteConnection)
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
                var command = new SqliteCommand($"INSERT INTO ZapReports(ReportId,High,Medium,Low,Information,ReportFileName,RunDate,UserId,ProjectId) " +
                                                $"VALUES({reportData.ReportId},{reportData.High},{reportData.Medium},{reportData.Low}," +
                                                $"{reportData.Information},'{reportData.ReportFileName}','{reportData.RunDate}',{reportData.UserId},{reportData.ProjectId})", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        public User Login(string userName, string password)
        {
            var userDetails = new User();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT UserId,FirstName,LastName,UserName,rol.RoleDesc as Role " +
                                                $"FROM User usr " +
                                                $"INNER JOIN Roles rol ON usr.RoleId = rol.RoleId " +
                                                $"WHERE UserName = '{userName}' " +
                                                $"AND Password='{password}'", sqliteConnection)
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
                var command = new SqliteCommand($"INSERT INTO User (FirstName,LastName,UserName,Password,CreatedOn,RoleId) " +
                                                $"VALUES('{user.FirstName}','{user.LastName}','{user.Username}','{user.Password}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{user.RoleId}')", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        public List<UserRole> GetRoles()
        {
            var userRoles = new List<UserRole>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT RoleId, RoleDesc FROM Roles", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userRoles.Add(new UserRole
                    {
                        RoleId = reader.GetInt32("RoleId"),
                        RoleDesc = reader.GetString("RoleDesc")
                    });
                }

                CloseConnection(sqliteConnection);
            }

            return userRoles;
        }

        public void CreateProject(CreateProjectModel createProjectModel)
        {
            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {

                var command = new SqliteCommand($"INSERT INTO Project (ProjectName,StartDate,EndDate,Description) " +
                                                $"VALUES('{createProjectModel.ProjectName}','{createProjectModel.StartDate}','{createProjectModel.EndDate}','{createProjectModel.Description}'); SELECT Last_Insert_Rowid();", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };


                OpenConnection(sqliteConnection);

                var projectId = Convert.ToInt32(command.ExecuteScalar());

                command = new SqliteCommand($"INSERT INTO UserProjectMap (ProjectId,UserId) VALUES({projectId},{createProjectModel.UserId})", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        public List<Project> GetProjectList(int userId)
        {
            var projects = new List<Project>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT pro.ProjectId,ProjectName " +
                    $"FROM Project pro " +
                    $"INNER JOIN UserProjectMap usr ON pro.ProjectId = usr.ProjectId " +
                    $"WHERE usr.UserId={userId}", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    projects.Add(new Project
                    {
                        ProjectId = reader.GetInt32("ProjectId"),
                        ProjectName = reader.GetString("ProjectName")
                    });
                }

                CloseConnection(sqliteConnection);
            }

            return projects;
        }

        public List<Project> GetProjectList()
        {
            var projects = new List<Project>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ProjectId,ProjectName FROM Project", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    projects.Add(new Project
                    {
                        ProjectId = reader.GetInt32("ProjectId"),
                        ProjectName = reader.GetString("ProjectName")
                    });
                }

                CloseConnection(sqliteConnection);
            }

            return projects;
        }

        public List<ProjectWiseIssues> GetIssuesCountByProject(int userId)
        {
            var issues = new List<ProjectWiseIssues>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ProjectName, sum(High)+sum(Medium)+sum(Low) as IssuesCount " +
                    $"FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    $"WHERE UserId = {userId} " +
                    "GROUP BY rpt.ProjectId", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    issues.Add(
                        new ProjectWiseIssues
                        {
                            Project = reader.GetString("ProjectName"),
                            Issues = reader.GetInt32("IssuesCount")
                        });
                }

                CloseConnection(sqliteConnection);
            }

            return issues;
        }

        public List<ProjectWiseIssues> GetIssuesCountByProject()
        {
            var issues = new List<ProjectWiseIssues>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ProjectName, sum(High)+sum(Medium)+sum(Low) as IssuesCount " +
                    $"FROM ZapReports rpt " +
                    "INNER JOIN Project pro ON rpt.ProjectId = pro.ProjectId " +
                    "GROUP BY rpt.ProjectId", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    issues.Add(
                        new ProjectWiseIssues
                        {
                            Project = reader.GetString("ProjectName"),
                            Issues = reader.GetInt32("IssuesCount")
                        });
                }

                CloseConnection(sqliteConnection);
            }

            return issues;
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