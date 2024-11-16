using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mview.Domain.IRepositories;
using Mview.Domain.Repositories;


namespace Mview.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomain(this IServiceCollection service)
        {
            service.AddScoped<ILicensePlateChecksRepository, LicensePlateChecksRepository>();


        }
    }
}
