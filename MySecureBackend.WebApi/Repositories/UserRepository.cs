using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User?> GetByUserName(string userName)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM [User] WHERE UserName = @UserName",
                new { UserName = userName });
        }

        public async Task Create(User user)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "INSERT INTO [User] (UserName, PasswordHash) VALUES (@UserName, @PasswordHash)",
                user
                );
        }
    }
}
