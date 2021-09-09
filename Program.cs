using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ElectronNET.API;

namespace SieveToolkit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Console.WriteLine("hi");
                    webBuilder.UseElectron(args).UseKestrel(options => options.ListenAnyIP(Convert.ToInt32(BridgeSettings.WebPort), opt => opt.UseHttps("localhost.pfx", "mahan1387")));
                    webBuilder.UseStartup<Startup>();
                });
    }
}
