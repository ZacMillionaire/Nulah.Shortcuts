using System.Linq;
using DynamicData;
using Nulah.Shortcuts.Domain;

namespace Nulah.Shortcuts.ViewModels.DesignModels;

public class ShortcutListDesignModel : ShortcutListViewModel
{
	public ShortcutListDesignModel()
	{
		_shortcutCache.AddOrUpdate(Enumerable.Range(1, 10)
			.Select(x => new ShortcutViewModel(new ShortcutDto()
			{
				Id = x,
				Title = $"Shortcut not from database {x}",
				ShortcutLocation = $"{x}/whatever/a/b"
			})));
	}
}