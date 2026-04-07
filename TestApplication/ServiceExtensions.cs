using TestApplication.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TestApplication
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(cfg => { }, assembly);

            services.AddValidatorsFromAssembly(assembly);

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(assembly));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            return services;
        }
    }
}
