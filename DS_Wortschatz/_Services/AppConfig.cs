using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DS_Wortschatz._Services
{
    public static class AppConfig
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("_Services/appsettings.json", optional: false, reloadOnChange: true);
                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }
    }
}
