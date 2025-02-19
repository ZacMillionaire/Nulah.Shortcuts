using System;
using System.Reactive.Disposables;
using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nulah.Shortcuts.ViewModels;

public class MainWindowViewModel : ViewModelBase<IMainWindowViewModel>, IMainWindowViewModel
{
	[Reactive]
	public IShortcutListViewModel ShortcutListViewModel { get; set; }

	public MainWindowViewModel()
	{
	}

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		ShortcutListViewModel = serviceProvider.GetRequiredService<IShortcutListViewModel>();
	}
}