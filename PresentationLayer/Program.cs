using Microsoft.Extensions.DependencyInjection;
using Presentation_Tier;

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
            Application.Run(new frmMain());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            
        }

    }
}