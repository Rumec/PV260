using BusinessLayer.DataLoading;
using BusinessLayer.DiffComputing;
using BusinessLayer.Notifications;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementation;
using BusinessLayer.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLayer;
using Microsoft.Extensions.Configuration;
using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace PresentationLayer
{
    public class Program
    {
        private readonly IApp _app;

        public static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public Program(IApp app)
        {
            _app = app;
        }

        public void Run()
        {
            _app.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            
            return Host.CreateDefaultBuilder(args).ConfigureServices(
                services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<IApp, App>();
                    services.AddTransient<IDataSetService, DataSetService>();
                    services.AddTransient<IUserEmailService, UserEmailService>();
                    services.AddTransient<IFileUrlService, FileUrlService>();
                    services.AddTransient<IEmailUi, EmailUi>();
                    services.AddTransient<IDataSetUi, DataSetUi>();
                    services.AddTransient<IConfigUi, ConfigUi>();
                    services.AddTransient<IDataLoader, CsvFileLoader>();
                    services.AddTransient<IDataDownloader, CsvFileDownloader>();
                    services.AddTransient<IDiffComputer, DiffComputer>();
                    services.AddTransient<IEmailSender, GmailSender>();
                    services.AddTransient<IMessageBuilder, HtmlMessageBuilder>();
                    services.AddTransient<IConsoleIoWrapper, ConsoleIoWrapper>();

                    services.AddOptions<SmtpSettings>().Bind(config.GetSection(nameof(SmtpSettings)));
                    
                    services.AddTransient<IDataSyncJob, DataSyncJob>();
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    services.AddDbContext<StockSystemDbContext>(options => options.UseSqlite("DataSource=..\\..\\..\\..\\DataLayer\\app.db"));
                    /*
                     * NOTE: For all UNIX users:
                     *       You have to specify absolute path for some reason (because reasons).
                     *       E.G.: services.AddDbContext<StockSystemDbContext>(options => options.UseSqlite("DataSource=/home/rumec/Documents/c#/cv/PV260/PV260_Project/DataLayer/app.db"));
                     * 
                     * The connection string needs to be changed based on action
                     * for running app it needs to be: DataSource=..\\..\\..\\..\\DataLayer\\app.db
                     * for updating database: DataSource=..\\DataLayer\\app.db
                     *
                     * Migration (use in DataLayer folder):
                     * dotnet ef migrations add NameOfMigration --startup-project ..\PresentationLayer\PresentationLayer.csproj
                     *
                     * Update database after migration (use in DataLayer folder)
                     * dotnet ef database update --startup-project ..\PresentationLayer\PresentationLayer.csproj
                     */
                });
        }
    }
}