using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Prospecto.ViewMvc
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
              webBuilder
                  .UseStartup<Startup>()
                  .UseUrls("http://0.0.0.0:5000", "https://localhost:44323");
          });
    }
}
