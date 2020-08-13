using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProjectDependenciesVisualizer.Engine;
using ProjectDependenciesVisualizer.Engine.Interfaces;

namespace ProjectDependenciesVisualizer.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IRunner, DotNetRunner>();
            services.AddSingleton<IDependencyGraphGenerator, DependencyGraphService>();
            services.AddSingleton<ISingleProjectAnalyzer, SingleProjectAnalyzer>();
            services.AddSingleton<IFrameworkTargetAnalyzer, FrameworkTargetAnalyzer>();
            services.AddSingleton<IProjectsAnalyzer, ProjectsAnalyzer>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
