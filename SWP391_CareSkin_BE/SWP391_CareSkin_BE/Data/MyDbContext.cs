using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Data

{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //region Dbset
        public DbSet<Customers> Customers { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Promotions> Promotions { get; set; }
        public DbSet<PromotionProducts> promotionProducts { get; set; }
        public DbSet<SkinCareRoutines> SkinCareRoutines { get; set; }
        public DbSet<SkinCareRoutineProducts> SkinCareRoutineProducts { get; set; }
        public DbSet<SkinTypes> skinTypes { get; set; }
        public DbSet<Staffs> Staffs { get; set; }

        // end Dbset


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //primary key 
            modelBuilder.Entity<Customers>().HasKey(u => u.CustomerId);
            modelBuilder.Entity<Admin>().HasKey(a => a.AdminId);
            modelBuilder.Entity<Staffs>().HasKey(s => s.StaffId);
            modelBuilder.Entity<Brands>().HasKey(b => b.BrandId);
            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
            modelBuilder.Entity<SkinTypes>().HasKey(st => st.Id);
            modelBuilder.Entity<SkinCareRoutines>().HasKey(scr => scr.Id);
            modelBuilder.Entity<Promotions>().HasKey(p => p.PromotionId);
            modelBuilder.Entity<Carts>().HasKey(c => c.CartId);
            modelBuilder.Entity<FAQ>().HasKey(f => f.FAQId);
            modelBuilder.Entity<OrderProducts>().HasKey(op => op.OrderProductId);
            modelBuilder.Entity<PromotionProducts>().HasKey(pp => new { pp.ProductId, pp.PromotionId });
            modelBuilder.Entity<SkinCareRoutineProducts>().HasKey(srp => new { srp.SkinCareRoutineId, srp.ProductId });
        
            // end primary key


            //relationship

            

            //end relationship
        }
    }
}
