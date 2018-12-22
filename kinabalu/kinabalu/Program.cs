using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kinabalu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel((context, options) =>
                {
                    options.ListenAnyIP(5005, listenOptions =>
                    {
                        listenOptions.UseHttps(httpsOptions =>
                        {
                            var localhostCert = CertificateLoader.LoadFromStoreCert(
                                "localhost", "My", StoreLocation.CurrentUser,
                                allowInvalid: true);

                            httpsOptions.ServerCertificateSelector = (connectionContext, name) => localhostCert;
                        });
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory());
    }
}
