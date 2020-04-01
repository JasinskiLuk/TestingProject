using Dapper;
using DapperTesting.DTOs;
using DapperTesting.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperTesting.DbServices
{
    public class DbDateTestService : DbBaseService, IDateTest
    {
        public DbDateTestService(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task AddDate(DateTestDTO DTO)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
            await conn.QueryAsync(@"INSERT INTO [dbo].[DateTest]([Date1], [DateTime1], [DateTime2])
                                    VALUES(@Date1, @DateTime1, @DateTime2)",
                                        new { DTO.Date1, DTO.DateTime1, DTO.DateTime2 });
        }

        public async Task EditDate(DateTestDTO DTO)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.QueryAsync(@"UPDATE [dbo].[DateTest]
                                    SET [Date1] = @Date1,
                                        [DateTime1] = @DateTime1,
                                        [DateTime2] = @DateTime2
                                    WHERE [Id] = @Id",
                                        new { DTO.Id, DTO.Date1, DTO.DateTime1, DTO.DateTime2 });
        }

        public async Task<IEnumerable<DateTestDTO>> GetDate()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            var model = await conn.QueryAsync<DateTestDTO>(@"SELECT [Id], [Date1], [DateTime1], [DateTime2]
                                                        FROM [dbo].[DateTest]");
            return model;
        }

        public async Task DeleteDate(int Id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync(@"DELETE FROM [DateTest]
                                      WHERE [Id] = @Id", new { Id });
        }
    }
}
