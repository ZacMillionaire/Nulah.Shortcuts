using Avalonia.ReactiveUI;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI;

namespace Nulah.Shortcuts.Views.Shortcuts;

public partial class ShortcutList : ReactiveUserControl<IShortcutListViewModel>
{
	public ShortcutList()
	{
		InitializeComponent();
		this.WhenActivated(disposable =>
		{
			
		});
	}
}