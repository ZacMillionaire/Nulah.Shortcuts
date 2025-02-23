namespace Nulah.Shortcuts.Data.Criteria;

public class ShortcutCriteria
{
	/// <summary>
	/// Includes all shortcuts, exclusive with <see cref="DeletedOnly"/> and takes precedence.
	/// </summary>
	public bool IncludeDeleted { get; set; }

	/// <summary>
	/// Include only deleted shortcuts. Filter ignored if <see cref="IncludeDeleted"/> is true.
	/// </summary>
	public bool DeletedOnly { get; set; }
}