using Nulah.Shortcuts.Data.Models;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Domain.Enums;
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
			return conn.Table<Shortcut>()
				.Select(ToDto)
				.ToList();
		});
	}

	public ShortcutDto CreateShortcut(string title, string link, byte[]? shortcutImageBlob)
	{
		return WithConnection(conn =>
		{
			var newShortcut = new Shortcut()
			{
				Title = title,
				Link = link,
				Type = ShortcutType.DefaultToProcessOpen,
				ImageBlob = shortcutImageBlob,
			};

			conn.Insert(newShortcut);

			return ToDto(newShortcut);
		});
	}

	private ShortcutDto ToDto(Shortcut shortcut)
	{
		return new ShortcutDto()
		{
			Title = shortcut.Title,
			ShortcutLocation = shortcut.Link,
			Id = shortcut.Id,
			Type = shortcut.Type,
			ImageBlob = shortcut.ImageBlob
		};
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