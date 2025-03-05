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

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly MyDbContext _context;

        public PromotionService(IPromotionRepository promotionRepository, MyDbContext context)
        {
            _promotionRepository = promotionRepository;
            _context = context;
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

            bool isActive = DateOnly.FromDateTime(DateTime.UtcNow) >= request.StartDate && DateOnly.FromDateTime(DateTime.UtcNow) <= request.EndDate;

            PromotionMapper.UpdateEntity(promotion, request, isActive);
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // Get the updated promotion
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        public async Task<bool> DeletePromotionAsync(int promotionId)
        {
            await _promotionRepository.DeletePromotionAsync(promotionId);
            return true;
        }

        public async Task<List<PromotionDTO>> GetPromotionsForCustomerAsync(int customerId)
        {
            var promotions = await _promotionRepository.GetPromotionsForCustomerAsync(customerId);
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<List<ProductDiscountDTO>> GetProductDiscountsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            var productDiscounts = new List<ProductDiscountDTO>();

            foreach (var promotion in promotions)
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
            // Kiểm tra xem promotion có tồn tại không
            var promotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }
            else if (!promotion.IsActive)
            {
                throw new Exception("Promotion is not active");
            }

            // Kiểm tra xem sản phẩm đã có discount active chưa
            var activeDiscounts = await _promotionRepository.GetPromotionsForProductAsync(request.ProductId);
            if (activeDiscounts.Any())
            {
                throw new Exception("Product already has an active discount");
            }

            // Tạo đối tượng PromotionProduct từ request
            var promotionProduct = PromotionMapper.ToEntity(request);
            promotionProduct.IsActive = promotion.IsActive;

            // Tính toán giá salePrice dựa trên promotionId và productId
            promotionProduct.SalePrice = await CalculateSalePrice(request.PromotionId, request.ProductId);

            // Lưu đối tượng PromotionProduct vào cơ sở dữ liệu
            await _promotionRepository.AddPromotionProductAsync(promotionProduct);

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

            //var activeDiscounts = await _promotionRepository.GetPromotionsForProductAsync(request.ProductId);
            //if (activeDiscounts.Any())
            //{
            //    throw new Exception("Product already has an active discount");
            //}

            // Kiểm tra xem discount của sản phẩm đã tồn tại trong promotion chưa
            var promotionProduct = promotion.PromotionProducts?.FirstOrDefault(pp => pp.ProductId == request.ProductId);
            if (promotionProduct == null)
            {
                throw new Exception("Product discount not found");
            }

            // Cập nhật trạng thái discount theo request
            promotionProduct.IsActive = request.IsActive;

            // Tính toán lại giá salePrice dựa trên promotionId và productId
            promotionProduct.SalePrice = await CalculateSalePrice(request.PromotionId, request.ProductId);

            // Cập nhật thông tin promotion (bao gồm mảng PromotionProducts đã được cập nhật)
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // Lấy lại promotion đã cập nhật và chuyển sang DTO để trả về
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(request.PromotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }



        private async Task<decimal> CalculateSalePrice(int promotionId, int productId)
        {
            // Tìm bản ghi PromotionProduct cho sản phẩm và khuyến mãi tương ứng, chỉ lấy các bản ghi đang active.
            var promotionProduct = await _context.PromotionProducts
                .Include(pp => pp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(pp => pp.Promotion)
                .FirstOrDefaultAsync(pp => pp.PromotionId == promotionId
                                        && pp.ProductId == productId
                                        && pp.IsActive);

            // Nếu không tìm thấy khuyến mãi active cho sản phẩm, trả về giá gốc của sản phẩm.
            if (promotionProduct == null)
            {
                var product = await _context.Products
                    .Include(p => p.ProductVariations)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                if (product == null)
                    throw new Exception("Không tìm thấy sản phẩm.");

                // Giả sử dùng variation đầu tiên làm giá gốc
                var basePrice = product.ProductVariations.FirstOrDefault()?.Price ?? 0;
                return basePrice;
            }

            // Lấy giá gốc từ ProductVariation (giả sử dùng variation đầu tiên)
            var originalPrice = promotionProduct.Product.ProductVariations.FirstOrDefault()?.Price ?? 0;
            decimal discountPercent = promotionProduct.Promotion.DiscountPercent;

            // Tính giá bán sau khi giảm giá: giá gốc - (giá gốc * phần trăm giảm giá / 100)
            decimal calculatedSalePrice = originalPrice - (originalPrice * discountPercent / 100);

            // Cập nhật lại giá bán trong PromotionProduct
            promotionProduct.SalePrice = calculatedSalePrice;
            await _context.SaveChangesAsync();

            return calculatedSalePrice;
        }


    }
}
