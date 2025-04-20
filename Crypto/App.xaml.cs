using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Crypto.Modal;
using Crypto.View;
using Crypto.ViewModal;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            var services = new ServiceCollection();

            
            services.AddSingleton<IWindowService, WindowService>();

           
            services.AddTransient<MainViewModal>();
            services.AddTransient<MainWindow>();
            services.AddTransient<CalculatorViewModal>();
            services.AddTransient<CalculatorWindow>();


            _serviceProvider = services.BuildServiceProvider();

            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}


