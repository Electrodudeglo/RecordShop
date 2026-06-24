
namespace RecordShop.Extensions
{
    using MySqlConnector;
    using RecordShop;
    public static class DatabaseStartupExtensions
    {
        public static async Task EnsureDatabaseConnectionAsync(this WebApplication app)
        {
            var config = app.Services.GetRequiredService<IConfiguration>();
            string connectionString = config.GetConnectionString("DefaultConnection") ?? "connection string not found. Check appsettingsJson Files";

            if(!await TryToConnect(connectionString))
            {
                Console.WriteLine("❌ Application startup aborted — cannot reach database.");
                Environment.Exit(1); // Hard stop
            }
            Console.WriteLine("✅ Database connection successful.");
        }
        public static async Task<bool> TryToConnect(string conn)
        {
            try
            {
                using var connection = new MySqlConnection(conn);
                await connection.OpenAsync();
                using var cmd = new MySqlCommand("SELECT 1;", connection);
                await cmd.ExecuteScalarAsync();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"❌ Database connection failed: {e.Message}");
                return false;
            }            
        }
    }   
}
