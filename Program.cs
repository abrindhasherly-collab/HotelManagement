using System.Text;
using HotelManagement.Data;
using HotelManagement.Mapper;
using HotelManagement.Repository;
using HotelManagement.Service;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Log4net
            var logRepository = LogManager.GetRepository(
                System.Reflection.Assembly.GetEntryAssembly()!);

            XmlConfigurator.Configure(
                logRepository,
                new FileInfo("log4net.config"));

            var builder = WebApplication.CreateBuilder(args);

            // Logging
            builder.Logging.ClearProviders();

            // Controllers
            builder.Services.AddControllers();

            // OpenAPI
            builder.Services.AddOpenApi();

            // Repository
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();

            // Service
            builder.Services.AddScoped<IHotelService, HotelService>();

            // DbContext
            builder.Services.AddDbContext<HotelManagementDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString(
                            "DefaultConnection"));
                });

            // AutoMapper
            builder.Services.AddAutoMapper(
                p => p.AddProfile<HotelProfile>());

            // JWT Settings
            var jwtSettings =
                builder.Configuration.GetSection("JwtSettings");

            var jwtKey = jwtSettings["Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception(
                    "JwtSettings:Key is missing in appsettings.json");
            }

            // JWT Authentication
            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                jwtSettings["Issuer"],

                            ValidAudience =
                                jwtSettings["Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey))
                        };
                });

            // Authorization
            builder.Services.AddAuthorization();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // OpenAPI
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // CORS
            app.UseCors("AllowAngular");

            // Authentication MUST come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers
            app.MapControllers();

            // API URL
            app.Run("http://localhost:5000");
        }
    }
}