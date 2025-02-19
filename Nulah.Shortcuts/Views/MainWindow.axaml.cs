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

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
	public MainWindow() : this(null)
	{
	}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

	public MainWindow(ILogger<MainWindow> logger)
	{
		_logger = logger;

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
	}

	private void FadeBorder_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.Source is Border { Name: "FadeBorder" })
		{
			// Call close so the MainWindowOnClosing handler becomes the single responsible method
			Close();
		}
	}

	private void MainWindowOnClosing(object? sender, WindowClosingEventArgs e)
	{
		e.Cancel = true;
		Hide();
	}
}