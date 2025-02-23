using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.Core;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels.Shortcuts;

public interface IShortcutCreateEditViewModel : IViewModelInterface
{
	Action<ShortcutDto>? ShortcutCreated { get; set; }
}

public partial class ShortcutCreateEditViewModel : ViewModelBase<IShortcutCreateEditViewModel>, IShortcutCreateEditViewModel
{
	public Action<ShortcutDto>? ShortcutCreated { get; set; }

	[Reactive]
	private string _title = string.Empty;

	[Reactive]
	private string _link = string.Empty;

	[Reactive]
	private byte[]? _shortcutImage;

	[Reactive]
	private bool _isEnabled;

	private IObservable<bool> _canCreateShortcut;
	private readonly ImageProcessing? _imageProcesor;
	private readonly ShortcutsRepository? _shortcutRepository;

#pragma warning disable CS8618, CS9264
	protected ShortcutCreateEditViewModel()
	{
	}
#pragma warning restore CS8618, CS9264

	// ReSharper disable once UnusedMember.Global - Used by dependency injection
	public ShortcutCreateEditViewModel(IServiceProvider serviceProvider, ILogger<ShortcutCreateEditViewModel> logger) : this()
	{
		_shortcutRepository = serviceProvider.GetRequiredService<ShortcutsRepository>();
		_imageProcesor = serviceProvider.GetRequiredService<ImageProcessing>();

		_canCreateShortcut = this.WhenAnyValue<ShortcutCreateEditViewModel, bool, string, string, ShortcutsRepository?>(
			x => x.Title,
			x => x.Link,
			x => x._shortcutRepository,
			(x, y, z) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y) && z is not null
		);

		IsEnabled = true;
	}


	[ReactiveCommand(CanExecute = nameof(_canCreateShortcut))]
	private async Task CreateShortcut()
	{
		if (_shortcutRepository is not null)
		{
			IsEnabled = false;
			await Task.Yield();
			var newShortcut = _shortcutRepository.CreateShortcut(_title, _link, _shortcutImage);

			ShortcutCreated?.Invoke(newShortcut);

			Reset();
			IsEnabled = true;
		}
	}

	[ReactiveCommand]
	private async Task FilePicker()
	{
		// Start async operation to open the dialog.
		// Get top level from the current control. Alternatively, you can use Window reference instead.
		if (_imageProcesor is not null && TopLevel.GetTopLevel(App.GetMainWindow()) is { } topLevel)
		{
			IsEnabled = false;
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
				var resized = _imageProcesor.ResizeImage(stream, 100);

				ShortcutImage = resized;
			}

			IsEnabled = true;
		}
	}

	private void Reset()
	{
		Title = Link = string.Empty;
		ShortcutImage = null;
	}
}