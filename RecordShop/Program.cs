
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens; 
using RecordShop.Middleware;
using RecordShop.Repository;
using RecordShop.Services;
using System.ComponentModel.Design;
using System.Text;
using RecordShop.Extensions;
using RecordShop.External;
using RecordShop.Options;

namespace RecordShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<AuthMiddleware>();
          
            // Add services to the container.
            builder.Services.AddScoped<IMusicRecordRepo,MusicRecordRepository>();
            builder.Services.AddScoped<IMusicRecordService, MusicRecordService>();
            builder.Services.AddHttpClient<IDeezerApiClient, DeezerApiClient>();


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
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });
            }

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<CustomLogger>();

            var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
            var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();

            var jwtErrors = jwtOptions.Validate(builder.Environment.IsDevelopment());
            if (jwtErrors.Count > 0)
            {
                foreach (var error in jwtErrors)
                {
                    Console.WriteLine($"❌ {error}");
                }
                Console.WriteLine("❌ Application startup aborted — invalid JWT configuration.");
                Environment.Exit(1); // Hard stop
            }

            builder.Services.Configure<JwtOptions>(jwtSection);

            builder.Services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });

            var app = builder.Build();

            if(!builder.Environment.IsDevelopment())
            {
                await app.EnsureDatabaseConnectionAsync();
            }

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMiddleware<CustomLogger>();
            app.UseMiddleware<AuthMiddleware>();

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
