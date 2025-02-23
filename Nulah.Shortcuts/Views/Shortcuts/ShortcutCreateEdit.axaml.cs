using Avalonia.ReactiveUI;
using Nulah.Shortcuts.ViewModels.Shortcuts;

namespace Nulah.Shortcuts.Views.Shortcuts;

public partial class ShortcutCreateEdit : ReactiveUserControl<IShortcutCreateEditViewModel>
{
	public ShortcutCreateEdit()
	{
		InitializeComponent();
	}
}