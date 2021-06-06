using EdlightApiServer.Hubs;
using EdlightApiServer.Services.HashingService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqliteDataExecuter;
using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace EdlightApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IHashingService), typeof(HashingImplementation));
            services.AddControllers();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<EntityChangedHub>($"/{nameof(EntityChangedHub)}");
            });

            if (Environment.GetCommandLineArgs().Length == 2)
            {
                DBConnector.UseTestBase = Environment.GetCommandLineArgs()[1] == "TestDB";
            }
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU");

            if (!Directory.Exists(Environment.CurrentDirectory + "\\Files")) Directory.CreateDirectory(Environment.CurrentDirectory + "\\Files");
            if (!Directory.Exists(Environment.CurrentDirectory + "\\Plans")) Directory.CreateDirectory(Environment.CurrentDirectory + "\\Plans");

            Console.WriteLine("EntityChangedHub configured");
            Console.WriteLine("Files folder exist/created");
            Console.WriteLine("UI lang testing:");
            Console.WriteLine("current CI = " + CultureInfo.CurrentCulture.Name);
            Console.WriteLine("Test DateTimes:");
            Console.WriteLine(DateTime.Now.ToShortDateString());
            Console.WriteLine(DateTime.Now.ToShortTimeString());
            Console.WriteLine($"Use TestDB : {DBConnector.UseTestBase}");
            Console.WriteLine($"DB Path : {DBConnector.DBPath()}");
            Console.WriteLine($"DB exist : {File.Exists(DBConnector.DBPath())}");
            Console.WriteLine($"Hostname is : {Dns.GetHostName()}");
            Console.WriteLine("Current version 2.0.0_Stable");
        }
    }
}
