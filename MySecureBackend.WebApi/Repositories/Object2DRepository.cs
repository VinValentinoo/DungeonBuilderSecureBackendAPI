using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories.Interfaces;

namespace MySecureBackend.WebApi.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string _connectionString;

        public Object2DRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Object2D>> GetByEnvId(int envId)
        {
            using var conn = new SqlConnection(_connectionString);

            return await conn.QueryAsync<Object2D>(
                "SELECT * FROM Object2D WHERE EnvironmentId = @EnvironmentId",
                new { EnvironmentId = envId }
                );
        }

        public async Task Create(Object2D obj)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.ExecuteAsync(
                @"INSERT INTO Object2D (EnvironmentId, ObjectType, PosX, PosY, Rotation, Scale) VALUES (@EnvironmentId, @ObjectType, @PosX, @PosY, @Rotation, @Scale)",
                obj
                );
        }

        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "DELETE FROM Object2D WHERE Id = @Id",
                new { Id = id }
                );
        }

        public async Task DeleteByEnv(int envId)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "DELETE FROM Object2D WHERE EnvironmentId = @EnvironmentId",
                new { EnvironmentId = envId }
                );
        }
    }
}
