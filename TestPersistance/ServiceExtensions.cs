using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestPersistance.Context;

namespace TestPersistance
{
    public static class ServiceExtensions
    {
        //public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration)
        //{
        //    GPGService PGP = new GPGService();
        //    string cadenaOracleEcuador = PGP.DecryptString(configuration["PGP:CorreoCadenasConexion"].ToString(), configuration.GetConnectionString("DefaultConnectionOracleEcuador"));
            
        //    services.AddDbContext<ApplicationContext>(options => options.UseLazyLoadingProxies(true).UseOracle(
        //       cadenaOracleEcuador,
        //       b => b.MigrationsAssembly(typeof(ApplicationDbContextOracleEcuador).Assembly.FullName)));
            
        //    #region Repositories
        //    //services.AddTransient(typeof(IRepositoryAsync), typeof(RepositoryAsync));
        //    #endregion

        //    #region Verifiacion Token
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    }).AddJwtBearer(o =>
        //    {
        //        o.RequireHttpsMetadata = false;
        //        o.SaveToken = false;
        //        var key = configuration["JWTSettings:Key"];
        //        o.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero,
        //            ValidIssuer = configuration["JWTSettings:Issuer"],
        //            ValidAudience = configuration["JWTSettings:Audience"],
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        //        };

        //        o.Events = new JwtBearerEvents()
        //        {
        //            //OnAuthenticationFailed = c =>
        //            //{
        //            //    c.NoResult();
        //            //    c.Response.StatusCode = 500;
        //            //    c.Response.ContentType = "text/plain";
        //            //    return c.Response.WriteAsync(c.Exception.ToString());
        //            //},
        //            OnChallenge = context =>
        //            {
        //                context.HandleResponse();
        //                context.Response.StatusCode = 401;
        //                context.Response.ContentType = "application/json";
        //                var result = JsonConvert.SerializeObject(new Response<string>("Usted no esta autorizado"));
        //                return context.Response.WriteAsync(result);
        //            },
        //            OnForbidden = context =>
        //            {
        //                context.Response.StatusCode = 400;
        //                context.Response.ContentType = "application/json";
        //                var result = JsonConvert.SerializeObject(new Response<string>("Usted no tiene permisos sobre este recurso"));
        //                return context.Response.WriteAsync(result);
        //            }
        //        };
        //    });
        //    #endregion

        //}
    }
}