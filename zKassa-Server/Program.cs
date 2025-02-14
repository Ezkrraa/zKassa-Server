using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using SwaggerThemes;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server
{
    public class Program
    {
        public const string fastLimitName = "FastLimit";
        public const string slowLimitName = "SlowLimit";
        public const string verySlowLimitName = "VerySlowLimit";

        public static void Main(string[] args)
        {
#if DEBUG
            // don't allow seeding in prod
            if (args.Any(arg => arg.Equals("--seed-db")))
            {
                Seeder.SeedDatabase();
                Console.WriteLine("Seeded database successfully");
                return;
            }
#endif
            if (args.Any(arg => arg.Equals("--create-db")))
            {
                // can't always create a database on startup if we want migrations to work
                Seeder.CreateDatabase();
            }

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Bearer";
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                        ),
                    };
                });

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddFixedWindowLimiter(
                    fastLimitName,
                    limit =>
                    {
                        limit.QueueLimit = 2;
                        limit.Window = TimeSpan.FromSeconds(5);
                        limit.PermitLimit = 10;
                        limit.AutoReplenishment = true;
                    }
                );
                options.AddFixedWindowLimiter(
                    slowLimitName,
                    limit =>
                    {
                        limit.QueueLimit = 0;
                        limit.Window = TimeSpan.FromMinutes(1);
                        limit.PermitLimit = 10;
                        limit.AutoReplenishment = true;
                    }
                );
                options.AddFixedWindowLimiter(
                    verySlowLimitName, // for auth and creating distribution centers
                    limit =>
                    {
                        limit.QueueLimit = 0;
                        limit.Window = TimeSpan.FromMinutes(60);
                        limit.PermitLimit = 40;
                        limit.AutoReplenishment = true;
                    }
                );
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ZDbContext>();

            builder
                .Services.AddIdentity<Employee, IdentityRole>(o =>
                {
                    o.Password.RequiredLength = 6;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ZDbContext>()
                .AddUserStore<UserStore<Employee, IdentityRole, ZDbContext>>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

#if DEBUG
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                });
            });
#endif

            builder.Services.AddTransient<JwtService>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(SwaggerThemes.Theme.Monokai);
            }

            //app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
