using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Data

{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //region Dbset
        public DbSet<User> Users { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionProduct> promotionProducts { get; set; }
        public DbSet<SkinCareRoutine> SkinCareRoutines { get; set; }
        public DbSet<SkinCareRoutineProduct> SkinCareRoutineProducts { get; set; }
        public DbSet<SkinType> skinTypes { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        // end Dbset


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //primary key 
            modelBuilder.Entity<User>().HasKey(u => u.IdUser);
            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Staff>().HasKey(s => s.Staff_Id);
            modelBuilder.Entity<Brand>().HasKey(b => b.Brand_Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Product_Id);
            modelBuilder.Entity<SkinType>().HasKey(st => st.Id);
            modelBuilder.Entity<SkinCareRoutine>().HasKey(scr => scr.Id);
            modelBuilder.Entity<Promotion>().HasKey(p => p.Id);
            modelBuilder.Entity<Cart>().HasKey(c => c.Id);
            modelBuilder.Entity<FAQ>().HasKey(f => f.Id);
            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.Order_ID, op.Product_ID });
            modelBuilder.Entity<PromotionProduct>().HasKey(pp => new { pp.Product_Id, pp.Promotion_Id });
            modelBuilder.Entity<SkinCareRoutineProduct>().HasKey(srp => new { srp.SkinCare_Routine_Id, srp.Product_Id });
        
            // end primary key


            //relationship

            modelBuilder.Entity<Cart>()
               .HasOne(c => c.Product)
               .WithMany()
               .HasForeignKey(c => c.Product_Id);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.Product_ID);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brands)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.Brand_ID);

            modelBuilder.Entity<PromotionProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.PromotionProducts)
                .HasForeignKey(pp => pp.Product_Id);

            modelBuilder.Entity<PromotionProduct>()
                .HasOne(pp => pp.Promotion)
                .WithMany(pr => pr.PromotionProducts)
                .HasForeignKey(pp => pp.Promotion_Id);

            modelBuilder.Entity<SkinCareRoutine>()
                .HasOne(sr => sr.SkinType)
                .WithMany()
                .HasForeignKey(sr => sr.Skin_Type_Id);

            modelBuilder.Entity<SkinCareRoutineProduct>()
                .HasOne(srp => srp.SkinCareRoutine)
                .WithMany(scr => scr.SkinCareRoutineProducts)
                .HasForeignKey(srp => srp.SkinCare_Routine_Id);

            modelBuilder.Entity<SkinCareRoutineProduct>()
                .HasOne(srp => srp.Product)
                .WithMany(p => p.SkinCareRoutineProducts)
                .HasForeignKey(srp => srp.Product_Id);


            //end relationship
        }
    }
}
