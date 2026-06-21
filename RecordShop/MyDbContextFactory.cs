namespace RecordShop
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {

        public MyDbContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            var connectionString = "server=localhost;database=recordshop;user=root;password=placeholder;";
            
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
            
            optionsBuilder.UseMySql(connectionString,serverVersion);

            return new MyDbContext(optionsBuilder.Options);
        }

    }
}
