using SQLite;

namespace Nulah.Shortcuts.Data.Models;

internal class BaseEntity
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
}