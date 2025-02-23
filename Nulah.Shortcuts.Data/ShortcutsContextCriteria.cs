using System.Linq.Expressions;
using Nulah.Shortcuts.Data.Criteria;
using Nulah.Shortcuts.Data.Models;

namespace Nulah.Shortcuts.Data;

public partial class ShortcutsContext
{
	private static Expression<Func<Shortcut, bool>> BuildShortcutQuery(ShortcutCriteria? shortcutQuery)
	{
		// Set criteria to a new instance if null is given
		shortcutQuery ??= new ShortcutCriteria();

		// Filter out any deleted as our default base
		Expression<Func<Shortcut, bool>> baseFunc = DefaultFilter;

		// These 2 parameters override the baseFunc and are not additive
		if (shortcutQuery.DeletedOnly)
		{
			baseFunc = x=> x.IsDeleted;
		}

		// Apply IncludedDeleted after DeletedOnly as it overrides
		if (shortcutQuery.IncludeDeleted)
		{
			baseFunc = AllFilter;
		}
		
		// if (!string.IsNullOrWhiteSpace(shortcutQuery.ShortcutName))
		// {
		// 	baseFunc = baseFunc.And(x => x.Name.Contains(shortcutQuery.Name));
		// }

		if (baseFunc.CanReduce)
		{
			baseFunc.Reduce();
		}

		return baseFunc;
	}

	/// <summary>
	/// Returns all non-deleted shortcuts
	/// </summary>
	private static Expression<Func<Shortcut, bool>> DefaultFilter => x => x.IsDeleted == false;
	/// <summary>
	/// Returns all shortcuts
	/// </summary>
	private static Expression<Func<Shortcut, bool>> AllFilter => x => true;
}