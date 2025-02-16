using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Domain;
using ReactiveUI;

namespace Nulah.Shortcuts.ViewModels;

public class ShortcutListViewModel : ViewModelBase
{
	private readonly ShortcutsRepository? _shortcutRepository;

	protected readonly SourceCache<ShortcutDto, int> _shortcutCache = new(x => x.Id);
	private readonly ReadOnlyObservableCollection<ShortcutDto> _shortcuts;

	public ReadOnlyObservableCollection<ShortcutDto> Shortcuts => _shortcuts;

	public ShortcutListViewModel(ShortcutsRepository? shortcutRepository = null)
	{
		_shortcutRepository = shortcutRepository ?? App.GetRequiredService<ShortcutsRepository>();
		_shortcutCache
			.Connect()
			.DeferUntilLoaded()
			.Bind(out _shortcuts)
			.Subscribe();

		LoadShortcuts();
	}

	private void LoadShortcuts()
	{
		if (_shortcutRepository != null)
		{
			var loadedShortcuts = _shortcutRepository.GetShortcuts();
			_shortcutCache.Edit(cache =>
			{
				cache.Load(loadedShortcuts);
			});
		}
	}
}

public class ShortcutListDesignModel : ShortcutListViewModel
{
	public ShortcutListDesignModel() : base(null)
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