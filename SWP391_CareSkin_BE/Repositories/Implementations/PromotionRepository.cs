using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly MyDbContext _context;

        public PromotionRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetAllPromotionsAsync()
        {
            return await _context.Promotions
                .Include(p => p.PromotionProducts)
                .Include(p => p.PromotionCustomers)
                .ToListAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _context.Promotions
                .Include(p => p.PromotionProducts)
                .Include(p => p.PromotionCustomers)
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId);
        }

        public async Task<List<Promotion>> GetActivePromotionsAsync()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _context.Promotions
                .Where(p => p.Start_Date <= currentDate && p.End_Date >= currentDate)
                .Include(p => p.PromotionProducts)
                .Include(p => p.PromotionCustomers)
                .ToListAsync();
        }

        public async Task<Promotion> AddPromotionAsync(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromotionAsync(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPromotionProductAsync(PromotionProduct promotionProduct)
        {
            _context.PromotionProducts.Add(promotionProduct);
            await _context.SaveChangesAsync();
        }

        public async Task AddPromotionCustomerAsync(int promotionId, int customerId)
        {
            var promotionCustomer = new PromotionCustomer
            {
                PromotionId = promotionId,
                CustomerId = customerId
            };
            _context.PromotionCustomers.Add(promotionCustomer);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePromotionProductAsync(int promotionId, int productId)
        {
            var promotionProduct = await _context.PromotionProducts
                .FirstOrDefaultAsync(pp => pp.PromotionId == promotionId && pp.ProductId == productId);
            if (promotionProduct != null)
            {
                _context.PromotionProducts.Remove(promotionProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemovePromotionCustomerAsync(int promotionId, int customerId)
        {
            var promotionCustomer = await _context.PromotionCustomers
                .FirstOrDefaultAsync(pc => pc.PromotionId == promotionId && pc.CustomerId == customerId);
            if (promotionCustomer != null)
            {
                _context.PromotionCustomers.Remove(promotionCustomer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Promotion>> GetPromotionsForCustomerAsync(int customerId)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _context.Promotions
                .Where(p => p.Start_Date <= currentDate && p.End_Date >= currentDate)
                .Where(p => p.PromotionCustomers.Any(pc => pc.CustomerId == customerId))
                .Include(p => p.PromotionProducts)
                .Include(p => p.PromotionCustomers)
                .ToListAsync();
        }

        public async Task<List<PromotionProduct>> GetPromotionsForProductAsync(int productId)
        {
            return await _context.PromotionProducts
                .Where(pp => pp.ProductId == productId && pp.IsActive)
                .ToListAsync();
        }
    }
}
