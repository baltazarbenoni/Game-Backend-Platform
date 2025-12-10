using Application.Interfaces;
using Infrastructure.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ApplicationDbContext _context;
        public async Task CreateAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();        
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token && t.ExpiresAt > DateTime.UtcNow);
        }
        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await  _context.SaveChangesAsync();
        }
        public async Task RevokeAll(Guid id)
        {
            var tokens = await _context.RefreshTokens.Where(rt => rt.UserId == id && rt.IsActive).ToListAsync();
            foreach(var token in tokens)
            {
                token.Revoke();
                _context.RefreshTokens.Update(token);
            }
            await  _context.SaveChangesAsync();
        }
    }
}