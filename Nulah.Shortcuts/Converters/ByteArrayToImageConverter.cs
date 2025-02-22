using System;
using System.Globalization;
using System.IO;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Nulah.Shortcuts.Converters;

public class ByteArrayToImageConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (targetType == typeof(IImage))
		{
			if (value is byte[] { Length: > 0 } imageBlob)
			{
				try
				{
					return new Bitmap(new MemoryStream(imageBlob));
				}
				catch (Exception ex)
				{
					// Should probably log sort of error here
					// Fallback if the content of imageBlob is not a valid image (eg, the image the imageBlob represents was
					// actually an SVG)
					return new BindingNotification(ex, BindingErrorType.Error);
				}
			}

			// Return a default icon if we have no valid value for design mode
			if (Design.IsDesignMode)
			{
				return new Bitmap(AssetLoader.Open(new Uri("avares://Nulah.Shortcuts/Assets/avalonia-logo.ico")));
			}

			return null;
		}

		return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}