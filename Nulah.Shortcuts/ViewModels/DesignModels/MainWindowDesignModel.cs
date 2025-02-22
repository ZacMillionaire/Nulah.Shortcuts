namespace Nulah.Shortcuts.ViewModels.DesignModels;

public class MainWindowDesignModel : MainWindowViewModel
{
	public MainWindowDesignModel()
	{
		ContentViewModel = new ShortcutManagerDesignModel();
	}
}