using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.ViewModels;
using ReactiveUI;

namespace Nulah.Shortcuts.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
	private readonly ILogger _logger;

	public MainWindow() : this(null)
	{
		Debugger.Launch();
	}

	public MainWindow(ILogger<MainWindow> logger)
	{
		Debugger.Launch();
		_logger = logger;
		_logger.LogInformation("Creating MainWindow");

		InitializeComponent();

		this.WhenActivated(disposable =>
		{
			this.OneWayBind(
					ViewModel,
					vm => vm.ShortcutListViewModel,
					view => view.ShortcutList.ViewModel
				)
				.DisposeWith(disposable);
		});
		
		IsVisible = true;
		WindowState = WindowState.Maximized;
		CanResize = false;
		ShowInTaskbar = false;
		ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
		ExtendClientAreaTitleBarHeightHint = 0;
		ExtendClientAreaToDecorationsHint = true;

		Closing += MainWindowOnClosing;
		
		_logger.LogInformation("MainWindow Created");
	}

	private void FadeBorder_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.Source is Border { Name: "FadeBorder" })
		{
			_logger.LogInformation("FadeBorder clicked");
			// Call close so the MainWindowOnClosing handler becomes the single responsible method
			Close();
		}
	}

	private void MainWindowOnClosing(object? sender, WindowClosingEventArgs e)
	{
		_logger.LogInformation("Hiding MainWindow");
		e.Cancel = true;
		Hide();
	}
}