using System.Linq;
using DynamicData;
using Nulah.Shortcuts.Domain;

namespace Nulah.Shortcuts.ViewModels.DesignModels;

public class ShortcutListDesignModel : Shortcuts.ShortcutListViewModel
{
	public ShortcutListDesignModel()
	{
		_shortcutCache.AddOrUpdate(Enumerable.Range(1, 10)
			.Select(x => new Shortcuts.ShortcutViewModel(new ShortcutDto()
			{
				Id = x,
				Title = $"Shortcut not from database {x}",
				ShortcutLocation = $"{x}/whatever/a/b"
			})));
		_shortcutCache.AddOrUpdate(new Shortcuts.ShortcutViewModel(new ShortcutDto()
		{
			Id = 11,
			Title = $"Shortcut not from database 11f asdf 3 asdf 236w ayszerdyfgcvb",
			ShortcutLocation = $"11/whatever/a/b"
		}));
		_shortcutCache.AddOrUpdate(new Shortcuts.ShortcutViewModel(new ShortcutDto()
		{
			Id = 12,
			Title = $"Deleted shortcut",
			ShortcutLocation = $"11/whatever/a/b",
			IsDeleted = true
		}));
	}
}