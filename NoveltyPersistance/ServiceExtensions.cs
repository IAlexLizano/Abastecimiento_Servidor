using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NoveltyApplication.Interfaces;
using NoveltyPersistance.Context;
using NoveltyPersistance.Repository;
using Shared.Global;
using System.Text;

namespace NoveltyPersistance
{
    /// <summary>
    /// Extensiones de servicios para configuración de autenticación y autorización
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Agrega la infraestructura de identidad y autenticación JWT
        /// </summary>
        public static void AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException($"Connection string no encontrado");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

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
            services.AddTransient<INoveltyRepository, NoveltyRepository>();
        }
    }
}
