using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.ViewModels;
using Nulah.Shortcuts.Views;
using ReactiveUI;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using Splat;

namespace Nulah.Shortcuts;

public class App : Application
{
	private readonly IServiceProvider _provider;
	private readonly ILogger<App> _logger;
	private SimpleReactiveGlobalHook? _taskPoolGlobalHook;

	private MainWindow? _mainWindow;

#pragma warning disable CS8618, CS9264
	public App()
	{
	}
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

		if (!Design.IsDesignMode)
		{
			if (Environment.GetEnvironmentVariable("GlobalHooks") == "false")
			{
				// This will cause things to lag like fuck when you hit a break point so try to avoid having it enabled if possible
				InitHooks();
			}
		}
	}

	/// <summary>
	/// Returns the current main window used for classic desktop style applications
	/// </summary>
	/// <returns></returns>
	public static MainWindow? GetMainWindow()
	{
		if (Current is { ApplicationLifetime: IClassicDesktopStyleApplicationLifetime desktop })
		{
			return desktop.MainWindow as MainWindow;
		}

		return null;
	}

	private void InitHooks()
	{
		_taskPoolGlobalHook = new SimpleReactiveGlobalHook(GlobalHookType.Keyboard);
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
				// TODO: I'd love to figure out a way to avoid the mainwindow showing until the content view is fully ready
				desktop.MainWindow = _mainWindow;
			}
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void ShowMainWindow()
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			if (_mainWindow is { IsVisible: false })
			{
				ShowAndBringToFront(_mainWindow);
			}
		});
	}

	private void OnKeyReleased(KeyboardHookEventArgs e, IReactiveGlobalHook hook)
	{
		if (e.RawEvent.Mask == (ModifierMask.LeftMeta | ModifierMask.LeftShift)
		    // This combination is to react when any of the 3 keys are lifted, its kind of jank
		    // but so is keybinding in general.
		    // Technically win+shift+c+literally every other key would be a valid combination, but as long as
		    // one of the 3 primary keys are released it'll trigger.
		    // Is that a bug? No. It's a funny feature and if someone wants to do it?
		    // d=====(￣▽￣*)b good for them :D
		    && e.Data.KeyCode is KeyCode.VcC or KeyCode.VcLeftMeta or KeyCode.VcLeftShift)
		{
			ShowMainWindow();
		}
	}

	private void OpenWindowTrayMenuItem_OnClick(object? sender, EventArgs e)
	{
		ShowMainWindow();
	}

	private void ShowAndBringToFront(MainWindow window)
	{
		window.Show();
		// Ensure it's displayed on top
		window.Activate();
	}
}