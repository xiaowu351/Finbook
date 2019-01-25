using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.API.Application.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connectionString = string.Empty;

        public ProjectQueries(string connStr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connStr) ? connStr : throw new ArgumentNullException(nameof(connStr));
        }

        public async Task<dynamic> GetProjectDetailsAsync(int projectId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync(@"SELECT 
                                                                p.Company,
                                                                p.City,
                                                                p.Area,
                                                                p.Province,
                                                                p.FinStage,
                                                                p.FinMoney,
                                                                p.Valuation,
                                                                p.FinPercentage,
                                                                p.Introduction,
                                                                p.UserId,
                                                                p.Income,
                                                                p.Revenue,
                                                                p.Avatar,
                                                                p.BrokerageOptions,
                                                                pvr.Tags,
                                                                pvr.Visible
                                                                FROM 
                                                                projects as p 
                                                                LEFT JOIN
                                                                projectvisiblerules as pvr
                                                                ON p.Id = pvr.ProjectId

                                                                WHERE p.Id = @projectId",new { projectId});
            }
        }

        public async Task<IEnumerable<dynamic>> GetProjectsByUserIdAsync(int userId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(@"SELECT 
                                                        p.Company,
                                                        p.City,
                                                        p.Area,
                                                        p.Province,
                                                        p.FinStage,
                                                        p.FinMoney,
                                                        p.Valuation,
                                                        p.FinPercentage,
                                                        p.Introduction,
                                                        p.UserId,
                                                        p.Income,
                                                        p.Revenue,
                                                        p.Avatar,
                                                        p.BrokerageOptions,
                                                        pvr.Tags,
                                                        pvr.Visible
                                                        FROM 
                                                        projects as p 
                                                        LEFT JOIN
                                                        projectvisiblerules as pvr
                                                        ON p.Id = pvr.ProjectId

                                                        WHERE p.UserId = @userId", new { userId});
            }
        }
    }
}
