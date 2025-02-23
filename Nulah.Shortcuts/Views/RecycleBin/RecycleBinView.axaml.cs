using Avalonia.ReactiveUI;
using Nulah.Shortcuts.ViewModels.RecycleBin;

namespace Nulah.Shortcuts.Views.RecycleBin;

public partial class RecycleBinView : ReactiveUserControl<IRecycleBinViewModel>
{
	public RecycleBinView()
	{
		InitializeComponent();
	}
}