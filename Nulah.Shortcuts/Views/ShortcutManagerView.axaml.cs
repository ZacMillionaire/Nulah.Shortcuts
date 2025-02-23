using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nulah.Shortcuts.ViewModels.Shortcuts;
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
		_logger.LogInformation("Shortcut manager view ready");

		this.WhenActivated(disposable =>
		{
			this.OneWayBind(
					ViewModel,
					vm => vm.ShortcutListViewModel,
					view => view.ShortcutList.ViewModel)
				.DisposeWith(disposable);
			
			this.OneWayBind(
					ViewModel,
					vm => vm.ShortcutActionViewModel,
					view => view.ShortcutAction.ViewModel)
				.DisposeWith(disposable);
		});
		InitializeComponent();
	}
}