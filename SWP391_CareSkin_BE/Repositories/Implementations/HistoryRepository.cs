using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly MyDbContext _context;

        public HistoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoryDTO>> GetAllAsync()
        {
            var histories = await _context.Historys.ToListAsync();
            return histories.Select(h => HistoryMapper.ToDTO(h));
        }

        public async Task<HistoryDTO> GetByIdAsync(int id)
        {
            var history = await _context.Historys.FindAsync(id);
            return history != null ? HistoryMapper.ToDTO(history) : null;
        }

        public async Task<HistoryDTO> AddAsync(HistoryDTO historyDTO)
        {
            var history = HistoryMapper.ToEntity(historyDTO);
            _context.Historys.Add(history);
            await _context.SaveChangesAsync();
            return HistoryMapper.ToDTO(history);
        }

        public async Task<bool> DeleteHistoryAsync(int id) 
        {
            var history = await _context.Historys.FindAsync(id);
            if (history == null) return false;

            _context.Historys.Remove(history);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
