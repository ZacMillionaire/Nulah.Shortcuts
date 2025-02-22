using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
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

	[Reactive]
	private byte[]? _shortcutImage;

	private readonly ImageProcessing? _imageProcesor;

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
			LoadShortcuts();
			d.Dispose();
		});
	}

	// ReSharper disable once UnusedMember.Global - Used by dependency injection
	public ShortcutListViewModel(IServiceProvider serviceProvider, ILogger<ShortcutListViewModel> logger) : this()
	{
		_logger = logger;
		_shortcutRepository = serviceProvider.GetRequiredService<ShortcutsRepository>();
		_imageProcesor = serviceProvider.GetRequiredService<ImageProcessing>();
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
			_shortcutCache.AddOrUpdate(_shortcutRepository.CreateShortcut(_title, _link, _shortcutImage));

			Reset();
		}
	}

	[ReactiveCommand]
	private async Task FilePicker()
	{
		// Start async operation to open the dialog.
		// Get top level from the current control. Alternatively, you can use Window reference instead.
		if (_imageProcesor is not null && TopLevel.GetTopLevel(App.GetMainWindow()) is { } topLevel)
		{
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Select image",
				AllowMultiple = false,
				FileTypeFilter =
				[
					FilePickerFileTypes.ImageAll
				],
			});

			if (files.Count == 1)
			{
				await using var stream = await files[0].OpenReadAsync();
				var resized = _imageProcesor.ResizeImage(stream);

				ShortcutImage = resized;
			}
		}
	}

	[ReactiveCommand]
	private void OpenShortcut(ShortcutDto shortcut)
	{
		App.GetMainWindow()?.Close();
	}

	private void Reset()
	{
		Title = Link = string.Empty;
		ShortcutImage = null;
	}
}