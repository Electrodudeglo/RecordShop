
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecordShop.Middleware;
using RecordShop.Repository;
using RecordShop.Services;
using System.ComponentModel.Design;

namespace RecordShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
            // Add services to the container.
            builder.Services.AddScoped<IMusicRecordRepo,MusicRecordRepository>();
            builder.Services.AddScoped<IMusicRecordService, MusicRecordService>();

            if (builder.Environment.IsDevelopment())
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                var keepAliveConnection = new SqliteConnection(connectionString);
                keepAliveConnection.Open();

                builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite(keepAliveConnection));
            }
            else
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Services.AddDbContext<MyDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            }

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<CustomLogger>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                    db.Database.EnsureCreated();
                    SeedData.Initialize(db);
                }
            }
            else
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                db.Database.Migrate();
                SeedData.Initialize(db);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {             
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<CustomLogger>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
