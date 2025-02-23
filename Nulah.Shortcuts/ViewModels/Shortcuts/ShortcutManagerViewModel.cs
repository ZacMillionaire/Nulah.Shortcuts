using System;
using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Models.Interfaces;
using Nulah.Shortcuts.ViewModels.RecycleBin;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels.Shortcuts;

public interface IShortcutManagerViewModel : IViewModelInterface
{
	IShortcutListViewModel ShortcutListViewModel { get; }
	IViewModelInterface ShortcutActionViewModel { get; set; }
}

public partial class ShortcutManagerViewModel : ViewModelBase<IShortcutManagerViewModel>, IShortcutManagerViewModel
{
	private readonly IServiceProvider _serviceProvider;

	public IShortcutListViewModel ShortcutListViewModel { get; init; }

	[Reactive]
	private IViewModelInterface _shortcutActionViewModel;

#pragma warning disable CS8618, CS9264
	public ShortcutManagerViewModel()
	{
	}
#pragma warning disable

	// ReSharper disable once UnusedMember.Global
	public ShortcutManagerViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		ShortcutListViewModel = _serviceProvider.GetRequiredService<IShortcutListViewModel>();
	}

	[ReactiveCommand]
	private void ChangeAction(ShortcutManagerActionMode action)
	{
		if (action == ShortcutManagerActionMode.Hide)
		{
			ShortcutActionViewModel = null;
		}

		if (action == ShortcutManagerActionMode.CreateEdit && _shortcutActionViewModel is not IShortcutCreateEditViewModel)
		{
			ShortcutActionViewModel = _serviceProvider.GetRequiredService<IShortcutCreateEditViewModel>();
			((IShortcutCreateEditViewModel)ShortcutActionViewModel).ShortcutCreated = ShortcutListViewModel.AddShortcut;
			ShortcutListViewModel.FilterShortcuts();
		}

		if (action == ShortcutManagerActionMode.RecycleBin)
		{
			ShortcutActionViewModel = _serviceProvider.GetRequiredService<IRecycleBinViewModel>();
			ShortcutListViewModel.FilterShortcuts(true);
		}
	}
}

public enum ShortcutManagerActionMode
{
	Hide = -1,
	CreateEdit = 0,
	RecycleBin = 1,
}