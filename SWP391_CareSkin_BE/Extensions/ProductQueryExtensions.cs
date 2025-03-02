using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Extensions
{
    public static class ProductQueryExtensions
    {
        public static IQueryable<Product> ApplyKeywordFilter(this IQueryable<Product> query, string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var likeKeyword = $"%{keyword}%";
                return query.Where(p =>
                    EF.Functions.Like(p.ProductName, likeKeyword) ||
                    EF.Functions.Like(p.Description, likeKeyword) ||
                    EF.Functions.Like(p.Brand.Name, likeKeyword) ||
                    p.ProductMainIngredients.Any(i => EF.Functions.Like(i.IngredientName, likeKeyword)) ||
                    p.ProductDetailIngredients.Any(i => EF.Functions.Like(i.IngredientName, likeKeyword))
                );
            }
            return query;
        }

        public static IQueryable<Product> ApplyCategoryFilter(this IQueryable<Product> query, string category)
        {
            return !string.IsNullOrWhiteSpace(category) ? query.Where(p => p.Category == category) : query;
        }

        public static IQueryable<Product> ApplyBrandFilter(this IQueryable<Product> query, int? brandId)
        {
            return brandId.HasValue ? query.Where(p => p.BrandId == brandId.Value) : query;
        }

        public static IQueryable<Product> ApplyPriceFilter(this IQueryable<Product> query, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue || maxPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariations.Any(v =>
                    (!minPrice.HasValue || v.Price >= minPrice.Value) &&
                    (!maxPrice.HasValue || v.Price <= maxPrice.Value)
                ));
            }
            return query;
        }

        public static IQueryable<Product> ApplyMlFilter(this IQueryable<Product> query, int? minMl, int? maxMl)
        {
            if (minMl.HasValue || maxMl.HasValue)
            {
                query = query.Where(p => p.ProductVariations.Any(v =>
                    (!minMl.HasValue || v.Ml >= minMl.Value) &&
                    (!maxMl.HasValue || v.Ml <= maxMl.Value)
                ));
            }
            return query;
        }

        public static IQueryable<Product> ApplySorting(this IQueryable<Product> query, string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(p => p.ProductName),
                "price_asc" => query.OrderBy(p => p.ProductVariations.Select(v => v.Price).DefaultIfEmpty().Min()),
                "price_desc" => query.OrderByDescending(p => p.ProductVariations.Select(v => v.Price).DefaultIfEmpty().Min()),
                _ => query.OrderByDescending(p => p.ProductId) // Default sort by newest
            };
        }
    }
}
