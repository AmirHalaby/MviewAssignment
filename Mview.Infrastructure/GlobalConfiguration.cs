using Microsoft.Extensions.Configuration;
using Mview.Infrastructure.ConnectionStringReader;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Infrastructure
{
    public static class GlobalConfiguration
    {
        public static string ConnectionString { get; private set; } = string.Empty;

        public static async Task LoadConfiguration(IConfiguration configuration)
        {
            ConnectionString = await ConnectionStringBuilder.GetConnectionString(configuration.GetConnectionString("MviewDB"));

            
        }
    }
}
