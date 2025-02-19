using Nulah.Shortcuts.Data.Models;
using Nulah.Shortcuts.Domain;
using SQLite;

namespace Nulah.Shortcuts.Data;

public class ShortcutsContext
{
	private readonly string _databaseLocation;

	public ShortcutsContext(string databaseLocation)
	{
		_databaseLocation = databaseLocation;
		WithConnection(conn =>
		{
			conn.CreateTable<Shortcut>();
		});
	}

	public List<ShortcutDto> GetShortcuts()
	{
		return WithConnection(conn =>
		{
			return conn.Table<Shortcut>().Select(x => new ShortcutDto()
				{
					Id = x.Id,
					Title = x.Title,
					Type = x.Type,
					ShortcutLocation = x.Link
				})
				.ToList();
		});
		return Enumerable.Range(1, 10)
			.Select(x => new ShortcutDto()
			{
				Id = x,
				Title = $"Shortcut not from database {x}",
				ShortcutLocation = $"{x}/whatever/a/b"
			})
			.ToList();
	}

	private T WithConnection<T>(Func<SQLiteConnection, T> func)
	{
		using var conn = new SQLiteConnection(_databaseLocation);
		return func(conn);
	}

	private void WithConnection(Action<SQLiteConnection> action)
	{
		using var conn = new SQLiteConnection(_databaseLocation);
		action(conn);
	}
}