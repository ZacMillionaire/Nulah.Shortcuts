using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Nulah.Shortcuts.ViewModels;
using Nulah.Shortcuts.Views;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace Nulah.Shortcuts;

public partial class App : Application
{
	private SimpleReactiveGlobalHook? _taskPoolGlobalHook;

	private MainWindow? _mainWindow;

	public override void Initialize()
	{
		if (!Design.IsDesignMode)
		{
			InitMainWindow();
			InitHooks();
		}

		AvaloniaXamlLoader.Load(this);
	}

	private void InitMainWindow()
	{
		_mainWindow = new MainWindow()
		{
			//	IsVisible = false
			WindowState = WindowState.Maximized,
			CanResize = false,
			ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
			ExtendClientAreaTitleBarHeightHint = 0,
			ExtendClientAreaToDecorationsHint = true,
		};
		_mainWindow.DataContext = new MainWindowViewModel();
		_mainWindow.Closing += MainWindowOnClosing;
	}

	private void InitHooks()
	{
		_taskPoolGlobalHook = new SimpleReactiveGlobalHook(TaskPoolScheduler.Default);
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
			//desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
			desktop.MainWindow = _mainWindow;
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void OnKeyReleased(KeyboardHookEventArgs e, IReactiveGlobalHook hook)
	{
		if (e.RawEvent.Mask == (ModifierMask.LeftMeta | ModifierMask.LeftShift)
		    && (e.Data.KeyCode == KeyCode.VcC || e.Data.KeyCode == KeyCode.VcLeftMeta || e.Data.KeyCode == KeyCode.VcLeftShift)
		    && _mainWindow is not null)
		{
			Dispatcher.UIThread.Invoke(() =>
			{
				if (!_mainWindow.IsVisible)
				{
					_mainWindow.WindowState = WindowState.Maximized;
					_mainWindow.Show();
					Console.WriteLine("Open magic window");
				}
			});
		}
	}
}