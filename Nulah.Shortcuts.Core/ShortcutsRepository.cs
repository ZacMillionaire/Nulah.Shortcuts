using Nulah.Shortcuts.Data;
using Nulah.Shortcuts.Domain;

namespace Nulah.Shortcuts.Core;

public class ShortcutsRepository
{
	private readonly ShortcutsContext _context;

	public ShortcutsRepository(ShortcutsContext context)
	{
		_context = context;
	}

	public List<ShortcutDto> GetShortcuts()
	{
		return _context.GetShortcuts();
	}
}