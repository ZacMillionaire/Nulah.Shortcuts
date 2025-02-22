using System;
using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels;

public interface IShortcutManagerViewModel : IViewModelInterface
{
	IShortcutListViewModel ShortcutListViewModel { get; set; }
}

public class ShortcutManagerViewModel : ViewModelBase<IShortcutManagerViewModel>, IShortcutManagerViewModel
{
	[Reactive]
	public IShortcutListViewModel ShortcutListViewModel { get; set; }

#pragma warning disable CS8618, CS9264
	public ShortcutManagerViewModel()
	{
	}
#pragma warning disable

	public ShortcutManagerViewModel(IServiceProvider serviceProvider)
	{
		ShortcutListViewModel = serviceProvider.GetRequiredService<IShortcutListViewModel>();
	}
}