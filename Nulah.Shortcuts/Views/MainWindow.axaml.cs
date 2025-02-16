using Avalonia.Controls;
using Avalonia.Input;

namespace Nulah.Shortcuts.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void FadeBorder_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.Source is Border { Name: "FadeBorder" })
		{
			Hide();
		}
	}
}