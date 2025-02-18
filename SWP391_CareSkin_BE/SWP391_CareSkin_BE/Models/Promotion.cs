﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Promotion")]
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        public string Name { get; set; }

        public decimal Discount_percent;

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
        public virtual ICollection<PromotionCustomer> PromotionCustomers { get; set; } = new List<PromotionCustomer>();
    }
}
