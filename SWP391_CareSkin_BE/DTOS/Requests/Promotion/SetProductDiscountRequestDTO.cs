using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Promotion
{
    public class SetProductDiscountRequestDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int PromotionId { get; set; }
    }
}
