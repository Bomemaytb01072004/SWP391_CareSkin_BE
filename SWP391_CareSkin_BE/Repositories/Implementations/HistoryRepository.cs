using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
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

        public List<History> GetAllHistories() => _context.Historys.ToList();

        public History GetHistoryById(int historyId) => _context.Historys.Find(historyId);

        public void AddHistory(History history)
        {
            _context.Historys.Add(history);
            SaveChanges();
        }

        public void UpdateHistory(History history)
        {
            _context.Historys.Update(history);
            SaveChanges();
        }

        public void DeleteHistory(int historyId)
        {
            var history = GetHistoryById(historyId);
            if (history != null)
            {
                _context.Historys.Remove(history);
                SaveChanges();
            }
        }

        public void SaveChanges() => _context.SaveChanges();
    }

}
