using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories.Interfaces
{
    public interface IEnvironmentRepository
    {
        Task<IEnumerable<Environment2D>> GetByUserId(int userId);
        Task<Environment2D?> GetById(int id);
        Task Create(Environment2D env);
        Task Delete(int id);
        Task<int> CountByUser(int userId);
        Task<bool> DoesNameExists(int userId, string name);
    }
}
