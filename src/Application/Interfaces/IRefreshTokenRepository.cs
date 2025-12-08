using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}