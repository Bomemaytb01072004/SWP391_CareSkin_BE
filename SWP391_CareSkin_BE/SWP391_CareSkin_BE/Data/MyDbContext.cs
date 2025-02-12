using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Data

{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //region Dbset
        public DbSet<Customers> Customers { get; set; }

        // end Dbset
    }
}
