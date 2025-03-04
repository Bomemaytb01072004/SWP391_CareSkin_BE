using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class SkinTypeRepository : ISkinTypeRepository
    {
        private readonly MyDbContext _context;

        public SkinTypeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<SkinType>> GetAllAsync()
        {
            return await _context.SkinTypes.ToListAsync();
        }

        public async Task<SkinType> GetByIdAsync(int id)
        {
            return await _context.SkinTypes.FindAsync(id);
        }

        public async Task<SkinType> CreateAsync(SkinType skinType)
        {
            _context.SkinTypes.Add(skinType);
            await _context.SaveChangesAsync();
            return skinType;
        }

        public async Task<SkinType> UpdateAsync(SkinType skinType)
        {
            _context.Entry(skinType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return skinType;
        }

        public async Task DeleteAsync(SkinType skinType)
        {
            _context.SkinTypes.Remove(skinType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SkinTypes.AnyAsync(x => x.SkinTypeId == id);
        }
    }
}
