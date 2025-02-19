namespace Nulah.Shortcuts.ViewModels;

public class MainWindowDesignModel : MainWindowViewModel
{
	public MainWindowDesignModel()
	{
		ShortcutListViewModel = new ShortcutListDesignModel();
	}
}