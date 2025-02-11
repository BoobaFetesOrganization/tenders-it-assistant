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
                   services.AddGenAiChatInfrastructureDatabaseSqlLite(hostContext.Configuration);
               });
            
            builder.Build();

            Console.WriteLine("Sql Migrations console ends");
        }
    }
}
