using Auth.Context;
using Auth.DTOs;
using Auth.Interfaces;
using Auth.Services;
using Domains.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Auth
{
    /// <summary>
    /// Extensiones de servicios para configuración de autenticación y autorización
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Agrega la infraestructura de identidad y autenticación JWT
        /// </summary>
        public static void AddAuthenticationInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Configurar JWT Settings
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            var jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
            if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Key))
                throw new InvalidOperationException("JWTSettings no está configurado correctamente en appsettings.json");

            #if DEBUG
            IdentityModelEventSource.ShowPII = true;
            #endif

            // Configurar autenticación JWT
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = "role"
                    };

                    // Manejo de eventos de autenticación
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var response = JsonConvert.SerializeObject(
                                new { message = "Autenticación fallida: Token inválido o expirado" });
                            return context.Response.WriteAsync(response);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var response = JsonConvert.SerializeObject(
                                new { message = "No está autorizado para acceder a este recurso" });
                            return context.Response.WriteAsync(response);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var response = JsonConvert.SerializeObject(
                                new { message = "No tiene permisos para acceder a este recurso" });
                            return context.Response.WriteAsync(response);
                        }
                    };
                });

            // Registrar servicios
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHashingService, HashingService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILoginService, LoginService>();
        }

        /// <summary>
        /// Agrega el DbContext de autenticación
        /// </summary>
        public static void AddAuthenticationDbContext(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName = "DefaultConnection")
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' no encontrado");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
        }
    }
}
