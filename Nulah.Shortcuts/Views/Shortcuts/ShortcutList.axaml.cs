using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Nulah.Shortcuts.ViewModels;

namespace Nulah.Shortcuts.Views.Shortcuts;

public partial class ShortcutList : ReactiveUserControl<ShortcutListViewModel>
{
	public ShortcutList()
	{
		InitializeComponent();
	}
}