using System.Configuration;
using BusinessLayer;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation_Tier;
using PresentationLayer.Helper;

namespace PresentationLayer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain(ServiceProvider.GetRequiredService<AppServices>()));
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"]
                .ConnectionString;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddTransient<IPatientService,PatientService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<AppServices>();

        }

    }
}