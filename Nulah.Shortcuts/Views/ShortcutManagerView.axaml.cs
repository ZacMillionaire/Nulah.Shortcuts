using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nulah.Shortcuts.Models.Interfaces;
using Nulah.Shortcuts.ViewModels;
using ReactiveUI;

namespace Nulah.Shortcuts.Views;

public partial class ShortcutManagerView : ReactiveUserControl<IShortcutManagerViewModel>
{
	private readonly ILogger<ShortcutManagerView> _logger;

	public ShortcutManagerView() : this(NullLogger<ShortcutManagerView>.Instance)
	{
	}

	public ShortcutManagerView(ILogger<ShortcutManagerView> logger)
	{
		_logger = logger;
		InitializeComponent();
		_logger.LogInformation("Shortcut manager view ready");

		this.WhenActivated(disposable =>
		{
			this.OneWayBind(
					ViewModel,
					vm => vm.ShortcutListViewModel,
					view => view.ShortcutList.ViewModel)
				.DisposeWith(disposable);
		});
	}
}