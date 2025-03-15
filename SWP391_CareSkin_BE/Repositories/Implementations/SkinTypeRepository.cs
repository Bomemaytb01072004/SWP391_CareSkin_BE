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

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SkinTypes.AnyAsync(x => x.SkinTypeId == id);
        }

        public async Task<SkinType> GetByNameAsync(string typeName)
        {
            return await _context.SkinTypes
                .FirstOrDefaultAsync(x => x.TypeName.ToLower() == typeName.ToLower());
        }

        public async Task<List<SkinType>> GetActiveSkinTypesAsync()
        {
            return await _context.SkinTypes
                .Where(st => st.IsActive)
                .ToListAsync();
        }

        public async Task<List<SkinType>> GetInactiveSkinTypesAsync()
        {
            return await _context.SkinTypes
                .Where(st => !st.IsActive)
                .ToListAsync();
        }
    }
}
