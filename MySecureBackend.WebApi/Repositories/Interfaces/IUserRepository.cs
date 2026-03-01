using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserName(string userName);
        Task Create(User user);
    }
}
