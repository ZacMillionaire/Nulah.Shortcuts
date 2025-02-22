using Avalonia;
using Avalonia.Controls.Primitives;

namespace Nulah.Shortcuts.Controls;

/// <summary>
/// Displays a Byte[] as an image if it is a valid image, otherwise displays nothing silently
/// </summary>
public class ByteArrayImage : TemplatedControl
{
	public static readonly StyledProperty<byte[]?> ByteArrayProperty
		= AvaloniaProperty.Register<ByteArrayImage, byte[]?>(nameof(ByteArray));

	public byte[]? ByteArray
	{
		get => GetValue(ByteArrayProperty);
		set => SetValue(ByteArrayProperty, value);
	}
}