using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Domain;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels;

public partial class ShortcutListViewModel : ViewModelBase<IShortcutListViewModel>, IShortcutListViewModel
{
	private readonly ILogger<ShortcutListViewModel> _logger;
	private readonly ShortcutsRepository? _shortcutRepository;

	protected readonly SourceCache<ShortcutDto, int> _shortcutCache = new(x => x.Id);
	private readonly ReadOnlyObservableCollection<ShortcutDto> _shortcuts;

	private IObservable<bool> _canCreateShortcut;

	public ReadOnlyObservableCollection<ShortcutDto> Shortcuts => _shortcuts;

	[Reactive]
	private string _title = string.Empty;

	[Reactive]
	private string _link = string.Empty;

#pragma warning disable CS8618, CS9264
	protected ShortcutListViewModel()
	{
		_shortcutCache
			.Connect()
			.DeferUntilLoaded()
			.Bind(out _shortcuts)
			.Subscribe();

		_canCreateShortcut = this.WhenAnyValue(
			x => x.Title,
			x => x.Link,
			(x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)
		);
	}
#pragma warning restore CS8618, CS9264

	public ShortcutListViewModel(IServiceProvider serviceProvider, ILogger<ShortcutListViewModel> logger) : this()
	{
		_logger = logger;
		_shortcutRepository = serviceProvider.GetRequiredService<ShortcutsRepository>();
		_logger.LogInformation("Creating ShortcutListViewModel");

		LoadShortcuts();
	}

	private void LoadShortcuts()
	{
		if (_shortcutRepository != null)
		{
			_logger?.LogInformation("Loading shortcuts");
			var loadedShortcuts = _shortcutRepository.GetShortcuts();

			_shortcutCache.Edit(cache =>
			{
				_logger?.LogInformation("adding shortcuts to cache");
				cache.Load(loadedShortcuts);
			});
		}
	}

	private int whatever = 0;

	[ReactiveCommand(CanExecute = nameof(_canCreateShortcut))]
	private async Task CreateShortcut()
	{
		await Task.Yield();
		_shortcutCache.AddOrUpdate(new ShortcutDto()
		{
			Id = whatever++,
			Title = _title,
			ShortcutLocation = _link
		});
		Title = Link = string.Empty;
	}
}