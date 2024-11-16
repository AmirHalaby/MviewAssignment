using Microsoft.Extensions.DependencyInjection;
using Mview.Application.BL.LicensePlateChecks;
using Mview.Application.Services.LicensePlateChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services) 
        {
            var appliactionAssembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddScoped<ILicensePlateChecksBL, LicensePlateChecksBL>();
            services.AddScoped<ILicensePlateChecksService, LicensePlateChecksService>();
            services.AddAutoMapper(appliactionAssembly);

        }
    }
}
