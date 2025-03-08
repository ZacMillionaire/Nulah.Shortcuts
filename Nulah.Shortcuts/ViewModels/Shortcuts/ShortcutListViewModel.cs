using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Data.Criteria;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels.Shortcuts;

public partial class ShortcutListViewModel : ViewModelBase<IShortcutListViewModel>, IShortcutListViewModel
{
	private readonly ILogger<ShortcutListViewModel> _logger = NullLogger<ShortcutListViewModel>.Instance;
	private readonly ShortcutsRepository? _shortcutRepository;

	protected readonly SourceCache<ShortcutViewModel, int> _shortcutCache = new(x => x.Id);
	private readonly ReadOnlyObservableCollection<ShortcutViewModel> _shortcuts;

	public ReadOnlyObservableCollection<ShortcutViewModel> Shortcuts => _shortcuts;

	[Reactive]
	private bool _isEnabled;

	private string _searchString = string.Empty;

	protected ShortcutListViewModel()
	{
		// Future use
		//var shortcutTextFilter = this.WhenAnyValue(viewModel => viewModel._searchString)
		//	.Select(MakeFilter);

		_shortcutCache
			.Connect()
			// Future use
			//.Filter(shortcutTextFilter)
			.DeferUntilLoaded()
			.SortBy(x => x.Id)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out _shortcuts)
			.Subscribe();

		this.WhenActivated(async d =>
		{
			FilterShortcuts();
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

	public void AddShortcut(ShortcutDto shortcut)
	{
		_shortcutCache.AddOrUpdate(new ShortcutViewModel(shortcut));
	}

	public void FilterShortcuts(bool deletedOnly = false)
	{
		LoadShortcuts(new ShortcutCriteria()
		{
			DeletedOnly = deletedOnly
		});
	}

	private Func<ShortcutViewModel, bool> MakeFilter(string searchString)
	{
		// TODO: update this so it filters by search string
		return x => true;
		//return shortcutViewModel => shortcutViewModel.;
	}

	private void LoadShortcuts(ShortcutCriteria? criteria = null)
	{
		if (_shortcutRepository != null)
		{
			IsEnabled = false;
			_logger.LogInformation("Loading shortcuts");
			var loadedShortcuts = _shortcutRepository.GetShortcuts(criteria)
				.Select(x => new ShortcutViewModel(x));

			_shortcutCache.Edit(cache =>
			{
				_logger.LogInformation("adding shortcuts to cache");
				cache.Load(loadedShortcuts);
				IsEnabled = true;
			});
		}
	}

	[ReactiveCommand]
	private void OpenShortcut(ShortcutViewModel shortcut)
	{
		if (!string.IsNullOrWhiteSpace(shortcut.ShortcutLocation))
		{
			Process.Start("explorer", shortcut.ShortcutLocation);
			App.GetMainWindow()?.Close();
		}
	}

	[ReactiveCommand]
	private void DeleteShortcut(ShortcutViewModel shortcut)
	{
		shortcut.DeleteClicked = true;
	}

	[ReactiveCommand]
	private void ConfirmDeleteShortcut(ShortcutViewModel shortcut)
	{
		if (_shortcutRepository is not null)
		{
			_shortcutRepository.DeleteShortcut(shortcut.Id);
			_shortcutCache.Remove(shortcut);
		}
	}

	[ReactiveCommand]
	private void RestoreShortcut(ShortcutViewModel shortcut)
	{
		if (_shortcutRepository is not null)
		{
			_shortcutRepository.RestoreShortcut(shortcut.Id);
			FilterShortcuts(true);
		}
	}

	[ReactiveCommand]
	private void CancelDeleteShortcut(ShortcutViewModel shortcut)
	{
		shortcut.DeleteClicked = false;
	}
}