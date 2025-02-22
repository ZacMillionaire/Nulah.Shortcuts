namespace Nulah.Shortcuts.ViewModels.DesignModels;

public class ShortcutManagerDesignModel : ShortcutManagerViewModel
{
	public ShortcutManagerDesignModel()
	{
		ShortcutListViewModel = new ShortcutListDesignModel();
	}
}