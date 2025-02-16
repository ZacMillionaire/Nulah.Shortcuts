using Avalonia.Controls;
using Avalonia.Input;

namespace Nulah.Shortcuts.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		this.Hide();
	}
}