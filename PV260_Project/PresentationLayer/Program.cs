using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PresentationLayer.ConsoleApps;

namespace PresentationLayer
{
    public class Program
    {
        private readonly IApp _app;

        public static void Main() {
            var args = Environment.GetCommandLineArgs();
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
            var consoleApp = new ConsoleApp();
            consoleApp.Run();
        }

        public Program(IApp app) {
            _app = app;
        }

        public void Run() {
            _app.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args).ConfigureServices(
                services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<IApp, ConsoleApp>();
                });
        }
    }
}
