using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UseCases;
using UseCases.UserGroup;
using System.Text;
using Domain;
using Repository;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
            var appSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>() ?? default!;
            var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>() ?? default!;
            builder.Services.AddSingleton(appSettings);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(connectionStr, x => x.MigrationsAssembly("Domain")));

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("REFRESHTOKENPROVIDER");

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromSeconds(appSettings.RefreshTokenExpireSeconds);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidIssuer = appSettings.Issuer,
                        ValidAudience = appSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey)),
                        ClockSkew = TimeSpan.FromSeconds(0)
                    };
                });

                   
            // Register EmailService with its dependencies
            builder.Services.AddTransient<IEmailService, EmailService>(provider => new EmailService(
                smtpHost: smtpSettings.Host,
                smtpPort: smtpSettings.Port,
                smtpUser: smtpSettings.User,
                smtpPass: smtpSettings.Password
            ));
            builder.Services.AddTransient<TodoUseCases>();
            builder.Services.AddScoped<ISharedTodoRepository, SharedTodoRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            builder.Services.AddScoped<ITokenUtil, TokenUtil>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
            
            builder.Services.AddTransient<UserService>();
         
     
                
                // Other service registrations...
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("webAppRequests", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(appSettings.Audience)
                    .AllowCredentials();
                });
            });

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "App Api", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(
                    new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using var scope = app.Services.CreateScope();
                //var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                //initialiser.InitialiseAsync();
            }
            app.UseHttpsRedirection();
            app.UseCors("webAppRequests");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}