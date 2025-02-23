using Nulah.Shortcuts.Data.Criteria;
using Nulah.Shortcuts.Data.Models;
using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Domain.Enums;
using SQLite;

namespace Nulah.Shortcuts.Data;

public partial class ShortcutsContext
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

	public List<ShortcutDto> GetShortcuts(ShortcutCriteria? filter)
	{
		return WithConnection(conn => conn.Table<Shortcut>()
			.Where(BuildShortcutQuery(filter))
			.Select(ToDto)
			.ToList());
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

	public void DeleteShortcut(int shortcutId, bool softDelete = true)
	{
		WithConnection(conn =>
		{
			var shortcutToDelete = conn.Table<Shortcut>()
				.FirstOrDefault(x => x.Id == shortcutId);

			// TODO: Hard delete not implemented yet so only soft delete is possible
			if (shortcutToDelete is not null)
			{
				shortcutToDelete.IsDeleted = true;
				conn.Update(shortcutToDelete);
			}
		});
	}

	public void RestoreShortcut(int shortcutId)
	{
		WithConnection(conn =>
		{
			var shortcutToDelete = conn.Table<Shortcut>()
				.FirstOrDefault(x => x.Id == shortcutId);

			if (shortcutToDelete is not null)
			{
				shortcutToDelete.IsDeleted = false;
				conn.Update(shortcutToDelete);
			}
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
			ImageBlob = shortcut.ImageBlob,
			IsDeleted = shortcut.IsDeleted,
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