using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories.Interfaces;
using Npgsql;

namespace MySecureBackend.WebApi.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly string _connectionString;
    
        public EnvironmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Environment2D>> GetByUserId(int userId)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            return await conn.QueryAsync<Environment2D>(
                "SELECT * FROM Environment2D WHERE UserId = @UserId",
                new { UserId = userId }
                );
        }

        public async Task<Environment2D?> GetById(int id)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            return await conn.QueryFirstOrDefaultAsync<Environment2D>(
                "SELECT * FROM Environment2D WHERE Id = @Id",
                new { Id = id }
                );
        }

        public async Task Create(Environment2D env)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.ExecuteAsync(
                @"INSERT INTO Environment2D (UserId, Name, Width, Height) VALUES (@UserId, @Name, @Width, @Height)",
                env
                );
        }

        public async Task Delete(int id)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.ExecuteAsync(
                @"DELETE FROM Environment2D WHERE Id = @Id",
                new { Id = id }
                );
        }

        public async Task<int> CountByUser(int  userId)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Environment2D WHERE UserId = @UserId",
                new { UserId = userId }
                );
        }

        public async Task<bool> DoesNameExists(int userId, string name)
        {
            //using var conn = new SqlConnection(_connectionString);
            using var conn = new NpgsqlConnection(_connectionString);

            var count = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Environment2D WHERE UserId = @UserId AND Name = @Name",
                new { UserId = userId, Name = name}
                );

            return count > 0;
        }
    }
}
