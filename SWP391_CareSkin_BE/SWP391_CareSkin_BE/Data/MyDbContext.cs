using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models.NewFolder;
using SWP391_CareSkin_BE.Models.Order;
using SWP391_CareSkin_BE.Models.Product;
using SWP391_CareSkin_BE.Models.Promotion;
using SWP391_CareSkin_BE.Models.Quiz;
using SWP391_CareSkin_BE.Models.Routine;

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
        public DbSet<History> Historys { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PromotionOrder> PromotionOrders { get; set; }
        public DbSet<PromotionProduct> PromotionProducts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
        public DbSet<RatingFeedback> RatingFeedbacks { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SkinCareRoutineProduct> SkinCareRoutineProducts { get; set; }
        public DbSet<SkinCareRoutine> SkinCareRoutines { get; set; }
        public DbSet<SkinType> SkinTypes { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Support> Supports { get; set; }


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
            modelBuilder.Entity<PromotionOrder>().HasKey(p => new { p.OrderId, p.PromotionId });
            modelBuilder.Entity<PromotionProduct>().HasKey(p => new { p.ProductId, p.PromotionId });
            modelBuilder.Entity<Promotion>().HasKey(p => p.PromotionId);
            modelBuilder.Entity<Question>().HasKey(q => q.QuestionsId);
            modelBuilder.Entity<Quiz>().HasKey(q => q.QuizId);
            modelBuilder.Entity<RatingFeedback>().HasKey(r => r.Id);
            modelBuilder.Entity<Result>().HasKey(r => r.ResultId);
            modelBuilder.Entity<SkinCareRoutineProduct>().HasKey(s => new { s.SkinCareRoutineId, s.ProductId });
            modelBuilder.Entity<SkinCareRoutine>().HasKey(s => s.Id);
            modelBuilder.Entity<SkinType>().HasKey(s => s.SkinTypeId);
            modelBuilder.Entity<Staff>().HasKey(s => s.StaffId);
            modelBuilder.Entity<Support>().HasKey(s => s.SuppportId);



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
                .HasMany(c => c.Historys)
                .WithOne(h => h.Customer)
                .HasForeignKey(h => h.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Results)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);

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

            modelBuilder.Entity<Order>()
                .HasMany(o => o.PromotionOrders)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId);

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
                .HasMany(p => p.SkinCareRoutineProducts)
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

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionProducts)
                .WithOne(p => p.Promotion)
                .HasForeignKey(p => p.PromotionId);

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionOrders)
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
                .HasMany(q => q.Results)
                .WithOne(r => r.Quiz)
                .HasForeignKey(r => r.QuizId);

            modelBuilder.Entity<SkinCareRoutine>()
                .HasMany(s => s.SkinCareRoutineProducts)
                .WithOne(s => s.SkinCareRoutine)
                .HasForeignKey(s => s.SkinCareRoutineId);

            modelBuilder.Entity<SkinType>()
                .HasMany(s => s.Results)
                .WithOne(r => r.SkinType)
                .HasForeignKey(r => r.SkinTypeId);

            modelBuilder.Entity<SkinType>()
                .HasMany(s => s.SkinCareRoutines)
                .WithOne(s => s.SkinType)
                .HasForeignKey(s => s.SkinTypeId);

            modelBuilder.Entity<Staff>()
                .HasMany(s => s.Supports)
                .WithOne(s => s.Staff)
                .HasForeignKey(s => s.StaffId);

            //end relationship
        }
    }
}
