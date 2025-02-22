using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels;

public partial class ShortcutListViewModel : ViewModelBase<IShortcutListViewModel>, IShortcutListViewModel
{
	private readonly ILogger<ShortcutListViewModel> _logger = NullLogger<ShortcutListViewModel>.Instance;
	private readonly ShortcutsRepository? _shortcutRepository;

	protected readonly SourceCache<ShortcutDto, int> _shortcutCache = new(x => x.Id);
	private readonly ReadOnlyObservableCollection<ShortcutDto> _shortcuts;

	private IObservable<bool> _canCreateShortcut;

	public ReadOnlyObservableCollection<ShortcutDto> Shortcuts => _shortcuts;

	[Reactive]
	private string _title = string.Empty;

	[Reactive]
	private string _link = string.Empty;

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
			x => x._shortcutRepository,
			(x, y, z) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y) && z is not null
		);

		this.WhenActivated(async d =>
		{
			await Task.Delay(1000);
			LoadShortcuts();
			d.Dispose();
		});
	}

	// ReSharper disable once UnusedMember.Global - Used by dependency injection
	public ShortcutListViewModel(IServiceProvider serviceProvider, ILogger<ShortcutListViewModel> logger) : this()
	{
		_logger = logger;
		_shortcutRepository = serviceProvider.GetRequiredService<ShortcutsRepository>();
		_logger.LogInformation("Creating ShortcutListViewModel");
	}

	private void LoadShortcuts()
	{
		if (_shortcutRepository != null)
		{
			_logger.LogInformation("Loading shortcuts");
			var loadedShortcuts = _shortcutRepository.GetShortcuts();

			_shortcutCache.Edit(cache =>
			{
				_logger.LogInformation("adding shortcuts to cache");
				cache.Load(loadedShortcuts);
			});
		}
	}

	[ReactiveCommand(CanExecute = nameof(_canCreateShortcut))]
	private async Task CreateShortcut()
	{
		if (_shortcutRepository is not null)
		{
			await Task.Yield();
			_shortcutCache.AddOrUpdate(_shortcutRepository.CreateShortcut(_title, _link));

			Reset();
		}
	}

	private void Reset()
	{
		Title = Link = string.Empty;
	}
}