using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RecordShop.Model;

namespace RecordShop
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<MusicRecordModel> MusicRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
