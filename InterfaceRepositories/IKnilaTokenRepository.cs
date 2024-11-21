using Knila_Projects.Entities;

namespace Knila_Projects.InterfaceRepositories
{
    public interface IKnilaTokenRepository
    {
        Task AddAsync(KnilaToken knilaToken);

        Task<KnilaToken> GetTokenByUserIdAsync(int userId);
        Task<IEnumerable<KnilaToken>> GetTokensByUserIdAsync(int userId);

        Task<KnilaToken> GetLatestTokenByUserIdAsync(int userId);
    }
}
