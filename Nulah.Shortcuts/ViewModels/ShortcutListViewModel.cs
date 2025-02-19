using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Domain;
using ReactiveUI;

namespace Nulah.Shortcuts.ViewModels;

public interface IShortcutListViewModel : IViewModelInterface
{
}

public class ShortcutListViewModel : ViewModelBase<IShortcutListViewModel>, IShortcutListViewModel
{
	private readonly ILogger<ShortcutListViewModel> _logger;
	private readonly ShortcutsRepository? _shortcutRepository;

	protected readonly SourceCache<ShortcutDto, int> _shortcutCache = new(x => x.Id);
	private readonly ReadOnlyObservableCollection<ShortcutDto> _shortcuts;

	public ReadOnlyObservableCollection<ShortcutDto> Shortcuts => _shortcuts;

	protected ShortcutListViewModel()
	{
		_shortcutCache
			.Connect()
			.DeferUntilLoaded()
			.Bind(out _shortcuts)
			.Subscribe();
	}

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
}

public class ShortcutListDesignModel : ShortcutListViewModel
{
	public ShortcutListDesignModel()
	{
		_shortcutCache.AddOrUpdate(Enumerable.Range(1, 10)
			.Select(x => new ShortcutDto()
			{
				Id = x,
				Title = $"Shortcut not from database {x}",
				ShortcutLocation = $"{x}/whatever/a/b"
			}));
	}
}