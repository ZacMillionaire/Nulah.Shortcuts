using Nulah.Shortcuts.Domain;

namespace Nulah.Shortcuts.Models.Interfaces;

public interface IShortcutListViewModel : IViewModelInterface
{
	void AddShortcut(ShortcutDto shortcut);
	void FilterShortcuts(bool deletedOnly = false);
}