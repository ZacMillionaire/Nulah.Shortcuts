using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Data;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.ViewModels;
using Nulah.Shortcuts.Views;
using Nulah.Shortcuts.Views.Shortcuts;
using ReactiveUI;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using Splat;

namespace Nulah.Shortcuts;

public partial class App : Application
{
	private readonly IServiceProvider _provider;
	private readonly ILogger<App> _logger;
	private SimpleReactiveGlobalHook? _taskPoolGlobalHook;

	private MainWindow? _mainWindow;

#pragma warning disable CS8618, CS9264
	public App(){}
#pragma warning restore CS8618, CS9264
	
	public App(IServiceProvider provider, ILogger<App> logger)
	{
		_provider = provider;
		_logger = logger;
	}

	public override void Initialize()
	{
		_logger.LogInformation("Initialising App");
		AvaloniaXamlLoader.Load(this);
		_logger.LogInformation("AvaloniaXamlLoader Loaded");

		// Have to do this after the loader otherwise things just won't load lol!
		// if (!Design.IsDesignMode)
		// {
		// 	Services();
		//
		// 	InitMainWindow();
		//
		// 	if (Environment.GetEnvironmentVariable("GlobalHooks") == "true")
		// 	{
		// 		// This will cause things to lag like fuck when you hit a break point so try to avoid having it enabled if possible
		// 		InitHooks();
		// 	}
		// }
	}

	private void InitHooks()
	{
		_taskPoolGlobalHook = new SimpleReactiveGlobalHook();
		// _taskPoolGlobalHook.HookEnabled.Subscribe(OnHookEnabled);
		_taskPoolGlobalHook.KeyReleased
			.Subscribe(e => OnKeyReleased(e, _taskPoolGlobalHook));
		_taskPoolGlobalHook.RunAsync();
	}

	public override void OnFrameworkInitializationCompleted()
	{
		_logger.LogInformation("Framework Initilization Complete");

		Locator.CurrentMutable.UnregisterCurrent(typeof(IViewLocator));
		Locator.CurrentMutable.Register(_provider.GetRequiredService<ServiceViewLocator>, typeof(IViewLocator));

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// without this the tray icon will close as soon as the right click menu is dismissed if no other window is
			// open
			desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

			_mainWindow = _provider.GetRequiredService<MainWindow>();
			// the main window does not automatically locate its viewmodel so we set it once
			_mainWindow.ViewModel = _provider.GetRequiredService<MainWindowViewModel>();
			if (_mainWindow is { IsVisible: true })
			{
				_logger.LogInformation("Displaying MainWindow on startup");
				desktop.MainWindow = _mainWindow;
				desktop.MainWindow.BringIntoView();
			}
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void OnKeyReleased(KeyboardHookEventArgs e, IReactiveGlobalHook hook)
	{
		if (e.RawEvent.Mask == (ModifierMask.LeftMeta | ModifierMask.LeftShift)
		    && (e.Data.KeyCode == KeyCode.VcC || e.Data.KeyCode == KeyCode.VcLeftMeta || e.Data.KeyCode == KeyCode.VcLeftShift))
		{
			ShowMainWindow();
		}
	}

	private void ShowMainWindow()
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			if (_mainWindow is { IsVisible: false })
			{
				_mainWindow.WindowState = WindowState.Maximized;
				_mainWindow.Show();
				Console.WriteLine("Open magic window");
			}
		});
	}

	private void OpenWindowTrayMenuItem_OnClick(object? sender, EventArgs e)
	{
		ShowMainWindow();
	}
}