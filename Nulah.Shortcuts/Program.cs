using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Data;
using Nulah.Shortcuts.Models.Interfaces;
using Nulah.Shortcuts.ViewModels;
using Nulah.Shortcuts.ViewModels.RecycleBin;
using Nulah.Shortcuts.ViewModels.Shortcuts;
using Nulah.Shortcuts.Views;
using Nulah.Shortcuts.Views.RecycleBin;
using Nulah.Shortcuts.Views.Shortcuts;
using ReactiveUI;
using Splat;
using ShortcutListViewModel = Nulah.Shortcuts.ViewModels.Shortcuts.ShortcutListViewModel;

namespace Nulah.Shortcuts;

sealed class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		using var host = new HostBuilder()
			.ConfigureServices(ConfigureServices)
			.Build();

		host.StartAsync();

		BuildAvaloniaApp(host.Services)
			.StartWithClassicDesktopLifetime(args);
	}

	private static void ConfigureServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<App>()
			.AddSingleton<ServiceViewLocator>()
			// Contexts
			.AddSingleton<ShortcutsContext>(_ =>
			{
				var dataLocation = Path.Join(AppContext.BaseDirectory, "data");
				Directory.CreateDirectory(dataLocation);
				return new ShortcutsContext(Path.Join(dataLocation, "app.db"));
			})
			// Core
			.AddTransient<ImageProcessing>()
			// Main window requirements
			.AddSingleton<MainWindow>()
			.AddTransient<MainWindowViewModel>()
			// Repositories
			.AddSingleton<ShortcutsRepository>()
			// View models
			.AddViewModel<IShortcutListViewModel, ShortcutListViewModel>()
			.AddViewModel<IShortcutManagerViewModel, ShortcutManagerViewModel>()
			.AddViewModel<IShortcutCreateEditViewModel, ShortcutCreateEditViewModel>()
			.AddViewModel<IRecycleBinViewModel, RecycleBinViewModel>()
			// Views
			.AddView<ShortcutList, IShortcutListViewModel>()
			.AddView<ShortcutManagerView, IShortcutManagerViewModel>()
			.AddView<ShortcutCreateEdit, IShortcutCreateEditViewModel>()
			.AddView<RecycleBinView, IRecycleBinViewModel>()
			.AddLogging(builder => builder.AddConsole());
	}


	// Avalonia configuration, don't remove; also used by visual designer.
	// ReSharper disable once UnusedMember.Global
	public static AppBuilder BuildAvaloniaApp()
	{
		// For design time we only care about the collection
		var designTimeServiceCollection = new ServiceCollection();
		ConfigureServices(designTimeServiceCollection);

		return BuildAvaloniaApp(designTimeServiceCollection.BuildServiceProvider());
	}

	private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
	{
		Locator.CurrentMutable.UnregisterCurrent(typeof(IViewLocator));
		Locator.CurrentMutable.Register(serviceProvider.GetRequiredService<ServiceViewLocator>, typeof(IViewLocator));

		var appBuilder = AppBuilder.Configure(serviceProvider.GetRequiredService<App>)
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();

		return appBuilder;
	}
}