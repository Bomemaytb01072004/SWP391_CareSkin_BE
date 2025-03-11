using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Data

{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //region Dbset
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<BlogNew> BlogNews { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetailIngredient> productDetailIngredients { get; set; }
        public DbSet<ProductMainIngredient> ProductMainIngredients { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductUsage> ProductUsages { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<ProductForSkinType> productForSkinTypes { get; set; }
        public DbSet<PromotionCustomer> PromotionCustomers { get; set; }
        public DbSet<PromotionProduct> PromotionProducts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
        public DbSet<RatingFeedback> RatingFeedbacks { get; set; }
        public DbSet<RatingFeedbackImage> RatingFeedbackImages { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<RoutineProduct> RoutineProducts { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<SkinType> SkinTypes { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<VnpayTransactions> VnpayTransactions { get; set; }
        public DbSet<UserQuizAttempt> UserQuizAttempts { get; set; }

        // end Dbset


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //primary key 
            modelBuilder.Entity<Admin>().HasKey(a => a.AdminId);
            modelBuilder.Entity<Answer>().HasKey(a => a.AnswerId);
            modelBuilder.Entity<BlogNew>().HasKey(b => b.BlogId);
            modelBuilder.Entity<Brand>().HasKey(b => b.BrandId);
            modelBuilder.Entity<Cart>().HasKey(c => c.CartId);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<FAQ>().HasKey(f => f.FAQId);
            modelBuilder.Entity<History>().HasKey(h => h.HistoryId);
            modelBuilder.Entity<OrderProduct>().HasKey(o => o.OrderProductId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderStatus>().HasKey(o => o.OrderStatusId);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<ProductDetailIngredient>().HasKey(p => p.ProductDetailIngredientId);
            modelBuilder.Entity<ProductMainIngredient>().HasKey(p => p.ProductMainIngredientId);
            modelBuilder.Entity<ProductPicture>().HasKey(p => p.ProductPictureId);
            modelBuilder.Entity<ProductUsage>().HasKey(p => p.ProductUsageId);
            modelBuilder.Entity<ProductVariation>().HasKey(p => p.ProductVariationId);
            modelBuilder.Entity<ProductForSkinType>().HasKey(p => new { p.ProductForSkinTypeId });
            modelBuilder.Entity<PromotionCustomer>().HasKey(p => new { p.CustomerId, p.PromotionId });
            modelBuilder.Entity<PromotionProduct>().HasKey(p => new { p.PromotionProductId });
            modelBuilder.Entity<Promotion>().HasKey(p => p.PromotionId);
            modelBuilder.Entity<Question>().HasKey(q => q.QuestionsId);
            modelBuilder.Entity<Quiz>().HasKey(q => q.QuizId);
            modelBuilder.Entity<RatingFeedback>().HasKey(r => r.RatingFeedbackId);
            modelBuilder.Entity<RatingFeedbackImage>().HasKey(r => r.RatingFeedbackImageId);
            modelBuilder.Entity<Result>().HasKey(result => result.ResultId);
            modelBuilder.Entity<RoutineProduct>().HasKey(s => new { s.RoutineProductId });
            modelBuilder.Entity<Routine>().HasKey(s => s.RoutineId);
            modelBuilder.Entity<SkinType>().HasKey(s => s.SkinTypeId);
            modelBuilder.Entity<Staff>().HasKey(s => s.StaffId);
            modelBuilder.Entity<Support>().HasKey(s => s.SuppportId);
            modelBuilder.Entity<VnpayTransactions>().HasKey(v => v.Id);
            modelBuilder.Entity<UserQuizAttempt>().HasKey(u => u.UserQuizAttemptId);



            // end primary key


            //relationship



            modelBuilder.Entity<Answer>()
                .HasMany(a => a.Historys)
                .WithOne(h => h.Answer)
                .HasForeignKey(h => h.AnswerId);

            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.BlogNews)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.UserQuizAttempts)
                .WithOne(h => h.Customer)
                .HasForeignKey(h => h.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Results)
                .WithOne(result => result.Customer)
                .HasForeignKey(result => result.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Carts)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.RatingFeedbacks)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Supports)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.PromotionCustomers)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderProducts)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderStatus>()
                .HasMany(o => o.Orders)
                .WithOne(o => o.OrderStatus)
                .HasForeignKey(o => o.OrderStatusId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Carts)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.RoutineProducts)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.PromotionProducts)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderProducts)
                .WithOne(o => o.Product)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.RatingFeedbacks)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductVariations)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductMainIngredients)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductDetailIngredients)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductUsages)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductPictures)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductForSkinTypes)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductVariation>()
                .HasMany(p => p.Carts)
                .WithOne(c => c.ProductVariation)
                .HasForeignKey(c => c.ProductVariationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductVariation>()
                .HasMany(p => p.OrderProducts)
                .WithOne(o => o.ProductVariation)
                .HasForeignKey(o => o.ProductVariationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Promotion)
                .HasForeignKey(o => o.PromotionId);

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionProducts)
                .WithOne(p => p.Promotion)
                .HasForeignKey(p => p.PromotionId);

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionCustomers)
                .WithOne(p => p.Promotion)
                .HasForeignKey(p => p.PromotionId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Historys)
                .WithOne(h => h.Question)
                .HasForeignKey(h => h.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(q => q.Quiz)
                .HasForeignKey(q => q.QuizId);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.UserQuizAttempts)
                .WithOne(u => u.Quiz)
                .HasForeignKey(u => u.QuizId);

            modelBuilder.Entity<Result>()
                .HasOne(result => result.Customer)
                .WithMany(c => c.Results)
                .HasForeignKey(result => result.CustomerId);

            modelBuilder.Entity<Result>()
                .HasOne(result => result.SkinType)
                .WithMany(s => s.Results)
                .HasForeignKey(result => result.SkinTypeId);

            modelBuilder.Entity<Routine>()
                .HasMany(s => s.RoutineProducts)
                .WithOne(s => s.Routine)
                .HasForeignKey(s => s.RoutineId);

            modelBuilder.Entity<SkinType>()
                .HasMany(s => s.Results)
                .WithOne(result => result.SkinType)
                .HasForeignKey(result => result.SkinTypeId);

            modelBuilder.Entity<SkinType>()
                .HasMany(s => s.Routines)
                .WithOne(s => s.SkinType)
                .HasForeignKey(s => s.SkinTypeId);

            modelBuilder.Entity<SkinType>()
                .HasMany(s => s.ProductForSkinTypes)
                .WithOne(p => p.SkinType)
                .HasForeignKey(p => p.SkinTypeId);

            modelBuilder.Entity<Staff>()
                .HasMany(s => s.Supports)
                .WithOne(s => s.Staff)
                .HasForeignKey(s => s.StaffId);
            modelBuilder.Entity<VnpayTransactions>()
                .HasOne(v => v.order)
                .WithMany(o => o.VnpayTransactions)
                .HasForeignKey(v => v.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VnpayTransactions>()
                .Property(v => v.Amount)
                .HasColumnType("decimal(18, 4)");

            modelBuilder.Entity<RatingFeedback>()
                .HasMany(r => r.RatingFeedbackImages)
                .WithOne(r => r.RatingFeedback)
                .HasForeignKey(r => r.RatingFeedbackId);

            modelBuilder.Entity<RatingFeedbackImage>()
                .HasOne(r => r.RatingFeedback)
                .WithMany(r => r.RatingFeedbackImages)
                .HasForeignKey(r => r.RatingFeedbackId);

            modelBuilder.Entity<UserQuizAttempt>()
                .HasMany(u => u.Histories)
                .WithOne(r => r.UserQuizAttempt)
                .HasForeignKey(r => r.AttemmptId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserQuizAttempt>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.UserQuizAttempts)
                .HasForeignKey(u => u.CustomerId);

            modelBuilder.Entity<UserQuizAttempt>()
                .HasOne(u => u.Quiz)
                .WithMany(q => q.UserQuizAttempts)
                .HasForeignKey(u => u.QuizId);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.UserQuizAttempt)
                .WithOne()
                .HasForeignKey<Result>(r => r.UserQuizAttemptId)
                .OnDelete(DeleteBehavior.NoAction);
            //end relationship
        }
    }
}
