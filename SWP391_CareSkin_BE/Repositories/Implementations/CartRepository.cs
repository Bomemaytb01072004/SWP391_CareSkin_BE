﻿using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly MyDbContext _context;

        public CartRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cart>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            return await _context.Carts
                .Include(c => c.Product)  // Để lấy thông tin Product (nếu cần)
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddCartItemAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int cartId)
        {
            var cart = await GetCartItemByIdAsync(cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cart> GetCartItemByIdAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
        }
    }
}
