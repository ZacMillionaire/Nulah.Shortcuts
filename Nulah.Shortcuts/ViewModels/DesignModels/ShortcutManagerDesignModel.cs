using Nulah.Shortcuts.ViewModels.Shortcuts;

namespace Nulah.Shortcuts.ViewModels.DesignModels;

public class ShortcutManagerDesignModel : ShortcutManagerViewModel
{
	public ShortcutManagerDesignModel()
	{
		ShortcutListViewModel = new ShortcutListDesignModel();
		ShortcutActionViewModel = new ShortcutCreateEditDesignModel();
	}
}