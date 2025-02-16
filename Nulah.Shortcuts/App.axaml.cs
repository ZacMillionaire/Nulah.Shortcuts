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
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Data;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.ViewModels;
using Nulah.Shortcuts.Views;
using Nulah.Shortcuts.Views.Shortcuts;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using Splat;

namespace Nulah.Shortcuts;

public partial class App : Application
{
	private SimpleReactiveGlobalHook? _taskPoolGlobalHook;

	private MainWindow? _mainWindow;

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);

		// Have to do this after the loader otherwise things just won't load lol!
		if (!Design.IsDesignMode)
		{
			Services();

			InitMainWindow();

			if (Environment.GetEnvironmentVariable("GlobalHooks") == "true")
			{
				// This will cause things to lag like fuck when you hit a break point so try to avoid having it enabled if possible
				InitHooks();
			}
		}
	}

	private void Services()
	{
		var dataLocation = Path.Join(AppContext.BaseDirectory, "data");
		Directory.CreateDirectory(dataLocation);

		Locator.CurrentMutable.RegisterLazySingleton(() => new ShortcutsRepository(new ShortcutsContext(Path.Join(dataLocation, "app.db"))));
	}

	public static T GetRequiredService<T>()
	{
#if DEBUG
		// We don't care if this returns null during design time
#pragma warning disable CS8603 // Possible null reference return.
		if (Design.IsDesignMode)
		{
			return default;
		}
#pragma warning restore CS8603 // Possible null reference return.
#endif

		return Locator.Current.GetService<T>() ?? throw new Exception($"{typeof(T)} not registered");
	}

	private void InitMainWindow()
	{
		_mainWindow = new MainWindow
		{
			IsVisible = true,
			WindowState = WindowState.Maximized,
			CanResize = false,
			ShowInTaskbar = false,
			ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
			ExtendClientAreaTitleBarHeightHint = 0,
			ExtendClientAreaToDecorationsHint = true,
			DataContext = new MainWindowViewModel()
		};

		_mainWindow.Closing += MainWindowOnClosing;
	}

	private void InitHooks()
	{
		_taskPoolGlobalHook = new SimpleReactiveGlobalHook();
		// _taskPoolGlobalHook.HookEnabled.Subscribe(OnHookEnabled);
		_taskPoolGlobalHook.KeyReleased
			.Subscribe(e => OnKeyReleased(e, _taskPoolGlobalHook));
		_taskPoolGlobalHook.RunAsync();
	}

	private void MainWindowOnClosing(object? sender, WindowClosingEventArgs e)
	{
		if (sender is MainWindow mainWindow)
		{
			e.Cancel = true;
			mainWindow.Hide();
		}
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// without this the tray icon will close as soon as the right click menu is dismissed if no other window is
			// open
			desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

			// Only show the main window on start up if the instance was created with it set to true
			if (_mainWindow is { IsVisible: true })
			{
				desktop.MainWindow = _mainWindow;
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