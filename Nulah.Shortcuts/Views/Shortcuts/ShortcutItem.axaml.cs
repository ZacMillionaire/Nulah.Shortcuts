using Avalonia;
using Avalonia.Controls.Primitives;

namespace Nulah.Shortcuts.Views.Shortcuts;

public class ShortcutItem : TemplatedControl
{
	public static readonly StyledProperty<string> TitleProperty =
		AvaloniaProperty.Register<ShortcutItem, string>(nameof(Title));

	public static readonly StyledProperty<string> ShortcutLinkProperty =
		AvaloniaProperty.Register<ShortcutItem, string>(nameof(ShortcutLink));

	public static readonly StyledProperty<object?> ShortcutContentProperty =
		AvaloniaProperty.Register<ShortcutItem, object?>(nameof(ShortcutContent));

	public static readonly StyledProperty<object?> ShortcutActionsProperty =
		AvaloniaProperty.Register<ShortcutItem, object?>(nameof(ShortcutActions));

	public string Title
	{
		get => GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public string ShortcutLink
	{
		get => GetValue(ShortcutLinkProperty);
		set => SetValue(ShortcutLinkProperty, value);
	}

	public object? ShortcutContent
	{
		get => GetValue(ShortcutContentProperty);
		set => SetValue(ShortcutContentProperty, value);
	}

	public object? ShortcutActions
	{
		get => GetValue(ShortcutActionsProperty);
		set => SetValue(ShortcutActionsProperty, value);
	}
}