using Nulah.Shortcuts.Core;
using ReactiveUI;

namespace Nulah.Shortcuts.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
	public ShortcutListViewModel ShortcutListViewModel { get; init; }

	public MainWindowViewModel()
	{
		ShortcutListViewModel = new ShortcutListViewModel(App.GetRequiredService<ShortcutsRepository>());
	}
}

public class MainWindowDesignModel : MainWindowViewModel
{
	public MainWindowDesignModel()
	{
		ShortcutListViewModel = new ShortcutListDesignModel();
	}
}