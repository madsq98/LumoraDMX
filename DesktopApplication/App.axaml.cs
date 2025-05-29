using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DesktopApplication.ViewModels;
using DesktopApplication.Views;
using FrontendServices;
using FrontendServices.Services.Project;
using FrontendServices.Services.ProjectDmx;
using FrontendServices.Services.SimpleDmx;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;

namespace DesktopApplication
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddSingleton<ProjectService>();
                serviceCollection.AddSingleton<SimpleDmxService>();
                serviceCollection.AddSingleton<ProjectDmxService>();

                Services = serviceCollection.BuildServiceProvider();

                DisableAvaloniaDataAnnotationValidation();

                var splash = new SplashWindow();
                splash.Show();

                Dispatcher.UIThread.Post(async () =>
                {
                    await Task.Delay(3000); // Simulate loading time

                    string? backendAddress = null;

                    try
                    {
                        var discovered = await NetworkAnnouncer.DiscoverAsync(NetworkAnnouncer.DISCOVER_TYPE);

                        if (discovered != null && discovered.Count > 0)
                        {
                            var service = discovered.First();
                            backendAddress = $"http://{service.IPAddress}:{service.Port}";
                            Console.WriteLine($"Found backend at {backendAddress}");
                        }
                        else
                        {
                            Console.WriteLine("Backend service not found in Zeroconf response.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error discovering backend: {ex.Message}");
                    }

                    if (!string.IsNullOrEmpty(backendAddress))
                    {
                        Services.GetRequiredService<ProjectService>().SetBackend(backendAddress);
                        Services.GetRequiredService<SimpleDmxService>().SetBackend(backendAddress);

                        var mainWindow = new MainWindow
                        {
                            DataContext = new MainWindowViewModel(Services)
                        };

                        desktop.MainWindow = mainWindow;
                        mainWindow.Show();

                        var msgBox = MessageBoxManager.GetMessageBoxStandard("Backend", $"Access swagger at {backendAddress}/swagger");
                        //await msgBox.ShowAsync();
                    }
                    else
                    {
                        var msgBox = MessageBoxManager.GetMessageBoxStandard("Error", "Could not locate Lumora DMX backend on the network.");

                        await msgBox.ShowAsync();
                    }

                    splash.Close();
                });
            }

            AppDomain.CurrentDomain.ProcessExit += (_, __) =>
            {
                var simpleDmxService = Services.GetRequiredService<SimpleDmxService>();

                for(int i = 1; i <= 255; i++)
                {
                    _ = simpleDmxService.SetDmxChannel(i, 0);
                }
                // Final cleanup
                Console.WriteLine("ProcessExit called!");
            };

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}