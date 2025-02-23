using Nulah.Shortcuts.Data;
using Nulah.Shortcuts.Data.Criteria;
using Nulah.Shortcuts.Domain;

namespace Nulah.Shortcuts.Core;

public class ShortcutsRepository
{
	private readonly ShortcutsContext _context;

	public ShortcutsRepository(ShortcutsContext context)
	{
		_context = context;
	}

	public List<ShortcutDto> GetShortcuts(ShortcutCriteria? filter)
	{
		return _context.GetShortcuts(filter);
	}
	
	public ShortcutDto CreateShortcut(string title, string link, byte[]? shortcutImageBlob)
	{
		return _context.CreateShortcut(title, link, shortcutImageBlob);
	}

	public void DeleteShortcut(int shortcutId, bool softDelete = true)
	{
		_context.DeleteShortcut(shortcutId, softDelete);
	}

	public void RestoreShortcut(int shortcutId)
	{
		_context.RestoreShortcut(shortcutId);
	}
}