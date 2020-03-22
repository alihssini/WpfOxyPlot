using Microsoft.Extensions.DependencyInjection;
using WpfChallenge.DataProvider;
using System.Windows;

namespace WpfChallenge
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public readonly ServiceProvider ServiceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFitter, LinearFitter>();
            services.AddSingleton<IFitter, PowerFitter>();
            services.AddSingleton<IFitter, ExponentialFitter>();
            services.AddSingleton<IPointsFileReader, PointsFileReader>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
