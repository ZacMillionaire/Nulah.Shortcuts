using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Nulah.Shortcuts.ViewModels;
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