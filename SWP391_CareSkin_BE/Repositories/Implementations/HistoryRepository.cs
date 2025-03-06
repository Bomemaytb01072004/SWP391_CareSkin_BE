using Google;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly MyDbContext _context;

        public HistoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<History> AddAsync(History history)
        {
            _context.Historys.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }
    }
}
