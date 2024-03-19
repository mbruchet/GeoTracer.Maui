
using GeoTracer.Api.Data;
using GeoTracer.Api.EndPoints;
using GeoTracer.Shared;
using GeoTracer.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GeoTracer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("GeoTracerConnection"), sql =>
                {
                    sql.MigrationsAssembly(typeof(Program).Assembly.FullName);
                });

                if(builder.Environment.IsDevelopment())
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if(!db.Users.Any())
            {
                var admin = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Email = "admin@monexample.com",
                    Name = "admin",
                    JsRoles = "admin",
                    UserName = "admin@monexample.com"
                };

                var hashed = new PasswordHasher().HashPassword("demoPassword01!");
                admin.Password = hashed.Hash;
                admin.SaltPassword = hashed.Salt;

                db.Users.Add(admin);
                db.SaveChanges();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.RegisterAuthenticationEndPoint(builder.Configuration);

            app.Run();
        }
    }
}
