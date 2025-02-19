using System;
using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels;

public class MainWindowViewModel : ViewModelBase<IMainWindowViewModel>, IMainWindowViewModel
{
	[Reactive]
	public IShortcutListViewModel ShortcutListViewModel { get; set; }

#pragma warning disable CS8618, CS9264
	public MainWindowViewModel()
	{
	}
#pragma warning restore CS8618, CS9264

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		ShortcutListViewModel = serviceProvider.GetRequiredService<IShortcutListViewModel>();
	}
}