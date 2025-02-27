using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenAIChat.Infrastructure.Database.Sqlite.Migrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sql Migrations console starts");

            var builder = Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   // register AutoMapper to scan all assemblies in the current domain
                   services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

                   services.AddGenAiChatInfrastructureDatabase(hostContext.Configuration);
               });
            
            builder.Build();

            Console.WriteLine("Sql Migrations console ends");
        }
    }
}
