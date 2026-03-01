using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories.Interfaces
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2D>> GetByEnvId(int envId);
        Task Create(Object2D obj);
        Task Delete(int id);
        Task DeleteByEnv(int envId);
    }
}
