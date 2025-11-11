using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WpfAppMvvmDI
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // Registrar nuestros ViewModels como "Singleton"
            // (una sola instancia para toda la app)
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<SettingsViewModel>();

            // Registrar la Vista (MainWindow)
            // Se crea una nueva cada vez que se pide (Transient),
            // aunque aquí solo la pediremos una vez.
            services.AddTransient<MainWindow>();

            // Construir el proveedor de servicios
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Al arrancar, "pedir" una instancia de MainWindow al contenedor de DI
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            // Mostrar la ventana manualmente
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
