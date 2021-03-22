using Microsoft.Extensions.Configuration;
using NetAPorter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetAPorter.Factory
{
    public class AppSettingsFactory
    {
        private IConfiguration Configuration { get; }

        public AppSettingsFactory()
        {
            var env = Environment.GetEnvironmentVariable("TEST_RUN_ENVIRONMENT") ?? "local";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public AppSettings LoadAppSettings()
        {
            var appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            return appSettings;
        }
    }
}
