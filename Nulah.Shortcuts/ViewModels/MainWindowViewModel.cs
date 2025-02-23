using System;
using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Models.Interfaces;
using Nulah.Shortcuts.ViewModels.Shortcuts;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels;

public class MainWindowViewModel : ViewModelBase<IMainWindowViewModel>, IMainWindowViewModel
{
	[Reactive]
	public IViewModelInterface ContentViewModel { get; set; }

#pragma warning disable CS8618, CS9264
	public MainWindowViewModel()
	{
	}
#pragma warning restore CS8618, CS9264

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		ContentViewModel = serviceProvider.GetRequiredService<IShortcutManagerViewModel>();
	}
}