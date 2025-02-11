using Microsoft.EntityFrameworkCore;

namespace  SWP391_CareSkin_BE.Data

{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //region Dbset
        public DbSet<User> Users { get; set; }

        // end Dbset
    }
}
