using SWP391_CareSkin_BE.DTOS.ProductPicture;
using SWP391_CareSkin_BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class ProductPictureMapper
    {
        public static ProductPictureDTO ToDTO(ProductPicture productPicture)
        {
            if (productPicture == null)
                return null;
                
            return new ProductPictureDTO
            {
                ProductPictureId = productPicture.ProductPictureId,
                ProductId = productPicture.ProductId,
                ProductName = productPicture.Product?.ProductName,
                PictureUrl = productPicture.PictureUrl
            };
        }
        
        public static List<ProductPictureDTO> ToDTOList(IEnumerable<ProductPicture> productPictures)
        {
            return productPictures?.Select(ToDTO).ToList() ?? new List<ProductPictureDTO>();
        }

        public static ProductPicture ToEntity(CreateProductPictureDTO createDto)
        {
            if (createDto == null)
                return null;

            return new ProductPicture
            {
                ProductId = createDto.ProductId,
                PictureUrl = string.Empty // Will be set after image upload
            };
        }

        public static void UpdateEntity(ProductPicture productPicture, string pictureUrl)
        {
            if (productPicture == null)
                return;

            productPicture.PictureUrl = pictureUrl;
        }
    }
}
