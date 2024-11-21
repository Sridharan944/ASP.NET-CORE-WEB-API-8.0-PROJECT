using Knila_Projects.Entities;

namespace Knila_Projects.InterfaceRepositories
{
    public interface IUserRepository
    {
        Task<UserLogin?> GetUserByCredentialsAsync(string username, string password);

    }
}
