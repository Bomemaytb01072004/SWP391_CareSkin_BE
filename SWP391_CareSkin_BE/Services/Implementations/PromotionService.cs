using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.DTOS.Responses.Promotion;
using SWP391_CareSkin_BE.DTOS.Requests.Promotion;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Repositories.Implementations;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly MyDbContext _context;
        private readonly IProductRepository _productRepository;

        public PromotionService(IPromotionRepository promotionRepository, MyDbContext context, IProductRepository productRepository)
        {
            _promotionRepository = promotionRepository;
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<List<PromotionDTO>> GetAllPromotionsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<PromotionDTO> GetPromotionByIdAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(promotion);
        }

        public async Task<List<PromotionDTO>> GetActivePromotionsAsync()
        {
            var promotions = await _promotionRepository.GetActivePromotionsAsync();
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<List<PromotionDTO>> GetActivePromotionsByTypeAsync(PromotionType promotionType)
        {
            var promotions = await _promotionRepository.GetActivePromotionsAsync();
            return promotions
                .Where(p => p.PromotionType == promotionType)
                .Select(PromotionMapper.ToDTO)
                .ToList();
        }

        public async Task<PromotionDTO> CreatePromotionAsync(PromotionCreateRequestDTO request)
        {
            bool isActive = DateOnly.FromDateTime(DateTime.UtcNow) >= request.StartDate && DateOnly.FromDateTime(DateTime.UtcNow) <= request.EndDate;

            var promotion = PromotionMapper.ToEntity(request, isActive);
            await _promotionRepository.AddPromotionAsync(promotion);

            // Get the complete promotion with relationships
            var createdPromotion = await _promotionRepository.GetPromotionByIdAsync(promotion.PromotionId);
            return PromotionMapper.ToDTO(createdPromotion);
        }

        public async Task<PromotionDTO> UpdatePromotionAsync(int promotionId, PromotionUpdateRequestDTO request)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return null;

            // Determine if the promotion should be active based on current date and request dates
            bool isActive = DateOnly.FromDateTime(DateTime.UtcNow) >= request.StartDate && DateOnly.FromDateTime(DateTime.UtcNow) <= request.EndDate;

            // Update the promotion entity
            PromotionMapper.UpdateEntity(promotion, request, isActive);
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // If this is a product promotion, update the associated promotion products
            if (promotion.PromotionType == PromotionType.Product && promotion.PromotionProducts != null)
            {
                await UpdatePromotionProductsStatus(promotion, isActive);
            }

            // Get the updated promotion
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        private async Task UpdatePromotionProductsStatus(Promotion promotion, bool promotionIsActive)
        {
            // If the promotion is not active, deactivate all its promotion products
            if (!promotionIsActive)
            {
                foreach (var promotionProduct in promotion.PromotionProducts)
                {
                    if (promotionProduct.IsActive)
                    {
                        promotionProduct.IsActive = false;
                        
                        // Reset SalePrice for all variations of this product
                        var product = await _context.Products
                            .Include(p => p.ProductVariations)
                            .FirstOrDefaultAsync(p => p.ProductId == promotionProduct.ProductId);

                        if (product != null)
                        {
                            foreach (var variation in product.ProductVariations)
                            {
                                variation.SalePrice = 0;
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return;
            }

            // If the promotion is active, we need to check each inactive promotion product
            // to see if it can be activated
            foreach (var promotionProduct in promotion.PromotionProducts.Where(pp => !pp.IsActive))
            {
                // Check if this product already has any active promotion
                bool hasActivePromotion = await _context.PromotionProducts
                    .AnyAsync(pp => pp.ProductId == promotionProduct.ProductId && 
                              pp.IsActive);

                // Only activate if no active promotions exist for this product
                if (!hasActivePromotion)
                {
                    promotionProduct.IsActive = true;
                    
                    // Calculate and set SalePrice for all variations of this product
                    await CalculateSalePrice(promotion.PromotionId, promotionProduct.ProductId);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<PromotionDTO> DeactivatePromotionAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return null;

            // Set the promotion to inactive
            promotion.IsActive = false;
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // If this is a product promotion, deactivate all its promotion products
            if (promotion.PromotionType == PromotionType.Product && promotion.PromotionProducts != null)
            {
                foreach (var promotionProduct in promotion.PromotionProducts)
                {
                    if (promotionProduct.IsActive)
                    {
                        promotionProduct.IsActive = false;
                        
                        // Reset SalePrice for all variations of this product
                        var product = await _context.Products
                            .Include(p => p.ProductVariations)
                            .FirstOrDefaultAsync(p => p.ProductId == promotionProduct.ProductId);

                        if (product != null)
                        {
                            foreach (var variation in product.ProductVariations)
                            {
                                variation.SalePrice = 0;
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }

            // Get the updated promotion
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        public async Task<bool> DeletePromotionAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return false;

            // Implement soft delete by setting IsActive to false
            promotion.IsActive = false;
            await _promotionRepository.UpdatePromotionAsync(promotion);
            return true;
        }

        public async Task<List<ProductDiscountDTO>> GetProductDiscountsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            var productDiscounts = new List<ProductDiscountDTO>();

            foreach (var promotion in promotions.Where(p => p.PromotionType == PromotionType.Product))
            {
                if (promotion.PromotionProducts != null)
                {
                    foreach (var pp in promotion.PromotionProducts)
                    {
                        var dto = PromotionMapper.ToProductDiscountDTO(pp, promotion);
                        if (dto != null)
                        {
                            productDiscounts.Add(dto);
                        }
                    }
                }
            }

            return productDiscounts;
        }

        public async Task<PromotionDTO> SetProductDiscountAsync(SetProductDiscountRequestDTO request)
        {
            // Kiểm tra xem promotion có tồn tại không và khuyến mãi đang active
            var promotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            if (promotion == null || !promotion.IsActive)
            {
                throw new Exception("Promotion not found or is not active.");
            }

            // Kiểm tra promotion có phải là loại dành cho sản phẩm không
            if (promotion.PromotionType != PromotionType.Product)
            {
                throw new Exception("This promotion is not for products. Please select a product promotion.");
            }

            // Kiểm tra xem sản phẩm đã có discount active chưa
            var activeDiscounts = await _promotionRepository.GetPromotionsForProductAsync(request.ProductId);
            if (activeDiscounts.Any(pp => pp.IsActive))
            {
                throw new Exception("Product already has an active discount");
            }

            // Tạo đối tượng PromotionProduct từ request
            var promotionProduct = PromotionMapper.ToEntity(request);
            promotionProduct.IsActive = promotion.IsActive;

            // Lưu đối tượng PromotionProduct vào cơ sở dữ liệu
            await _promotionRepository.AddPromotionProductAsync(promotionProduct);

            // Tính toán giá salePrice cho tất cả các variation của sản phẩm
            await CalculateSalePrice(request.PromotionId, request.ProductId);

            // Lấy lại promotion đã cập nhật (bao gồm danh sách sản phẩm) và chuyển đổi sang DTO
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        public async Task<PromotionDTO> UpdateProductDiscountStatusAsync(UpdateProductDiscountStatusDTO request)
        {
            // Kiểm tra xem promotion có tồn tại không
            var promotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            // Kiểm tra promotion có phải là loại dành cho sản phẩm không
            if (promotion.PromotionType != PromotionType.Product)
            {
                throw new Exception("This promotion is not for products. Please select a product promotion.");
            }

            // Kiểm tra xem discount của sản phẩm đã tồn tại trong promotion chưa
            var promotionProduct = promotion.PromotionProducts?.FirstOrDefault(pp => pp.ProductId == request.ProductId);
            if (promotionProduct == null)
            {
                throw new Exception("Product discount not found");
            }
            
            // Nếu đang cố gắng kích hoạt một promotion (IsActive = true), kiểm tra xem sản phẩm đã có promotion active nào khác chưa
            if (request.IsActive)
            {
                // Lấy tất cả các promotion product của sản phẩm này
                var activePromotions = await _context.PromotionProducts
                    .Where(pp => pp.ProductId == request.ProductId && pp.IsActive && pp.PromotionProductId != promotionProduct.PromotionProductId)
                    .AnyAsync();
                
                if (activePromotions)
                {
                    throw new Exception("This product already has an active promotion. Please disable it first before activating another one.");
                }
            }

            // Cập nhật trạng thái discount theo request
            promotionProduct.IsActive = request.IsActive;

            // Cập nhật thông tin promotion (bao gồm mảng PromotionProducts đã được cập nhật)
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // Nếu trạng thái là active, tính toán lại giá SalePrice cho tất cả các variation
            // Nếu trạng thái là inactive, reset SalePrice về 0 cho tất cả các variation
            if (request.IsActive)
            {
                await CalculateSalePrice(request.PromotionId, request.ProductId);
            }
            else
            {
                // Reset SalePrice về 0 cho tất cả các variation của sản phẩm
                var product = await _context.Products
                    .Include(p => p.ProductVariations)
                    .FirstOrDefaultAsync(p => p.ProductId == request.ProductId);

                if (product != null)
                {
                    foreach (var variation in product.ProductVariations)
                    {
                        variation.SalePrice = 0;
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // Lấy lại promotion đã cập nhật và chuyển sang DTO để trả về
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        public async Task<List<ProductDTO>> GetProductsWithDiscountAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            // Kiểm tra promotion có phải là loại dành cho sản phẩm không
            if (promotion.PromotionType != PromotionType.Product)
            {
                throw new Exception("This promotion is not for products.");
            }

            var productIds = promotion.PromotionProducts
                .Where(pp => pp.IsActive)
                .Select(pp => pp.ProductId)
                .ToList();

            var products = await _context.Products
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductPictures)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            // Convert to DTOs
            var productDTOs = products.Select(p => ProductMapper.ToDTO(p)).ToList();

            return productDTOs;
        }

        public async Task<decimal> CalculateSalePrice(int promotionId, int productId)
        {
            // Tìm khuyến mãi với PromotionId đã được cung cấp
            var promotion = await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId);

            // Nếu không tìm thấy khuyến mãi, ném ra exception.
            if (promotion == null)
            {
                throw new Exception("Promotion not found.");
            }

            // Kiểm tra promotion có phải là loại dành cho sản phẩm không
            if (promotion.PromotionType != PromotionType.Product)
            {
                throw new Exception("This promotion is not for products.");
            }

            // Tìm product và product variations
            var product = await _context.Products
                .Include(p => p.ProductVariations)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                throw new Exception("Product not found.");

            // Lấy phần trăm giảm giá từ khuyến mãi
            decimal discountPercent = promotion.DiscountPercent;

            // Tính giá bán sau khi giảm giá cho từng variation
            bool anyChanges = false;
            foreach (var variation in product.ProductVariations)
            {
                decimal originalPrice = variation.Price;
                decimal calculatedSalePrice = originalPrice - (originalPrice * discountPercent / 100);
                
                // Cập nhật SalePrice cho variation
                if (variation.SalePrice != calculatedSalePrice)
                {
                    variation.SalePrice = calculatedSalePrice;
                    anyChanges = true;
                }
            }

            // Lưu các thay đổi vào database nếu có
            if (anyChanges)
            {
                await _context.SaveChangesAsync();
            }

            // Trả về giá sale của variation đầu tiên (cho backward compatibility)
            return product.ProductVariations.FirstOrDefault()?.SalePrice ?? 0;
        } 
    }
}
